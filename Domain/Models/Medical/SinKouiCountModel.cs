using Domain.Models.Receipt.Recalculation;
using Helper.Constants;

namespace Domain.Models.Medical
{
    public class SinKouiCountModel
    {
        public SinKouiCountModel(int hpId, long ptId, int sinYm, int sinDay, int sinDate, long raiinNo, int rpNo, int seqNo, int count)
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            SinDay = sinDay;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            SeqNo = seqNo;
            Count = count;
            PtHokenPatterns = new();
            SinKouiDetailModels = new();
        }

        public SinKouiCountModel(int hpId, long ptId, int sinDate, long raiinNo, List<PtHokenPatternModel> ptHokenPatterns, List<SinKouiDetailModel> sinKouiDetailModels)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtHokenPatterns = ptHokenPatterns;
            SinKouiDetailModels = sinKouiDetailModels;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int SinDay { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int RpNo { get; private set; }

        public int SeqNo { get; private set; }

        public int Count { get; private set; }

        public List<PtHokenPatternModel> PtHokenPatterns { get; private set; }

        public List<SinKouiDetailModel> SinKouiDetailModels { get; private set; }

        public bool IsFirstVisit => SinKouiDetailModels?.Count > 0 && SinKouiDetailModels.Exists(p => ReceErrCdConst.IsFirstVisitCd.Contains(p.ItemCd));

        public bool IsReVisit => SinKouiDetailModels?.Count > 0 && SinKouiDetailModels.Exists(p => ReceErrCdConst.IsReVisitCd.Contains(p.ItemCd));

        public bool ExistSameFirstVisit => SinKouiDetailModels?.Count > 0 && SinKouiDetailModels.Exists(p => p.ItemCd == "101110040" || p.ItemCd == "111011810");
    }
}
