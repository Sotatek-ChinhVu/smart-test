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
            try
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
                // Check Duplicate Pattern
                if (inputData.ValidModel.ListHokenPattern.Any())
                {
                    var patternHokenOnlyCheckDuplicate = inputData.ValidModel.ListHokenPattern.Where(pattern => pattern.HokenKbn >= 1 && pattern.HokenKbn <= 4 && pattern.IsDeleted == 0);
                    var duplicateQuery = patternHokenOnlyCheckDuplicate.GroupBy(x => new { x.HokenId, x.Kohi1Id, x.Kohi2Id, x.Kohi3Id, x.Kohi4Id })
                                            .Where(g => g.Count() > 1);
                    if (duplicateQuery != null && duplicateQuery.Any())
                    {
                        var paramsMessageCheckDuplicate = new string[] { "同じ組合せの保険・公１・公２・公３・公４を持つ組合せ" };
                        messageError = String.Format(ErrorMessage.MessageType_mEnt00020, paramsMessageCheckDuplicate);
                        return new ValidInsuranceOtherOutputData(false, messageError, ValidInsuranceOtherStatus.InvalidDuplicatePattern);
                    }
                }

                // Check Age
                if (inputData.ValidModel.IsSelectedHokenInfEmptyModel || !inputData.ValidModel.SelectedHokenInfIsShahoOrKokuho)
                {
                    return new ValidInsuranceOtherOutputData(true, String.Empty, ValidInsuranceOtherStatus.Success);
                }

                var checkAge = CheckAge(inputData);
                if(!checkAge.Result)
                {
                    return checkAge;
                }   
                return new ValidInsuranceOtherOutputData(true, messageError, ValidInsuranceOtherStatus.Success);
            }
            catch (Exception)
            {
                return new ValidInsuranceOtherOutputData(true, "Validate Exception", ValidInsuranceOtherStatus.Success);
            }
        }

        private ValidInsuranceOtherOutputData CheckAge(ValidInsuranceOtherInputData inputData)
        {
            var messageError = "";
            if (inputData.ValidModel.Sindate >= 20080401 && (inputData.ValidModel.ListHokenPattern.Count > 0 && inputData.ValidModel.ListHokenInf.Count > 0))
            {
                var patternHokenOnlyCheckAge = inputData.ValidModel.ListHokenPattern.Where(pattern => pattern.IsDeleted == 0 && !pattern.IsExpirated);
                int age = CIUtil.SDateToAge(inputData.ValidModel.PtBirthday, inputData.ValidModel.Sindate);
                // hoken exist in at least 1 pattern
                var inUsedHokens = inputData.ValidModel.ListHokenInf.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && !hoken.IsExpirated
                                                            && patternHokenOnlyCheckAge.Any(pattern => pattern.HokenId == hoken.HokenId));

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
