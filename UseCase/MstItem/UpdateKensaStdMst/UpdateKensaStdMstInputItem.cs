using Helper.Common;

namespace UseCase.MstItem.UpdateKensaStdMst
{
    public class UpdateKensaStdMstInputItem
    {
        public UpdateKensaStdMstInputItem(string kensaItemcd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh
                                         , int startDate, bool isModified, int isDeleted)
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
        }

        public string KensaItemcd { get; private set; }

        public string MaleStd { get; private set; }

        public string MaleStdLow { get; private set; }

        public string MaleStdHigh { get; private set; }

        public string FemaleStd { get; private set; }

        public string FemaleStdLow { get; private set; }

        public string FemaleStdHigh { get; private set; }

        public int StartDate { get; private set; }

        public bool IsModified { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
