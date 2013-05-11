using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using DataCommon;


namespace RM_3000.Forms.Analyze
{
    public partial class frmAnalyzeDataSelector : Form
    {
        /// <summary>
        /// タスクキャンセル
        /// </summary>
        CancellationTokenSource cts = null;

        /// <summary>
        /// タスクオブジェクト
        /// </summary>
        Task task = null;

        /// <summary>
        /// 選択フォルダ名
        /// </summary>
        public string SelectFolderName
        {
            get
            {
                return _SelectFolderName;
            }
            set
            {
                if (_SelectFolderName ==  System.IO.Path.GetDirectoryName(value))
                    return;

                _SelectFolderName =  System.IO.Path.GetDirectoryName(value);

                SetEnalbedControls(false);

                txtFileName.Text = _SelectFolderName;

                this.Cursor = Cursors.WaitCursor;

                cts = new System.Threading.CancellationTokenSource();

                task = Task.Factory.StartNew(() =>
                {
                    //ファイルリストの更新
                    CreateList();
                }, cts.Token );

                //コールバッグ
                task.ContinueWith((x) => { if (x.IsCompleted) LoadListCompleted(); });

            }
        }

        /// <summary>
        /// 選択解析フォルダ名
        /// </summary>
        public string SelectAnalyzeDataFolder
        {
            get{ return _SelectAnalyzeDataFolder;}
            set
            {
                _SelectAnalyzeDataFolder = System.IO.Path.GetDirectoryName(value); ;
                SelectFolderName = _SelectAnalyzeDataFolder.Substring(0, _SelectAnalyzeDataFolder.LastIndexOf('\\') + 1);
            }
        }

        /// <summary>
        /// 選択解析データ
        /// </summary>
        public AnalyzeData SelectAnalyzeData { get; set; }


        /// <summary>
        /// 選択フォルダ名
        /// </summary>
        private string _SelectFolderName = string.Empty;

        /// <summary>
        /// 選択解析フォルダ名
        /// </summary>
        private string _SelectAnalyzeDataFolder = string.Empty;

        /// <summary>
        /// 解析データリスト
        /// </summary>
        private List<AnalyzeData> AnalyzeDataList = new List<AnalyzeData>();

        /// <summary>
        /// ファイルリストの更新
        /// </summary>
        private void CreateList()
        {
            string[] Directroies = System.IO.Directory.GetDirectories(SelectFolderName);

            AnalyzeDataList.Clear();
            ShowList();

            //Application.DoEvents();

            foreach (string tmpFolder in Directroies)
            {
                AnalyzeData tmp = new AnalyzeData() { DirectoryPath = tmpFolder + @"\" };

                try
                {
                    //デシリアライズできれば
                    tmp.Desirialize_WithOut_Data();

                    //リストに追加
                    AnalyzeDataList.Add(tmp);
                }
                catch
                {
                }
                finally
                {
                    //Application.DoEvents();
                }
            }
            
        }

        /// <summary>
        /// ファイルリストから再表示
        /// </summary>
        private void ShowList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { ShowList(); });
                return;
            }

            //表示クリア
            dgvDataList.Rows.Clear();

            foreach (AnalyzeData ana in AnalyzeDataList)
            {

                if (rdoMode1.Checked && ana.MeasureSetting.Mode != 1)
                    continue;
                if(rdoMode2.Checked && ana.MeasureSetting.Mode != 2)
                    continue;
                if(rdoMode3.Checked && ana.MeasureSetting.Mode != 3)
                    continue;

                string tmp = ana.DirectoryPath.Replace(this.SelectFolderName + @"\", "").Replace(@"\","");

                TimeSpan tmpt = ana.MeasureData.EndTime - ana.MeasureData.StartTime;

                dgvDataList.Rows.Add(new object[] 
                {
                    tmp
                    , AppResource.GetString("TXT_MODE") + ana.MeasureSetting.Mode
                    , ana.MeasureData.StartTime
                    , string.Format("{0:D2}:{1:D2}:{2:D2}", (int)Math.Floor(tmpt.TotalHours), tmpt.Minutes, tmpt.Seconds)
                    , ana.MeasureData.SamplesCount
                    , ana.MeasureSetting.MeasTagList[0] != -1 ? ana.ChannelsSetting.ChannelSettingList[0].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[1] != -1 ? ana.ChannelsSetting.ChannelSettingList[1].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[2] != -1 ? ana.ChannelsSetting.ChannelSettingList[2].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[3] != -1 ? ana.ChannelsSetting.ChannelSettingList[3].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[4] != -1 ? ana.ChannelsSetting.ChannelSettingList[4].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[5] != -1 ? ana.ChannelsSetting.ChannelSettingList[5].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[6] != -1 ? ana.ChannelsSetting.ChannelSettingList[6].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[7] != -1 ? ana.ChannelsSetting.ChannelSettingList[7].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[8] != -1 ? ana.ChannelsSetting.ChannelSettingList[8].ChKind.ToString() : ""
                    , ana.MeasureSetting.MeasTagList[9] != -1 ? ana.ChannelsSetting.ChannelSettingList[9].ChKind.ToString() : ""
                });

            }

            if (dgvDataList.Rows.Count != 0)
            {
                dgvDataList.ScrollBars = ScrollBars.None;
                dgvDataList.ScrollBars = ScrollBars.Both;
                dgvDataList.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        /// <summary>
        /// リストデータのロード完了コールバック
        /// </summary>
        private void LoadListCompleted()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { LoadListCompleted(); });
                return;
            }

