using System;
using System.Xml;
using System.Xml.Serialization;
using DataCommon;

namespace CommonLib
{
    /// <summary>
    /// System setting value
    /// </summary>
    [Serializable]
    public class SystemConfig
    {
        #region private member
        /// <summary>
        /// Sensor High Value of B Sensor
        /// </summary>
        private decimal sensorHighValue_B = 10000;

        /// <summary>
        /// Sensor High Value of R Sensor
        /// </summary>
        private decimal sensorHighValue_R = 10000;

        /// <summary>
        /// 3D Value Rate of R Sensor
        /// </summary>
        private decimal valueRate_3D_R = 1; 

        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SystemConfig()
        {
            this.SystemLanguage = CommonResource.LANGUAGE.Japanese;
        }
        #endregion

        #region public member
        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "SystemConfig.xml";
        /// <summary>
        /// current system language
        /// </summary>
        public CommonResource.LANGUAGE SystemLanguage { set; get; }
        /// <summary>
        /// シミュレーションモード
        /// </summary>
        public bool IsSimulationMode { set; get; }
        /// <summary>
        /// デバッグモード
        /// </summary>
        public bool IsDebugMode { set; get; }
        /// <summary>
        /// max small Axis
        /// </summary>
        [System.ComponentModel.DefaultValue(20)]
        public decimal MaxSmallAxis { set; get; }
        /// <summary>
        /// Sensor High Value of B Sensor
        /// </summary>
        [System.ComponentModel.DefaultValue(10000)]
        public decimal SensorHighValue_B { set { this.sensorHighValue_B = value; } get { return this.sensorHighValue_B; } }
        /// <summary>
        /// Sensor High Value of R Sensor
        /// </summary>
        [System.ComponentModel.DefaultValue(10000)]
        public decimal SensorHighValue_R { set { this.sensorHighValue_R = value; } get { return this.sensorHighValue_R; } }
        /// <summary>
        /// 3D Value Rate of R Sensor
        /// </summary>
        [System.ComponentModel.DefaultValue(1)]
        public decimal ValueRate_3D_R { set { this.valueRate_3D_R = value; } get { return this.valueRate_3D_R; } }
        /// <summary>
        /// NumPointLimit(小数点桁数制限)
        /// </summary>
        [System.ComponentModel.DefaultValue(1)]
        public int NumPointLimit { get; set; }
        /// <summary>
        /// 設置画面平均処理フラグ
        /// </summary>
        public bool IsPositionViewAverage { get; set; }

        #endregion

        #region public method
        /// <summary>
        /// output data to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("SystemConfig - SystemLanguage = {0}, IsSimulationMode = {1}", this.SystemLanguage, this.IsSimulationMode);
        }
        /// <summary>
        /// 設定をXMLファイルから読み込む
        /// </summary>
        public void LoadXmlFile()
        {
            var filePath = GetFilePath();
            XmlTextReader reader = null;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string tempString = string.Empty;
                    reader = new XmlTextReader(filePath);
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.Equals("SystemLanguage"))
                                {
                                    tempString = reader.GetAttribute("Language");
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.SystemLanguage = (CommonResource.LANGUAGE)Enum.Parse(typeof(CommonResource.LANGUAGE), tempString);
                                    }
                                    
                                }
                                else if (reader.Name.Equals("IsSimulationMode"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.IsSimulationMode = bool.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("IsDebugMode"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.IsDebugMode = bool.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("MaxSmallAxis"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.MaxSmallAxis = decimal.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("SensorHighValue_B"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.SensorHighValue_B = decimal.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("SensorHighValue_R"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.SensorHighValue_R = decimal.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("ValueRate_3D_R"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.ValueRate_3D_R = decimal.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("NumPointLimit"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.NumPointLimit = int.Parse(tempString);
                                    }
                                }
                                else if (reader.Name.Equals("IsPositionViewAverage"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.IsPositionViewAverage = bool.Parse(tempString);
                                    }
                                }

                                break;
                            case XmlNodeType.EndElement:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        /// <summary>
        /// 設定をXMLファイルに保存する
        /// </summary>
        public void SaveXmlFile()
        {
            // Writing the file requires a TextWriter.
            XmlTextWriter textWriter = null;
            string filePath = GetFilePath();
            try
            {
                if (!System.IO.Path.GetDirectoryName(filePath).Equals(""))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
                    {
                        throw new System.IO.FileNotFoundException(filePath, filePath);
                    }
                }

                // Create a new file
                textWriter = new XmlTextWriter(filePath, null);
                // Opens the document
                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("root");
                textWriter.WriteWhitespace("\n");
                //System Language
                textWriter.WriteStartElement("SystemLanguage");
                textWriter.WriteAttributeString("Language", this.SystemLanguage.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("IsSimulationMode");
                textWriter.WriteString(this.IsSimulationMode.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("IsDebugMode");
                textWriter.WriteString(this.IsDebugMode.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("MaxSmallAxis");
                textWriter.WriteString(this.MaxSmallAxis.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("SensorHighValue_B");
                textWriter.WriteString(this.SensorHighValue_B.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("SensorHighValue_R");
                textWriter.WriteString(this.SensorHighValue_R.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("ValueRate_3D_R");
                textWriter.WriteString(this.ValueRate_3D_R.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("NumPointLimit");
                textWriter.WriteString(this.NumPointLimit.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                textWriter.WriteStartElement("IsPositionViewAverage");
                textWriter.WriteString(this.IsPositionViewAverage.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");
                
                //end root
                textWriter.WriteEndElement();
                textWriter.WriteWhitespace("\n");

                // Ends the document.
                textWriter.WriteEndDocument();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":\n" + ex.StackTrace);
                throw new Exception(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                }
            }
        }
        /// <summary>
        /// Switch system language
        /// </summary>
        /// <param name="languageType"></param>
        public void SwitchLanguage(DataCommon.CommonResource.LANGUAGE languageType)
        {
            //set system language
            string langType = string.Empty;
            try
            {
                this.SystemLanguage = languageType;
                if (this.SystemLanguage == DataCommon.CommonResource.LANGUAGE.English)
                {
                    CommonResource.CurrentSystemLanguage = CommonResource.LANGUAGE.English;
                    langType = "en-US"; 
                }
                else if (this.SystemLanguage == DataCommon.CommonResource.LANGUAGE.Japanese)
                {
                    CommonResource.CurrentSystemLanguage = CommonResource.LANGUAGE.Japanese;
                    langType = "ja-JP"; 
                }
                else if (this.SystemLanguage == DataCommon.CommonResource.LANGUAGE.Chinese)
                {
                    CommonResource.CurrentSystemLanguage = CommonResource.LANGUAGE.Chinese;
                    langType = "zh-Hans"; 
                }
            }
            catch
            { 
                langType = string.Empty; 
            }
            if (!string.IsNullOrEmpty(langType))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(langType);
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(langType);
            }
            else { System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture; }
        }
        #endregion

        #region private method
        /// <summary>
        /// アプリケーション設定ファイルのパスを取得する
        /// </summary>
        /// <returns>アプリケーション設定ファイルのパス</returns>
        private string GetFilePath()
        {
            return SystemDirectoryPath.SystemPath + FileName;
        }
        #endregion

    }
}
