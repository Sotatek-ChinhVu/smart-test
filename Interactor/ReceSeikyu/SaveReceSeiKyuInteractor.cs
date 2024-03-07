using Domain.Models.Diseases;
using Domain.Models.DrugDetail;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.ReceSeikyu.Save;
using Interactor.Receipt;
using System.Text;

namespace Interactor.ReceSeikyu
{
    public class SaveReceSeiKyuInteractor : ISaveReceSeiKyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;
        private readonly ICalcultateCustomerService _calcultateCustomerService;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IInsuranceMstRepository _insuranceMstRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IPtDiseaseRepository _ptDiseaseRepository;
        private readonly ICommonMedicalCheck _commonMedicalCheck;
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly IDrugDetailRepository _drugDetailRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        private readonly ICommonReceRecalculation _commonReceRecalculation;
        private IMessenger? _messenger;

        public SaveReceSeiKyuInteractor(ITenantProvider tenantProvider, IReceSeikyuRepository receptionRepository, ICalcultateCustomerService calcultateCustomerService
            , IReceiptRepository receiptRepository, ISystemConfRepository systemConfRepository, IInsuranceMstRepository insuranceMstRepository, IMstItemRepository mstItemRepository
            , IPtDiseaseRepository ptDiseaseRepository, IOrdInfRepository ordInfRepository, ICommonMedicalCheck commonMedicalCheck, ITodayOdrRepository todayOdrRepository, IDrugDetailRepository drugDetailRepository, ICommonReceRecalculation commonReceRecalculation)
        {
            _receSeikyuRepository = receptionRepository;
            _calcultateCustomerService = calcultateCustomerService;
            _receiptRepository = receiptRepository;
            _systemConfRepository = systemConfRepository;
            _insuranceMstRepository = insuranceMstRepository;
            _mstItemRepository = mstItemRepository;
            _ptDiseaseRepository = ptDiseaseRepository;
            _ordInfRepository = ordInfRepository;
            _commonMedicalCheck = commonMedicalCheck;
            _todayOdrRepository = todayOdrRepository;
            _drugDetailRepository = drugDetailRepository;
            _tenantProvider = tenantProvider;
            _commonReceRecalculation = commonReceRecalculation;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveReceSeiKyuOutputData Handle(SaveReceSeiKyuInputData inputData)
        {
            _messenger = inputData.Messenger;
            try
            {
                List<ReceInfo> receInfos = new List<ReceInfo>();

                if (inputData.HpId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidHpId);

                if (inputData.UserId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidUserId);

                foreach (var modifiedReceSeikyu in inputData.ReceSeiKyus)
                {
                    if (modifiedReceSeikyu.IsAddNew && modifiedReceSeikyu.IsDeleted == 1) continue;
                    if (modifiedReceSeikyu.IsAddNew)
                    {
                        receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SinYm));

                        if (modifiedReceSeikyu.IsChecked)
                        {
                            receInfos.Add(new ReceInfo(modifiedReceSeikyu.PtId, modifiedReceSeikyu.HokenId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.SeikyuYm));
                        }
                    }
                    else if (modifiedReceSeikyu.IsDeleted == DeleteTypes.Deleted)
                    {
                        //_receSeikyuRepository.EntryDeleteHenJiyuu(inputData.HpId, modifiedReceSeikyu.PtId, modifiedReceSeikyu.SinYm, modifiedReceSeikyu.HokenId, inputData.UserId);

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
                var listSourceSeikyu = inputData.ReceSeiKyus.Where(item => item.IsNotCompletedSeikyu && item.IsModified).ToList();
                if (listSourceSeikyu != null && listSourceSeikyu.Count > 0)
                {
                    foreach (ReceSeikyuModel model in listSourceSeikyu)
                    {
                        if (model.IsChecked && model.SeikyuYm == 999999)
                        {
                            model.SetSeikyuYm(inputData.SinYm);
                        }
                    }
                    // Add new
                    isSuccessSeikyuProcess = isSuccessSeikyuProcess && _receSeikyuRepository.SaveReceSeiKyu(inputData.HpId, inputData.UserId, listSourceSeikyu);
                }
                #endregion

                #region complete seikyu
                bool isSuccessCompletedSeikyu = true;
                var deletedSourceList = inputData.ReceSeiKyus.Where(item => item.IsCompletedSeikyu && item.IsModified).ToList();
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
                                                              receSeikyu.SeikyuKbn,
                                                              receSeikyu.PreHokenId,
                                                              receSeikyu.Cmt,
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
                                                              receSeikyu.IsChecked,
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
                    isSuccessCompletedSeikyu = _receSeikyuRepository.InsertNewReceSeikyu(insertDefaultList, inputData.UserId, inputData.HpId);
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
                                _receSeikyuRepository.RemoveReceSeikyuDuplicateIfExist(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId, inputData.UserId, inputData.HpId);

                                //Call httpClient 
                                _calcultateCustomerService.RunCaculationPostAsync(TypeCalculate.ReceFutanCalculateMain, new
                                {
                                    PtIds = new List<long>() { receSeikyu.PtId },
                                    SeikyuYm = receSeikyu.SeikyuYm
                                }).Wait();
                            }

                            // Case insert new sinym
                            else
                            {
                                _calcultateCustomerService.RunCaculationPostAsync(TypeCalculate.ReceFutanCalculateMain, new
                                {
                                    PtIds = new List<long>() { receSeikyu.PtId },
                                    SeikyuYm = receSeikyu.SinYm
                                }).Wait();

                                // Update receip seikyu from seikyuym = 999999 to new seikyuym
                                _receSeikyuRepository.UpdateSeikyuYmReceipSeikyuIfExist(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId, receSeikyu.SeikyuYm, inputData.UserId, inputData.HpId);
                            }
                        }
                    }
                }
                #endregion

                var errorText = _commonReceRecalculation.RecalculateInSeikyuPending(_messenger, inputData.HpId, inputData.UserId, receInfos);
                if (isSuccessSeikyuProcess && isSuccessCompletedSeikyu)
                {
                    _messenger.Send(new RecalculateInSeikyuPendingStatus(errorText, 100, true, true));
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Successful);
                }
                else
                {
                    _messenger.Send(new RecalculateInSeikyuPendingStatus(errorText, 100, true, false));
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Failed);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
                _receiptRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _insuranceMstRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
                _ptDiseaseRepository.ReleaseResource();
                _ordInfRepository.ReleaseResource();
                _commonMedicalCheck.ReleaseResource();
                _todayOdrRepository.ReleaseResource();
                _drugDetailRepository.ReleaseResource();
                _tenantProvider.DisposeDataContext();
                _commonReceRecalculation.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
