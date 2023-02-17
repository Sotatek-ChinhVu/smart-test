using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyobyoKeika;

namespace Interactor.Receipt;

public class SaveSyobyoKeikaListInteractor : ISaveSyobyoKeikaListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveSyobyoKeikaListInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveSyobyoKeikaListOutputData Handle(SaveSyobyoKeikaListInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveSyobyoKeikaListStatus.ValidateSuccess)
            {
                return new SaveSyobyoKeikaListOutputData(responseValidate);
            }
            var listSyobyoKeikaModel = inputData.SyobyoKeikaList.Select(item => ConvertToSyobyoKeikaModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                                .ToList();

            if (_receiptRepository.SaveSyobyoKeikaList(inputData.HpId, inputData.UserId, listSyobyoKeikaModel))
            {
                return new SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus.Successed);
            }
            return new SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus.Failed);
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

    private SaveSyobyoKeikaListStatus ValidateInput(SaveSyobyoKeikaListInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveSyobyoKeikaListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveSyobyoKeikaListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveSyobyoKeikaListStatus.InvalidHokenId;
        }
        else if (!inputData.SyobyoKeikaList.Any())
        {
            return SaveSyobyoKeikaListStatus.Failed;
        }
        else if (inputData.SyobyoKeikaList.Any(item => item.SinDay > 31 || item.SinDay < 1))
        {
            return SaveSyobyoKeikaListStatus.InvalidSinDay;
        }
        else if (inputData.SyobyoKeikaList.Any(item => !item.IsDeleted && item.Keika == string.Empty))
        {
            return SaveSyobyoKeikaListStatus.InvalidKeika;
        }
        var syobyoKeikaDBList = _receiptRepository.GetSyobyoKeikaList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
        var seqNoList = inputData.SyobyoKeikaList.Where(item => item.SeqNo > 0).Select(item => item.SeqNo).ToList();
        var seqNoListQuery = seqNoList.Distinct().ToList();
        var syobyoKeikaCount = syobyoKeikaDBList.Count(item => seqNoListQuery.Contains(item.SeqNo));
        if (seqNoList.Any() && syobyoKeikaCount != seqNoList.Count)
        {
            return SaveSyobyoKeikaListStatus.InvalidSeqNo;
        }
        var sindayDBList = syobyoKeikaDBList.Select(item => item.SinDay);
        if (sindayDBList.Any() && inputData.SyobyoKeikaList.Any(item => !item.IsDeleted && sindayDBList.Contains(item.SinDay)))
        {
            return SaveSyobyoKeikaListStatus.InvalidSinDay;
        }
        return SaveSyobyoKeikaListStatus.ValidateSuccess;
    }
}
