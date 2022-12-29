using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Santei;
using Domain.Models.User;
using UseCase.Santei.SaveListSanteiInf;

namespace Interactor.Santei;

public class SaveListSanteiInfInteractor : ISaveListSanteiInfInputPort
{
    private readonly ISanteiInfRepository _santeiInfRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;

    public SaveListSanteiInfInteractor(ISanteiInfRepository santeiInfRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository)
    {
        _santeiInfRepository = santeiInfRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
    }

    public SaveListSanteiInfOutputData Handle(SaveListSanteiInfInputData inputData)
    {
        try
        {
            var resultValidate = ValidateInput(inputData);
            if (resultValidate != SaveListSanteiInfStatus.ValidateSuccess)
            {
                return new SaveListSanteiInfOutputData(resultValidate);
            }


            return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Successed);
        }
        catch (Exception)
        {
            return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Failed);
        }
        finally
        {
            _santeiInfRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
        }
    }

    private SaveListSanteiInfStatus ValidateInput(SaveListSanteiInfInputData input)
    {
        if (input.HpId <= 0 || !_hpInfRepository.CheckHpId(input.HpId))
        {
            return SaveListSanteiInfStatus.InvalidHpId;
        }
        else if (input.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { input.PtId }))
        {
            return SaveListSanteiInfStatus.InvalidPtId;
        }
        else if (input.UserId <= 0 || !_userRepository.CheckExistedUserId(input.UserId))
        {
            return SaveListSanteiInfStatus.InvalidUserId;
        }

        var listSanteiInf = _santeiInfRepository.GetOnlyListSanteiInf(input.HpId, input.PtId);
        var listSanteiInfDetail = _santeiInfRepository.GetListSanteiInfDetailModel(input.HpId, input.PtId, true);
        foreach (var santeiInf in input.ListSanteiInfs)
        {

        }

        return SaveListSanteiInfStatus.ValidateSuccess;
    }
}
