using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace Interactor.SuperSetDetail;

public class GetSuperSetDetailInteractor : IGetSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetSuperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetSuperSetDetailOutputData Handle(GetSuperSetDetailInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidHpId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSetCd);
            }
            else if (inputData.Sindate <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSindate);
            }
            var result = _superSetDetailRepository.GetSuperSetDetail(inputData.HpId, inputData.SetCd, inputData.Sindate);
            return new GetSuperSetDetailOutputData(result, GetSuperSetDetailListStatus.Successed);
        }
        catch
        {
            return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.Failed);
        }
    }
}
