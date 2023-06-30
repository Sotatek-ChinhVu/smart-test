using Domain.Models.SinKoui;
using UseCase.SinKoui.GetSinKoui;

namespace Interactor.SinKoui;

public class GetSinKouiInteractor : IGetSinKouiListInputPort
{
    private readonly ISinKouiRepository _sinKouiRepository;

    public GetSinKouiInteractor(ISinKouiRepository sinKouiRepository)
    {
        _sinKouiRepository = sinKouiRepository;
    }

    public GetSinKouiListOutputData Handle(GetSinKouiInputData input)
    {
        try
        {
            if (input.PtId <= 0)
            {
                return new GetSinKouiListOutputData(GetSinKouiListStatus.InvalidPtId);
            }

            var sinKoui = _sinKouiRepository.GetListKaikeiInf(input.HpId, input.PtId);
            return new GetSinKouiListOutputData(GetSinKouiListStatus.Success, sinKoui);
        }
        finally
        {
            _sinKouiRepository.ReleaseResource();
        }
    }
}
