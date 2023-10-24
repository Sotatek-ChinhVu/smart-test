using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3020.Models;

namespace Reporting.Statistics.Sta3020.DB
{
    public class CoSta3020Finder : RepositoryBase, ICoSta3020Finder
    {
        private readonly ICoHpInfFinder _hpInfFinder;

        public CoSta3020Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
        }
        public CoHpInfModel GetHpInf(int hpId, int sinDate)
        {
            return _hpInfFinder.GetHpInf(hpId, sinDate);
        }

        public List<CoListSetModel> GetListSet(int hpId, CoSta3020PrintConf printConf)
        {
            List<CoListSetModel> retData = new List<CoListSetModel>();

            #region リストセット
            //条件１-セット区分
            List<int> setKbns = new List<int> { };
            if (printConf.SetKbnKanri) setKbns.Add(13);
            if (printConf.SetKbnZaitaku) setKbns.Add(14);
            if (printConf.SetKbnSyoho) setKbns.Add(20);
            if (printConf.SetKbnYoho) setKbns.Add(21);
            if (printConf.SetKbnChusya) setKbns.Add(30);
            if (printConf.SetKbnChusyaSyugi) setKbns.Add(31);
            if (printConf.SetKbnSyochi) setKbns.Add(40);
            if (printConf.SetKbnSyujutsu) setKbns.Add(50);
            if (printConf.SetKbnKensa) setKbns.Add(60);
            if (printConf.SetKbnGazo) setKbns.Add(70);
            if (printConf.SetKbnSonota) setKbns.Add(80);
            if (printConf.SetKbnJihi) setKbns.Add(95);

            if (setKbns.Count > 0 && !(printConf.TgtData == 2 && printConf.ItemSearchOpt == 1 && (printConf.ItemCds?.Count ?? 0) > 0))
            {
                //セットの世代を取得
                int generationId = GetListSetGenerationId(hpId, printConf.StdDate);

                var listSetMsts = NoTrackingDataContext.ListSetMsts.Where(x => x.HpId == hpId && x.GenerationId == generationId && x.IsDeleted == 0);
                listSetMsts = listSetMsts.Where(x => setKbns.Contains(x.SetKbn));

                var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);

                //項目の有効期限
                var maxTenMsts = tenMsts
                    .GroupBy(x => new { x.HpId, x.ItemCd })
                    .Select(x => new
                    {
                        x.Key.HpId,
                        x.Key.ItemCd,
                        MaxStartDate = x.Max(d => d.StartDate),
                        MaxEndDate = x.Max(d => d.EndDate)
                    });

                //基準日の世代
                var stdDateTenMsts = tenMsts.Where(x => x.StartDate <= printConf.StdDate && printConf.StdDate <= x.EndDate)
                    .Select(x => new { x.HpId, x.ItemCd, x.StartDate, x.EndDate, x.OdrUnitName, x.CnvUnitName, x.KensaItemCd, x.KensaItemSeqNo });

                //最新の世代
                var latestTenMsts = (
                    from tenMst in tenMsts
                    join maxTenMst in maxTenMsts on
                        new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                        new { maxTenMst.HpId, maxTenMst.ItemCd, StartDate = maxTenMst.MaxStartDate }
                    select new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate, tenMst.EndDate, tenMst.OdrUnitName, tenMst.CnvUnitName, tenMst.KensaItemCd, tenMst.KensaItemSeqNo }
                    );

                //最新の世代から基準日が期限内である項目を除く
                latestTenMsts = latestTenMsts
                    .Where(x => !(x.StartDate <= printConf.StdDate && printConf.StdDate <= x.EndDate));

                //基準日の世代(基準日に有効な項目)　＋　最新の世代(基準日に有効じゃない項目)
                var unionTenMsts = stdDateTenMsts.Union(latestTenMsts);

