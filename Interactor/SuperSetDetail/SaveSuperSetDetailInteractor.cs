﻿using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

namespace Interactor.SuperSetDetail;

public class SaveSuperSetDetailInteractor : ISaveSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly ISetMstRepository _setMstRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IUserRepository _userRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;
    private readonly AmazonS3Options _options;
    private const string SUSPECTED_CD = "8002";
    private const string FREE_WORD = "0000999";

    public SaveSuperSetDetailInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, ITenantProvider tenantProvider, ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository, ISetMstRepository setMstRepository, IUserRepository userRepository, IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
        _userRepository = userRepository;
        _setMstRepository = setMstRepository;
        _userRepository = userRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveSuperSetDetailOutputData Handle(SaveSuperSetDetailInputData inputData)
    {
        try
        {
            var statusValidate = ValidateSuperSetDetail(inputData);
            if (statusValidate != SaveSuperSetDetailStatus.ValidateSuccess)
            {
                return new SaveSuperSetDetailOutputData(statusValidate);
            }

            var result = _superSetDetailRepository.SaveSuperSetDetail(
                                                                        inputData.SetCd,
                                                                        inputData.UserId,
                                                                        inputData.HpId,
                                                                        ConvertToListSetByomeiModel(inputData.SetByomeiModelInputs),
                                                                        ConvertToSetKarteInfModel(inputData.SaveSetKarteInputItem),
                                                                        ConvertToListSetOrderInfModel(inputData.SaveSetOrderInputItems)
                                                                    );
            switch (result)
            {
                case 1:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetByomeiFailed);
                case 2:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetKarteInfFailed);
                case 3:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetOrderInfFailed);
            }

            if (inputData.FileItem.IsUpdateFile)
            {
                if (result == 0)
                {
                    var listFileItems = inputData.FileItem.ListFileItems;
                    if (!listFileItems.Any())
                    {
                        listFileItems = new List<string> { string.Empty };
                    }
                    SaveSetFile(inputData.HpId, inputData.PtId, inputData.SetCd, listFileItems, true);
                    return new SaveSuperSetDetailOutputData(result, SaveSuperSetDetailStatus.Successed);
                }
                else
                {
                    SaveSetFile(inputData.HpId, inputData.PtId, inputData.SetCd, inputData.FileItem.ListFileItems, false);
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.Failed);
                }
            }
            return new SaveSuperSetDetailOutputData(result, SaveSuperSetDetailStatus.Successed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
            _setMstRepository.ReleaseResource();
            _superSetDetailRepository.ReleaseResource();
        }
    }

    private void SaveSetFile(int hpId, long ptId, int setCd, List<string> listFileName, bool saveSuccess)
    {
        List<string> listFolders = new();
        string path = string.Empty;
        listFolders.Add(CommonConstants.Store);
        listFolders.Add(CommonConstants.Karte);
        listFolders.Add(CommonConstants.SetPic);
        listFolders.Add(setCd.ToString());
        path = _amazonS3Service.GetFolderUploadOther(listFolders);
        string host = _options.BaseAccessUrl + "/" + path;
        var listUpdates = listFileName.Select(item => item.Replace(host, string.Empty)).ToList();
        if (saveSuccess)
        {
            List<SetFileInfModel> setFileInfModelList = new();
            var ptInf = _patientInforRepository.GetById(hpId, ptId, 0, 0);
            long ptNum = ptInf != null ? ptInf.PtNum : 0;
            var fileInfUpdateTemp = CopyFileFromKarteToSuperSet(ptNum, path, listFileName);
            if (fileInfUpdateTemp.Any())
            {
                foreach (var item in fileInfUpdateTemp)
                {
                    if (item.Key == item.Value)
                    {
                        setFileInfModelList.Add(new SetFileInfModel(false, item.Value));
                    }
                    else
                    {
                        setFileInfModelList.Add(new SetFileInfModel(true, item.Value));
                    }
                }
            }
            _superSetDetailRepository.SaveListSetKarteFile(hpId, setCd, host, setFileInfModelList, false);
        }
        else
        {
            _superSetDetailRepository.ClearTempData(hpId, listUpdates.ToList());
            foreach (var item in listUpdates)
            {
                _amazonS3Service.DeleteObjectAsync(path + item);
            }
        }
    }

    private Dictionary<string, string> CopyFileFromKarteToSuperSet(long ptNum, string pathSaveSet, List<string> listFileFromKarte)
    {
        Dictionary<string, string> fileInfUpdateTemp = new();

        var listFolderPath = new List<string>(){
                                            CommonConstants.Store,
                                            CommonConstants.Karte
                                        };
        string baseAccessUrl = _options.BaseAccessUrl;
        string karteHost = baseAccessUrl + "/" + _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);

        foreach (var oldFileLink in listFileFromKarte)
        {
            string oldFileName = Path.GetFileName(oldFileLink);
            if (oldFileLink.Contains(karteHost))
            {
                string newFile = baseAccessUrl + "/" + pathSaveSet + _amazonS3Service.GetUniqueFileNameKey(oldFileName.Trim());
                var copySuccess = _amazonS3Service.CopyObjectAsync(oldFileLink.Replace(baseAccessUrl, string.Empty), newFile.Replace(baseAccessUrl, string.Empty)).Result;
                if (copySuccess)
                {
                    fileInfUpdateTemp.Add(oldFileName, newFile);
                }
            }
            else
            {
                fileInfUpdateTemp.Add(oldFileName, oldFileName);
            }
        }
        return fileInfUpdateTemp;
    }

    private List<SetByomeiModel> ConvertToListSetByomeiModel(List<SaveSetByomeiInputItem> inputItems)
    {
        List<SetByomeiModel> result = new();
        foreach (var item in inputItems)
        {
            result.Add(new SetByomeiModel(
                            item.Id,
                            item.IsSyobyoKbn,
                            item.SikkanKbn,
                            item.NanByoCd,
                            item.FullByomei,
                            item.IsSuspected,
                            item.IsDspRece,
                            item.IsDspKarte,
                            item.ByomeiCmt,
                            item.ByomeiCd,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            item.PrefixSuffixList.Select(pre =>
                                    new PrefixSuffixModel(
                                            pre.Code,
                                            pre.Name
                                        )
                                ).ToList()
                        )
                );
        }
        return result;
    }

    private SetKarteInfModel ConvertToSetKarteInfModel(SaveSetKarteInputItem inputItem)
    {
        return new SetKarteInfModel(
                inputItem.HpId,
                inputItem.SetCd,
                inputItem.RichText,
                inputItem.Text
            );
    }

    private List<SetOrderInfModel> ConvertToListSetOrderInfModel(List<SaveSetOrderInfInputItem> inputItems)
    {
        List<SetOrderInfModel> listSetInfModels = new();
        foreach (var inputMst in inputItems)
        {
            var listSetOrderInfDetailModels = inputMst.OrdInfDetails.Select(detail =>
                    new SetOrderInfDetailModel(
                            detail.SinKouiKbn,
                            detail.ItemCd,
                            detail.ItemName,
                            detail.ItemName,
                            detail.Suryo,
                            detail.UnitName,
                            detail.UnitSBT,
                            detail.TermVal,
                            detail.KohatuKbn,
                            detail.SyohoKbn,
                            detail.SyohoLimitKbn,
                            detail.DrugKbn,
                            detail.YohoKbn,
                            detail.Kokuji1,
                            detail.Kokuji2,
                            detail.IsNodspRece,
                            detail.IpnCd,
                            detail.IpnName,
                            detail.Bunkatu,
                            detail.CmtName,
                            detail.CmtOpt,
                            detail.FontColor,
                            detail.CommentNewline,
                            string.Empty,
                            string.Empty,
                            0,
                            0,
                            new()
                        )
                ).ToList();
            var model = new SetOrderInfModel(
                    inputMst.Id,
                    inputMst.RpNo,
                    inputMst.RpEdaNo,
                    inputMst.OdrKouiKbn,
                    inputMst.RpName,
                    inputMst.InoutKbn,
                    inputMst.SikyuKbn,
                    inputMst.SyohoSbt,
                    inputMst.SanteiKbn,
                    inputMst.TosekiKbn,
                    inputMst.DaysCnt,
                    inputMst.SortNo,
                    inputMst.IsDeleted,
                    listSetOrderInfDetailModels
                );
            listSetInfModels.Add(model);
        }

        return listSetInfModels;
    }

    private SaveSuperSetDetailStatus ValidateSuperSetDetail(SaveSuperSetDetailInputData inputData)
    {
        var notAllowSave = _userRepository.NotAllowSaveMedicalExamination(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.UserId);
        if (notAllowSave)
        {
            return SaveSuperSetDetailStatus.MedicalScreenLocked;
        }
        else if (inputData.HpId <= 0)
        {
            return SaveSuperSetDetailStatus.InvalidHpId;
        }
        else if (inputData.SetCd < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetCd;
        }
        else if (inputData.UserId <= 0)
        {
            return SaveSuperSetDetailStatus.InvalidUserId;
        }
        else if (!_setMstRepository.CheckExistSetMstBySetCd(inputData.SetCd))
        {
            return SaveSuperSetDetailStatus.SetCdNotExist;
        }

        // Validate SetByomeiModel
        List<string> listByomeiCode = new();
        foreach (var item in inputData.SetByomeiModelInputs)
        {
            listByomeiCode.AddRange(item.PrefixSuffixList.Where(item => item.Code != SUSPECTED_CD).Select(item => item.Code).ToList());
            if (item.ByomeiCd != FREE_WORD)
            {
                listByomeiCode.Add(item.ByomeiCd);
            }
            if (item.Id < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetByomeiId;
            }
            else if (item.SikkanKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSikkanKbn;
            }
            else if (item.NanByoCd < 0)
            {
                return SaveSuperSetDetailStatus.InvalidNanByoCd;
            }
            else if (item.FullByomei.Length > 160)
            {
                return SaveSuperSetDetailStatus.FullByomeiMaxlength160;
            }
            else if (item.ByomeiCmt.Length > 80)
            {
                return SaveSuperSetDetailStatus.ByomeiCmtMaxlength80;
            }
        }
        var listByomeiCd = _mstItemRepository.DiseaseSearch(listByomeiCode).Select(item => item.ByomeiCd).ToList();
        foreach (var item in inputData.SetByomeiModelInputs.Select(item => item.PrefixSuffixList))
        {
            foreach (var presufCode in item.Select(item => item.Code))
            {
                if (!(presufCode == SUSPECTED_CD || presufCode == FREE_WORD || listByomeiCd.Any(code => code == presufCode)))
                {
                    return SaveSuperSetDetailStatus.InvalidByomeiCdOrSyusyokuCd;
                }
            }
        }

        // validate Order
        var listOrderModels = _superSetDetailRepository.GetOnlyListOrderInfModel(inputData.HpId, inputData.SetCd);
        foreach (var mst in inputData.SaveSetOrderInputItems)
        {
            if (mst.Validation() != SaveSuperSetDetailStatus.ValidateOrderSuccess)
            {
                return mst.Validation();
            }
            // check exist RpNo and RpEdaNo
            else if (mst.Id > 0 && !listOrderModels.Any(model => model.Id == mst.Id && model.RpNo == mst.RpNo && model.RpEdaNo == mst.RpEdaNo && model.IsDeleted == 0))
            {
                return SaveSuperSetDetailStatus.RpNoOrRpEdaNoIsNotExist;
            }

            foreach (var detail in mst.OrdInfDetails)
            {
                if (detail.Validate() != SaveSuperSetDetailStatus.ValidateOrderDetailSuccess)
                {
                    return detail.Validate();
                }
            }
        }
        return SaveSuperSetDetailStatus.ValidateSuccess;
    }
}
