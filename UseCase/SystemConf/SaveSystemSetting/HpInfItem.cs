﻿using Helper.Constants;

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
    }
}
