//
// NumericTextbox.cs
//
// $Id$
//
// Definition of the NumericTextBox class
//
// Copyright (c) 2009
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Win32;

namespace RM_3000.Controls
{
    /// <summary>
    /// numberic input textbox
    /// </summary>
    public class NumericTextBox : TextBox
    {
        #region private properties
        /// <summary>
        /// copy key
        /// </summary>
        private const int WM_COPY = 0x301;
        /// <summary>
        /// cut key
        /// </summary>
        private const int WM_CUT = 0x300;
        /// <summary>
        /// paste key
        /// </summary>
        private const int WM_PASTE = 0x302;
        /// <summary>
        /// clear key
        /// </summary>
        private const int WM_CLEAR = 0x303;
        /// <summary>
        /// undo key
        /// </summary>
        private const int WM_UNDO = 0x304;
        /// <summary>
        /// undo something
        /// </summary>
        private const int EM_UNDO = 0xC7;
        /// <summary>
        /// Can undo menu available 
        /// </summary>
        private const int EM_CANUNDO = 0xC6;

        /// <summary>
        /// allow space key in textbox
        /// </summary>
        private bool _AllowSpace = false;
        /// <summary>
        /// set available input value to Integer or float
        /// </summary>
        private bool _IsInteger = true;
        /// <summary>
        /// Allow input minus value
        /// </summary>
        private bool _AllowMinus = true;
        /// <summary>
        /// Allow input string value
        /// </summary>
        private bool _AllowString = false;
        #endregion

        #region public properties
        /// <summary>
        /// Allow input space
        /// </summary>
        public bool AllowSpace
        {
            set { this._AllowSpace = value; }
            get { return this._AllowSpace; }
        }
        /// <summary>
        /// set available input value to Integer or float
        /// </summary>
        public bool IsInteger
        {
            set { _IsInteger = value; }
            get { return _IsInteger; }
        }
        /// <summary>
        /// set available input minus value
        /// </summary>
        public bool AllowMinus
        {
            set { _AllowMinus = value; }
            get { return _AllowMinus; }
        }
        /// <summary>
        /// set available input string
        /// </summary>
        public bool AllowString
        {
            set { _AllowString = value; }
            get { return _AllowString; }
        }

        #endregion

        #region public events
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public NumericTextBox()
        {
            this.MaxLength = 10;
        }

        #endregion

