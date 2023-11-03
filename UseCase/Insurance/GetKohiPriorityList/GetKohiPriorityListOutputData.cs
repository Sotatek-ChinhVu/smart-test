using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetKohiPriorityList
{
    public class GetKohiPriorityListOutputData : IOutputData
    {
        public List<KohiPriorityModel> Data { get; private set; } = new List<KohiPriorityModel>();

        public GetKohiPriorityListStatus Status { get; private set; }

        public GetKohiPriorityListOutputData(List<KohiPriorityModel> data, GetKohiPriorityListStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}
