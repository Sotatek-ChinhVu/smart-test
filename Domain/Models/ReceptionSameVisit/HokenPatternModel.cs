using Domain.Constant;
using Domain.Models.Insurance;
using Entity.Tenant;
using Helper.Common;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionSameVisit
{
    public class HokenPatternModel : ObservableObject
    {
        public PtHokenInf? PtHokenInf { get; } = null;
        public PtHokenPattern? PtHokenPattern { get; } = null;
        public PtKohi? PtKohi1 { get; } = null;
        public PtKohi? PtKohi2 { get; } = null;
        public PtKohi? PtKohi3 { get; } = null;
        public PtKohi? PtKohi4 { get; } = null;

        public HokenPatternModel(PtHokenPattern ptHokenPattern, PtHokenInf ptHokenInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
        {
            PtHokenPattern = ptHokenPattern;
            PtHokenInf = ptHokenInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
        }

        public HokenPatternModel(long ptId, int hokenPid, int startDate, int endDate, int hokenSbtCd, int hokenKbn, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, KohiInfModel kohi1, KohiInfModel kohi2, KohiInfModel kohi3, KohiInfModel kohi4, int sinDate, string hokenMstHoubetu, int hokenMstFutanRate, long raiinNo, int raiinInfSyosaisinKbn, int raiinInfJikanKbn, int raiinInfSanteiKbn)
        {
            PtId = ptId;
            HokenPid = hokenPid;
            StartDate = startDate;
            EndDate = endDate;
            HokenSbtCd = hokenSbtCd;
            HokenKbn = hokenKbn;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            Kohi1 = kohi1;
            Kohi2 = kohi2;
            Kohi3 = kohi3;
            Kohi4 = kohi4;
            SinDate = sinDate;
            HokenMstHoubetu = hokenMstHoubetu;
            HokenMstFutanRate = hokenMstFutanRate;
            RaiinNo = raiinNo;
            RaiinInfSyosaisinKbn = raiinInfSyosaisinKbn;
            RaiinInfSanteiKbn = raiinInfSanteiKbn;
            RaiinInfJikanKbn = raiinInfJikanKbn;
            
        }

        public long PtId { get; private set; }

        public int HokenPid { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenSbtCd { get; private set; }

        public int HokenKbn { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }

        public KohiInfModel Kohi1 { get; private set; }

        public KohiInfModel Kohi2 { get; private set; }

        public KohiInfModel Kohi3 { get; private set; }

        public KohiInfModel Kohi4 { get; private set; }

        public int SinDate { get; private set; }

        public string HokenMstHoubetu { get; private set; }

        public int HokenMstFutanRate { get; private set; }
        
        public long RaiinNo { get; private set; }

        public int RaiinInfSyosaisinKbn { get; private set; }

        public int RaiinInfJikanKbn { get; private set; }

        public int RaiinInfSanteiKbn { get; private set; }

        public string HokenName
        {
            get => GetHokenName();
        }

        public string HokenKbnName
        {
            get
            {
                string result = string.Empty;
                if (PtHokenPattern == null || PtHokenPattern.PtId == 0 && PtHokenPattern.HokenPid == 0 && PtHokenPattern.HpId == 0)
                {
                    return "";
                }

                if (PtHokenInf == null)
                {
                    result = "公費";
                    return result;
                }

                if (PtHokenInf.Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    result = "公費";
                    return result;
                }

                switch (HokenKbn)
                {
                    case 0:
                        result = "自費";
                        break;
                    case 1:
                        result = "社保";
                        break;
                    case 2:
                        if (PtHokenInf.HokensyaNo?.Length == 8 &&
                            PtHokenInf.HokensyaNo.StartsWith("39"))
                        {
                            result = "後期";
                        }
                        else if (PtHokenInf.HokensyaNo?.Length == 8 &&
                            PtHokenInf.HokensyaNo.StartsWith("67"))
                        {
                            result = "退職";
                        }
                        else
                        {
                            result = "国保";
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                        result = "労災";
                        break;
                    case 14:
                        result = "自賠";
                        break;
                }
                return result;
            }
        }

        private string GetHokenName()
        {
            string hokenName = HokenPid.ToString().PadLeft(3, '0') + ". ";

            if (!(StartDate <= SinDate && EndDate >= SinDate))
            {
                hokenName = "×" + hokenName;
            }

            string prefix = string.Empty;
            string postfix = string.Empty;
            if (HokenSbtCd == 0)
            {
                switch (HokenKbn)
                {
                    case 0:
                        if (!string.IsNullOrWhiteSpace(HokenMstHoubetu))
                        {
                            if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                hokenName += "自費 " + HokenMstFutanRate + "%";
                            }
                            else if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ " + HokenMstFutanRate + "%";
                            }
                            else
                            {
                                hokenName += "自費";
                            }
                        }
                        else
                        {
                            hokenName += "自費";
                        }
                        break;
                    case 11:
                        hokenName += "労災（短期給付）";
                        break;
                    case 12:
                        hokenName += "労災（傷病年金）";
                        break;
                    case 13:
                        hokenName += "労災（アフターケア）";
                        break;
                    case 14:
                        hokenName += "自賠責";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (HokenSbtCd < 0)
                {
                    return hokenName;
                }
                string subHokenSbtCd = HokenSbtCd.ToString().PadRight(3, '0');
                int firstNum = Int32.Parse(subHokenSbtCd[0].ToString());
                int secondNum = Int32.Parse(subHokenSbtCd[1].ToString());
                int thirNum = Int32.Parse(subHokenSbtCd[2].ToString());
                switch (firstNum)
                {
                    case 1:
                        hokenName += "社保";
                        break;
                    case 2:
                        hokenName += "国保";
                        break;
                    case 3:
                        hokenName += "後期";
                        break;
                    case 4:
                        hokenName += "退職";
                        break;
                    case 5:
                        hokenName += "公費";
                        break;
                }

                if (secondNum > 0)
                {

                    if (thirNum == 1)
                    {
                        prefix += "単独";
                    }
                    else
                    {
                        prefix += thirNum + "併";
                    }

                    if (Kohi1 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi1.HokenSbtKbn != 2)
                        {
                            postfix += Kohi1.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi2 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi2.HokenSbtKbn != 2)
                        {
                            postfix += Kohi2.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi3 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi3.HokenSbtKbn != 2)
                        {
                            postfix += Kohi3.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi4 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi4.HokenSbtKbn != 2)
                        {
                            postfix += Kohi4.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(postfix))
            {
                hokenName = hokenName + prefix + "(" + postfix + ")";
            }

            string sBuff = "";
            if (StartDate > 0)
            {
                sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate2(StartDate));
            }
            else
            {
                sBuff = string.Format("{0, -11}", " ");
            }

            sBuff += " ～ ";

            if (EndDate > 0 && EndDate < 99999999)
            {
                sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate2(EndDate));
            }
            else
            {
                sBuff += string.Format("{0, -11}", " ");
            }

            return hokenName + " " + sBuff;
        }
    }
}
