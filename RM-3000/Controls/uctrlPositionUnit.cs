using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000.Controls
{
    public partial class uctrlPositionUnit : UserControl
    {


        #region Public Property

        [Category("Appearance")] 
        public int ChannelNo 
        { 
            get { return _ChannelNo; }
            set 
            { 
                _ChannelNo = value;
                lblName.Text = string.Format("Ch {0}:タグ {0}", _ChannelNo);
            } 
        }

        public decimal NowValue
        {
            get { 
                
                    if(lblNowValue.Text == string.Empty || 
                        lblNowValue.Text == "----")
                        return decimal.MaxValue;

                    return decimal.Parse(lblNowValue.ToString());
                }
            set
            {
                if (value == decimal.MaxValue)
                    lblNowValue.Text = "----";
                else
                    lblNowValue.Text = value.ToString();

            }
        }


        public decimal ZeroValue
        {
            get
            {

                if (lblZeroValue.Text == string.Empty ||
                    lblZeroValue.Text == "----")
                    return decimal.MaxValue;

                return decimal.Parse(lblZeroValue.ToString());
            }
            set
            {
                if (value == decimal.MaxValue)
                    lblZeroValue.Text = "----";
                else
                    lblZeroValue.Text = value.ToString();
            }
        }


        #endregion

        #region Private Valiables

        private int _ChannelNo = 0;

        #endregion


        public uctrlPositionUnit()
        {
            InitializeComponent();


        }

    }
}
