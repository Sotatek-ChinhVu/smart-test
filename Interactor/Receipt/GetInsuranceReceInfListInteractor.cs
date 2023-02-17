using Domain.Models.Receipt;
using UseCase.Receipt.GetInsuranceReceInfList;

namespace Interactor.Receipt;

public class GetInsuranceReceInfListInteractor : IGetInsuranceReceInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetInsuranceReceInfListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetInsuranceReceInfListOutputData Handle(GetInsuranceReceInfListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetInsuranceReceInfList(inputData.HpId, inputData.SeikyuYm, inputData.HokenId, inputData.SinYm, inputData.PtId);
            return new GetInsuranceReceInfListOutputData(result, GetInsuranceReceInfListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
