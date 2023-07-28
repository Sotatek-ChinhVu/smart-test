using Domain.Models.Receipt;
using UseCase.Receipt.CheckExisReceInfEdit;

namespace Interactor.Receipt
{
    public class CheckExisReceInfEditInteractor : ICheckExisReceInfEditInputPort
    {
        private readonly IReceiptRepository _receiptRepository;

        public CheckExisReceInfEditInteractor(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public CheckExisReceInfEditOutputData Handle(CheckExisReceInfEditInputData inputData)
        {
            try
            {
                var receInfEdit = _receiptRepository.CheckExisReceInfEdit(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
                if (receInfEdit)
                {
                    return new CheckExisReceInfEditOutputData(CheckExisReceInfEditStatus.Success);
                }
                else
                {
                    return new CheckExisReceInfEditOutputData(CheckExisReceInfEditStatus.Failed);
                }
            }
            finally
            {
                _receiptRepository.ReleaseResource();
            }

        }
    }
}
