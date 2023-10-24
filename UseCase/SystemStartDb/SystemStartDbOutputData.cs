using Domain.Models.TimeZone;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemStartDbs
{
    public class SystemStartDbOutputData : IOutputData
    {
        public SystemStartDbOutputData(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
