using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;
using DataCommon;

using RM_3000.Forms.Parts;
using RM_3000.Forms.Graph;
using RM_3000.Classes;

namespace RM_3000.Forms.Measurement
{
    /// <summary>
    /// 計測中画面
    /// </summary>
    public partial class frmMeasureMain : Form
    {
        #region private member
        /// <summary>
        /// close button flag
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;
        /// <summary>
        /// 測定設定
        /// </summary>
        private MeasureSetting measSetting = null;
        /// <summary>
        /// 2Dグラフリスト
        /// </summary>
        private frmGraph2D[] graph2DList = new frmGraph2D[MeasureSetting.NumberOfGraph];
        /// <summary>
        /// 現在値リスト画面
        /// </summary>
        private frmTagValueList tagValueListForm;
        /// <summary>
        /// 測定制御画面
        /// </summary>
        private frmMeasureController controllerForm;
        /// <summary>
        /// 測定情報画面
        /// </summary>
        private frmMeasureInfo measInfoForm;
        /// <summary>
        /// グラフ設定画面
        /// </summary>
        private frmGraphController graphControllerForm;
        /// <summary>
        /// システム設定
        /// </summary>
        private SystemConfig systemSetting;
        /// <summary>
        /// 測定時間計測タイマー
        /// </summary>
        private System.Diagnostics.Stopwatch swMeasure = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// 測定データ収集タスク
        /// </summary>
        private MeasureDataTask measureTask = null;
        /// <summary>
        /// 測定開始済みフラグ
        /// </summary>
        private bool bAllReadyStart = false;
        /// <summary>
        /// 測定シーケンスクラス
        /// </summary>
        private RM_3000.Sequences.TestSequence testSquence = RM_3000.Sequences.TestSequence.GetInstance();
        /// <summary>
        /// 2Dグラフ現在値
        /// </summary>
        private decimal currentLine = 0.0m;
        /// <summary>
        /// 測定完了済みフラグ
        /// </summary>
        private bool bMeasureClosed = false;

        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmMeasureMain(LogManager log)
        {
            InitializeComponent();

            this.log = log;
            this.systemSetting = new SystemConfig();
            this.systemSetting.LoadXmlFile();

            //測定状態の変更イベント
            testSquence.StatusChanged += new Sequences.TestSequence.StatusChangedEventHander(testSquence_StatusChanged);
        }

