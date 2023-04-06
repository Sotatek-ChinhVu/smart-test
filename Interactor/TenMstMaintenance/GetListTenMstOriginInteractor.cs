using Domain.Models.TenMstMaintenance;
using UseCase.TenMstMaintenance.GetListTenMstOrigin;

namespace Interactor.TenMstMaintenance
{
    public class GetListTenMstOriginInteractor : IGetListTenMstOriginInputPort
    {
        private readonly ITenMstMaintenanceRepository _tenMstMaintenanceRepository;

        public GetListTenMstOriginInteractor(ITenMstMaintenanceRepository tenMstMaintenanceRepository)
        {
            _tenMstMaintenanceRepository = tenMstMaintenanceRepository;
        }

        public GetListTenMstOriginOutputData Handle(GetListTenMstOriginInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new GetListTenMstOriginOutputData(new List<TenMstOriginModel>(), GetListTenMstOriginStatus.InvalidItemCd);

                var tenMstModelLists = _tenMstMaintenanceRepository.GetGroupTenMst(inputData.ItemCd);
                if (!tenMstModelLists.Any())
                    return new GetListTenMstOriginOutputData(tenMstModelLists, GetListTenMstOriginStatus.NoData);
                else
                    return new GetListTenMstOriginOutputData(tenMstModelLists, GetListTenMstOriginStatus.Successful);
            }
            finally
            {
                _tenMstMaintenanceRepository.ReleaseResource();
            }
        }
    }
}
