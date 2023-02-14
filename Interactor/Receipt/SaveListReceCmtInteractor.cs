using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Domain.Models.User;
using UseCase.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace Interactor.Receipt;

public class SaveListReceCmtInteractor : ISaveListReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveListReceCmtInteractor(IReceiptRepository receiptRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveListReceCmtOutputData Handle(SaveListReceCmtInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveListReceCmtStatus.ValidateSuccess)
            {
                return new SaveListReceCmtOutputData(responseValidate);
            }
            List<ReceCmtModel> listReceCmtModel = inputData.ListReceCmt.Select(item => ConvertToReceCmtModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item)).ToList();
            if (_receiptRepository.SaveListReceCmt(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveListReceCmtOutputData(SaveListReceCmtStatus.Successed);
            }
            return new SaveListReceCmtOutputData(SaveListReceCmtStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveListReceCmtStatus ValidateInput(SaveListReceCmtInputData inputData)
    {
        if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return SaveListReceCmtStatus.InvalidHpId;
        }
        else if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return SaveListReceCmtStatus.InvalidUserId;
        }
        else if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveListReceCmtStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveListReceCmtStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveListReceCmtStatus.InvalidSinYm;
        }
        if (!inputData.ListReceCmt.Any())
        {
            return SaveListReceCmtStatus.ValidateSuccess;
        }
        var listItemCds = inputData.ListReceCmt.Where(item => item.ItemCd != string.Empty).Select(item => item.ItemCd.Trim()).Distinct().ToList();
        if (listItemCds.Any() && _mstItemRepository.GetCheckItemCds(listItemCds).Count != listItemCds.Count)
        {
            return SaveListReceCmtStatus.InvalidItemCd;
        }
        var listReceCmtIds = inputData.ListReceCmt.Where(item => item.Id > 0).Select(item => item.Id).Distinct().ToList();
        if (listReceCmtIds.Any() && !_receiptRepository.CheckExistReceCmt(inputData.HpId, listReceCmtIds))
        {
            return SaveListReceCmtStatus.InvalidReceCmtId;
        }
        else if (inputData.ListReceCmt.Any(item => item.CmtKbn > 1 || item.CmtKbn < 0))
        {
            return SaveListReceCmtStatus.InvalidCmtKbn;
        }
        else if (inputData.ListReceCmt.Any(item => item.CmtSbt > 1 || item.CmtSbt < 0))
        {
            return SaveListReceCmtStatus.InvalidCmtSbt;
        }
        return SaveListReceCmtStatus.ValidateSuccess;
    }

    private ReceCmtModel ConvertToReceCmtModel(long ptId, int sinYm, int hokenId, ReceCmtItem inputItem)
    {
        return new ReceCmtModel(
                                    inputItem.Id,
                                    ptId,
                                    inputItem.SeqNo,
                                    sinYm,
                                    hokenId,
                                    inputItem.CmtKbn,
                                    inputItem.CmtSbt,
                                    inputItem.Cmt,
                                    inputItem.CmtData,
                                    inputItem.ItemCd,
                                    inputItem.IsDeleted
                                );
    }
}
