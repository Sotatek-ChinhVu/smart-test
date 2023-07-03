using Domain.Models.Receipt;
using UseCase.SinKoui.GetSinKoui;

namespace Interactor.SinKoui;

public class GetListSinKouiInteractor : IGetListSinKouiInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListSinKouiInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetListSinKouiOutputData Handle(GetListSinKouiInputData input)
    {
        try
        {
            if (input.PtId <= 0)
            {
                return new GetListSinKouiOutputData(GetListSinKouiStatus.InvalidPtId, new());
            }

            var sinKoui = _receiptRepository.GetListKaikeiInf(input.HpId, input.PtId);
            return new GetListSinKouiOutputData(GetListSinKouiStatus.Success, sinKoui);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
