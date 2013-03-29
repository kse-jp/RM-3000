using System;
using System.Xml;
using System.Xml.Serialization;

namespace DataCommon
{
    /// <summary>
    /// 基本設定
    /// </summary>
    [Serializable]
    public class TestData : DataClassBase
    {
        #region private member
        /// <summary>
        /// 位置番号
        /// </summary>
        private int position = 0;
        #endregion

        #region public member
        /// <summary>
        /// 位置番号
        /// 0～10
        /// </summary>
        /// <remarks>0は回転数固定</remarks>
        public int Position
        {
            set
            {
                if (value < 0 || value > 10)
                { throw new Exception(string.Format("Position {0} {1}", AppResource.GetString("ERROR_VALUE_OUT_OF_RANGE"), "0~10")); }
                position = value;
            }
            get { return position; }
        }
        /// <summary>
        /// ショット値
        /// </summary>
        /// <remarks>回転数はモード2時も標準値</remarks>
        public DataValue DataValues { set; get; }
        #endregion

        #region constructor
        public TestData()
        {
            typeOfClass = TYPEOFCLASS.TestData;
        }
        #endregion

        #region private method
        #endregion

        #region public method
        /// <summary>
        /// data string output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = string.Format("TestData - Position={0},DataValues={1}", position, DataValues);
            return s;
        }
        #endregion
    }
}
