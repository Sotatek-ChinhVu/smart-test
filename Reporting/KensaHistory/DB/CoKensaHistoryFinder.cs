﻿using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.KensaHistory.Models;
using System.Linq.Dynamic.Core;
using static Domain.Models.KensaIrai.ListKensaInfDetailModel;

namespace Reporting.KensaHistory.DB
{
    public class CoKensaHistoryFinder : RepositoryBase, ICoKensaHistoryFinder
    {
        public CoKensaHistoryFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public HpInfModel GetHpInf(int hpId)
        {
            var hpInf = NoTrackingDataContext.HpInfs.FirstOrDefault(item => item.HpId == hpId);
            return hpInf != null ? new HpInfModel(hpId,
                                                    hpInf.StartDate,
                                                    hpInf.HpCd ?? string.Empty,
                                                    hpInf.RousaiHpCd ?? string.Empty,
                                                    hpInf.HpName ?? string.Empty,
                                                    hpInf.ReceHpName ?? string.Empty,
                                                    hpInf.KaisetuName ?? string.Empty,
                                                    hpInf.PostCd ?? string.Empty,
                                                    hpInf.PrefNo,
                                                    hpInf.Address1 ?? string.Empty,
                                                    hpInf.Address2 ?? string.Empty,
                                                    hpInf.Tel ?? string.Empty,
                                                    hpInf.FaxNo ?? string.Empty,
                                                    hpInf.OtherContacts ?? string.Empty
                                                ) : new HpInfModel();
        }

        public PtInf GetPtInf(int hpId, long ptId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
            return ptInf;
        }

