using System;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace DataCommon
{
    public class SampleDataManager : DataClassBase , IList<SampleData>, ICloneable
    {
        #region public Const
        /// <summary>
        /// 一時読込制限件数(100,000件)
        /// </summary>
        public const int MAX_DATA_READ_LENGTH = 100000;

        /// <summary>
        /// 一時読込制限数 MODE2(10件)
        /// </summary>
        public const int MAX_DATA_READ_LENGTH_MODE2 = 10;

        /// <summary>
        /// 1データのサイズ
        /// </summary>
        public const int ONE_DATA_SIZE = 4;

        /// <summary>
        /// チャンネルMAX
        /// </summary>
        public const int CHANNEL_COUNT_MAX = 11;

        /// <summary>
        /// ファイル名
        /// </summary>
        public static string FileName = "Meas.dat";

        /// <summary>
        /// 一度に書き込むデータ量制限
        /// </summary>
        public const int MAX_DATA_WRITE_AT_ONCE = 3000;

        #endregion

        #region public properties
        /// <summary>
        /// 全データ数
        /// </summary>
        public int SamplesCount 
        {
            get
            {
                return (int)HeaderData.SamplesCount;
            }
        }

        /// <summary>
        /// SampleDataHeader
        /// </summary>
        public SampleDataHeader HeaderData { get; set; }

        /// <summary>
        /// 自動Writeを行う
        /// </summary>
        public bool AutoWriteFlag
        {
            get 
            {
                return _AutoWriteFlag;
            }
            set 
            {
                if (_AutoWriteFlag == value)
                    return;

                if (value)
                {
                    thWrite = new System.Threading.Thread(new System.Threading.ThreadStart(WriteThreadMethod));
                    if (thWrite.Name == null) thWrite.Name = "SampleDataManager-AutoWriteThread";
                    thWrite.Start();
                }
                else
                {
                    if (thWrite != null)
                    {
                        sglEnd.Set();
                        
                        while (thWrite.IsAlive)
                        {
                            System.Threading.Thread.Sleep(10);
                        }

                        sglEnd.Reset();

                        thWrite = null;
                    }
                
                }

                _AutoWriteFlag = value;

                
            }
        }

        /// <summary>
        /// 保存フォルダパス
        /// </summary>
        public string FolderPath { get; set; }

        #endregion

        #region private Valiables

        /// <summary>
        /// サンプルデータリスト(現在オンメモリのデータ)
        /// </summary>
        /// <remarks>上限10,000件</remarks>
        private List<SampleData> sampleDatas = new List<SampleData>(MAX_DATA_READ_LENGTH);

        /// <summary>
        /// 現在オンメモリデータのStartIndex
        /// </summary>
        private int startIndex = 0;
        /// <summary>
        /// 現在オンメモリデータのEndIndex
        /// </summary>
        private int endIndex = 0;

        /// <summary>
        /// ファイルストリーム
        /// </summary>
        private FileStream fs = null;

        /// <summary>
        /// ファイル操作Lockオブジェクト
        /// </summary>
        private object FileLock = new object();

        /// <summary>
        /// 自動書き込みフラグ
        /// </summary>
        private bool _AutoWriteFlag = false;

        /// <summary>
        /// ファイルフルパス
        /// </summary>
        private string filePath
        {
            get { return FolderPath + @"\" + FileName; }
        }
        
        #endregion

        #region 作業用変数
        /// <summary>
        /// 1サンプルのデータ量（モード１、３は1ショット、モード2は１サイクル）
        /// </summary>
        private int SizeofOneSample = 0;

        /// <summary>
        /// 書込み用スレッド
        /// </summary>
        private System.Threading.Thread thWrite = null;
        /// <summary>
        /// 書込み用スレッド終了signal
        /// </summary>
        private System.Threading.ManualResetEvent sglEnd = new System.Threading.ManualResetEvent(false);

        /// <summary>
        /// 書込み専用データリスト
        /// </summary>
        private List<SampleData> sampleDatastoWrite = new List<SampleData>();

        /// <summary>
        /// 書込み用ロック
        /// </summary>
        private object toWriteLock = new object();

        #endregion

        #region IList<SampleData> メンバー

        public int IndexOf(SampleData item)
        {
            return sampleDatas.IndexOf(item);
        }

        public void Insert(int index, SampleData item)
        {
            sampleDatas.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            sampleDatas.RemoveAt(index); 
        }

        public SampleData this[int index]
        {
            get
            {
                return sampleDatas[index];
            }
            set
            {
                sampleDatas[index] = value;
            }
        }

        #endregion

        #region ICollection<SampleData> メンバー

        public void Add(SampleData item)
        {
            //ヘッダにサンプル数をインクリメント
            HeaderData.SamplesCount++;


            if (AutoWriteFlag)
            {
                //書込み用にサンプルデータを保存
                //書込みは別スレッドで行う。
                lock (toWriteLock)
                {
                    sampleDatastoWrite.Add(item);
                }
            }
            else
            {
                //バッファ
                sampleDatas.Add(item);
                //終了位置を調整
                this.endIndex++;

                //書込みモードでなければオンメモリ
                // オンメモリの範囲をオーバしている分を削除する。
                if ((HeaderData.Mode == ModeType.MODE2 && sampleDatas.Count > MAX_DATA_READ_LENGTH_MODE2) ||
                    (HeaderData.Mode != ModeType.MODE2 && sampleDatas.Count > MAX_DATA_READ_LENGTH))
                {
                    sampleDatas.RemoveAt(0);

                    //開始位置を調整
                    this.startIndex++;
                }

            }


        }

        public void Clear()
        {
            sampleDatas.Clear();
        }

        public bool Contains(SampleData item)
        {
            return sampleDatas.Contains(item);
        }

        public void CopyTo(SampleData[] array, int arrayIndex)
        {
            sampleDatas.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return sampleDatas.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SampleData item)
        {
            return Remove(item);
        }

        #endregion

        #region IEnumerable<SampleData> メンバー

        public IEnumerator<SampleData> GetEnumerator()
        {
            return sampleDatas.GetEnumerator();
        }

        #endregion

        #region IEnumerable メンバー

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return sampleDatas.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// 範囲追加
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<SampleData> items)
        {
            sampleDatas.AddRange(items);

            //ヘッダにサンプル数をインクリメント
            HeaderData.SamplesCount += (UInt32)((List<SampleData>) items).Count;

            //終了位置を調整
            this.endIndex += ((List<SampleData>)items).Count;

            if (AutoWriteFlag)
            {
                //書込み用にサンプルデータを保存
                //書込みは別スレッドで行う。
                lock (toWriteLock)
                {
                    sampleDatastoWrite.AddRange(items);
                }
            }

            // オンメモリの範囲をオーバしている分を削除する。
            if ((HeaderData.Mode == ModeType.MODE2 && sampleDatas.Count > MAX_DATA_READ_LENGTH_MODE2))
            {
                //開始位置を調整
                this.startIndex += (sampleDatas.Count - MAX_DATA_READ_LENGTH_MODE2);

                sampleDatas.RemoveRange(0, sampleDatas.Count - MAX_DATA_READ_LENGTH_MODE2);
            }
            else if(HeaderData.Mode != ModeType.MODE2 && sampleDatas.Count > MAX_DATA_READ_LENGTH)
            {
                //開始位置を調整
                this.startIndex += (sampleDatas.Count - MAX_DATA_READ_LENGTH);

                sampleDatas.RemoveRange(0, sampleDatas.Count - MAX_DATA_READ_LENGTH);
            }
        }

        /// <summary>
        /// 範囲取得
        /// </summary>
        /// <param name="_startindex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remark>_startindexは0から開始。ListのGetRangeと同じイメージです。</remark>
        public List<SampleData> GetRange(int _startindex, int length)
        {
            List<SampleData> ret = null;

            lock (FileLock)
            {

                int readstartindex = 0;
                int readCount = 0;

                if (_startindex < SamplesCount)
                {
                    //現在データ範囲内にない場合
                    if (_startindex < this.startIndex || _startindex + length > this.endIndex)
                    {

                        if (length > MAX_DATA_READ_LENGTH)
                        {
                            if (SamplesCount - _startindex < MAX_DATA_READ_LENGTH)
                            {
                                readstartindex = SamplesCount - MAX_DATA_READ_LENGTH;
                            }
                            else
                            {
                                readstartindex = _startindex;
                            }

                            readCount = MAX_DATA_READ_LENGTH;
                        }
                        else
                        {
                            readstartindex = _startindex;
                            readCount = length;
                        }

                        //データ範囲読込
                        DeserializeData(readstartindex, readCount);
                    }

                    if (_startindex + length > this.endIndex)
                    {
                        length = this.endIndex - _startindex;
                    }

                    ret = new List<SampleData>();

                    foreach (SampleData sd in sampleDatas.GetRange(_startindex - this.startIndex, length))
                    {
                        if (sd != null)
                            ret.Add((SampleData)sd.Clone());
                        else
                            ret.Add(null);
                    }
                }

            }
            return ret;
        }

        /// <summary>
        /// CloseData
        /// </summary>
        public void CloseData()
        {
            if (fs != null)
                fs.Close();
        }


        #region データのシリアライズ/デシリアライズ
        
        /// <summary>
        /// 現在データを保存する（自動ルートの中で呼ばれる)
        /// </summary>
        public void SerializeData(SampleData smp)
        {
            List<byte> buffer = new List<byte>();

            //ファイルストリームのOPEN
            if (fs == null)
            {
                if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath); 
                
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                //ヘッダの書き込み
                //ファイルを初回に開いたときにヘッダ情報を書き込む
                fs.Write(HeaderData.Data, 0, SampleDataHeader.DATALENGTH);

            }
            else
            {
                //全体のサンプル数の更新
                fs.Seek(1, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(HeaderData.SamplesCount), 0, sizeof(UInt32));

                //次の書き込みのためポインタを最後に
                fs.Seek(0, SeekOrigin.End);
            }
            
            //データ保存
            switch (HeaderData.Mode)
            {
                case ModeType.MODE1:
                case ModeType.MODE3:
                    foreach (ChannelData ch in smp.ChannelDatas)
                    {
                        if (ch == null) continue;
                        if (ch.DataValues == null) continue;

                        //ChannelPosition
                        buffer.Add((byte)ch.Position);

                        if (ch.DataValues is Value_MaxMin)
                        {
                            buffer.AddRange(BitConverter.GetBytes((float)((Value_MaxMin)ch.DataValues).MaxValue));
                            buffer.AddRange(BitConverter.GetBytes((float)((Value_MaxMin)ch.DataValues).MinValue));
                        }
                        else if (ch.DataValues is Value_Standard)
                        {
                            buffer.AddRange(BitConverter.GetBytes((float)((Value_Standard)ch.DataValues).Value));
                        }
                    }
                    

                    break;

                case ModeType.MODE2:
                    foreach (ChannelData ch in smp.ChannelDatas)
                    {
                        if (ch == null) continue;

                        //初回時ならばサンプリング数を書込み
                        if (buffer.Count == 0)
                        {
                            foreach (ChannelData chdata in smp.ChannelDatas)
                            {

                                if (chdata != null && chdata.Position != 0 && chdata.DataValues != null)
                                {
                                    buffer.AddRange(BitConverter.GetBytes((UInt16)((Value_Mode2)chdata.DataValues).Values.Length));
                                    break;
                                }
                            }
                        }

                        //ChannelPosition
                        buffer.Add((byte)ch.Position);

                        // チャンネル0 回転数
                        if (ch.Position == 0)
                        {
                            buffer.AddRange(BitConverter.GetBytes((float)((Value_Standard)ch.DataValues).Value));
                        }
                        else
                        {
                            //データの埋め込み
                            foreach (decimal d in ((Value_Mode2)ch.DataValues).Values)
                            {
                                buffer.AddRange(BitConverter.GetBytes((float)d));
                            }
                        }
                    }


                    break;
            }

            //データの書き込み
            fs.Write(buffer.ToArray(), 0, buffer.Count);
            fs.Flush();
        }


        /// <summary>
        /// ヘッダ部分のみ読み込む
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public void DeserializeOnlyHeader()
        {
            byte[] buffer = new byte[10000];
            byte[] tmpsizebuff = new byte[2];
            int channelcount = 0;

            try
            {
                lock (FileLock)
                {
                    //ファイルストリームのOPEN
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }

                    if (File.Exists(filePath))
                    {
                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    }
                    else
                    {
                        //ファイル読み込みエラー
                        throw new Exception(CommonResource.GetString("ERROR_FILE_NOT_FOUND"));
                    }

                    //ヘッダデータの読込
                    if (HeaderData == null)
                    {
                        fs.Read(buffer, 0, SampleDataHeader.DATALENGTH);

                        HeaderData = new SampleDataHeader();
                        HeaderData.Data = (new List<byte>(buffer)).GetRange(0, SampleDataHeader.DATALENGTH).ToArray();
                    }

                    //レコードサイズ
                    SizeofOneSample = HeaderData.SizeofOneSample;

                    //チャンネル数
                    channelcount = HeaderData.ChannelCount;
                }
            }
            finally
            {
                fs.Close();
                fs = null;
            }
        }

        /// <summary>
        /// 範囲分のデータを読み込む
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public void DeserializeData(int startIndex, int length)
        {
            byte[] buffer = new byte[10000];
            byte[] tmpsizebuff = new byte[2];
            UInt16 samplecount = 0;
            int channelcount = 0;


            //ファイルストリームのOPEN
            if (fs == null)
            {
                if (File.Exists(filePath))
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    //ファイル読み込みエラー
                    throw new Exception(CommonResource.GetString("ERROR_FILE_NOT_FOUND"));
                }
            }

            //ヘッダデータの読込
            if (HeaderData == null)
            {
                fs.Read(buffer, 0, SampleDataHeader.DATALENGTH);

                HeaderData = new SampleDataHeader();
                HeaderData.Data = (new List<byte>(buffer)).GetRange(0, SampleDataHeader.DATALENGTH).ToArray();
            }

            //レコードサイズ
            SizeofOneSample = HeaderData.SizeofOneSample;

            //チャンネル数
            channelcount = HeaderData.ChannelCount;

            //データのクリア
            sampleDatas.Clear();

            #region スタート位置の調整
            //データ読込　スタート位置の調整
            int pos = SampleDataHeader.DATALENGTH;
            switch (HeaderData.Mode)
            {
                case ModeType.MODE1:
                case ModeType.MODE3:

                    //サンプルサイズ(＝レコードサイズ + ChPosition(チャンネル分)）から始まり位置を取得
                    pos += startIndex * (SizeofOneSample + channelcount);

                    //開始位置に移動
                    fs.Seek(pos, SeekOrigin.Begin);

                    break;
                case ModeType.MODE2:

                    //開始位置に移動
                    fs.Seek(pos, SeekOrigin.Begin);


                    //サンプルサイズから1つずつのレコードサイズを取得しながら
                    //ずらしていく
                    for (int i = 0; i < startIndex; i++)
                    {
                        fs.Read(tmpsizebuff, 0, 2);

                        samplecount = BitConverter.ToUInt16(tmpsizebuff, 0);

                        //pos += 2 + (samplecount * SizeofOneSample);

                        //回転数は各1ショット毎に１つなので下記式となる
                        //回転数 + ChPosition(チャンネル分+回転数分) +  ( サンプル数 * (1レコードサイズ - 回転数分)  )
                        //fs.Read(buffer, 0, ONE_DATA_SIZE + channelcount + (samplecount * (SizeofOneSample - ONE_DATA_SIZE)));
                        fs.Seek(ONE_DATA_SIZE + channelcount + (samplecount * (SizeofOneSample - ONE_DATA_SIZE)), SeekOrigin.Current);
                        //fs.Seek(channelcount + (samplecount * SizeofOneSample), SeekOrigin.Current);
                    }

                    break;
            }

            #endregion

            #region データ取得
            if (length > HeaderData.SamplesCount)
                length = (int)HeaderData.SamplesCount;

            //データを取ってくる
            for (int i = 0; i < length; i++)
            {
                SampleData smp = new SampleData();
                smp.ChannelDatas = new ChannelData[CHANNEL_COUNT_MAX];

                int offset = 0;

                switch (HeaderData.Mode)
                {
                    case ModeType.MODE1:
                    case ModeType.MODE3:
                        #region モード１・モード３
                        fs.Read(buffer, 0, SizeofOneSample + channelcount);

                        for (int channelNo = 0; channelNo < CHANNEL_COUNT_MAX; channelNo++)
                        {
                            switch (this.HeaderData.ChannelsDataType[channelNo])
                            {
                                case SampleDataHeader.CHANNELDATATYPE.NONE:
                                    continue;

                                case SampleDataHeader.CHANNELDATATYPE.SINGLEDATA:
                                    smp.ChannelDatas[channelNo] = new ChannelData();
                                    smp.ChannelDatas[channelNo].DataValues = new Value_Standard();

                                    smp.ChannelDatas[channelNo].Position = buffer[offset];
                                    offset++;

                                    ((Value_Standard)smp.ChannelDatas[channelNo].DataValues).Value = (decimal)BitConverter.ToSingle(buffer, offset);
                                    offset += ONE_DATA_SIZE;
                                    break;
                                case SampleDataHeader.CHANNELDATATYPE.DOUBLEDATA:
                                    smp.ChannelDatas[channelNo] = new ChannelData();
                                    smp.ChannelDatas[channelNo].DataValues = new Value_MaxMin();

                                    smp.ChannelDatas[channelNo].Position = buffer[offset];
                                    offset++;

                                    ((Value_MaxMin)smp.ChannelDatas[channelNo].DataValues).MaxValue = (decimal)BitConverter.ToSingle(buffer, offset);
                                    offset += ONE_DATA_SIZE;
                                    ((Value_MaxMin)smp.ChannelDatas[channelNo].DataValues).MinValue = (decimal)BitConverter.ToSingle(buffer, offset);
                                    offset += ONE_DATA_SIZE;
                                    break;

                            }
                        }
                        #endregion

                        break;

                    case ModeType.MODE2:
                        #region モード2
                        for (int channelNo = 0; channelNo < CHANNEL_COUNT_MAX; channelNo++)
                        {
                            switch (this.HeaderData.ChannelsDataType[channelNo])
                            {
                                case SampleDataHeader.CHANNELDATATYPE.NONE:
                                    continue;

                                case SampleDataHeader.CHANNELDATATYPE.SINGLEDATA:

                                    smp.ChannelDatas[channelNo] = new ChannelData();

                                    if (channelNo == 0)
                                    {
                                        //サンプル数の読込
                                        fs.Read(tmpsizebuff, 0, 2);
                                        samplecount = BitConverter.ToUInt16(tmpsizebuff, 0);

                                        //チャンネル番号の読込
                                        fs.Read(tmpsizebuff, 0, 1);
                                        smp.ChannelDatas[channelNo].Position = tmpsizebuff[0];

                                        smp.ChannelDatas[channelNo].DataValues = new Value_Standard();

                                        //回転数の取得
                                        fs.Read(buffer, 0, ONE_DATA_SIZE);

                                        ((Value_Standard)smp.ChannelDatas[channelNo].DataValues).Value = (decimal)BitConverter.ToSingle(buffer, 0);
                                    }
                                    else
                                    {
                                        smp.ChannelDatas[channelNo].DataValues = new Value_Mode2();

                                        //チャンネル番号の読込
                                        fs.Read(tmpsizebuff, 0, 1);
                                        smp.ChannelDatas[channelNo].Position = tmpsizebuff[0];

                                        ((Value_Mode2)smp.ChannelDatas[channelNo].DataValues).Values = new decimal[samplecount];

                                        for (int sampleIndex = 0; sampleIndex < samplecount; sampleIndex++)
                                        {
                                            //データの取得（サンプルデータ分 = 1ショット分の１Chデータ）
                                            fs.Read(buffer, 0, ONE_DATA_SIZE);
                                            //offset = 0;
                                            ((Value_Mode2)smp.ChannelDatas[channelNo].DataValues).Values[sampleIndex] = (decimal)BitConverter.ToSingle(buffer, 0);
                                            //offset += ONE_DATA_SIZE;
                                        }
                                    }
                                    break;
                                case SampleDataHeader.CHANNELDATATYPE.DOUBLEDATA:
                                    //存在しない。
                                    break;
                            }
                        }
                        #endregion

                        break;
                }

                //読込データをオンメモリする。
                sampleDatas.Add(smp);
            }

            #endregion

            //オンメモリのデータのインデックス設定
            this.startIndex = startIndex;
            this.endIndex = startIndex + length;

        }
        
        #endregion

        /// <summary>
        /// ファイル書込み用スレッド
        /// </summary>
        private void WriteThreadMethod()
        {
            List<SampleData> tmpSamples = null;

            int tmpCount = 0;

            //Write終了指示がない、または、書込み待ちがあればループ
            //つまり、終了指示があっても、書き込み待ちがあれば書ききる。
            while (!sglEnd.WaitOne(0) || sampleDatastoWrite.Count != 0)
            {

                if (sampleDatastoWrite.Count != 0)
                {
                    lock (toWriteLock)
                    {
                        //書込み対象を取得
                        tmpSamples = sampleDatastoWrite.GetRange(0, (sampleDatastoWrite.Count < MAX_DATA_WRITE_AT_ONCE ? sampleDatastoWrite.Count : MAX_DATA_WRITE_AT_ONCE));
                        //取得した分をクリア
                        sampleDatastoWrite.RemoveRange(0, (sampleDatastoWrite.Count < MAX_DATA_WRITE_AT_ONCE ? sampleDatastoWrite.Count : MAX_DATA_WRITE_AT_ONCE));
                    }

                    //たまっている分Write
                    foreach (SampleData smp in tmpSamples)
                    {
                        SerializeData(smp);
                        tmpCount++;

                        if (tmpCount >= 1000)
                        {
                            fs.Flush(true);
                            System.Threading.Thread.Sleep(1);
                            GC.Collect();
                            tmpCount = 0;
                        }
                    }

                    tmpSamples = null;
                    
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
           
            if (fs != null)
            {
                //ファイル閉じ
                fs.Close();
                GC.Collect();

            }

        }

        #region ICloneable メンバー

        public object Clone()
        {
            SampleDataManager ret = new SampleDataManager();

            ret.HeaderData = (SampleDataHeader)this.HeaderData.Clone();
            ret.AutoWriteFlag = this.AutoWriteFlag;
            ret.FolderPath = this.FolderPath;

            if(this.sampleDatas != null)
                for (int i = 0; i < this.sampleDatas.Count; i++)
                {
                    ret.sampleDatas.Add((SampleData)this.sampleDatas[i].Clone());
                }

            ret.startIndex = this.startIndex;
            ret.endIndex = this.endIndex;

            return ret;
        }
        #endregion
    }
}
