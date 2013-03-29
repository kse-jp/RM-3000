using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using DataCommon;
using CommonLib;

namespace RM_3000
{
    public class DataGenerator
    {
        private int _MaxDataPerLoop;
        private int _MaxLoop;
        private double _StartVal;
        private List<double[]> _DataOutput;
        private List<SampleData> _SampleDataOutput;
        private int _MaxChannel;
        private object _Lock = new object();
        private double _CalcValue;
        private int _ShotCount;
        /// <summary>
        /// 測定設定
        /// </summary>
        private MeasureSetting measSetting = null;
        /// <summary>
        /// channelsSetting
        /// </summary>
        private ChannelsSetting chSetting = null;


        public int ShotCount
        {
            set
            {
                _ShotCount = value;
            }
        }

        public double StartValue
        {
            set
            {
                _StartVal = value;
            }
        }

        public List<double[]> DataOutput
        {
            get
            {
                return _DataOutput;
            }
        }

        public List<SampleData> SampleDataOutput
        {
            get
            {
                return _SampleDataOutput;
            }
        }

        public int MaxDataPerLoop
        {
            get
            {
                return _MaxDataPerLoop;
            }
            set
            {
                _MaxDataPerLoop = value;
            }
        }

        public double CalcValue
        {
            get
            {
                return _CalcValue;
            }
            set
            {
                _CalcValue = value;
            }
        }

        public int MaxLoop
        {
            get
            {
                return _MaxLoop;
            }
            set
            {
                _MaxLoop = value;
            }
        }

        public int MaxChannel
        {
            get
            {
                return _MaxChannel;
            }
            set
            {
                _MaxChannel = value;
            }
        }

        //public delegate void DataCreatedEventHandler(List<double[]> dataOut);
        public delegate void SampleDataCreatedEventHandler(List<SampleData> dataOut);


        public event SampleDataCreatedEventHandler OnSampleDataCreated = null;
        //public event DataCreatedEventHandler OnDataCreated = null;

        public DataGenerator(int maxDataPerLoop, int maxChannel, int maxLoop)
        {
            _MaxDataPerLoop = maxDataPerLoop;
            _MaxChannel = maxChannel;
            _MaxLoop = maxLoop;
        }

        public DataGenerator()
        {
            _MaxDataPerLoop = 0;
            _MaxChannel = 0;
            _MaxLoop = 0;


            // 測定中                
            this.measSetting = SystemSetting.MeasureSetting;
            this.chSetting = SystemSetting.ChannelsSetting;
        }



        public void CreateData()
        {

            for (int loop = 0; loop < _MaxLoop; loop++)
            {

                double starval = _StartVal + loop * _MaxDataPerLoop;
                lock (_Lock)
                {
                    _DataOutput = new List<double[]>();

                    for (int i = 0; i < _MaxDataPerLoop; i++)
                    {
                        double[] datas = new double[_MaxChannel + 1];

                        datas[0] =starval+ i;
                        for (int j = 1; j < _MaxChannel + 1; j++)
                        {

                            Random abcd = new Random(loop);

                            if (_ShotCount > 1)
                                datas[j] = (.00026 + (j * .00001 * CalcValue)) * System.Math.Pow((i % 3750) - 2000, 2) + (.002 * j) * (datas[0] % 3750) + 1200 + (abcd.Next(-100, 100) * 0.75);
                            else if (_ShotCount == 1)
                                datas[j] = (.00027 + (j * .00001 * CalcValue)) * System.Math.Pow((i % 3750) - 1850, 2) + (.002 * j) * (datas[0] % 3750) + 1200 + (j * abcd.Next(-25, 25) * 0.75);
                        }
                        _DataOutput.Add(datas);
                    }
                }
                _SampleDataOutput = ConvertToSampleDatas(_DataOutput, true);
                if (OnSampleDataCreated != null)
                    OnSampleDataCreated(_SampleDataOutput);
                //if (OnDataCreated != null)
                //    OnDataCreated(_DataOutput);

                System.Threading.Thread.Sleep(300);

            }
        }


