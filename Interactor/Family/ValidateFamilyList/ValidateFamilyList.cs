using Domain.Models.Family;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Helper.Common;
using UseCase.Family;

namespace Interactor.Family.ValidateFamilyList;

public class ValidateFamilyList : IValidateFamilyList
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IPatientInforRepository _patientInforRepository;
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
                                                                 {"OT",10 }};

    public ValidateFamilyList(IFamilyRepository familyRepository, IPatientInforRepository patientInforRepository, IMstItemRepository mstItemRepository)
    {
        _familyRepository = familyRepository;
        _patientInforRepository = patientInforRepository;
        _mstItemRepository = mstItemRepository;
    }

    public void ReleaseResource()
    {
        _familyRepository.ReleaseResource();
        _patientInforRepository.ReleaseResource();
        _mstItemRepository.ReleaseResource();
    }

    public ValidateFamilyListStatus ValidateData(int hpId, long ptId, List<FamilyItem> listFamily)
    {
        // validate data
        var ptIdList = listFamily.Select(item => item.PtId).ToList();
        var familyPtIdList = listFamily.Where(item => item.FamilyPtId > 0).Select(item => item.FamilyPtId).ToList();
        familyPtIdList.AddRange(ptIdList);
        familyPtIdList.Add(ptId);
        familyPtIdList = familyPtIdList.Distinct().ToList();
        var ptInfList = _patientInforRepository.SearchPatient(hpId, familyPtIdList);
        if (familyPtIdList.Any(id => id <= 0) || ptId <= 0 || ptInfList.Count != familyPtIdList.Count)
        {
            return ValidateFamilyListStatus.InvalidPtIdOrFamilyPtId;
        }
        List<FamilyRekiItem> familyRekiList = new();

        // validate familyId
        var listFamilyId = listFamily.Where(item => item.FamilyId > 0).Select(item => item.FamilyId).ToList();
        var onlyFamlilyList = _familyRepository.GetListByPtId(hpId, ptId);
        if (onlyFamlilyList.Count(item => listFamilyId.Contains(item.FamilyId)) != listFamilyId.Count)
        {
            return ValidateFamilyListStatus.InvalidFamilyId;
        }

        foreach (var familyItem in listFamily)
        {
            // check family ptInf information
            if (familyItem.FamilyPtId > 0)
            {
                var familyPtInf = ptInfList.FirstOrDefault(item => item.PtId == familyItem.FamilyPtId);
                if (familyPtInf == null)
                {
                    return ValidateFamilyListStatus.InvalidPtIdOrFamilyPtId;
                }
                else if (familyItem.KanaName != familyPtInf.KanaName)
                {
                    return ValidateFamilyListStatus.InvalidKanaName;
                }
                else if (familyItem.Name != familyPtInf.Name)
                {
                    return ValidateFamilyListStatus.InvalidName;
                }
                else if (familyItem.Birthday != familyPtInf.Birthday)
                {
                    return ValidateFamilyListStatus.InvalidBirthday;
                }
                else if (familyItem.Sex != familyPtInf.Sex)
                {
                    return ValidateFamilyListStatus.InvalidSex;
                }
            }

            if (familyItem.Name.Length > 100)
            {
                return ValidateFamilyListStatus.InvalidNameMaxLength;
            }
            else if (familyItem.KanaName.Length > 100)
            {
                return ValidateFamilyListStatus.InvalidKanaNameMaxLength;
            }
            else if (familyItem.Sex < 1 || familyItem.Sex > 2)
            {
                return ValidateFamilyListStatus.InvalidSex;
            }
            else if (familyItem.Birthday != 0 && CIUtil.SDateToShowSDate(familyItem.Birthday) == string.Empty)
            {
                return ValidateFamilyListStatus.InvalidBirthday;
            }

            // check duplicate family member
            if (onlyFamlilyList.Any(item => (familyItem.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                            && item.PtId == familyItem.PtId
                                            && item.FamilyPtId != 0
                                            && item.FamilyPtId == familyItem.FamilyPtId))
            {
                return ValidateFamilyListStatus.DuplicateFamily;
            }

            // validate ZokugaraCd, only validate with case family member is main ptId, consider input value then compare with data in database to validate
            if (familyItem.PtId == ptId)
            {
                if (!dicZokugaraCd.Keys.Contains(familyItem.ZokugaraCd))
                {
                    return ValidateFamilyListStatus.InvalidZokugaraCd;
                }
                var totalItemSameZokugaraCd = onlyFamlilyList.Count(item => (item.FamilyId == 0 || item.FamilyId != familyItem.FamilyId)
                                                                            && item.ZokugaraCd.Equals(familyItem.ZokugaraCd)) + 1;
                if (totalItemSameZokugaraCd > dicZokugaraCd[familyItem.ZokugaraCd])
                {
                    return ValidateFamilyListStatus.InvalidZokugaraCd;
                }
            }

            // if family exist in database have ptId not equal ptId input => return false
            var familyItemUpdate = onlyFamlilyList.FirstOrDefault(item => item.FamilyId == familyItem.FamilyId);
            if (familyItemUpdate != null && familyItemUpdate.PtId != familyItem.PtId)
            {
                return ValidateFamilyListStatus.InvalidPtIdOrFamilyPtId;
            }

            // validate other field
            else if (familyItem.IsDead > 2 || familyItem.IsDead < 0)
            {
                return ValidateFamilyListStatus.InvalidIsDead;
            }
            else if (familyItem.IsSeparated > 2 || familyItem.IsSeparated < 0)
            {
                return ValidateFamilyListStatus.InvalidIsSeparated;
            }
            else if (familyItem.Biko.Length > 120)
            {
                return ValidateFamilyListStatus.InvalidBiko;
            }
            else if (familyItem.SortNo < 0)
            {
                return ValidateFamilyListStatus.InvalidSortNo;
            }
            familyRekiList.AddRange(familyItem.PtFamilyRekiList);
        }
        // validate FamilyRekiInputItem
        return ValidateFamilyRekiListInputItem(hpId, familyRekiList);
    }

    private ValidateFamilyListStatus ValidateFamilyRekiListInputItem(int hpId, List<FamilyRekiItem> familyRekiList)
    {
        // validate familyRekiId
        var listFamilyRekiId = familyRekiList.Where(item => item.Id > 0).Select(item => item.Id).Distinct().ToList();
        if (!_familyRepository.CheckExistFamilyRekiList(hpId, listFamilyRekiId))
        {
            return ValidateFamilyListStatus.InvalidFamilyRekiId;
        }

        // validate byomei
        var byomeiCdList = familyRekiList.Select(item => item.ByomeiCd).Distinct().ToList();
        var byomeiList = _mstItemRepository.DiseaseSearch(byomeiCdList);
        foreach (var input in familyRekiList)
        {
            // validate byomeiCd and byomei
            if (!input.ByomeiCd.Equals(FREE_WORD))
            {
                var byomeiItem = byomeiList.FirstOrDefault(item => item.ByomeiCd.Equals(input.ByomeiCd));
                if (byomeiItem == null)
                {
                    return ValidateFamilyListStatus.InvalidByomeiCd;
                }
                else if (byomeiItem.Byomei != input.Byomei)
                {
                    return ValidateFamilyListStatus.InvalidByomei;
                }
            }
            if (input.Cmt.Length > 100)
            {
                return ValidateFamilyListStatus.InvalidCmt;
            }
            else if (input.SortNo < 0)
            {
                return ValidateFamilyListStatus.InvalidSortNo;
            }
        }
        return ValidateFamilyListStatus.ValidateSuccess;
    }
}
