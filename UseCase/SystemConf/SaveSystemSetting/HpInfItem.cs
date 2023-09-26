using Helper.Constants;
using static Helper.Constants.StatusConstant;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class HpInfItem
    {
        public HpInfItem(int hpId, int startDate, string hpCd, string rousaiHpCd, string hpName, string receHpName, string kaisetuName, string postCd, int prefNo, string address1, string address2, string tel, string faxNo, string otherContacts, int updateId, ModelStatus hpInfModelStatus)
        {
            HpId = hpId;
            StartDate = startDate;
            HpCd = hpCd;
            RousaiHpCd = rousaiHpCd;
            HpName = hpName;
            ReceHpName = receHpName;
            KaisetuName = kaisetuName;
            PostCd = postCd;
            PrefNo = prefNo;
            Address1 = address1;
            Address2 = address2;
            Tel = tel;
            FaxNo = faxNo;
            OtherContacts = otherContacts;
            UpdateId = updateId;
            HpInfModelStatus = hpInfModelStatus;
        }
        public int HpId { get; private set; }

        public int StartDate { get; private set; }

        public string HpCd { get; private set; }

        public string RousaiHpCd { get; private set; }

        public string HpName { get; private set; }

        public string ReceHpName { get; private set; }

        public string KaisetuName { get; private set; }

        public string PostCd { get; private set; }

        public int PrefNo { get; private set; }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string Tel { get; private set; }

        public string FaxNo { get; private set; }

        public string OtherContacts { get; private set; }

        public int UpdateId { get; private set; }

        public ModelStatus HpInfModelStatus { get; private set; }

        #region common
        public ValidationHpInfStatus Validation()
        {
            if (HpId <= 0)
                return ValidationHpInfStatus.InvalidHpId;
            if (HpCd.Length > 7)
                return ValidationHpInfStatus.InvalidHpCd;
            if (RousaiHpCd.Length > 7)
                return ValidationHpInfStatus.InvalidRousaiHpCd;
            if (HpName.Length > 80)
                return ValidationHpInfStatus.InvalidHpName;
            if (ReceHpName.Length > 80)
                return ValidationHpInfStatus.InvalidReceHpName;
            if (KaisetuName.Length > 40)
                return ValidationHpInfStatus.InvalidKaisetuName;
            if (PostCd.Length > 7)
                return ValidationHpInfStatus.InvalidPostCd;
            if (Address1.Length > 100)
                return ValidationHpInfStatus.InvalidAddress1;
            if (Address2.Length > 100)
                return ValidationHpInfStatus.InvalidAddress2;
            if (Tel.Length > 15)
                return ValidationHpInfStatus.InvalidTel;

            return ValidationHpInfStatus.None;
        }
        #endregion
    }
}
