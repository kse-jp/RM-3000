using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RM_3000.Classes
{
    /// <summary>
    /// DataGridViewMaskedTextBoxCellオブジェクトの列
    /// </summary>
    public class DataGridViewColorTextBoxColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewColorTextBoxColumn()
            : base()
        {
            this.CellTemplate = new DataGridViewColorTextBoxCell();
        }

        // CellTemplateの取得と設定
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // DataGridViewColorTextBoxCellしかCellTemplateに設定できないようにする
                if (!(value is DataGridViewColorTextBoxCell))
                {
                    throw new InvalidCastException("DataGridViewColorTextBoxCellオブジェクトを指定してください。");
                }
                base.CellTemplate = value;
            }
        }
    }

    /// <summary>
    /// 設定色をDataGridViewに表示する
    /// </summary>
    public class DataGridViewColorTextBoxCell : DataGridViewTextBoxCell
    {
        public DataGridViewColorTextBoxCell()
        {
        }

        public Color Color
        {
            get
            {
                if (this.Value.ToString().Length > 0)
                    return Color.FromName(this.Value.ToString());
                else
                    return Color.Black;
            }
            set
            {
                this.Value = value.Name;
            }
        }
        /// <summary>
        /// 無効な色かどうか
        /// </summary>
        public bool IsInvalidColor
        { 
            get { return (this.Color.A == 0 && this.Color.R == 0 && this.Color.G == 0 && this.Color.B == 0); }
        }
        /// <summary>
        /// セルの値のデータ型を指定する
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(object); }
        }
        /// <summary>
        /// 新しいレコード行のセルの既定値を指定する
        /// </summary>
        public override object DefaultNewRowValue
        {
            get { return base.DefaultNewRowValue; }
        }
        /// <summary>
        /// 描画メソッドのオーバーライド
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellState"></param>
        /// <param name="value"></param>
        /// <param name="formattedValue"></param>
        /// <param name="errorText"></param>
        /// <param name="cellStyle"></param>
        /// <param name="advancedBorderStyle"></param>
        /// <param name="paintParts"></param>
        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates cellState,
            object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // セルの境界線（枠）を描画する
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                this.PaintBorder(graphics, clipBounds, cellBounds,
                    cellStyle, advancedBorderStyle);
            }

            // 境界線の内側に範囲を取得する
            var borderRect = this.BorderWidths(advancedBorderStyle);
            var paintRect = new Rectangle(
                cellBounds.Left + borderRect.Left,
                cellBounds.Top + borderRect.Top,
                cellBounds.Width - borderRect.Right,
                cellBounds.Height - borderRect.Bottom);

            // 背景色を決定する
            // 選択されている時とされていない時で色を変える
            var isSelected = (cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            Color bkColor;
            if (isSelected && (paintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground)
            {
                bkColor = cellStyle.SelectionBackColor;
            }
            else
            {
                bkColor = cellStyle.BackColor;
            }
            // 背景を描画する
            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                using (var backBrush = new SolidBrush(bkColor))
                {
                    graphics.FillRectangle(backBrush, paintRect);
                }
            }

            // Paddingを差し引く
            paintRect.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
            paintRect.Width -= cellStyle.Padding.Horizontal;
            paintRect.Height -= cellStyle.Padding.Vertical;

            // 選択されている色と色名を描画する
            if ((paintParts & DataGridViewPaintParts.ContentForeground) ==
                DataGridViewPaintParts.ContentForeground)
            {
                if (this.Value != null && !string.IsNullOrWhiteSpace(this.Value.ToString()) && !this.IsInvalidColor)
                {
                    using (var b = new SolidBrush(this.Color))
                    {
                        // Draw a rectangle and fill it with the current color
                        // and add the name to the right of the color
                        graphics.DrawRectangle(Pens.Black, paintRect.Left + 5, paintRect.Top + 1, 20, paintRect.Height - 5);
                        graphics.FillRectangle(b, paintRect.Left + 6, paintRect.Top + 2, 19, paintRect.Height - 6);
                        graphics.DrawString(this.Value.ToString(), cellStyle.Font, Brushes.Black, paintRect.Left + 28, paintRect.Top);
                    }
                }
            }

            // フォーカスの枠を表示する
            if (this.DataGridView.CurrentCellAddress.X == this.ColumnIndex && this.DataGridView.CurrentCellAddress.Y == this.RowIndex &&
                (paintParts & DataGridViewPaintParts.Focus) == DataGridViewPaintParts.Focus && this.DataGridView.Focused)
            {
                // フォーカス枠の大きさを適当に決める
                var focusRect = paintRect;
                focusRect.Inflate(-1, -1);
                ControlPaint.DrawFocusRectangle(graphics, focusRect);
            }

            // エラーアイコンの表示
            if ((paintParts & DataGridViewPaintParts.ErrorIcon) == DataGridViewPaintParts.ErrorIcon &&
                this.DataGridView.ShowCellErrors && !string.IsNullOrEmpty(errorText))
            {
                // エラーアイコンを表示させる領域を取得
                var iconBounds = this.GetErrorIconBounds(graphics, cellStyle, rowIndex);
                iconBounds.Offset(cellBounds.X, cellBounds.Y);
                // エラーアイコンを描画
                this.PaintErrorIcon(graphics, iconBounds, cellBounds, errorText);
            }
        
        
        }
    }
    

}
