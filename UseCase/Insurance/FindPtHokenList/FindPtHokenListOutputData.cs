using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.FindPtHokenList
{
    public class FindPtHokenListOutputData : IOutputData
    {
        public List<HokenInfModel> Data { get; private set; }

        public FindPtHokenListStatus Status { get; private set; }

        public FindPtHokenListOutputData(List<HokenInfModel> data, FindPtHokenListStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}