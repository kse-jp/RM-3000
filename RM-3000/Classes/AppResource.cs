using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Collections;

namespace RM_3000
{
    /// <summary>
    /// utility class for set text string from resource file
    /// </summary>
    public class AppResource
    {
        #region private member
        private static System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
        //private static System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
        //private static System.Resources.ResourceManager rm = new System.Resources.ResourceManager(asm.GetName().Name + ".Properties.Resources", asm);
        private ArrayList sControl;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AppResource()
        {
            
        }
        #endregion

        #region private method
        private void EnableControls2(Control parent, bool bEnable)
        {
            foreach (Control c in parent.Controls)
            {
                switch (c.GetType().Name.ToLower())
                {
                    case "radiobutton":
                    case "checkbox":
                    case "linklabel":
                    case "button":
                    case "textbox":
                    case "datagridview":
                    case "exdatagridview":
                    case "combobox":
                        if (bEnable)
                        {
                            foreach (object o in sControl)
                            {
                                if (o.ToString() == c.Name)
                                {
                                    c.Enabled = bEnable;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (c.Enabled)
                            {
                                c.Enabled = bEnable;
                                sControl.Add(c.Name);
                            }
                        }
                        break;
                    case "groupbox":
                    case "tabpage":
                    case "tabcontrol":
                    case "panel":
                    case "tablelayoutpanel":
                        EnableControls2(c, bEnable);
                        break;
                }
            }
        }
        #endregion 

        #region public method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String GetString(String id)
        {
            try
            {
                if (id.Length == 0) { return ""; }
                String s = rm.GetString(id);
                return ((s == null ? id : s));
            }
            catch
            {
                return id;
            }

        }

        public static void SetControlsText(Control parent)
        {
            Control c;
            string s;
            parent.Text = GetString(parent.Text);
            for (int i = 0; i <= parent.Controls.Count - 1; i++)
            {
                c = parent.Controls[i];
                switch (c.GetType().Name.ToLower())
                {
                    case "radiobutton":
                    case "checkbox":
                    case "linklabel":
                    case "button":
                    case "label":
                    case "toolstripdropdownbutton":
                    case "autofontsizelabel":
                        c.Text = GetString(c.Text);
                        break;
                    case "groupbox":
                    case "tabpage":
                    case "tabcontrol":
                    case "panel":
                    case "tablelayoutpanel":
                        SetControlsText(c);
                        break;
                    case "toolstrip":
                        for (int i2 = 0; i2 < ((ToolStrip)c).Items.Count; i2++)
                        {
                            s = ((ToolStrip)c).Items[i2].Text;
                            ((ToolStrip)c).Items[i2].Text = GetString(s);
                        }
                        break;
                    case "datagridview":
                    case "customdatagridview":
                    case "exdatagridview":
                        DataGridViewColumn itm;
                        for (int i2 = 0; i2 < ((DataGridView)c).Columns.Count; i2++)
                        {
                            itm = ((DataGridView)c).Columns[i2];
                            itm.HeaderText = GetString(itm.HeaderText);
                        }
                        break;
                    case "combobox":
                        if (((ComboBox)c).Items.Count == 1)
                        {
                            s = ((ComboBox)c).Items[0].ToString();
                            s = GetString(s);
                            ((ComboBox)c).Items.Clear();
                            ((ComboBox)c).Items.AddRange(s.Split(",".ToCharArray()));
                        }
                        break;
                    case "listview":
                        ColumnHeader itm2;
                        for (int i3 = 0; i3 < ((ListView)c).Columns.Count; i3++)
                        {
                            itm2 = ((ListView)c).Columns[i3];
                            itm2.Text = GetString(itm2.Text);
                        }
                        break;
                }
            }
        }
        public void EnableControls(Control parent, bool bEnable)
        {
            if (bEnable)
            {
                if (sControl == null || sControl.Count == 0)
                {
                    return;
                }
            }
            else
            {
                sControl = new ArrayList();
            }

            EnableControls2(parent, bEnable);

            if (bEnable)
            {
                sControl.Clear();
            }
        }

        
        #endregion
    }
}
