using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using CommonLib;
using DataCommon;

namespace RM_3000
{
    [Serializable]
    public class HardInfoStruct : InfoStructBase
    {
        public delegate void ChangeExportModeEventHander();
        /// <summary>
        /// 海外モード変更イベント
        /// </summary>
        public event ChangeExportModeEventHander ChangeExportMode = delegate { };

        /// <summary>
        /// XMLファイル名
        /// </summary>
        public static string FileName = "HardInfoStruct.xml";


        /// <summary>
        /// ボード情報
        /// </summary>
        [XmlIgnore]
        public List<BoardInfoStruct> BoardInfos { get { return _BoardInfos; } set { _BoardInfos = value; } }

        /// <summary>
        /// 海外モード
        /// </summary>
        public bool IsExportMode 
        {
            get { return _IsExportMode;}
            set 
            {
                if (_IsExportMode != value)
                {
                    _IsExportMode = value;
                    ChangeExportMode();
                }
            } 
        }

        private List<BoardInfoStruct> _BoardInfos = new List<BoardInfoStruct>(Constants.MAX_CHANNELCOUNT);

        private bool _IsExportMode = false;

        /// <summary>
        /// 暖気中フラグ
        /// </summary>
        [XmlIgnore]
        public bool IsWarmingUp { get; set; }

        /// <summary>
        /// 暖気残り時間
        /// </summary>
        [XmlIgnore]
        public string RestTimeOFWarmingUp { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HardInfoStruct()
        {
            //ボード情報の初期化
            for (int index = 0; index <= Constants.MAX_CHANNELCOUNT; index++)
            {
                _BoardInfos.Add(new BoardInfoStruct());
            }

            IsWarmingUp = false;
            RestTimeOFWarmingUp = string.Empty;
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
                                if (reader.Name.Equals("IsExportMode"))
                                {
                                    tempString = reader.ReadElementContentAsString();
                                    if (!string.IsNullOrEmpty(tempString))
                                    {
                                        this.IsExportMode = bool.Parse(tempString);
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

                textWriter.WriteStartElement("IsExportMode");
                textWriter.WriteString(this.IsExportMode.ToString());
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