                var kensaMsts = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId);

                var joinDetails = (
                from listSetMst in listSetMsts
                join unionTenMst in unionTenMsts on
                    new { listSetMst.HpId, listSetMst.ItemCd } equals
                    new { unionTenMst.HpId, unionTenMst.ItemCd } into unionTenMstJoins
                from unionTenMstJoin in unionTenMstJoins.DefaultIfEmpty()
                join maxTenMst in maxTenMsts on
                    new { unionTenMstJoin.HpId, unionTenMstJoin.ItemCd } equals
                    new { maxTenMst.HpId, maxTenMst.ItemCd } into maxTenMstJoins
                from maxTenMstJoin in maxTenMstJoins.DefaultIfEmpty()
                join kensaMst in kensaMsts on
                    new { unionTenMstJoin.HpId, unionTenMstJoin.KensaItemCd, unionTenMstJoin.KensaItemSeqNo } equals
                    new { kensaMst.HpId, kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo } into kensaMstJoins
                from kensaMstJoin in kensaMstJoins.DefaultIfEmpty()
                select
                    new
                    {
                        listSetMst.SetKbn,
                        listSetMst.Level1,
                        listSetMst.Level2,
                        listSetMst.Level3,
                        listSetMst.Level4,
                        listSetMst.Level5,
                        listSetMst.SetId,
                        listSetMst.ItemCd,
                        listSetMst.SetName,
                        listSetMst.IsTitle,
                        listSetMst.SelectType,
                        listSetMst.Suryo,
                        listSetMst.UnitSbt,
                        OdrUnitName = unionTenMstJoin.OdrUnitName == null ? string.Empty : unionTenMstJoin.OdrUnitName,
                        CnvUnitName = unionTenMstJoin.CnvUnitName == null ? string.Empty : unionTenMstJoin.CnvUnitName,
                        KensaItemCd = kensaMstJoin.KensaItemCd == null ? string.Empty : kensaMstJoin.KensaItemCd,
                        CenterItemCd1 = kensaMstJoin.CenterItemCd1 == null ? string.Empty : kensaMstJoin.CenterItemCd1,
                        CenterItemCd2 = kensaMstJoin.CenterItemCd2 == null ? string.Empty : kensaMstJoin.CenterItemCd2,
                        MaxEndDate = maxTenMstJoin.MaxEndDate == null ? 0 : maxTenMstJoin.MaxEndDate
                    }
                ).AsEnumerable();

                //条件２-対象データ
                switch (printConf.TgtData)
                {
                    case 1:
                        {
                            //期限切れ
                            joinDetails = joinDetails.Where(x => (x.MaxEndDate == 0 ? 99999999 : x.MaxEndDate) < printConf.StdDate);
                            break;
                        }
                    case 2:
                        {
                            //項目選択
                            //検索ワード
                            if ((printConf.ListSearchWord?.Count ?? 0) > 0)
                            {
                                if (printConf.SearchOpt == 1)
                                {
                                    //and検索
                                    joinDetails = joinDetails.Where(x => printConf.ListSearchWord!.All(key => x.SetName.Contains(key)));
                                }
                                else
                                {
                                    //or検索
                                    joinDetails = joinDetails.Where(x => printConf.ListSearchWord!.Any(key => x.SetName.Contains(key)));
                                }
                            }

                            //検索項目
                            if (printConf.ItemSearchOpt == 0 && ((printConf.ItemCds?.Count ?? 0) > 0))
                            {
                                joinDetails = joinDetails.Where(x => x.ItemCd != null && printConf.ItemCds!.Any(key => x.ItemCd.Contains(key)));
                            }
                            break;
                        }
                }

                retData = joinDetails.AsEnumerable().Select(
                data => new CoListSetModel()
                {
                    SetKbn = data.SetKbn,
                    Level1 = data.Level1,
                    Level2 = data.Level2,
                    Level3 = data.Level3,
                    Level4 = data.Level4,
                    Level5 = data.Level5,
                    SetCd = data.SetId,
                    ItemCd = data.ItemCd,
                    SetName = data.SetName,
                    IsTitle = data.IsTitle,
                    SelectType = data.SelectType,
                    Suryo = data.Suryo,
                    UnitName = data.UnitSbt == 2 ? data.CnvUnitName : data.OdrUnitName,
                    KensaItemCd = data.KensaItemCd,
                    CenterItemCd1 = data.CenterItemCd1,
                    CenterItemCd2 = data.CenterItemCd2,
                    MaxEndDate = data.MaxEndDate
                }).ToList();
            }
            #endregion

            #region 病名セット
            if (printConf.SetKbnByomei && !(printConf.TgtData == 2 && printConf.ItemSearchOpt == 0 && (printConf.ItemCds?.Count ?? 0) > 0))
            {
                //セットの世代を取得
                int generationId = GetByomeiSetGenerationId(hpId, printConf.StdDate);
                var byomeiSetMsts = NoTrackingDataContext.ByomeiSetMsts.Where(x => x.HpId == hpId && x.GenerationId == generationId && x.IsDeleted == 0);
                var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(x => x.HpId == hpId);

                var joinDetails = (
                from byomeiSetMst in byomeiSetMsts
                join byomeiMst in byomeiMsts on
                    new { byomeiSetMst.HpId, byomeiSetMst.ByomeiCd } equals
                    new { byomeiMst.HpId, byomeiMst.ByomeiCd } into byomeiMstJoins
                from byomeiMstJoin in byomeiMstJoins.DefaultIfEmpty()
                select
                    new
                    {
                        byomeiSetMst.Level1,
                        byomeiSetMst.Level2,
                        byomeiSetMst.Level3,
                        byomeiSetMst.Level4,
                        byomeiSetMst.Level5,
                        byomeiSetMst.SeqNo,
                        byomeiSetMst.ByomeiCd,
                        byomeiSetMst.SetName,
                        byomeiSetMst.IsTitle,
                        byomeiSetMst.SelectType,
                        DelDate = byomeiMstJoin != null ? byomeiMstJoin.DelDate : 0
                    }
                ).AsEnumerable();


                //条件２-対象データ
                switch (printConf.TgtData)
                {
                    case 1:
                        {
                            //期限切れ
                            joinDetails = joinDetails.Where(x => (x.DelDate == 0 ? 99999999 : x.DelDate) < printConf.StdDate);
                            break;
                        }
                    case 2:
                        {
                            //項目選択
                            //検索ワード
                            if ((printConf.ListSearchWord?.Count ?? 0) > 0)
                            {
                                if (printConf.SearchOpt == 1)
                                {
                                    //and検索
                                    joinDetails = joinDetails.Where(x => printConf.ListSearchWord!.All(key => x.SetName.Contains(key)));
                                }
                                else
                                {
                                    //or検索
                                    joinDetails = joinDetails.Where(x => printConf.ListSearchWord!.Any(key => x.SetName.Contains(key)));
                                }
                            }

                            //検索項目
                            if (printConf.ItemSearchOpt == 1 && ((printConf.ItemCds?.Count ?? 0) > 0))
                            {
                                joinDetails = joinDetails.Where(x => printConf.ItemCds!.Any(key => x.ByomeiCd.Contains(key)));
                            }
                            break;
                        }
                }

                retData.AddRange(joinDetails.AsEnumerable().Select(data => new CoListSetModel()
                {
                    SetKbn = 0,
                    Level1 = data.Level1,
                    Level2 = data.Level2,
                    Level3 = data.Level3,
                    Level4 = data.Level4,
                    Level5 = data.Level5,
                    SetCd = data.SeqNo,
                    ItemCd = data.ByomeiCd,
                    SetName = data.SetName,
                    IsTitle = data.IsTitle,
                    SelectType = data.SelectType,
                    MaxEndDate = data.DelDate
                }
                ).ToList()
                );

            }
            #endregion

            return retData;
        }

        /// <summary>
        /// リストセットの世代取得
        /// </summary>
        /// <param name="stdDate">基準日</param>
        /// <returns></returns>
        private int GetListSetGenerationId(int hpId, int stdDate)
        {
            var generation = NoTrackingDataContext.ListSetGenerationMsts
                .Where(x => x.HpId == hpId && x.StartDate <= stdDate && x.IsDeleted == 0)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }

        /// <summary>
        /// 病名セットの世代取得
        /// </summary>
        /// <param name="stdDate">基準日</param>
        /// <returns></returns>
        private int GetByomeiSetGenerationId(int hpId, int stdDate)
        {
            var generation = NoTrackingDataContext.ByomeiSetGenerationMsts
                .Where(x => x.HpId == hpId && x.StartDate <= stdDate && x.IsDeleted == 0)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }
    }
}
