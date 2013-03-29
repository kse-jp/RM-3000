using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApplication
{
    public partial class Graph3DForm : Form
    {
        private DataCommon.SensorPositionSetting _SensorPosSetting;

        public DataCommon.SensorPositionSetting SensorPosSetting
        {
            get
            {
                return _SensorPosSetting;
            }
            set
            {
                _SensorPosSetting = value;
            }
        }

        public Graph3DForm()
        {
            InitializeComponent();
        }

        public void CloseDispatcher()
        {
            this.ucGraph3DViewer1.Dispatcher.InvokeShutdown();
        }

        private void Graph3DForm_Load(object sender, EventArgs e)
        {

            ucGraph3DViewer1.GraphInfo = SetGraphInfo(_SensorPosSetting);
            ucGraph3DViewer1.OnAnimationCompleted += new Graph3DLib.ucGraph3DViewer.AnimationCompletedEventHandler(ucGraph3DViewer1_OnAnimationCompleted);
            

        }

        private void ucGraph3DViewer1_OnAnimationCompleted(double duration)
        {
            this.ucGraph3DViewer1.ClearGraphData();
            MessageBox.Show("Complete:" + duration.ToString());
        }

        public void DataCreated(List<double[]> dataOut)
        {
            this.ucGraph3DViewer1.ClearGraphData();
            this.ucGraph3DViewer1.ReadData(dataOut);
            this.ucGraph3DViewer1.CreateAnimation();            

        }

        public void StartAnimation()
        {
            this.ucGraph3DViewer1.ResetPosition();
            this.ucGraph3DViewer1.StartAnimation();            
        }

        public void StopAnimation()
        {
            this.ucGraph3DViewer1.StopAnimation();
            
        }

        public void SetSize(int size)
        {
            this.ucGraph3DViewer1.ModelScaleSize = size;
        }

        

        public void SetSpeed(double speedRatio)
        {
            this.ucGraph3DViewer1.SetSpeed(speedRatio);
        }

        public void MoveForward(double timeMove)
        {
            this.ucGraph3DViewer1.MoveForward(timeMove);
        }

        public void MoveBackward(double timeMove)
        {
            this.ucGraph3DViewer1.MoveBackward(timeMove);
        }

        public void ClearData()
        {
            this.ucGraph3DViewer1.ClearGraphData();
        }

        private Graph3DLib.GraphInfo SetGraphInfo(DataCommon.SensorPositionSetting posSetting)
        {
            Graph3DLib.GraphInfo graphinfo = new Graph3DLib.GraphInfo();
            graphinfo.GraphName = "";
            graphinfo.GraphNo = 1;
            graphinfo.SensorPositions = new Graph3DLib.SensorPosition[posSetting.PositionList.Length];

            for (int i=0;i<posSetting.PositionList.Length;i++)
            {
                if (_SensorPosSetting.PositionList[i] != null)
                {
                    Graph3DLib.SensorPosition senpos = new Graph3DLib.SensorPosition();
                    senpos.Way = (Graph3DLib.WayType)posSetting.PositionList[i].Way;
                    senpos.Target = (Graph3DLib.TargetType)posSetting.PositionList[i].Target;
                    senpos.ChNo = posSetting.PositionList[i].ChNo;
                    senpos.X = posSetting.PositionList[i].X;
                    senpos.Z = posSetting.PositionList[i].Z;
                    graphinfo.SensorPositions[i] = senpos;
                }               
            }

            return graphinfo;
            
        }
    }
}
