using Domain.Models.ReceptionInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidateRousaiJibai;

namespace Interactor.Insurance
{
    public class ValidateRousaiJibaiInteractor : IValidateRousaiJibaiInputPort
    {
        private readonly IReceptionInsuranceRepository _insuranceResponsitory;
        public ValidateRousaiJibaiInteractor(IReceptionInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public ValidateRousaiJibaiOutputData Handle(ValidateRousaiJibaiInputData inputData)
        {
            if (inputData.HokenKbn < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidHokenKbn);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSinDate);
            }

            if (inputData.SelectedHokenInfRousaiSaigaiKbn < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSaigaiKbn);
            }

            if (inputData.SelectedHokenInfRousaiSyobyoDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSyobyoDate);
            }

            if (inputData.SelectedHokenInfRyoyoStartDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoStartDate);
            }

            if (inputData.SelectedHokenInfRyoyoEndDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoEndDate);
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfStartDate);
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfEndDate);
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate);
            }
            
            switch(inputData.HokenKbn)
            {
                // 労災(短期給付)	
                case 11:
                    result = IsValidRodo();
                    break;
                // 労災(傷病年金)
                case 12:
                    result = IsValidNenkin();
                    break;
                // アフターケア
                case 13:
                    result = IsValidKenko();
                    break;
                // 自賠責
                case 14:
                    result = IsValidJibai();
            }

            return new ValidateRousaiJibaiOutputData(true, string.Empty, ValidateRousaiJibaiStatus.InvalidSuccess);
        }
    }
}
