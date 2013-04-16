using System;
using DataCommon;
using CommonLib;

namespace RM_3000
{
    /// <summary>
    /// System Setting
    /// </summary>
    public static class SystemSetting
    {
        #region private member
        
        #endregion

        #region public member
        /// <summary>
        /// ChannelsSetting
        /// </summary>
        public static ChannelsSetting ChannelsSetting = null;
        /// <summary>
        /// ConstantSetting
        /// </summary>
        public static ConstantSetting ConstantSetting = null;
        /// <summary>
        /// DataTagSetting
        /// </summary>
        public static DataTagSetting DataTagSetting = null;
        /// <summary>
        /// MeasureSetting
        /// </summary>
        public static MeasureSetting MeasureSetting = null;
        /// <summary>
        /// SensorPositionSetting
        /// </summary>
        public static SensorPositionSetting PositionSetting = null;
        /// <summary>
        /// TagChannelRelationSetting
        /// </summary>
        public static TagChannelRelationSetting RelationSetting = null;
        /// <summary>
        /// CalibrationTable 
        /// </summary>
        public static CalibrationTableArray CalibrationTables = new CalibrationTableArray();
        /// <summary>
        /// SystemConfig
        /// </summary>
        public static SystemConfig SystemConfig = null;

        /// <summary>
        /// ハード情報構造体
        /// </summary>
        public static HardInfoStruct HardInfoStruct = new HardInfoStruct();

        #endregion

        #region constructor
        
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// load settings from xml files
        /// </summary>
        public static void LoadInstance()
        {
            MeasureSetting = new MeasureSetting();
            string xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + MeasureSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                MeasureSetting = (MeasureSetting)MeasureSetting.Deserialize(xmlFilePath);
                if (MeasureSetting.Mode1_MeasCondition != null)
                {
                    MeasureSetting.Mode1_MeasCondition.IsUpdated = false;
                }
            }
            else
            {
                MeasureSetting = new MeasureSetting() { FilePath = xmlFilePath };
            }

            ChannelsSetting = new ChannelsSetting();
            xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + ChannelsSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                ChannelsSetting = (ChannelsSetting)ChannelsSetting.Deserialize(xmlFilePath);
            }
            else
            {
                ChannelsSetting = new ChannelsSetting() { FilePath = xmlFilePath };
            }

            ConstantSetting = new ConstantSetting();
            xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + ConstantSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                ConstantSetting = (ConstantSetting)ConstantSetting.Deserialize(xmlFilePath);
            }
            else
            {
                ConstantSetting = new ConstantSetting() { FilePath = xmlFilePath };
            }

            DataTagSetting = new DataTagSetting();
            xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + DataTagSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                DataTagSetting = (DataTagSetting)DataTagSetting.Deserialize(xmlFilePath);
            }
            else
            {
                DataTagSetting = new DataTagSetting() { FilePath = xmlFilePath };
            }

            RelationSetting = new TagChannelRelationSetting();
            xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + TagChannelRelationSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                RelationSetting = (TagChannelRelationSetting)TagChannelRelationSetting.Deserialize(xmlFilePath);
            }
            else
            {
                RelationSetting = new TagChannelRelationSetting() { FilePath = xmlFilePath };
            }

            PositionSetting = new SensorPositionSetting();
            xmlFilePath = CommonLib.SystemDirectoryPath.SystemPath + SensorPositionSetting.FileName;
            if (System.IO.File.Exists(xmlFilePath))
            {
                PositionSetting = (SensorPositionSetting)SensorPositionSetting.Deserialize(xmlFilePath);
            }
            else
            {
                PositionSetting = new SensorPositionSetting() { FilePath = xmlFilePath };
            }

            SystemConfig = new SystemConfig();
            SystemConfig.LoadXmlFile();

            HardInfoStruct = new HardInfoStruct();
            HardInfoStruct.LoadXmlFile();


        }
        #endregion
    }
}
