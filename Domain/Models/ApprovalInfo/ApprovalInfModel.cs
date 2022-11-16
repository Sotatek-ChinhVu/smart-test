using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Models.ApprovalInfo
{
    public class ApprovalInfModel
    {
        private ApprovalInfModel approvalInf;
        private PtInf ptInf;
        private RaiinInf raiinInf;
        private string tantoName;

        public ApprovalInfModel(ApprovalInf approvalInf, PtInf ptInf, RaiinInf raiinInf, string tantoName, string kaName)
        {
            KaName = kaName;
        }

        public ApprovalInfModel(ApprovalInfModel approvalInf, PtInf ptInf, RaiinInf raiinInf, string tantoName, string kaName)
        {
            this.approvalInf = approvalInf;
            this.ptInf = ptInf;
            this.raiinInf = raiinInf;
            this.tantoName = tantoName;
            KaName = kaName;
        }

        public ApprovalInfModel(int sinDate, int num, string name, string kanaName, string drName, string kaName)
        {
            SinDate = sinDate;
            Num = num;
            Name = name;
            KanaName = kanaName;
            DrName = drName;
            KaName = kaName;
        }
        public int SinDate { get; private set; }
        public int Num { get; private set; }
        public string Name { get; private set; }
        public string KanaName { get; private set; }
        public string DrName { get; private set; }
        public string KaName { get; private set; }
    }
}
