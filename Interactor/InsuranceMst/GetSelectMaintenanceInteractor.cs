using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.GetSelectMaintenance;

namespace Interactor.InsuranceMst
{
    public class GetSelectMaintenanceInteractor : IGetSelectMaintenanceInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetSelectMaintenanceInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetSelectMaintenanceOutputData Handle(GetSelectMaintenanceInputData inputData)
        {
            if (inputData.HpId <= 0)
                return new GetSelectMaintenanceOutputData(new List<SelectMaintenanceModel>(), GetSelectMaintenanceStatus.InvalidHpId);

            if(inputData.HokenNo < 0)
                return new GetSelectMaintenanceOutputData(new List<SelectMaintenanceModel>(), GetSelectMaintenanceStatus.InvalidHokenNo);

            if (inputData.HokenEdaNo < 0)
                return new GetSelectMaintenanceOutputData(new List<SelectMaintenanceModel>(), GetSelectMaintenanceStatus.InvalidHokenEdaNo);

            if (inputData.PrefNo < 0)
                return new GetSelectMaintenanceOutputData(new List<SelectMaintenanceModel>(), GetSelectMaintenanceStatus.InvalidPrefNo);

            if (inputData.StartDate < 0)
                return new GetSelectMaintenanceOutputData(new List<SelectMaintenanceModel>(), GetSelectMaintenanceStatus.InvalidStartDate);

            try
            {
                var datas = _insuranceMstReponsitory.GetSelectMaintenance(inputData.HpId, inputData.HokenNo, inputData.HokenEdaNo, inputData.PrefNo, inputData.StartDate);
                if (datas.Any())
                    return new GetSelectMaintenanceOutputData(datas, GetSelectMaintenanceStatus.Successful);
                else
                    return new GetSelectMaintenanceOutputData(datas, GetSelectMaintenanceStatus.DataNotFound);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
