using Domain.Models.InsuranceMst;
using Helper;
using UseCase.InsuranceMst.SaveHokenSyaMst;

namespace Interactor.InsuranceMst
{
    public class SaveHokenSyaMstInteractor : ISaveHokenSyaMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public SaveHokenSyaMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public SaveHokenSyaMstOutputData Handle(SaveHokenSyaMstInputData inputData)
        {
            var modelSave = new HokensyaMstModel(inputData.HpId
                                                , inputData.Name
                                                , inputData.KanaName
                                                , inputData.HoubetuKbn
                                                , inputData.Houbetu
                                                , inputData.HokenKbn
                                                , inputData.PrefNo
                                                , inputData.HokensyaNo
                                                , inputData.Kigo
                                                , inputData.Bango
                                                , inputData.RateHonnin
                                                , inputData.RateKazoku
                                                , inputData.PostCode
                                                , inputData.Address1
                                                , inputData.Address2
                                                , inputData.Tel1
                                                , inputData.IsKigoNa);

            var validations = Validation(modelSave);
            if(validations.Any())
            {
                string msgValidation = string.Empty;
                foreach (var item in validations)
                    msgValidation += string.IsNullOrEmpty(msgValidation) ? item.GetDescription() : $",{item.GetDescription()}";

                return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Failed, msgValidation);
            }
            try
            {
                bool result = _insuranceMstReponsitory.SaveHokenSyaMst(modelSave);

                if (result)
                    return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Successful,string.Empty);
                else
                    return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Failed, string.Empty);
            }
            catch
            {
                return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Failed, string.Empty);
            }
        }

        private IEnumerable<SaveHokenSyaMstValidation> Validation(HokensyaMstModel model)
        {
            var result = new List<SaveHokenSyaMstValidation>();

            if (model.HpId <= 0)
                result.Add(SaveHokenSyaMstValidation.InvalidHpId);

            if (string.IsNullOrEmpty(model.HokensyaNo) || model.HokensyaNo.Length > 8)
                result.Add(SaveHokenSyaMstValidation.InvalidHokensyaNo);

            if(model.KanaName != null && model.KanaName.Length > 100) 
                result.Add(SaveHokenSyaMstValidation.InvalidKanaName);

            if (model.Name != null && model.Name.Length > 100)
                result.Add(SaveHokenSyaMstValidation.InvalidName);

            if (model.HoubetuKbn != null && model.HoubetuKbn.Length > 2)
                result.Add(SaveHokenSyaMstValidation.InvalidHoubetuKbn);

            if (model.Houbetu != null && model.Houbetu.Length > 3)
                result.Add(SaveHokenSyaMstValidation.InvalidHoubetu);

            if (model.HokenKbn < 0)
                result.Add(SaveHokenSyaMstValidation.InvalidHokenKbn);

            if (model.PrefNo < 0)
                result.Add(SaveHokenSyaMstValidation.InvalidPrefNo);

            if (model.Kigo != null && model.Kigo.Length > 80)
                result.Add(SaveHokenSyaMstValidation.InvalidKigo);

            if (model.Bango != null && model.Bango.Length > 80)
                result.Add(SaveHokenSyaMstValidation.InvalidBango);

            if (model.RateHonnin < 0)
                result.Add(SaveHokenSyaMstValidation.InvalidRateHonnin);

            if (model.RateKazoku < 0)
                result.Add(SaveHokenSyaMstValidation.InvalidRateKazoku);

            if (model.PostCode != null && model.PostCode.Length > 7)
                result.Add(SaveHokenSyaMstValidation.InvalidPostCode);

            if (model.Address1 != null && model.Address1.Length > 200)
                result.Add(SaveHokenSyaMstValidation.InvalidAddress1);

            if (model.Address2 != null && model.Address2.Length > 200)
                result.Add(SaveHokenSyaMstValidation.InvalidAddress2);

            if (model.Tel1 != null && model.Tel1.Length > 15)
                result.Add(SaveHokenSyaMstValidation.InvalidTel1);

            return result;
        }
    }
}
