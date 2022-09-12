using Domain.Models.SuperSetDetail;
using UseCase.SupperSetDetail.GetSetByomeiList;

namespace Interactor.SupperSetDetail;

public class GetSetByomeiListInteractor : IGetSetByomeiListInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetSetByomeiListInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetSetByomeiListOutputData Handle(GetSetByomeiListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSetByomeiListOutputData(GetSetByomeiListStatus.InvalidHpId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSetByomeiListOutputData(GetSetByomeiListStatus.InvalidSetCd);
            }
            var result = _superSetDetailRepository.GetSetByomeiList(inputData.HpId, inputData.SetCd);
            return new GetSetByomeiListOutputData(result, GetSetByomeiListStatus.Successed);
        }
        catch
        {
            return new GetSetByomeiListOutputData(GetSetByomeiListStatus.Failed);
        }
    }
}
