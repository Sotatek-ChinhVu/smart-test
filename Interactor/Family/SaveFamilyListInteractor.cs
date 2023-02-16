using Domain.Models.Family;
using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Common;
using UseCase.Family;
using UseCase.Family.SaveFamilyList;

namespace Interactor.Family;

public class SaveFamilyListInteractor : ISaveFamilyListInputPort
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

    public SaveFamilyListInteractor(IFamilyRepository familyRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IMstItemRepository mstItemRepository)
    {
        _familyRepository = familyRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveFamilyListOutputData Handle(SaveFamilyListInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult != SaveFamilyListStatus.ValidateSuccess)
            {
                return new SaveFamilyListOutputData(validateResult);
            }
            var familyList = ConvertToFamilyList(inputData.ListFamily);
            if (!_familyRepository.SaveFamilyList(inputData.HpId, inputData.UserId, familyList))
            {
                return new SaveFamilyListOutputData(SaveFamilyListStatus.Failed);
            }
            return new SaveFamilyListOutputData(SaveFamilyListStatus.Successed);
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

    private List<FamilyModel> ConvertToFamilyList(List<FamilyItem> familyInputList)
    {
        List<FamilyModel> result = new();
        foreach (var family in familyInputList)
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
                                            family.PtFamilyRekiList
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

    private SaveFamilyListStatus ValidateData(SaveFamilyListInputData input)
    {
        // validate simple param
        if (input.HpId <= 0 || !_hpInfRepository.CheckHpId(input.HpId))
        {
            return SaveFamilyListStatus.InvalidHpId;
        }
        else if (input.UserId <= 0 || !_userRepository.CheckExistedUserId(input.UserId))
        {
            return SaveFamilyListStatus.InvalidUserId;
        }

        // validate data
        var ptIdList = input.ListFamily.Select(item => item.PtId).ToList();
        var familyPtIdList = input.ListFamily.Where(item => item.FamilyPtId > 0).Select(item => item.FamilyPtId).ToList();
        familyPtIdList.AddRange(ptIdList);
        familyPtIdList.Add(input.PtId);
        familyPtIdList = familyPtIdList.Distinct().ToList();
        if (ptIdList.Any(id => id <= 0) || input.PtId <= 0 || !_patientInforRepository.CheckExistIdList(familyPtIdList))
        {
            return SaveFamilyListStatus.InvalidPtIdOrFamilyPtId;
        }
        return ValidateFamilyListInputItem(input.HpId, input.PtId, input.ListFamily);
    }

    private SaveFamilyListStatus ValidateFamilyListInputItem(int hpId, long ptId, List<FamilyItem> listFamily)
    {
        List<FamilyRekiItem> familyRekiList = new();
        // validate familyId
        var listFamilyId = listFamily.Where(item => item.FamilyId > 0).Select(item => item.FamilyId).ToList();
        var onlyFamlilyList = _familyRepository.GetListByPtId(hpId, ptId);
        if (onlyFamlilyList.Count(item => listFamilyId.Contains(item.FamilyId)) != listFamilyId.Count)
        {
            return SaveFamilyListStatus.InvalidFamilyId;
        }

        foreach (var familyItem in listFamily)
        {
            // check duplicate family member
            if (onlyFamlilyList.Any(item => (familyItem.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                            && item.PtId == familyItem.PtId
                                            && item.FamilyPtId != 0
                                            && item.FamilyPtId == familyItem.FamilyPtId))
            {
                return SaveFamilyListStatus.DuplicateFamily;
            }

            // validate ZokugaraCd, only validate with case family member is main ptId, consider input value then compare with data in database to validate
            if (familyItem.PtId == ptId)
            {
                if (!dicZokugaraCd.Keys.Contains(familyItem.ZokugaraCd))
                {
                    return SaveFamilyListStatus.InvalidZokugaraCd;
                }
                var totalItemSameZokugaraCd = onlyFamlilyList.Count(item => (item.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                                                            && item.ZokugaraCd.Equals(familyItem.ZokugaraCd)) + 1;
                if (totalItemSameZokugaraCd > dicZokugaraCd[familyItem.ZokugaraCd])
                {
                    return SaveFamilyListStatus.InvalidZokugaraCd;
                }
            }

            // if family exist in database have ptId not equal ptId input => return false
            var familyItemUpdate = onlyFamlilyList.FirstOrDefault(item => item.FamilyId == familyItem.FamilyId);
            if (familyItemUpdate != null && familyItemUpdate.PtId != familyItem.PtId)
            {
                return SaveFamilyListStatus.InvalidPtIdOrFamilyPtId;
            }

            // validate other field
            else if (familyItem.Name.Length > 100)
            {
                return SaveFamilyListStatus.InvalidName;
            }
            else if (familyItem.KanaName.Length > 100)
            {
                return SaveFamilyListStatus.InvalidKanaName;
            }
            else if (familyItem.Sex > 2 || familyItem.Sex < 0)
            {
                return SaveFamilyListStatus.InvalidSex;
            }
            else if (familyItem.Birthday != 0 && CIUtil.SDateToShowSDate(familyItem.Birthday) == string.Empty)
            {
                return SaveFamilyListStatus.InvalidBirthday;
            }
            else if (familyItem.IsDead > 2 || familyItem.IsDead < 0)
            {
                return SaveFamilyListStatus.InvalidIsDead;
            }
            else if (familyItem.IsSeparated > 2 || familyItem.IsSeparated < 0)
            {
                return SaveFamilyListStatus.InvalidIsSeparated;
            }
            else if (familyItem.Biko.Length > 120)
            {
                return SaveFamilyListStatus.InvalidBiko;
            }
            else if (familyItem.SortNo < 0)
            {
                return SaveFamilyListStatus.InvalidSortNo;
            }
            familyRekiList.AddRange(familyItem.PtFamilyRekiList);
        }
        // validate FamilyRekiInputItem
        return ValidateFamilyRekiListInputItem(hpId, familyRekiList);
    }

    private SaveFamilyListStatus ValidateFamilyRekiListInputItem(int hpId, List<FamilyRekiItem> familyRekiList)
    {
        // validate familyRekiId
        var listFamilyRekiId = familyRekiList.Where(item => item.Id > 0).Select(item => item.Id).ToList();
        if (!_familyRepository.CheckExistFamilyRekiList(hpId, listFamilyRekiId))
        {
            return SaveFamilyListStatus.InvalidFamilyRekiId;
        }

        // validate byomei
        var byomeiCdList = familyRekiList.Select(item => item.ByomeiCd).ToList();
        var byomeiList = _mstItemRepository.DiseaseSearch(byomeiCdList);
        foreach (var input in familyRekiList)
        {
            // validate byomeiCd and byomei
            if (!input.ByomeiCd.Equals(FREE_WORD))
            {
                var byomeiItem = byomeiList.FirstOrDefault(item => item.ByomeiCd.Equals(input.ByomeiCd));
                if (byomeiItem == null)
                {
                    return SaveFamilyListStatus.InvalidByomeiCd;
                }
                else if (byomeiItem.Byomei != input.Byomei)
                {
                    return SaveFamilyListStatus.InvalidByomei;
                }
            }
            if (input.Cmt.Length > 100)
            {
                return SaveFamilyListStatus.InvalidCmt;
            }
            else if (input.SortNo < 0)
            {
                return SaveFamilyListStatus.InvalidSortNo;
            }
        }
        return SaveFamilyListStatus.ValidateSuccess;
    }
}
