using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using System.Text;
using UseCase.Receipt.SaveReceiptEdit;

namespace Interactor.Receipt;

public class SaveReceiptEditInteractor : ISaveReceiptEditInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveReceiptEditInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveReceiptEditOutputData Handle(SaveReceiptEditInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveReceiptEditStatus.ValidateSuccess)
            {
                return new SaveReceiptEditOutputData(responseValidate);
            }

            ReceiptEditModel model = ConvertToReceiptEditModel(inputData);

            if (_receiptRepository.SaveReceiptEdit(inputData.HpId, inputData.UserId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId, model))
            {
                return new SaveReceiptEditOutputData(SaveReceiptEditStatus.Successed);
            }
            return new SaveReceiptEditOutputData(SaveReceiptEditStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveReceiptEditStatus ValidateInput(SaveReceiptEditInputData inputData)
    {
        if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveReceiptEditStatus.InvalidSinYm;
        }
        else if (inputData.SeikyuYm.ToString().Length != 6)
        {
            return SaveReceiptEditStatus.InvalidSeikyuYm;
        }
        else if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SaveReceiptEditStatus.InvalidPtId;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveReceiptEditStatus.InvalidHokenId;
        }
        else if (!_receiptRepository.CheckExistReceiptEdit(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId, inputData.ReceiptEdit.SeqNo))
        {
            return SaveReceiptEditStatus.InvalidSeqNo;
        }
        else if (inputData.ReceiptEdit.Tokki1Id.Length > 3
                 || inputData.ReceiptEdit.Tokki2Id.Length > 3
                 || inputData.ReceiptEdit.Tokki3Id.Length > 3
                 || inputData.ReceiptEdit.Tokki4Id.Length > 3
                 || inputData.ReceiptEdit.Tokki5Id.Length > 3)
        {
            return SaveReceiptEditStatus.InvalidTokkiItem;
        }
        else if (inputData.ReceiptEdit.HokenNissu > 99
                 || inputData.ReceiptEdit.Kohi1Nissu > 99
                 || inputData.ReceiptEdit.Kohi2Nissu > 99
                 || inputData.ReceiptEdit.Kohi3Nissu > 99
                 || inputData.ReceiptEdit.Kohi4Nissu > 99)
        {
            return SaveReceiptEditStatus.InvalidNissuItem;
        }
        return SaveReceiptEditStatus.ValidateSuccess;
    }

    private ReceiptEditModel ConvertToReceiptEditModel(SaveReceiptEditInputData inputData)
    {
        var receInf = _receiptRepository.GetReceInf(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
        var tokkiMstDictionary = _receiptRepository.GetTokkiMstDictionary(inputData.HpId);

        var receEdit = inputData.ReceiptEdit;
        StringBuilder tokki = new();
        tokki.Append(receEdit.Tokki1.Trim());
        tokki.Append(receEdit.Tokki2.Trim());
        tokki.Append(receEdit.Tokki3.Trim());
        tokki.Append(receEdit.Tokki4.Trim());
        tokki.Append(receEdit.Tokki5.Trim());

        string tokki1 = GetTokkiItem(tokkiMstDictionary, receEdit.Tokki1Id);
        string tokki2 = GetTokkiItem(tokkiMstDictionary, receEdit.Tokki2Id);
        string tokki3 = GetTokkiItem(tokkiMstDictionary, receEdit.Tokki3Id);
        string tokki4 = GetTokkiItem(tokkiMstDictionary, receEdit.Tokki4Id);
        string tokki5 = GetTokkiItem(tokkiMstDictionary, receEdit.Tokki5Id);
        string receSbt = receInf?.ReceSbt ?? string.Empty;
        string houbetu = receInf?.Houbetu ?? string.Empty;

        return new ReceiptEditModel(
                   receEdit.SeqNo,
                   tokki1,
                   tokki2,
                   tokki3,
                   tokki4,
                   tokki5,
                   receEdit.HokenNissu,
                   receEdit.Kohi1Nissu,
                   receEdit.Kohi2Nissu,
                   receEdit.Kohi3Nissu,
                   receEdit.Kohi4Nissu,
                   receEdit.Kohi1ReceKyufu,
                   receEdit.Kohi2ReceKyufu,
                   receEdit.Kohi3ReceKyufu,
                   receEdit.Kohi4ReceKyufu,
                   receEdit.HokenReceTensu,
                   receEdit.HokenReceFutan,
                   receEdit.Kohi1ReceTensu,
                   receEdit.Kohi1ReceFutan,
                   receEdit.Kohi2ReceTensu,
                   receEdit.Kohi2ReceFutan,
                   receEdit.Kohi3ReceTensu,
                   receEdit.Kohi3ReceFutan,
                   receEdit.Kohi4ReceTensu,
                   receEdit.Kohi4ReceFutan,
                   receSbt,
                   houbetu,
                   tokki.ToString(),
                   receEdit.IsDeleted
            );
    }

    private string GetTokkiItem(Dictionary<string, string> tokkiMstDictionary, string tokkiId)
    {
        string tokki = tokkiId;
        if (!string.IsNullOrEmpty(tokkiId) && tokkiMstDictionary.ContainsKey(tokkiId.Trim()))
        {
            var tokiName = tokkiMstDictionary[tokkiId.Trim()];
            if (tokiName != null)
            {
                tokki = tokkiId.Trim() + tokiName;
            }
        }
        return tokki;
    }
}