        /// <summary>
        /// 測定状態の変更イベント
        /// </summary>
        /// <param name="status"></param>
        void testSquence_StatusChanged(Sequences.TestSequence.TestStatusType status)
        {

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker) delegate{ testSquence_StatusChanged(status); });
                return;
            }


            switch (status)
            {
                case Sequences.TestSequence.TestStatusType.Run:

                    controllerForm.SetMeasureStatus(frmMeasureController.MeasureStatus.Start);

                    bAllReadyStart = true;

                    this.measureTask.Start();

                    this.swMeasure.Reset();
                    this.swMeasure.Start();
                    ShowStatusMessage(AppResource.GetString("TXT_MEASURE_START"));

                    break;
                case Sequences.TestSequence.TestStatusType.Pause:
                    controllerForm.SetMeasureStatus(frmMeasureController.MeasureStatus.Stop);

                    //for (int i = 0; i < this.graph2DList.Length; i++)
                    //{
                    //    if (this.graph2DList[i] != null)
                    //    {
                    //        this.graph2DList[i].IsRealTime = false;
                    //    }
                    //}

                    this.swMeasure.Stop();

                    ShowStatusMessage(AppResource.GetString("MSG_MEAS_STOP_TEST"));

                    this.measureTask.Pause();

                    break;
                case Sequences.TestSequence.TestStatusType.Stop:
                    controllerForm.SetMeasureStatus(frmMeasureController.MeasureStatus.Exit);

                    ShowStatusMessage(AppResource.GetString("MSG_MEAS_END"));
                    try
                    {
                        this.controllerForm.Enabled = false;
                        this.graphControllerForm.Enabled = false;
                        this.Enabled = false;

                        RealTimeData.EndData();
                        this.measureTask.Pause();

                        bool bret = true;

                        //データが一つでも受信されていればデータ保存する。
                        if (RealTimeData.receiveCount != 0)
                        {
                            ShowStatusMessage(AppResource.GetString("MSG_MEAS_SAVE_FILES"));
                            // 測定設定ファイル群及びデータファイルを保存する
                            bret = SaveMeasureFiles();
                        }

                        if (bret)
                            testSquence.ExitTest();
                        else
                        {
                            //画面終了しない。再開があるため。
                            return;
                        }

                        //測定完了フラグオン
                        bMeasureClosed = true;
                    }
                    finally
                    {
                        this.controllerForm.Enabled = true;
                        this.graphControllerForm.Enabled = true;
                        this.Enabled = true;

                    }

                    //画面終了
                    if (this.InvokeRequired)
                        this.Invoke((MethodInvoker)delegate() { this.Close(); });
                    else
                        this.Close();

                    break;

                //緊急停止
                case Sequences.TestSequence.TestStatusType.EmergencyStop:
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate()
                            {
                                this.measureTask.Pause();

                                MessageBox.Show(AppResource.GetString("MSG_MEAS_EMERGENCY_STOP"), AppResource.GetString("TXT_MEASUREMENT"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                //測定を停止する。
                                testSquence.EndTest();
                            }
                            );
                    }
                    break;
            }
        }
        #endregion

        #region public method
        /// <summary>
        /// 温度センサー有無をチェックする
        /// </summary>
        /// <param name="strErrChannel"></param>
        /// <returns></returns>
        public bool CheckTempSensor(out string strErrChannel)
        {
            if (!SystemSetting.SystemConfig.IsSimulationMode)
                return testSquence.CheckTempSensor(out strErrChannel);
            else
            {
                strErrChannel = string.Empty;
                return true;
            }
        }
        #endregion

        #region private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bCond_MeasurePause"></param>
        private void GotCondition(bool bCond_MeasurePause)
        {

            this.controllerForm.SetHardTime(RealTimeData.LastDataReceiveTime.ToString("HH:mm:ss"));

            var time = DateTime.Now - RealTimeData.GetStartTime();
            this.controllerForm.SetMeasureTime(time.ToString("hh\\:mm\\:ss\\.fff"));

            this.controllerForm.SetCondition(bCond_MeasurePause);

        }

        /// <summary>
        /// 測定データをセットする
        /// </summary>
        /// <param name="dataList">測定データ</param>
        public void SetMeasureData(SampleData[] dataList)
        {
            try
            {
                //var time = DateTime.Now - RealTimeData.GetStartTime();
                //this.controllerForm.SetMeasureTime(time.ToString("hh\\:mm\\:ss\\.fff"));

                //総サンプル数の取得
                this.controllerForm.SetMeasureCount(RealTimeData.receiveCount.ToString());
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Error Message
        /// </summary>
        /// <param name="ex"></param>
        private void ShowErrorMessage(string message)
        {
            if (this.log != null) this.log.PutErrorLog(message);
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// show measurement status
        /// </summary>
        /// <param name="message"></param>
        private void ShowStatusMessage(string message)
        {
            if (this.controllerForm != null)
            {
                this.controllerForm.SetMeasureMessage(message);
            }
        }
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureMain_Load(object sender, EventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureMain.frmMeasureMain_Load() - 測定中画面のロードを開始しました。");

            try
            {
                RealTimeData.DataTagSetting = (DataTagSetting)SystemSetting.DataTagSetting.Clone();
                // 言語切替
                AppResource.SetControlsText(this);

                // 測定設定ファイル読み込み
                var xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + MeasureSetting.FileName;
                if (System.IO.File.Exists(xmlFilePath))
                {
                    this.measSetting = (MeasureSetting)MeasureSetting.Deserialize(xmlFilePath);
                }
                else
                {
                    MessageBox.Show(AppResource.GetString("ERROR_MEASURE_SETTING_FILE_NOT_FOUND"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                // 2Dグラフの設定
                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.measSetting.GraphSettingList.Length <= i) break;

                    if (this.measSetting.GraphSettingList[i].IsValid)
                    {
                        this.graph2DList[i] = new frmGraph2D(this.log, i) { MdiParent = this, CurrentValueLineChanged = Graph2DCurrentValueLineChanged, FormHidden = Graph2DFormHidden };
                        this.graph2DList[i].Resize += new EventHandler(this.graph_Resize);
                        this.graph2DList[i].FormClosed += new FormClosedEventHandler(this.graph2D_FormClosed);
                    }
                }


                // 各種表示フォームの設定
                this.tagValueListForm = new frmTagValueList(this.log) { MdiParent = this, Top = 80, Left = 0 };
                this.controllerForm = new frmMeasureController(this.log) { MdiParent = this, Top = 0, Left = 0, MeasureStatusChanged = MeasureStatusChangedCallback, Mode = this.measSetting.Mode };
                this.measInfoForm = new frmMeasureInfo(this.log) { MdiParent = this, Top = 0, Left = 1000, MeasSetting = this.measSetting };
                this.graphControllerForm = new frmGraphController(this.log) { MdiParent = this, Left = 0, Mode = this.measSetting.Mode, GraphFormList = this.graph2DList, GraphZoomInOccurred = this.Graph2DZoomIn, GraphZoomOutOccurred = this.Graph2DZoomOut, GraphArrangeOccurred = this.Graph2DArrange };

                // 測定データ収集タスク
                this.measureTask = new MeasureDataTask(this.log);
                this.measureTask.GotCondition += new MeasureDataTask.GotConditionDelegate(this.GotCondition);
                this.measureTask.DataReceived += new MeasureDataTask.DataReceivedDelegate(this.SetMeasureData);
                this.measureTask.DataReceived += new MeasureDataTask.DataReceivedDelegate(this.tagValueListForm.SetMeasureData);
                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        this.measureTask.DataReceived += new MeasureDataTask.DataReceivedDelegate(this.graph2DList[i].SetMeasureData);
                    }
                }

                ArrangeGraphForms();
                SetMaximumGraphArea(null);

                // 測定処理クラス初期化
                testSquence.InitPreMeasure(true);
                ShowStatusMessage(AppResource.GetString("MSG_MEAS_INIT"));
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureMain.frmMeasureMain_Load() - 測定中画面のロードを終了しました。");
        }

        /// <summary>
        /// フォーム表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureMain_Shown(object sender, EventArgs e)
        {
            try
            {
                this.controllerForm.Show();
                this.measInfoForm.Show();
                this.tagValueListForm.Show();

                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        this.graph2DList[i].Show();
                    }
                }

                this.graphControllerForm.Show();    // グラフ表示後に表示すること
                this.graphControllerForm.Top = this.controllerForm.Height + this.tagValueListForm.Height;


                string strErrChannel = string.Empty;

                //温度センサー有無確認
                if (!CheckTempSensor(out strErrChannel))
                {
                    if (strErrChannel != string.Empty)
                        MessageBox.Show(string.Format(AppResource.GetString("MSG_TEMPCHECK"), strErrChannel), AppResource.GetString("MSG_TEMPCHECK_CAPTION"));

                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// フォームクロージングイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.log != null) this.log.PutLog("frmMeasureMain.frmMeasureMain_FormClosing() - in");

            try
            {
                this.controllerForm.MeasureStatusChanged = null;

                // 2Dグラフを終了する
                foreach (var f in this.graph2DList)
                {
                    if (f != null)
                    {
                        f.Close();
                    }
                }

                // シミュレータ及び測定データ収集タスクを終了する
                if (this.systemSetting.IsSimulationMode)
                {
                    RealTimeData.StopSimulator();
                }

                //計測停止していなければ(フォームの×が押された）
                if (!bMeasureClosed)
                {
                    // シミュレータ及び測定データ収集タスクを終了する
                    if (!this.systemSetting.IsSimulationMode)
                    {
                        //計測停止処理
                        ShowStatusMessage(AppResource.GetString("MSG_MEAS_END"));

                        if (testSquence.TestStatus == Sequences.TestSequence.TestStatusType.Stop)
                        {
                            //既に測定は停止しているため、強制的に停止した事にする。
                            testSquence_StatusChanged(Sequences.TestSequence.TestStatusType.Stop);
                        }
                        else
                        {
                            //測定終了処理
                            if (!testSquence.EndTest())
                            {
                                //エラーとなっても試験は終了しているものとみなす。
                                testSquence.TestStatus = Sequences.TestSequence.TestStatusType.Stop;
                            }
                        }
                    }
                    else
                    {
                        //Simulatorモードならここで終了処理
                        RealTimeData.EndData();

                        bool bret = false;

                        if (RealTimeData.receiveCount != 0)
                        {
                            ShowStatusMessage(AppResource.GetString("MSG_MEAS_SAVE_FILES"));
                            //測定設定ファイル群及びデータファイルを保存する
                            bret = SaveMeasureFiles();

                            if (bret)
                            {
                                //測定終了後処理
                                testSquence.ExitTest();

                            }
                            else
                            {
                                e.Cancel = true;
                                if (this.log != null) this.log.PutLog("frmMeasureMain.frmMeasureMain_FormClosing() - Cancel out");
                                return;

                            }
                        }
                    }
                }

                this.measureTask.Stop();

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

            if (this.log != null) this.log.PutLog("frmMeasureMain.frmMeasureMain_FormClosing() - out");
        }

        /// <summary>
        /// frmMeasureMain_Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMeasureMain_Resize(object sender, EventArgs e)
        {
            try
            {
                SetMaximumGraphArea(null);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// グラフフォームの位置を調整する
        /// </summary>
        private void ArrangeGraphForms()
        {
            try
            {
                var count = 0;
                var topOffset = this.controllerForm.Top + this.controllerForm.Height;
                var leftOffset = this.tagValueListForm.Width;

                for (int i = 0; i < this.graph2DList.Length; i++)
                {
                    if (this.graph2DList[i] != null)
                    {
                        var g = this.graph2DList[i];
                        g.WindowState = FormWindowState.Normal;
                        g.Height = 300;
                        g.Width = 945;
                        g.Top = (count * 300) + topOffset;
                        g.Left = leftOffset;
                        g.ZoomReset();
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定ステータス変更イベント
        /// </summary>
        /// <param name="status"></param>
        private void MeasureStatusChangedCallback(frmMeasureController.MeasureStatus status)
        {
            System.Threading.Thread Methodth = null;
            try
            {
                switch (status)
                {
                    case frmMeasureController.MeasureStatus.Start:      // 測定開始

                        //for (int i = 0; i < this.graph2DList.Length; i++)
                        //{
                        //    if (this.graph2DList[i] != null)
                        //    {
                        //        this.graph2DList[i].IsRealTime = true;
                        //    }
                        //}

                        if (this.systemSetting.IsSimulationMode)
                        {
                            // シミュレーション開始
                            if (this.measSetting.Mode == 1)
                            {
                                RealTimeData.StartSimulatorMode(1);
                            }
                            else if (this.measSetting.Mode == 2)
                            {
                                RealTimeData.StartSimulatorMode(2);
                            }
                            else if (this.measSetting.Mode == 3)
                            {
                                RealTimeData.StartSimulatorMode(3);
                            }
                        }
                        else
                        {
                            if (bAllReadyStart)
                            {
                                //Methodth = new System.Threading.Thread();
                                //測定開始処理
                                testSquence.ResumeTest();
                            }
                            else
                            {
                                //測定開始処理
                                testSquence.StartTest();
                            }
                        }

                        //Simulatorモードならば開始状態処理をここにいれる。
                        if (this.systemSetting.IsSimulationMode)
                        {
                            bAllReadyStart = true;

                            this.measureTask.Start();

                            this.swMeasure.Reset();
                            this.swMeasure.Start();
                            ShowStatusMessage(AppResource.GetString("TXT_MEASURE_START"));
                        }
                        break;

                    case frmMeasureController.MeasureStatus.Stop:       // 測定停止

                        if (this.systemSetting.IsSimulationMode)
                        {
                            RealTimeData.StopSimulator();

                            ////Simulatorモードならばここで後処理をする
                            //for (int i = 0; i < this.graph2DList.Length; i++)
                            //{
                            //    if (this.graph2DList[i] != null)
                            //    {
                            //        this.graph2DList[i].IsRealTime = false;
                            //    }
                            //}
                            ShowStatusMessage(AppResource.GetString("MSG_MEAS_STOP_TEST"));
                            this.measureTask.Pause();

                            ////測定設定ファイル群及びデータファイルを保存する
                            //SaveMeasureFiles();
                        }
                        else
                        {
                            //測定停止処理
                            testSquence.StopTest();
                        }

                        break;

                    case frmMeasureController.MeasureStatus.Exit:       // 終了

                        if (!this.systemSetting.IsSimulationMode)
                        {
                            if (testSquence.TestStatus == Sequences.TestSequence.TestStatusType.Stop)
                            {
                                //既に測定は停止しているため、強制的に停止した事にする。
                                testSquence_StatusChanged(Sequences.TestSequence.TestStatusType.Stop);
                            }
                            else
                            {
                                //測定終了処理
                                if (!testSquence.EndTest())
                                {
                                    //停止処理が失敗しても終了として処理を行う
                                    testSquence.TestStatus = Sequences.TestSequence.TestStatusType.Stop;
                                }
                            }
                        }
                        else
                        {
                            bool bret = true;

                            //Simulatorモードならばここで終了
                            RealTimeData.EndData();
                            this.measureTask.Pause();

                            //データが一つでも受信されていればデータ保存する。
                            if (RealTimeData.receiveCount != 0)
                            {
                                ShowStatusMessage(AppResource.GetString("MSG_MEAS_SAVE_FILES"));
                                // 測定設定ファイル群及びデータファイルを保存する
                                bret = SaveMeasureFiles();
                            }

                            if (bret)
                            {
                                //測定終了後処理
                                testSquence.ExitTest();

                                //測定完了フラグオン
                                bMeasureClosed = true;
                                //画面終了
                                this.Close();
                            }
                            else
                            {
                                //リアルタイム保存の再開
                                RealTimeData.ResumeData();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        /// <summary>
        /// 測定設定ファイル群及びデータファイルを保存する
        /// </summary>
        private bool SaveMeasureFiles()
        {
            try
            {
                //var directory = CommonLib.SystemDirectoryPath.MeasureData + DateTime.Now.ToString(AppResource.GetString("TXT_DATA_DIRECTORY_NAME_FORMAT")) + @"\";
                var directory = RealTimeData.FolderPath + @"\";

                var filesCopyFrom = new string[] 
                {
                    //CommonLib.SystemDirectoryPath.SystemPath + DataTagSetting.FileName,　//DataTag設定はRealTimeDataのものをSerializeする。
                    CommonLib.SystemDirectoryPath.SystemPath + ConstantSetting.FileName,
                    CommonLib.SystemDirectoryPath.SystemPath + MeasureSetting.FileName,
                    CommonLib.SystemDirectoryPath.SystemPath + ChannelsSetting.FileName,
                    CommonLib.SystemDirectoryPath.SystemPath + SensorPositionSetting.FileName,
                    CommonLib.SystemDirectoryPath.SystemPath + TagChannelRelationSetting.FileName,
                    CommonLib.SystemDirectoryPath.SystemPath + CalibrationTableArray.FileName //検量線追加
                };

                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                foreach (var file in filesCopyFrom)
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Copy(file, directory + System.IO.Path.GetFileName(file));
                    }
                }

                //DataTag設定はRealTimeDataのものをSerializeする。
                RealTimeData.DataTagSetting.FilePath = directory + DataTagSetting.FileName;
                RealTimeData.DataTagSetting.Serialize();

                frmInputSaveFileName frm = new frmInputSaveFileName();

                string DefaultFileName = System.IO.Path.GetFileName(directory.Substring(0, directory.Length - 1));
                frm.FolderName = directory.Substring(0, directory.Length - (2 + DefaultFileName.Length));
                frm.FileName = DefaultFileName;
                DialogResult ret = frm.ShowDialog();

                //保存あり
                if (ret == System.Windows.Forms.DialogResult.Yes)
                {
                    //ファイル保存場所指定があればフォルダ名変更
                    if (frm.FileName != DefaultFileName)
                    {
                        //すでにフォルダがあれば削除する
                        if (System.IO.Directory.Exists(directory.Replace(DefaultFileName, frm.FileName)))
                            System.IO.Directory.Delete(directory.Replace(DefaultFileName, frm.FileName), true);

                        //データの移動
                        System.IO.Directory.Move(directory, directory.Replace(DefaultFileName, frm.FileName));
                    }
                }
                //保存せず
                else if (ret == System.Windows.Forms.DialogResult.No)
                {
                    //既に保存されているデータを破棄する（削除）。
                    System.IO.Directory.Delete(directory, true);

                }
                else if (ret == System.Windows.Forms.DialogResult.Cancel)
                {
                    //何もせずに抜ける
                    return false;
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set Max Size of Graph Area 
        /// </summary>
        private void SetMaximumGraphArea(object formInput)
        {
            try
            {
                var verticalscroll = 20;
                var horizontalscroll = 20;

                var initx = this.tagValueListForm.Location.X + this.tagValueListForm.Width;
                var inity = this.controllerForm.Location.Y + this.controllerForm.Height;

                var width = this.DisplayRectangle.Width - initx - verticalscroll;
                var height = this.DisplayRectangle.Height - inity - horizontalscroll;

                if (formInput == null)
                {
                    for (int i = 0; i < this.graph2DList.Length; i++)
                    {
                        if (this.graph2DList[i] != null)
                        {
                            this.graph2DList[i].InitialMaxPoint = new Point(initx, inity);
                            this.graph2DList[i].MaxFormSize = new Size(width, height);
                        }
                    }
                }
                else
                {
                    var form2d = formInput as frmGraph2D;

                    if (form2d != null)
                    {
                        form2d.InitialMaxPoint = new Point(initx, inity);
                        form2d.MaxFormSize = new Size(width, height);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// form graph2D and graph3D resize check maximum form area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graph_Resize(object sender, EventArgs e)
        {
            try
            {


                SetMaximumGraphArea(sender);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        /// 2D表示画面の終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void graph2D_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                var f = sender as frmGraph2D;
                f.FormClosed -= graph2D_FormClosed;
                f.Resize -= graph_Resize;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
        #region Delegates for 2D Graph Controller
        /// <summary>
        /// 2Dグラフ拡大
        /// </summary>
        private void Graph2DZoomIn()
        {
            for (int i = 0; i < this.graph2DList.Length; i++)
            {
                if (this.graph2DList[i] != null)
                {
                    var info = this.graph2DList[i].GraphInfo;

                    if (this.graph2DList[i].PlotCount / 2 < info.MaxDataSizeX * 0.05 || this.graph2DList[i].PlotCount / 2 <= 0)
                    {
                        if (Convert.ToInt32(info.MaxDataSizeX * 0.05) > 0)
                            this.graph2DList[i].PlotCount = Convert.ToInt32(info.MaxDataSizeX * 0.05);                      
                    }
                    else
                    {
                        this.graph2DList[i].PlotCount = this.graph2DList[i].PlotCount / 2;
                        this.graph2DList[i].ZoomXCounter++;
                    }
                }
            }
        }
        /// <summary>
        /// 2Dグラフ縮小
        /// </summary>
        private void Graph2DZoomOut()
        {
            for (int i = 0; i < this.graph2DList.Length; i++)
            {
                if (this.graph2DList[i] != null)
                {
                    var info = this.graph2DList[i].GraphInfo;
                    int maxdatasize = info.MaxDataSizeX;

                    if (this.measSetting.Mode == 3 && maxdatasize > 1)
                        maxdatasize--;

                    if (this.graph2DList[i].PlotCount * 2 > maxdatasize )
                    {
                        this.graph2DList[i].ZoomXCounter = 0;
                        this.graph2DList[i].PlotCount = maxdatasize;
                    }
                    else
                    {
                        this.graph2DList[i].PlotCount = this.graph2DList[i].PlotCount * 2;
                        this.graph2DList[i].ZoomXCounter--;
                    }


                }
            }
        }
        /// <summary>
        /// 2Dグラフ位置調整
        /// </summary>
        private void Graph2DArrange()
        {
            ArrangeGraphForms();
        }
        /// <summary>
        /// 現在値ライン変更コールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        /// <param name="currentLine">現在値ライン</param>
        private void Graph2DCurrentValueLineChanged(int index, decimal currentLine)
        {
            if (this.currentLine == currentLine)
            {
                return;
            }
            this.currentLine = currentLine;

            for (int i = 0; i < this.graph2DList.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                if (this.graph2DList[i] != null)
                {
                    try { this.graph2DList[i].CurrentLine = this.currentLine; }
                    catch { }
                }
            }
        }
        /// <summary>
        /// グラフフォームクローズコールバック
        /// </summary>
        /// <param name="index">グラフインデックス [0-5]</param>
        private void Graph2DFormHidden(int index)
        {
            if (this.graphControllerForm != null)
            {
                this.graphControllerForm.RefreshGraph(index);
            }
        }
        #endregion

        private void frmMeasureMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //測定状態の変更イベント
            testSquence.StatusChanged -= testSquence_StatusChanged;
        }

        #endregion

    }
}
