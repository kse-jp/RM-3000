using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TestApplication
{
    public class DataGenerator
    {
        private int _MaxDataPerLoop;
        private int _MaxLoop;
        private double _StartVal;
        private List<double[]> _DataOutput;
        private int _MaxChannel;
        private object _Lock = new object();
        private double _CalcValue;
        private int _ShotCount;


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

        public delegate void DataCreatedEventHandler(List<double[]> dataOut);

        public event DataCreatedEventHandler OnDataCreated = null;

        public DataGenerator(int maxDataPerLoop, int maxChannel, int maxLoop)
        {
            _MaxDataPerLoop = maxDataPerLoop;
            _MaxChannel = maxChannel;
            _MaxLoop = maxLoop;
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

                        datas[0] = starval + i;
                        for (int j = 1; j < _MaxChannel + 1; j++)
                        {

                            Random abcd = new Random(loop);

                            if (_ShotCount > 1)
                                datas[j] = (.00026 + (j * .00001 * CalcValue)) * System.Math.Pow((datas[0] % 3750) - 2000, 2) + (.002 * j) * (datas[0] % 3750) + 1170 + (abcd.Next(-100, 100) * 0.75);
                            else if (_ShotCount == 1)
                                datas[j] = (.00026 + (j * .00001 * CalcValue)) * System.Math.Pow((datas[0] % 3750) - 2000, 2) + (.002 * j) * (datas[0] % 3750) + 1170 + (j * 0.75);

                        }
                        _DataOutput.Add(datas);
                    }
                }
                if (OnDataCreated != null)
                    OnDataCreated(_DataOutput);

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
                if (OnDataCreated != null)
                    OnDataCreated(_DataOutput);

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
                if (OnDataCreated != null)
                    OnDataCreated(_DataOutput);

                System.Threading.Thread.Sleep(300);
                //_StartVal += incrementx;
            }
        }

        public void CreateData3DMode()
        {            
            bool isincrease= true;
            double initdata= 2150;

            for (int loop = 0; loop < _MaxLoop; loop++)
            {
                double starval = (_StartVal + (loop * _MaxDataPerLoop));
                lock (_Lock)
                {
                    Random abcd = new Random(loop);
                    _DataOutput = new List<double[]>();
                    for (int i = 0; i < _MaxDataPerLoop; i++)
                    {
                        double[] datas = new double[_MaxChannel + 1];

                        datas[0] = starval + i;
                        if (i % 1000 == 0)
                        {
                            if (!isincrease)
                                isincrease = true;
                            else
                                isincrease = false;
                        }

                        if (isincrease)
                            initdata++;
                        else
                            initdata--;

                        for (int j = 1; j < _MaxChannel + 1; j++)
                        {
                            if (j == 2 || j == 7 || j == 8 || j == 9||j==10)
                            {
                                if (j == 8)
                                    datas[j] = initdata + NextDouble(abcd, -20, 20);
                                if (j == 7)
                                    datas[j] = initdata + NextDouble(abcd, -20, 20);
                                else
                                    datas[j] = initdata;
                            }
                            else
                            {
                                datas[j] = 1200 + NextDouble(abcd, -25 + j, 25 + j);
                            }
                      
                        }
                        _DataOutput.Add(datas);
                    }
                }
                if (OnDataCreated != null)
                    OnDataCreated(_DataOutput);

                System.Threading.Thread.Sleep(300);
                //_StartVal += incrementx;
            }
        }

    }
}
