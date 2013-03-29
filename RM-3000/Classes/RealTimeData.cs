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

                if (bRecord)
                {
                    //テストデータとして記憶
                    RealMeasureData.SampleDatas.Add(realdata);
                }

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
                    bool bOffsetCalc = (SystemSetting.MeasureSetting.Mode == (int)ModeType.MODE1 && receiveCount != 0) &&
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
