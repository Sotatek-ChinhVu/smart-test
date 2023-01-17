using Domain.Models.Family;
using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Common;
using UseCase.Family.SaveListFamily;

namespace Interactor.Family;

public class SaveListFamilyInteractor : ISaveListFamilyInputPort
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private const string FREE_WORD = "0000999";
    private readonly Dictionary<string, int> dicZokugaraCd = new Dictionary<string, int>() {
                                                                                                {"GF1",1 },
                                                                                                {"GM1",1 },
                                                                                                {"GF2",1 },
                                                                                                {"GM2",1 },
                                                                                                {"FA",1 },
                                                                                                {"MO",1 },
                                                                                                {"MA",1 },
                                                                                                {"BB",10 },
                                                                                                {"BS",10 },
                                                                                                {"LB",10 },
                                                                                                {"LS",10 },
                                                                                                {"SO",10 },
                                                                                                {"DA",10 },
                                                                                                {"GC",10 },
                                                                                                {"BR",10 },
                                                                                                {"OT",10 },
                                                                                            };

    public SaveListFamilyInteractor(IFamilyRepository familyRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IMstItemRepository mstItemRepository)
    {
        _familyRepository = familyRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveListFamilyOutputData Handle(SaveListFamilyInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult != SaveListFamilyStatus.ValidateSuccess)
            {
                return new SaveListFamilyOutputData(validateResult);
            }
            return new SaveListFamilyOutputData(SaveListFamilyStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private List<FamilyModel> ConvertToListFamily(List<FamilyInputItem> listFamilyInput)
    {
        List<FamilyModel> result = new();

        return result;
    }

    private SaveListFamilyStatus ValidateData(SaveListFamilyInputData input)
    {
        // validate simple param
        if (input.HpId <= 0 || !_hpInfRepository.CheckHpId(input.HpId))
        {
            return SaveListFamilyStatus.InvalidHpId;
        }
        else if (input.UserId <= 0 || !_userRepository.CheckExistedUserId(input.UserId))
        {
            return SaveListFamilyStatus.InvalidUserId;
        }

        // validate data
        var listPtId = input.ListFamily.Where(item => item.FamilyPtId > 0).Select(item => item.FamilyPtId).ToList();
        listPtId.Add(input.PtId);
        if (input.PtId <= 0 || !_patientInforRepository.CheckExistListId(listPtId))
        {
            return SaveListFamilyStatus.InvalidPtIdOrFamilyPtId;
        }
        return ValidateListFamilyInputItem(input.HpId, input.ListFamily);
    }

    private SaveListFamilyStatus ValidateListFamilyInputItem(int hpId, List<FamilyInputItem> listFamily)
    {
        List<FamilyRekiInputItem> listFamilyReki = new();
        // validate familyId
        var listFamilyId = listFamily.Where(item => item.FamilyId > 0).Select(item => item.FamilyId).ToList();
        var listOnlyFamlily = _familyRepository.GetListFamilyForValidate(hpId, listFamilyId);
        if (listOnlyFamlily.Count != listFamilyId.Count)
        {
            return SaveListFamilyStatus.InvalidFamilyId;
        }

        foreach (var input in listFamily)
        {
            // validate ZokugaraCd, consider input value then compare with data in database to validate
            if (!dicZokugaraCd.Keys.Contains(input.ZokugaraCd))
            {
                return SaveListFamilyStatus.InvalidZokugaraCd;
            }
            var totalItemSameZokugaraCd = listOnlyFamlily.Count(item => item.ZokugaraCd.Equals(input.ZokugaraCd)) + 1;
            if (totalItemSameZokugaraCd > dicZokugaraCd[input.ZokugaraCd])
            {
                return SaveListFamilyStatus.InvalidZokugaraCd;
            }

            // validate other field
            else if (input.Name.Length > 100)
            {
                return SaveListFamilyStatus.InvalidName;
            }
            else if (input.KanaName.Length > 100)
            {
                return SaveListFamilyStatus.InvalidKanaName;
            }
            else if (input.Sex > 2 || input.Sex < 0)
            {
                return SaveListFamilyStatus.InvalidSex;
            }
            else if (CIUtil.SDateToShowSDate(input.Birthday) == string.Empty)
            {
                return SaveListFamilyStatus.InvalidBirthday;
            }
            else if (input.IsDead > 2 || input.IsDead < 0)
            {
                return SaveListFamilyStatus.InvalidIsDead;
            }
            else if (input.IsSeparated > 2 || input.IsSeparated < 0)
            {
                return SaveListFamilyStatus.InvalidIsSeparated;
            }
            else if (input.Biko.Length > 120)
            {
                return SaveListFamilyStatus.InvalidBiko;
            }
            else if (input.SortNo < 0)
            {
                return SaveListFamilyStatus.InvalidSortNo;
            }
            listFamilyReki.AddRange(input.ListPtFamilyReki);
        }
        // validate FamilyRekiInputItem
        return ValidateListFamilyRekiInputItem(hpId, listFamilyReki);
    }

    private SaveListFamilyStatus ValidateListFamilyRekiInputItem(int hpId, List<FamilyRekiInputItem> listFamilyReki)
    {
        // validate familyRekiId
        var listFamilyRekiId = listFamilyReki.Where(item => item.Id > 0).Select(item => item.Id).ToList();
        if (!_familyRepository.CheckExistListFamilyReki(hpId, listFamilyRekiId))
        {
            return SaveListFamilyStatus.InvalidFamilyRekiId;
        }

        // validate byomei
        var listByomeiCd = listFamilyReki.Select(item => item.ByomeiCd).ToList();
        var listByomei = _mstItemRepository.DiseaseSearch(listByomeiCd);
        foreach (var input in listFamilyReki)
        {

        }
        return SaveListFamilyStatus.ValidateSuccess;
    }
}
