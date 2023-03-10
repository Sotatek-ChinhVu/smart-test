using Domain.Models.CalculateModel;
using Entity.Tenant;
using Interactor.CalculateService;
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


            return new GetInsuranceInfOutputData(GetInsuranceInfStatus.Successed);
        }

        private void EditReceInf(ReceInfModelDto receInf)
        {
            //CreateNewInsuranceInf();

            //if (receInf == null) return;

            //if (receInf.HokenKbn <= 2)
            //{
            //    EditReceInfByHoken();
            //}
            //else if (receInf.HokenKbn >= 11 && receInf.HokenKbn <= 13)
            //{
            //    EditReceInfByRosai();
            //}
            //else if (receInf.HokenKbn == 14)
            //{
            //    EditReceInfByJibai();
            //}

            //InsuranceInf.Nissu = receInf.receInfModels.Nissu;
            //InsuranceInf.Tensu = FormatIntToString(_receInf.Tensu);
            //// 2020.03.18 Fix comment #2798
            //// Using PtFutan instead of IchibuFutan from ReceInf
            //InsuranceInf.IchibuFutan = FormatIntToString(_receInf.PtFutan);

            //if (string.IsNullOrWhiteSpace(InsuranceInf.Tensu))
            //    InsuranceInf.Tensu = "0";
            //if (string.IsNullOrWhiteSpace(InsuranceInf.IchibuFutan))
            //    InsuranceInf.IchibuFutan = "0";
        }
    }
}
