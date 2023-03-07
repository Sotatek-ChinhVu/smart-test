using Domain.Models.Insurance;
using Infrastructure.Interfaces;
using UseCase.Schema.GetListInsuranceScan;

namespace Interactor.Schema
{
    public class GetListInsuranceScanInteractor : IGetListInsuranceScanInputPort
    {
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly IInsuranceRepository _insuranceRepository;

        public GetListInsuranceScanInteractor(IAmazonS3Service amazonS3Service, IInsuranceRepository insuranceRepository)
        {
            _amazonS3Service = amazonS3Service;
            _insuranceRepository = insuranceRepository;
        }

        public GetListInsuranceScanOutputData Handle(GetListInsuranceScanInputData inputData)
        {
            var datas = new List<InsuranceScanModel>();
            try
            {
                if (inputData.HpId < 0)
                    return new GetListInsuranceScanOutputData(GetListInsuranceScanStatus.InvalidHpId, datas);

                if (inputData.PtId < 0)
                    return new GetListInsuranceScanOutputData(GetListInsuranceScanStatus.InvalidPtId, datas);

                datas = _insuranceRepository.GetListInsuranceScanByPtId(inputData.HpId, inputData.PtId);

                if (!datas.Any())
                    return new GetListInsuranceScanOutputData(GetListInsuranceScanStatus.NoData, datas);

                datas.ForEach(x =>
                {
                    x.SetDisplayImage(_amazonS3Service.GetAccessBaseS3());
                });
                return new GetListInsuranceScanOutputData(GetListInsuranceScanStatus.Successful, datas);
            }
            finally
            {
                _insuranceRepository.ReleaseResource();
            }
        }
    }
}
