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
    /// ���A���^�C������f�[�^�Ǘ��N���X
    /// </summary>
    public class RealTimeData
    {
        /// <summary>
        /// �L�^���t���O
        /// </summary>
        public static bool bRecord = false;

        /// <summary>
        /// ���[�h�P�����ɂ��ۑ��Ώۃt���O
        /// </summary>
        public static bool bMode1_Now_Record = false;

        /// <summary>
        /// Mode1�X�^�[�g����
        /// </summary>
        public static DateTime Cond_StartTime_Mode1;

        /// <summary>
        /// Mode1�������f�p�V���b�g�J�E���g
        /// </summary>
        public static int Cond_ShotCount_Mode1;

        /// <summary>
        /// Mode1�������f�p�����~����
        /// </summary>
        public static DateTime Cond_StopTime_Mode1;

        /// <summary>
        /// �����ɂ�鑪��ꎞ��~��ԃt���O
        /// </summary>
        public static bool bCond_MeasurePause
        {
            get { return (!bMode1_Now_Record && RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1); }
        }

        /// <summary>
        /// ���ώZ�o�p�ێ��̈�
        /// </summary>
        public static List<SampleData> TmpAverage_Samples = new List<SampleData>();


        private static MeasureData RealMeasureData = new MeasureData();

        private static List<SampleData> Samples = new List<SampleData>();

        private static object lockobj_samples = new object();

        /// <summary>
        /// �[���l�ۑ��p�^�O�ݒ�
        /// </summary>
        public static DataTagSetting DataTagSetting = (DataTagSetting)SystemSetting.DataTagSetting.Clone();

        /// <summary>
        /// �ۑ��p�t�H���_�p�X
        /// </summary>
        public static string FolderPath { get; set; }

        /// <summary>
        /// �f�[�^��M�J�E���g
        /// </summary>
        public static int receiveCount = 0;
        
        /// <summary>
        /// �V�X�e���ݒ�
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

                //���[�h1�̏ꍇ
                if (RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1)
                {
                    //���[�h�P�̏����ɓ����Ă��邩�H
                    bMode1_Now_Record = Judge_Mode1_Condition(realdata);

                    //���ϑ���̏ꍇ
                    if (SystemSetting.MeasureSetting.Mode1_MeasCondition.MeasConditionType == Mode1_MeasCondition.EnumMeasConditionType.MEAS_AVG_SHOTS)
                    {
                        // �ۑ��^�C�~���O = ���ω��Z������
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
                                        //����͂܂����������ς��Ă���
                                        if (shotindex == 0)
                                        {
                                            ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value /= TmpAverage_Samples.Count + 1;
                                        }

                                        ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value += ((Value_Standard)tmp.ChannelDatas[i].DataValues).Value / (TmpAverage_Samples.Count + 1);

                                        ////�S�đ������񂾂Ȃ�A���ω��Z
                                        //if (shotindex + 1 == TmpAverage_Samples.Count)
                                        //    ((Value_Standard)realdata.ChannelDatas[i].DataValues).Value /= TmpAverage_Samples.Count + 1;
                                    }
                                    else if (realdata.ChannelDatas[i].DataValues is Value_MaxMin)
                                    {
                                        //����͂܂����������ς��Ă���
                                        if (shotindex == 0)
                                        {
                                            ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue /= TmpAverage_Samples.Count + 1;
                                            ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue /= TmpAverage_Samples.Count + 1;
                                        }

                                        ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue += ((Value_MaxMin)tmp.ChannelDatas[i].DataValues).MaxValue / (TmpAverage_Samples.Count + 1);
                                        ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue += ((Value_MaxMin)tmp.ChannelDatas[i].DataValues).MinValue / (TmpAverage_Samples.Count + 1);

                                        ////�S�đ������񂾂Ȃ�A���ω��Z
                                        //if (shotindex + 1 == TmpAverage_Samples.Count)
                                        //{
                                        //    ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MaxValue /= TmpAverage_Samples.Count + 1;
                                        //    ((Value_MaxMin)realdata.ChannelDatas[i].DataValues).MinValue /= TmpAverage_Samples.Count + 1;
                                        //}
                                    }
                                }
                            }

                            //���ߍ��ݗp���N���A
                            TmpAverage_Samples.Clear();
                            GC.Collect();
                        }
                        // �ۑ��^�C�~���O�ł͂Ȃ�
                        else
                        {
                            //���Ϗ����p�ɗ��ߍ���
                            TmpAverage_Samples.Add(realdata);
                        }
                    }                
                }

                //�ۑ��w�����肩�AMode1�Ȃ�΃��[�h�P����OK���𔻕�
                if (bRecord && (bMode1_Now_Record || RM_3000.Sequences.TestSequence.GetInstance().Mode != Sequences.TestSequence.ModeType.Mode1))
                {
                    //�e�X�g�f�[�^�Ƃ��ċL��
                    RealMeasureData.SampleDatas.Add(realdata);
                }

                //���[�h1�ŕۑ��^�C�~���O�ł͂Ȃ����́A�`��ɂ����߂Ȃ��B
                if (bCond_MeasurePause)
                    return;

                SampleData realdata_sample = (SampleData)realdata.Clone();

                //���[�h�P����擾�Ȃ�΃I�t�Z�b�g�p���I�[����ݒ�
                if (SystemSetting.MeasureSetting.Mode == (int)ModeType.MODE1 && receiveCount == 0)
                {
                    //�`�����l�������[�v
                    for (int i = 1; i < realdata_sample.ChannelDatas.Length; i++)
                    {
                        if (realdata_sample.ChannelDatas[i] == null) continue;
                        if (realdata_sample.ChannelDatas[i].DataValues == null) continue;

                        //�`�����l����B��R�Ȃ��
                        if (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                            || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R)
                        {
                            //TagNo1�Ń`�F�b�N
                            if (SystemSetting.RelationSetting.RelationList[i].TagNo_1 != -1 &&
                                RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero == 0)
                            {
                                //�m�[�}���f�[�^
                                if (realdata_sample.ChannelDatas[i].DataValues is Value_Standard)
                                {
                                    RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero =
                                        ((Value_Standard)realdata_sample.ChannelDatas[i].DataValues).Value;
                                }
                                //MaxMin�f�[�^
                                else
                                {
                                    RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_1).StaticZero =
                                        (((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MinValue + ((Value_MaxMin)realdata_sample.ChannelDatas[i].DataValues).MaxValue) / 2;
                                }
                            }

                            //TagNo2 �Ń`�F�b�N
                            if (SystemSetting.RelationSetting.RelationList[i].TagNo_2 != -1 &&
                                RealTimeData.DataTagSetting.GetTag(SystemSetting.RelationSetting.RelationList[i].TagNo_2).StaticZero == 0)
                            {
                                //�m�[�}���Ȃ�Ύg��Ȃ�
                                if (realdata_sample.ChannelDatas[i].DataValues is Value_Standard)
                                {
                                }
                                //MaxMin�Ńf�[�^���g��
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
        /// ���[�h�P�̏����ɂ��A����͈͓��ł��邩�𔻒�
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

                    //���Ԋu�擾���̓V���b�g�����f�N�������g���A0�ȉ��ɂȂ�����ۑ��Ƃ���B
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

                    //���ώ擾���̓V���b�g�����C���N�������g���A�ۑ�ON���ɖ{�֐��̊O���ŉ��Z�B
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

                    //���莞
                    if (bMode1_Now_Record)
                    {

                        if (Cond_ShotCount_Mode1 >= cond.Inverval_time2shot_shots)
                        {
                            Cond_ShotCount_Mode1 = 0;
                            Cond_StopTime_Mode1 = realdata.SampleTime;
                            ret = false;
                        }
                        //����V���b�g���������񑪒�Ԋu����ɗ��Ă��܂��Ă���ꍇ
                        else if ((realdata_time - Cond_StartTime_Mode1).TotalMinutes >= cond.Inverval_time2shot_time)
                        {
                            //����V���b�g�����N���A������̂܂܂Ƃ���B
                            Cond_ShotCount_Mode1 = 0;
                            Cond_StartTime_Mode1 = realdata.SampleTime;
                            Cond_ShotCount_Mode1++;
                        }
                        else
                        {
                            Cond_ShotCount_Mode1++;
                        }

                    }
                    //�����莞
                    else
                    {
                        //���Ԍo�߂����Ă����
                        if ((realdata_time - Cond_StartTime_Mode1).TotalMinutes >= cond.Inverval_time2shot_time)
                        {
                            Cond_StartTime_Mode1 = realdata.SampleTime;

                            Cond_ShotCount_Mode1 = 1;

                            ret = true;
                        }
                    }
                    break;

                case Mode1_MeasCondition.EnumMeasConditionType.MEAS_INT_TIME2TIME:
                    //���莞
                    if (bMode1_Now_Record)
                    {

                        if (realdata_time.Hour != Cond_StartTime_Mode1.Hour && realdata_time.Hour == 0)
                            realdata_time.AddDays(1);

                        //���Ԍo�߂����Ă����
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
                    //�����莞
                    else
                    {
                        if (realdata_time.Hour != Cond_StopTime_Mode1.Hour && realdata_time.Hour == 0)
                            realdata_time.AddDays(1);

                        //���Ԍo�߂����Ă����
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

            #region ���t�O���Z
            foreach (SampleData sdata in ret)
            {
                //�`�����l�������[�v
                for (int i = 1; i < sdata.ChannelDatas.Length; i++)
                {
                    if (sdata.ChannelDatas[i] == null) continue;
                    if (sdata.ChannelDatas[i].DataValues == null) continue;

                    #region ���[�h�P���̃I�t�Z�b�g�Ή�
                    //���[�h�P �� �`�����l����B��R�Ȃ��
                    //bool bOffsetCalc = (SystemSetting.MeasureSetting.Mode == (int)ModeType.MODE1 && receiveCount != 0) &&
                    //                    (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                    //                        || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R);

                    bool bOffsetCalc = (RM_3000.Sequences.TestSequence.GetInstance().Mode == Sequences.TestSequence.ModeType.Mode1 &&receiveCount != 0) &&
                                        (SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.B
                                        || SystemSetting.ChannelsSetting.ChannelSettingList[i - 1].ChKind == ChannelKindType.R);
                    #endregion

                    //TagNo1�Ń`�F�b�N
                    if (SystemSetting.RelationSetting.RelationList[i].TagNo_1 != -1)
                    {
                        int tmpTagNo = SystemSetting.RelationSetting.RelationList[i].TagNo_1;

                        //�m�[�}���f�[�^
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard)
                        {
                            // �I�t�Z�b�g�v�Z
                            if (bOffsetCalc)
                                ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // ���؂�v�Z
                            ((Value_Standard)sdata.ChannelDatas[i].DataValues).Value =
                                (decimal)CalcOperator.ToRoundDown((double)((Value_Standard)sdata.ChannelDatas[i].DataValues).Value, SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //MaxMin�f�[�^
                        else if( sdata.ChannelDatas[i].DataValues is Value_MaxMin)
                        {
                            // �I�t�Z�b�g�v�Z
                            if (bOffsetCalc)
                            {
                                //������5um�ȉ��Ȃ��MinValue��MaxValue�ɂ����B
                                if (((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue - ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue <= 5)
                                    ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue = ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue;

                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            }
                            // ���؂�v�Z
                            ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue =
                                (decimal)CalcOperator.ToRoundDown((double)((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MaxValue, SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);

                        }
                        //Mode2�f�[�^
                        else if (sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                            for (int sampleindex = 0; sampleindex < ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values.Length; sampleindex++ )
                            {
                                ((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex] =
                                    (decimal)CalcOperator.ToRoundDown((double)((Value_Mode2)sdata.ChannelDatas[i].DataValues).Values[sampleindex], SystemSetting.DataTagSetting.GetTag(tmpTagNo).Point);
                            }
                        }
                    }

                    //TagNo2 �Ń`�F�b�N
                    if (SystemSetting.RelationSetting.RelationList[i].TagNo_2 != -1)
                    {
                        int tmpTagNo = SystemSetting.RelationSetting.RelationList[i].TagNo_2;

                        //�m�[�}���Ȃ�Ύg��Ȃ�
                        if (sdata.ChannelDatas[i].DataValues is Value_Standard || sdata.ChannelDatas[i].DataValues is Value_Mode2)
                        {
                        }
                        //MaxMin�Ńf�[�^���g��
                        else
                        {
                            // �I�t�Z�b�g�v�Z
                            if (bOffsetCalc)
                                ((Value_MaxMin)sdata.ChannelDatas[i].DataValues).MinValue -= RealTimeData.DataTagSetting.GetTag(tmpTagNo).StaticZero;

                            // ���؂�v�Z
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
        ///// ����̃����W�f�[�^���擾���܂��B
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
        /// RealTimeData�̏�����
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

                //�^�O�ݒ��Clone
                //RealTimeData.DataTagSetting = SystemSetting.DataTagSetting.Clone();
                
            }

            Samples.Clear();
            TmpAverage_Samples.Clear();

            //���[�h�P�����̏�����
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
        /// RealTimeData�́Y����
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
                        dlg = Forms.Common.dlgProgress.ShowProgress("����f�[�^��M�����҂�"
                            , "����f�[�^���������Ă��܂��B���΂炭���҂����������B"
                            , string.Empty
                            , System.Windows.Forms.ProgressBarStyle.Marquee
                            , null);

                    dlg.CancelVisibled = false;

                    System.Threading.Thread.Sleep(10);
                    dlg.Update();
                    System.Windows.Forms.Application.DoEvents();
                }

                //�ۑ������̏I��
                RealMeasureData.EndMeasure();

                if (dlg != null)
                    dlg.Close();

                //�f�[�^��1�ł���M���Ă���΃f�[�^�o��
                if (receiveCount != 0)
                {
                    RealMeasureData.FilePath = FolderPath + @"\" + MeasureData.FileName;
                    RealMeasureData.Serialize();
                }
            }
        }

        /// <summary>
        /// RealTimeData�̎����ۑ��ĊJ
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
        /// ���ʂ̃N���A
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
