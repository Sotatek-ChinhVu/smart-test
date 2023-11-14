using Helper.Common;

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

        public CmtKbnMstModel()
        {
            ItemCd = string.Empty;
        }

        public long Id { get; private set; }

        public int StartDate { get; private set; }

        public string StartDateBinding
        {
            get => CheckDefaultValue() ? string.Empty : CIUtil.SDateToShowSDate(StartDate);
        }

        public int EndDate { get; private set; }


        public string EndDateBinding
        {
            get
            {
                if (!CheckDefaultValue())
                {
                    if (EndDate == 99999999)
                    {
                        return "9999/99/99";
                    }
                    return CIUtil.SDateToShowSDate(EndDate);
                }
                return string.Empty;
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
