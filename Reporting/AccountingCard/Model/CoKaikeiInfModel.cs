using Entity.Tenant;
using Helper.Common;
using Helper.Constants;

namespace Reporting.AccountingCard.Model
{
    public class CoKaikeiInfModel
    {
        public KaikeiInf KaikeiInf { get; set; }
        public List<KaikeiDetail> KaikeiDtls { get; set; }
        public PtInf PtInf { get; set; }
        public PtHokenInf PtHokenInf { get; set; }
        public HokenMst HokenMst { get; set; }
        public List<CoPtKohiModel> PtKohis { get; set; }

        /// <summary>
        /// 会計情報
        /// </summary>
        /// <param name="kaikeiInf"></param>
        /// <param name="ptHokenInf"></param>
        /// <param name="hokenMst"></param>
        /// <param name="ptKohis"></param>
        public CoKaikeiInfModel(
            KaikeiInf kaikeiInf, List<KaikeiDetail> kaikeiDtls, PtInf ptInf, PtHokenInf ptHokenInf, HokenMst hokenMst, List<CoPtKohiModel> ptKohis)
        {
            KaikeiInf = kaikeiInf;
            KaikeiDtls = kaikeiDtls;
            PtInf = ptInf;
            PtHokenInf = ptHokenInf;
            HokenMst = hokenMst;
            PtKohis = ptKohis;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get => KaikeiInf.HpId;
        }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => KaikeiInf.PtId;
        }

        /// <summary>
        /// 保険種別区分
		///		0:保険なし 
		///		1:主保険   
		///		2:マル長   
		///		3:労災  
		///		4:自賠
		///		5:生活保護 
		///		6:分点公費
		///		7:一般公費  
		///		8:自費
		/// </summary>
        public int HokenSbtKbn
        {
            get => HokenMst.HokenSbtKbn;
        }

