using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKbnInf.Upsert;

public class UpsertRaiinKbnInfOutputData : IOutputData
{
    public UpsertRaiinKbnInfOutputData(string message)
    {
        Message = message;
    }

    public UpsertRaiinKbnInfOutputData(bool success)
    {
        Success = success;
        Status = 1;
    }

    public int Status { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
}
