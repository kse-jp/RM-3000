using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace DataCommon
{
    /// <summary>
    /// base class for all setting class
    /// </summary>
    [Serializable]
    public abstract class SettingBase
    {
        #region private member
        /// <summary>
        /// setting class type
        /// </summary>
        protected TYPEOFCLASS typeOfClass;
        /// <summary>
        /// previous save data
        /// </summary>
        protected SettingBase oldValue;
        #endregion

        #region public member
        /// <summary>
        /// data type enum
        /// </summary>
        //[XmlAttribute("TypeOfClass")]
        [XmlIgnore]
        public TYPEOFCLASS TypeOfClass
        {
            set { typeOfClass = value; }
            get { return typeOfClass; }
        }
        /// <summary>
        /// ファイルパス
        /// </summary>
        [XmlIgnore]
        public string FilePath { set; get; }
        /// <summary>
        /// data is updated flag
        /// </summary>
        public bool IsUpdated { set; get; }
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SettingBase()
        { 
        }
        #endregion

        #region private mehtod
        #endregion

        #region public method
        /// <summary>
        /// Revert to last save
        /// </summary>
        public virtual void Revert()
        {
            
        }
        /// <summary>
        /// serialize class to xml file
        /// </summary>
        public void Serialize()
        {
            try
            {
                if (!string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(this.FilePath)))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.FilePath)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.FilePath));
                    }
                }
                var serializer = new XmlSerializer(this.GetType());
                using (var writer = new System.IO.StreamWriter(this.FilePath))
                {
                    serializer.Serialize(writer, this);
                    writer.Close();
                }
                this.oldValue = (SettingBase)this.MemberwiseClone();
                this.IsUpdated = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// deserialize from xml file name.Read class type from xml file
        /// </summary>
        /// <param name="filePath">xml file path</param>
        public static SettingBase Deserialize(string filePath)
        {
            var dataType = string.Empty;
            SettingBase data = null;
            XmlSerializer serializer = null;
            var doc = new XmlDocument();

            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception(CommonResource.GetString("ERROR_FILE_NOT_FOUND"));
                }
                doc.Load(filePath);

                if (!string.IsNullOrWhiteSpace(doc.DocumentElement.Name))
                {
                    dataType = doc.DocumentElement.Name;
                    switch (dataType)
                    {
                        case "DataTagSetting":
                            serializer = new XmlSerializer(typeof(DataTagSetting));
                            break;
                        case "DataTag":
                            serializer = new XmlSerializer(typeof(DataTag));
                            break;
                        case "ChannelsSetting":
                            serializer = new XmlSerializer(typeof(ChannelsSetting));
                            break;
                        case "ChannelSetting":
                            serializer = new XmlSerializer(typeof(ChannelSetting));
                            break;
                        case "RelationSetting":
                            serializer = new XmlSerializer(typeof(RelationSetting));
                            break;
                        case "TagChannelRelationSetting":
                            serializer = new XmlSerializer(typeof(TagChannelRelationSetting));
                            break;
                        case "PositionSetting":
                            serializer = new XmlSerializer(typeof(PositionSetting));
                            break;
                        case "SensorPositionSetting":
                            serializer = new XmlSerializer(typeof(SensorPositionSetting));
                            break;
                        case "MeasureSetting":
                            serializer = new XmlSerializer(typeof(MeasureSetting));
                            break;
                        case "ChannelMeasSetting":
                            serializer = new XmlSerializer(typeof(ChannelMeasSetting));
                            break;
                        case "ConstantData":
                            serializer = new XmlSerializer(typeof(ConstantData));
                            break;
                        case "ConstantSetting":
                            serializer = new XmlSerializer(typeof(ConstantSetting));
                            break;
                        case "GraphSetting":
                            serializer = new XmlSerializer(typeof(GraphSetting));
                            break;
                        case "GraphTag":
                            serializer = new XmlSerializer(typeof(GraphTag));
                            break;
                        case "B_BoardSetting":
                            serializer = new XmlSerializer(typeof(B_BoardSetting));
                            break;
                        case "L_BoardSetting":
                            serializer = new XmlSerializer(typeof(L_BoardSetting));
                            break;
                        case "R_BoardSetting":
                            serializer = new XmlSerializer(typeof(R_BoardSetting));
                            break;
                        case "V_BoardSetting":
                            serializer = new XmlSerializer(typeof(V_BoardSetting));
                            break;
                        case "TagChannelPattern":
                            serializer = new XmlSerializer(typeof(TagChannelPattern));
                            break;
                        case "Value_Mode2":
                            serializer = new XmlSerializer(typeof(Value_Mode2));
                            break;
                        case "Value_Standard":
                            serializer = new XmlSerializer(typeof(Value_Standard));
                            break;
                        case "TestData":
                            serializer = new XmlSerializer(typeof(ChannelData));
                            break;
                        case "MeasureData":
                            serializer = new XmlSerializer(typeof(MeasureData));
                            break;
                        case "AnalyzeData":
                            serializer = new XmlSerializer(typeof(AnalyzeData));
                            break;
                    }
                }
                else { throw new Exception(CommonResource.GetString("ERROR_XML_CLASSTYPE_NOT_EXIST")); }
                doc = null;
                if (serializer != null)
                {
                    using (FileStream f = new FileStream(filePath, FileMode.Open))
                    {
                        XmlTextReader r = new XmlTextReader(f);
                        data = (SettingBase)serializer.Deserialize(r);
                        data.FilePath = filePath;
                        data.IsUpdated = false;
                        f.Close();
                    }

                    //data.After_Deserialize(filePath);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// generic Serialize object to xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void SerializeToXml<T>(T obj, string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty( System.IO.Path.GetDirectoryName(filePath)))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                    }
                }
                var ser = new XmlSerializer(typeof(T));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ser.Serialize(fileStream, obj);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// generic deserialize xml file to known class type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string filePath)
        {
            T result;
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception(CommonResource.GetString("ERROR_FILE_NOT_FOUND"));
                }
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (FileStream f = new FileStream(filePath, FileMode.Open))
                {
                    XmlTextReader r = new XmlTextReader(f);
                    result = (T)ser.Deserialize(r);
                    f.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Binary Serialize object
        /// </summary>
        /// <param name="filePath"></param>
        public bool BinarySerialize(string filePath)
        {
            try
            {
                if (!System.IO.Path.GetDirectoryName(filePath).Equals(string.Empty))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                    }
                }
                return ObjectSerializer.BinarySerialize(filePath, this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Binary Deserialize file to object
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static SettingBase BinaryDeserialize(string filePath)
        {
            SettingBase result = null;
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception(CommonResource.GetString("ERROR_FILE_NOT_FOUND"));
                }
                result = (SettingBase)ObjectSerializer.BinaryDeserialze(filePath);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// デシリアライズの後処理
        /// </summary>
        /// <param name="xmlFilename"></param>
        protected virtual void After_Deserialize(string xmlFilename)
        {
        }
        
        #endregion
    }
    /// <summary>
    /// SettingBase inherit classes specific Object Serialize/Deserialize
    /// </summary>
    internal class ObjectSerializer
    {
        #region private member
        ///// <summary>
        ///// max arraysize
        ///// </summary>
        //private static int MaxArraySize = 100000;
        #endregion

        #region private method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private static bool BytesToBinaryFile(string filePath, byte[] byteArray)
        {
            System.IO.FileStream _FileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(byteArray, 0, byteArray.Length);

            // close file stream
            _FileStream.Close();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static byte[] BinaryFileToBytes(string filePath)
        {
            byte[] _Buffer = null;

            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // attach filestream to binary reader
                System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);

                // get total byte length of the file
                long _TotalBytes = new System.IO.FileInfo(filePath).Length;

                // read entire file into buffer
                _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);

                // close file reader
                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

            return _Buffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="md"></param>
        /// <returns></returns>
        //private static bool MeasureDataSerialize(BinaryWriter writer, MeasureData md)
        private static bool MeasureDataSerialize(FileStream writer, MeasureData md)
        {
            //int count = 0;
            try
            {
#if true

                writer.Write(BitConverter.GetBytes((int)md.TypeOfClass),0,sizeof(int));
                writer.Write(BitConverter.GetBytes(md.TestDate.Ticks), 0 , sizeof(long));
                writer.Write(BitConverter.GetBytes(md.StartTime.Ticks), 0 , sizeof(long));
                writer.Write(BitConverter.GetBytes(md.EndTime.Ticks), 0 , sizeof(long));
                writer.Write(BitConverter.GetBytes(md.SamplingTiming), 0, sizeof(int));

                //Delete 2012-10-03 M.Ohno Change save sequence of SampleDatas
                //if (md.SampleDatas != null)
                //{
                //    //count = md.SampleDatas.Length;
                //    count = md.SampleDatas.Count;
                //    //write array size
                //    writer.Write(BitConverter.GetBytes(count),0,sizeof(int));
                //    for (int i = 0; i < count; i++)
                //    {
                //        if (md.SampleDatas[i] != null)
                //        {
                //            //data class is not NULL
                //            writer.Write(BitConverter.GetBytes(md.SampleDatas[i].ChannelDatas.Length), 0, sizeof(int));
                //            //writer.Write(0);

                //            //Add 2012-08-21 M.OHno
                //            for (int j = 0; j < md.SampleDatas[i].ChannelDatas.Length; j++ )
                //            {
                //                writer.Write(BitConverter.GetBytes(md.SampleDatas[i].ChannelDatas[j].Position), 0, sizeof(int));
                //                DataValueSerialize(writer, md.SampleDatas[i].ChannelDatas[j].DataValues);
                //            //Add 2012-08-21 M.OHno

                //                //writer.Write(md.SampleDatas[i].Position);
                //                //DataValueSerialize(writer, md.SampleDatas[i].DataValues);
                //            }
                //        }
                //        else
                //        {
                //            //data class is NULL
                //            writer.Write(BitConverter.GetBytes(-1), 0, sizeof(int));
                //        }

                //    }

                //}
                //else
                //{
                //    // array is NULL, write array size
                //    writer.Write(BitConverter.GetBytes(-1), 0, sizeof(int));
                //}
                //Delete 2012-10-03 M.Ohno Change save sequence of SampleDatas

                return true;

#else

                writer.Write((int)md.TypeOfClass);
                writer.Write(md.IsNew);
                writer.Write(md.IsUpdated);
                writer.Write(md.IsDeleted);
                writer.Write(md.TestDate.Ticks);
                writer.Write(md.StartTime.Ticks);
                writer.Write(md.EndTime.Ticks);
                writer.Write(md.SamplingTiming);
                if (md.SampleDatas != null)
                {
                    //count = md.SampleDatas.Length;
                    count = md.SampleDatas.Count;
                    //write array size
                    writer.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        if (md.SampleDatas[i] != null)
                        {
                            //data class is not NULL
                            writer.Write(md.SampleDatas[i].ChannelDatas.Length);
                            //writer.Write(0);

                            //Add 2012-08-21 M.OHno
                            for (int j = 0; j < md.SampleDatas[i].ChannelDatas.Length; j++ )
                            {
                                writer.Write(md.SampleDatas[i].ChannelDatas[j].Position);
                                DataValueSerialize(writer, md.SampleDatas[i].ChannelDatas[j].DataValues);
                            //Add 2012-08-21 M.OHno

                                //writer.Write(md.SampleDatas[i].Position);
                                //DataValueSerialize(writer, md.SampleDatas[i].DataValues);
                            }
                        }
                        else
                        {
                            //data class is NULL
                            writer.Write(-1);
                        }

                    }

                }
                else
                {
                    // array is NULL, write array size
                    writer.Write(-1);
                }
                return true;
#endif
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static MeasureData MeasureDataDeserialize(BinaryReader reader)
        {
            //int count = 0;
            //int tempInt = 0;
            try
            {
                MeasureData md = new MeasureData();
                //md.TypeOfClass = 
                switch (reader.ReadInt32())
                {
                    case (int)TYPEOFCLASS.AnalyzeData:
                        md.TypeOfClass = TYPEOFCLASS.AnalyzeData;
                        break;
                    case (int)TYPEOFCLASS.MeasureData:
                        md.TypeOfClass = TYPEOFCLASS.MeasureData;
                        break;
                }
                md.TestDate = DateTime.FromBinary(BitConverter.ToInt64(reader.ReadBytes(sizeof(long)), 0));
                md.StartTime = DateTime.FromBinary(BitConverter.ToInt64(reader.ReadBytes(sizeof(long)), 0));
                md.EndTime = DateTime.FromBinary(BitConverter.ToInt64(reader.ReadBytes(sizeof(long)), 0));
                md.SamplingTiming = reader.ReadUInt32();

                //Delete 2012-10-03 M.Ohno Change save sequence of SampleDatas

                ////read array count
                //count = reader.ReadInt32();
                //if (count > MaxArraySize)
                //{
                //    throw new Exception("invalid array size");
                //}
                //if (count >= 0)
                //{
                //    //md.SampleDatas = new ChannelData[count];
                //    for (int i = 0; i < count; i++)
                //    {
                //        SampleData sampledata = new SampleData();

                //        //validate null class
                //        tempInt = reader.ReadInt32();
                //        if (tempInt >= 0)
                //        {
                //            sampledata.ChannelDatas = new ChannelData[tempInt];

                //            for (int j = 0; j < tempInt; j++)
                //            {
                //                sampledata.ChannelDatas[j] = new ChannelData();

                //                sampledata.ChannelDatas[j].Position = reader.ReadInt32();
                                
                //                int tempInt2 = reader.ReadInt32();
                //                //validate data value class is not null
                //                if (tempInt2 >= 0)
                //                {
                //                    //read datavalue class type
                //                    sampledata.ChannelDatas[j].DataValues = DataValueDeserialize(reader);
                //                }
                //            }

                //            //md.SampleDatas[i] = new ChannelData();
                //            //md.SampleDatas[i].Position = reader.ReadInt32();
                //            //tempInt = reader.ReadInt32();
                //            ////validate data value class is not null
                //            //if (tempInt >= 0)
                //            //{
                //            //    //read datavalue class type
                //            //    md.SampleDatas[i].DataValues = DataValueDeserialize(reader);
                //            //}
                //        }

                //        md.SampleDatas.Add(sampledata);
                //    }
                //}

                //Delete 2012-10-03 M.Ohno Change save sequence of SampleDatas

                return md;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
//        private static bool DataValueSerialize(BinaryWriter writer, DataValue dv)
        private static bool DataValueSerialize(FileStream writer, DataValue dv)
        {
            try
            {
#if true
                if (dv != null)
                {
                    //write data class normal
                    writer.Write(BitConverter.GetBytes(0), 0, sizeof(int));
                    if (dv.GetType() == typeof(Value_Standard))
                    {
                        //write class type no. Value_startd
                        writer.Write(BitConverter.GetBytes(1), 0, sizeof(int));
                        Value_Standard vs = (Value_Standard)dv;
                        //write value
                        writer.Write(BitConverter.GetBytes((double)vs.Value), 0, sizeof(double));
                    }
                    else if (dv.GetType() == typeof(Value_Mode2))
                    {
                        //write class type no. Value_mode2
                        writer.Write(BitConverter.GetBytes(2), 0, sizeof(int));
                        Value_Mode2 vm = (Value_Mode2)dv;
                        if (vm.Values != null)
                        {
                            int vmCount = vm.Values.Length;
                            writer.Write(BitConverter.GetBytes(vmCount), 0, sizeof(int));
                            for (int vmIndex = 0; vmIndex < vmCount; vmIndex++)
                            {
                                writer.Write(BitConverter.GetBytes((double)vm.Values[vmIndex]), 0, sizeof(double));
                            }
                        }
                        else
                        {
                            writer.Write(BitConverter.GetBytes(0), -1, sizeof(int));
                        }

                    }
                }
                else 
                {
                    writer.Write(BitConverter.GetBytes(0), -1, sizeof(int));
                }

                //Add 2012-08-22 M.Ohno
                writer.Flush();
                //Add 2012-08-22 M.Ohno

                return true;

#else
                if (dv != null)
                {
                    //write data class normal
                    writer.Write(0);
                    if (dv.GetType() == typeof(Value_Standard))
                    {
                        //write class type no. Value_startd
                        writer.Write(1);
                        Value_Standard vs = (Value_Standard)dv;
                        //write value
                        writer.Write(vs.Value);
                    }
                    else if (dv.GetType() == typeof(Value_Mode2))
                    {
                        //write class type no. Value_mode2
                        writer.Write(2);
                        Value_Mode2 vm = (Value_Mode2)dv;
                        if (vm.Values != null)
                        {
                            int vmCount = vm.Values.Length;
                            writer.Write(vmCount);
                            for (int vmIndex = 0; vmIndex < vmCount; vmIndex++)
                            {
                                writer.Write(vm.Values[vmIndex]);
                                //Add 2012-08-22 M.Ohno
                                writer.Flush();
                                //Add 2012-08-22 M.Ohno
                            }
                        }
                        else
                        {
                            writer.Write(-1);
                        }

                    }
                }
                else { writer.Write(-1); }


                return true;

#endif
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static DataValue DataValueDeserialize(BinaryReader reader)
        {
            int tempInt = 0;
            try
            {
                //read datavalue class type
                tempInt = reader.ReadInt32();
                if (tempInt == 1)
                {
                    Value_Standard vs = new Value_Standard();
                    vs.Value = reader.ReadDecimal();
                    return vs;
                }
                else if (tempInt == 2)
                {
                    Value_Mode2 vm = new Value_Mode2();
                    //read array count
                    tempInt = reader.ReadInt32();
                    if (tempInt >= 0)
                    {
                        vm.Values = new decimal[tempInt];
                        for (int vmIndex = 0; vmIndex < tempInt; vmIndex++)
                        {
                            vm.Values[vmIndex] = reader.ReadDecimal();
                        }
                        return vm;
                    }
                    return null;
                }
                else { return null; }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static bool DataTagSettingSerialize(BinaryWriter writer, DataTagSetting ds)
        {
            int count = 0;
            System.Text.UnicodeEncoding encode = new UnicodeEncoding();
            byte[] tagNameBytes = null;
            byte[] UnitBytes = null;
            byte[] ExpressionBytes = null;
            try
            {
                if (ds != null)
                {
                    //class object is not null
                    writer.Write(0);
                    writer.Write((int)ds.TypeOfClass);
                    if (ds.DataTagList != null)
                    {
                        count = ds.DataTagList.Length;
                        //array size
                        writer.Write(count);
                        for (int i = 0; i < count; i++)
                        {
                            if (ds.DataTagList[i] != null)
                            {
                                //class object is NOT NULL
                                writer.Write(0);
                                DataTag dt = ds.DataTagList[i];
                                writer.Write(dt.TagNo);
                                writer.Write(dt.TagKind);
                                writer.Write(dt.Point);
                                writer.Write(dt.StaticZero);

                                if (string.IsNullOrEmpty(dt.TagNameE))
                                {
                                    dt.TagNameE = string.Empty;
                                }
                                tagNameBytes = encode.GetBytes(dt.TagNameE);
                                //write string length (int)
                                writer.Write(tagNameBytes.Length);
                                if (tagNameBytes.Length > 0)
                                {
                                    writer.Write(tagNameBytes);
                                }
                                

                                if (string.IsNullOrEmpty(dt.TagNameJ))
                                {
                                    dt.TagNameJ = string.Empty;
                                }
                                tagNameBytes = encode.GetBytes(dt.TagNameJ);
                                //write string length (int)
                                writer.Write(tagNameBytes.Length);
                                if (tagNameBytes.Length > 0)
                                {
                                    writer.Write(tagNameBytes);
                                }


                                if (string.IsNullOrEmpty(dt.TagNameC))
                                {
                                    dt.TagNameC = string.Empty;
                                }
                                tagNameBytes = encode.GetBytes(dt.TagNameC);
                                //write string length (int)
                                writer.Write(tagNameBytes.Length);
                                writer.Write(tagNameBytes);
                                if (tagNameBytes.Length > 0)
                                {
                                    writer.Write(tagNameBytes);
                                }

                                if (string.IsNullOrEmpty(dt.UnitE))
                                {
                                    dt.UnitE = string.Empty;
                                }
                                UnitBytes = encode.GetBytes(dt.UnitE);
                                //write string length (int)
                                writer.Write(UnitBytes.Length);
                                if (UnitBytes.Length > 0)
                                {
                                    writer.Write(UnitBytes);
                                }
                                

                                if (string.IsNullOrEmpty(dt.UnitJ))
                                {
                                    dt.UnitJ = string.Empty;
                                }
                                UnitBytes = encode.GetBytes(dt.UnitJ);
                                //write string length (int)
                                writer.Write(UnitBytes.Length);
                                if (UnitBytes.Length > 0)
                                {
                                    writer.Write(UnitBytes);
                                }

                                if (string.IsNullOrEmpty(dt.UnitC))
                                {
                                    dt.UnitC = string.Empty;
                                }
                                UnitBytes = encode.GetBytes(dt.UnitC);
                                //write string length (int)
                                writer.Write(UnitBytes.Length);
                                if (UnitBytes.Length > 0)
                                {
                                    writer.Write(UnitBytes);
                                }

                                if (string.IsNullOrEmpty(dt.Expression))
                                {
                                    dt.Expression = string.Empty;
                                }
                                ExpressionBytes = encode.GetBytes(dt.Expression);
                                //write string length (int)
                                writer.Write(ExpressionBytes.Length);
                                if (ExpressionBytes.Length > 0)
                                {
                                    writer.Write(ExpressionBytes);
                                }
                            }
                            else
                            {
                                //class object is NULL
                                writer.Write(-1);
                            }
                        }

                    }
                    else
                    { writer.Write(-1); }
                }
                else
                {
                    //class object is NULL
                    writer.Write(-1);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static DataTagSetting DataTagSettingDeserialize(BinaryReader reader)
        {
            int count = 0;
            int tempInt = 0;
            int length = 0;
            System.Text.UnicodeEncoding encode = new UnicodeEncoding();
            try
            {
                //class object is NULL?
                tempInt = reader.ReadInt32();
                if (tempInt < 0)
                {
                    return null;
                }
                DataTagSetting dt = new DataTagSetting();
                //dt.TypeOfClass = 
                reader.ReadInt32();
                //read array count
                count = reader.ReadInt32();
                if (count >= 0)
                {
                    dt.DataTagList = new DataTag[count];
                    for (int i = 0; i < count; i++)
                    {
                        //validate null class
                        tempInt = reader.ReadInt32();
                        if (tempInt >= 0)
                        {
                            dt.DataTagList[i] = new DataTag();
                            dt.DataTagList[i].TagNo = reader.ReadInt32();
                            dt.DataTagList[i].TagKind = reader.ReadInt32();
                            dt.DataTagList[i].Point = reader.ReadInt32();
                            dt.DataTagList[i].StaticZero = reader.ReadDecimal();


                            //read string bytes length
                            length = reader.ReadInt32();
                            //read class info
                            if (length > 0)
                            {
                                dt.DataTagList[i].TagNameE = encode.GetString(reader.ReadBytes(length));
                            }

                            //read string bytes length
                            length = reader.ReadInt32();
                            //read class info
                            if (length > 0)
                            {
                                dt.DataTagList[i].TagNameJ = encode.GetString(reader.ReadBytes(length));
                            }

                            //read string bytes length
                            length = reader.ReadInt32();
                            if (length > 0)
                            {
                                //read class info
                                dt.DataTagList[i].TagNameC = encode.GetString(reader.ReadBytes(length));
                            }

                            //read string bytes length
                            length = reader.ReadInt32();
                            if (length > 0)
                            {
                                //read string
                                dt.DataTagList[i].UnitE = encode.GetString(reader.ReadBytes(length));
                            }
                            //read string bytes length
                            length = reader.ReadInt32();
                            if (length > 0)
                            {
                                //read string
                                dt.DataTagList[i].UnitJ = encode.GetString(reader.ReadBytes(length));
                            }
                            //read string bytes length
                            length = reader.ReadInt32();
                            if (length > 0)
                            {
                                //read string
                                dt.DataTagList[i].UnitC = encode.GetString(reader.ReadBytes(length));
                            }

                            //read string bytes length
                            length = reader.ReadInt32();
                            if (length > 0)
                            {
                                //read class info
                                dt.DataTagList[i].Expression = encode.GetString(reader.ReadBytes(length));
                            }
                        }
                    }
                }
                return dt;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="cs"></param>
        /// <returns></returns>
        private static bool ChannelsSettingSerialize(BinaryWriter writer, ChannelsSetting cs)
        {
            int count = 0;
            try
            {
                if (cs != null)
                {
                    //object class is NOT NULL
                    writer.Write(0);
                    writer.Write((int)cs.TypeOfClass);
                    if (cs.ChannelMeasSetting != null)
                    {
                        writer.Write(0);
                        writer.Write(cs.ChannelMeasSetting.MainTrigger);
                        writer.Write((int)cs.ChannelMeasSetting.Mode2_Trigger);
                    }
                    else
                    {
                        //class object is NULL
                        writer.Write(-1);
                    }
                    if (cs.ChannelSettingList != null)
                    {
                        count = cs.ChannelSettingList.Length;
                        //array size
                        writer.Write(count);
                        for (int i = 0; i < count; i++)
                        {
                            if (cs.ChannelSettingList[i] != null)
                            {
                                //class object is NOT NULL
                                writer.Write(0);
                                writer.Write(cs.ChannelSettingList[i].ChNo);
                                writer.Write((int)cs.ChannelSettingList[i].ChKind);
                                writer.Write((int)cs.ChannelSettingList[i].Mode1_Trigger);
                                if (cs.ChannelSettingList[i].BoardSetting != null)
                                {
                                    //class object is NOT NULL
                                    writer.Write(0);
                                    if (cs.ChannelSettingList[i].BoardSetting.GetType() == typeof(B_BoardSetting))
                                    {
                                        //class type
                                        writer.Write(1);
                                        B_BoardSetting bb = (B_BoardSetting)cs.ChannelSettingList[i].BoardSetting;
                                        writer.Write(bb.Hold);
                                        writer.Write(bb.Precision);
                                    }
                                    else if (cs.ChannelSettingList[i].BoardSetting.GetType() == typeof(R_BoardSetting))
                                    {
                                        //class type
                                        writer.Write(2);
                                        R_BoardSetting rb = (R_BoardSetting)cs.ChannelSettingList[i].BoardSetting;
                                        writer.Write(rb.Precision);
                                    }
                                    else if (cs.ChannelSettingList[i].BoardSetting.GetType() == typeof(V_BoardSetting))
                                    {
                                        //class type
                                        writer.Write(3);
                                        V_BoardSetting vb = (V_BoardSetting)cs.ChannelSettingList[i].BoardSetting;
                                        writer.Write(vb.Filter);
                                        writer.Write(vb.Range);
                                        writer.Write(vb.Full);
                                        writer.Write(vb.Zero);
                                    }
                                    else if (cs.ChannelSettingList[i].BoardSetting.GetType() == typeof(L_BoardSetting))
                                    {
                                        //class type
                                        writer.Write(4);
                                        L_BoardSetting lb = (L_BoardSetting)cs.ChannelSettingList[i].BoardSetting;
                                        writer.Write(lb.Range);
                                        writer.Write(lb.SensorOutput);
                                        writer.Write(lb.Full);
                                    }
                                }
                                else
                                {
                                    //class object is NULL
                                    writer.Write(-1);
                                }
                            }
                            else
                            {
                                //class object is NULL
                                writer.Write(-1);
                            }
                        }
                    }
                    else
                    {
                        //Array is NULL
                        writer.Write(-1);
                    }
                }
                else
                {
                    //class object is NULL
                    writer.Write(-1);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ChannelsSetting ChannelsSettingDeserialize(BinaryReader reader)
        {
            ChannelsSetting cs = null;
            int count = 0;
            int tempInt = 0;
            try
            {
                tempInt = reader.ReadInt32();
                if (tempInt < 0)
                {
                    return null;
                }
                cs = new ChannelsSetting();
                //(int)cs.TypeOfClass
                reader.ReadInt32();
                tempInt = reader.ReadInt32();
                if (tempInt >= 0)
                {
                    cs.ChannelMeasSetting.MainTrigger = reader.ReadInt32();
                    cs.ChannelMeasSetting.Mode2_Trigger = (Mode2TriggerType)reader.ReadInt32();
                }
                count = reader.ReadInt32();
                if (count >= 0)
                {
                    //array size
                    cs.ChannelSettingList = new ChannelSetting[count];
                    for (int i = 0; i < count; i++)
                    {
                        tempInt = reader.ReadInt32();
                        if (tempInt >= 0)
                        {
                            //class object is NOT NULL
                            cs.ChannelSettingList[i] = new ChannelSetting();
                            cs.ChannelSettingList[i].ChNo = reader.ReadInt32();
                            cs.ChannelSettingList[i].ChKind = (ChannelKindType)reader.ReadInt32();
                            cs.ChannelSettingList[i].Mode1_Trigger = (Mode1TriggerType)reader.ReadInt32();
                            //class object is NOT NULL
                            tempInt = reader.ReadInt32();
                            if (tempInt >= 0)
                            {
                                //class type
                                tempInt = reader.ReadInt32();
                                if (tempInt == 1)
                                {
                                    B_BoardSetting bb = new B_BoardSetting();
                                    bb.Hold = reader.ReadInt32();
                                    bb.Precision = reader.ReadBoolean();
                                    cs.ChannelSettingList[i].BoardSetting = bb;
                                }
                                else if (tempInt == 2)
                                {
                                    R_BoardSetting rb = new R_BoardSetting();
                                    rb.Precision = reader.ReadBoolean();
                                    cs.ChannelSettingList[i].BoardSetting = rb;
                                }
                                else if (tempInt == 3)
                                {
                                    V_BoardSetting vb = new V_BoardSetting();
                                    vb.Filter = reader.ReadInt32();
                                    vb.Range = reader.ReadInt32();
                                    vb.Full = reader.ReadDecimal();
                                    vb.Zero = reader.ReadDecimal();
                                    cs.ChannelSettingList[i].BoardSetting = vb;
                                }
                                else if (tempInt == 4)
                                {
                                    L_BoardSetting lb = new L_BoardSetting();
                                    lb.Range = reader.ReadInt32();
                                    lb.SensorOutput = reader.ReadDecimal();
                                    lb.Full = reader.ReadDecimal();
                                }
                            }
                        }
                    }
                }
                return cs;
            }
            catch
            {

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool ChannelRelationSerialize(BinaryWriter writer, TagChannelRelationSetting cr)
        {
            int count = 0;
            try
            {
                if (cr != null)
                {
                    //object class is NOT NULL
                    writer.Write(0);
                    writer.Write((int)cr.TypeOfClass);
                    if (cr.RelationList != null)
                    {
                        count = cr.RelationList.Length;
                        writer.Write(count);
                        for (int i = 0; i < count; i++)
                        {
                            if (cr.RelationList[i] != null)
                            {
                                writer.Write(0xFF);
                                writer.Write(cr.RelationList[i].ChannelNo);
                                writer.Write(cr.RelationList[i].TagNo_1);
                                writer.Write(cr.RelationList[i].TagNo_2);
                            }
                            else
                            {
                                writer.Write(-1);
                            }
                        }
                    }
                    else
                    {
                        writer.Write(-1);
                    }
                }
                else
                {
                    //class object is NULL
                    writer.Write(0);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static TagChannelRelationSetting ChannelRelationDeserialize(BinaryReader reader)
        {
            int count = 0;
            int tempInt = 0;
            TagChannelRelationSetting cr = null;
            try
            {
                tempInt = reader.ReadInt32();
                if (tempInt >= 0)
                {
                    cr = new TagChannelRelationSetting();
                    reader.ReadInt32();
                    //writer.Write((int)cr.TypeOfClass);
                    count = reader.ReadInt32();
                    if (count >= 0)
                    {
                        if (count > 11)
                        { count = 11; }
                        cr.RelationList = new RelationSetting[count];
                        for (int i = 0; i < count; i++)
                        {
                            tempInt = reader.ReadInt32();
                            if (tempInt >= 0)
                            {
                                cr.RelationList[i] = new RelationSetting();
                                cr.RelationList[i].ChannelNo = reader.ReadInt32();
                                cr.RelationList[i].TagNo_1 = reader.ReadInt32();
                                cr.RelationList[i].TagNo_2 = reader.ReadInt32();
                            }
                        }
                    }
                }

                return cr;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        private static bool SensorPositionSerialize(BinaryWriter writer, SensorPositionSetting sp)
        {
            int count = 0;
            try
            {
                if (sp != null)
                {
                    //object class is NOT NULL
                    writer.Write(0);
                    writer.Write((int)sp.TypeOfClass);
                    if (sp.PositionList != null)
                    {
                        count = sp.PositionList.Length;
                        writer.Write(count);
                        for (int i = 0; i < count; i++)
                        {
                            if (sp.PositionList[i] != null)
                            {
                                writer.Write(0xff);
                                writer.Write(sp.PositionList[i].ChNo);
                                writer.Write(sp.PositionList[i].X);
                                writer.Write(sp.PositionList[i].Z);
                            }
                            else
                            {
                                writer.Write(-1);
                            }
                        }
                    }
                    else
                    {
                        writer.Write(-1);
                    }
                }
                else
                {
                    //class object is NULL
                    writer.Write(-1);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static SensorPositionSetting SensorPositionDeserialize(BinaryReader reader)
        {
            int count = 0;
            int tempInt = 0;
            SensorPositionSetting sp = null;
            try
            {
                tempInt = reader.ReadInt32();
                if (tempInt >= 0)
                {
                    reader.ReadInt32();
                    //writer.Write((int)cr.TypeOfClass);
                    count = reader.ReadInt32();
                    if (count >= 0)
                    {
                        if (count > 11)
                        { count = 11; }
                        sp.PositionList = new PositionSetting[count];
                        for (int i = 0; i < count; i++)
                        {
                            tempInt = reader.ReadInt32();
                            if (tempInt >= 0)
                            {
                                sp.PositionList[i].ChNo = reader.ReadInt32();
                                sp.PositionList[i].X = reader.ReadInt32();
                                sp.PositionList[i].Z = reader.ReadInt32();
                            }
                        }
                    }
                }

                return sp;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        private static bool MeasureSettingSerialize(BinaryWriter writer, MeasureSetting ms)
        {
            int count = 0;
            int count2 = 0;
            System.Text.UnicodeEncoding encode = new UnicodeEncoding();
            byte[] titleBytes = null;
            byte[] colorBytes = null;
            try
            {
                if (ms != null)
                {
                    //object class is NOT NULL
                    writer.Write(0);
                    writer.Write((int)ms.TypeOfClass);
                    writer.Write(ms.Mode);
                    if (ms.MeasTagList != null)
                    {
                        count = ms.MeasTagList.Length;
                        writer.Write(count);
                        for (int i = 0; i < count; i++)
                        {
                            writer.Write(ms.MeasTagList[i]);
                        }
                    }
                    else
                    {
                        writer.Write(-1);
                    }
                    if (ms.GraphSettingList != null)
                    {
                        count = ms.GraphSettingList.Length;
                        writer.Write(count);
                        for (int j = 0; j < count; j++)
                        {
                            if (ms.GraphSettingList[j] != null)
                            {
                                //class object is NOT NULL
                                writer.Write(0);
                                if (string.IsNullOrEmpty(ms.GraphSettingList[j].Title))
                                {
                                    ms.GraphSettingList[j].Title = string.Empty;
                                }
                                titleBytes = encode.GetBytes(ms.GraphSettingList[j].Title);
                                //write string length (int)
                                writer.Write(titleBytes.Length);
                                if (titleBytes.Length > 0)
                                {
                                    writer.Write(titleBytes);
                                }
                                writer.Write(ms.GraphSettingList[j].CenterScale);
                                writer.Write(ms.GraphSettingList[j].Scale);
                                if (ms.GraphSettingList[j].GraphTagList != null)
                                {
                                    count2 = ms.GraphSettingList[j].GraphTagList.Length;
                                    writer.Write(count2);
                                    for (int k = 0; k < count2; k++)
                                    {
                                        if (ms.GraphSettingList[j].GraphTagList[k] != null)
                                        {
                                            //class object is NOT NULL
                                            writer.Write(0);
                                            writer.Write(ms.GraphSettingList[j].GraphTagList[k].GraphTagNo);
                                            writer.Write(ms.GraphSettingList[j].GraphTagList[k].BaseScale);
                                            if(string.IsNullOrEmpty(ms.GraphSettingList[j].GraphTagList[k].Color))
                                            {
                                                ms.GraphSettingList[j].GraphTagList[k].Color = string.Empty;
                                            }
                                            colorBytes = encode.GetBytes(ms.GraphSettingList[j].GraphTagList[k].Color);
                                            //write class info string length (int)
                                            writer.Write(colorBytes.Length);
                                            //write class assemply name to create instance when deserialize()
                                            writer.Write(colorBytes);
                                        }
                                        else
                                        {
                                            //class object is NULL
                                            writer.Write(-1);
                                        }
                                    }
                                }
                                else
                                {
                                    //class object is NULL
                                    writer.Write(-1);
                                }
                            }
                            else
                            {
                                //class object is NULL
                                writer.Write(-1);
                            }
                        }

                    }
                    else
                    {
                        writer.Write(-1);
                    }
                }
                else
                {
                    //class object is NULL
                    writer.Write(-1);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static MeasureSetting MeasureSettingDeserialize(BinaryReader reader)
        {
            int count = 0;
            int count2 = 0;
            int tempInt = 0;
            System.Text.UnicodeEncoding encode = new UnicodeEncoding();
            byte[] titleBytes = null;
            byte[] colorBytes = null;
            MeasureSetting ms = null;
            try
            {
                tempInt = reader.ReadInt32();
                if (tempInt >= 0)
                {
                    ms = new MeasureSetting();
                    reader.ReadInt32(); //writer.Write((int)cr.TypeOfClass);
                    ms.Mode = reader.ReadInt32();
                    count = reader.ReadInt32();
                    if (count >= 0)
                    {
                        int[] meas = new int[count];
                        
                        for (int i = 0; i < count; i++)
                        {
                            meas[i] = reader.ReadInt32();
                        }
                        ms.MeasTagList = meas;
                    }
                    count = reader.ReadInt32();
                    if (count >= 0)
                    {
                        ms.GraphSettingList = new GraphSetting[count];
                        for (int j = 0; j < count; j++)
                        {
                            tempInt = reader.ReadInt32();
                            if (tempInt >= 0)
                            {
                                ms.GraphSettingList[j] = new GraphSetting();
                                //string length
                                tempInt = reader.ReadInt32();
                                if (tempInt > 0)
                                {
                                    titleBytes = reader.ReadBytes(tempInt);
                                    ms.GraphSettingList[j].Title = encode.GetString(titleBytes);
                                }
                                ms.GraphSettingList[j].CenterScale = reader.ReadDecimal();
                                ms.GraphSettingList[j].Scale = reader.ReadDecimal();
                                count2 = reader.ReadInt32();
                                if (count2 >= 0)
                                {
                                    ms.GraphSettingList[j].GraphTagList = new GraphTag[count2];
                                    for (int k = 0; k < count2; k++)
                                    {
                                        tempInt = reader.ReadInt32();
                                        if (tempInt >= 0)
                                        {
                                            ms.GraphSettingList[j].GraphTagList[k] = new GraphTag();
                                            ms.GraphSettingList[j].GraphTagList[k].GraphTagNo = reader.ReadInt32();
                                            ms.GraphSettingList[j].GraphTagList[k].BaseScale = reader.ReadBoolean();
                                            tempInt = reader.ReadInt32();
                                            if (tempInt > 0)
                                            {
                                                colorBytes = reader.ReadBytes(tempInt);
                                                ms.GraphSettingList[j].GraphTagList[k].Color = encode.GetString(colorBytes);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return ms;
            }
            catch
            {
                throw;
            }
        }
        ///// <summary>
        ///// anaylizeData Serialize
        ///// </summary>
        ///// <param name="writer"></param>
        ///// <param name="ad"></param>
        ///// <returns></returns>
        //private static bool AnalyzeDataSerialize(BinaryWriter writer, AnalyzeData ad)
        //{
        //    try
        //    {
        //        if (ad != null)
        //        {
        //            //object class is NOT NULL
        //            writer.Write(0);
        //            writer.Write((int)ad.TypeOfClass);
        //            writer.Write(ad.IsNew);
        //            writer.Write(ad.IsUpdated);
        //            writer.Write(ad.IsDeleted);
        //            MeasureSettingSerialize(writer, ad.MeasureSetting);
        //            ChannelRelationSerialize(writer, ad.TagChannelRelationSetting);
        //            DataTagSettingSerialize(writer, ad.DataTagSetting);
        //            ChannelsSettingSerialize(writer, ad.ChannelsSetting);
        //            //Test
        //            //MeasureDataSerialize(writer, ad.MeasureData);
        //        }
        //        else
        //        {
        //            //class object is NULL
        //            writer.Write(-1);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //private static AnalyzeData AnalyzeDataDeserialize(BinaryReader reader)
        //{
        //    int tempInt = 0;
        //    AnalyzeData ad = null;
        //    try
        //    {
        //        tempInt = reader.ReadInt32();
        //        if (tempInt >= 0)
        //        {
        //            ad = new AnalyzeData();
        //            reader.ReadInt32();
        //            //writer.Write((int)cr.TypeOfClass);
        //            ad.IsNew = reader.ReadBoolean();
        //            ad.IsUpdated = reader.ReadBoolean();
        //            ad.IsDeleted = reader.ReadBoolean();
        //            ad.MeasureSetting = MeasureSettingDeserialize(reader);
        //            ad.TagChannelRelationSetting = ChannelRelationDeserialize(reader);
        //            ad.DataTagSetting = DataTagSettingDeserialize(reader);
        //            ad.ChannelsSetting = ChannelsSettingDeserialize(reader);
        //            ad.MeasureData = MeasureDataDeserialize(reader);
        //        }

        //        return ad;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region public method
        /// <summary>
        /// serialize
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool BinarySerialize(string filePath, object source)
        {
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(m);
            System.IO.FileStream writer2 = new System.IO.FileStream(filePath,FileMode.Create);


            System.Text.UnicodeEncoding encode = new UnicodeEncoding();
            try
            {
                string classInfo = source.GetType().AssemblyQualifiedName;
                byte[] classBytes = encode.GetBytes(classInfo);
                //write class info string length (int)
                //writer.Write(classBytes.Length);
                writer2.Write(BitConverter.GetBytes(classBytes.Length), 0, sizeof(int));
                //write class assemply name to create instance when deserialize()
                //writer.Write(classBytes);
                writer2.Write(classBytes, 0, classBytes.Length);

                if (source.GetType() == typeof(MeasureData))
                {
                    MeasureDataSerialize(writer2, (MeasureData)source);
                    //MeasureDataSerialize(writer, (MeasureData)source);
                }
                //if (source.GetType() == typeof(AnalyzeData))
                //{
                //    AnalyzeDataSerialize(writer, (AnalyzeData)source);
                //}
                else if (source.GetType() == typeof(DataTagSetting))
                {
                    DataTagSettingSerialize(writer, (DataTagSetting)source);
                }
                else if (source.GetType() == typeof(ChannelsSetting))
                {
                    ChannelsSettingSerialize(writer, (ChannelsSetting)source);
                }
                else if (source.GetType() == typeof(TagChannelRelationSetting))
                {
                    ChannelRelationSerialize(writer, (TagChannelRelationSetting)source);
                }
                else if (source.GetType() == typeof(SensorPositionSetting))
                {
                    SensorPositionSerialize(writer, (SensorPositionSetting)source);
                }
                else if (source.GetType() == typeof(MeasureSetting))
                {
                    MeasureSettingSerialize(writer, (MeasureSetting)source);
                }
                //test
                //BytesToBinaryFile(filePath, m.ToArray());
                
                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                writer2.Close();
                writer.Close();
                m.Close();
            }
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object BinaryDeserialze(string filePath)
        {
            byte[] buffer = null;
            System.IO.MemoryStream m = null;
            System.IO.BinaryReader reader = null;
            string classInfo = string.Empty;
            string tempString = string.Empty;
            int length = 0;
            object target = null;
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return null;
                }
                buffer = BinaryFileToBytes(filePath);
                m = new System.IO.MemoryStream(buffer);
                reader = new System.IO.BinaryReader(m);
                System.Text.UnicodeEncoding encode = new UnicodeEncoding();
                //read string bytes length
                length = reader.ReadInt32();
                //read class info
                classInfo = encode.GetString(reader.ReadBytes(length));
                target = Activator.CreateInstance(Type.GetType(classInfo));
                if (target.GetType() == typeof(MeasureData))
                {
                    return MeasureDataDeserialize(reader);
                }
                //if (target.GetType() == typeof(AnalyzeData))
                //{
                //    return AnalyzeDataDeserialize(reader);
                //}
                else if (target.GetType() == typeof(DataTagSetting))
                {
                    return DataTagSettingDeserialize(reader);
                }
                else if (target.GetType() == typeof(ChannelsSetting))
                {
                    return ChannelsSettingDeserialize(reader);
                }
                else if (target.GetType() == typeof(TagChannelRelationSetting))
                {
                    return ChannelRelationDeserialize(reader);
                }
                else if (target.GetType() == typeof(SensorPositionSetting))
                {
                    return SensorPositionDeserialize(reader);
                }
                else if (target.GetType() == typeof(MeasureSetting))
                {
                    return MeasureSettingDeserialize(reader);
                }
                return target;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (m != null) { m.Close(); }
            }
        }

        #endregion

    }
}
