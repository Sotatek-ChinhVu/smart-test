using Domain.Common;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta9000.Models;

namespace Reporting.Statistics.Sta9000.DB;

public interface ICoSta9000Finder : IRepositoryBase
{
    /// <summary>
    /// 患者情報
    /// </summary>
    List<CoPtInfModel> GetPtInfs(
        int hpId,
        CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
        CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
        CoSta9000KensaConf? kensaConf
    );

    /// <summary>
    /// 患者情報
    /// </summary>
    List<CoPtInfModel> GetPtInfs(
        int hpId,
        CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
        CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
        CoSta9000KensaConf? kensaConf, List<long> ptIds
    );

    /// <summary>
    /// 処方一覧
    /// </summary>
    List<CoDrugOdrModel> GetDrugOrders(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 病名情報
    /// </summary>
    List<CoPtByomeiModel> GetPtByomeis(
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 保険情報
    /// </summary>
    List<CoPtHokenModel> GetPtHokens(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 来院情報
    /// </summary>
    List<CoRaiinInfModel> GetRaiinInfs(
        int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 診療情報(オーダー)
    /// </summary>
    List<CoOdrInfModel> GetOdrInfs(int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 診療情報(算定)
    /// </summary>
    List<CoSinKouiModel> GetSinKouis(int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// カルテ情報
    /// </summary>
    List<CoKarteInfModel> GetKarteInfs(int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf
    );

    /// <summary>
    /// 検査情報
    /// </summary>
    List<CoKensaModel> GetKensaInfs(int hpId,
        CoSta9000PtConf ptConf, CoSta9000HokenConf hokenConf, CoSta9000ByomeiConf byomeiConf,
        CoSta9000RaiinConf raiinConf, CoSta9000SinConf sinConf, CoSta9000KarteConf karteConf, CoSta9000KensaConf kensaConf
    );

    /// <summary>
    /// 医療機関情報
    /// </summary>
    CoHpInfModel GetHpInf(int hpId, int sinDate);

    Dictionary<int, string> GetUserSNameByUserIdDictionary(int hpId, List<int> userIdList);
}
