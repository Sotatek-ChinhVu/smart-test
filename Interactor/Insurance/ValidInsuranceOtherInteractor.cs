using Domain.Constant;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidPatternOther;

namespace Interactor.Insurance
{
    public class ValidInsuranceOtherInteractor : IValidInsuranceOtherInputPort
    {
        public ValidInsuranceOtherOutputData Handle(ValidInsuranceOtherInputData inputData)
        {
            if (inputData.ValidModel.Sindate < 0)
            {
                return new ValidInsuranceOtherOutputData(false, String.Empty, ValidInsuranceOtherStatus.InvalidSindate);
            }
            
            if (inputData.ValidModel.PtBirthday < 0)
            {
                return new ValidInsuranceOtherOutputData(false, String.Empty, ValidInsuranceOtherStatus.InvalidPtBirthday);
            }

            var messageError = "";
            if (inputData.ValidModel.IsSelectedHokenInfEmptyModel || !inputData.ValidModel.SelectedHokenInfIsShahoOrKokuho)
            {
                return new ValidInsuranceOtherOutputData(true, String.Empty, ValidInsuranceOtherStatus.Success);
            }

            if (inputData.ValidModel.Sindate >= 20080401 && (inputData.ValidModel.ListHokenPattern.Count > 0 && inputData.ValidModel.ListHokenInf.Count > 0))
            {
                var PatternHokenOnly = inputData.ValidModel.ListHokenPattern.Where(pattern => pattern.IsDeleted == 0 && !pattern.IsExpirated);

                int age = CIUtil.SDateToAge(inputData.ValidModel.PtBirthday, inputData.ValidModel.Sindate);

                // hoken exist in at least 1 pattern
                var inUsedHokens = inputData.ValidModel.ListHokenInf.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && !hoken.IsExpirated
                                                            && PatternHokenOnly.Any(pattern => pattern.HokenId == hoken.HokenId));

                var elderHokenQuery = inUsedHokens.Where(hoken => hoken.EndDate >= inputData.ValidModel.Sindate
                                                                        && hoken.HokensyaNo != null && hoken.HokensyaNo != ""
                                                                        && hoken.HokensyaNo.Length == 8 && hoken.HokensyaNo.StartsWith("39"));
                if (elderHokenQuery != null)
                {
                    if (age >= 75 && !elderHokenQuery.Any())
                    {
                        var paramsMessage75 = new string[] { "後期高齢者保険が入力されていません。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage75);
                        return new ValidInsuranceOtherOutputData(false, messageError, ValidInsuranceOtherStatus.InvalidAge75);
                    }
                    else if (age < 65 && elderHokenQuery.Any())
                    {
                        var paramsMessage65 = new string[] { "後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage65);
                        return new ValidInsuranceOtherOutputData(true, messageError, ValidInsuranceOtherStatus.InvalidAge65);
                    }
                }
            }
            return new ValidInsuranceOtherOutputData(true, messageError, ValidInsuranceOtherStatus.Success);
        }
    }
}
