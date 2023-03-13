using Domain.Models.ReceSeikyu;
using Helper.Constants;
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
                if (inputData.HpId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidHpId, string.Empty);

                if(inputData.UserAct <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidUserId, string.Empty);

                List<ReceInfo> receInfos = new List<ReceInfo>();

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
                    var insertDefaultList = new List<Entity.Tenant.ReceSeikyu>();
                    foreach (var receSeikyu in deletedSourceList)
                    {
                        if (listSourceSeikyu != null && listSourceSeikyu.Any(p => p.PtId == receSeikyu.PtId && p.SinYm == receSeikyu.SinYm &&
                            p.HokenId == receSeikyu.HokenId && p.IsDeleted == DeleteTypes.None)) continue;

                        var entity = receSeikyu.ReceSeikyu.DeepClone();
                        // Uncheck and deleted = insert 999999 record
                        if (!receSeikyu.IsChecked // Uncheck = remove
                            || receSeikyu.SinYm != receSeikyu.OriginSinYm // Add new
                            || receSeikyu.IsDeleted == 1) // Delete record
                        {
                            entity.SeikyuYm = 999999;
                        }
                        receSeikyu.ReceSeikyu.SeikyuYm = receSeikyu.OriginSeikyuYm;
                        insertDefaultList.Add(entity);
                    }
                    // Insert new rece_seikyu record with seikyuym = 999999
                    isSuccessCompletedSeikyu = _henTukiokureCommandHanlder.InsertNewReceSeikyu(insertDefaultList);
                    if (isSuccessCompletedSeikyu)
                    {
                        foreach (var receSeikyu in deletedSourceList)
                        {
                            if (listSourceSeikyu.Any(p => p.PtId == receSeikyu.PtId && p.SinYm == receSeikyu.SinYm &&
                                p.HokenId == receSeikyu.HokenId && p.IsDeleted == DeleteTypes.None)) continue;

                            // Case update seikyuym
                            if (receSeikyu.SinYm == receSeikyu.OriginSinYm)
                            {
                                // Delete update seikyu record
                                ReceSeikyu receSeikyuDuplicate = _registerRequestFinder.GetReceSeikyuDuplicate(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId);
                                if (receSeikyuDuplicate != null)
                                {
                                    receSeikyuDuplicate.IsDeleted = 1;
                                    _henTukiokureCommandHanlder.UpdateReceSeikyu(new List<ReceSeikyu>() { receSeikyuDuplicate });
                                }

                                ReceFutanViewModel receFutanVM = new ReceFutanViewModel();
                                receFutanVM.ReceFutanCalculateMain(new List<long>() { receSeikyu.PtId }, receSeikyu.OriginSeikyuYm);
                                receFutanVM.Dispose();
                            }
                            // Case insert new sinym
                            else
                            {
                                // Calculation for remove record from receinf
                                ReceFutanViewModel receFutanVM = new ReceFutanViewModel();
                                receFutanVM.ReceFutanCalculateMain(new List<long>() { receSeikyu.PtId }, receSeikyu.SinYm);
                                receFutanVM.Dispose();

                                // Update receip seikyu from seikyuym = 999999 to new seikyuym
                                ReceSeikyu receSeikyuUpdate = _registerRequestFinder.GetReceSeikyuForUpdate(receSeikyu.PtId, receSeikyu.SinYm, receSeikyu.HokenId);
                                if (receSeikyuUpdate != null)
                                {
                                    receSeikyuUpdate.SeikyuYm = receSeikyu.SeikyuYm;
                                    _henTukiokureCommandHanlder.UpdateReceSeikyu(new List<ReceSeikyu>() { receSeikyuUpdate });
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
