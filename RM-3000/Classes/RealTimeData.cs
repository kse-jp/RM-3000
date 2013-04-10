using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DataCommon;
using CommonLib;


namespace RM_3000
{
    /// <summary>
    /// リアルタイム測定データ管理クラス
    /// </summary>
    public class RealTimeData
    {
        /// <summary>
        /// 記録中フラグ
        /// </summary>
        public static bool bRecord = false;

        /// <summary>
        /// モード１条件による保存対象フラグ
        /// </summary>
        public static bool bMode1_Now_Record = false;

        /// <summary>
        /// Mode1スタート時間
        /// </summary>
        public static DateTime Cond_StartTime_Mode1;

        /// <summary>
        /// Mode1条件判断用ショットカウント
        /// </summary>
        public static int Cond_ShotCount_Mode1;

        /// <summary>
        /// Mode1条件判断用測定停止時間
        /// </summary>
        public static DateTime Cond_StopTime_Mode1;

        /// <summary>
        /// 条件による測定一時停止状態フラグ
        /// </summary>
        public static bool bCond_MeasurePause
        {
            get { return (!bMode1_Now_Record && RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1); }
        }

        /// <summary>
        /// 平均算出用保持領域
        /// </summary>
        public static List<SampleData> TmpAverage_Samples = new List<SampleData>();


        private static MeasureData RealMeasureData = new MeasureData();

        private static List<SampleData> Samples = new List<SampleData>();

        private static object lockobj_samples = new object();

        /// <summary>
        /// ゼロ値保存用タグ設定
        /// </summary>
        public static DataTagSetting DataTagSetting = (DataTagSetting)SystemSetting.DataTagSetting.Clone();

        /// <summary>
        /// 保存用フォルダパス
        /// </summary>
        public static string FolderPath { get; set; }

        /// <summary>
        /// データ受信カウント
        /// </summary>
        public static int receiveCount = 0;
        
        /// <summary>
        /// システム設定
        /// </summary>
        private static SystemConfig systemSetting = new SystemConfig();

        private static DataGenerator dataGenerator = null;
        private static Thread dataThread = null;

        /// <summary>
        /// Constructor
        /// </summary>
        static RealTimeData()
        {
            systemSetting.LoadXmlFile();

            if (systemSetting.IsSimulationMode)
            {
                if (dataGenerator == null)
                {
                    dataGenerator = new DataGenerator();
                    dataGenerator.OnSampleDataCreated += SimulationDataGenerated;
                }
            }

        }

