using Domain.Models.SuperSetDetail;
using UseCase.SupperSetDetail.SupperSetDetail;

namespace Interactor.SupperSetDetail;

public class GetSuperSetDetailInteractor : IGetSupperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetSuperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetSupperSetDetailOutputData Handle(GetSupperSetDetailInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSupperSetDetailOutputData(GetSupperSetDetailListStatus.InvalidHpId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSupperSetDetailOutputData(GetSupperSetDetailListStatus.InvalidSetCd);
            }
            var result = _superSetDetailRepository.GetSuperSetDetail(inputData.HpId, inputData.SetCd, inputData.Sindate);
            return new GetSupperSetDetailOutputData(result, GetSupperSetDetailListStatus.Successed);
        }
        catch
        {
            return new GetSupperSetDetailOutputData(GetSupperSetDetailListStatus.Failed);
        }
    }
}
