using Domain.Models.ReceSeikyu;
using Helper.Constants;
using Helper.Mapping;
using UseCase.ReceSeikyu.GetList;
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
                    // Get add new list
                    var addedList = listSourceSeikyu.FindAll(item => item.OriginSinYm != item.SinYm && item.SeqNo == 0).ToList();

                    // Add new
                    isSuccessSeikyuProcess = isSuccessSeikyuProcess && _henTukiokureCommandHanlder.InsertNewReceSeikyu(addedList);

                    // Update
                    isSuccessSeikyuProcess = isSuccessSeikyuProcess && _henTukiokureCommandHanlder.UpdateReceSeikyu(listSourceSeikyu.FindAll(u => u.OriginSinYm == u.SinYm).Select(u => u.ReceSeikyu).ToList());
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
