using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommon
{
    public class TagData
    {

        #region private member
        /// <summary>
        /// 項目番号
        /// </summary>
        private int tagNo = -1;
        #endregion

        #region public member
        /// <summary>
        /// 項目番号
        /// </summary>
        public int TagNo
        {
            set
            {
                tagNo = value;
            }
            get { return tagNo; }
        }
        /// <summary>
        /// データ
        /// </summary>
        public DataValue DataValues { set; get; }
        #endregion

        #region constructor
        public TagData()
        {
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
            string s = string.Format("TagData - TagNo={0},DataValues={1}", tagNo, DataValues);
            return s;
        }
        #endregion

    }
}
