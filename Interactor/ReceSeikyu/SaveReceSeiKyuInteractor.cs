using Domain.Models.ReceSeikyu;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Repositories;
using UseCase.ReceSeikyu.Save;

namespace Interactor.ReceSeikyu
{
    public class SaveReceSeiKyuInteractor : ISaveReceSeiKyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;

        public SaveReceSeiKyuInteractor(IReceSeikyuRepository receptionRepository)
        {
            _receSeikyuRepository = receptionRepository;
        }

        public SaveReceSeiKyuOutputData Handle(SaveReceSeiKyuInputData inputData)
        {
            try
            {
                List<ReceInfo> receInfos = new List<ReceInfo>();

                if (inputData.HpId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidHpId, new List<long>(), 0, receInfos);

                if(inputData.UserAct <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidUserId, new List<long>(), 0 , receInfos);

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

                List<long> calculatePtIds = new List<long>();
                int seikyuYmCalculation = 0;


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

                                calculatePtIds.Add(receSeikyu.PtId);
                                seikyuYmCalculation = receSeikyu.OriginSeikyuYm;

                            }
                            // Case insert new sinym
                            else
                            {
                                calculatePtIds.Add(receSeikyu.PtId);
                                seikyuYmCalculation = receSeikyu.SinYm;

                                // Update receip seikyu from seikyuym = 999999 to new seikyuym
                                _receSeikyuRepository.UpdateSeikyuYmReceipSeikyuIfExist(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId, receSeikyu.SeikyuYm, inputData.UserAct, inputData.HpId);
                            }
                        }
                    }
                }
                #endregion

                if (isSuccessSeikyuProcess && isSuccessCompletedSeikyu)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Successful, calculatePtIds , seikyuYmCalculation, receInfos);
                else
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.Failed, calculatePtIds, seikyuYmCalculation, receInfos);
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
