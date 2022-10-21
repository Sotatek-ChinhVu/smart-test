using static Helper.Constants.PtAlrgyDrugConst;

namespace Domain.Models.SpecialNote.ImportantNote
{
    public class PtAlrgyDrugModel
    {
        public PtAlrgyDrugModel(int hpId, long ptId, int seqNo, int sortNo, string itemCd, string drugName, int startDate, int endDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ItemCd = itemCd;
            DrugName = drugName;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string ItemCd { get; private set; }

        public string DrugName { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }

        public ValidationStatus Validation()
        {

            if (PtId <= 0)
            {
                return ValidationStatus.InvalidPtId;
            }
            if (ItemCd.Length > 10)
            {
                return ValidationStatus.InvalidItemCd;
            }
            if (DrugName.Length > 100)
            {
                return ValidationStatus.InvalidDrugName;
            }
            if (StartDate < 0)
            {
                return ValidationStatus.InvalidStartDate;
            }
            if (EndDate < 0)
            {
                return ValidationStatus.InvalidEndDate;
            }
            if (Cmt.Length > 100)
            {
                return ValidationStatus.InvalidCmt;
            }

            return ValidationStatus.Valid;
        }
    }
}