        #region public method
        /// <summary>
        /// AddRealData
        /// </summary>
        /// <param name="realdata">NowRegistData</param>
        public static void AddRealData(SampleData realdata)
        {
            lock (lockobj_samples)
            {

                //モード1の場合
                if (RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1)
                {
                    //モード１の条件に入っているか？
                    bMode1_Now_Record = Judge_Mode1_Condition(realdata);

                    //平均測定の場合
                    if (SystemSetting.MeasureSetting.Mode1_MeasCondition.MeasConditionType == Mode1_MeasCondition.EnumMeasConditionType.MEAS_AVG_SHOTS)
                    {
                        // 保存タイミング = 平均演算をする
                        if (bMode1_Now_Record)
                        {                           
                            for (int shotindex = 0 ; shotindex < TmpAverage_Samples.Count ; shotindex++)
                            {
                                SampleData tmp = TmpAverage_Samples[shotindex];

                                for (int i = 0; i < tmp.ChannelDatas.Length; i++)
                                {
                                    if (realdata.ChannelDatas[i] == null) continue;

                                    if (realdata.ChannelDatas[i].DataValues is Value_Standard)
                                    {
                                        //初回はまず自分も平均しておく
                                        if (shotindex == 0)
                                        {
                                            ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value /= TmpAverage_Samples.Count + 1;
                                        }

                                        ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value += ((Value_Standard)tmp.ChannelDatas[i].DataValues).Value / (TmpAverage_Samples.Count + 1);

                                        ////全て足しこんだなら、平均演算
                                        //if (shotindex + 1 == TmpAverage_Samples.Count)
                                        //    ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value /= TmpAverage_Samples.Count + 1;
                                    }
                                    else if (realdata.ChannelDatas[i].DataValues is Value_MaxMin)
                                    {
                                        //初回はまず自分も平均しておく
                                        if (shotindex == 0)
                                        {
                                            ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue /= TmpAverage_Samples.Count + 1;
                                            ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue /= TmpAverage_Samples.Count + 1;
                                        }

                                        ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue += ((Value_MaxMin)tmp.ChannelDatas[i].DataValues).MaxValue / (TmpAverage_Samples.Count + 1);
                                        ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue += ((Value_MaxMin)tmp.ChannelDatas[i].DataValues).MinValue / (TmpAverage_Samples.Count + 1);

                                        ////全て足しこんだなら、平均演算
                                        //if (shotindex + 1 == TmpAverage_Samples.Count)
                                        //{
                                        //    ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue /= TmpAverage_Samples.Count + 1;
                                        //    ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue /= TmpAverage_Samples.Count + 1;
                                        //}
                                    }
                                }
                            }

                            //溜め込み用をクリア
                            TmpAverage_Samples.Clear();
                            GC.Collect();
                        }
                        // 保存タイミングではない
                        else
                        {
                            //平均処理用に溜め込み
                            TmpAverage_Samples.Add(realdata);
                        }
                    }                
                }

                //保存指示ありかつ、Mode1ならばモード１判定OKかを判別
                if (bRecord && (bMode1_Now_Record || RM_3000.Sequences.TestSequence.GetInstance().Mode != Sequences.TestSequence.ModeType.Mode1))
                {
                    //テストデータとして記憶
                    RealMeasureData.SampleDatas.Add(realdata);
                }

                //モード1で保存タイミングではない時は、描画にもためない。
                if (bCond_MeasurePause)
                    return;

                SampleData realdata_sample = (SampleData)realdata.Clone();

                //モード１初回取得ならばオフセット用動的ゼロを設定
                if (SystemSetting.MeasureSetting.Mode == (int)ModeType.MODE1 && receiveCount == 0)
                {
                    //チャンネル分ループ
                    for (int i = 1; i < realdata_sample.ChannelDatas.Length; i++)
                    {
                        if (realdata_sample.ChannelDatas[i] == null) continue;
                        if (realdata_sample.ChannelDatas[i].DataValues == null) continue;

                        //チャンネルがBかRならば
                        if (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                            || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R)
                        {
                            //TagNo1でチェック
                            if (SystemSetting.RelationSetting.RelationList[i].TagNo_1 != -1 &&
                                RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero == 0)
                            {
                                //ノーマルデータ
                                if (realdata_sample.ChannelDatas[i].DataValues is Value_Standard)
                                {
                                    RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero =
                                        ((Value_Standard)realdata_sample.ChannelDatas[i].DataValues).Value;
                                }
                                //MaxMinデータ
                                else
                                {
                                    RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero =
                                        (((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MinValue + ((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MaxValue) / 2;
                                }
                            }

                            //TagNo2 でチェック
                            if (SystemSetting.RelationSetting.RelationList[i].TagNo_2 != -1 &&
                                RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_2).StaticZero == 0)
                            {
                                //ノーマルならば使わない
                                if (realdata_sample.ChannelDatas[i].DataValues is Value_Standard)
                                {
                                }
                                //MaxMinでデータを使う
                                else
                                {
                                    RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_2).StaticZero =
                                        (((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MinValue + ((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MaxValue) / 2;
                                }
                            }
                        }
                    }
                }


                Samples.Add(realdata_sample);
                receiveCount++;

            }        
        }

        /// <summary>
        /// モード１の条件により、測定範囲内であるかを判定
        /// </summary>
        /// <param name="realdata"></param>
        /// <returns></returns>
        private static bool Judge_Mode1_Condition(SampleData realdata)
        {
            bool ret = bMode1_Now_Record;

            DateTime realdata_time = realdata.SampleTime;

            Mode1_MeasCondition cond = SystemSetting.MeasureSetting.Mode1_MeasCondition;

            switch (cond.MeasConditionType)
            {
                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_ALL_SHOTS:
                    ret = true;
                    break;

                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_SHOTS:

                    //一定間隔取得時はショット数をデクリメントし、0以下になったら保存とする。
                    if (Cond_ShotCount_Mode1 <= 0)
                    {
                        Cond_ShotCount_Mode1 = cond.Interval_count;
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }

                    Cond_ShotCount_Mode1--;
                    
                    break;

                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_AVG_SHOTS:

                    //平均取得時はショット数をインクリメントし、保存ON時に本関数の外側で演算。
                    Cond_ShotCount_Mode1++;

                    if (Cond_ShotCount_Mode1 >= cond.Average_count)
                    {
                        ret = true;
                        Cond_ShotCount_Mode1 = 0;
                    }
                    else
                    {
                        ret = false;
                    }

                    break;

                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_TIME2SHOTS:

                    if (realdata_time.Hour != Cond_StartTime_Mode1.Hour && realdata_time.Hour == 0)
                        realdata_time.AddDays(1);

                    //測定時
                    if (bMode1_Now_Record)
                    {

                        if (Cond_ShotCount_Mode1 >= cond.Inverval_time2shot_shots)
                        {
                            Cond_ShotCount_Mode1 = 0;
                            Cond_StopTime_Mode1 = realdata.SampleTime;
                            ret = false;
                        }
                        //測定ショット数よりも次回測定間隔が先に来てしまっている場合
                        else if ((realdata_time - Cond_StartTime_Mode1).TotalMinutes >= cond.Inverval_time2shot_time)
                        {
                            //測定ショット数をクリアし測定のままとする。
                            Cond_ShotCount_Mode1 = 0;
                            Cond_StartTime_Mode1 = realdata.SampleTime;
                            Cond_ShotCount_Mode1++;
                        }
                        else
                        {
                            Cond_ShotCount_Mode1++;
                        }

                    }
                    //未測定時
                    else
                    {
                        //時間経過をしていれば
                        if ((realdata_time - Cond_StartTime_Mode1).TotalMinutes >= cond.Inverval_time2shot_time)
                        {
                            Cond_StartTime_Mode1 = realdata.SampleTime;

                            Cond_ShotCount_Mode1 = 1;

                            ret = true;
                        }
                    }
                    break;

                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_TIME2TIME:
                    //測定時
                    if (bMode1_Now_Record)
                    {

                        if (realdata_time.Hour != Cond_StartTime_Mode1.Hour && realdata_time.Hour == 0)
                            realdata_time.AddDays(1);

                        //時間経過をしていれば
                        if ((realdata_time - Cond_StartTime_Mode1).TotalMinutes >= cond.Inverval_time2time_meastime)
                        {
                            Cond_ShotCount_Mode1 = 0;
                            Cond_StopTime_Mode1 = realdata.SampleTime;

                            ret = false;
                        }
                        else
                        {
                            Cond_ShotCount_Mode1++;
                        }

                    }
                    //未測定時
                    else
                    {
                        if (realdata_time.Hour != Cond_StopTime_Mode1.Hour && realdata_time.Hour == 0)
                            realdata_time.AddDays(1);

                        //時間経過をしていれば
                        if ((realdata_time - Cond_StopTime_Mode1).TotalMinutes >= cond.Inverval_time2time_stoptime)
                        {
                            Cond_StartTime_Mode1 = realdata.SampleTime;
                            Cond_ShotCount_Mode1++;
                            ret = true;
                        }
                    }

                    break;
            }

            return ret;
        }

        /// <summary>
        /// GetRealTimeDatas
        /// </summary>
        /// <returns></returns>
        public static List<SampleData> GetRealTimeDatas()
        {
            List<SampleData> ret = null;

            //if (systemSetting.IsSimulationMode)
            //{

            //    lock (lockobj_samples)
            //    {
            //        ret = new List<SampleData>(Samples);
            //        Samples.Clear();
            //    }
            //}
            //else
            //{

            lock (lockobj_samples)
            {
                ret = new List<SampleData>(Samples);

                Samples.Clear();

                GC.Collect();
            }

            #region 送付前演算
            foreach (SampleData sdata in ret)
            {
                //チャンネル分ループ
                for (int i = 1; i < sdata.ChannelDatas.Length; i++)
                {
                    if (sdata.ChannelDatas[i] == null) continue;
                    if (sdata.ChannelDatas[i].DataValues == null) continue;

                    #region モード１時のオフセット対応
                    //モード１ で チャンネルがBかRならば
                    //bool bOffsetCalc = (SystemSetting.MeasureSetting.Mode == (int)ModeType.MODE1 && receiveCount != 0) &&
                    //                    (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                    //                        || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R);

                    bool bOffsetCalc = (RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1 &&receiveCount != 0) &&
                                        (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                                        || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R);
                    #endregion

                    //TagNo1でチェック
                    if (SystemSetting.RelationSetting.RelationList[i].TagNo_1 != -1)
                    {
                        int tmpTagNo = SystemSetting.RelationSetting.RelationList[i].TagNo_1;

                        //ノーマルデータ
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard)
                        {
                            // オフセット計算
                            if (bOffsetCalc)
                                ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // 桁切り計算
                            ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value =
                                (decimal)CalcOperator.ToRoundDown((double)((Value_Standard)sdata.ChannelDatas[i].DataValues).Value, SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //MaxMinデータ
                        else if( sdata.ChannelDatas[i].DataValues is Value_MaxMin)
                        {
                            // オフセット計算
                            if (bOffsetCalc)
                            {
                                //差分が5um以下ならばMinValueをMaxValueにいれる。
                                if (((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue - ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue <= 5)
                                    ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue = ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue;

                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            }
                            // 桁切り計算
                            ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue =
                                (decimal)CalcOperator.ToRoundDown((double)((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue, SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //Mode2データ
                        else if (sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                            for (int sampleindex = 0; sampleindex < ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values.Length; sampleindex++ )
                            {
                                ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex] =
                                    (decimal)CalcOperator.ToRoundDown((double)((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex], SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);
                            }
                        }
                    }

                    //TagNo2 でチェック
                    if (SystemSetting.RelationSetting.RelationList[i].TagNo_2 != -1)
                    {
                        int tmpTagNo = SystemSetting.RelationSetting.RelationList[i].TagNo_2;

                        //ノーマルならば使わない
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard || sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                        }
                        //MaxMinでデータを使う
                        else
                        {
                            // オフセット計算
                            if (bOffsetCalc)
                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // 桁切り計算
                            ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue =
                                (decimal)CalcOperator.ToRoundDown((double)((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue, SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);
                        }
                    }
                }
            }
            #endregion

            //    Samples.Clear();

            //    GC.Collect();
            //}
            //}

            return ret;
        }

        /// <summary>
        /// GetLastData
        /// </summary>
        /// <param name="ClearFlag">ClearFlag</param>
        /// <returns></returns>
        public static SampleData GetLastData(bool ClearFlag)
        {
            if (Samples.Count == 0) return null;

            SampleData ret = Samples.Last();

            if (ClearFlag)
            {
                Samples.Clear();
                GC.Collect();
            }

            return ret;
        }
        ///// <summary>
        ///// 特定のレンジデータを取得します。
        ///// </summary>
        ///// <param name="StartIndex"></param>
        ///// <param name="Count"></param>
        ///// <returns></returns>
        //public static List<SampleData> GetRangeDatas(int StartIndex, int Count)
        //{
        //    List<SampleData> ret = null;
        //    return ret;
        //}

        /// <summary>
        /// RealTimeDataの初期化
        /// </summary>
        /// <returns></returns>
        public static bool InitData(ChannelsSetting ch_setting, MeasureSetting meas_setting, string folderPath)
        {
            if (bRecord)
            {
                RealMeasureData.EndTime = DateTime.MaxValue;
                RealMeasureData.StartTime = DateTime.Now;
                RealMeasureData.InitializeforMeasure(ch_setting, meas_setting, folderPath);
                FolderPath = folderPath;

                //タグ設定のClone
                //RealTimeData.DataTagSetting = SystemSetting.DataTagSetting.Clone();
                
            }

            Samples.Clear();
            TmpAverage_Samples.Clear();

            //モード１条件の初期化
            Cond_ShotCount_Mode1 = 0;
            Cond_StartTime_Mode1 = RealMeasureData.StartTime;
            Cond_StopTime_Mode1 = RealMeasureData.StartTime;

            receiveCount = 0;

            return true;
        }

        /// <summary>
        /// GetStartTime
        /// </summary>
        /// <returns></returns>
        public static DateTime GetStartTime()
        {
            if (RealMeasureData != null)
                return RealMeasureData.StartTime;
            else
                return DateTime.Now;
        }

        /// <summary>
        /// SetStartTime
        /// </summary>
        /// <returns></returns>
        public static void SetStartTime(DateTime value)
        {
            if (RealMeasureData != null)
                RealMeasureData.StartTime = value;
        }

        /// <summary>
        /// RealTimeDataの〆処理
        /// </summary>
        /// <returns></returns>
        public static void EndData()
        {
            if (bRecord)
            {
                RealMeasureData.EndTime = DateTime.Now;

                Forms.Common.dlgProgress dlg = null;

                while (Sequences.TestSequence.GetInstance().ReserveRestCount != 0)
                {
                    if (dlg == null)
                        dlg = Forms.Common.dlgProgress.ShowProgress("測定データ受信処理待ち"
                            , "測定データを処理しています。しばらくお待ちください。"
                            , string.Empty
                            , System.Windows.Forms.ProgressBarStyle.Marquee
                            , null);

                    dlg.CancelVisibled = false;

                    System.Threading.Thread.Sleep(10);
                    dlg.Update();
                    System.Windows.Forms.Application.DoEvents();
                }

                //保存処理の終了
                RealMeasureData.EndMeasure();

                if (dlg != null)
                    dlg.Close();

                //データが1つでも受信していればデータ出力
                if (receiveCount != 0)
                {
                    RealMeasureData.FilePath = FolderPath + @"\" + MeasureData.FileName;
                    RealMeasureData.Serialize();
                }
            }
        }

        /// <summary>
        /// RealTimeDataの自動保存再開
        /// </summary>
        /// <returns></returns>
        public static void ResumeData()
        {
            if (bRecord)
            {
                RealMeasureData.ResumeMeasure();
            }
        }

        /// <summary>
        /// 結果のクリア
        /// </summary>
        public static void ClearData()
        {
            if(RealMeasureData != null)
                RealMeasureData.ClearMeasure();

            if(Samples != null)
                Samples.Clear();

            if (TmpAverage_Samples != null)
                TmpAverage_Samples.Clear();

            GC.Collect();
        }

        #endregion

        #region private method

        #endregion

        #region Simulation Methods
#if true
        public static void StartSimulatorMode(int mode)
        {
            if (mode == 1)
                StartGraph1();
            else if (mode == 2)
                StartGraph2();
            else if (mode == 3)
                StartGraph3();
        }

        public static void StopSimulator()
        {
            if (dataThread != null)
            {
                dataThread.Abort();
            }
        }

        /// <summary>
        /// simulator mode1
        /// </summary>
        private static void StartGraph1()
        {
            if (dataThread != null)
            {
                if (dataThread.IsAlive)
                {
                    dataThread.Abort();
                }
            }

            dataThread = new System.Threading.Thread(dataGenerator.CreateDataMode1);
            dataThread.IsBackground = true;
            dataGenerator.MaxChannel = 11;
            dataGenerator.CalcValue = -3;
            dataGenerator.MaxDataPerLoop = 20;
            dataGenerator.MaxLoop = 1000;
            dataGenerator.StartValue = 0;

            dataThread.Start();

        }

        private static void StartGraph2()
        {
            if (dataThread != null)
            {
                if (dataThread.IsAlive)
                {
                    dataThread.Abort();

                }
            }
            dataThread = new System.Threading.Thread(dataGenerator.CreateData);
            dataThread.IsBackground = true;
            dataGenerator.MaxChannel = 11;
            dataGenerator.CalcValue = 0.75;
            dataGenerator.MaxDataPerLoop = 3750;
            dataGenerator.MaxLoop = 1000;
            dataGenerator.StartValue = 100;
            dataGenerator.ShotCount = 1;

            dataThread.Start();

        }

        private static void StartGraph3()
        {
            if (dataThread != null)
            {
                if (dataThread.IsAlive)
                {
                    dataThread.Abort();

                }
            }
            dataThread = new System.Threading.Thread(dataGenerator.CreateDataMode3);
            dataThread.IsBackground = true;
            dataGenerator.MaxChannel = 11;
            dataGenerator.CalcValue = 9.2;
            dataGenerator.MaxDataPerLoop = 1000;
            dataGenerator.MaxLoop = 1000;
            dataGenerator.StartValue = 0;

            dataThread.Start();

        }

        private static void SimulationDataGenerated(List<SampleData> dataOut)
        {
            foreach (SampleData sd in dataOut)
            {
                AddRealData(sd);
            }

            //Samples = new List<SampleData>(dataOut);
        }

#endif
        #endregion


    }
}
