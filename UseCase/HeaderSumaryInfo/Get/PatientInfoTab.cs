using Domain.Models.KensaInfDetail;
using Domain.Models.PtCmtInf;
using Domain.Models.PtPregnancy;
using Domain.Models.SeikaturekiInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PatientInfoTab
    {
        public PatientInfoTab(List<PtPregnancyModel> pregnancyItems, List<PtCmtInfModel> ptCmtInfItems, List<SeikaturekiInfModel> seikatureInfItems, List<KensaInfDetailModel> kensaInfoDetailItems)
        {
            PregnancyItems = pregnancyItems;
            PtCmtInfItems = ptCmtInfItems;
            SeikatureInfItems = seikatureInfItems;
            KensaInfoDetailItems = kensaInfoDetailItems;
        }

        public List<PtPregnancyModel> PregnancyItems { get; private set; }
        public List<PtCmtInfModel> PtCmtInfItems { get; private set; }
        public List<SeikaturekiInfModel> SeikatureInfItems { get; private set; }
        public List<KensaInfDetailModel> KensaInfoDetailItems { get; private set; }
    }
}
