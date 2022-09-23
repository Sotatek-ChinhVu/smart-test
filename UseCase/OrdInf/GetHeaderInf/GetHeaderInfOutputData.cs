using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetHeaderInf
{
    public class GetHeaderInfOutputData : IOutputData
    {

        public OrdInfModel OdrInfs { get; private set; }
        public GetHeaderInfStatus Status { get; private set; }

        public GetHeaderInfOutputData(OrdInfModel odrInfs, GetHeaderInfStatus status)
        {
            OdrInfs = odrInfs;
            Status = status;
        }
    }
}
