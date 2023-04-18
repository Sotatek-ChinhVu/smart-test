using Reporting.Structs;

namespace Reporting.Receipt.Service
{
    public class ReceiptCoReportService
    {
        private List<long> PtId;
        SeikyuType SeikyuType;
        private int SeikyuYm;
        private int SinYm;
        private int HokenId;
        private int KaId;
        private int GrpId;
        private int TantoId;
        private int Target;
        private string ReceSbt;
        private int PrintNoFrom;
        private int PrintNoTo;

        private int PaperKbn;
        private bool IncludeTester;
        private bool IncludeOutDrug;
        private int Sort;

        public void InitParam(
            int seikyuYm, long ptId, int sinYm, int hokenId, int kaId, int tantoId,
            int target, string receSbt, int printNoFrom, int printNoTo,
            SeikyuType seikyuType, bool includeTester, bool includeOutDrug,
            int sort
        )
        {
            PtId = new List<long>();
            if (ptId > 0)
            {
                PtId.Add(ptId);
            }

            InitParam(
                seikyuYm: seikyuYm, ptId: PtId, sinYm: sinYm, hokenId: hokenId,
                kaId: kaId, tantoId: tantoId, target: target, receSbt: receSbt,
                printNoFrom: printNoFrom, printNoTo: printNoTo,
                seikyuType: seikyuType,
                includeTester: includeTester, includeOutDrug: includeOutDrug,
                sort: sort);
        }

        public void InitParam(
            int seikyuYm, List<long> ptId, int sinYm, int hokenId, int kaId, int tantoId,
            int target, string receSbt, int printNoFrom, int printNoTo,
            SeikyuType seikyuType, bool includeTester, bool includeOutDrug,
            int sort
        )
        {
            SeikyuType = seikyuType;

            SeikyuYm = seikyuYm;
            PtId = new List<long>();
            if (ptId != null)
            {
                PtId.AddRange(ptId.GroupBy(p => p).Select(p => p.Key).ToList());
            }

            SinYm = sinYm;
            HokenId = hokenId;
            KaId = kaId;
            TantoId = tantoId;
            Target = target;
            ReceSbt = receSbt;
            PrintNoFrom = 0;
            PrintNoTo = 999999999;
            IncludeTester = includeTester;
            IncludeOutDrug = includeOutDrug;
            Sort = sort;

            GrpId = 0;
            if (Sort > 100)
            {
                GrpId = Sort % 100;
            }

            if (printNoFrom > 0 && printNoTo > 0 && printNoFrom <= printNoTo)
            {
                PrintNoFrom = printNoFrom;
                PrintNoTo = printNoTo;
            }
        }
    }
}
