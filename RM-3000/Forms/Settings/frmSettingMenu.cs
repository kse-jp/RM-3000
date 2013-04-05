using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CommonLib;

namespace RM_3000.Forms.Settings
{
    public partial class frmSettingMenu : Form
    {
        public event EventHandler LanguageChanged;

        List<Image> imageList1 = new List<Image>();

        CommonLib.SystemConfig systemconfig = null;

        public frmSettingMenu()
        {
            InitializeComponent();

            ContentsLoad();
        }
        /// <summary>
        /// ログ
        /// </summary>
        private readonly LogManager log;

        private Bitmap bmBackgoundBase;
        private Bitmap bmBackgound;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="log">ログ</param>
        public frmSettingMenu(LogManager log)
        {
            InitializeComponent();

           this.SetStyle(ControlStyles.UserPaint, true);
           this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
           this.SetStyle(ControlStyles.DoubleBuffer, true);
           this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
 
            this.log = log;

            AppResource.SetControlsText(this);
 
            ContentsLoad();

            InitButtonImage();

            InitLanguageButtonImage();

            this.LanguageChanged += new EventHandler(frmSettingMenu_LanguageChanged);

        }


        ///// <summary>
        ///// Override OnPaint of Form for write backgound
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    if (bmBackgound != null)
        //    {
        //        Graphics dc = e.Graphics;

        //        dc.DrawImage(bmBackgound, 0, 0);
        //        dc.Dispose();
        //    }

        //    base.OnPaint(e);
        //}

        /// <summary>
        /// コンテンツ画像等のロード
        /// </summary>
        /// <remarks>
        /// リソースに入れると大量データとなるため、
        /// 逐次ロードするようにする。
        /// </remarks>
        private void ContentsLoad()
        {
            System.IO.FileStream fs;

            // Channel Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Channel_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Channel_MouseON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Channel_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Tag Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Tag_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Tag_MouseON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Tag_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Relation Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Relation_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Relation_MouseON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_Relation_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // SensorPosition Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorPosition_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorPosition_MouseON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorPosition_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Position Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorAdjustment_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorAdjustment_MouseON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Button_SensorAdjustment_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Channes Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_Chinese_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_Chinese_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // English Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_English_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_English_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            // Japanese Button
            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_Japanese_OFF.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            fs = System.IO.File.OpenRead("Resources\\Images\\Buttons\\Setting\\Language\\Button_Japanese_ON.png");
            imageList1.Add(Image.FromStream(fs, false, false));
            fs.Close();

            ////BackGround
            //fs = System.IO.File.OpenRead("Resources\\Images\\Backgrounds\\Back_Bis.png");
            //this.BackgroundImage = Image.FromStream(fs, false, false);
            //fs.Close();            

            // BackGround
            fs = System.IO.File.OpenRead("Resources\\Images\\Backgrounds\\Back_Bis.png");
            var btmp = Image.FromStream(fs, false, false);
            fs.Close();
            bmBackgoundBase = new Bitmap(btmp, btmp.Width, btmp.Height); 

        }

        private void InitButtonImage()
        {
            picChannelSettingButton.Image = imageList1[0];
            picChannelSettingButton.Tag = 0;

            picTagSettingButton.Image = imageList1[3];
            picTagSettingButton.Tag = 3;

            picRelationButton.Image = imageList1[6];
            picRelationButton.Tag = 6;

            picSensorLocationButton.Image = imageList1[9];
            picSensorLocationButton.Tag = 9;

            picSensorPositiontButton.Image = imageList1[12];
            picSensorPositiontButton.Tag = 12;

        }

        private void InitLanguageButtonImage()
        {
            picChanese.Image = imageList1[15];
            picChanese.Tag = 15;

            picEnglish.Image = imageList1[17];
            picEnglish.Tag = 17;

            picJapanese.Image = imageList1[19];
            picJapanese.Tag = 19;
        }

        private void pic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;

            if (pic.Tag == null) return;
            int pos = (int)pic.Tag;

            if (pos % 3 != 0) return;

            pos++;
            pic.Image = imageList1[pos];

            pic.Tag = pos;

        }

        private void pic_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;

            if (pic.Tag == null) return;

            int pos = (int)pic.Tag;

            if (pos % 3 != 1) return;

            pos--;
            pic.Image = imageList1[pos];

