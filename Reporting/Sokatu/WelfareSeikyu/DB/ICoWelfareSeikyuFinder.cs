using Domain.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.DB;

public interface ICoWelfareSeikyuFinder : IRepositoryBase
{
    /// <summary>
    /// 福祉請求書
    /// </summary>
    /// <param name="seikyuYm">請求年月</param>
    /// <param name="seikyuType">月遅れ・返戻</param>
    /// <param name="kohiHokenNos">保険番号リスト</param>
    /// <param name="futanCheck">公費負担有無</param>
    /// <returns></returns>
    List<CoWelfareReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, List<int> kohiHokenNos, FutanCheck futanCheck, int hokenKbn);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="seikyuYm">請求年月</param>
    /// <param name="seikyuType">月遅れ・返戻</param>
    /// <param name="kohiHoubetus">法別番号リスト</param>
    /// <param name="isKohiFutan">公費負担有無</param>
    /// <param name="isKohiKisai">true:併用レセの公費を含む</param>
    /// <returns></returns>
    List<CoWelfareReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> kohiHoubetus, FutanCheck futanCheck, int hokenKbn, bool isKohiKisai = false);

    CoHpInfModel GetHpInf(int hpId, int seikyuYm);

    List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos);

    List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm);

    /// <summary>
    /// 院外処方の有無
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <returns></returns>
    bool IsOutDrugOrder(int hpId, long ptId, int sinYm);
}