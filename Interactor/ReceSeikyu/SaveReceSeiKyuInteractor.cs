using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Helper.Constants;
using Helper.Mapping;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Repositories;
using Interactor.CalculateService;
using System.Net.NetworkInformation;
using UseCase.ReceSeikyu.Save;

namespace Interactor.ReceSeikyu
{
    public class SaveReceSeiKyuInteractor : ISaveReceSeiKyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;
        private readonly ICalcultateCustomerService _calcultateCustomerService;
        bool isStopCalc = false;

        public SaveReceSeiKyuInteractor(IReceSeikyuRepository receptionRepository, ICalcultateCustomerService calcultateCustomerService)
        {
            _receSeikyuRepository = receptionRepository;
            _calcultateCustomerService = calcultateCustomerService;
        }

        public SaveReceSeiKyuOutputData Handle(SaveReceSeiKyuInputData inputData)
        {
            try
            {
                List<ReceInfo> receInfos = new List<ReceInfo>();

                if (inputData.HpId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidHpId);

                if(inputData.UserAct <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidUserId);

                foreach (var modifiedReceSeikyu in inputData.ReceSeiKyus)
                {
                    if (modifiedReceSeikyu.IsAddNew && modifiedReceSeikyu.IsDeleted == 1) continue;

                    if (modifiedReceSeikyu.IsAddNew)
                    {
                        receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SeikyuYm));

                        if (modifiedReceSeikyu.IsChecked)
                        {
                            receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SeikyuYm));
                        }
                    }
                    else if (modifiedReceSeikyu.IsDeleted == DeleteTypes.None)
                    {
                        _receSeikyuRepository.EntryDeleteHenJiyuu(modifiedReceSeikyu.PtId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.HokenId , inputData.UserAct);

                        receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SinYm));


                        receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.OriginSeikyuYm));

                    }
                    else if (modifiedReceSeikyu.SeikyuYm != modifiedReceSeikyu.OriginSeikyuYm)
                    {
                        if (modifiedReceSeikyu.SeikyuYm == 999999)
                        {
                            receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.OriginSeikyuYm));
                        }
                        else if (modifiedReceSeikyu.SeikyuYm > 0)
                        {
                            receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SeikyuYm));

                            if (modifiedReceSeikyu.OriginSeikyuYm > 0 && modifiedReceSeikyu.OriginSeikyuYm != 999999)
                            {
                                receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.OriginSeikyuYm));
                            }
                        }
                    }
                }

                #region not complete seikyu
                bool isSuccessSeikyuProcess = true;
                var listSourceSeikyu = inputData.ReceSeiKyus.Where(item => item.IsNotCompletedSeikyu).ToList();
                if (listSourceSeikyu != null && listSourceSeikyu.Count > 0)
                {
                    foreach (ReceSeikyuModel model in listSourceSeikyu)
                    {
                        if (model.IsChecked == true && model.SeikyuYm == 999999)
                        {
                            model.SetSeikyuYm(inputData.SinYm);
                        }
                    }
                    // Add new
                    isSuccessSeikyuProcess = isSuccessSeikyuProcess && _receSeikyuRepository.SaveReceSeiKyu(inputData.HpId, inputData.UserAct, listSourceSeikyu);
                }
                #endregion

                #region complete seikyu
                bool isSuccessCompletedSeikyu = true;
                var deletedSourceList = inputData.ReceSeiKyus.Where(item => item.IsCompletedSeikyu).ToList();
                if (deletedSourceList != null && deletedSourceList.Count > 0)
                {
                    var insertDefaultList = new List<ReceSeikyuModel>();
                    foreach (var receSeikyu in deletedSourceList)
                    {
                        if (listSourceSeikyu != null && listSourceSeikyu.Any(p => p.PtId == receSeikyu.PtId && p.SinYm == receSeikyu.SinYm &&
                            p.HokenId == receSeikyu.HokenId && p.IsDeleted == DeleteTypes.None)) continue;

                        var entityClone = new ReceSeikyuModel(receSeikyu.SinDay,
                                                              receSeikyu.HpId,
                                                              receSeikyu.PtId,
                                                              receSeikyu.PtName,
                                                              receSeikyu.SinYm,
                                                              receSeikyu.ReceListSinYm,
                                                              receSeikyu.HokenId,
                                                              receSeikyu.HokensyaNo,
                                                              receSeikyu.SeqNo,
                                                              receSeikyu.SeikyuYm,
                                                              receSeikyu.SeikyuYmBinding,
                                                              receSeikyu.SeikyuKbn,
                                                              receSeikyu.PreHokenId,
                                                              receSeikyu.Cmt,
                                                              receSeikyu.IsChecked,
                                                              receSeikyu.PtNum,
                                                              receSeikyu.HokenKbn,
                                                              receSeikyu.Houbetu,
                                                              receSeikyu.HokenStartDate,
                                                              receSeikyu.HokenEndDate,
                                                              receSeikyu.IsModified,
                                                              receSeikyu.OriginSeikyuYm,
                                                              receSeikyu.OriginSinYm,
                                                              receSeikyu.IsAddNew,
                                                              receSeikyu.IsDeleted,
                                                              receSeikyu.ListRecedenHenJiyuuModel);
                        // Uncheck and deleted = insert 999999 record
                        if (!receSeikyu.IsChecked // Uncheck = remove
                            || receSeikyu.SinYm != receSeikyu.OriginSinYm // Add new
                            || receSeikyu.IsDeleted == 1) // Delete record
                        {
                            entityClone.SetSeikyuYm(999999);
                        }
                        receSeikyu.SetSeikyuYm(receSeikyu.OriginSeikyuYm);
                        insertDefaultList.Add(entityClone);
                    }

                    // Insert new rece_seikyu record with seikyuym = 999999
                    isSuccessCompletedSeikyu = _receSeikyuRepository.InsertNewReceSeikyu(insertDefaultList, inputData.UserAct , inputData.HpId);
                    if (isSuccessCompletedSeikyu)
                    {
                        foreach (var receSeikyu in deletedSourceList)
                        {
                            if (listSourceSeikyu != null && listSourceSeikyu.Any(p => p.PtId == receSeikyu.PtId && p.SinYm == receSeikyu.SinYm &&
                                p.HokenId == receSeikyu.HokenId && p.IsDeleted == DeleteTypes.None)) continue;

                            // Case update seikyuym
                            if (receSeikyu.SinYm == receSeikyu.OriginSinYm)
                            {
                                // Delete update seikyu record
                                _receSeikyuRepository.RemoveReceSeikyuDuplicateIfExist(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId, inputData.UserAct, inputData.HpId);


                                //Call httpClient 
                                _calcultateCustomerService.RunCaculationPostAsync<string>(TypeCalculate.ReceFutanCalculateMain, new
                                {
                                    PtIds = new List<long>() { receSeikyu.PtId },
                                    SeikyuYm = receSeikyu.SeikyuYm
                                }).Wait();
                            }
                            // Case insert new sinym
                            else
                            {
                                _calcultateCustomerService.RunCaculationPostAsync<string>(TypeCalculate.ReceFutanCalculateMain, new
                                {
                                    PtIds = new List<long>() { receSeikyu.PtId },
                                    SeikyuYm = receSeikyu.SinYm
                                }).Wait();

                                // Update receip seikyu from seikyuym = 999999 to new seikyuym
                                _receSeikyuRepository.UpdateSeikyuYmReceipSeikyuIfExist(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId, receSeikyu.SeikyuYm, inputData.UserAct, inputData.HpId);
                            }
                        }
                    }
                }
                #endregion

                if(receInfos.Any())
                {

                    //Rece Calculation
                    int totalRecord = receInfos.Count;
                    if (!isStopCalc)
                    {
                        for (int i = 0; i < receInfos.Count; i++)
                        {
                            _calcultateCustomerService.RunCaculationPostAsync<string>(TypeCalculate.RunCalculateMonth, new
                            {
                                HpId = inputData.HpId,
                                SeikyuYm = receInfos[i].SeikyuYm,
                                PtIds = new List<long> { receInfos[i].PtId }
                            }).Wait();
                            Messenger.Instance.Send(new RecalculateInSeikyuPendingStatus($"計算処理中.. 残り[{(receInfos.Count - (i + 1))}件]です", (int)Math.Round((double)(100 * (i + 1)) / totalRecord)));
                        }
                    }

                    

                    

                }

                if (isSuccessSeikyuProcess && isSuccessCompletedSeikyu)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Successful);
                else
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Failed);
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
