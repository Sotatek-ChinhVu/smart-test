using Reporting.Accounting.Model;

namespace Reporting.Accounting.DB;

public interface ICoAccountingFinder
{
    /// <summary>
    /// 医療機関情報取得
    /// </summary>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    CoHpInfModel FindHpInf(int hpId, int sinDate);

    List<(long, int)> FindPtInf(int hpId, List<(int grpId, string grpCd)> grpConditions);

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId"></param>
    /// <returns></returns>
    CoPtInfModel FindPtInf(int hpId, long ptId);

    /// <summary>
    /// 会計情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">開始診療日</param>
    /// <param name="endDate">終了診療日</param>
    /// <param name="raiinNos">来院番号のリスト</param>
    /// <param name="hokenId">保険ID</param>
    /// <param name="miseisanKbn">未精算区分</param>
    /// <param name="saiKbn">差異区分</param>
    /// <param name="misyuKbn">未収区分</param>
    /// <param name="seikyuKbn">請求区分</param>
    /// <param name="hokenKbn">保険区分</param>
    /// <param name="hokenSeikyu">保険請求</param>
    /// <param name="jihiSeikyu">自費請求</param>
    /// <returns></returns>
    List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId
        , int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, bool hokenSeikyu, bool jihiSeikyu, ref List<CoWarningMessage> warningMessages);
    List<CoKaikeiInfModel> FindKaikeiInfNyukinBase(int hpId, long ptId, int startDate, int endDate, int hokenId
        , int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, bool hokenSeikyu, bool jihiSeikyu, ref List<CoWarningMessage> warningMessages);
    /// <summary>
    /// 会計書情報を取得する（リスト用）
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="ptConditions"></param>
    /// <param name="grpConditions"></param>
    /// <param name="sort">
    ///     0: 患者番号・患者カナ氏名順
    ///     1: 患者カナ氏名・患者番号順
    /// </param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <returns></returns>
    List<CoKaikeiInfListModel> FindKaikeiInfList(
        int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
        int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, ref List<CoWarningMessage> warningMessages);

    /// <summary>
    /// 所見情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">開始日</param>
    /// <param name="endDate">終了日</param>
    /// <param name="raiinNos">来院番号</param>
    List<CoKarteInfModel> FindKarteInf(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos);

    /// <summary>
    /// オーダー情報を取得する（投薬のみ）
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="raiinNos"></param>
    /// <returns></returns>
    List<CoOdrInfModel> FindOdrInfData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos);
    /// <summary>
    /// オーダー詳細情報を取得する（投薬のみ）
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="raiinNos"></param>
    /// <returns></returns>
    List<CoOdrInfDetailModel> FindOdrInfDetailData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos);

    /// <summary>
    /// 患者病名情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">取得期間開始日</param>
    /// <param name="endDate">取得期間終了日</param>
    /// <returns>患者病名情報モデル</returns>
    List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate);



    /// <summary>
    /// 自費種別マスタを取得する
    /// </summary>
    /// <returns></returns>
    List<CoJihiSbtMstModel> FindJihiSbtMst(int hpId);
    /// <summary>
    /// 患者グループ名称マスタを取得する
    /// </summary>
    /// <returns></returns>
    List<CoPtGrpNameMstModel> FindPtGrpNameMst(int hpId);
    /// <summary>
    /// 患者グループ項目マスタを取得する
    /// </summary>
    /// <returns></returns>
    List<CoPtGrpItemModel> FindPtGrpItemMst(int hpId);
    /// <summary>
    /// 患者メモを取得する
    /// </summary>
    /// <param name="ptId">患者ID </param>
    /// <returns></returns>
    CoPtMemoModel FindPtMemo(int hpId, long ptId);
    /// <summary>
    /// 予約来院情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日、この日以降の予約来院情報を取得する</param>
    /// <returns></returns>
    List<CoRaiinInfModel> FindYoyakuRaiinInf(int hpId, long ptId, int sinDate);

    List<CoSystemGenerationConfModel> FindSystemGenerationConf(int hpId, int GrpCd);

    List<long> GetPtNums(int hpId, List<long> ptIds);

    long GetPtNum(int hpId, long ptId);

    List<RaiinInfModel> GetOyaRaiinInfList(int hpId, List<long> raiinNoList, long ptId);

    List<CoAccountDueListModel> GetAccountDueList(int hpId, long ptId);

    List<PtGrpNameMstModel> GetPtGrpNameMstModels(int hpId);

    void ReleaseResource();
}
