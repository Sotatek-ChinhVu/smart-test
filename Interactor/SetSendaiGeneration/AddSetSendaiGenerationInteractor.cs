﻿using Domain.Models.SetGenerationMst;
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
        private readonly ISetGenerationMstRepository _inputItemRepository;
        private IMessenger? _messenger;
        bool isStopProcess = false;

        public AddSetSendaiGenerationInteractor(ISetGenerationMstRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
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

                var result = _inputItemRepository.AddSetSendaiGeneration(inputData.UserId, inputData.HpId, inputData.StartDate);
                if (result != null)
                {
                    // Process Clone
                    var getCountProcess = _inputItemRepository.GetCountStepProcess(result.TargetGeneration, result.SourceGeneration, inputData.HpId, inputData.UserId);
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
                            var saveSetKbn = _inputItemRepository.SaveCloneKbnMst(result.TargetGeneration, result.SourceGeneration, inputData.HpId, inputData.UserId);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add SetKbnMst Successs!" : $"Add SetKbnMst Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //Byomei
                        if (getCountProcess.SetByomeisSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetByomeisSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount;
                            // save Byomei
                            var saveSetKbn = _inputItemRepository.SaveCloneByomei(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add Byomei Successs!" : $"Add Byomei Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //KarteInf
                        if (getCountProcess.SetKarteInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetKarteInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount;
                            // save KarteInf
                            var saveSetKbn = _inputItemRepository.SaveCloneKarteInf(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add KarteInf Successs!" : $"Add KarteInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //KarteImgInf
                        if (getCountProcess.SetKarteImgInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetKarteImgInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount;
                            // save KarteImgInf
                            var saveSetKbn = _inputItemRepository.SaveCloneKarteImgInf(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add KarteImgInf Successs!" : $"Add KarteImgInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInf
                        if (getCountProcess.SetOdrInfsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount;
                            // save OdrInf
                            var saveSetKbn = _inputItemRepository.SaveCloneOdrInf(inputData.HpId, inputData.UserId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInf Successs!" : $"Add OdrInf Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInfDetail
                        if (getCountProcess.SetOdrInfDetailsSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfDetailsSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount + getCountProcess.SetOdrInfDetailsSourceCount;
                            // save OdrInfDetail
                            var saveSetKbn = _inputItemRepository.SaveCloneOdrInfDetail(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInfDetail Successs!" : $"Add OdrInfDetail Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }

                        //OdrInfCmt
                        if (getCountProcess.SetOdrInfCmtSourceCount > 0 && totalCountCheck > 0)
                        {
                            totalCountCheck -= getCountProcess.SetOdrInfCmtSourceCount;
                            var countResult = getCountProcess.SetMstsBackupedCount + getCountProcess.SetKbnMstSourceCount + getCountProcess.SetByomeisSourceCount + getCountProcess.SetKarteInfsSourceCount + getCountProcess.SetKarteImgInfsSourceCount + getCountProcess.SetOdrInfsSourceCount + getCountProcess.SetOdrInfDetailsSourceCount + getCountProcess.SetOdrInfCmtSourceCount;
                            // save OdrInfCmt
                            var saveSetKbn = _inputItemRepository.SaveCloneOdrInfCmt(inputData.HpId, getCountProcess.ListSetMst, getCountProcess.ListDictContain);
                            _messenger.Send(new ProcessSetSendaiGenerationStatus(saveSetKbn ? $"Add OdrInfCmt Successs!" : $"Add OdrInfCmt Faid!", (int)Math.Round((double)(100 * countResult) / getCountProcess.TotalCount), false, false));
                        }
                    }
                    else
                    {
                        // Update faild. Stop process
                        _messenger.Send(new ProcessSetSendaiGenerationStatus($"Add MstBackup Faild!", 0, false, false));
                    }
                    return new AddSetSendaiGenerationOutputData(true, AddSetSendaiGenerationStatus.Success);
                }
                return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.Faild);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}