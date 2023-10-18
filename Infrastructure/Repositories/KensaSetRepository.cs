using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using Domain.Models.KensaSetDetail;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Domain.Models.KensaIrai.ListKensaInfDetailModel;

namespace Infrastructure.Repositories
{
    public class KensaSetRepository : RepositoryBase, IKensaSetRepository
    {
        public KensaSetRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted, List<KensaSetDetailModel> kensaSetDetails)
        {
            bool successed = false;
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        int kensaSetId = setId;
                        // Create kensaSet
                        if (setId == 0)
                        {
                            var kensaSet = TrackingDataContext.KensaSets.Add(new KensaSet()
                            {
                                HpId = hpId,
                                CreateId = userId,
                                UpdateId = userId,
                                SetName = setName,
                                SortNo = sortNo,
                                CreateMachine = CIUtil.GetComputerName(),
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                IsDeleted = 0,
                            });

                            TrackingDataContext.SaveChanges();
                            kensaSetId = kensaSet.Entity.SetId;
                        }

                        // Update kensaSet
                        else
                        {
                            var KensaSet = TrackingDataContext.KensaSets.FirstOrDefault(x => x.HpId == hpId && x.SetId == setId);
                            if (KensaSet == null)
                            {
                                transaction.Rollback();
                            }

                            KensaSet.SetName = setName;
                            KensaSet.SortNo = sortNo;
                            KensaSet.IsDeleted = isDeleted;
                            KensaSet.UpdateId = userId;
                            KensaSet.UpdateDate = CIUtil.GetJapanDateTimeNow();

                            // Delete kensaSetDetail
                            if (isDeleted == DeleteTypes.Deleted)
                            {
                                var kensaSetDetails = TrackingDataContext.KensaSetDetails.Where(x => x.IsDeleted == DeleteTypes.None && x.SetId == setId && x.HpId == hpId).ToList();
                                foreach (var item in kensaSetDetails)
                                {
                                    item.IsDeleted = DeleteTypes.Deleted;
                                }
                            }
                        }
                        if (isDeleted == DeleteTypes.None)
                        {
                            foreach (var item in kensaSetDetails)
                            {
                                // Create kensaSetDetail
                                if (item.SetEdaNo == 0)
                                {
                                    TrackingDataContext.KensaSetDetails.Add(new KensaSetDetail()
                                    {
                                        HpId = hpId,
                                        SetId = kensaSetId,
                                        KensaItemCd = item.KensaItemCd,
                                        KensaItemSeqNo = item.KensaItemSeqNo,
                                        SortNo = item.SortNo,
                                        CreateId = userId,
                                        UpdateId = userId,
                                        CreateMachine = CIUtil.GetComputerName(),
                                        UpdateMachine = CIUtil.GetComputerName(),
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        IsDeleted = DeleteTypes.None,
                                    });
                                }

                                // Update kensaSetDetail
                                else
                                {
                                    var kensaSetDetail = TrackingDataContext.KensaSetDetails.FirstOrDefault(x => x.HpId == hpId && x.SetId == item.SetId && x.SetEdaNo == item.SetEdaNo);
                                    if (kensaSetDetail == null)
                                    {
                                        transaction.Rollback();
                                    }
                                    kensaSetDetail.SortNo = item.SortNo;
                                    kensaSetDetail.IsDeleted = item.IsDeleted;
                                    kensaSetDetail.UpdateId = userId;
                                    kensaSetDetail.UpdateMachine = CIUtil.GetComputerName();
                                    kensaSetDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();

                                    // Delete kensaSetDetail childrens
                                    if (item.IsDeleted == DeleteTypes.Deleted)
                                    {
                                        var itemCdChildrens = NoTrackingDataContext.KensaMsts.Where(x => x.OyaItemCd == item.KensaItemCd).Select(x => x.KensaItemCd).ToList();
                                        var kensaSetDetailChildrens = TrackingDataContext.KensaSetDetails.Where(x => x.IsDeleted == DeleteTypes.None && x.SetId == setId && x.HpId == hpId
                                        && itemCdChildrens.Contains(x.KensaItemCd)).ToList();

                                        foreach (var setDetail in kensaSetDetailChildrens)
                                        {
                                            setDetail.IsDeleted = DeleteTypes.Deleted;
                                        }
                                    }
                                }
                            }
                        }

                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        successed = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                });
            return successed;
        }

        public List<KensaSetModel> GetListKensaSet(int hpId)
        {
            return NoTrackingDataContext.KensaSets.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => new KensaSetModel(
                x.HpId,
                x.SetId,
                x.SetName,
                x.SortNo,
                x.IsDeleted,
                x.CreateDate,
                x.CreateId,
                x.UpdateDate,
                x.UpdateId
                )).ToList();
        }

        public List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId)
        {
            var res = new List<KensaSetDetailModel>();
            var data = (from t1 in NoTrackingDataContext.KensaSetDetails
                        join t2 in NoTrackingDataContext.KensaMsts on t1.KensaItemCd equals t2.KensaItemCd
                        where t1.HpId == hpId && t1.SetId == setId && t1.IsDeleted == DeleteTypes.None
                        select new KensaSetDetailModel(
                        t1.HpId,
                        t1.SetId,
                        t1.SetEdaNo,
                        t1.KensaItemCd,
                        t2.OyaItemCd ?? string.Empty,
                        t2.KensaName ?? string.Empty,
                        t1.KensaItemSeqNo,
                        t1.SortNo,
                        new(),
                        t1.IsDeleted
                        )).ToList();

            var parents = data.Where(x => string.IsNullOrEmpty(x.OyaItemCd)).ToList();

            foreach (var item in parents)
            {
                var children = data.Where(x => x.OyaItemCd == item.KensaItemCd).Select(x => new KensaSetDetailModel(
                       x.HpId,
                       x.SetId,
                       x.SetEdaNo,
                       x.KensaItemCd,
                       x.OyaItemCd ?? string.Empty,
                       x.KensaName ?? string.Empty,
                       x.KensaItemSeqNo,
                       x.SortNo,
                       new(),
                       x.IsDeleted
                       )).ToList();
                res.Add(new KensaSetDetailModel(
                       item.HpId,
                       item.SetId,
                       item.SetEdaNo,
                       item.KensaItemCd,
                       item.OyaItemCd ?? string.Empty,
                       item.KensaName ?? string.Empty,
                       item.KensaItemSeqNo,
                       item.SortNo,
                       children,
                       item.IsDeleted
                       ));
            }

            return res;
        }

        public List<KensaCmtMstModel> GetListKensaCmtMst(int hpId, string keyWord)
        {
            string bigKeyWord = keyWord.ToUpper()
                                   .Replace("ｧ", "ｱ")
                                   .Replace("ｨ", "ｲ")
                                   .Replace("ｩ", "ｳ")
                                   .Replace("ｪ", "ｴ")
                                   .Replace("ｫ", "ｵ")
                                   .Replace("ｬ", "ﾔ")
                                   .Replace("ｭ", "ﾕ")
                                   .Replace("ｮ", "ﾖ")
                                   .Replace("ｯ", "ﾂ");

            // Get kensa in KensaMst
            var kensaInKensaMst = from t1 in NoTrackingDataContext.KensaCmtMsts
                                  join t2 in NoTrackingDataContext.KensaCenterMsts on t1.CenterCd equals t2.CenterCd
                                  where t1.HpId == hpId && t1.IsDeleted == DeleteTypes.None && (t1.CMT ?? "").ToUpper().Contains(bigKeyWord)
                                  select new KensaCmtMstModel(
                                      t1.CmtCd,
                                      t1.CMT ?? string.Empty,
                                      t1.CmtSeqNo,
                                      t2.CenterName ?? string.Empty
                                  );
            return kensaInKensaMst.ToList();
        }

        public bool UpdateKensaInfDetail(int hpId, int userId, List<KensaInfDetailUpdateModel> kensaInfDetails)
        {
            bool successed = false;
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        foreach (var item in kensaInfDetails)
                        {
                            // Create
                            if (item.SeqNo == 0)
                            {

                                // Create KensaInf
                                var kensaInf = TrackingDataContext.KensaInfs.Add(new KensaInf()
                                {
                                    HpId = hpId,
                                    PtId = item.PtId,
                                    IraiCd = 0,
                                    IraiDate = item.IraiDate,
                                    RaiinNo = item.RaiinNo,
                                    InoutKbn = 0,
                                    Status = 0,
                                    TosekiKbn = 0,
                                    SikyuKbn = 0,
                                    ResultCheck = 0,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    CreateMachine = CIUtil.GetComputerName(),
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = 0
                                });

                                TrackingDataContext.SaveChanges();

                                //Create kensaInfDetail
                                TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                                {
                                    HpId = hpId,
                                    PtId = item.PtId,
                                    IraiCd = kensaInf.Entity.IraiCd,
                                    IraiDate = item.IraiDate,
                                    RaiinNo = item.RaiinNo,
                                    KensaItemCd = item.KensaItemCd,
                                    ResultVal = CIUtil.ToHalfsize(item.ResultVal),
                                    ResultType = item.ResultType,
                                    AbnormalKbn = item.AbnormalKbn,
                                    CmtCd1 = item.CmtCd1,
                                    CmtCd2 = item.CmtCd2,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    CreateMachine = CIUtil.GetComputerName(),
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = 0,
                                });
                            }

                            // Update kensaInfDetail
                            else
                            {
                                var kensaInfDetail = TrackingDataContext.KensaInfDetails.FirstOrDefault(x => x.HpId == hpId && x.PtId == item.PtId && x.IraiCd == item.IraiCd && x.SeqNo == item.SeqNo);
                                if (kensaInfDetail == null)
                                {
                                    transaction.Rollback();
                                }

                                kensaInfDetail.ResultVal = item.ResultVal;
                                kensaInfDetail.ResultType = item.ResultType;
                                kensaInfDetail.AbnormalKbn = item.AbnormalKbn;
                                kensaInfDetail.CmtCd1 = item.CmtCd1;
                                kensaInfDetail.CmtCd2 = item.CmtCd2;
                                kensaInfDetail.IsDeleted = item.IsDeleted;
                                kensaInfDetail.UpdateId = userId;
                                kensaInfDetail.UpdateMachine = CIUtil.GetComputerName();
                                kensaInfDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();

                            }
                        }

                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        successed = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                });
            return successed;
        }

        public ListKensaInfDetailModel GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity)
        {
            IQueryable<KensaInfDetail> kensaInfDetails;

            var userConf = NoTrackingDataContext.UserConfs.Where(x => x.UserId == userId && x.HpId == hpId && x.GrpCd == 1002);

            bool SortIraiDateAsc = true;

            if (userConf.Where(x => x.GrpItemCd == 1).FirstOrDefault()?.Val == 1)
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

            if (iraiCd != 0)
            {
                kensaInfDetails = kensaInfDetails.Where(x => x.IraiCd == iraiCd);
            }
            var data = (from t1 in kensaInfDetails
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
                            t4.Sex == 1 ? t2.MaleStdLow ?? string.Empty : t2.FemaleStdLow ?? string.Empty,
                            t4.Sex == 1 ? t2.MaleStdHigh ?? string.Empty : t2.FemaleStdHigh ?? string.Empty,
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
                        )).AsEnumerable();

            if (showAbnormalKbn)
            {
                data = data.Where(x => x.AbnormalKbn.Equals(AbnormalKbnType.High) || x.AbnormalKbn.Equals(AbnormalKbnType.Low));
            }

            // Sort data by user setting
            if (setId == 0)
            {

                var confSort = userConf.Where(x => x.GrpItemCd == 2).FirstOrDefault();
                var sortType = confSort?.Val;
                var sortCoulum = confSort?.Param;

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
                    case SortKensaMstColumn.KensaKna:
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

            // Get list with start date
            if (SortIraiDateAsc && startDate != 0)
            {
                data = data.Where(x => x.IraiDate >= startDate);
            }
            else if (startDate != 0)
            {
                data = data.Where(x => x.IraiDate <= startDate);
            }

            var kensaItemCds = data.GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.Std }).Select(x => new { x.Key.KensaItemCd, x.Key.KensaName, x.Key.Unit, x.Key.Std });

            // Get list iraiCd
            IOrderedEnumerable<object> kensaInfDetailColOrder = data.OrderBy(x => x.IraiDate);
            var kensaInfDetailCol = SortIraiDateAsc ? data
                                .OrderBy(x => x.IraiDate)
                                .GroupBy(x => new { x.IraiCd, x.IraiDate, x.Nyubi, x.Yoketu, x.Bilirubin, x.SikyuKbn, x.TosekiKbn })
                                .Select(group => new KensaInfDetailColModel(group.Key.IraiCd, group.Key.IraiDate, group.Key.Nyubi, group.Key.Yoketu, group.Key.TosekiKbn, group.Key.SikyuKbn, group.Key.TosekiKbn))

                            : data.OrderByDescending(x => x.IraiDate)
                                .GroupBy(x => new { x.IraiCd, x.IraiDate, x.Nyubi, x.Yoketu, x.Bilirubin, x.SikyuKbn, x.TosekiKbn })
                                 .Select(group => new KensaInfDetailColModel(group.Key.IraiCd, group.Key.IraiDate, group.Key.Nyubi, group.Key.Yoketu, group.Key.TosekiKbn, group.Key.SikyuKbn, group.Key.TosekiKbn));

            kensaInfDetailCol = kensaInfDetailCol.Take(itemQuantity);


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

            var result = new ListKensaInfDetailModel(kensaInfDetailCol.ToList(), kensaInfDetailData);
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
