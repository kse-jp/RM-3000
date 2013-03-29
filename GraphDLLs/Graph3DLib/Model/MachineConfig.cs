using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Graph3DLib.Model
{
    /// <summary>
    /// machine config class (read from app.config)
    /// </summary>
    public static class MachineConfig
    {
        #region Public Function
        /// <summary>
        /// read colors string for machine part
        /// </summary>
        /// <returns>string array</returns>
        public static string[] ReadColorStrings()
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"Graph3DLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);
                
                string[] colors = new string[9];
                colors[(int)MachinePart.MachinePlate] = config.AppSettings.Settings["ColorMachinePlate"].Value.ToString();
                colors[(int)MachinePart.LowerPress] = config.AppSettings.Settings["ColorLowerPress"].Value.ToString();
                colors[(int)MachinePart.UpperPress] =config.AppSettings.Settings["ColorUpperPress"].Value.ToString();
                colors[(int)MachinePart.Stripper] = config.AppSettings.Settings["ColorStripper"].Value.ToString();
                colors[(int)MachinePart.Shaft] = config.AppSettings.Settings["ColorShaft"].Value.ToString();
                colors[(int)MachinePart.MachineTopBottom] = config.AppSettings.Settings["ColorMachineTopBottom"].Value.ToString();
                colors[(int)MachinePart.MachinePost] =config.AppSettings.Settings["ColorMachinePost"].Value.ToString();
                colors[(int)MachinePart.MachinePostLine] = config.AppSettings.Settings["ColorMachinePostLine"].Value.ToString();
                colors[(int)MachinePart.Ram] = config.AppSettings.Settings["ColorRam"].Value.ToString();
                

                return colors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     

        /// <summary>
        /// read double value from app.config
        /// </summary>
        /// <param name="valueName">value name in config</param>
        /// <returns>double value</returns>
        public static double ReadDoubleValue(string valueName)
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"Graph3DLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

                string stringval = config.AppSettings.Settings[valueName].Value.ToString();

                double outp = Convert.ToDouble(stringval);
                return outp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// read int value from app.config
        /// </summary>
        /// <param name="valueName">value name</param>
        /// <returns>int value</returns>
        public static int ReadIntValue(string valueName)
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"Graph3DLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

                string stringval = config.AppSettings.Settings[valueName].Value.ToString();         

                int outp = Convert.ToInt32(stringval);
                return outp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// read color value from app.config
        /// </summary>
        /// <param name="valueName">value name</param>
        /// <returns>color value</returns>
        public static Color ReadColorValue(string valueName)
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"Graph3DLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

                string stringval = config.AppSettings.Settings[valueName].Value.ToString();         
                
                Color outp= (Color)ColorConverter.ConvertFromString(stringval);
                return outp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// read string value from app.config
        /// </summary>
        /// <param name="valueName">value name</param>
        /// <returns>string value</returns>
        public static string ReadStringValue(string valueName)
        {
            try
            {
                ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"Graph3DLib.dll.config" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);

                string stringval = config.AppSettings.Settings[valueName].Value.ToString();                         
                return stringval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   
        #endregion
    }
}
