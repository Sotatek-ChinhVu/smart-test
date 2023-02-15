using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace Interactor.Receipt;

public class SaveListSyoukiInfInteractor : ISaveListSyoukiInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveListSyoukiInfInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveListSyoukiInfOutputData Handle(SaveListSyoukiInfInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveListSyoukiInfStatus.ValidateSuccess)
            {
                return new SaveListSyoukiInfOutputData(responseValidate);
            }
            var listReceCmtModel = inputData.ListSyoukiInf.Select(item => ConvertToSyoukiInfModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                          .ToList();

            if (_receiptRepository.SaveListSyoukiInf(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveListSyoukiInfOutputData(SaveListSyoukiInfStatus.Successed);
            }
            return new SaveListSyoukiInfOutputData(SaveListSyoukiInfStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveListSyoukiInfStatus ValidateInput(SaveListSyoukiInfInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveListSyoukiInfStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveListSyoukiInfStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveListSyoukiInfStatus.InvalidSinYm;
        }
        if (!inputData.ListSyoukiInf.Any())
        {
            return SaveListSyoukiInfStatus.ValidateSuccess;
        }
        var listSyoukiInfDB = _receiptRepository.GetListSyoukiInf(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
        var listSeqNo = inputData.ListSyoukiInf.Where(item => item.SeqNo > 0).Select(item => item.SeqNo).Distinct().ToList();
        var countSyoukiInf = listSyoukiInfDB.Count(item => listSeqNo.Contains(item.SeqNo));
        if (listSeqNo.Any() && countSyoukiInf != listSeqNo.Count)
        {
            return SaveListSyoukiInfStatus.InvalidSeqNo;
        }
        var listSyoukiKbn = inputData.ListSyoukiInf.Select(item => new SyoukiKbnMstModel(item.SyoukiKbn, item.SyoukiKbnStartYm)).ToList();
        if (listSyoukiKbn.Any() && !_receiptRepository.CheckExistSyoukiKbn(inputData.SinYm, listSyoukiKbn))
        {
            return SaveListSyoukiInfStatus.InvalidSyoukiKbn;
        }
        return SaveListSyoukiInfStatus.ValidateSuccess;
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
