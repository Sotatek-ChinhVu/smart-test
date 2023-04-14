using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveReceStatus;

namespace Interactor.Receipt;

public class SaveReceStatusInteractor : ISaveReceStatusInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveReceStatusInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveReceStatusOutputData Handle(SaveReceStatusInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData.ReceStatus);
            if (responseValidate != SaveReceStatusStatus.ValidateSuccess)
            {
                return new SaveReceStatusOutputData(responseValidate);
            }

            if (_receiptRepository.SaveReceStatus(inputData.HpId, inputData.UserId, ConvertToReceStatusModel(inputData.ReceStatus)))
            {
                return new SaveReceStatusOutputData(SaveReceStatusStatus.Successed);
            }
            return new SaveReceStatusOutputData(SaveReceStatusStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private ReceStatusModel ConvertToReceStatusModel(ReceStatusItem inputData)
    {
        return new ReceStatusModel(
                   inputData.PtId,
                   inputData.SeikyuYm,
                   inputData.HokenId,
                   inputData.SinYm,
                   inputData.FusenKbn,
                   inputData.IsPaperRece,
                   inputData.IsOutput,
                   inputData.StatusKbn,
                   inputData.IsPrechecked);
    }

    private SaveReceStatusStatus ValidateInput(ReceStatusItem inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SaveReceStatusStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveReceStatusStatus.InvalidSinYm;
        }
        else if (inputData.SeikyuYm.ToString().Length != 6)
        {
            return SaveReceStatusStatus.InvalidSeikyuYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveReceStatusStatus.InvalidHokenId;
        }
        else if (inputData.FusenKbn < 0)
        {
            return SaveReceStatusStatus.InvalidFusenKbn;
        }
        else if (inputData.StatusKbn < 0 || inputData.StatusKbn > 9)
        {
            return SaveReceStatusStatus.InvalidStatusKbn;
        }
        return SaveReceStatusStatus.ValidateSuccess;
    }
}
