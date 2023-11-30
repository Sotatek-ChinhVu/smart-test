using Domain.Common;
using Reporting.Sijisen.Model;

namespace Reporting.Sijisen.DB;

public interface ICoSijisenFinder : IRepositoryBase
{
    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>患者情報</returns>
    CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);

    /// <summary>
    /// 来院情報取得に診療科マスタとユーザーマスタを結合したデータを取得
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>
    /// 指定の患者の指定の診療日の来院情報
    /// SIN_START_TIME順にソート
    /// </returns>
    CoRaiinInfModel FindRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo);
    /// <summary>
    /// 指定診療日の中で、指定来院番号以外の来院の情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>
    /// 指定の患者の指定の診療日の来院情報
    /// SIN_START_TIME順にソート
    /// </returns>
    List<CoRaiinInfModel> FindOtherRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo);

    /// <summary>
    /// オーダー情報取得
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>
    /// 指定の患者の指定の診療日のオーダー情報
    /// 削除分は除く
    /// </returns>
    List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns);

    /// <summary>
    /// オーダー詳細情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID </param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns></returns>
    List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns);
    /// <summary>
    /// 予約オーダー情報取得
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="rsvDate">予約日</param>
    /// <returns></returns>
    List<CoRsvkrtOdrInfModel> FindRsvKrtOdrInf(int hpId, long ptId, int rsvDate, List<(int from, int to)> odrKouiKbns);
    /// <summary>
    /// 予約オーダー詳細情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <param name="rsvDate">予約日</param>
    /// <returns></returns>
    List<CoRsvkrtOdrInfDetailModel> FindRsvKrtOdrInfDetail(int hpId, long ptId, int rsvDate, List<(int from, int to)> odrKouiKbns);

    /// <summary>
    /// 来院区分情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID </param>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns></returns>
    List<CoRaiinKbnInfModel> FindRaiinKbnInf(int hpId, long ptId, int sinDate, long raiinNo);

    List<CoRaiinKbnMstModel> FindRaiinKbnMst(int hpId);

    /// <summary>
    /// 最終来院日を取得する（状態が計算以上の来院の中で最大の日）
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <returns></returns>
    int GetLastSinDate(int hpId, long ptId);
}
