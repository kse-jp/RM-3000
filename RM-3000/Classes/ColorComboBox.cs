using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RM_3000.Classes
{
    /// <summary>
    /// 色選択コンボボックス
    /// </summary>
    public class ColorComboBox : ComboBox
    {
        /// <summary>
        /// 色リスト
        /// </summary>
        public List<Color> ListColors
        {
            get { return _ListColors; }
            set
            {
                _ListColors = value;
            }
        }
       
        /// <summary>
        /// 色リスト
        /// </summary>
        private List<Color> _ListColors = new List<Color>();

        /// <summary>
        /// 選択色
        /// </summary>
        public Color Color
        {
            get
            {
                if (this.Text.Length > 0)
                    return Color.FromName(this.Text);
                else
                    return Color.Black;
            }
            set
            {
                this.Text = value.Name;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorComboBox()
        {
            this.Text = string.Empty;
            this.SelectedIndex = -1;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.Width = 150;

            //var c = Color.Black;
            //var t = c.GetType();

            //var pis = t.GetProperties();
            //foreach (var p in pis)
            //{
            //    // Filter out all properties that aren't colors and add to the dropdownlist
            //    if (p.PropertyType == typeof(System.Drawing.Color))
            //        this.Items.Add(p.Name);
            //}

        }

        /// <summary>
        /// 設定ListColorsでリストボックスを初期化
        /// </summary>
        public void InitColors()
        {
            this.Items.Clear();

            foreach (Color cl in _ListColors)
            {
                this.Items.Add(cl.Name);
            }

        }

        /// <summary>
        /// 描画イベント
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>The combobox is set to OwnerDrawFixed, so we are responsible to draw all items</remarks>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            var s = (string)this.Items[e.Index];
            using (var b = new SolidBrush(Color.FromName(s)))
            {
                // Draw a rectangle and fill it with the current color
                // and add the name to the right of the color
                e.Graphics.DrawRectangle(Pens.Black, 5, e.Bounds.Top + 1, 20, e.Bounds.Height - 5);
                e.Graphics.FillRectangle(b, 6, e.Bounds.Top + 2, 19, e.Bounds.Height - 6);
                e.Graphics.DrawString(s, this.Font, Brushes.Black, 28, e.Bounds.Top);
            }
        }
    }
}
