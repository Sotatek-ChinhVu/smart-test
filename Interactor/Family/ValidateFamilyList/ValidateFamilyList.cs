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
        if (ptId <= 0 || ptInfList.Count != familyPtIdList.Count)
        {
            return ValidateFamilyListStatus.InvalidPtIdOrFamilyPtId;
        }
        List<FamilyRekiItem> familyRekiList = new();

        // validate familyId
        var listFamilyId = listFamily.Where(item => item.FamilyId > 0).Select(item => item.FamilyId).Distinct().ToList();
        var onlyFamlilyList = _familyRepository.GetListByPtId(hpId, ptId);
        if (onlyFamlilyList.Exists(item => item.FamilyPtId == ptId) || listFamily.Exists(item => item.FamilyPtId == ptId && !item.IsRevertItem))
        {
            return ValidateFamilyListStatus.FamilyNotAllow;
        }

        // add item to family if don't exist in input list
        var familyIdNotExistDB = onlyFamlilyList.Select(item => item.FamilyId).Distinct()
                                .Where(familyId => !listFamilyId.Contains(familyId))
                                .Distinct()
                                .ToList();
        var familyNotExistDB = onlyFamlilyList.Where(item => familyIdNotExistDB.Contains(item.FamilyId)).ToList();
        foreach (var item in familyNotExistDB)
        {
            listFamily.Add(new FamilyItem(
                               item.FamilyId,
                               item.PtId,
                               item.ZokugaraCd,
                               item.FamilyPtId,
                               item.Name,
                               item.KanaName,
                               item.Sex,
                               item.Birthday,
                               item.IsDead,
                               item.IsSeparated,
                               item.Biko,
                               item.SortNo,
                               false,
                               new(),
                               false));
        }

        foreach (var familyItem in listFamily)
        {
            // check family ptInf information
            if (familyItem.Name.Length > 100)
            {
                return ValidateFamilyListStatus.InvalidNameMaxLength;
            }
            else if (familyItem.KanaName.Length > 100)
            {
                return ValidateFamilyListStatus.InvalidKanaNameMaxLength;
            }
            else if (familyItem.Birthday != 0 && CIUtil.SDateToShowSDate(familyItem.Birthday) == string.Empty)
            {
                return ValidateFamilyListStatus.InvalidBirthday;
            }

            // check duplicate family member
            if (onlyFamlilyList.Select(item => item.FamilyPtId).Distinct().Count(item => item == familyItem.PtId) > 1
                || listFamily.Count(item => item.PtId == familyItem.PtId
                                            && item.FamilyPtId != 0
                                            && !item.IsDeleted
                                            && item.FamilyPtId == familyItem.FamilyPtId) > 1)
            {
                return ValidateFamilyListStatus.DuplicateFamily;
            }

            // validate ZokugaraCd, only validate with case family member is main ptId, consider input value then compare with data in database to validate
            if (familyItem.PtId == ptId)
            {
                if (!dicZokugaraCd.ContainsKey(familyItem.ZokugaraCd))
                {
                    return ValidateFamilyListStatus.InvalidZokugaraCd;
                }

                var totalItemSameZokugaraCd = listFamily.Count(item => item.PtId == ptId
                                                                       && item.ZokugaraCd.Equals(familyItem.ZokugaraCd)
                                                                       && !item.IsDeleted);

                if (totalItemSameZokugaraCd > dicZokugaraCd[familyItem.ZokugaraCd] && !familyItem.IsDeleted)
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
        return ValidateFamilyRekiListInputItem(familyRekiList);
    }

    private ValidateFamilyListStatus ValidateFamilyRekiListInputItem(List<FamilyRekiItem> familyRekiList)
    {
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
