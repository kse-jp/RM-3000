using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RM_3000.Forms.Maintenance
{
    public partial class frmSystemMainte : Form
    {
        public frmSystemMainte()
        {
            InitializeComponent();

            pictureBox2.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }
    }
}
