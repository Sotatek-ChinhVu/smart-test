using UseCase.Core.Sync.Core;
using static Helper.Constants.TodayKarteConst;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.OrdInfs.Upsert
{
    public class UpsertOutputData : IOutputData
    {

        public Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>> ValidationOdrs { get; private set; }
        public Dictionary<int, KeyValuePair<int, TodayKarteValidationStatus>> ValidationKartes { get; private set; }

    }
}
