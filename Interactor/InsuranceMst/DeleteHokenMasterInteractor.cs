using Domain.Constant;
using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.DeleteHokenMaster;

namespace Interactor.InsuranceMst
{
    public class DeleteHokenMasterInteractor : IDeleteHokenMasterInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public DeleteHokenMasterInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public DeleteHokenMasterOutputData Handle(DeleteHokenMasterInputData inputData)
        {
            if (inputData.HpId < 0)
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidHpId, string.Empty);

            if (inputData.PrefNo < 0)
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidPrefNo, string.Empty);

            if (inputData.HokenNo < 0)
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidHokenNo, string.Empty);

            if (inputData.HokenEdaNo < 0)
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidHokenEdaNo, string.Empty);

            if (inputData.StartDate < 0)
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidStartDate , string.Empty);

            if (inputData.HokenNo < 900)
            {
                string message = string.Format(ErrorMessage.MessageType_mDel01060, new string[] { "権限がない", "保険マスタ" });
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.InvalidHokenNoLessThan900, message);
            }

            try
            {
                bool result = _insuranceMstReponsitory.DeleteHokenMaster(inputData.HpId, inputData.HokenNo, inputData.HokenEdaNo, inputData.PrefNo, inputData.StartDate);
                if (result)
                    return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.Successful, string.Empty);
                else
                    return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.Failed, string.Empty);
            }
            catch
            {
                return new DeleteHokenMasterOutputData(DeleteHokenMasterStatus.Exception, string.Empty);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
