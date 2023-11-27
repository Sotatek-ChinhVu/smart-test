using Domain.Models.SetGenerationMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SetSendaiGeneration.Restore;
using UseCase.SetSendaiGeneration.Delete;
using Helper.Messaging;
using Helper.Messaging.Data;
using Domain.Models.SetMst;

namespace Interactor.SetSendaiGeneration
{
    public class RestoreSetSendaiGenerationInteractor : IRestoreSetSendaiGenerationInputPort
    {
        private readonly ISetGenerationMstRepository _setGenerationMstRepository;
        private readonly ISetMstRepository _setMstRepository;
        private IMessenger? _messenger;

        public RestoreSetSendaiGenerationInteractor(ISetGenerationMstRepository setGenerationMstRepository, ISetMstRepository setMstRepository)
        {
            _setGenerationMstRepository = setGenerationMstRepository;
            _setMstRepository = setMstRepository;
        }

        public RestoreSetSendaiGenerationOutputData Handle(RestoreSetSendaiGenerationInputData inputData)
        {
            _messenger = inputData.Messenger;
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidHpId);
                }

                if (inputData.UserId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidUserId);
                }

                if (inputData.RestoreGenerationId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidRestoreGenerationId);
                }

                var result = _setGenerationMstRepository.RestoreSetSendaiGeneration(inputData.RestoreGenerationId, inputData.HpId, inputData.UserId);
                if (result != null)
                {
                    // Process Clone
                    var getCountProcess = _setGenerationMstRepository.GetCountStepProcess(result.TargetGeneration, result.SourceGeneration, inputData.HpId, inputData.UserId);
                    if (getCountProcess != null && getCountProcess.TotalCount > 0)
                    {
                        var totalCountCheck = getCountProcess.TotalCount;
                        // Update success -> process save data
                        _messenger.Send(new ProcessSetSendaiGenerationStatus($"Add MstBackup Sucess!", (int)Math.Round((double)(100 * getCountProcess.SetMstsBackupedCount) / getCountProcess.TotalCount), false, false));
                        totalCountCheck -= getCountProcess.SetMstsBackupedCount;

                        // setkbn
                        if (getCountProcess.SetKbnMstSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetKbnMstSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount;
                            // save setkbn source
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneKbnMst(result.TargetGeneration, result.SourceGeneration, inputData.HpId, inputData.UserId);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add SetKbnMst Successs!" : $"Add SetKbnMst Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //Byomei
                        if (getCountProcess.SetByomeisSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetByomeisSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount;
                            // save Byomei
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneByomei(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add Byomei Successs!" : $"Add Byomei Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //KarteInf
                        if (getCountProcess.SetKarteInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetKarteInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount;
                            // save KarteInf
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneKarteInf(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add KarteInf Successs!" : $"Add KarteInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //KarteImgInf
                        if (getCountProcess.SetKarteImgInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetKarteImgInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount;
                            // save KarteImgInf
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneKarteImgInf(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add KarteImgInf Successs!" : $"Add KarteImgInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInf
                        if (getCountProcess.SetOdrInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount;
                            // save OdrInf
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneOdrInf(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInf Successs!" : $"Add OdrInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInfDetail
                        if (getCountProcess.SetOdrInfDetailsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfDetailsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount + getCountProcess.SetOdrInfDetailsSourceCount;
                            // save OdrInfDetail
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneOdrInfDetail(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInfDetail Successs!" : $"Add OdrInfDetail Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInfCmt
                        if (getCountProcess.SetOdrInfCmtSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfCmtSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount + getCountProcess.SetOdrInfDetailsSourceCount + getCountProcess.SetOdrInfCmtSourceCount;
                            // save OdrInfCmt
                            var saveSetKbn = _setGenerationMstRepository.SaveCloneOdrInfCmt(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInfCmt Successs!" : $"Add OdrInfCmt Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }
                    }
                    else
                    {
                        // Update faild. Stop process
                        _messenger.Send(new ProcessSetSendaiGenerationStatus($"Add MstBackup Faild!", 0, false, false));
                    }
                    _setGenerationMstRepository.ReloadCache(inputData.HpId, true);
                    _setMstRepository.DeleteKey(result.TargetGeneration);
                    return new RestoreSetSendaiGenerationOutputData(true, RestoreSetSendaiGenerationStatus.Success);
                }
                return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.Faild);
            }
            finally
            {
                _setGenerationMstRepository.ReleaseResource();
                _setMstRepository.ReleaseResource();
            }
        }
    }
}