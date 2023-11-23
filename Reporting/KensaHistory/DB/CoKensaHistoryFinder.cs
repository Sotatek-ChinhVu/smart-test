using Domain.Models.HpInf;
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

        public HpInfModel GetHpInf(int hpId, int sinDate)
        {
            var hpInf = NoTrackingDataContext.HpInfs.Where(item => item.HpId == hpId && item.StartDate < sinDate).OrderBy(x => x.StartDate).First();
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

            var kensaSetDetailById = NoTrackingDataContext.KensaSetDetails.Where(x => x.SetId == setId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).GroupBy(item => item.KensaItemCd)
               .Select(group => new
               {
                   KensaItemCd = group.Key,
                   SortNo = group.Min(item => item.SortNo)
               });

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
                                   join t2 in kensaSetDetailById on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId && t1.IsDeleted == DeleteTypes.None
                                   select new
                                   {
                                       Result = t1
                                   }
                            ).Select(x => x.Result);
            }

            IEnumerable<ListKensaInfDetailItemModel> data = (from t1 in kensaInfDetails
                                                             join t2 in NoTrackingDataContext.KensaMsts
                                                              on new { t1.KensaItemCd, t1.HpId } equals new { t2.KensaItemCd, t2.HpId }
                                                             join t3 in NoTrackingDataContext.KensaInfs on new { t1.HpId, t1.PtId, t1.IraiCd } equals new { t3.HpId, t3.PtId, t3.IraiCd }
                                                             join t4 in NoTrackingDataContext.PtInfs on new { t1.PtId, t1.HpId } equals new { t4.PtId, t4.HpId }
                                                             join t5 in NoTrackingDataContext.KensaCmtMsts.Where(x => x.IsDeleted == DeleteTypes.None)
                                                                  on t1.CmtCd1 equals t5.CmtCd into leftJoinT5
                                                             from t5 in leftJoinT5.DefaultIfEmpty()
                                                             join t6 in NoTrackingDataContext.KensaCmtMsts.Where(x => x.IsDeleted == DeleteTypes.None)
                                                                  on t1.CmtCd2 equals t6.CmtCd into leftJoinT6
                                                             from t6 in leftJoinT6.DefaultIfEmpty()
                                                             join t7 in NoTrackingDataContext.KensaStdMsts
                                                                 on t1.KensaItemCd equals t7.KensaItemCd into leftJoinT7
                                                             from t7 in leftJoinT7.DefaultIfEmpty()
                                                             where t2.IsDelete == DeleteTypes.None && t3.IsDeleted == DeleteTypes.None && t4.IsDelete == DeleteTypes.None
                                                             where t2.KensaItemSeqNo == NoTrackingDataContext.KensaMsts.Where(m => m.HpId == t2.HpId && m.KensaItemCd == t2.KensaItemCd && m.IsDelete == DeleteTypes.None).Min(m => m.KensaItemSeqNo)
                                                             where t5.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t5.CmtCd && m.IsDeleted == DeleteTypes.None).Min(m => m.CmtSeqNo)
                                                             where t6.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t6.CmtCd && m.IsDeleted == DeleteTypes.None).Min(m => m.CmtSeqNo)
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
                                                                 (t3.CenterCd == t5.CenterCd || string.IsNullOrEmpty(t5.CenterCd)) ? (t5.CMT ?? string.Empty) : "不明",
                                                                 (t3.CenterCd == t6.CenterCd || string.IsNullOrEmpty(t6.CenterCd)) ? (t6.CMT ?? string.Empty) : "不明",
                                                                 t7.MaleStd ?? string.Empty,
                                                                 t7.FemaleStd ?? string.Empty,
                                                                 t7.MaleStdLow ?? string.Empty,
                                                                 t7.FemaleStdLow ?? string.Empty,
                                                                 t7.MaleStdHigh ?? string.Empty,
                                                                 t7.FemaleStdHigh ?? string.Empty,
                                                                 t2.Unit ?? string.Empty,
                                                                 t3.Nyubi ?? string.Empty,
                                                                 t3.Yoketu ?? string.Empty,
                                                                 t3.Bilirubin ?? string.Empty,
                                                                 t3.SikyuKbn,
                                                                 t3.TosekiKbn,
                                                                 t3.InoutKbn,
                                                                 t3.Status,
                                                                 DeleteTypes.None,
                                                                 t1.SeqGroupNo
                                                             ));

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
                    group.Key.Bilirubin,
                    group.Key.SikyuKbn,
                    group.Key.TosekiKbn,
                    index
                ));

            var kensaIraiCdSet = new HashSet<long>(kensaInfDetailCol.Select(item => item.IraiCd));

            data = data.Where(x => kensaIraiCdSet.Contains(x.IraiCd));

            var kensaItemDuplicate = data.GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.MaleStd, x.FemaleStd, x.IraiCd }).SelectMany(group => group.Skip(1)).Select(x => x);

            var seqNos = new HashSet<long>(kensaItemDuplicate.Select(item => item.SeqNo));

            var kensaItemWithOutDuplicate = data.GroupBy(x => new { x.KensaItemCd, x.SeqGroupNo })
                                                .Select(group =>
                                                {
                                                    var newItem = group.First();
                                                    newItem.SetRowSeqId(string.Join("-", group.Select(x => x.SeqNo)));
                                                    return newItem;
                                                })
                                                .ToList();

            // Get list with start date and end date
            if (endDate == 0)
            {
                data = data.Where(x => x.IraiDate >= startDate && x.IraiDate <= 99999999);
            }
            else
            {
                data = data.Where(x => x.IraiDate >= startDate && x.IraiDate <= endDate);
            }

            var kensaItemCds = data.GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.MaleStd, x.FemaleStd, x.SeqNo, x.SeqGroupNo }).Select(x => new { x.Key.KensaItemCd, x.Key.KensaName, x.Key.Unit, x.Key.MaleStd, x.Key.FemaleStd, x.Key.SeqNo, x.Key.SeqGroupNo });

            var kensaInfDetailData = new List<KensaInfDetailDataModel>();

            foreach (var kensaMstItem in kensaItemCds)
            {
                var dynamicArray = new List<ListKensaInfDetailItemModel>();

                foreach (var item in kensaInfDetailCol)
                {
                    var dynamicDataItem = data.Where(x => x.SeqGroupNo == kensaMstItem.SeqGroupNo && x.IraiCd == item.IraiCd && x.KensaItemCd == kensaMstItem.KensaItemCd).FirstOrDefault();

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
                     kensaMstItem.MaleStd,
                     kensaMstItem.FemaleStd,
                     kensaMstItem.SeqNo,
                     dynamicArray
                );

                kensaInfDetailData.Add(rowData);
            }

            var kensaInfDetailRows = new List<KensaInfDetailDataModel>();

            var groupRowDataItem = data
                .GroupBy(x => new { x.KensaItemCd, x.SeqGroupNo })
                .ToDictionary(
                    group =>
                    {
                        var newItem = group.First();
                        newItem.SetRowSeqId(string.Join("-", group.Select(x => x.SeqNo)));
                        return newItem;
                    },
                    group => group.ToList());

            foreach (var item in groupRowDataItem)
            {
                var kensaInfDetailDataItem = kensaInfDetailData.FirstOrDefault(x => x.SeqNo == item.Key.SeqNo);
                var rowData = new KensaInfDetailDataModel(
                     kensaInfDetailDataItem?.KensaItemCd ?? string.Empty,
                     kensaInfDetailDataItem?.KensaName ?? string.Empty,
                     kensaInfDetailDataItem?.Unit ?? string.Empty,
                     kensaInfDetailDataItem?.MaleStd ?? string.Empty,
                     kensaInfDetailDataItem?.FemaleStd ?? string.Empty,
                     item.Key.KensaKana,
                     item.Key.SortNo,
                     item.Key.SeqNo,
                     item.Key.SeqParentNo,
                     item.Key.RowSeqId,
                     kensaInfDetailDataItem?.DynamicArray ?? new()
                );
                kensaInfDetailRows.Add(rowData);
            }

            var kensaInfDetailRowItem = new List<KensaInfDetailDataModel>();

            if (setId == 0)
            {
                var sortCoulumItem = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 0).FirstOrDefault()?.Val;
                var sortTypeItem = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 1).FirstOrDefault()?.Val;

                // Get all parent item
                kensaInfDetailRowItem = kensaInfDetailRows.Where(x => x.SeqParentNo == 0).ToList();
                kensaInfDetailRowItem = SortRow(kensaInfDetailRowItem);

                // Children item
                var childrenItems = kensaInfDetailRows.Where(x => x.SeqParentNo > 0).ToList();

                // Append childrends
                for (int i = 0; i < kensaInfDetailRowItem.Count; i++)
                {
                    var item = kensaInfDetailRowItem[i];
                    var childrens = childrenItems.Where(x => item.RowSeqId.Contains(x.SeqParentNo.ToString())).ToList();
                    if (childrens != null && childrens.Any())
                    {
                        if (childrens.Count > 1)
                        {
                            childrens = SortRow(childrens);
                        }
                        kensaInfDetailRowItem.InsertRange(i + 1, childrens);
                    }
                }
                List<KensaInfDetailDataModel> SortRow(List<KensaInfDetailDataModel> data)
                {

                    switch (sortCoulumItem)
                    {
                        case SortKensaMstColumn.KensaItemCd:
                            if (sortTypeItem == 1)
                            {
                                return data.OrderByDescending(x => x.KensaItemCd).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaItemCd).ToList();
                            }
                        case SortKensaMstColumn.KensaKana:
                            if (sortTypeItem == 1)
                            {
                                return data.OrderByDescending(x => x.KensaKana).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaKana).ToList();
                            }
                        default:
                            if (sortTypeItem == 1)
                            {
                                return data.OrderByDescending(x => x.SortNo).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.SortNo).ToList();
                            }
                    }
                }
            }
            else
            {
                var kensasetDetail = NoTrackingDataContext.KensaSetDetails.Where(x => x.SetId == setId && x.IsDeleted == DeleteTypes.None).OrderBy(x => x.SortNo).ToList();
                foreach (var cunrentItemSet in kensasetDetail)
                {
                    var lastItemSet = kensasetDetail.LastOrDefault(x => x.KensaItemCd == cunrentItemSet.KensaItemCd);
                    if (cunrentItemSet == lastItemSet)
                    {
                        var listRow = kensaInfDetailRows.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).ToList();
                        if (listRow.Count > 0)
                        {
                            kensaInfDetailRowItem.AddRange(listRow);
                        }
                        else
                        {
                            var duplicatRow = kensaInfDetailRowItem.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).LastOrDefault();
                            if (duplicatRow != null)
                            {
                                kensaInfDetailRowItem.Add(duplicatRow);
                            }
                        }
                    }
                    else
                    {
                        var row = kensaInfDetailRows.FirstOrDefault(x => x.KensaItemCd == cunrentItemSet.KensaItemCd);
                        if (row == null)
                        {
                            var duplicatRow = kensaInfDetailRowItem.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).LastOrDefault();
                            if (duplicatRow != null)
                            {
                                kensaInfDetailRowItem.Add(duplicatRow);
                            }
                        }
                        else
                        {
                            kensaInfDetailRowItem.Add(row);
                            kensaInfDetailRows.Remove(row);
                        }
                    }
                }
            }

            //print Report
            List<string> itemName = new();
            foreach (var item in kensaInfDetailRowItem)
            {
                itemName.Add(item.KensaName);
            }

            itemName.Distinct();

            List<CoKensaResultMultiModel> result = new();
            List<KensaResultMultiItem> kensaResultMultiItems = new();
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);

            foreach (var item in kensaInfDetailRowItem)
            {
                switch (ptInf?.Sex)
                {
                    case 1: result.Add(new CoKensaResultMultiModel(item.KensaName, item.Unit, item.MaleStd, new(), new())); break;
                    case 2: result.Add(new CoKensaResultMultiModel(item.KensaName, item.Unit, item.FemaleStd, new(), new())); break;
                }

            }

            foreach (var item in kensaInfDetailRowItem)
            {
                foreach (var kensaResultMultiItem in item.DynamicArray)
                {
                    kensaResultMultiItems.Add(new KensaResultMultiItem(kensaResultMultiItem.ResultVal, kensaResultMultiItem.AbnormalKbn, kensaResultMultiItem.ResultType));
                }
            }

            List<long> date = new();

            if (itemName.Count == 0)
            {
                return (result, date);
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

            date.AddRange(kensaInfDetailCol.Select(x => x.IraiDate).ToList());
            date.Remove(0);

            if (userConf?.Where(x => x.GrpItemCd == 0).FirstOrDefault()?.Val == 0 || userConf?.Where(x => x.GrpItemCd == 0).FirstOrDefault()?.Val == null)
            {
                date = date.OrderBy(x => x).ToList();
            }
            else
            {
                date = date.OrderByDescending(x => x).ToList();
            }

            result.Add(new CoKensaResultMultiModel("", "", "", new(), date));

            return (result, date);
        }

        public ListKensaInfDetailModel GetListKensaInf(int hpId, int userId, long ptId, int setId, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int startDate)
        {
            IQueryable<KensaInfDetail> kensaInfDetails;
            List<long>? listSeqNoItems = new();

            var userConf = NoTrackingDataContext.UserConfs.Where(x => x.UserId == userId && x.HpId == hpId && x.GrpCd == 1002);

            var kensaSetDetailById = NoTrackingDataContext.KensaSetDetails.Where(x => x.SetId == setId && x.HpId == hpId && x.IsDeleted == DeleteTypes.None).GroupBy(item => item.KensaItemCd)
               .Select(group => new
               {
                   KensaItemCd = group.Key,
                   SortNo = group.Min(item => item.SortNo)
               });

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
                // Flter data with KensaSet
                kensaInfDetails = (from t1 in NoTrackingDataContext.KensaInfDetails
                                   join t2 in kensaSetDetailById on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId && t1.IsDeleted == DeleteTypes.None
                                   select new
                                   {
                                       Result = t1
                                   }
                            ).Select(x => x.Result);
            }

            IEnumerable<ListKensaInfDetailItemModel> data = (from t1 in kensaInfDetails
                        join t2 in NoTrackingDataContext.KensaMsts
                         on new { t1.KensaItemCd, t1.HpId } equals new { t2.KensaItemCd, t2.HpId }
                        join t3 in NoTrackingDataContext.KensaInfs on new { t1.HpId, t1.PtId, t1.IraiCd } equals new { t3.HpId, t3.PtId, t3.IraiCd }
                        join t4 in NoTrackingDataContext.PtInfs on new { t1.PtId, t1.HpId } equals new { t4.PtId, t4.HpId }
                        join t5 in NoTrackingDataContext.KensaCmtMsts.Where(x => x.IsDeleted == DeleteTypes.None)
                             on t1.CmtCd1 equals t5.CmtCd into leftJoinT5
                        from t5 in leftJoinT5.DefaultIfEmpty()
                        join t6 in NoTrackingDataContext.KensaCmtMsts.Where(x => x.IsDeleted == DeleteTypes.None)
                             on t1.CmtCd2 equals t6.CmtCd into leftJoinT6
                        from t6 in leftJoinT6.DefaultIfEmpty()
                        join t7 in NoTrackingDataContext.KensaStdMsts
                            on t1.KensaItemCd equals t7.KensaItemCd into leftJoinT7
                        from t7 in leftJoinT7.DefaultIfEmpty()
                        where t2.IsDelete == DeleteTypes.None && t3.IsDeleted == DeleteTypes.None && t4.IsDelete == DeleteTypes.None
                        where t2.KensaItemSeqNo == NoTrackingDataContext.KensaMsts.Where(m => m.HpId == t2.HpId && m.KensaItemCd == t2.KensaItemCd && m.IsDelete == DeleteTypes.None).Min(m => m.KensaItemSeqNo)
                        where t5.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t5.CmtCd && m.IsDeleted == DeleteTypes.None).Min(m => m.CmtSeqNo)
                        where t6.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t6.CmtCd && m.IsDeleted == DeleteTypes.None).Min(m => m.CmtSeqNo)
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
                            (t3.CenterCd == t5.CenterCd || string.IsNullOrEmpty(t5.CenterCd)) ? (t5.CMT ?? string.Empty) : "不明",
                            (t3.CenterCd == t6.CenterCd || string.IsNullOrEmpty(t6.CenterCd)) ? (t6.CMT ?? string.Empty) : "不明",
                            t7.MaleStd ?? string.Empty,
                            t7.FemaleStd ?? string.Empty,
                            t7.MaleStdLow ?? string.Empty,
                            t7.FemaleStdLow ?? string.Empty,
                            t7.MaleStdHigh ?? string.Empty,
                            t7.FemaleStdHigh ?? string.Empty,
                            t2.Unit ?? string.Empty,
                            t3.Nyubi ?? string.Empty,
                            t3.Yoketu ?? string.Empty,
                            t3.Bilirubin ?? string.Empty,
                            t3.SikyuKbn,
                            t3.TosekiKbn,
                            t3.InoutKbn,
                            t3.Status,
                            DeleteTypes.None,
                            t1.SeqGroupNo
                        ));

            if (showAbnormalKbn)
            {
                data = data.Where(x => x.AbnormalKbn.Equals(AbnormalKbnType.High) || x.AbnormalKbn.Equals(AbnormalKbnType.Low));
            }

            #region Get Col dynamic

            // Sort col by IraiDate
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
                    group.Key.Bilirubin,
                    group.Key.SikyuKbn,
                    group.Key.TosekiKbn,
                    index
                ));


            var totalCol = kensaInfDetailCol.Count();

            if (!listSeqNoItems.Any())
            {
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
            }

            #endregion

            #region Get Row dynamic
            // Filter data with col
            var kensaIraiCdSet = new HashSet<long>(kensaInfDetailCol.Select(item => item.IraiCd));
            data = data.Where(x => kensaIraiCdSet.Contains(x.IraiCd));

            var groupRowData = data
                .GroupBy(x => new { x.KensaItemCd, x.SeqGroupNo })
                .ToDictionary(
                    group =>
                    {
                        var newItem = group.First();
                        newItem.SetRowSeqId(string.Join("-", group.Select(x => x.SeqNo)));
                        return newItem;
                    },
                    group => group.ToList());

            var kensaInfDetailData = new List<KensaInfDetailDataModel>();

            foreach (var item in groupRowData)
            {
                var row = item.Key;
                kensaInfDetailData.Add(new KensaInfDetailDataModel(
                row.KensaItemCd,
                row.KensaName,
                row.Unit,
                row.MaleStd,
                row.FemaleStd,
                row.KensaKana,
                row.SortNo,
                row.SeqNo,
                row.SeqParentNo,
                row.RowSeqId,
                item.Value
            ));
            }

            // Sort row by user config
            List<KensaInfDetailDataModel> kensaInfDetailRows = new List<KensaInfDetailDataModel>();

            if (setId == 0 && showAbnormalKbn == false)
            {
                var sortCoulum = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 0).FirstOrDefault()?.Val;
                var sortType = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 1).FirstOrDefault()?.Val;

                // Get all parent item
                kensaInfDetailRows = kensaInfDetailData.Where(x => x.SeqParentNo == 0).ToList();
                kensaInfDetailRows = SortRow(kensaInfDetailRows);

                // Children item
                var childrenItems = kensaInfDetailData.Where(x => x.SeqParentNo > 0).ToList();

                // Append childrends
                for (int i = 0; i < kensaInfDetailRows.Count; i++)
                {
                    var item = kensaInfDetailRows[i];
                    var childrens = childrenItems.Where(x => item.RowSeqId.Contains(x.SeqParentNo.ToString())).ToList();
                    if (childrens != null && childrens.Any())
                    {
                        if (childrens.Count > 1)
                        {
                            childrens = SortRow(childrens);
                        }
                        kensaInfDetailRows.InsertRange(i + 1, childrens);
                    }
                }
                List<KensaInfDetailDataModel> SortRow(List<KensaInfDetailDataModel> data)
                {

                    switch (sortCoulum)
                    {
                        case SortKensaMstColumn.KensaItemCd:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.KensaItemCd).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaItemCd).ToList();
                            }
                        case SortKensaMstColumn.KensaKana:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.KensaKana).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaKana).ToList();
                            }
                        default:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.SortNo).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.SortNo).ToList();
                            }
                    }
                }
            }

            // Filter row by KensaSet
            else if(setId != 0 && showAbnormalKbn == false)
            {
                var kensasetDetail = NoTrackingDataContext.KensaSetDetails.Where(x => x.SetId == setId && x.IsDeleted == DeleteTypes.None).OrderBy(x => x.SortNo).ToList();
                foreach (var cunrentItemSet in kensasetDetail)
                {
                    var lastItemSet = kensasetDetail.LastOrDefault(x => x.KensaItemCd == cunrentItemSet.KensaItemCd);
                    if (cunrentItemSet == lastItemSet)
                    {
                        var listRow = kensaInfDetailData.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).ToList();
                        if (listRow.Count > 0)
                        {
                            kensaInfDetailRows.AddRange(listRow);
                        }
                        else
                        {
                            var duplicatRow = kensaInfDetailRows.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).LastOrDefault();
                            if (duplicatRow != null)
                            {
                                kensaInfDetailRows.Add(duplicatRow);
                            }
                        }
                    }
                    else
                    {
                        var row = kensaInfDetailData.FirstOrDefault(x => x.KensaItemCd == cunrentItemSet.KensaItemCd);
                        if (row == null)
                        {
                            var duplicatRow = kensaInfDetailRows.Where(x => x.KensaItemCd == cunrentItemSet.KensaItemCd).LastOrDefault();
                            if (duplicatRow != null)
                            {
                                kensaInfDetailRows.Add(duplicatRow);
                            }
                        }
                        else
                        {
                            kensaInfDetailRows.Add(row);
                            kensaInfDetailData.Remove(row);
                        }
                    }
                }
            }
            else
            {
                var sortCoulum = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 0).FirstOrDefault()?.Val;
                var sortType = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 1).FirstOrDefault()?.Val;

                // Get all parent item
                kensaInfDetailRows = SortRow(kensaInfDetailData);

                List<KensaInfDetailDataModel> SortRow(List<KensaInfDetailDataModel> data)
                {
                    switch (sortCoulum)
                    {
                        case SortKensaMstColumn.KensaItemCd:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.KensaItemCd).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaItemCd).ToList();
                            }
                        case SortKensaMstColumn.KensaKana:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.KensaKana).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.KensaKana).ToList();
                            }
                        default:
                            if (sortType == 1)
                            {
                                return data.OrderByDescending(x => x.SortNo).ToList();
                            }
                            else
                            {
                                return data.OrderBy(x => x.SortNo).ToList();
                            }
                    }
                }
            }
            #endregion

            #region Filter by list seqNo get data for chart
            if (listSeqNoItems != null && listSeqNoItems.Count > 0)
            {
                bool IsNumeric(string input)
                {
                    double result;
                    return double.TryParse(input, out result);
                }

                //Filter rows get row by list seqNo
                kensaInfDetailRows = kensaInfDetailRows.Where(x => listSeqNoItems.Contains(x.SeqNo)).ToList();
                var uniqueIraiCds = kensaInfDetailRows
                       .SelectMany(item => item.DynamicArray)
                       .Where(x => IsNumeric(x.ResultVal))
                       .Select(subItem => subItem.IraiCd)
                       .Where(iraiCd => iraiCd != 0)
                       .Distinct().ToList();

                // Filter col by list Iraicd 
                kensaInfDetailCol = kensaInfDetailCol.Where(x => uniqueIraiCds.Contains(x.IraiCd)).ToList();

                int index = 0;
                foreach (var item in kensaInfDetailCol)
                {
                    item.SetIndex(index);
                    index++;
                }

                // pagging
                totalCol = kensaInfDetailCol.Count();
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

                // Filter  data dynamic array by list col pagging
                var iraiCds = new HashSet<long>(kensaInfDetailCol.Select(item => item.IraiCd));

                kensaInfDetailRows = kensaInfDetailRows.Select(item => new KensaInfDetailDataModel(
                        item.KensaItemCd,
                        item.KensaName,
                        item.Unit,
                        item.MaleStd,
                        item.FemaleStd,
                        item.KensaKana,
                        item.SortNo,
                        item.SeqNo,
                        item.SeqParentNo,
                        item.DynamicArray.Where(x => iraiCds.Contains(x.IraiCd)).ToList()
                    )).ToList();
            }
            #endregion

            var result = new ListKensaInfDetailModel(kensaInfDetailCol.ToList(), kensaInfDetailRows, totalCol);
            return result;
        }
    }
}