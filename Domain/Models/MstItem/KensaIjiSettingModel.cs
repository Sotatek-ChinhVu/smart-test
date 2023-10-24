using Helper.Common;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class KensaIjiSettingModel
    {
        public KensaIjiSettingModel(string itemCd, string kensaItemCd, string name, string santeiItemCd, string receName, double ten, int startDate, int endDate)
        {
            ItemCd = itemCd;
            KensaItemCd = kensaItemCd;
            Name = name;
            SanteiItemCd = santeiItemCd;
            ReceName = receName;
            Ten = ten;
            StartDate = startDate;
            EndDate = endDate;
        }
        public string ItemCd { get; private set; }

        public string KensaItemCd { get; private set; }

        public string Name { get; private set; }

        public string SanteiItemCd { get; private set; }

        public string ReceName { get; private set; }

        public double Ten { get; private set; }
        public string TenDisplay
        {
            get
            {
                if (CheckDefaultValue() || string.IsNullOrEmpty(SanteiItemCd)) return string.Empty;
                return Ten.AsString();
            }
        }

        public int StartDate { get; private set; }

        public string StartDateDisplay
        {
            get
            {
                if (CheckDefaultValue()) return string.Empty;
                if (StartDate == 99999999) return "9999/99/99";
                if (StartDate == 0) return "0000/00/00";
                return CIUtil.SDateToShowSDate(StartDate);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    StartDate = 0;
                    return;
                }
                int startDateConvert = CIUtil.ShowSDateToSDate(value.AsString());
                if (StartDate != startDateConvert)
                {
                    StartDate = startDateConvert;
                }
            }
        }
        public int EndDate { get; private set; }

        public string EndDateDisplay
        {
            get
            {
                if (CheckDefaultValue()) return string.Empty;
                if (EndDate == 99999999) return "9999/99/99";
                if (EndDate == 0) return "0000/00/00";
                return CIUtil.SDateToShowSDate(EndDate);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    EndDate = 99999999;
                    return;
                }
                int endDateConvert = CIUtil.ShowSDateToSDate(value.AsString());
                if (EndDate != endDateConvert)
                {
                    EndDate = endDateConvert;
                }
            }
        }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }
    }
}
