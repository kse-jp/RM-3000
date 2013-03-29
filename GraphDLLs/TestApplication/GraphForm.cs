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
    public partial class GraphForm : Form
    {                       
        private System.Threading.Thread _DataThread;
        private DataGenerator datagen = null;
        private List<double[]> _GraphData;

        public List<double[]> GraphData
        {
            get
            {
                return _GraphData;
            }
            set
            {
                _GraphData = value;
            }
        }

        //public bool IsRealTime
        //{
        //    set
        //    {
        //        this.ucGraphViewer1.IsRealTime = value;
        //    }
        //}

        public int PlotCount
        {
            set { this.ucGraphViewer1.PlotCount = value; }
            get { return this.ucGraphViewer1.PlotCount; }
        }

        public decimal CurrentLine
        {
            get
            {
                return ucGraphViewer1.CurrentLine;
            }
            set
            {
                ucGraphViewer1.CurrentLine = value;
            }
        }

        public GraphLib.GraphInfo GraphInfo
        {
            get
            {
                return this.ucGraphViewer1.GraphInfo;
            }
            set
            {
                this.ucGraphViewer1.UpdateGraphInfo(value,true);
                
            }
        }

        public GraphLib.LanguageMode SelectLanguage
        {
            get
            {
                return this.ucGraphViewer1.SelectLanguage;
            }
            set
            {
                this.ucGraphViewer1.SelectLanguage =value;

            }
        }

        public GraphForm()
        {
            InitializeComponent();
            
            
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            this.ucGraphViewer1.GraphGridLoad(elementHost1.Width, elementHost1.Height);
            this.ucGraphViewer1.CurrentLineChanged += CurrentLineChanged;
            this.ucGraphViewer1.EnableCurrentLine = true;                      
        }

        private void CurrentLineChanged(decimal inpdata)
        {

        }
        public void CloseDispatcher()
        {
            this.ucGraphViewer1.Dispose();
            //this.ucGraphViewer1.Dispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void GraphForm_Resize(object sender, EventArgs e)
        {
           this.ucGraphViewer1.ResizeGraph(elementHost1.Width,elementHost1.Height);          
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
            
        }

        public void StartGraph()
        {
            //this._DataThread = new System.Threading.Thread(datagen.CreateData);
            //this._DataThread.IsBackground = true;          
            //this._DataThread.Start();
            
        }

        public void DataCreated(List<double[]> dataOut)
        {
            //this.ucGraphViewer1.IsRealTime = true;
            this.ucGraphViewer1.ReadData(dataOut);
            this.ucGraphViewer1.CreateGraph();


        }

        

        public void GraphZoomIn()
        {
            this.ucGraphViewer1.ZoomIn();
            
        }

      

        public void GraphZoomOut()
        {
            this.ucGraphViewer1.ZoomOut();
        }

        public void GraphZoomReset()
        {
            this.ucGraphViewer1.ZoomReset();
            //this.ucGraphViewer1.MaximumX = 1000;
        }

        private void GraphForm_Move(object sender, EventArgs e)
        {
            this.Refresh();            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ucGraphViewer1.IsRealTime = false;
            this._DataThread.Abort();
        }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        public void CenterScaleUpdate(decimal center,decimal scale)
        {
            this.ucGraphViewer1.CenterScale = center;
            this.ucGraphViewer1.Scale = scale;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StartGraph();
        }

       
    }
}
