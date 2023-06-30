using Domain.Models.SinKoui;
using Entity.Tenant;
using UseCase.SinKoui.GetSinKoui;

namespace Interactor.SinKoui;

public class GetListSinKouiInteractor : IGetListSinKouiInputPort
{
    private readonly ISinKouiRepository _sinKouiRepository;

    public GetListSinKouiInteractor(ISinKouiRepository sinKouiRepository)
    {
        _sinKouiRepository = sinKouiRepository;
    }

    public GetListSinKouiOutputData Handle(GetListSinKouiInputData input)
    {
        try
        {
            if (input.PtId <= 0)
            {
                return new GetListSinKouiOutputData(GetListSinKouiStatus.InvalidPtId, new());
            }

            var sinKoui = _sinKouiRepository.GetListKaikeiInf(input.HpId, input.PtId);
            return new GetListSinKouiOutputData(GetListSinKouiStatus.Success, sinKoui);
        }
        finally
        {
            _sinKouiRepository.ReleaseResource();
        }
    }
}
