using Domain.Models.SetGenerationMst;
using UseCase.SetSendaiGeneration.Delete;

namespace Interactor.SetSendaiGeneration;

public class DeleteSetSendaiGenerationInteractor : IDeleteSendaiGenerationInputPort
{
    private readonly ISetGenerationMstRepository _inputItemRepository;

    public DeleteSetSendaiGenerationInteractor(ISetGenerationMstRepository inputItemRepository)
    {
        _inputItemRepository = inputItemRepository;
    }

    public DeleteSendaiGenerationOutputData Handle(DeleteSendaiGenerationInputData inputData)
    {
        try
        {
            if (inputData.RowIndex == 0)
            {
                return new DeleteSendaiGenerationOutputData(false, DeleteSendaiGenerationStatus.InvalidRowIndex0);
            }

            if (inputData.RowIndex < 0)
            {
                return new DeleteSendaiGenerationOutputData(false, DeleteSendaiGenerationStatus.InvalidRowIndex);
            }

            if (inputData.GenerationId <= 0)
            {
                return new DeleteSendaiGenerationOutputData(false, DeleteSendaiGenerationStatus.InvalidGenerationId);
            }

            if (inputData.UserId <= 0)
            {
                return new DeleteSendaiGenerationOutputData(false, DeleteSendaiGenerationStatus.InvalidUserId);
            }

            var result = _inputItemRepository.DeleteSetSenDaiGeneration(inputData.HpId, inputData.GenerationId, inputData.UserId);
            if (result)
            {
                return new DeleteSendaiGenerationOutputData(result, DeleteSendaiGenerationStatus.Success);
            }
            return new DeleteSendaiGenerationOutputData(false, DeleteSendaiGenerationStatus.Faild);
        }
        finally
        {
            _inputItemRepository.ReleaseResource();
        }
    }
}
