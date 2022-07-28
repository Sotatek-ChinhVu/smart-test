using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListOutputData : IOutputData
{
    public GetReceptionListOutputData(string message)
    {
        Message = message;
    }

    public GetReceptionListOutputData(List<ReceptionRowModel> models)
    {
        Models = models;
        Status = 1;
    }

    public int Status { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
    public List<ReceptionRowModel> Models { get; set; } = new List<ReceptionRowModel>();
}