        private static (string, string) GetValueLowHigSdt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (string.Empty, string.Empty);
            }
            else
            {
                string[] values = input.Split("-");

                if (values.Length == 2)
                {
                    return (values[0], values[1]);
                }
                else
                {
                    return (string.Empty, string.Empty);
                }
            }
        }

        public (List<CoKensaResultMultiModel>, List<long>) GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn)
        {
            IQueryable<KensaInfDetail> kensaInfDetails;

            var userConf = NoTrackingDataContext.UserConfs.Where(x => x.UserId == userId && x.HpId == hpId && x.GrpCd == 1002);

            bool SortIraiDateAsc = true;

            if (userConf.Where(x => x.GrpItemCd == 0).FirstOrDefault()?.Val == 1)
            {
                SortIraiDateAsc = false;
            }

            if (setId == 0)
            {
                kensaInfDetails = NoTrackingDataContext.KensaInfDetails
               .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None);
            }
            else
            {
                // Fllter data with KensaSet
                kensaInfDetails = (from t1 in NoTrackingDataContext.KensaInfDetails
                                   join t2 in NoTrackingDataContext.KensaSetDetails on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId
                                   select new
                                   {
                                       Result = t1,
                                       SortNo = t2.SortNo,
                                   }
                            ).OrderBy(x => x.SortNo).Select(x => x.Result);
            }

            IEnumerable< ListKensaInfDetailItemModel> data = (from t1 in kensaInfDetails
                        join t2 in NoTrackingDataContext.KensaMsts
                         on new { t1.KensaItemCd, t1.HpId } equals new { t2.KensaItemCd, t2.HpId }
                        join t3 in NoTrackingDataContext.KensaInfs on new { t1.HpId, t1.PtId, t1.IraiCd } equals new { t3.HpId, t3.PtId, t3.IraiCd }
                        join t4 in NoTrackingDataContext.PtInfs on new { t1.PtId, t1.HpId } equals new { t4.PtId, t4.HpId }
                        join t5 in NoTrackingDataContext.KensaCmtMsts
                             on t1.CmtCd1 equals t5.CMT into leftJoinT5
                        from t5 in leftJoinT5.DefaultIfEmpty()
                        join t6 in NoTrackingDataContext.KensaCmtMsts
                             on t1.CmtCd2 equals t6.CMT into leftJoinT6
                        from t6 in leftJoinT6.DefaultIfEmpty()
                        select new ListKensaInfDetailItemModel
                        (
                            t1.PtId,
                            t1.IraiCd,
                            t1.RaiinNo,
                            t1.IraiDate,
                            t1.SeqNo,
                            t1.SeqParentNo,
                            t2.KensaName ?? string.Empty,
                            t2.KensaKana ?? string.Empty,
                            t2.SortNo,
                            t1.KensaItemCd ?? string.Empty,
                            t1.ResultVal ?? string.Empty,
                            t1.ResultType ?? string.Empty,
                            t1.AbnormalKbn ?? string.Empty,
                            t1.CmtCd1 ?? string.Empty,
                            t1.CmtCd2 ?? string.Empty,
                            (!string.IsNullOrEmpty(t3.CenterCd) && t3.CenterCd.Equals(t5.CenterCd)) ? "不明" : t5.CMT ?? string.Empty,
                            (!string.IsNullOrEmpty(t3.CenterCd) && t3.CenterCd.Equals(t6.CenterCd)) ? "不明" : t6.CMT ?? string.Empty,
                            t4.Sex == 1 ? t2.MaleStd ?? string.Empty : t2.FemaleStd ?? string.Empty,
                            t4.Sex == 1 ? GetValueLowHigSdt(t2.MaleStd).Item1 : GetValueLowHigSdt(t2.FemaleStd).Item1,
                            t4.Sex == 1 ? GetValueLowHigSdt(t2.MaleStd).Item2 : GetValueLowHigSdt(t2.FemaleStd).Item2,
                            t2.MaleStd ?? string.Empty,
                            t2.FemaleStd ?? string.Empty,
                            t2.Unit ?? string.Empty,
                            t3.Nyubi ?? string.Empty,
                            t3.Yoketu ?? string.Empty,
                            t3.Bilirubin ?? string.Empty,
                            t3.SikyuKbn,
                            t3.TosekiKbn,
                            t3.InoutKbn,
                            t3.Status,
                            DeleteTypes.None
                        ));

            if (showAbnormalKbn)
            {
                data = data.Where(x => x.AbnormalKbn.Equals(AbnormalKbnType.High) || x.AbnormalKbn.Equals(AbnormalKbnType.Low));
            }

            // Sort data by user setting
            if (setId == 0)
            {

                var confSort = userConf.Where(x => x.GrpItemCd == 1).FirstOrDefault();
                var sortType = confSort?.Val;
                var sortCoulum = confSort?.GrpItemEdaNo;

                switch (sortCoulum)
                {
                    case SortKensaMstColumn.KensaItemCd:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.KensaItemCd);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaItemCd);
                        }
                        break;
                    case SortKensaMstColumn.KensaKana:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.KensaKana);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaKana);
                        }
                        break;
                    default:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.SortNo);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaItemCd);
                        }
                        break;

                }
            }

            // Get list with start date and end date
            if (endDate == 0)
            {
                data = data.Where(x => x.IraiDate <= startDate);
            }
            else
            {
                data = data.Where(x => x.IraiDate >= startDate && x.IraiDate <= endDate);
            }

            var kensaItemCds = data.GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.Std }).Select(x => new { x.Key.KensaItemCd, x.Key.KensaName, x.Key.Unit, x.Key.Std });

            // Get list iraiCd
            IOrderedEnumerable<object> kensaInfDetailColOrder = data.OrderBy(x => x.IraiDate);
            var kensaInfDetailCol = SortIraiDateAsc ? data
                                .OrderBy(x => x.IraiDate)
                                .GroupBy(x => new { x.IraiCd, x.IraiDate, x.Nyubi, x.Yoketu, x.Bilirubin, x.SikyuKbn, x.TosekiKbn })
                                .Select((group, index) => new KensaInfDetailColModel(group.Key.IraiCd, group.Key.IraiDate, group.Key.Nyubi, group.Key.Yoketu, group.Key.TosekiKbn, group.Key.SikyuKbn, group.Key.TosekiKbn, index))

                            : data.OrderByDescending(x => x.IraiDate)
                                .GroupBy(x => new { x.IraiCd, x.IraiDate, x.Nyubi, x.Yoketu, x.Bilirubin, x.SikyuKbn, x.TosekiKbn })
                                 .Select((group, index) => new KensaInfDetailColModel(group.Key.IraiCd, group.Key.IraiDate, group.Key.Nyubi, group.Key.Yoketu, group.Key.TosekiKbn, group.Key.SikyuKbn, group.Key.TosekiKbn, index));

            var kensaInfDetailData = new List<KensaInfDetailDataModel>();

            foreach (var kensaMstItem in kensaItemCds)
            {
                var dynamicArray = new List<ListKensaInfDetailItemModel>();

                foreach (var item in kensaInfDetailCol)
                {
                    var dynamicDataItem = data.Where(x => x.IraiCd == item.IraiCd && x.KensaItemCd == kensaMstItem.KensaItemCd).FirstOrDefault();

                    if (dynamicDataItem == null)
                    {
                        dynamicArray.Add(new ListKensaInfDetailItemModel(
                            ptId,
                            item.IraiCd
                        ));
                    }
                    else
                    {
                        dynamicArray.Add(dynamicDataItem);
                    }
                }

                var rowData = new KensaInfDetailDataModel(
                     kensaMstItem.KensaItemCd,
                     kensaMstItem.KensaName,
                     kensaMstItem.Unit,
                     kensaMstItem.Std,
                     dynamicArray
                );

                kensaInfDetailData.Add(rowData);
            }

            List<string> itemName = new();
            foreach (var item in kensaInfDetailData)
            {
                itemName.Add(item.KensaName);
            }

            itemName.Distinct();

            List<CoKensaResultMultiModel> result = new();
            List<KensaResultMultiItem> kensaResultMultiItems = new();

            foreach (var item in kensaInfDetailData)
            {
                result.Add(new CoKensaResultMultiModel(item.KensaName, item.Unit, item.Std, new(), new()));
            }

            foreach (var item in kensaInfDetailData)
            {
                foreach (var kensaResultMultiItem in item.DynamicArray)
                {
                    kensaResultMultiItems.Add(new KensaResultMultiItem(kensaResultMultiItem.ResultVal, kensaResultMultiItem.AbnormalKbn));
                }
            }

            int count = kensaResultMultiItems.Count / itemName.Count;
            int j = 0;

            for (int i = 0; i < result.Count; i++)
            {
                for (int k = 0; k < count; k++)
                {
                    result[i].KensaResultMultiItems.Add(kensaResultMultiItems[j++]);
                }
            }

            List<long> date = new();

            foreach (var item in kensaInfDetailData)
            {
                foreach (var kensaResultMultiItem in item.DynamicArray)
                {
                    if(startDate == 0 && kensaResultMultiItem.IraiDate <= endDate)
                    {
                        date.Add(kensaResultMultiItem.IraiDate);
                    }
                    else if (endDate == 0 && kensaResultMultiItem.IraiDate >= startDate)
                    {
                        date.Add(kensaResultMultiItem.IraiDate);
                    }
                    else if(kensaResultMultiItem.IraiDate >= startDate && kensaResultMultiItem.IraiDate <= endDate)
                    {
                        date.Add(kensaResultMultiItem.IraiDate);
                    }
                }
            }

            date = date.Distinct().ToList();
            date.Remove(0);
            result.Add( new CoKensaResultMultiModel("","","", new(), date));
            /*List<ListCoKensaResultMultiModel> coKensaResultMultiModels = new();
            foreach (var item in result)
            {
                coKensaResultMultiModels.
            }*/

            return (result, date);
        }

        public ListKensaInfDetailModel GetListKensaInf(int hpId, int userId, long ptId, int setId, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int startDate)
        {
            IQueryable<KensaInfDetail> kensaInfDetails;

            var userConf = NoTrackingDataContext.UserConfs.Where(x => x.UserId == userId && x.HpId == hpId && x.GrpCd == 1002);

            bool SortIraiDateAsc = true;

            if (userConf.Where(x => x.GrpItemCd == 0).FirstOrDefault()?.Val == 1)
            {
                SortIraiDateAsc = false;
            }

            if (setId == 0)
            {
                kensaInfDetails = NoTrackingDataContext.KensaInfDetails
               .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteTypes.None);
            }
            else
            {
                // Fllter data with KensaSet
                kensaInfDetails = (from t1 in NoTrackingDataContext.KensaInfDetails
                                   join t2 in NoTrackingDataContext.KensaSetDetails on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId
                                   select new
                                   {
                                       Result = t1,
                                       SortNo = t2.SortNo,
                                   }
                            ).OrderBy(x => x.SortNo).Select(x => x.Result);
            }

            IEnumerable<ListKensaInfDetailItemModel> data = (from t1 in kensaInfDetails
                        join t2 in NoTrackingDataContext.KensaMsts
                         on new { t1.KensaItemCd, t1.HpId } equals new { t2.KensaItemCd, t2.HpId }
                        join t3 in NoTrackingDataContext.KensaInfs on new { t1.HpId, t1.PtId, t1.IraiCd } equals new { t3.HpId, t3.PtId, t3.IraiCd }
                        join t4 in NoTrackingDataContext.PtInfs on new { t1.PtId, t1.HpId } equals new { t4.PtId, t4.HpId }
                        join t5 in NoTrackingDataContext.KensaCmtMsts
                             on t1.CmtCd1 equals t5.CmtCd into leftJoinT5
                        from t5 in leftJoinT5.DefaultIfEmpty()
                        join t6 in NoTrackingDataContext.KensaCmtMsts
                             on t1.CmtCd2 equals t6.CmtCd into leftJoinT6
                        from t6 in leftJoinT6.DefaultIfEmpty()
                        join t7 in NoTrackingDataContext.KensaStdMsts
                            on t1.KensaItemCd equals t7.KensaItemCd into leftJoinT7
                        from t7 in leftJoinT7.DefaultIfEmpty()
                        where t2.KensaItemSeqNo == NoTrackingDataContext.KensaMsts.Where(m => m.HpId == t2.HpId && m.KensaItemCd == t2.KensaItemCd).Min(m => m.KensaItemSeqNo)
                        && t3.IsDeleted == DeleteTypes.None && t1.IsDeleted == DeleteTypes.None
                        select new ListKensaInfDetailItemModel
                        (
                            t1.PtId,
                            t1.IraiCd,
                            t1.RaiinNo,
                            t1.IraiDate,
                            t1.SeqNo,
                            t1.SeqParentNo,
                            t2.KensaName ?? string.Empty,
                            t2.KensaKana ?? string.Empty,
                            t2.SortNo,
                            t1.KensaItemCd ?? string.Empty,
                            t1.ResultVal ?? string.Empty,
                            t1.ResultType ?? string.Empty,
                            t1.AbnormalKbn ?? string.Empty,
                            t1.CmtCd1 ?? string.Empty,
                            t1.CmtCd2 ?? string.Empty,
                            (!string.IsNullOrEmpty(t3.CenterCd) && t3.CenterCd.Equals(t5.CenterCd)) ? "不明" : t5.CMT ?? string.Empty,
                            (!string.IsNullOrEmpty(t3.CenterCd) && t3.CenterCd.Equals(t6.CenterCd)) ? "不明" : t6.CMT ?? string.Empty,
                            t4.Sex == 1 ? t7.MaleStd ?? string.Empty : t7.FemaleStd ?? string.Empty,
                            t4.Sex == 1 ? GetValueLowHigSdt(t7.MaleStd).Item1 : GetValueLowHigSdt(t7.FemaleStd).Item1,
                            t4.Sex == 1 ? GetValueLowHigSdt(t7.MaleStd).Item2 : GetValueLowHigSdt(t7.FemaleStd).Item2,
                            t7.MaleStd ?? string.Empty,
                            t7.FemaleStd ?? string.Empty,
                            t2.Unit ?? string.Empty,
                            t3.Nyubi ?? string.Empty,
                            t3.Yoketu ?? string.Empty,
                            t3.Bilirubin ?? string.Empty,
                            t3.SikyuKbn,
                            t3.TosekiKbn,
                            t3.InoutKbn,
                            t3.Status,
                            DeleteTypes.None
                        ));

            if (showAbnormalKbn)
            {
                data = data.Where(x => x.AbnormalKbn.Equals(AbnormalKbnType.High) || x.AbnormalKbn.Equals(AbnormalKbnType.Low));
            }

            // Sort data by user setting
            if (setId == 0)
            {
                var confSort = userConf.Where(x => x.GrpItemCd == 1).FirstOrDefault();
                var sortType = confSort?.Val;
                var sortCoulum = confSort?.GrpItemEdaNo;

                switch (sortCoulum)
                {
                    case SortKensaMstColumn.KensaItemCd:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.KensaItemCd);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaItemCd);
                        }
                        break;
                    case SortKensaMstColumn.KensaKana:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.KensaKana);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaKana);
                        }
                        break;
                    default:
                        if (sortType == 1)
                        {
                            data = data.OrderByDescending(x => x.SortNo);
                        }
                        else
                        {
                            data = data.OrderBy(x => x.KensaItemCd);
                        }
                        break;
                }
            }

            // Sort by IraiDate

            var sortedData = SortIraiDateAsc
                ? data.OrderBy(x => x.IraiDate)
                : data.OrderByDescending(x => x.IraiDate);



            var kensaInfDetailCol = sortedData
                .GroupBy(x => new { x.IraiCd, x.IraiDate, x.Nyubi, x.Yoketu, x.Bilirubin, x.SikyuKbn, x.TosekiKbn })
                .Select((group, index) => new KensaInfDetailColModel(
                    group.Key.IraiCd,
                    group.Key.IraiDate,
                    group.Key.Nyubi,
                    group.Key.Yoketu,
                    group.Key.TosekiKbn,
                    group.Key.SikyuKbn,
                    group.Key.TosekiKbn,
                    index
                ));


            var totalCol = kensaInfDetailCol.Count();

            // Get list with start date
            if (startDate > 0)
            {
                kensaInfDetailCol = kensaInfDetailCol.Where(x => x.IraiDate >= startDate);
            }
            else
            {
                // Get list with iraiCdStart
                if (iraiCdStart > 0)
                {

                    int currentIndex = 0;
                    foreach (var obj in kensaInfDetailCol)
                    {
                        if (obj.IraiCd == iraiCdStart)
                        {
                            break;
                        }
                        currentIndex++;
                    }

                    if (getGetPrevious)
                    {
                        kensaInfDetailCol = kensaInfDetailCol.TakeWhile(x => x.IraiCd != iraiCdStart);
                    }
                    else
                    {
                        kensaInfDetailCol = kensaInfDetailCol.Skip(currentIndex + 1);
                    }
                }
            }


            var kensaIraiCdSet = new HashSet<long>(kensaInfDetailCol.Select(item => item.IraiCd));
            data = data.Where(x => kensaIraiCdSet.Contains(x.IraiCd));
            var kensaItemCds = data.Where(x => kensaIraiCdSet.Contains(x.IraiCd)).GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.Std }).Select(x => new { x.Key.KensaItemCd, x.Key.KensaName, x.Key.Unit, x.Key.Std });

            var groupRowData = data
                .GroupBy(x => x.KensaItemCd)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());

            var kensaInfDetailData = new List<KensaInfDetailDataModel>();

            foreach (var kensaMstItem in kensaItemCds)
            {
                if (groupRowData.TryGetValue(kensaMstItem.KensaItemCd, out var dynamicArray))
                {
                    kensaInfDetailData.Add(new KensaInfDetailDataModel(
                        kensaMstItem.KensaItemCd,
                        kensaMstItem.KensaName,
                        kensaMstItem.Unit,
                        kensaMstItem.Std,
                        dynamicArray
                    ));
                }
            }

            var result = new ListKensaInfDetailModel(kensaInfDetailCol.ToList(), kensaInfDetailData, totalCol);
            return result;
        }
    }
}