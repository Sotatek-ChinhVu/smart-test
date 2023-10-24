using Domain.Models.Receipt;
using UseCase.Receipt.CheckExistSyobyoKeika;

namespace Interactor.Receipt;

public class CheckExistSyobyoKeikaInteractor : ICheckExistSyobyoKeikaInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public CheckExistSyobyoKeikaInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public CheckExistSyobyoKeikaOutputData Handle(CheckExistSyobyoKeikaInputData inputData)
    {
        try
        {
            if (_receiptRepository.CheckExistSyobyoKeikaSinDay(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId, inputData.SinDay))
            {
                return new CheckExistSyobyoKeikaOutputData(CheckExistSyobyoKeikaStatus.IsExisted);
            }
            return new CheckExistSyobyoKeikaOutputData(CheckExistSyobyoKeikaStatus.IsNotExisted);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
