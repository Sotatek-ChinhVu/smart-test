using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SavePtKyusei;

namespace Interactor.PatientInfor;

public class SavePtKyuseiInteractor : ISavePtKyuseiInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public SavePtKyuseiInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public SavePtKyuseiOutputData Handle(SavePtKyuseiInputData inputData)
    {
        try
        {
            var resultValidate = ValidatePtKyusei(inputData);
            if (resultValidate != SavePtKyuseiStatus.ValidateSuccess)
            {
                return new SavePtKyuseiOutputData(resultValidate);
            }
            var ptKyuseiList = inputData.PtKyuseiList.Select(item => new PtKyuseiModel(inputData.HpId,
                                                                                       inputData.PtId,
                                                                                       item.SeqNo,
                                                                                       item.KanaName,
                                                                                       item.Name,
                                                                                       item.EndDate,
                                                                                       item.IsDeleted))
                                                     .ToList();
            if (_patientInforRepository.SavePtKyusei(inputData.HpId, inputData.UserId, ptKyuseiList))
            {
                return new SavePtKyuseiOutputData(SavePtKyuseiStatus.Successed);
            }
            return new SavePtKyuseiOutputData(SavePtKyuseiStatus.Failed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }

    }

    private SavePtKyuseiStatus ValidatePtKyusei(SavePtKyuseiInputData inputData)
    {
        if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SavePtKyuseiStatus.InvalidPtId;
        }
        var ptKyuseiDBList = _patientInforRepository.PtKyuseiInfModels(inputData.HpId, inputData.PtId, false);
        foreach (var inputItem in inputData.PtKyuseiList)
        {
            if (inputItem.SeqNo != 0 && ptKyuseiDBList.FirstOrDefault(item => item.SeqNo == inputItem.SeqNo) == null)
            {
                return SavePtKyuseiStatus.InvalidSeqNo;
            }
            else if (string.IsNullOrEmpty(inputItem.Name))
            {
                return SavePtKyuseiStatus.InvalidName;
            }
            else if (string.IsNullOrEmpty(inputItem.KanaName))
            {
                return SavePtKyuseiStatus.InvalidKanaName;
            }
            else if (inputItem.EndDate.ToString().Length != 8)
            {
                return SavePtKyuseiStatus.InvalidSindate;
            }
        }
        return SavePtKyuseiStatus.ValidateSuccess;
    }
}
