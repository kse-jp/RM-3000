using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Graph3DLib
{
    /// <summary>
    /// class for input numberic in Textbox
    /// </summary>
    public static class NumericTextBox
    {
        #region Private Function
        /// <summary>
        /// check is numberic
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isInteger"></param>
        /// <returns></returns>
        private static Boolean IsNumberic(String text, bool isInteger)
        {
            try
            {
                if (!isInteger)
                {
                    double doublevalue = 0;
                    if (double.TryParse(text, out doublevalue))
                        return true;
                    else
                    {
                        if (text == "." || text == "-")
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    int intvalue = 0;
                    if (Int32.TryParse(text, out intvalue))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// event check textbox preview input 
        /// </summary>
        /// <param name="textBoxObj"></param>
        /// <param name="inpText"></param>
        /// <param name="isInteger"></param>
        /// <returns></returns>
        public static bool PreviewInput(object textBoxObj, string inpText, bool isInteger)
        {
            try
            {
                bool isnumberic = false;

                if (IsNumberic(inpText, isInteger))
                {
                    TextBox textbox = textBoxObj as TextBox;
                    if (textbox == null)
                        return false;

                    int ind = 0;
                    if (inpText == ".")
                    {
                        ind = textbox.Text.IndexOf(".", 0);
                        if (ind == -1)
                        {
                            int ind2 = textbox.Text.IndexOf("-", 0);
                            if (ind2 != -1)
                            {
                                if (textbox.SelectionStart == 1)
                                {
                                    textbox.Text = textbox.Text.Insert(1, "0" + inpText);
                                }
                                else if (textbox.SelectionStart != 0)
                                    isnumberic = true;

                            }
                            else
                                isnumberic = true;
                        }
                    }
                    else if (inpText == "-")
                    {
                        ind = textbox.Text.IndexOf("-", 0);
                        if (ind == -1)
                        {
                            textbox.Text = inpText + textbox.Text;
                        }
                    }
                    else
                    {
                        ind = textbox.Text.IndexOf("-", 0);
                        if (ind != -1)
                        {
                            int sel = textbox.SelectionStart;
                            textbox.Text = textbox.Text.Remove(ind, 1);
                            textbox.Text = "-" + textbox.Text;
                            if (sel + 1 > textbox.Text.Length - 1)
                            {
                                textbox.Text = textbox.Text + inpText;
                                textbox.SelectionStart = textbox.Text.Length - 1;
                            }
                            else
                            {
                                textbox.Text = textbox.Text.Insert(sel + 1, inpText);
                                textbox.SelectionStart = sel + 1;
                            }
                        }
                        else
                            isnumberic = true;
                    }

                }
                return isnumberic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// event check textbox change
        /// </summary>
        /// <param name="textBoxObj"></param>
        /// <param name="isInteger"></param>
        public static void Changed(object textBoxObj, bool isInteger)
        {
            try
            {
                TextBox textbox = textBoxObj as TextBox;
                if (textbox != null)
                {
                    if (!isInteger)
                    {
                        double doublevalue = 0;
                        if (!double.TryParse(textbox.Text, out doublevalue))
                            textbox.Text = "0";
                    }
                    else
                    {
                        int intvalue = 0;
                        if (!Int32.TryParse(textbox.Text, out intvalue))
                            textbox.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
