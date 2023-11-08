using Domain.Models.SetGenerationMst;
using Helper.Messaging;
using Helper.Messaging.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SetSendaiGeneration.Add;
using UseCase.SetSendaiGeneration.Delete;

namespace Interactor.SetSendaiGeneration
{
    public class AddSetSendaiGenerationInteractor : IAddSetSendaiGenerationInputPort
    {
        private readonly ISetGenerationMstRepository _setGenerationMstRepository;
        private IMessenger? _messenger;

        public AddSetSendaiGenerationInteractor(ISetGenerationMstRepository setGenerationMstRepository)
        {
            _setGenerationMstRepository = setGenerationMstRepository;
        }

        public AddSetSendaiGenerationOutputData Handle(AddSetSendaiGenerationInputData inputData)
        {
            _messenger = inputData.Messenger;
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidHpId);
                }

                if (inputData.StartDate <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidStartDate);
                }

                if (inputData.UserId <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidUserId);
                }

                var result = _setGenerationMstRepository.AddSetSendaiGeneration(inputData.UserId, inputData.HpId, inputData.StartDate);
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
                    return new AddSetSendaiGenerationOutputData(true, AddSetSendaiGenerationStatus.Success);
                }
                return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.Faild);
            }
            finally
            {
                _setGenerationMstRepository.ReleaseResource();
            }
        }
    }
}