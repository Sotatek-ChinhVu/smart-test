using Domain.Models.CalculateModel;
using Interactor.CalculateService;
using UseCase.Receipt.GetInsuranceInf;
using UseCase.Receipt.GetListReceInf;

namespace Interactor.Receipt
{
    public class GetInsuranceInfInteractor : IGetInsuranceInfInputPort
    {
        private readonly ICalculateService _calculateService;

        public GetInsuranceInfInteractor(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        public GetInsuranceInfOutputData Handle(GetInsuranceInfInputData inputData)
        {
            var receInfs = _calculateService.GetListReceInf(inputData);

            if (!receInfs.ReceInfModels.Any()) return new GetInsuranceInfOutputData(new(), GetInsuranceInfStatus.NoData);

            return new GetInsuranceInfOutputData(ConvertToInsuranceInfDto(receInfs), GetInsuranceInfStatus.Successed);
        }

        private List<InsuranceInfDto> ConvertToInsuranceInfDto(ReceInfModelDto receInf)
        {
            var insuranceInfs = new List<InsuranceInfDto>();

            foreach (var item in receInf.ReceInfModels)
            {
                insuranceInfs.Add(new InsuranceInfDto(item.InsuranceName, item.HokenKbn, item.Nissu, item.Tensu, item.IchibuFutan, item.EdaNo, item.Kigo, item.Bango,
                    item.Kohi1ReceKisai, item.Kohi1Id, item.Kohi2ReceKisai, item.Kohi2Id, item.Kohi3ReceKisai, item.Kohi3Id, item.Kohi4ReceKisai,
                    item.Kohi1FutansyaNo, item.Kohi1JyukyusyaNo, item.Kohi2FutansyaNo, item.Kohi2JyukyusyaNo, item.Kohi3FutansyaNo, item.Kohi3JyukyusyaNo,
                    item.Kohi4FutansyaNo, item.Kohi4JyukyusyaNo, item.HokensyaNo));
            }

            return insuranceInfs;
        }
    }
}
