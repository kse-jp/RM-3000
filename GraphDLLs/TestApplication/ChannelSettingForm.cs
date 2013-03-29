using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace TestApplication
{
    public partial class ChannelSettingForm : Form
    {
        private struct ColorProperties
        {
            public string ColorName;
            public string ColorValue;

            public string DisplayColorName
            {
                get
                {
                    return ColorName;
                }
                set
                {
                    ColorName = value;
                }
            }
            public string DisplayColorValue
            {
                get
                {
                    return ColorValue;
                }
                set
                {
                    ColorValue = value;
                }
            }
        }

        private List<GraphLib.ChannelInfo> _ChannelInfo;

        public List<GraphLib.ChannelInfo> ChannelInfo
        {
            get
            {
                return _ChannelInfo;
            }
            set
            {
                _ChannelInfo = value;
            }
        }

        public ChannelSettingForm()
        {
            InitializeComponent();
        }

        private void ChannelSettingForm_Load(object sender, EventArgs e)
        {
            label1.Text = global::TestApplication.Properties.Resources.LabelChannel;
            label32.Text = global::TestApplication.Properties.Resources.Color;
            label6.Text = global::TestApplication.Properties.Resources.LineSize;
            chkEnabled.Text = global::TestApplication.Properties.Resources.Enabled;
            button4.Text = global::TestApplication.Properties.Resources.OK;
            button1.Text = global::TestApplication.Properties.Resources.Cancel;

            PopulateDropDown();
            cmbChannelList.Items.Clear();
            cmbChannelList.DisplayMember = "DisplayCHName";
            cmbChannelList.ValueMember = "DisplayCHNumber";

            for (int i = 0; i < _ChannelInfo.Count; i++)
            {
                cmbChannelList.Items.Add(_ChannelInfo[i]);
            }

            if (cmbChannelList.Items.Count > 0)
                cmbChannelList.SelectedIndex = 0;
        }

        public void PopulateDropDown()
        {
            // Get the type of instance
            cmbColorList.DisplayMember = "DisplayColorName";
            cmbColorList.ValueMember = "DisplayColorValue";
            cmbColorList.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColorList.DrawMode = DrawMode.OwnerDrawFixed;
            Type t = typeof(System.Windows.Media.Colors);
            List<ColorProperties> colorlist = new List<ColorProperties>();
            foreach (PropertyInfo p1 in t.GetProperties())
            {

                System.Windows.Media.ColorConverter d = new System.Windows.Media.ColorConverter();
                try
                {
                    ColorProperties colorprop = new ColorProperties();

                    colorprop.ColorValue = d.ConvertFromInvariantString(p1.Name).ToString();
                    colorprop.ColorName = p1.Name;



                    // Add Items in DropDownList
                    ///cmbColorList.Items.Add(abc);                       
                    colorlist.Add(colorprop);


                }
                catch
                {
                    // Catch exceptions here
                }
            }
            cmbColorList.DataSource = colorlist;
        }

        private void cmbChannelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void cmbColorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetData();
        }

        private void GetData()
        {
            System.Windows.Media.ColorConverter d = new System.Windows.Media.ColorConverter();
            GraphLib.ChannelInfo channel = (GraphLib.ChannelInfo)cmbChannelList.SelectedItem;
            txtLineSize.Text = channel.CHLineSize.ToString();
            chkEnabled.Checked = channel.IsEnabled;
            cmbColorList.SelectedValue = d.ConvertToInvariantString(channel.CHColor);
        }

        private void SetData()
        {
            if (cmbChannelList.SelectedItem != null)
            {
                System.Windows.Media.ColorConverter d = new System.Windows.Media.ColorConverter();
                GraphLib.ChannelInfo channel = (GraphLib.ChannelInfo)cmbChannelList.SelectedItem;
                channel.CHLineSize = Convert.ToDouble(txtLineSize.Text);
                channel.IsEnabled = chkEnabled.Checked;
                channel.CHColor = (System.Windows.Media.Color)d.ConvertFromInvariantString(cmbColorList.SelectedValue.ToString());
                cmbChannelList.Items[cmbChannelList.SelectedIndex] = channel;
            }
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            SetData();
        }

        private void txtLineSize_TextChanged(object sender, EventArgs e)
        {
            SetData();
        }

        private void ChannelSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ChannelInfo.Clear();
            for (int i = 0; i < cmbChannelList.Items.Count; i++)
            {
                _ChannelInfo.Add((GraphLib.ChannelInfo)cmbChannelList.Items[i]);
            }
        }

        private void cmbColorList_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index == -1)
                return;

            // Get the name of the current item to be drawn, and make a brush of it
            ColorProperties colorprop = (ColorProperties)this.cmbColorList.Items[e.Index];
            SolidBrush b = new SolidBrush(Color.FromName(colorprop.ColorName));
            // Draw a rectangle and fill it with the current color
            // and add the name to the right of the color
            e.Graphics.DrawRectangle(Pens.Black, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.FillRectangle(b, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
            //e.Graphics.FillRectangle(b, 3, e.Bounds.Top + 2, 19, 10);
            e.Graphics.DrawString(colorprop.ColorName, this.Font, Brushes.Black, 0, e.Bounds.Top);
            b.Dispose();

        }


    }
}
