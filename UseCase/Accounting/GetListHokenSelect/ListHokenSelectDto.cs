using Domain.Models.Insurance;
using Helper.Common;

namespace UseCase.Accounting.GetListHokenSelect
{
    public class ListHokenSelectDto
    {
        public ListHokenSelectDto(HokenInfModel hokenInf)
        {
            HokenId = hokenInf.HokenId;
            HokenKbn = hokenInf.HokenKbn;
            HokensyaNo = hokenInf.HokensyaNo;
            Houbetu = hokenInf.Houbetu;
            HonkeKbn = hokenInf.HonkeKbn;
            StartDate = hokenInf.StartDate;
            EndDate = hokenInf.EndDate;
            HokenName = string.Empty;
            Date = string.Empty;
            SetHokenName();
        }

        public string HokenName { get; private set; }

        public int HokenId { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Houbetu { get; private set; }

        public int HonkeKbn { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Date { get; private set; }

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
    }
}
