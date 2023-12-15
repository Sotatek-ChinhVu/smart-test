using Domain.Common;
using Emr.Report.OutDrug.Model;
using Reporting.OutDrug.Model;

namespace Reporting.OutDrug.DB;

public interface ICoOutDrugFinder : IRepositoryBase
{
    /// <summary>
    /// オーダー情報データを取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>
    /// 指定の患者の指定の診療日のオーダー情報
    /// 削除分は除く
    /// </returns>
    List<CoOdrInfModel> FindOdrInfData(int hpId, long ptId, int sinDate, long raiinNo);

    /// <summary>
    /// オーダー情報詳細データを取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns></returns>
    List<CoOdrInfDetailModel> FindOdrInfDetailData(int hpId, long ptId, int sinDate, long raiinNo);

    /// <summary>
    /// 患者公費情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="kohiIds">公費IDリスト</param>
    /// <returns>患者公費情報のリスト</returns>
    List<CoPtKohiModel> FindPtKohi(int hpId, long ptId, int sinDate, HashSet<int> kohiIds);

    /// <summary>
    /// 医療機関情報を取得する
    /// </summary>
    /// <param name="sinDate">診療日</param>
    /// <returns>医療機関情報</returns>
    CoHpInfModel FindHpInf(int hpId, int sinDate);

    /// <summary>
    /// 来院情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>来院情報</returns>
    CoRaiinInfModel FindRaiinInf(int hpId, long ptId, int sinDate, long raiinNo);

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>患者情報</returns>
    CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);

    /// <summary>
    /// 患者保険情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="hokenId">保険ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>患者保険情報</returns>
    CoPtHokenInfModel FindPtHoken(int hpId, long ptId, int hokenId, int sinDate);

    /// <summary>
    /// 指定の期間に指定の項目が算定されているかチェックする
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <param name="itemCds">検索する項目の診療行為コードのリスト</param>
    /// <returns>true-算定されている</returns>
    bool CheckSanteiTerm(int hpId, long ptId, int startDate, int endDate, List<string> itemCds);

    /// <summary>
    /// 指定の期間に指定の項目がオーダーされているかチェックする
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <param name="itemCds">検索する項目の診療行為コードのリスト</param>
    /// <returns></returns>
    bool CheckOdrTerm(int hpId, long ptId, int startDate, int endDate, List<string> itemCds);
    /// <summary>
    /// 指定の来院に指定の項目がオーダーされているかチェックする
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="raiinNo">来院番号</param>
    /// <param name="itemCds">検索する項目の診療行為コードのリスト</param>
    /// <returns></returns>
    bool CheckOdrRaiin(int hpId, long ptId, long raiinNo, List<string> itemCds);
    /// <summary>
    /// マル長を持っているかチェックする
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="sinDate"></param>
    /// <returns>0-持っていない、1-マル長10000円を持っている、2-マル長20000円を持っている</returns>
    int ExistMarucyo(int hpId, long ptId, int sinDate, int hokenId);

    /// <summary>
    /// 用法補足情報を取得する
    /// </summary>
    /// <param name="yohoCd">項目コード</param>
    /// <param name="sinDate">診療日</param>
    List<CoYohoHosoku> FindYohoHosoku(int hpId, string itemCd, int sinDate);

    /// <summary>
    /// 重複投薬等チェック情報を取得する
    /// </summary>
    List<CoEpsChk> FindEPSChecks(int hpId, long ptId, long raiinNo);

    /// <summary>
    /// 用法マスタ情報を取得する
    /// </summary>
    /// <param name="yohoCd">用法コード</param>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    CoYohoMstModel FindYohoMst(int hpId, string yohoCd, int sinDate);

    /// <summary>
    /// １回量剤形に登録された単位か
    /// </summary>
    /// <param name="unitName">単位名</param>
    /// <returns>true:１回量剤形である, false:１回量剤形ではない</returns>
    public bool IsSingleDosageUnit(int hpId, string unitName);

    /// <summary>
    /// 力価情報(M46_DOSAGE_DRUG)取得
    /// </summary>
    /// <param name="yjCode">YJコード</param>
    /// <returns></returns>
    public CoDosageDrugModel GetDosageDrugModel(string yjCode);

    /// <summary>
    /// 今月オンライン資格確認した保険
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>今月オンライン資格確認した保険ID</returns>
    public int GetOnlineConfirmedHokenId(int hpId, long ptId, int sinDate);

    /// <summary>
    /// 電子処方箋登録情報
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>電子処方箋登録情報のリスト</returns>
    public List<CoEpsPrescription> FindEpsPrescription(int hpId, long ptId, long raiinNo);

    /// <summary>
    /// 電子処方箋の処方内容控え取得
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>電子処方箋の処方内容控えのリスト</returns>
    public List<CoEpsReference> GetEpsReferences(int hpId, long ptId, long raiinNo);
}
