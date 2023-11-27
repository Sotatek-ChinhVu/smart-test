using Domain.Common;
using Reporting.OrderLabel.Model;

namespace Reporting.OrderLabel.DB
{
    public interface ICoOrderLabelFinder : IRepositoryBase
    {
        /// <summary>
        /// 患者情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>患者情報</returns>
        CoPtInfModel FindPtInf(int hpId, long ptId);

        /// <summary>
        /// 来院情報取得に診療科マスタとユーザーマスタを結合したデータを取得
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns>
        /// 指定の患者の指定の診療日の来院情報
        /// SIN_START_TIME順にソート
        /// </returns>
        CoRaiinInfModel FindRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo);
        /// <summary>
        /// オーダー情報取得
        /// </summary>
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
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns);

        /// <summary>
        /// 予約オーダー情報取得
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="rsvDate">予約日</param>
        /// <returns></returns>
        List<CoRsvkrtOdrInfModel> FindRsvKrtOdrInf(int hpId, long ptId, int rsvDate, long rsvkrtNo, List<(int from, int to)> odrKouiKbns);
        /// <summary>
        /// 予約オーダー詳細情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="rsvDate">予約日</param>
        /// <param name="rsvkrtNo">予約カルテ番号</param>
        /// <param name="odrKouiKbns"></param>
        /// <returns></returns>
        List<CoRsvkrtOdrInfDetailModel> FindRsvKrtOdrInfDetail(int hpId, long ptId, int rsvDate, long rsvkrtNo, List<(int from, int to)> odrKouiKbns);
        /// <summary>
        /// 予約情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日（この日以降の予約を取得）</param>
        /// <returns></returns>
        List<CoYoyakuModel> FindYoyaku(int hpId, long ptId, int sinDate);

        List<CoUserMstModel> FindUserMst(int hpId);
    }
}
