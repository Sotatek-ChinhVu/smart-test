using Domain.Models.KensaInfDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class KensaInfDetailItem
    {
        public KensaInfDetailItem(KensaInfDetailModel kensaInfDetail)
        {
            if (kensaInfDetail != null)
            {
                IraiDate = kensaInfDetail.IraiDate;
                ResultVal = kensaInfDetail.ResultVal;
            }
        }

        public KensaInfDetailItem(string kensaItemCd, string kensaName, string resultVal, long sortNo)
        {
            KensaItemCd = kensaItemCd;
            KensaName = kensaName;
            IraiDate = 0;
            ResultVal = resultVal;
            Unit = "";
            SortNo = sortNo;
        }

        public string KensaItemCd { get; set; } = string.Empty;

        public string KensaName { get; set; } = string.Empty;

        public int IraiDate { get; set; }

        public string ResultVal { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public long SortNo { get; set; }
    }
}
