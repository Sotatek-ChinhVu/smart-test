using Domain.Models.InsuranceMst;
using Helper.Common;
using UseCase.InsuranceMst.GetHokenSyaMst;

namespace Interactor.InsuranceMst
{
    public class GetHokenSyaMstInteractor : IGetHokenSyaMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetHokenSyaMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetHokenSyaMstOutputData Handle(GetHokenSyaMstInputData inputData)
        {
            if (inputData.HpId < 0)
                return new GetHokenSyaMstOutputData(new HokensyaMstModel(), GetHokenSyaMstStatus.InvalidHpId);

            if (string.IsNullOrEmpty(inputData.HokensyaNo))
                return new GetHokenSyaMstOutputData(new HokensyaMstModel(), GetHokenSyaMstStatus.InvalidHokenSyaNo);

            if (inputData.HokenKbn < 0)
                return new GetHokenSyaMstOutputData(new HokensyaMstModel(), GetHokenSyaMstStatus.InvalidHokenKbn);

            try
            {
                string houbetuNo = string.Empty;
                string hokensyaNoSearch = string.Empty;
                CIUtil.GetHokensyaHoubetu(inputData.HokensyaNo, ref hokensyaNoSearch, ref houbetuNo);
                var data = _insuranceMstReponsitory.FindHokenSyaMstInf(inputData.HpId
                                                                      , inputData.HokensyaNo
                                                                      , inputData.HokenKbn
                                                                      , houbetuNo
                                                                      , hokensyaNoSearch);

                return new GetHokenSyaMstOutputData(data, GetHokenSyaMstStatus.Successful);
            }
            catch
            {
                return new GetHokenSyaMstOutputData(new HokensyaMstModel(), GetHokenSyaMstStatus.Exception);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}