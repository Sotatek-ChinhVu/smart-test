using Domain.Models.Accounting;
using Entity.Tenant;
using Helper.Common;

namespace Reporting.Receipt.Models
{
    public class HokenSelectModel
    {
        public KaikeiInfModel KaikeiInf { get; }

        public PtHokenInf PtHokenInf { get; }

        public HokenSelectModel(KaikeiInfModel kaikeiInf, PtHokenInf ptHokenInf)
        {
            KaikeiInf = kaikeiInf;
            PtHokenInf = ptHokenInf;
            SetHokenName();
        }

        private void SetHokenName()
        {
            var strHokenId = HokenId.ToString().PadLeft(2, '0');
            var hokenName = string.Empty;
            var honke = string.Empty;
            var date = string.Empty;

            switch (HokenKbn)
            {
                case 0:
                    if (Houbetu == "109")
                    {
                        hokenName = "自費レセ";
                    }
                    else
                    {
                        hokenName = "自費";
                    }
                    break;
                case 1:
                    if (Houbetu == "0")
                    {
                        hokenName = "公費";
                    }
                    else
                    {
                        hokenName = "社保";
                    }
                    break;
                case 2:
                    if (HokensyaNo.Length == 8 && HokensyaNo.StartsWith("39"))
                    {
                        hokenName = "後期";
                    }
                    else if (HokensyaNo.Length == 8 && HokensyaNo.StartsWith("67"))
                    {
                        hokenName = "退職";
                    }
                    else
                    {
                        hokenName = "国保";
                    }
                    break;
                case 11:
                    hokenName = "労災(短期給付)";
                    break;
                case 12:
                    hokenName = "労災(傷病年金)";
                    break;
                case 13:
                    hokenName = "アフターケア";
                    break;
                case 14:
                    hokenName = "自賠責";
                    break;
                default:
                    hokenName = string.Empty;
                    break;
            }

            if (HonkeKbn == 0)
            {
                honke = "";
            }
            else if (HonkeKbn == 1)
            {
                honke = "（本人）";
            }
            else
            {
                honke = "（家族）";
            }

            if (StartDate <= 0 && (EndDate <= 0 || EndDate >= 99999999))
            {
                date = string.Empty;
            }
            else
            {
                if (StartDate < 0)
                {
                    date = $"~{CIUtil.SDateToShowWDate(EndDate)}";
                }
                else if (EndDate <= 0 || EndDate >= 99999999)
                {
                    date = $"{CIUtil.SDateToShowWDate(StartDate)}~";
                }
                else
                {
                    date = $"{CIUtil.SDateToShowWDate(StartDate)}~{CIUtil.SDateToShowWDate(EndDate)}";
                }
            }

            HokenName = $"{strHokenId}{hokenName}{honke}";
            Date = date;
        }

        public string HokenName { get; set; }

        public string Date { get; set; }

        public bool IsChecked { get; set; }

        public int HokenId => KaikeiInf.HokenId;

        public int HokenKbn => KaikeiInf.HokenKbn;

        public string Houbetu => PtHokenInf.Houbetu;

        public string HokensyaNo => PtHokenInf.HokensyaNo;

        public int HonkeKbn => PtHokenInf.HonkeKbn;

        public int StartDate => PtHokenInf.StartDate;

        public int EndDate => PtHokenInf.EndDate;
    }
}
