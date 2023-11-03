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
using System.Data;
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
                        int maxKensaSetSortNo = NoTrackingDataContext.KensaSets.Where(c => c.HpId == hpId).AsEnumerable().Select(c => c.SortNo).DefaultIfEmpty(0).Max();
                        // Create kensaSet
                        if (setId == 0)
                        {
                            maxKensaSetSortNo++;
                            var kensaSet = TrackingDataContext.KensaSets.Add(new KensaSet()
                            {
                                HpId = hpId,
                                CreateId = userId,
                                UpdateId = userId,
                                SetName = setName,
                                SortNo = maxKensaSetSortNo,
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
                            if (sortNo > 0)
                            {
                                KensaSet.SortNo = sortNo;
                            }
                            KensaSet.IsDeleted = isDeleted;
                            KensaSet.UpdateId = userId;
                            KensaSet.UpdateDate = CIUtil.GetJapanDateTimeNow();

                            // Delete kensaSetDetail
                            if (isDeleted == DeleteTypes.Deleted)
                            {
                                var kensaSetDetails = TrackingDataContext.KensaSetDetails.Where(x => x.IsDeleted == DeleteTypes.None && x.SetId == setId && x.HpId == hpId).ToList();
                                foreach (var item in kensaSetDetails)
                                {
                                    item.UpdateId = userId;
                                    item.IsDeleted = DeleteTypes.Deleted;
                                    item.UpdateMachine = CIUtil.GetComputerName();
                                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                }
                            }
                        }

                        if (isDeleted == DeleteTypes.None)
                        {
                            int maxKensaSetDetailSortNo = NoTrackingDataContext.KensaSetDetails.Where(c => c.HpId == hpId && c.SetId == setId).AsEnumerable().Select(c => c.SortNo).DefaultIfEmpty(0).Max();

                            // Create kensaSetDetail Parent
                            var uniqIdParents = new HashSet<string>(kensaSetDetails.Where(x => x.SetEdaNo == 0 && !string.IsNullOrEmpty(x.UniqIdParent)).Select(item => item.UniqIdParent));
                            foreach (var item in kensaSetDetails.Where(x => x.SetEdaNo == 0 && uniqIdParents.Contains(x.UniqId)))
                            {
                                var kensaSetDetailParent = TrackingDataContext.KensaSetDetails.Add(new KensaSetDetail()
                                {
                                    HpId = hpId,
                                    SetId = kensaSetId,
                                    KensaItemCd = item.KensaItemCd,
                                    KensaItemSeqNo = item.KensaItemSeqNo,
                                    SortNo = item.SortNo == 0 ? ++maxKensaSetDetailSortNo : item.SortNo,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    CreateMachine = CIUtil.GetComputerName(),
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = DeleteTypes.None,
                                });
                                TrackingDataContext.SaveChanges();
                                int setEdaNoParent = kensaSetDetailParent.Entity.SetEdaNo;

                                // Create kensaSetDetail Children
                                foreach (var child in kensaSetDetails.Where(x => x.SetEdaNo == 0 && x.UniqIdParent.Equals(item.UniqId)))
                                {
                                    TrackingDataContext.KensaSetDetails.Add(new KensaSetDetail()
                                    {
                                        HpId = hpId,
                                        SetId = kensaSetId,
                                        KensaItemCd = child.KensaItemCd,
                                        KensaItemSeqNo = child.KensaItemSeqNo,
                                        SortNo = child.SortNo == 0 ? ++maxKensaSetDetailSortNo : child.SortNo,
                                        CreateId = userId,
                                        UpdateId = userId,
                                        SetEdaParentNo = setEdaNoParent,
                                        CreateMachine = CIUtil.GetComputerName(),
                                        UpdateMachine = CIUtil.GetComputerName(),
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        IsDeleted = DeleteTypes.None,
                                    });
                                }
                            }

                            // Create kensaSetDetail no children
                            foreach (var child in kensaSetDetails.Where(x => x.SetEdaNo == 0 && string.IsNullOrEmpty(x.UniqIdParent) && !uniqIdParents.Contains(x.UniqId)))
                            {
                                TrackingDataContext.KensaSetDetails.Add(new KensaSetDetail()
                                {
                                    HpId = hpId,
                                    SetId = kensaSetId,
                                    KensaItemCd = child.KensaItemCd,
                                    KensaItemSeqNo = child.KensaItemSeqNo,
                                    SortNo = child.SortNo == 0 ? ++maxKensaSetDetailSortNo : child.SortNo,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    SetEdaParentNo = 0,
                                    CreateMachine = CIUtil.GetComputerName(),
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = DeleteTypes.None,
                                });
                            }

                            // Update kensaSetDetail
                            foreach (var item in kensaSetDetails.Where(x => x.SetEdaNo != 0))
                            {
                                var kensaSetDetail = TrackingDataContext.KensaSetDetails.FirstOrDefault(x => x.HpId == hpId && x.SetId == item.SetId && x.SetEdaNo == item.SetEdaNo);
                                if (kensaSetDetail == null)
                                {
                                    transaction.Rollback();
                                }
                                if (kensaSetDetail.SortNo > 0)
                                {
                                    kensaSetDetail.SortNo = item.SortNo;
                                }
                                kensaSetDetail.IsDeleted = item.IsDeleted;
                                kensaSetDetail.UpdateId = userId;
                                kensaSetDetail.UpdateMachine = CIUtil.GetComputerName();
                                kensaSetDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();

                                // Delete kensaSetDetail childrens
                                if (item.IsDeleted == DeleteTypes.Deleted)
                                {
                                    var itemCdChildrens = NoTrackingDataContext.KensaSetDetails.Where(x => x.SetEdaParentNo == item.SetEdaNo).Select(x => x.KensaItemCd).ToList();
                                    var kensaSetDetailChildrens = TrackingDataContext.KensaSetDetails.Where(x => x.IsDeleted == DeleteTypes.None && x.SetId == setId && x.HpId == hpId
                                    && itemCdChildrens.Contains(x.KensaItemCd)).ToList();

                                    foreach (var setDetail in kensaSetDetailChildrens)
                                    {
                                        setDetail.IsDeleted = DeleteTypes.Deleted;
                                        setDetail.UpdateId = userId;
                                        setDetail.UpdateMachine = CIUtil.GetComputerName();
                                        setDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
            return NoTrackingDataContext.KensaSets.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).OrderBy(x => x.SortNo).Select(x => new KensaSetModel(
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
                        join t3 in NoTrackingDataContext.KensaStdMsts
                             on t1.KensaItemCd equals t3.KensaItemCd into leftJoinT3
                        from t3 in leftJoinT3.DefaultIfEmpty()
                        where t1.HpId == hpId && t1.SetId == setId && t1.IsDeleted == DeleteTypes.None
                        orderby t1.SortNo
                        select new KensaSetDetailModel(
                        t1.HpId,
                        t1.SetId,
                        t1.SetEdaNo,
                        t1.SetEdaParentNo,
                        t1.KensaItemCd,
                        t2.OyaItemCd ?? string.Empty,
                        t2.KensaName ?? string.Empty,
                        t1.KensaItemSeqNo,
                        t1.SortNo,
                        new(),
                        t1.IsDeleted,
                        t3.MaleStd ?? string.Empty,
                        t3.FemaleStd ?? string.Empty,
                        t3.MaleStdLow ?? string.Empty,
                        t3.FemaleStdLow ?? string.Empty,
                        t3.MaleStdHigh ?? string.Empty,
                        t3.FemaleStdHigh ?? string.Empty,
                        t2.Unit ?? string.Empty,
                        string.Empty,
                        string.Empty
                        )).ToList();

            var parents = data.Where(x => x.SetEdaParentNo == 0).ToList();

            foreach (var item in parents)
            {
                var childrens = data.Where(x => x.SetEdaParentNo == item.SetEdaNo).Select(x => new KensaSetDetailModel(
                       x.HpId,
                       x.SetId,
                       x.SetEdaNo,
                       x.SetEdaParentNo,
                       x.KensaItemCd,
                       x.OyaItemCd ?? string.Empty,
                       x.KensaName ?? string.Empty,
                       x.KensaItemSeqNo,
                       x.SortNo,
                       new(),
                       x.IsDeleted,
                       x.MaleStd,
                       x.FemaleStd,
                       x.MaleStdLow,
                       x.FemaleStdLow,
                       x.MaleStdHigh,
                       x.FemaleStdHigh,
                       x.Unit,
                       string.Empty,
                       string.Empty
                       )).ToList();
                res.Add(new KensaSetDetailModel(
                       item.HpId,
                       item.SetId,
                       item.SetEdaNo,
                       item.SetEdaParentNo,
                       item.KensaItemCd,
                       item.OyaItemCd ?? string.Empty,
                       item.KensaName ?? string.Empty,
                       item.KensaItemSeqNo,
                       item.SortNo,
                       childrens,
                       item.IsDeleted,
                       item.MaleStd,
                       item.FemaleStd,
                       item.MaleStdLow,
                       item.FemaleStdLow,
                       item.MaleStdHigh,
                       item.FemaleStdHigh,
                       item.Unit,
                       string.Empty,
                       string.Empty
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
                                  join t2 in NoTrackingDataContext.KensaCenterMsts
                                  on t1.CenterCd equals t2.CenterCd into leftJoinT2
                                  from t2 in leftJoinT2.DefaultIfEmpty()
                                  where t1.HpId == hpId && t1.IsDeleted == DeleteTypes.None && ((t1.CMT ?? "").ToUpper().Contains(bigKeyWord) || (t1.CmtCd != null && t1.CmtCd.Contains(keyWord)))
                                  where t1.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == hpId && m.CmtCd == t1.CmtCd).Min(m => m.CmtSeqNo)
                                  select new KensaCmtMstModel(
                                      t1.CmtCd ?? string.Empty,
                                      t1.CMT ?? string.Empty,
                                      t1.CmtSeqNo,
                                      t2.CenterName ?? string.Empty
                                  );
            return kensaInKensaMst.ToList();
        }

        public bool UpdateKensaInfDetail(int hpId, int userId, int ptId, long inputDataIraiCd, int inputDataIraiDate, List<KensaInfDetailUpdateModel> kensaInfDetails)
        {
            bool successed = false;
            long iraiCd = inputDataIraiCd;
            int iraiDate = inputDataIraiDate;
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        // Create KensaInf
                        if (iraiCd == 0)
                        {
                            var kensaInf = TrackingDataContext.KensaInfs.Add(new KensaInf()
                            {
                                HpId = hpId,
                                PtId = ptId,
                                IraiCd = 0,
                                IraiDate = iraiDate,
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
                            iraiCd = kensaInf.Entity.IraiCd;
                        }
                        else
                        {
                            var kensaInf = NoTrackingDataContext.KensaInfs.Where(x => x.HpId == hpId && x.IraiCd == iraiCd).FirstOrDefault();
                            if (kensaInf == null)
                            {
                                transaction.Rollback();
                            }
                            iraiDate = kensaInf.IraiDate;
                        }

                        var uniqIdParents = new HashSet<string>(kensaInfDetails.Where(x => x.SeqNo == 0 && !string.IsNullOrEmpty(x.UniqIdParent)).Select(item => item.UniqIdParent));

                        foreach (var item in kensaInfDetails.Where(x => uniqIdParents.Contains(x.UniqId)))
                        {
                            //Create kensaInfDetail Parent
                            long seqParentNo = 0;
                            if (item.IsDeleted == DeleteTypes.None)
                            {

                                var kensaInfDetailParent = TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                                {
                                    HpId = hpId,
                                    PtId = item.PtId,
                                    IraiCd = iraiCd,
                                    IraiDate = iraiDate,
                                    KensaItemCd = item.KensaItemCd,
                                    ResultVal = CIUtil.ToHalfsize(item.ResultVal),
                                    ResultType = item.ResultType,
                                    AbnormalKbn = item.AbnormalKbn,
                                    CmtCd1 = item.CmtCd1,
                                    CmtCd2 = item.CmtCd2,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    SeqParentNo = 0,
                                    CreateMachine = CIUtil.GetComputerName(),
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = DeleteTypes.None,
                                });
                                TrackingDataContext.SaveChanges();
                                seqParentNo = kensaInfDetailParent.Entity.SeqNo;
                            }

                            // Create children kensaInfDetail
                            foreach (var child in kensaInfDetails.Where(x => x.SeqNo == 0 && x.UniqIdParent.Equals(item.UniqId) && x.IsDeleted == DeleteTypes.None))
                            {
                                TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                                {
                                    HpId = hpId,
                                    PtId = child.PtId,
                                    IraiCd = iraiCd,
                                    IraiDate = iraiDate,
                                    KensaItemCd = child.KensaItemCd,
                                    ResultVal = CIUtil.ToHalfsize(child.ResultVal),
                                    ResultType = child.ResultType,
                                    AbnormalKbn = child.AbnormalKbn,
                                    CmtCd1 = child.CmtCd1,
                                    CmtCd2 = child.CmtCd2,
                                    CreateId = userId,
                                    UpdateId = userId,
                                    SeqParentNo = seqParentNo,
                                    UpdateMachine = CIUtil.GetComputerName(),
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    IsDeleted = DeleteTypes.None,
                                });
                            }
                        }

                        // Create kensaInfDetail no children
                        foreach (var item in kensaInfDetails.Where(x => x.SeqNo == 0 && string.IsNullOrEmpty(x.UniqIdParent) && !uniqIdParents.Contains(x.UniqId) && x.IsDeleted == DeleteTypes.None))
                        {
                            TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                            {
                                HpId = hpId,
                                PtId = item.PtId,
                                IraiCd = iraiCd,
                                IraiDate = iraiDate,
                                KensaItemCd = item.KensaItemCd,
                                ResultVal = CIUtil.ToHalfsize(item.ResultVal),
                                ResultType = item.ResultType,
                                AbnormalKbn = item.AbnormalKbn,
                                CmtCd1 = item.CmtCd1,
                                CmtCd2 = item.CmtCd2,
                                CreateId = userId,
                                UpdateId = userId,
                                SeqParentNo = 0,
                                UpdateMachine = CIUtil.GetComputerName(),
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                IsDeleted = DeleteTypes.None,
                            });
                        }


                        // Update kensaInfDetail

                        foreach (var item in kensaInfDetails.Where(x => x.SeqNo != 0))
                        {
                            var kensaInfDetail = TrackingDataContext.KensaInfDetails.FirstOrDefault(x => x.HpId == hpId && x.PtId == item.PtId && x.IraiCd == item.IraiCd && x.SeqNo == item.SeqNo);
                            if (kensaInfDetail == null)
                            {
                                transaction.Rollback();
                            }

                            // Delete children
                            if (item.IsDeleted == DeleteTypes.Deleted && kensaInfDetail.SeqParentNo == 0)
                            {
                                var childrens = TrackingDataContext.KensaInfDetails.Where(x => x.SeqParentNo == kensaInfDetail.SeqNo);
                                foreach (var child in childrens)
                                {
                                    child.IsDeleted = DeleteTypes.Deleted;
                                    child.UpdateId = userId;
                                    child.UpdateMachine = CIUtil.GetComputerName();
                                }
                            }

                            kensaInfDetail.ResultVal = item.ResultVal;
                            kensaInfDetail.ResultType = item.ResultType;
                            kensaInfDetail.AbnormalKbn = item.AbnormalKbn;
                            kensaInfDetail.CmtCd1 = item.CmtCd1;
                            kensaInfDetail.CmtCd2 = item.CmtCd2;
                            kensaInfDetail.IsDeleted = item.IsDeleted;
                            kensaInfDetail.UpdateId = userId;
                            kensaInfDetail.UpdateMachine = CIUtil.GetComputerName();
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

        public ListKensaInfDetailModel GetListKensaInfDetail(int hpId, int userId, long ptId, int setId, int iraiCd, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int itemQuantity, int startDate)
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
                // Flter data with KensaSet
                kensaInfDetails = (from t1 in NoTrackingDataContext.KensaInfDetails
                                   join t2 in NoTrackingDataContext.KensaSetDetails on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId && t2.SetId == setId
                                   select new
                                   {
                                       Result = t1
                                   }
                            ).Select(x => x.Result);
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
                        where t5.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t5.CmtCd).Min(m => m.CmtSeqNo)
                        where t6.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t6.CmtCd).Min(m => m.CmtSeqNo)
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
                            (t3.CenterCd != null && t3.CenterCd != t5.CenterCd) ? "不明" : t5.CMT ?? string.Empty,
                            (t3.CenterCd != null && t3.CenterCd != t6.CenterCd) ? "不明" : t6.CMT ?? string.Empty,
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
                            DeleteTypes.None
                        )).AsEnumerable();

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
                        kensaInfDetailCol = kensaInfDetailCol.TakeWhile(x => x.IraiCd != iraiCdStart).TakeLast(itemQuantity);
                    }
                    else
                    {
                        kensaInfDetailCol = kensaInfDetailCol.Skip(currentIndex + 1).Take(itemQuantity);
                    }
                }
                else
                {
                    if (getGetPrevious)
                    {
                        kensaInfDetailCol = kensaInfDetailCol.TakeLast(itemQuantity);
                    }
                    else
                    {
                        kensaInfDetailCol = kensaInfDetailCol.Take(itemQuantity);
                    }
                }
            }
            #endregion

            #region Get Row dynamic
            // Filter data with col
            var kensaIraiCdSet = new HashSet<long>(kensaInfDetailCol.Select(item => item.IraiCd));
            data = data.Where(x => kensaIraiCdSet.Contains(x.IraiCd));

            var kensaItemDuplicate = data.GroupBy(x => new { x.KensaItemCd, x.KensaName, x.Unit, x.MaleStd, x.FemaleStd, x.IraiCd }).SelectMany(group => group.Skip(1))
                .Select(x => x);
            var seqNos = new HashSet<long>(kensaItemDuplicate.Select(item => item.SeqNo));

            var kensaItemWithOutDuplicate = data.Where(x => !seqNos.Contains(x.SeqNo));


            var groupRowData = data
                .GroupBy(x => new { x.KensaItemCd })
                .ToDictionary(
                    group => group.Key.KensaItemCd,
                    group => group.Where(x => !seqNos.Contains(x.SeqNo)).ToList());


            var kensaInfDetailData = new List<KensaInfDetailDataModel>();

            foreach (var item in kensaItemWithOutDuplicate)
            {
                if (groupRowData.TryGetValue(item.KensaItemCd, out var dynamicArray))
                {
                    kensaInfDetailData.Add(new KensaInfDetailDataModel(
                        item.KensaItemCd,
                        item.KensaName,
                        item.Unit,
                        item.MaleStd,
                        item.FemaleStd,
                        item.KensaKana,
                        item.SortNo,
                        item.SeqNo,
                        item.SeqParentNo,
                        dynamicArray
                    ));
                }
            }

            foreach (var item in kensaItemDuplicate)
            {
                kensaInfDetailData.Add(new KensaInfDetailDataModel(
                    item.KensaItemCd,
                    item.KensaName,
                    item.Unit,
                    item.MaleStd,
                    item.FemaleStd,
                    item.KensaKana,
                    item.SortNo,
                    item.SeqNo,
                    item.SeqParentNo,
                    new List<ListKensaInfDetailItemModel> { item }
                ));
            }

            // Sort row by user config
            var kensaInfDetailRows = new List<KensaInfDetailDataModel>();
            if (setId == 0)
            {
                var sortCoulum = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 0).FirstOrDefault()?.Val;
                var sortType = userConf.Where(x => x.GrpItemCd == 1 && x.GrpItemEdaNo == 1).FirstOrDefault()?.Val;

                // Get all parent item
                kensaInfDetailRows = kensaInfDetailData.Where(x => x.SeqParentNo == 0).ToList();
                kensaInfDetailRows = SortRow(kensaInfDetailRows);
                // Children item
                var childrenItems = kensaInfDetailData.Where(x => x.SeqParentNo != 0).GroupBy(x => new { x.SeqParentNo })
                .ToDictionary(
                    group => group.Key.SeqParentNo,
                    group => group.ToList());

                // Append childrends
                for (int i = 0; i < kensaInfDetailRows.Count; i++)
                {
                    var item = kensaInfDetailRows[i];
                    if (childrenItems.TryGetValue(item.SeqNo, out var childrens))
                    {
                        if (childrens.Count() > 1)
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
            // Sort row by KensaSet SortNo
            else
            {

                kensaInfDetailRows = (from t1 in kensaInfDetailData
                                      join t2 in NoTrackingDataContext.KensaSetDetails on t1.KensaItemCd equals t2.KensaItemCd
                                      where t2.SetId == setId
                                      select new
                                      {
                                          Result = t1,
                                          SortNo = t2.SortNo,
                                      }
                           ).OrderBy(x => x.SortNo).Select(x => x.Result).ToList();
            }
            #endregion

            var result = new ListKensaInfDetailModel(kensaInfDetailCol.ToList(), kensaInfDetailRows, totalCol);
            return result;
        }

        public List<ListKensaInfDetailItemModel> GetKensaInfDetailByIraiCd(int hpId, int iraiCd)
        {
            var data = (from t1 in NoTrackingDataContext.KensaInfDetails.Where(x => x.IraiCd == iraiCd && x.HpId == hpId && x.IsDeleted == DeleteTypes.None)
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
                        && t3.IsDeleted == DeleteTypes.None
                        where t5.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t5.CmtCd).Min(m => m.CmtSeqNo)
                        where t6.CmtSeqNo == NoTrackingDataContext.KensaCmtMsts.Where(m => m.HpId == t2.HpId && m.CmtCd == t6.CmtCd).Min(m => m.CmtSeqNo)
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
                            (t3.CenterCd != null && t3.CenterCd != t5.CenterCd) ? "不明" : t5.CMT ?? string.Empty,
                            (t3.CenterCd != null && t3.CenterCd != t6.CenterCd) ? "不明" : t6.CMT ?? string.Empty,
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
                            DeleteTypes.None
                        )).ToList();
            return data;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
