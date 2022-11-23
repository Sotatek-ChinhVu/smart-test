using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace UseCase.NextOrder.Upsert
{
    public class UpsertNextOrderOutputData : IOutputData
    {
        public UpsertNextOrderOutputData(UpsertNextOrderStatus status, Dictionary<int, NextOrderStatus> validationNextOrder, List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)> validationOdrs, List<(int, KarteValidationStatus)> validationKarte)
        {
            Status = status;
            ValidationNextOrder = validationNextOrder;
            ValidationOdrs = validationOdrs;
            ValidationKarte = validationKarte;
        }

        public UpsertNextOrderStatus Status { get; private set; }

        public Dictionary<int, NextOrderStatus> ValidationNextOrder { get; private set; }

        public List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)> ValidationOdrs { get; private set; }

        public List<(int, KarteValidationStatus)> ValidationKarte { get; private set; }

        public (int, int, RsvkrtByomeiStatus) ValidationByomeis { get; private set; }
    }
}
