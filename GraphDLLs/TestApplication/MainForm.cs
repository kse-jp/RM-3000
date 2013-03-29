using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace TestApplication
{
    public partial class MainForm : Form
    {
        private GraphForm[] _Forms;
        private System.Threading.Thread _DataThread;
        private DataGenerator datagen = null;
        private object _Lock = new object();
        public delegate void SetValue(List<double[]> inpData);
        private int _MaxGraph = 10;
        private Graph3DForm _FormGraph3D;
        private int _Mode = 0;

        public MainForm()
        {
            InitializeComponent();
            this.datagen = new DataGenerator(3750, 10, 1000);

            this.datagen.OnDataCreated += this.DataCreated;
            if (System.Configuration.ConfigurationManager.AppSettings["MaximumGraph"] != null)
                Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaximumGraph"].ToString(), out _MaxGraph);

        }

        private DataCommon.SensorPositionSetting CreateSensorPositionSetting3D()
        {
            DataCommon.SensorPositionSetting data = new DataCommon.SensorPositionSetting();
            data.BolsterDepth = 100;
            data.BolsterWidth = 100;
            data.MoldDepth = 100;
            data.MoldPressDepth = 100;
            data.MoldPressWidth = 100;
            data.MoldWidth = 100;
            data.PositionList = new DataCommon.PositionSetting[10];

            DataCommon.PositionSetting[] poslists = new DataCommon.PositionSetting[10];


            DataCommon.PositionSetting posset = new DataCommon.PositionSetting();
            posset.ChNo = 1;
            posset.Way = DataCommon.PositionSetting.WayType.LEFT;
            posset.Target = DataCommon.PositionSetting.TargetType.LOWER;
            posset.X = 0;
            posset.Z = 0;
            poslists[0] = posset;

            posset = new DataCommon.PositionSetting();
            posset.ChNo = 2;
            posset.Way = DataCommon.PositionSetting.WayType.INVAILED;
            posset.Target = DataCommon.PositionSetting.TargetType.RUM;
            posset.X = -5;
            posset.Z = 5;
            poslists[1] = posset;

            posset = new DataCommon.PositionSetting();
            posset.ChNo = 3;
            posset.Way = DataCommon.PositionSetting.WayType.RIGHT;
            posset.Target = DataCommon.PositionSetting.TargetType.LOWER_PRESS;
            posset.X = 0;
            posset.Z = 0;
            poslists[2] = posset;


            posset = new DataCommon.PositionSetting();
            posset.ChNo = 4;
            posset.Way = DataCommon.PositionSetting.WayType.UP;
            posset.Target = DataCommon.PositionSetting.TargetType.STRIPPER;
            posset.X = 0;
            posset.Z = 0;
            poslists[3] = posset;

            posset = new DataCommon.PositionSetting();
            posset.ChNo = 5;
            posset.Way = DataCommon.PositionSetting.WayType.LEFT;
            posset.Target = DataCommon.PositionSetting.TargetType.UPPER;
            posset.X = 0;
            posset.Z = 0;
            poslists[4] = posset;

            posset = new DataCommon.PositionSetting();
            posset.ChNo = 6;
            posset.Way = DataCommon.PositionSetting.WayType.DOWN;
            posset.Target = DataCommon.PositionSetting.TargetType.UPPER_PRESS;
            posset.X = 10;
            posset.Z = 0;
            poslists[5] = posset;

            posset = new DataCommon.PositionSetting();
            posset.ChNo = 7;
            posset.Way = DataCommon.PositionSetting.WayType.INVAILED;
            posset.Target = DataCommon.PositionSetting.TargetType.STRIPPER;
            posset.X = 5;
            posset.Z = 5;
            poslists[6] = posset;



            posset = new DataCommon.PositionSetting();
            posset.ChNo = 8;
            posset.Way = DataCommon.PositionSetting.WayType.INVAILED;
            posset.Target = DataCommon.PositionSetting.TargetType.UPPER;
            posset.X = -5;
            posset.Z = -5;
            poslists[7] = posset;

            //posset = new DataCommon.PositionSetting();
            //posset.ChNo = 9;
            //posset.Way = DataCommon.PositionSetting.WayType.INVAILED;
            //posset.Target = DataCommon.PositionSetting.TargetType.STRIPPER;
            //posset.X = -10;
            //posset.Z = 15;
            //poslists[8] = posset;

            //posset = new DataCommon.PositionSetting();
            //posset.ChNo = 10;
            //posset.Way = DataCommon.PositionSetting.WayType.INVAILED;
            //posset.Target = DataCommon.PositionSetting.TargetType.STRIPPER;
            //posset.X = -5;
            //posset.Z = -9;
            //poslists[9] = posset;

            data.PositionList = poslists;

            return data;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja-JP", false);


            int width = 450;
            int height = 450;

            _FormGraph3D = new Graph3DForm();
            _FormGraph3D.MdiParent = this;
            _FormGraph3D.StartPosition = FormStartPosition.Manual;
            _FormGraph3D.Location = new System.Drawing.Point(width * (5 % 3), height * (5 / 3));
            _FormGraph3D.Size = new System.Drawing.Size(width, height);
            _FormGraph3D.SensorPosSetting = CreateSensorPositionSetting3D();
            _FormGraph3D.Show();

            _Forms = new GraphForm[_MaxGraph];
            cmbGraphList.Items.Clear();
            for (int i = 0; i < _MaxGraph; i++)
            {
                GraphForm graphform = new GraphForm();
                
                graphform.MdiParent = this;
                graphform.StartPosition = FormStartPosition.Manual;
                graphform.SelectLanguage = GraphLib.LanguageMode.Japanese;
                graphform.Location = new System.Drawing.Point(width * (i % 3), height * (i / 3));

                graphform.Text = global::TestApplication.Properties.Resources.GraphNameCombo + (i + 1).ToString();
                graphform.Name = "Graph" + (i + 1).ToString();
                graphform.Show();
                _Forms[i] = graphform;

                GraphLib.GraphInfo graphinfo = new GraphLib.GraphInfo();
                graphinfo.GraphNo = i + 1;
                graphinfo.GraphName = graphform.Text;
                graphinfo.IsEnabled = true;
                graphinfo.AxisNameX = "Axis X";
                graphinfo.AxisNameY = "Axis Y";
                graphinfo.MaxValueX = 20;
                graphinfo.MinValueX = 0;
                graphinfo.MaxValueY = 10;
                graphinfo.MinValueY = -10;
                graphinfo.MaxDataSizeX = 1500;
                graphinfo.MaxChannel = 10;
                graphinfo.IsLineGraph = false;
                graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();


                System.Windows.Media.Color[] testcolor = new System.Windows.Media.Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.AliceBlue, Colors.Aqua, Colors.Beige, Colors.BlueViolet, Colors.Crimson, Colors.DarkKhaki };

                for (int ch = 0; ch < 10; ch++)
                {

                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = testcolor[ch];
                    chinfo.CHLineSize = 1.5;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = global::TestApplication.Properties.Resources.ChannelNameCombo + (ch + 1).ToString();
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }

                cmbGraphList.Items.Add(graphinfo);
                graphform.GraphInfo = graphinfo;
            }

            cmbGraphList.ValueMember = "GraphNumber";
            cmbGraphList.DisplayMember = "DisplayGraphName";
            //this._DataThread = new System.Threading.Thread(datagen.CreateData);

            cmbMode.SelectedIndex = 0;
            cmbGraphList.SelectedIndex = 0;
            //ResourceManager res = new ResourceManager(typeof(global::TestApplication.Properties.Resources));            


            this.groupBox1.Text = global::TestApplication.Properties.Resources.SimulateData;
            this.label11.Text = global::TestApplication.Properties.Resources.Mode;
            this.label2.Text = global::TestApplication.Properties.Resources.MaxLoop;
            this.label5.Text = global::TestApplication.Properties.Resources.DataPerLoop;
            this.label8.Text = global::TestApplication.Properties.Resources.StatValue;
            this.label12.Text = global::TestApplication.Properties.Resources.CalcValue;
            this.button1.Text = global::TestApplication.Properties.Resources.Start;
            this.button2.Text = global::TestApplication.Properties.Resources.Stop;
            this.lblCh1.Text = global::TestApplication.Properties.Resources.CH1;
            this.lblCH2.Text = global::TestApplication.Properties.Resources.CH2;
            this.lblCH3.Text = global::TestApplication.Properties.Resources.CH3;
            this.lblCH4.Text = global::TestApplication.Properties.Resources.CH4;
            this.lblCH5.Text = global::TestApplication.Properties.Resources.CH5;
            this.lblCH6.Text = global::TestApplication.Properties.Resources.CH6;
            this.lblCH7.Text = global::TestApplication.Properties.Resources.CH7;
            this.lblCH8.Text = global::TestApplication.Properties.Resources.CH8;
            this.lblCH9.Text = global::TestApplication.Properties.Resources.CH9;
            this.lblCH10.Text = global::TestApplication.Properties.Resources.CH10;
            this.lblTime.Text = global::TestApplication.Properties.Resources.Time;
            //this.label3.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label4.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label7.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label10.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label13.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label16.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label19.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label22.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label25.Text = global::TestApplication.Properties.Resources.Unit;
            //this.label28.Text = global::TestApplication.Properties.Resources.Unit;
            this.label31.Text = global::TestApplication.Properties.Resources.TimeUnit;
            this.lblTime.Text = global::TestApplication.Properties.Resources.Time;
            this.btnEdit.Text = global::TestApplication.Properties.Resources.Edit;
            this.lblAxisNameX.Text = global::TestApplication.Properties.Resources.AxisNameX;
            this.lblAxisNameY.Text = global::TestApplication.Properties.Resources.AxisNameY;
            this.lblMaxPlot.Text = global::TestApplication.Properties.Resources.MaxPlot;
            this.lblMinMaxY.Text = global::TestApplication.Properties.Resources.MinMaxY;
            this.chkGraphEnabled.Text = global::TestApplication.Properties.Resources.Enabled;
            this.button4.Text = global::TestApplication.Properties.Resources.Update;
            this.Text = global::TestApplication.Properties.Resources.MainFormName + " - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            this.btnUpdAll.Text = global::TestApplication.Properties.Resources.UpdateAll;
            this.lblShotCount.Text = global::TestApplication.Properties.Resources.ShotCount;
            this.lblMaxChannel.Text = global::TestApplication.Properties.Resources.MaxCH;


        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ////Close Dispatcher for WPF !!!! Important!!!!
            //if (_Forms != null)
            //{
            //    if (_Forms.Length > 0)
            //        if (_Forms[0] != null)
            //        {
            //            GraphForm graphform = _Forms[0] as GraphForm;
            //            graphform.CloseDispatcher();
            //        }
            //}

            //if (_FormGraph3D != null)
            //    _FormGraph3D.CloseDispatcher();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(300);
            //for (int i = 0; i < cmbGraphList.Items.Count; i++)
            //{
            //    GraphLib.GraphInfo info = (GraphLib.GraphInfo)cmbGraphList.Items[i];
            //   // info.MaxPlotX = Convert.ToInt32(txtDataPerLoop.Text);
            //    if (cmbMode.SelectedIndex == 2)
            //        info.DecimalPointX = 3;
            //    else
            //        info.DecimalPointX = 0;

            //    cmbGraphList.Items[i] = info;
            //    _Forms[i].GraphInfo = info;

            //}

            if (cmbMode.SelectedIndex == 0)
            {

                StartGraph1();
            }
            else if (cmbMode.SelectedIndex == 1)
            {
                StartGraph2();
            }
            else if (cmbMode.SelectedIndex == 2)
            {
                StartGraph3();
            }
            else if (cmbMode.SelectedIndex == 3)
            {
                StartGraph3D();
            }

            _Mode = cmbMode.SelectedIndex;
            button1.Enabled = false;
            cmbMode.Enabled = false;


        }


        public void StartGraph2()
        {
            if (_DataThread != null)
            {
                if (this._DataThread.IsAlive)
                {
                    this._DataThread.Abort();

                }
            }
            this._DataThread = new System.Threading.Thread(datagen.CreateData);
            this._DataThread.IsBackground = true;
            this.datagen.MaxChannel = 10;
            this.datagen.CalcValue = Convert.ToDouble(txtCalcValue.Text);
            this.datagen.MaxDataPerLoop = Convert.ToInt32(txtDataPerLoop.Text);
            this.datagen.MaxLoop = Convert.ToInt32(txtMaxLoop.Text);
            this.datagen.StartValue = Convert.ToDouble(txtStartVal.Text); ;
            this.datagen.ShotCount = Convert.ToInt32(txtShotCount.Text);
            this._DataThread.Start();

        }

        public void StartGraph3()
        {
            if (_DataThread != null)
            {
                if (this._DataThread.IsAlive)
                {
                    this._DataThread.Abort();

                }
            }
            this._DataThread = new System.Threading.Thread(datagen.CreateDataMode3);
            this._DataThread.IsBackground = true;
            this.datagen.MaxChannel = 10;
            this.datagen.CalcValue = Convert.ToDouble(txtCalcValue.Text);
            this.datagen.MaxDataPerLoop = Convert.ToInt32(txtDataPerLoop.Text);
            this.datagen.MaxLoop = Convert.ToInt32(txtMaxLoop.Text);
            this.datagen.StartValue = Convert.ToDouble(txtStartVal.Text); ;

            this._DataThread.Start();

        }

        public void StartGraph3D()
        {
            if (_DataThread != null)
            {
                if (this._DataThread.IsAlive)
                {
                    this._DataThread.Abort();

                }
            }
            this._DataThread = new System.Threading.Thread(datagen.CreateData3DMode);
            this._DataThread.IsBackground = true;
            this.datagen.MaxChannel = 10;
            this.datagen.CalcValue = Convert.ToDouble(txtCalcValue.Text);
            this.datagen.MaxDataPerLoop = Convert.ToInt32(txtDataPerLoop.Text);
            this.datagen.MaxLoop = Convert.ToInt32(txtMaxLoop.Text);
            this.datagen.StartValue = Convert.ToDouble(txtStartVal.Text); ;
            this.datagen.ShotCount = Convert.ToInt32(txtShotCount.Text);
            this._DataThread.Start();

        }

        public void StartGraph1()
        {
            if (_DataThread != null)
            {
                if (this._DataThread.IsAlive)
                {
                    this._DataThread.Abort();

                }
            }

            this._DataThread = new System.Threading.Thread(datagen.CreateDataMode1);
            this._DataThread.IsBackground = true;
            this.datagen.MaxChannel = 10;
            this.datagen.CalcValue = Convert.ToDouble(txtCalcValue.Text);
            this.datagen.MaxDataPerLoop = Convert.ToInt32(txtDataPerLoop.Text);
            this.datagen.MaxLoop = Convert.ToInt32(txtMaxLoop.Text);
            this.datagen.StartValue = Convert.ToDouble(txtStartVal.Text); ;

            this._DataThread.Start();

        }

        public void DataCreated(List<double[]> dataOut)
        {
            List<double[]> dataoutp = new List<double[]>();
            dataoutp.AddRange(dataOut);
            if (_Mode != 3)
            {
                for (int i = 0; i < _MaxGraph; i++)
                {
                    GraphForm graphform = _Forms[i] as GraphForm;
                    graphform.DataCreated(dataoutp);
                    graphform.GraphData = dataoutp;
                    //graphform.IsRealTime = true;

                }
            }
            else
            {
                _FormGraph3D.DataCreated(dataoutp);
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetValue(SetValueOutput), dataoutp);
            }

        }

        public void SetValueOutput(List<double[]> inpData)
        {
            double[] data = (double[])inpData[inpData.Count - 1].Clone();

            if (data.Length == 11)
            {
                lblCH1Val.Text = data[1].ToString("0.00");
                lblCH2Val.Text = data[2].ToString("0.00");
                lblCH3Val.Text = data[3].ToString("0.00");
                lblCH4Val.Text = data[4].ToString("0.00");
                lblCH5Val.Text = data[5].ToString("0.00");
                lblCH6Val.Text = data[6].ToString("0.00");
                lblCH7Val.Text = data[7].ToString("0.00");
                lblCH8Val.Text = data[8].ToString("0.00");
                lblCH9Val.Text = data[9].ToString("0.00");
                lblCH10Val.Text = data[10].ToString("0.00");
            }

            if (data.Length > 1)
                txtTimeVal.Text = data[0].ToString();

        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (_DataThread != null)
            {
                _DataThread.Abort();
                for (int i = 0; i < _MaxGraph; i++)
                {
                    GraphForm graphform = _Forms[i] as GraphForm;
                    //graphform.IsRealTime = false;
                }
                button1.Enabled = true;
                cmbMode.Enabled = true;
            }
        }

        private void cmbGraphList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GraphLib.GraphInfo info = (GraphLib.GraphInfo)cmbGraphList.SelectedItem;

            txtAxisNameX.Text = info.AxisNameX;
            txtAxisNameY.Text = info.AxisNameY;
            txtMaxX.Text = info.MaxValueX.ToString();
            txtMinX.Text = info.MinValueX.ToString();
            txtMinY.Text = info.MinValueY.ToString();
            txtMaxY.Text = info.MaxValueY.ToString();
            txtMaxPlot.Text = info.MaxDataSizeX.ToString();
            txtShotCount.Text = info.ShotCount.ToString();
            txtMaxChannel.Text = info.MaxChannel.ToString();
            chkGraphEnabled.Checked = info.IsEnabled;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GraphLib.GraphInfo info = (GraphLib.GraphInfo)cmbGraphList.SelectedItem;

            info.AxisNameX = txtAxisNameX.Text;
            info.AxisNameY = txtAxisNameY.Text;
            info.MaxValueX = Convert.ToDouble(txtMaxX.Text);
            info.MinValueX = Convert.ToDouble(txtMinX.Text);
            info.MinValueY = Convert.ToDouble(txtMinY.Text);
            info.MaxValueY = Convert.ToDouble(txtMaxY.Text);
            info.MaxDataSizeX = Convert.ToInt32(txtMaxPlot.Text);
            info.IsEnabled = chkGraphEnabled.Checked;
            info.ShotCount = Convert.ToInt32(txtShotCount.Text);
            info.MaxChannel = Convert.ToInt32(txtMaxChannel.Text);

            cmbGraphList.Items[cmbGraphList.SelectedIndex] = info;

            _Forms[info.GraphNo - 1].GraphInfo = info;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ChannelSettingForm setform = new ChannelSettingForm();
            GraphLib.GraphInfo info = (GraphLib.GraphInfo)cmbGraphList.SelectedItem;
            setform.ChannelInfo = info.ChannelInfos;
            setform.ShowDialog();
            cmbGraphList.Items[cmbGraphList.SelectedIndex] = info;
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbMode.SelectedIndex)
            {
                case 0:
                    CreateGraphInfo1();
                    txtMaxLoop.Text = "1000";
                    txtDataPerLoop.Text = "1500";
                    txtStartVal.Text = "0";
                    txtCalcValue.Text = "-3";
                    break;
                case 1:
                    CreateGraphInfo2();
                    txtMaxLoop.Text = "1000";
                    txtDataPerLoop.Text = "3750";
                    txtStartVal.Text = "100";
                    txtCalcValue.Text = "0.75";
                    break;
                case 2:
                    CreateGraphInfo3();
                    txtMaxLoop.Text = "1000";
                    txtDataPerLoop.Text = "1000";
                    txtStartVal.Text = "0";
                    txtCalcValue.Text = "9.2";
                    break;
                case 3:
                    txtMaxLoop.Text = "1";
                    txtDataPerLoop.Text = "10000";
                    txtStartVal.Text = "0";
                    txtCalcValue.Text = "0";
                    break;
            }
        }



        private void CreateGraphInfo1()
        {
            cmbGraphList.Items.Clear();
            DateTime dt = DateTime.Now;
            for (int i = 0; i < _MaxGraph; i++)
            {

                GraphLib.GraphInfo graphinfo = _Forms[i].GraphInfo;
                graphinfo.GraphNo = i + 1;
                graphinfo.GraphName = global::TestApplication.Properties.Resources.GraphNameCombo + (i + 1).ToString();
                graphinfo.IsEnabled = true;
                graphinfo.AxisNameX = "Axis X";
                graphinfo.AxisNameY = "Axis Y";
                graphinfo.MaxValueX = 1500;
                graphinfo.MinValueX = 0;
                graphinfo.MaxValueY = 10;
                graphinfo.MinValueY = -10;
                graphinfo.MaxChannel = 10;
                graphinfo.MaxDataSizeX = 10000;
                graphinfo.ShotCount = 1;
                graphinfo.IsLineGraph = false;
                graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
                graphinfo.GraphMode = GraphLib.GraphMode.Moving;
                graphinfo.StartDateTime = dt;
                graphinfo.DateTimeFormat = "mm:ss.ff";
                graphinfo.ShowDateTimeAxisX = false;
                graphinfo.ShowValueLabelX = true;
                graphinfo.ShowValueLabelY = true;
                graphinfo.PlotCountX = 1500;
                graphinfo.IncrementX = 1;
                graphinfo.DistanceX = 1200;
                graphinfo.DistanceY = 5;
                graphinfo.AxisPositionX = null;
                //graphinfo.AxisPositionY =3;


                System.Windows.Media.Color[] testcolor = new System.Windows.Media.Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.AliceBlue, Colors.Aqua, Colors.Beige, Colors.BlueViolet, Colors.Crimson, Colors.DarkKhaki };

                for (int ch = 0; ch < graphinfo.MaxChannel; ch++)
                {

                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = testcolor[ch];
                    chinfo.CHLineSize = 1.5;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = global::TestApplication.Properties.Resources.ChannelNameCombo + (ch + 1).ToString();
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }

                cmbGraphList.Items.Add(graphinfo);
                _Forms[i].GraphInfo = graphinfo;
            }
            cmbGraphList.SelectedIndex = 0;
        }

        private void CreateGraphInfo2()
        {
            cmbGraphList.Items.Clear();
            for (int i = 0; i < _MaxGraph; i++)
            {

                GraphLib.GraphInfo graphinfo = _Forms[i].GraphInfo;
                graphinfo.GraphNo = i + 1;
                graphinfo.GraphName = global::TestApplication.Properties.Resources.GraphNameCombo + (i + 1).ToString();
                graphinfo.IsEnabled = true;
                graphinfo.AxisNameX = "Axis X";
                graphinfo.AxisNameY = "Axis Y";
                graphinfo.MaxValueX = 3750;
                graphinfo.MinValueX = 0;
                graphinfo.MaxValueY = 2000;
                graphinfo.MinValueY = 1000;
                graphinfo.MaxDataSizeX = 3750;
                graphinfo.MaxChannel = 10;
                graphinfo.ShotCount = 1;
                graphinfo.IsLineGraph = true;
                graphinfo.GraphMode = GraphLib.GraphMode.Normal;
                graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
                graphinfo.ShowDateTimeAxisX = false;
                graphinfo.ShowValueLabelX = true;
                graphinfo.ShowValueLabelY = true;
                graphinfo.PlotCountX = 3750;
                graphinfo.IncrementX = 1;
                graphinfo.DistanceX = 3250;


                System.Windows.Media.Color[] testcolor = new System.Windows.Media.Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.AliceBlue, Colors.Aqua, Colors.Beige, Colors.BlueViolet, Colors.Crimson, Colors.DarkKhaki };

                for (int ch = 0; ch < graphinfo.MaxChannel; ch++)
                {

                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = testcolor[ch];
                    chinfo.CHLineSize = 1.25;
                    chinfo.CHNo = ch+1;
                    chinfo.CHName = global::TestApplication.Properties.Resources.ChannelNameCombo + (ch + 1).ToString();
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }

                cmbGraphList.Items.Add(graphinfo);
                _Forms[i].GraphInfo = graphinfo;
            }
            cmbGraphList.SelectedIndex = 0;
        }


        private void CreateGraphInfo3()
        {
            cmbGraphList.Items.Clear();
            DateTime dt = DateTime.Now;
            for (int i = 0; i < _MaxGraph; i++)
            {

                GraphLib.GraphInfo graphinfo = _Forms[i].GraphInfo;
                graphinfo.GraphNo = i + 1;
                graphinfo.GraphName = global::TestApplication.Properties.Resources.GraphNameCombo + (i + 1).ToString();
                graphinfo.IsEnabled = true;
                graphinfo.AxisNameX = "Axis X";
                graphinfo.AxisNameY = "Axis Y";
                graphinfo.MaxValueX = 1000*50;
                graphinfo.MinValueX = 0;
                graphinfo.MaxValueY = 2000;
                graphinfo.MinValueY = 1200;
                graphinfo.MaxDataSizeX = 10000;
                graphinfo.MaxChannel = 1;
                graphinfo.IsLineGraph = true;
                graphinfo.ChannelInfos = new List<GraphLib.ChannelInfo>();
                graphinfo.DecimalPointX = 3;
                graphinfo.DecimalPointY = 2;
                graphinfo.ShotCount = 1;
                graphinfo.GraphMode = GraphLib.GraphMode.Moving;
                graphinfo.StartDateTime = dt;
                graphinfo.DateTimeFormat = "ss.ffff";
                graphinfo.ShowDateTimeAxisX = false;
                graphinfo.PlotCountX = 1000;
                graphinfo.IncrementX = 1;
                graphinfo.DistanceY = 500;

                System.Windows.Media.Color[] testcolor = new System.Windows.Media.Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.AliceBlue, Colors.Aqua, Colors.Beige, Colors.BlueViolet, Colors.Crimson, Colors.DarkKhaki };

                for (int ch = 0; ch < graphinfo.MaxChannel; ch++)
                {

                    GraphLib.ChannelInfo chinfo = new GraphLib.ChannelInfo();
                    chinfo.CHColor = testcolor[ch];
                    chinfo.CHLineSize = 1.25;
                    chinfo.CHNo = ch + 1;
                    chinfo.CHName = global::TestApplication.Properties.Resources.ChannelNameCombo + (ch + 1).ToString();
                    chinfo.IsEnabled = true;
                    graphinfo.ChannelInfos.Add(chinfo);
                }

                cmbGraphList.Items.Add(graphinfo);
                _Forms[i].GraphInfo = graphinfo;
            }
            cmbGraphList.SelectedIndex = 0;
        }

        private void txtCalcValue_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!(Convert.ToDouble(txtCalcValue.Text) <= 10))
                {
                    e.Cancel = true;
                    txtCalcValue.Text = "0";
                }
                else if (txtCalcValue.Text == string.Empty)
                {
                    e.Cancel = true;
                    txtCalcValue.Text = "0";
                }
            }
            catch (Exception)
            {
                e.Cancel = true;
                txtCalcValue.Text = "0";
            }
        }

        private void btnUpdAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _Forms.Length; i++)
            {
                GraphLib.GraphInfo info = _Forms[i].GraphInfo;

                info.AxisNameX = txtAxisNameX.Text;
                info.AxisNameY = txtAxisNameY.Text;
                info.MaxValueX = Convert.ToDouble(txtMaxX.Text);
                info.MinValueX = Convert.ToDouble(txtMinX.Text);
                info.MinValueY = Convert.ToDouble(txtMinY.Text);
                info.MaxValueY = Convert.ToDouble(txtMaxY.Text);
                info.MaxDataSizeX = Convert.ToInt32(txtMaxPlot.Text);
                info.IsEnabled = chkGraphEnabled.Checked;
                info.ShotCount = Convert.ToInt32(txtShotCount.Text);
                info.MaxChannel = Convert.ToInt32(txtMaxChannel.Text);
                cmbGraphList.Items[i] = info;
                _Forms[i].GraphInfo = info;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _MaxGraph; i++)
            {
                GraphForm graphform = _Forms[i] as GraphForm;
                //graphform.GraphZoomIn();
                graphform.PlotCount = graphform.PlotCount / 2;
                //graphform.CurrentLine = 5000;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _MaxGraph; i++)
            {
                GraphForm graphform = _Forms[i] as GraphForm;
                //graphform.GraphZoomOut();
                graphform.PlotCount = graphform.PlotCount * 2;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _MaxGraph; i++)
            {
                GraphForm graphform = _Forms[i] as GraphForm;
                graphform.GraphZoomReset();
            }
        }

        private void btnCenterScaleUpd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _MaxGraph; i++)
            {
                GraphForm graphform = _Forms[i] as GraphForm;
                graphform.CenterScaleUpdate(Convert.ToDecimal(txtCenter.Text), Convert.ToDecimal(txtScale.Text));
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _FormGraph3D.StartAnimation();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _FormGraph3D.StopAnimation();            
        }

        private void trackSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (trackSpeed.Value == 5)
                _FormGraph3D.SetSpeed(1);
            else if (trackSpeed.Value < 5)
                _FormGraph3D.SetSpeed(((double)trackSpeed.Value * 2) / 10);
            else if (trackSpeed.Value > 5)
                _FormGraph3D.SetSpeed(trackSpeed.Value - 5);
            
        }
       
        private void btnForward_Click(object sender, EventArgs e)
        {
            _FormGraph3D.MoveForward(100);
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            _FormGraph3D.MoveBackward(100);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _FormGraph3D.SetSize(0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _FormGraph3D.SetSize(1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _FormGraph3D.SetSize(2);
        }

    }
}
