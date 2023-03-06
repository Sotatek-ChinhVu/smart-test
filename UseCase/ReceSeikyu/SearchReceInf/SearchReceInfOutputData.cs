using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.SearchReceInf
{
    public class SearchReceInfOutputData : IOutputData
    {
        public SearchReceInfOutputData(SearchReceInfStatus status, IEnumerable<RegisterRequestModel> data)
        {
            Status = status;
            Data = data;
        }

        public SearchReceInfStatus Status { get; private set; }

        public IEnumerable<RegisterRequestModel> Data { get; private set; }

        public string PtName
        {
            get
            {
                if (Data != null && Data.Any())
                {
                    var first = Data.FirstOrDefault();
                    if (first != null) return first.PtName;
                    else return string.Empty;
                }
                else
                    return string.Empty;
            }
        }
    }
}