        /// <summary>
        /// 保険区分
        ///		0:自費 
        ///		1:社保 
        ///		2:国保
        ///		
        ///		11:労災(短期給付)
        ///		12:労災(傷病年金)
        ///		13:アフターケア
        ///		14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get => PtHokenInf.HokenKbn;
        }
        /// <summary>
        /// 法別番号
        /// </summary>
        public string Houbetu
        {
            get => PtHokenInf.Houbetu ?? string.Empty;
        }

        public string HokensyaNo
        {
            get => PtHokenInf.HokensyaNo ?? string.Empty;
        }
        public string Kigo
        {
            get => PtHokenInf.Kigo ?? string.Empty;
        }
        public string Bango
        {
            get => PtHokenInf.Bango ?? string.Empty;
        }
        public string EdaNo
        {
            get
            {
                string ret = "";

                if (!string.IsNullOrEmpty(PtHokenInf.EdaNo))
                {
                    ret = PtHokenInf.EdaNo.PadLeft(2, '0');
                }

                return ret;
            }
        }
        /// <summary>
        /// 記号番号
        /// </summary>
        public string KigoBango
        {
            get
            {
                return PtHokenInf.Kigo + "／" + PtHokenInf.Bango;
            }
        }
        /// <summary>
        /// 労災の交付番号
        /// </summary>
        public string RousaiKofuNo
        {
            get
            {
                return PtHokenInf.RousaiKofuNo ?? string.Empty;
            }
        }
        /// <summary>
        /// 労災事業所名
        /// </summary>
        public string RousaiJigyosyoName
        {
            get
            {
                return PtHokenInf.RousaiJigyosyoName ?? string.Empty;
            }
        }
        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get
            {
                return PtHokenInf.JibaiHokenName ?? string.Empty;
            }
        }
        /// <summary>
        /// 自賠担当者名
        /// </summary>
        public string JibaiTanto
        {
            get
            {
                return PtHokenInf.JibaiHokenTanto ?? string.Empty;
            }
        }
        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get => KaikeiInf.SinDate;
        }
        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get => KaikeiInf.RaiinNo;
        }
        /// <summary>
        /// 使用している公費の数
        /// </summary>
        public int KohiCount
        {
            get
            {
                if (PtKohis == null || !PtKohis.Any())
                {
                    return 0;
                }
                else
                {
                    return PtKohis.Count(p => new int[] { 5, 6, 7 }.Contains(p.HokenMst.HokenSbtKbn));
                }
            }
        }

        /// <summary>
        /// 保険の種類
        /// </summary>
        public string HokenSyu
        {
            get
            {
                #region sub method
                string _getHeiyo()
                {
                    string heiyo = "";
                    switch (KohiCount)
                    {
                        case 0:
                            heiyo += "単独";
                            break;
                        case 1:
                            heiyo = "２者併用";
                            break;
                        case 2:
                            heiyo = "３者併用";
                            break;
                        case 3:
                            heiyo = "４者併用";
                            break;
                        case 4:
                            heiyo = "５者併用";
                            break;
                    }
                    return heiyo;
                }

                string _getKohiHeiyo()
                {
                    string heiyo = "";
                    switch (KohiCount)
                    {
                        case 1:
                            heiyo = "公費単独";
                            break;
                        case 2:
                            heiyo = "公費併用";
                            break;
                        case 3:
                            heiyo = "公費３者併用";
                            break;
                        case 4:
                            heiyo = "公費４者併用";
                            break;
                    }
                    return heiyo;
                }

                string _getHonke()
                {
                    string honke = "・本人";
                    if (PtHokenInf.HonkeKbn == 2)
                    {
                        honke = "・家族";
                    }
                    return honke;
                }
                #endregion

                string ret = "";

                if (HokenMst.HokenSbtKbn == 0)
                {
                    ret = _getKohiHeiyo();
                }
                else if (HokenMst.HokenSbtKbn == 9)
                {
                    ret = "自レ";
                }
                else if (new int[] { 11, 12, 13 }.Contains(PtHokenInf.HokenKbn))
                {
                    ret = "労災";
                }
                else if (PtHokenInf.HokenKbn == 14)
                {
                    ret = "自賠";
                }
                else
                {
                    string honke = "";

                    if (PtHokenInf.HokenKbn == 1)
                    {
                        if (CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 70))
                        {
                            ret = "高齢";
                        }
                        else
                        {
                            ret = "社保";
                            honke = _getHonke();
                        }
                        ret += _getHeiyo();
                    }
                    else if (PtHokenInf.HokenKbn == 2)
                    {
                        if (PtHokenInf.Houbetu == "39")
                        {
                            ret = "後期";
                        }
                        else if (CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 70))
                        {
                            ret = "高齢";
                        }
                        else if (PtHokenInf.Houbetu == "67")
                        {
                            ret = "退職";
                            honke = _getHonke();
                        }
                        else
                        {
                            ret = "国保";
                            honke = _getHonke();
                        }

                        ret += _getHeiyo();
                    }

                    ret += honke;
                }

                return ret;
            }
        }

        /// <summary>
        /// 主保険の負担率
        /// </summary>
        public int HokenRate
        {
            get => KaikeiInf.HokenRate;
        }
        /// <summary>
        /// 負担率
        /// </summary>
        public int Rate
        {
            get => KaikeiInf.PtRate;
        }
        /// <summary>
        /// 高額区分
        /// </summary>
        public int KogakuKbn
        {
            get => PtHokenInf.KogakuKbn;
        }
        /// <summary>
        /// 負担率(%)
        /// </summary>
        public int? FutanRate
        {
            get
            {
                int? ret = null;

                if (PtHokenInf != null)
                {
                    if (new int[] { 1, 2 }.Contains(PtHokenInf.HokenKbn))
                    {

                        ret = GetHokenRate(PtHokenInf.Rate, HokenMst.HokenSbtKbn, PtHokenInf.KogakuKbn, PtHokenInf.Houbetu ?? string.Empty);

                    }
                    else if (PtHokenInf.HokenKbn == 0)
                    {
                        // 自費
                        ret = 100;
                    }
                }

                if (PtKohis != null)
                {
                    for (int i = 0; i < PtKohis.Count; i++)
                    {
                        if (PtKohis[i].HokenMst.FutanKbn == 0)
                        {
                            ret = 0;
                        }
                        else if (ret == null || ret > PtKohis[i].FutanRate)
                        {
                            ret = PtKohis[i].FutanRate;
                        }
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 主保険負担率計算
        /// </summary>
        /// <param name="futanRate">負担率</param>
        /// <param name="hokenSbtKbn">保険種別区分</param>
        /// <param name="kogakuKbn">高額療養費区分</param>
        /// <param name="houbetu">法別番号</param>
        /// <returns></returns>
        private int GetHokenRate(int futanRate, int hokenSbtKbn, int kogakuKbn, string houbetu)
        {
            int wrkRate = futanRate;

            switch (hokenSbtKbn)
            {
                case 0:
                    //主保険なし
                    break;
                case 1:
                    //主保険
                    if (IsPreSchool())
                    {
                        //６歳未満未就学児
                        wrkRate = 20;
                    }
                    else if (IsElder() && houbetu != "39")
                    {
                        wrkRate =
                            IsElder20per() ? 20 :  //前期高齢
                            IsElderExpat() ? 20 :  //75歳以上海外居住者
                             10;
                    }

                    if (IsElder() || houbetu == "39")
                    {
                        if ((kogakuKbn == 3 && KaikeiInf.SinDate < KaiseiDate.d20180801) ||
                            (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && KaikeiInf.SinDate >= KaiseiDate.d20180801))
                        {
                            //後期７割 or 高齢７割
                            wrkRate = 30;
                        }
                        else if (houbetu == "39" && kogakuKbn == 41 &&
                            KaikeiInf.SinDate >= KaiseiDate.d20221001)
                        {
                            //後期８割
                            wrkRate = 20;
                        }
                    }
                    break;
                default:
                    break;
            }

            return wrkRate;
        }

        /// <summary>
        /// 未就学かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsPreSchool()
        {
            return !CIUtil.IsStudent(PtInf.Birthday, KaikeiInf.SinDate);
        }

        /// <summary>
        /// 70歳以上かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsElder()
        {
            return CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 70);
        }
        /// <summary>
        /// 前期高齢2割かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsElder20per()
        {
            return CIUtil.Is70Zenki_20per(PtInf.Birthday, KaikeiInf.SinDate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsElderExpat()
        {
            //75歳以上で海外居住者の方は後期高齢者医療には加入せず、
            //協会、健保組合に加入することになり、高齢受給者証を提示した場合、
            //H26.5診療分からは所得に合わせ2割または3割負担となる。
            return CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 75) && KaikeiInf.SinDate >= 20140501;
        }

        /// <summary>
        /// 診療点数
        /// </summary>
        public int Tensu
        {
            get => KaikeiInf?.Tensu ?? 0;
        }

        /// <summary>
        /// 自費負担額
        /// </summary>
        public int JihiFutan
        {
            get => KaikeiInf?.JihiFutan ?? 0;
        }
        /// <summary>
        /// 自費内税
        /// </summary>
        public int JihiTax
        {
            get => KaikeiInf?.JihiTax ?? 0;
        }
        /// <summary>
        /// 自費外税
        /// </summary>
        public int JihiOuttax
        {
            get => KaikeiInf?.JihiOuttax ?? 0;
        }
        /// <summary>
        /// 患者負担額
        /// </summary>
        public int PtFutan
        {
            get => KaikeiInf?.PtFutan ?? 0;
        }
        /// <summary>
        /// 患者負担合計k額
        /// </summary>
        public int TotalPtFutan
        {
            get => KaikeiInf?.TotalPtFutan ?? 0;
        }

        /// <summary>
        /// 本人家族区分
        /// 0:本人　1:家族
        /// </summary>
        public string Honke
        {
            get
            {
                string ret = "本人";

                if (PtHokenInf.HonkeKbn == 2)
                {
                    ret = "家族";
                }

                return ret;
            }
        }
        /// <summary>
        /// 公費負担者番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiFutansyaNo(int index)
        {
            string ret = "";

            if (PtKohis != null && PtKohis.Any() && index >= 0 && index < PtKohis.Count)
            {
                ret = PtKohis[index].FutansyaNo;
            }

            return ret;
        }
        /// <summary>
        /// 公費受給者番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiJyukyusyaNo(int index)
        {
            string ret = string.Empty;

            if (PtKohis != null && PtKohis.Any() && index >= 0 && index < PtKohis.Count)
            {
                ret = PtKohis[index].JyukyusyaNo;
            }

            return ret;
        }

        public int Nissu
        {
            get => KaikeiDtls.Sum(p => p.Jitunisu) > 0 ? 1 : 0;
        }
    }
}
