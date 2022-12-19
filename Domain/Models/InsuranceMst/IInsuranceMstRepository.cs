using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public interface IInsuranceMstRepository
    {
        InsuranceMstModel GetDataInsuranceMst(int hpId, long ptId, int sinDate);

        IEnumerable<HokensyaMstModel> SearchListDataHokensyaMst(int hpId, int sinDate, string keyword);

        HokenMstModel GetHokenMstByFutansyaNo(int hpId, int sinDate, string futansyaNo);

        bool SaveHokenSyaMst(HokensyaMstModel model, int userId);

        HokensyaMstModel FindHokenSyaMstInf(int hpId, string hokensyaNo, int hokenKbn, string houbetuNo, string hokensyaNoSearch);
    }
}
