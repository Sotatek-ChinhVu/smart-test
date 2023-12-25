using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.DeleteJunkFileS3
{
    public interface IDeleteJunkFileS3InputPort : IInputPort<DeleteJunkFileS3InputData, DeleteJunkFileS3OutputData>
    {
    }
}
