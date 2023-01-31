using Domain.Constant;
using Domain.Models.InsuranceMst;
using Helper.Common;
using UseCase.InsuranceMst.SaveHokenMaster;

namespace Interactor.InsuranceMst
{
    public class SaveHokenMasterInteractor : ISaveHokenMasterInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public SaveHokenMasterInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public SaveHokenMasterOutputData Handle(SaveHokenMasterInputData inputData)
        {
            string message = string.Empty;
            if(inputData.HpId <= 0)
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidHpId, string.Empty);

            if (inputData.Insurance.IsAdded && inputData.Insurance.HokenNo < 900)
            {
                message = string.Format(ErrorMessage.MessageType_mInp00060, new string[] { "保険番号", "900" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidHokenNo, message);
            }

            if (inputData.Insurance.StartDate != 0 && string.IsNullOrEmpty(CIUtil.SDateToShowSDate(inputData.Insurance.StartDate)))
            {
                message = string.Format(ErrorMessage.MessageType_mNG01010, new string[] { "適用開始日" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidStartDate, message);
            }  

            if (inputData.Insurance.EndDate != 99999999 && string.IsNullOrEmpty(CIUtil.SDateToShowSDate(inputData.Insurance.EndDate)))
            {
                message = string.Format(ErrorMessage.MessageType_mNG01010, new string[] { "適用終了日" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidEndDate, message);
            }

            if (inputData.Insurance.StartDate > inputData.Insurance.EndDate)
            {
                message = string.Format(ErrorMessage.MessageType_mInp00110, new[] { "適用終了日 ", "適用開始日" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidStartDateMoreThanEndDate, message);
            }

            if (inputData.Insurance.PrefNo > 47)
            {
                message = string.Format(ErrorMessage.MessageType_mInp00050, new string[] { "都道府県番号", "0", "47" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidPrefNo, message);
            }

            if (_insuranceMstReponsitory.CheckDuplicateKey(inputData.HpId, inputData.Insurance))
            {
                message = string.Format(ErrorMessage.MessageType_mEnt01020, new string[] { "保険番号" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidDuplicateKey, message);
            }
            if (inputData.Insurance.AgeStart > inputData.Insurance.AgeEnd)
            {
                message = string.Format(ErrorMessage.MessageType_mInp00110, new string[] { "年齢条件終了", "年齢条件開始" });
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.InvalidDuplicateKey, message);
            }

            try
            {
                bool resultSuccess = false;
                if (inputData.Insurance.IsAdded)
                    resultSuccess = _insuranceMstReponsitory.CreateHokenMaster(inputData.HpId, inputData.UserId, inputData.Insurance);
                else
                    resultSuccess = _insuranceMstReponsitory.UpdateHokenMaster(inputData.HpId, inputData.UserId, inputData.Insurance);

                if (resultSuccess)
                    return new SaveHokenMasterOutputData(SaveHokenMasterStatus.Successful, string.Empty);
                else
                    return new SaveHokenMasterOutputData(SaveHokenMasterStatus.Failed, string.Empty);
            }
            catch
            {
                return new SaveHokenMasterOutputData(SaveHokenMasterStatus.Exception, string.Empty);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
