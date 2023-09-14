using Helper.Common;

namespace Domain.Models.MstItem
{
    public class KensaStdMstModel
    {
        public KensaStdMstModel(string kensaItemcd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh
                               , int startDate, bool isModified, bool isDeleted, int createId)
        {
            KensaItemcd = kensaItemcd;
            MaleStd = maleStd;
            MaleStdLow = maleStdLow;
            MaleStdHigh = maleStdHigh;
            FemaleStd = femaleStd;
            FemaleStdLow = femaleStdLow;
            FemaleStdHigh = femaleStdHigh;
            StartDate = startDate;
            IsModified = isModified;
            IsDeleted = isDeleted;
            CreateId = createId;
        }

        public KensaStdMstModel(string kensaItemcd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh
                               , int startDate, int createId)
        {
            KensaItemcd = kensaItemcd;
            MaleStd = maleStd;
            MaleStdLow = maleStdLow;
            MaleStdHigh = maleStdHigh;
            FemaleStd = femaleStd;
            FemaleStdLow = femaleStdLow;
            FemaleStdHigh = femaleStdHigh;
            StartDate = startDate;
            CreateId = createId;
        }

        public string KensaItemcd { get; private set; }

        public int CreateId { get; private set; }

        public bool IsAddNew => CreateId == 0;

        public string MaleStd { get; private set; }

        public string MaleStdLow { get; private set; }

        public string MaleStdHigh { get; private set; }

        public string FemaleStd { get; private set; }

        public string FemaleStdLow { get; private set; }

        public string FemaleStdHigh { get; private set; }

        public int StartDate { get; private set; }

        public string FormattedStartDate
        {
            get
            {
                if (StartDate == 0) return "0";
                if (StartDate == 99999999) return "9999/99/99";
                return CIUtil.SDateToShowSDate(StartDate);
            }
        }

        public bool IsModified { get; private set; }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                _isDeleted = value;
                IsModified = true;
            }
        }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(MaleStd) && string.IsNullOrEmpty(FemaleStd);
        }

        public bool IsDefault => CheckDefaultValue();
    }
}
