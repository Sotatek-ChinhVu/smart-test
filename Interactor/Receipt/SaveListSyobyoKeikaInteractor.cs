using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyobyoKeika;

namespace Interactor.Receipt;

public class SaveListSyobyoKeikaInteractor : ISaveListSyobyoKeikaInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveListSyobyoKeikaInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveListSyobyoKeikaOutputData Handle(SaveListSyobyoKeikaInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveListSyobyoKeikaStatus.ValidateSuccess)
            {
                return new SaveListSyobyoKeikaOutputData(responseValidate);
            }
            var listSyobyoKeikaModel = inputData.SyobyoKeikaList.Select(item => ConvertToSyobyoKeikaModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                                .ToList();

            if (_receiptRepository.SaveListSyobyoKeika(inputData.HpId, inputData.UserId, listSyobyoKeikaModel))
            {
                return new SaveListSyobyoKeikaOutputData(SaveListSyobyoKeikaStatus.Successed);
            }
            return new SaveListSyobyoKeikaOutputData(SaveListSyobyoKeikaStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SyobyoKeikaModel ConvertToSyobyoKeikaModel(long ptId, int sinYm, int hokenId, SyobyoKeikaItem inputItem)
    {
        return new SyobyoKeikaModel(
                                       ptId,
                                       sinYm,
                                       inputItem.SinDay,
                                       hokenId,
                                       inputItem.SeqNo,
                                       inputItem.Keika,
                                       inputItem.IsDeleted
                                   );
    }

    private SaveListSyobyoKeikaStatus ValidateInput(SaveListSyobyoKeikaInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveListSyobyoKeikaStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveListSyobyoKeikaStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveListSyobyoKeikaStatus.InvalidSinYm;
        }
        else if (!inputData.SyobyoKeikaList.Any())
        {
            return SaveListSyobyoKeikaStatus.ValidateSuccess;
        }
        else if (inputData.SyobyoKeikaList.Any(item => item.SinDay > 31 || item.SinDay < 1))
        {
            return SaveListSyobyoKeikaStatus.InvalidSinDay;
        }
        else if (inputData.SyobyoKeikaList.Any(item => !item.IsDeleted && item.Keika == string.Empty))
        {
            return SaveListSyobyoKeikaStatus.InvalidKeika;
        }
        var syobyoKeikaDBList = _receiptRepository.GetListSyobyoKeika(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
        var seqNoList = inputData.SyobyoKeikaList.Where(item => item.SeqNo > 0).Select(item => item.SeqNo).Distinct().ToList();
        var syobyoKeikaCount = syobyoKeikaDBList.Count(item => seqNoList.Contains(item.SeqNo));
        if (seqNoList.Any() && syobyoKeikaCount != seqNoList.Count)
        {
            return SaveListSyobyoKeikaStatus.InvalidSeqNo;
        }
        var sindayDBList = syobyoKeikaDBList.Select(item => item.SinDay);
        if (sindayDBList.Any() && inputData.SyobyoKeikaList.Any(item => !item.IsDeleted && sindayDBList.Contains(item.SinDay)))
        {
            return SaveListSyobyoKeikaStatus.InvalidSinDay;
        }
        return SaveListSyobyoKeikaStatus.ValidateSuccess;
    }
}