            //ファイルリストの再表示
            ShowList();

            this.Cursor = Cursors.Default;

            SetEnalbedControls(true);

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmAnalyzeDataSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォルダ選択ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFolderSelector_Click(object sender, EventArgs e)
        {
            var directory = string.Empty;
            var dialog = new FolderBrowserDialog();

            dialog.SelectedPath = string.IsNullOrEmpty(SelectFolderName) ? CommonLib.SystemDirectoryPath.MeasureData : SelectFolderName;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectFolderName = dialog.SelectedPath + "\\";
            }
        }

        /// <summary>
        /// OKボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (AnalyzeData ana in AnalyzeDataList)
            {
                if (ana.DirectoryPath.Replace(this.SelectFolderName + "\\", "").Replace("\\","") == dgvDataList.SelectedRows[0].Cells[0].Value.ToString())
                {
                    this.SelectAnalyzeData = (AnalyzeData)ana.Clone();
                    break;
                }
            }

            //this.SelectAnalyzeData = AnalyzeDataList[dgvDataList.SelectedRows[0].Index];
            this._SelectAnalyzeDataFolder = this.SelectAnalyzeData.DirectoryPath;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Cancelボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// Detelteボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDataList.SelectedRows != null)
            {

                foreach (AnalyzeData ana in AnalyzeDataList)
                {
                    if (ana.DirectoryPath.Replace(this.SelectFolderName + "\\", "").Replace("\\", "") == dgvDataList.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        this.SelectAnalyzeData = ana;
                        break;
                    }
                }

                if (System.IO.Directory.Exists(this.SelectAnalyzeData.DirectoryPath))
                {
                    if (MessageBox.Show(AppResource.GetString("MSG_DELETE_MEASUREDATA"), AppResource.GetString("MSG_DELETE_MEASUREDATA_TITLE"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.IO.Directory.Delete(this.SelectAnalyzeData.DirectoryPath, true);

                        dgvDataList.Rows.Remove(dgvDataList.SelectedRows[0]);

                        AnalyzeDataList.Remove(this.SelectAnalyzeData);

                        this.SelectAnalyzeData = null;

                    }
                }

            }
        }


        /// <summary>
        /// セルダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDataList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnOK.PerformClick();
            }
        }

        /// <summary>
        /// フィルタラジオ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoFillter_CheckedChanged(object sender, EventArgs e)
        {
            if(((RadioButton)sender).Checked)
                ShowList();
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeDataSelector_Load(object sender, EventArgs e)
        {
            // 言語切替
            AppResource.SetControlsText(this);

        }


        /// <summary>
        /// txtFileName キーダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tmpFileName = txtFileName.Text;

                if (tmpFileName.Substring(tmpFileName.Length - 1) != "\\")
                {
                    tmpFileName = tmpFileName + "\\";
                }

                if (System.IO.Directory.Exists(tmpFileName))
                {

                    SelectFolderName = tmpFileName;
                }
            }
        }

        /// <summary>
        /// フォーム閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnalyzeDataSelector_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (task != null && !task.IsCompleted)
            {
                cts.Cancel();

            }

            AnalyzeDataList.Clear();

            GC.Collect();
        }

        /// <summary>
        /// コントロールEnabled
        /// </summary>
        /// <param name="value"></param>
        private void SetEnalbedControls(bool value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { SetEnalbedControls(value); });
                return;
            }

            txtFileName.Enabled = value;
            rdoAll.Enabled = value;
            rdoMode1.Enabled = value;
            rdoMode2.Enabled = value;
            rdoMode3.Enabled = value;
            btnCancel.Enabled = value;
            btnOK.Enabled = value;
            dgvDataList.Enabled = value;
            btnFolderSelector.Enabled = value;
            btnDelete.Enabled = value;

        }
    }
}
