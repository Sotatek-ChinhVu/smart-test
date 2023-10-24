using Domain.Models.Receipt;
using UseCase.Receipt.CheckExistsReceInf;

namespace Interactor.Receipt;

public class CheckExistsReceInfInteractor : ICheckExistsReceInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public CheckExistsReceInfInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public CheckExistsReceInfOutputData Handle(CheckExistsReceInfInputData inputData)
    {
        try
        {
            return new CheckExistsReceInfOutputData(_receiptRepository.CheckExistsReceInf(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId), CheckExistsReceInfStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
