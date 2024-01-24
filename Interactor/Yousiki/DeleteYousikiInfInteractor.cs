using Domain.Models.Yousiki;
using UseCase.Yousiki.DeleteYousikiInf;

namespace Interactor.Yousiki;

public class DeleteYousikiInfInteractor : IDeleteYousikiInfInputPort
{
    private readonly IYousikiRepository _yousikiRepository;

    public DeleteYousikiInfInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public DeleteYousikiInfOutputData Handle(DeleteYousikiInfInputData inputData)
    {
        try
        {
            if (!_yousikiRepository.IsYousikiExist(inputData.HpId, inputData.SinYm, inputData.PtId))
            {
                return new DeleteYousikiInfOutputData(DeleteYousikiInfStatus.InvalidYousikiInf);
            }
            else if (_yousikiRepository.DeleteYousikiInf(inputData.HpId, inputData.UserId, inputData.SinYm, inputData.PtId))
            {
                return new DeleteYousikiInfOutputData(DeleteYousikiInfStatus.Successed);
            }
            return new DeleteYousikiInfOutputData(DeleteYousikiInfStatus.Failed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }
}
