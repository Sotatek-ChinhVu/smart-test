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
            var listFamily = ConvertToListFamily(inputData.ListFamily);
            if (!_familyRepository.SaveListFamily(inputData.HpId, inputData.UserId, listFamily))
            {
                return new SaveListFamilyOutputData(SaveListFamilyStatus.Failed);
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
        foreach (var family in listFamilyInput)
        {
            result.Add(new FamilyModel(
                                            family.FamilyId,
                                            family.PtId,
                                            family.ZokugaraCd,
                                            family.FamilyPtId,
                                            family.Name,
                                            family.KanaName,
                                            family.Sex,
                                            family.Birthday,
                                            family.IsDead,
                                            family.IsSeparated,
                                            family.Biko,
                                            family.SortNo,
                                            family.IsDeleted,
                                            family.ListPtFamilyReki
                                            .Select(reki => new PtFamilyRekiModel(
                                                                                    reki.Id,
                                                                                    reki.ByomeiCd,
                                                                                    reki.Byomei,
                                                                                    reki.Cmt,
                                                                                    reki.SortNo,
                                                                                    reki.IsDeleted
                                                                                 )).ToList()
                                        ));
        }
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
        var listPtId = input.ListFamily.Select(item => item.PtId).ToList();
        var listFamilyPtId = input.ListFamily.Where(item => item.FamilyPtId > 0).Select(item => item.FamilyPtId).ToList();
        listFamilyPtId.AddRange(listPtId);
        listFamilyPtId.Add(input.PtId);
        listFamilyPtId = listFamilyPtId.Distinct().ToList();
        if (listPtId.Any(id => id <= 0) || input.PtId <= 0 || !_patientInforRepository.CheckExistListId(listFamilyPtId))
        {
            return SaveListFamilyStatus.InvalidPtIdOrFamilyPtId;
        }
        return ValidateListFamilyInputItem(input.HpId, input.PtId, input.ListFamily, listFamilyPtId);
    }

    private SaveListFamilyStatus ValidateListFamilyInputItem(int hpId, long ptId, List<FamilyInputItem> listFamily, List<long> listFamilyPtId)
    {
        List<FamilyRekiInputItem> listFamilyReki = new();
        // validate familyId
        var listFamilyId = listFamily.Where(item => item.FamilyId > 0).Select(item => item.FamilyId).ToList();
        var listOnlyFamlily = _familyRepository.GetListByPtIdId(hpId, ptId);
        if (listOnlyFamlily.Count(item => listFamilyId.Contains(item.FamilyId)) != listFamilyId.Count)
        {
            return SaveListFamilyStatus.InvalidFamilyId;
        }

        foreach (var familyItem in listFamily)
        {
            // check duplicate family member
            if (listOnlyFamlily.Any(item => (familyItem.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                            && item.PtId == familyItem.PtId
                                            && item.FamilyPtId != 0
                                            && item.FamilyPtId == familyItem.FamilyPtId))
            {
                return SaveListFamilyStatus.DuplicateFamily;
            }

            // validate ZokugaraCd, only validate with case family member is main ptId, consider input value then compare with data in database to validate
            if (familyItem.PtId == ptId)
            {
                if (!dicZokugaraCd.Keys.Contains(familyItem.ZokugaraCd))
                {
                    return SaveListFamilyStatus.InvalidZokugaraCd;
                }
                var totalItemSameZokugaraCd = listOnlyFamlily.Count(item => (item.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                                                            && item.ZokugaraCd.Equals(familyItem.ZokugaraCd)) + 1;
                if (totalItemSameZokugaraCd > dicZokugaraCd[familyItem.ZokugaraCd])
                {
                    return SaveListFamilyStatus.InvalidZokugaraCd;
                }
            }

            // if family exist in database have ptId not equal ptId input => return false
            var familyItemUpdate = listOnlyFamlily.FirstOrDefault(item => item.FamilyId == familyItem.FamilyId);
            if (familyItemUpdate != null && familyItemUpdate.PtId != familyItem.PtId)
            {
                return SaveListFamilyStatus.InvalidPtIdOrFamilyPtId;
            }

            // validate other field
            else if (familyItem.Name.Length > 100)
            {
                return SaveListFamilyStatus.InvalidName;
            }
            else if (familyItem.KanaName.Length > 100)
            {
                return SaveListFamilyStatus.InvalidKanaName;
            }
            else if (familyItem.Sex > 2 || familyItem.Sex < 0)
            {
                return SaveListFamilyStatus.InvalidSex;
            }
            else if (familyItem.Birthday != 0 && CIUtil.SDateToShowSDate(familyItem.Birthday) == string.Empty)
            {
                return SaveListFamilyStatus.InvalidBirthday;
            }
            else if (familyItem.IsDead > 2 || familyItem.IsDead < 0)
            {
                return SaveListFamilyStatus.InvalidIsDead;
            }
            else if (familyItem.IsSeparated > 2 || familyItem.IsSeparated < 0)
            {
                return SaveListFamilyStatus.InvalidIsSeparated;
            }
            else if (familyItem.Biko.Length > 120)
            {
                return SaveListFamilyStatus.InvalidBiko;
            }
            else if (familyItem.SortNo < 0)
            {
                return SaveListFamilyStatus.InvalidSortNo;
            }
            listFamilyReki.AddRange(familyItem.ListPtFamilyReki);
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
            // validate byomeiCd and byomei
            if (!input.ByomeiCd.Equals(FREE_WORD))
            {
                var byomeiItem = listByomei.FirstOrDefault(item => item.ByomeiCd.Equals(input.ByomeiCd));
                if (byomeiItem == null)
                {
                    return SaveListFamilyStatus.InvalidByomeiCd;
                }
                else if (byomeiItem.Byomei != input.Byomei)
                {
                    return SaveListFamilyStatus.InvalidByomei;
                }
            }
            if (input.Cmt.Length > 100)
            {
                return SaveListFamilyStatus.InvalidCmt;
            }
            else if (input.SortNo < 0)
            {
                return SaveListFamilyStatus.InvalidSortNo;
            }
        }
        return SaveListFamilyStatus.ValidateSuccess;
    }
}
