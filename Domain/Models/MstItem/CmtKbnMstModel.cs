using Helper.Common;
using Helper.Extension;

namespace Domain.Models.MstItem
{
    public class CmtKbnMstModel
    {   
        public CmtKbnMstModel(long id, int startDate, int endDate, int cmtKbn, string itemCd, bool isDeleted)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            CmtKbn = cmtKbn;
            ItemCd = itemCd;
            IsDeleted = isDeleted;
        }

        public long Id { get; private set; }

        public int StartDate { get; private set; }

        public string StartDateBinding
        {
            get => CheckDefaultValue() ? string.Empty : CIUtil.SDateToShowSDate(StartDate);
            set
            {
                if (string.IsNullOrEmpty(value?.Trim()))
                {
                    StartDate = 0;
                    return;
                }
                string startDate = CIUtil.ShowSDateToSDate(value).AsString();
                if (startDate == "0")
                {
                    StartDate = CIUtil.ShowWDateToSDate(value);
                }
                else
                {
                    StartDate = startDate.AsInteger();
                }
            }
        }

        public int EndDate { get; private set; }


        public string EndDateBinding
        {
            get => CheckDefaultValue() ? string.Empty : (EndDate == 99999999 ? "9999/99/99" : CIUtil.SDateToShowSDate(EndDate));
            set
            {
                if (string.IsNullOrEmpty(value?.Trim()) || value == "99999999")
                {
                    EndDate = 99999999;
                    return;
                }
                string endDate = CIUtil.ShowSDateToSDate(value).AsString();
                if (endDate == "0")
                {
                    EndDate = CIUtil.ShowWDateToSDate(value);
                }
                else
                {
                    EndDate = endDate.AsInteger();
                }
            }
        }

        public int CmtKbn { get; private set; }

        public string ItemCd { get; private set; }

        public bool IsDeleted { get; private set; }


        public bool CheckDefaultValue()
        {
            return Id == 0 && CmtKbn == -1 && StartDate == 0 && EndDate == 99999999;
        }
    }
}
