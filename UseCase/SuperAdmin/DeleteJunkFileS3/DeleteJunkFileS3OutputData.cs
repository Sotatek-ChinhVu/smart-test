using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.DeleteJunkFileS3
{
    public class DeleteJunkFileS3OutputData : IOutputData
    {
        public DeleteJunkFileS3OutputData(bool success)
        {
            Success = success;
        }
        public bool Success { get; private set; }
    }
}
