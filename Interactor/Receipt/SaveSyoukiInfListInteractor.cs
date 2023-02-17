using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace Interactor.Receipt;

public class SaveSyoukiInfListInteractor : ISaveSyoukiInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveSyoukiInfListInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveSyoukiInfListOutputData Handle(SaveSyoukiInfListInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveSyoukiInfListStatus.ValidateSuccess)
            {
                return new SaveSyoukiInfListOutputData(responseValidate);
            }
            var listReceCmtModel = inputData.SyoukiInfList.Select(item => ConvertToSyoukiInfModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                          .ToList();

            if (_receiptRepository.SaveSyoukiInfList(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus.Successed);
            }
            return new SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveSyoukiInfListStatus ValidateInput(SaveSyoukiInfListInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveSyoukiInfListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveSyoukiInfListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveSyoukiInfListStatus.InvalidHokenId;
        }
        if (!inputData.SyoukiInfList.Any())
        {
            return SaveSyoukiInfListStatus.ValidateSuccess;
        }
        var listSyoukiInfDB = _receiptRepository.GetSyoukiInfList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
        var listSeqNo = inputData.SyoukiInfList.Where(item => item.SeqNo > 0).Select(item => item.SeqNo).Distinct().ToList();
        var countSyoukiInf = listSyoukiInfDB.Count(item => listSeqNo.Contains(item.SeqNo));
        if (listSeqNo.Any() && countSyoukiInf != listSeqNo.Count)
        {
            return SaveSyoukiInfListStatus.InvalidSeqNo;
        }
        var listSyoukiKbn = inputData.SyoukiInfList.Select(item => new SyoukiKbnMstModel(item.SyoukiKbn, item.SyoukiKbnStartYm)).ToList();
        if (listSyoukiKbn.Any() && !_receiptRepository.CheckExistSyoukiKbn(inputData.SinYm, listSyoukiKbn))
        {
            return SaveSyoukiInfListStatus.InvalidSyoukiKbn;
        }
        return SaveSyoukiInfListStatus.ValidateSuccess;
    }

    private SyoukiInfModel ConvertToSyoukiInfModel(long ptId, int sinYm, int hokenId, SyoukiInfItem syoukiInf)
    {
        return new SyoukiInfModel(
                ptId,
                sinYm,
                hokenId,
                syoukiInf.SeqNo,
                syoukiInf.SortNo,
                syoukiInf.SyoukiKbn,
                syoukiInf.Syouki,
                syoukiInf.IsDeleted
            );
    }
}
