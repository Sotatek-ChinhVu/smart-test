using Domain.Models.InsuranceMst;
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

            if (inputData.HpId < 0)
                return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.InvalidHpID);

            if (string.IsNullOrEmpty(inputData.HokensyaNo))
                return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.InvalidHokenSyaNo);


            bool reuslt = _insuranceMstReponsitory.SaveHokenSyaMst(new HokensyaMstModel(inputData.HpId
                                                                                      ,inputData.Name
                                                                                      ,inputData.KanaName
                                                                                      ,inputData.HoubetuKbn
                                                                                      ,inputData.Houbetu
                                                                                      ,inputData.HokenKbn
                                                                                      ,inputData.PrefNo
                                                                                      ,inputData.HokensyaNo
                                                                                      ,inputData.Kigo
                                                                                      ,inputData.Bango
                                                                                      ,inputData.RateHonnin
                                                                                      ,inputData.RateKazoku
                                                                                      ,inputData.PostCode
                                                                                      ,inputData.Address1
                                                                                      ,inputData.Address2
                                                                                      ,inputData.Tel1));
            if (reuslt)
                return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Successful);
            else return new SaveHokenSyaMstOutputData(SaveHokenSyaMstStatus.Failed);
        }
    }
}
