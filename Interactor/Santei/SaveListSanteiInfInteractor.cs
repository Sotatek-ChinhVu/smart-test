using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Santei;
using Domain.Models.User;
using UseCase.Santei.SaveListSanteiInf;
using Helper.Common;

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
            var listSanteiInfs = ConvertToSanteiInfModel(inputData.PtId, inputData.ListSanteiInfs);
            var listSanteiInfDetails = ConvertToSanteiInfDetailModel(inputData.PtId, inputData.ListSanteiInfs);
            if (_santeiInfRepository.SaveSantei(inputData.HpId, inputData.UserId, listSanteiInfs, listSanteiInfDetails))
            {
                return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Successed);
            }
            return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Failed);
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
        else if (!_santeiInfRepository.CheckExistItemCd(input.HpId, input.ListSanteiInfs.Select(item => item.ItemCd).ToList()))
        {
            return SaveListSanteiInfStatus.InvalidItemCd;
        }

        // get list of SanteiInfs that doesn't contain deleted item in DB to validate SanteiInf
        var listSanteiInfs = _santeiInfRepository.GetOnlyListSanteiInf(input.HpId, input.PtId);
        var listSanteiIdDeletes = input.ListSanteiInfs.Where(item => item.IsDeleted).Select(item => item.Id).ToList();
        listSanteiInfs = listSanteiInfs.Where(item => !listSanteiIdDeletes.Contains(item.Id)).ToList();

        // get list of SanteiInfDetails to validate SanteiInfDetail
        var listSanteiInfDetails = _santeiInfRepository.GetListSanteiInfDetailModel(input.HpId, input.PtId);

        // get list of Byomeis to validate 
        var listByomeis = _santeiInfRepository.GetListSanteiByomeis(input.HpId, input.PtId, input.SinDate, input.HokenPid);
        if (listSanteiInfDetails.Any())
        {
            listByomeis.AddRange(from item in listSanteiInfDetails// Add byomei to byomei List
                                 where listByomeis.FirstOrDefault(byomei => byomei.Equals(item.Byomei)) == null && !string.IsNullOrEmpty(item.Byomei)
                                 select item.Byomei);
        }

        foreach (var santeiInf in input.ListSanteiInfs)
        {
            // validate itemCd, itemCd is not duplicate
            if (santeiInf.Id <= 0 && listSanteiInfs.Any(item => item.ItemCd == santeiInf.ItemCd))
            {
                return SaveListSanteiInfStatus.InvalidItemCd;
            }
            else if (santeiInf.Id > 0 && !listSanteiInfs.Any(item => item.Id == santeiInf.Id && item.ItemCd == santeiInf.ItemCd))
            {
                return SaveListSanteiInfStatus.InvalidItemCd;
            }

            // validate AlertTerm, AlertTerm must exist in dictionary
            //    { 2, "日" },
            //    { 3, "暦週" },
            //    { 4, "暦月" },
            //    { 5, "週" },
            //    { 6, "月" },
            else if (santeiInf.AlertTerm < 2 || santeiInf.AlertTerm > 6)
            {
                return SaveListSanteiInfStatus.InvalidAlertTerm;
            }

            // validate AlertDays
            else if (santeiInf.AlertDays < 0 && santeiInf.AlertDays > int.MaxValue)
            {
                return SaveListSanteiInfStatus.InvalidAlertDays;
            }

            // if itemCd of santeiInf have any SanteiInfDetail, not allow
            else if ((santeiInf.ItemCd.StartsWith("KN") || santeiInf.ItemCd.StartsWith("IGE")) && santeiInf.ListSanteInfDetails.Any())
            {
                return SaveListSanteiInfStatus.ThisSanteiInfDoesNotAllowSanteiInfDetail;
            }

            // validate list SanteiInfDetails
            foreach (var santeiInfDetail in santeiInf.ListSanteInfDetails)
            {
                // validate KisanSbt, KisanSbt must exist in dictionary
                //{ 0, string.Empty },
                //{ 1, "初回算定" },
                //{ 2, "発症" },
                //{ 3, "急性憎悪" },
                //{ 4, "治療開始" },
                //{ 5, "手術" },
                //{ 6, "初回診断" }
                if (santeiInfDetail.KisanSbt < 1 || santeiInfDetail.KisanSbt > 6)
                {
                    return SaveListSanteiInfStatus.InvalidKisanSbt;
                }

                // validate EndDate
                else if (CIUtil.SDateToShowSDate(santeiInfDetail.EndDate) == string.Empty)
                {
                    return SaveListSanteiInfStatus.InvalidEndDate;
                }

                // validate KisanDate
                else if (CIUtil.SDateToShowSDate(santeiInfDetail.KisanDate) == string.Empty)
                {
                    return SaveListSanteiInfStatus.InvalidKisanDate;
                }

                // validate Byomei
                else if (!listByomeis.Contains(santeiInfDetail.Byomei))
                {
                    return SaveListSanteiInfStatus.InvalidByomei;
                }

                // validate HosokuComment
                else if (santeiInfDetail.HosokuComment.Length > 80)
                {
                    return SaveListSanteiInfStatus.InvalidHosokuComment;
                }
            }
        }
        return SaveListSanteiInfStatus.ValidateSuccess;
    }

    private List<SanteiInfModel> ConvertToSanteiInfModel(long ptId, List<SanteiInfInputItem> listSanteiInfInputs)
    {
        return listSanteiInfInputs.Select(item => new SanteiInfModel(
                                                                        item.Id,
                                                                        ptId,
                                                                        item.ItemCd,
                                                                        item.AlertDays,
                                                                        item.AlertTerm,
                                                                        item.IsDeleted
                                                                    )).ToList();
    }

    private List<SanteiInfDetailModel> ConvertToSanteiInfDetailModel(long ptId, List<SanteiInfInputItem> listSanteiInfInputs)
    {
        List<SanteiInfDetailModel> result = new();
        foreach (var santaiInf in listSanteiInfInputs)
        {
            var listSantaiIntDetails = santaiInf.ListSanteInfDetails.Select(item => new SanteiInfDetailModel(
                                                                                                                item.Id,
                                                                                                                ptId,
                                                                                                                santaiInf.ItemCd,
                                                                                                                item.EndDate,
                                                                                                                item.KisanSbt,
                                                                                                                item.KisanDate,
                                                                                                                item.Byomei,
                                                                                                                item.HosokuComment,
                                                                                                                item.Comment,
                                                                                                                santaiInf.IsDeleted ? santaiInf.IsDeleted : item.IsDeleted
                                                                                                            )).ToList();
            result.AddRange(listSantaiIntDetails);
        }
        return result;
    }
}
