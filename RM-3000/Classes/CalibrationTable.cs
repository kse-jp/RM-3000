using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM_3000
{
    /// <summary>
    /// 検量線情報
    /// </summary>
    public class CalibrationTable
    {
        /// <summary>
        /// チャンネルNo
        /// </summary>
        public int ChannnelNo { get; set; }

        /// <summary>
        /// 算出用検量線
        /// </summary>
        public CalibrationCurveList Calc_CalibrationCurveList { get; set; }

        /// <summary>
        /// 基準検量線
        /// </summary>
        public CalibrationCurve Base_CalibrationCurve { get; set; }

        /// <summary>
        /// 基準温度データ
        /// </summary>
        public List<CalibrationCurve> Base_TempCalibrationCurveList { get; set; }

        
        /// <summary>
        ///　基準となるベース情報からテーブルを生成する。
        /// </summary>
        public bool CreateCalibrationTable()
        {
            try
            {
                //枠の作成
                if (Calc_CalibrationCurveList == null)
                    Calc_CalibrationCurveList = new CalibrationCurveList();
                else
                    Calc_CalibrationCurveList.Clear();

                //温度補償データのデータを基準検量線に適応する
                foreach (CalibrationCurve cc in Base_TempCalibrationCurveList)
                {
                    //不一致ならば次へ
                    if (cc.TempData != Base_CalibrationCurve.TempData) continue;

                    //一致しているのでコピー対象
                    foreach (CalibrationCurvePoint cp in cc)
                    {
                        int index = Base_CalibrationCurve.FindIndex((x) => x.FarData == cp.FarData);

                        if (index == -1)
                            Base_CalibrationCurve.Add(cp);
                        else
                            Base_CalibrationCurve[index].OutData = cp.OutData;
                    }

                    //ソート　距離で昇順
                    Base_CalibrationCurve.Sort(new System.Comparison<CalibrationCurvePoint>((x, y) => (int)(x.FarData - y.FarData)));

                    //System.Diagnostics.Debug.Print(Base_CalibrationCurve.ToString());
                }


                //0.0～50.0℃までの検量線格納枠の作成
                for (int tempIndex = 0; tempIndex <= 500; tempIndex++)
                {
                    Calc_CalibrationCurveList.Add(new CalibrationCurve());
                    Calc_CalibrationCurveList[tempIndex].TempData = tempIndex;

                    foreach (CalibrationCurvePoint ccp in Base_CalibrationCurve)
                    {
                        CalibrationCurvePoint tmpccp = new CalibrationCurvePoint();

                        tmpccp.OutData = ccp.OutData;
                        tmpccp.FarData = ccp.FarData;

                        Calc_CalibrationCurveList[tempIndex].Add(tmpccp);

                    }
                }

                #region 温度基準データの存在する距離に対し、全温度時の出力値を算出
                //温度位置からその距離における温度差分線を作成

                //係数
                List<CoefCondition> Coefs = new List<CoefCondition>();

                //登録距離分ループ
                for (int farIndex = 0; farIndex < Base_TempCalibrationCurveList[0].Count; farIndex++)
                {
                    //温度分ループ
                    for (int basetempIndex = 0; basetempIndex + 1 < Base_TempCalibrationCurveList.Count; basetempIndex++)
                    {
                        CalibrationCurve bcc1 = Base_TempCalibrationCurveList[basetempIndex];
                        CalibrationCurve bcc2 = Base_TempCalibrationCurveList[basetempIndex + 1];

                        //温度差の係数算出
                        Coefs.Add(new CoefCondition()
                                {
                                    Coef = (double)((bcc1[farIndex].OutData - bcc2[farIndex].OutData) / (bcc1.TempData - bcc2.TempData)),
                                    MaxOutput = bcc2[farIndex].OutData,
                                    MinOutput = bcc1[farIndex].OutData,
                                    MaxRange = bcc2.TempData,
                                    MinRange = bcc1.TempData,
                                    targetValue = bcc1[farIndex].FarData
                                }
                            );
                    }
                }

                //算出
                for (int coefIndex = 0; coefIndex < Coefs.Count; coefIndex++)
                {
                    //ターゲット距離のIndexを取得
                    int targetIndex = Base_CalibrationCurve.FindIndex((x) => x.FarData == Coefs[coefIndex].targetValue);

                    //検量線分ループ
                    for (int CalcIndex = 0; CalcIndex < Calc_CalibrationCurveList.Count; CalcIndex++)
                    {
                        CalibrationCurve cc = Calc_CalibrationCurveList[CalcIndex];

                        //最小値と一致ならば入れ込み
                        if (cc.TempData == Coefs[coefIndex].MinRange)
                        {
                            cc[targetIndex].OutData = Coefs[coefIndex].MinOutput;
                        }
                        //最大値と一致ならば入れ込み
                        else if (cc.TempData == Coefs[coefIndex].MaxRange)
                        {
                            cc[targetIndex].OutData = Coefs[coefIndex].MaxOutput;
                        }
                        //範囲内
                        else if (cc.TempData > Coefs[coefIndex].MinRange && cc.TempData < Coefs[coefIndex].MaxRange)
                        {
                            cc[targetIndex].OutData = Coefs[coefIndex].CalcOutput(cc.TempData);
                        }
                        //範囲外で最初の係数
                        else if (cc.TempData < Coefs[coefIndex].MinRange &&
                            (coefIndex == 0 || coefIndex != 0 && Coefs[coefIndex].targetValue != Coefs[coefIndex - 1].targetValue))
                        {
                            cc[targetIndex].OutData = Coefs[coefIndex].CalcOutput(cc.TempData);
                        }
                        //範囲外で最後の係数
                        else if (cc.TempData > Coefs[coefIndex].MaxRange &&
                            (coefIndex == Coefs.Count - 1 || coefIndex != Coefs.Count - 1 && Coefs[coefIndex].targetValue != Coefs[coefIndex + 1].targetValue))
                        {
                            cc[targetIndex].OutData = Coefs[coefIndex].CalcOutput(cc.TempData);
                        }
                        //それ以外ならば次の範囲へ
                        else
                        {
                            //次の範囲が別ターゲットならば抜ける
                            if (Coefs[coefIndex].targetValue != Coefs[coefIndex + 1].targetValue)
                            {
                                break;
                            }
                            else
                            {
                                coefIndex++;
                                CalcIndex--;
                            }
                        }
                    }

                }

                #endregion

                #region 検量線基準値の比率をもとに、各温度時の検量線を仕上げる

                //算術基準ターゲットの取得
                List<decimal> targetList = new List<decimal>();
                foreach (CoefCondition coef in Coefs)
                {
                    if (targetList.Count == 0)
                        targetList.Add(coef.targetValue);
                    else if (targetList[targetList.Count - 1] != coef.targetValue)
                        targetList.Add(coef.targetValue);
                }



                //検量線分ループ
                for (int CalcIndex = 0; CalcIndex < Calc_CalibrationCurveList.Count; CalcIndex++)
                {
                    CalibrationCurve cc = Calc_CalibrationCurveList[CalcIndex];

                    for (int targetIndex = 0; targetIndex + 1 < targetList.Count; targetIndex++)
                    {
                        //対象の距離
                        int MinFarIndex = Base_CalibrationCurve.FindIndex((x) => x.FarData == targetList[targetIndex]);
                        int MaxFarIndex = Base_CalibrationCurve.FindIndex((x) => x.FarData == targetList[targetIndex + 1]);

                        //範囲分の差分値
                        UInt16 BaseDiff = (UInt16)(Base_CalibrationCurve[MaxFarIndex].OutData - Base_CalibrationCurve[MinFarIndex].OutData);
                        UInt16 DiffValue = (UInt16)(cc[MaxFarIndex].OutData - cc[MinFarIndex].OutData);

                        //各値を埋める
                        for (int farIndex = MinFarIndex + 1; farIndex < MaxFarIndex; farIndex++)
                        {
                            cc[farIndex].OutData = (UInt16)(cc[MinFarIndex].OutData + ((Base_CalibrationCurve[farIndex].OutData - Base_CalibrationCurve[MinFarIndex].OutData) * ((double)DiffValue / (double)BaseDiff)));
                        }

                        // 温度補償範囲外の埋め込みが必要な場合
                        // Δ0の距離が対象
                        if (targetIndex == 0 && MinFarIndex != 0)
                        {
                            for (int farIndex = MinFarIndex - 1; farIndex >= 0; farIndex--)
                            {
                                BaseDiff = Base_CalibrationCurve[MinFarIndex].OutData;
                                DiffValue = cc[MinFarIndex].OutData;

                                cc[farIndex].OutData = (UInt16)(cc[MinFarIndex].OutData + ((Base_CalibrationCurve[farIndex].OutData - Base_CalibrationCurve[MinFarIndex].OutData) * ((double)DiffValue / (double)BaseDiff)));
                            }
                        }
                        //Δ∞の距離が対象
                        //係数はそのままで演算
                        else if (targetIndex == targetList.Count - 2 && MaxFarIndex != cc.Count - 1)
                        {
                            for (int farIndex = MaxFarIndex + 1; farIndex < cc.Count; farIndex++)
                            {
                                cc[farIndex].OutData = (UInt16)(cc[MaxFarIndex].OutData + ((Base_CalibrationCurve[farIndex].OutData - Base_CalibrationCurve[MaxFarIndex].OutData) * ((double)DiffValue / (double)BaseDiff)));
                            }
                        }
                    }
                }

                #endregion
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 算出
        /// </summary>
        /// <param name="srcValule">元値</param>
        /// <param name="MaxValue">最大振幅データ</param>
        /// <param name="tempData">温度データ</param>
        /// <returns></returns>
        public decimal Calc(decimal srcValue, decimal MaxValue, int tempData = -1)
        {
            decimal ret = -1;

            //温度データがない場合は、最大振幅補正をする。
            if (tempData == -1)
            {
                srcValue = srcValue * Base_CalibrationCurve[Base_CalibrationCurve.Count-1].OutData / MaxValue;

                tempData = (int)Base_CalibrationCurve.TempData;
                
                //for (int tempIndex = 0; tempIndex + 1 < Calc_CalibrationCurveList.Count; tempIndex++)
                //{
                //    //一度ずつ設定なので、最大振幅データが範囲内に入っていれば範囲下限値を現在温度とする。
                //    if (MaxValue >= Calc_CalibrationCurveList[tempIndex][Calc_CalibrationCurveList[tempIndex].Count - 1].OutData &&
                //        MaxValue < Calc_CalibrationCurveList[tempIndex + 1][Calc_CalibrationCurveList[tempIndex + 1].Count - 1].OutData)
                //    {
                //        tempData = tempIndex;
                //    }

                //}
            }

            if (tempData > this.Calc_CalibrationCurveList.Count || tempData == -1)
            {
                ret = 0;
            }
            else
            {   
                //温度データから対象の検量線を取得
                CalibrationCurve cc = this.Calc_CalibrationCurveList[tempData];

                if (cc[0].OutData > srcValue)
                    ret = 0;
                else
                {

                    //2分探索
                    int farIndex = -1;
                    try
                    {
                        farIndex = BinarySearchforFarIndex(cc, 0, cc.Count - 1, srcValue);

                        if (farIndex != -1)
                        {
                            ret = ((cc[farIndex + 1].FarData - cc[farIndex].FarData) / (cc[farIndex + 1].OutData - cc[farIndex].OutData)) * (srcValue - cc[farIndex].OutData) + cc[farIndex].FarData;
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }

                    //for (int farIndex = 0; farIndex + 1 < cc.Count; farIndex++)
                    //{
                    //    if (cc[farIndex].OutData <= srcValule && cc[farIndex + 1].OutData > srcValule)
                    //    {
                    //        ret = ((cc[farIndex + 1].FarData - cc[farIndex].FarData) / (cc[farIndex + 1].OutData - cc[farIndex].OutData)) * (srcValule - cc[farIndex].OutData) + cc[farIndex].FarData;
                    //        break;
                    //    }
                    //}
                }

                //範囲外ならば最大値
                if (ret == -1)
                {
                    ret = cc[cc.Count - 1].FarData;

                    //TODO:後で閾値のデータを出す。
                }
            }
            return ret;
        }

        /// <summary>
        /// 最大振幅温度取得用2分探索
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="srcValule"></param>
        /// <returns>Index、-1で未発見</returns>
        public int BinarySearchforTempIndex(CalibrationCurveList cc, int startIndex, int endIndex, decimal srcValule)
        {
            int tempIndex = (startIndex + endIndex) / 2;

            if (tempIndex + 1 >= cc.Count)
            {
                return -1;
            }

            if (srcValule >= cc[tempIndex][Calc_CalibrationCurveList[tempIndex].Count - 1].OutData &&
                        srcValule < cc[tempIndex + 1][Calc_CalibrationCurveList[tempIndex + 1].Count - 1].OutData)
            {
                return tempIndex;
            }
            else if (cc[tempIndex][Calc_CalibrationCurveList[tempIndex].Count - 1].OutData > srcValule)
            {
                endIndex = tempIndex - 1;
            }
            else
            {
                startIndex = tempIndex + 1;
            }

            if (startIndex > endIndex)
                return -1;

            return BinarySearchforTempIndex(cc, startIndex, endIndex, srcValule);
        }


        /// <summary>
        /// 検量線2分探索
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="srcValule"></param>
        /// <returns>Index、-1で未発見</returns>
        public int BinarySearchforFarIndex(CalibrationCurve cc, int startIndex, int endIndex, decimal srcValule)
        {
            int farIndex = (startIndex + endIndex) / 2;

            if (farIndex + 1 >= cc.Count)
            {
                return -1;
            }


            if (cc[farIndex].OutData <= srcValule && cc[farIndex + 1].OutData > srcValule)
            {
                return farIndex;
            }
            else if (cc[farIndex].OutData > srcValule)
            {
                endIndex = farIndex - 1;
            }
            else
            {
                startIndex = farIndex + 1;
            }

            if (startIndex > endIndex)
                return -1;

            return BinarySearchforFarIndex(cc, startIndex, endIndex, srcValule);
        }

    }

    /// <summary>
    /// 検量線ポイントクラス
    /// </summary>
    public class CalibrationCurvePoint
    {
        public decimal FarData　{ get;set; }
        public UInt16 OutData { get; set; }
    }

    /// <summary>
    /// 検量線クラス
    /// </summary>
    public class CalibrationCurve : List<CalibrationCurvePoint>
    {
        public decimal TempData { get; set; }

        /// <summary>
        /// コピーを作成する
        /// </summary>
        /// <returns></returns>
        public CalibrationCurve GetCopy()
        {
            CalibrationCurve ret = new CalibrationCurve();

            ret.TempData = this.TempData;

            ret.AddRange(new List<CalibrationCurvePoint>(this.ToList()));

            return ret;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("温度={0}, ",this.TempData);

            foreach (CalibrationCurvePoint cc in this)
            {
                sb.AppendFormat("{0},{1},", cc.FarData, cc.OutData);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 距離リスト CSV出力用文字列
        /// </summary>
        /// <returns></returns>
        public string GetFarListCsvString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (CalibrationCurvePoint cc in this)
            {
                sb.AppendFormat("{0},", cc.FarData);
            }

            return sb.ToString().Substring(0,sb.Length - 1);
        }

        /// <summary>
        /// 出力リスト　CSV出力用文字列
        /// </summary>
        /// <returns></returns>
        public string GetOutputListCsvString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (CalibrationCurvePoint cc in this)
            {
                sb.AppendFormat("{0},", cc.OutData);
            }

            return sb.ToString();
        }


    }

    /// <summary>
    /// 検量線リストクラス
    /// </summary>
    public class CalibrationCurveList : List<CalibrationCurve>
    {
        public CalibrationCurve SearchCalibrationCurveList(decimal temp_Value)
        {
            foreach(CalibrationCurve cc in this)
            {
                if(cc.TempData == temp_Value)
                {
                    return cc;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 条件・係数クラス
    /// </summary>
    public class CoefCondition
    {
        /// <summary>
        /// 係数
        /// </summary>
        public double Coef {get;set;}
        /// <summary>
        /// 最小値
        /// </summary>
        public UInt16　MinOutput { get; set; }
        /// <summary>
        /// 最大値
        /// </summary>
        public UInt16  MaxOutput { get; set; }
        /// <summary>
        /// 範囲最小値
        /// </summary>
        public decimal MinRange { get; set; }
        /// <summary>
        /// 範囲最大値
        /// </summary>
        public decimal MaxRange { get; set; }
        /// <summary>
        /// 指定位置
        /// </summary>
        public decimal targetValue { get; set; }

        public UInt16 CalcOutput(decimal targetsrc)
        {
            return (UInt16)(MinOutput + ((double)(targetsrc - MinRange) * Coef));
        }

    }
}
