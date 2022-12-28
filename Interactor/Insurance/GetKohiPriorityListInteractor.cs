using Domain.Models.Insurance;
using UseCase.Insurance.GetKohiPriorityList;

namespace Interactor.Insurance
{
    public class GetKohiPriorityListInteractor : IGetKohiPriorityListInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public GetKohiPriorityListInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public GetKohiPriorityListOutputData Handle(GetKohiPriorityListInputData inputData)
        {
            try
            {
                var datas = _insuranceResponsitory.GetKohiPriorityList();
                if(datas.Any())
                    return new GetKohiPriorityListOutputData(datas, GetKohiPriorityListStatus.Successful);
                else
                    return new GetKohiPriorityListOutputData(datas, GetKohiPriorityListStatus.DataNotFound);
            }
            catch(Exception ex)
            {
                return new GetKohiPriorityListOutputData(new List<KohiPriorityModel>(), GetKohiPriorityListStatus.Exception);
            }
            finally
            {
                _insuranceResponsitory.ReleaseResource();
            }
        }
    }
}