        public void CreateDataMode1()
        {
            for (int loop = 0; loop < _MaxLoop; loop++)
            {
                lock (_Lock)
                {
                    double starval = _StartVal + loop * _MaxDataPerLoop;
                    Random rand1 = new Random(loop);
                    _DataOutput = new List<double[]>();
                    for (int i = 0; i < _MaxDataPerLoop; i++)
                    {
                        double[] datas = new double[_MaxChannel + 1];

                        datas[0] = starval + i;
                        for (int j = 1; j < _MaxChannel + 1; j++)
                        {

                            if (j == 1)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue, _CalcValue + 2);
                            }
                            else if (j == 2)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue + 2, _CalcValue + 2.5);
                            }
                            else if (j == 3)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue + 3, _CalcValue + 4);
                            }
                            else if (j == 4)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue + 3.75, _CalcValue + 4.55);
                            }
                            else if (j == 5)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 1.5, _CalcValue);
                            }
                            else if (j == 6)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 2.5, _CalcValue - 1.5);
                            }

                            else if (j == 7)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 2.5, _CalcValue - 3);
                            }
                            else if (j == 8)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 3, _CalcValue - 4);
                            }
                            else if (j == 9)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 4.8, _CalcValue - 4.2);
                            }
                            else if (j == 10)
                            {
                                datas[j] = NextDouble(rand1, _CalcValue - 4.22, _CalcValue - 5.75);
                            }


                        }
                        _DataOutput.Add(datas);
                    }
                }

                _SampleDataOutput = ConvertToSampleDatas(_DataOutput, false);
                if (OnSampleDataCreated != null)
                    OnSampleDataCreated(_SampleDataOutput);
                //if (OnDataCreated != null)
                //    OnDataCreated(_DataOutput);

                System.Threading.Thread.Sleep(300);
                //_StartVal++;
            }
        }


        double NextDouble(Random rand, double min, double max)
        {
            return min + (rand.NextDouble() * (max - min));
        }

        public void CreateDataMode3()
        {
            try
            {
                double maxvaly = 3000;
                double minvaly = 1200;
                double incrementx = 1;
                bool isdown = true;
                for (int loop = 0; loop < _MaxLoop; loop++)
                {
                    double starval = (_StartVal + (loop * _MaxDataPerLoop)) * incrementx;
                    lock (_Lock)
                    {

                        _DataOutput = new List<double[]>();
                        for (int i = 0; i < _MaxDataPerLoop; i++)
                        {
                            double[] datas = new double[_MaxChannel + 1];

                            datas[0] = starval + (i * incrementx);
                            if ((i + (loop * _MaxDataPerLoop)) % 80 == 0 && (i + (loop * _MaxDataPerLoop)) != 0)
                            {
                                if (isdown)
                                    isdown = false;
                                else
                                    isdown = true;
                            }

                            for (int j = 1; j < _MaxChannel + 1; j++)
                            {

                                //datas[j] = System.Math.Cos((double)(_CalcValue / 300) * (i + (loop * 1000))) * (j*50) + 1500 + (j * 20);                            
                                Random abcd = new Random(loop);

                                int calc = (i + (loop * _MaxDataPerLoop)) % 80;
                                if (isdown)
                                {
                                    if ((calc >= 75 && calc <= 80))
                                    {
                                        datas[j] = minvaly;
                                        //datas[j] += NextDouble(abcd, -10, 1.5);
                                    }
                                    else
                                    {
                                        datas[j] = (maxvaly - (((maxvaly - minvaly) / 75) * (calc))) - (j);
                                        //datas[j] += NextDouble(abcd, -10, 1.0);
                                    }
                                }
                                else
                                {
                                    if ((calc >= 30 && calc <= 80))
                                    {
                                        datas[j] = maxvaly;
                                        //datas[j] += NextDouble(abcd, 0, 25);
                                    }
                                    else
                                    {
                                        datas[j] = minvaly + ((maxvaly - minvaly) / 30) * (calc) + (j);
                                        //datas[j] += NextDouble(abcd, -10, 1.0);
                                    }
                                }
                            }
                            _DataOutput.Add(datas);
                        }
                    }

                    _SampleDataOutput = ConvertToSampleDatas(_DataOutput, false);

                    if (OnSampleDataCreated != null)
                        OnSampleDataCreated(_SampleDataOutput);

                    //if (OnDataCreated != null)
                    //    OnDataCreated(_DataOutput);

                    System.Threading.Thread.Sleep(300);

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        private List<SampleData> ConvertToSampleDatas(List<double[]> inpData, bool isMode2)
        {
            List<SampleData> listsample = new List<SampleData>();

            if (isMode2)
            {
                SampleData sample = new SampleData();
                ChannelData[] chdatas = new ChannelData[_MaxChannel];

                for (int i = 0; i < _MaxChannel; i++)
                {
                    ChannelData chdata = new ChannelData();
                    DataValue dataval = null;
                    chdata.Position = i;


                    Value_Mode2 valmode2 = new Value_Mode2();
                    Value_Standard valstd = new Value_Standard();
                    valmode2.Values = new Decimal[inpData.Count];
                    for (int j = 0; j < inpData.Count; j++)
                    {
                        decimal decval = 0;
                        if (Convert.ToDecimal(inpData[j][i]) > 9999.999M)
                        {
                            decval = 9999.999M;
                        }
                        else if (Convert.ToDecimal(inpData[j][i]) < -9999.999M)
                        {
                            decval = -9999.999M;
                        }
                        else
                        {
                            decval = Convert.ToDecimal(inpData[j][i]);
                        }

                        if (i == 0)
                            valstd.Value = decval;
                        else
                            valmode2.Values[j] = decval;
                    }

                    if (i == 0)
                        dataval = valstd;
                    else
                        dataval = valmode2;
                    
                    chdata.DataValues = dataval;
                    chdatas[i] = chdata;
                }

                sample.ChannelDatas = chdatas;
                listsample.Add(sample);
            }
            else
            {
                for (int j = 0; j < inpData.Count; j++)
                {
                    SampleData sample = new SampleData();
                    ChannelData[] chdatas = new ChannelData[_MaxChannel];

                    for (int i = 0; i < _MaxChannel; i++)
                    {
                        ChannelData chdata = new ChannelData();
                        DataValue dataval = null;
                        chdata.Position = i;

                        for (int k = 0; k < this.measSetting.MeasTagList.Length; k++)
                        {
                            decimal decval = Convert.ToDecimal(inpData[j][i]);
                            if (decval > 9999.999M)
                            {
                                decval = 9999.999M;
                            }
                            else if (decval < -9999.999M)
                            {
                                decval = -9999.999M;
                            }

                            if (this.measSetting.Mode == 1 && this.chSetting.ChannelSettingList[k].ChKind ==
                            ChannelKindType.R && this.chSetting.ChannelSettingList[k].Mode1_Trigger == Mode1TriggerType.MAIN)
                            {


                                Value_MaxMin valmaxmin = new Value_MaxMin();
                                valmaxmin.MinValue = decval;
                                valmaxmin.MaxValue = decval;
                                dataval = valmaxmin;
                            }
                            else
                            {
                                Value_Standard valstandard = new Value_Standard();
                                valstandard.Value = decval;
                                dataval = valstandard;
                            }
                        }

                        chdata.DataValues = dataval;
                        chdatas[i] = chdata;
                    }

                    sample.ChannelDatas = chdatas;
                    listsample.Add(sample);
                }
            }

            return listsample;
        }


    }
}
