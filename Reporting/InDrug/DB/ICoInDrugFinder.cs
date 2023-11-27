using Domain.Common;
using Reporting.InDrug.Model;

namespace Reporting.InDrug.DB
{
    public interface ICoInDrugFinder : IRepositoryBase
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
        /// オーダー情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日のオーダー情報
        /// 削除分は除く
        /// </returns>
        List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo);

        /// <summary>
        /// オーダー詳細情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID </param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