        #region private method
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NumericTextBox
            // 
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ResumeLayout(false);
        }
        /// <summary>
        /// Check valid input number
        /// </summary>
        /// <returns></returns>
        private bool CheckValidNumber(string inputCharacter)
        {
            try
            {
                if (inputCharacter.Equals("-") && this.Text.Contains(inputCharacter))
                {
                    return false;
                }
                //
                if (_IsInteger)
                {
                    if (_AllowMinus)
                    {
                        if (Regex.Match(this.Text, "^([-]|[0-9])[0-9]*$").Value != this.Text)
                        {
                            return false;
                        }
                    }
                    else if (Regex.Match(this.Text, "^[0-9]").Value != this.Text)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_AllowMinus)
                    {
                        string IntegerPattern = "^([-]|[0-9])[0-9]*$";
                        string realPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
                        if (Regex.Match(this.Text, "(" + IntegerPattern + ")|(" + realPattern + ")").Value != this.Text)
                        {
                            return false;
                        }
                    }
                    else if (Regex.Match(this.Text, "^[.][0-9]+[0-9]*[.]*[0-9]").Value != this.Text)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
                return false;
            }
            
            return true;
        }
       /// <summary>
       /// key down event
       /// </summary>
       /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                if (_AllowString)
                {
                    e.Handled = false;
                    return;
                }
                // bypass other keys!  
                if (IsValidOtherKey(e.KeyCode))
                {
                    if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                        || (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z))
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = e.Handled;
                        return;
                    }
                    else if ((e.KeyCode < Keys.D0 && e.KeyCode != Keys.Space)
                    || (e.KeyCode > Keys.Z && e.KeyCode < Keys.NumPad0))
                    {
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = e.Handled;
                    }
                    return;
                }
                TextBox textBox = this; // sender as TextBox;

                {

                    if (_IsInteger && !_AllowMinus)
                    {
                        e.Handled = !IsValidIntegerKey(textBox, e.KeyCode, e.KeyValue, false);
                    }
                    else if (_IsInteger && _AllowMinus)
                    {
                        e.Handled = !IsValidIntegerKey(textBox, e.KeyCode, e.KeyValue, true);
                    }
                    else if (!_IsInteger && !_AllowMinus)
                    {
                        e.Handled = !IsValidDecmialKey(textBox, e.KeyCode, e.KeyValue, false);
                    }
                    else if (!_IsInteger && _AllowMinus)
                    {
                        e.Handled = !IsValidDecmialKey(textBox, e.KeyCode, e.KeyValue, true);
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    e.SuppressKeyPress = e.Handled;
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
        }
        /// <summary>
        /// Determines whether the specified key is valid other key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool IsValidOtherKey(Keys key)
        {
            // allow control keys
            if ((ModifierKeys & Keys.Control) != 0 || (ModifierKeys & Keys.Shift) != 0 || (ModifierKeys & Keys.Alt) != 0)
            {
                return true;
            }
            
            // allow
            // Back, Tab, Enter, Shift, Ctrl, Alt, CapsLock, Escape, PageUp, PageDown
            // End, Home, Left, Up, Right, Down, Insert, Delete 
            // except for space!
            // allow all Fx keys
            if (
                (key < Keys.D0 && key != Keys.Space)
                || (key > Keys.Z && key < Keys.NumPad0))
            {
                return true;
            }
            
            return false;
        }
        /// <summary>
        /// Determines whether the specified key is valid integer key for the specified text box. 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="key"></param>
        /// <param name="platformKeyCode"></param>
        /// <param name="negativeAllowed"></param>
        /// <returns></returns>
        private static bool IsValidIntegerKey(TextBox textBox, Keys key, int platformKeyCode, bool negativeAllowed)
        {
            try
            {
                if ((ModifierKeys & Keys.Shift) != 0 || (ModifierKeys & Keys.Control) != 0 || (ModifierKeys & Keys.Alt) != 0)
                {
                    return false;
                }
                if (Keys.D0 <= key && key <= Keys.D9)
                {
                    return true;
                }
                if (Keys.NumPad0 <= key && key <= Keys.NumPad9)
                {
                    return true;
                }
                //
                if (negativeAllowed && (key == Keys.Subtract || key == Keys.OemMinus))
                {
                    if (textBox.SelectionStart > 0)
                    {
                        return 0 == textBox.Text.Length;
                    }
                    else
                    {
                        if (textBox.Text.Contains("-"))
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
            return false;
        }
        /// <summary>
        /// Determines whether the specified key is valid decmial key for the specified text box.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="key"></param>
        /// <param name="platformKeyCode"></param>
        /// <param name="negativeAllowed"></param>
        /// <returns></returns>
        private static bool IsValidDecmialKey(TextBox textBox, Keys key, int platformKeyCode, bool negativeAllowed)
        {
            try
            {
                if (IsValidIntegerKey(textBox, key, platformKeyCode, negativeAllowed))
                {
                    return true;
                }
                if (key == Keys.Decimal || key == Keys.OemPeriod)
                {
                    return !textBox.Text.Contains(".");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
            return false;
        }
        /// <summary>
        /// Determines whether the specified key is valid alpha key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool IsValidAlphaKey(Keys key)
        {
            if (Keys.A <= key && key <= Keys.Z)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Capture command keys
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        return true;
                    case Keys.Control | Keys.V:
                    case Keys.Shift | Keys.Insert:
                        if (Clipboard.ContainsText())
                        {
                            //check valid clipboard data
                            string tempData = Clipboard.GetText();
                            if (CheckValidClipboardText(tempData))
                            {
                                //@COMMENT_OUT
                                //2010.11.24,Chen, fixed Ctrl+V shortcut
                                /*
                                base.ProcessCmdKey(ref msg, keyData);
                                 */
                                for (int k = 0; k < tempData.Length; k++) // cannot use SendKeys.Send
                                {
                                    SendCharKey(tempData[k]);
                                }
                                return true;
                            }
                        }
                        return true;
                    case Keys.Control | Keys.C:
                    case Keys.Control | Keys.Insert:
                        return true;
                    case Keys.Control | Keys.X:
                    case Keys.Shift | Keys.Delete:
                        return true;
                    case Keys.Control | Keys.Delete:
                        return true;
                    case Keys.Control | Keys.Z:
                        return true;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SendCharKey(char c)
        {
            int WM_CHAR = 0x0102;
            Message msg = new Message();

            msg.HWnd = this.Handle;
            msg.Msg = WM_CHAR;
            msg.WParam = (IntPtr)c;
            msg.LParam = IntPtr.Zero;

            base.WndProc(ref msg);
        }
        /// <summary>
        /// Windows process message
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case WM_COPY:
                        break;
                    case WM_PASTE:
                        if (Clipboard.ContainsText())
                        {
                            //check valid clipboard data
                            string tempData = Clipboard.GetText();
                            if (CheckValidClipboardText(tempData))
                            {
                                base.WndProc(ref m);
                            }
                        }
                        break;
                    case WM_CLEAR:
                        base.WndProc(ref m);
                        break;
                    case WM_CUT:
                        Clipboard.SetDataObject(base.SelectedText);
                        break;
                    //TODO: ova ne ke bide loso da se napravi
                    case WM_UNDO:
                    case EM_UNDO:
                        m.Result = new IntPtr(1);
                        break;
                    case EM_CANUNDO:
                        m.Result = new IntPtr(0);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
        }
        /// <summary>
        /// Check clipboard text data
        /// </summary>
        /// <param name="clipBoardText"></param>
        /// <returns></returns>
        private bool CheckValidClipboardText(string clipBoardText)
        {
            try
            {
                //
                string tempData = clipBoardText;
                if (_AllowString)
                {
                    return true;
                }
                if (_IsInteger)
                {
                    if (_AllowMinus)
                    {
                        if (Regex.Match(tempData, "^([-]|[0-9])[0-9]*$").Value != tempData)
                        {
                            return false;
                        }
                    }
                    else if (Regex.Match(tempData, "^[0-9]*$").Value != tempData)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (_AllowMinus)
                    {
                        string IntegerPattern = "^([-]|[0-9])[0-9]*$";
                        string realPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
                        if (Regex.Match(tempData, "(" + IntegerPattern + ")|(" + realPattern + ")").Value != tempData)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (Regex.Match(tempData, "^[.][0-9]+[0-9]*[.]*[0-9]").Value != tempData)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}{1}{2}", ex.Message, ":\n", ex.StackTrace));
            }
            return true;
        }
        #endregion

        #region public method
        /// <summary>
        /// Get integer value from Numeric textbox
        /// </summary>
        /// <returns>int value</returns>
        public int GetIntValue()
        {
            return Int32.Parse(this.Text);
        }
        /// <summary>
        /// Get double value from Numeric textbox
        /// </summary>
        /// <returns>double</returns>
        public double GetDoubleValue()
        {
            return double.Parse(this.Text);
        }
        #endregion

        #region interface
        #endregion
    }

}
