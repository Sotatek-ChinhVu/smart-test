using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.GetRecedenHenJiyuuList;

namespace Interactor.ReceSeikyu;

public class GetRecedenHenJiyuuListInteractor : IGetRecedenHenJiyuuListInputPort
{
    private readonly IReceSeikyuRepository _receSeikyuRepository;

    public GetRecedenHenJiyuuListInteractor(IReceSeikyuRepository receSeikyuRepository)
    {
        _receSeikyuRepository = receSeikyuRepository;
    }

    public GetRecedenHenJiyuuListOutputData Handle(GetRecedenHenJiyuuListInputData inputData)
    {
        try
        {
            var result = _receSeikyuRepository.GetRecedenHenJiyuuModels(inputData.HpId, inputData.PtId, inputData.SinYm);
            return new GetRecedenHenJiyuuListOutputData(GetRecedenHenJiyuuListStatus.Successed, result);
        }
        finally
        {
            _receSeikyuRepository.ReleaseResource();
        }
    }
}