            pic.Tag = pos;

        }

        private void picButton_Click(object sender, EventArgs e)
        {

            PictureBox pic = (PictureBox)sender;

            SetEnabled(false);
            
            //設置画面の場合通信可能確認
            if (pic.Name == "picSensorPositiontButton" && !Sequences.CommunicationMonitor.GetInstance().IsCanMeasure && !SystemSetting.SystemConfig.IsSimulationMode)
            {
                MessageBox.Show(AppResource.GetString("MSG_DONT_SETUP_MEAS"));
                SetEnabled(true);
                return;

            }

            InitButtonImage();

            if (pic.Tag != null)
            {
                int pos = (int)pic.Tag;

                pos += 2;
                pic.Image = imageList1[pos];

                pic.Tag = pos;
            }
            
            Form frm = null;

            switch (pic.Name)
            {
                case "picSensorLocationButton":
                    frm = new frmLocationSetting(this.log);
                    break;
                case "picChannelSettingButton":
                    frm = new frmChannelSetting(this.log);
                    frm.TopLevel = true;
                    break;
                case "picTagSettingButton":
                    frm = new frmTagSetting(this.log);
                    break;
                case "picRelationButton":
                    frm = new frmTagChannelRelation(this.log);
                    break;
                case "picSensorPositiontButton":
                    frm = new frmPositionSetting();
                    break;
            }

            if (frm != null)
            {
                frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                frm.BringToFront();

                frm.Show(this);
            }
        }

        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            InitButtonImage();

            SetEnabled(true);

        }

        /// <summary>
        /// 画面の有効無効設定
        /// </summary>
        /// <param name="value"></param>
        private void SetEnabled(bool value)
        {
            this.Enabled = value;

            this.Parent.Parent.Enabled = value;

        }

        private void picLanguageButton_Click(object sender, EventArgs e)
        {

            InitLanguageButtonImage();

            PictureBox pic = (PictureBox)sender;
            if (pic.Tag != null)
            {
                int pos = (int)pic.Tag;

                pos += 1;
                pic.Image = imageList1[pos];

                pic.Tag = pos;
            }

            switch (pic.Name)
            {
                case "picJapanese":
                    if (systemconfig.SystemLanguage != DataCommon.CommonResource.LANGUAGE.Japanese)
                    {
                        systemconfig.SwitchLanguage(DataCommon.CommonResource.LANGUAGE.Japanese);
                        systemconfig.SaveXmlFile();                        
                    }
                    break;
                case "picChanese":

                    if (systemconfig.SystemLanguage != DataCommon.CommonResource.LANGUAGE.Chinese)
                    {
                        systemconfig.SwitchLanguage(DataCommon.CommonResource.LANGUAGE.Chinese);
                        systemconfig.SaveXmlFile();
                    }
                    break;
                case "picEnglish":

                    if (systemconfig.SystemLanguage != DataCommon.CommonResource.LANGUAGE.English)
                    {
                        systemconfig.SwitchLanguage(DataCommon.CommonResource.LANGUAGE.English);
                        systemconfig.SaveXmlFile();
                    }
                    break;
            }

            //変更イベント
            LanguageChanged(this, new EventArgs());
        
        }

        private void frmSettingMenu_Load(object sender, EventArgs e)
        {
            systemconfig = new SystemConfig();
            systemconfig.LoadXmlFile();

            switch (systemconfig.SystemLanguage)
            {
                case DataCommon.CommonResource.LANGUAGE.Chinese:
                    picLanguageButton_Click(picChanese,new EventArgs());
                    break;
                case DataCommon.CommonResource.LANGUAGE.Japanese:
                    picLanguageButton_Click(picJapanese, new EventArgs());
                    break;
                case DataCommon.CommonResource.LANGUAGE.English:
                    picLanguageButton_Click(picEnglish, new EventArgs());
                    break;
            }
        }
        /// <summary>
        /// frmSettingMenu_Resize for set new background size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettingMenu_Resize(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized) return;

            //if (this.bmBackgound != null)
            //{
            //    this.bmBackgound.Dispose();
            //    this.bmBackgound = null;
            //}

            //if (this.bmBackgoundBase != null && this.ClientRectangle.Width != 0 && this.ClientRectangle.Height != 0)
            //{
            //    this.bmBackgound = new Bitmap(this.bmBackgoundBase, this.ClientRectangle.Width, this.ClientRectangle.Height);
            //}

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.WindowState != FormWindowState.Minimized && this.bmBackgoundBase != null && this.ClientRectangle.Width != 0 && this.ClientRectangle.Height != 0)
                e.Graphics.DrawImage(this.bmBackgoundBase, this.ClientRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // 何もしない
            // base.OnPaintBackground(pevent);
        }

        void frmSettingMenu_LanguageChanged(object sender, EventArgs e)
        {
            lblChannelSetting.Text = AppResource.GetString("TXT_CHANNEL_SETTING");
            lblSensorLocationSetting.Text = AppResource.GetString("TXT_SETTINGMAIN_LOCATIONSETTING");
            lblSensorPositionSetting.Text = AppResource.GetString("TXT_SETTINGMAIN_POSITIONSETTING");
            lblTagSetting.Text = AppResource.GetString("TXT_SETTINGMAIN_TAGSETTING");
            lblRelationSetting.Text = AppResource.GetString("TXT_SETTINGMAIN_RELATIONSETTING");
            lblLanguageSetting.Text = AppResource.GetString("TXT_SETTING_LANGUAGE");
        }
    }
}
