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
                    catch
                    {
                        transaction.Rollback();
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
                x.CreateMachine,
                x.UpdateDate,
                x.UpdateId,
                x.UpdateMachine
                )).ToList();
        }

        public List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId)
        {
            var result = (from t1 in NoTrackingDataContext.KensaSetDetails
                          join t2 in NoTrackingDataContext.KensaMsts on t1.KensaItemCd equals t2.KensaItemCd
                          where t1.HpId == hpId && t1.SetId == setId && t1.IsDeleted == DeleteTypes.None
                          select new KensaSetDetailModel(
                          t1.HpId,
                          t1.SetId,
                          t1.SetEdaNo,
                          t1.KensaItemCd,
                          t2.KensaName,
                          t1.KensaItemSeqNo,
                          t1.SortNo,
                          t1.IsDeleted
                          )).ToList();
            return result;
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

            //get kensa in KensaMst
            var kensaInKensaMst = from t1 in NoTrackingDataContext.KensaCmtMsts
                                  join t2 in NoTrackingDataContext.KensaCenterMsts on t1.CenterCd equals t2.CenterCd
                                  where t1.HpId == hpId && t1.IsDeleted == DeleteTypes.None && (t1.CMT ?? "").ToUpper().Contains(bigKeyWord)
                                  select new KensaCmtMstModel(t1.CmtCd, t1.CMT, t1.CmtSeqNo, t2.CenterName);
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
                            // Create kensaInfDetail
                            if (item.SeqNo == 0)
                            {
                                TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                                {
                                    HpId = hpId,
                                    PtId = item.PtId,
                                    IraiCd = item.IraiCd,
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
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                });
            return successed;
        }

        public (List<ListKensaInfDetailModel>, int) GetListKensaInfDetail(int hpId, int userId, int ptId, int setId, int pageIndex, int pageSize)
        {
            var result = new List<ListKensaInfDetailModel>();
            var kensaInfDetails = new List<KensaInfDetail>();
            UserConf userConf = null;

            if (setId == 0)
            {
                kensaInfDetails = NoTrackingDataContext.KensaInfDetails
               .Where(x => x.HpId == hpId && x.PtId == ptId).OrderBy(x => x.IraiDate)
               .ToList();

                userConf = NoTrackingDataContext.UserConfs.FirstOrDefault(x => x.UserId == userId && x.HpId == hpId && x.GrpCd == 1002);
            }

            else
            {
                // Fllter data with KensaSet
                kensaInfDetails = (from t1 in NoTrackingDataContext.KensaInfDetails
                                   join t2 in NoTrackingDataContext.KensaSetDetails on t1.KensaItemCd equals t2.KensaItemCd
                                   where t1.HpId == hpId && t1.PtId == ptId
                                   select new
                                   {
                                       result = t1
                                   }
                            ).Select(x => x.result).OrderBy(x => x.IraiDate).ToList();
            }

            // Get list iraiCd
            var iraiCds = kensaInfDetails
                .GroupBy(x => x.IraiCd)
                .Select(group => group.Key)
                    .ToList();

            var total = iraiCds.Count();

            iraiCds = iraiCds.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var sex = 1;
            foreach (var item in iraiCds)
            {
                var kensaInfDetailByIraiCds = (from t1 in kensaInfDetails.Where(x => x.IraiCd == item)
                                               join t2 in NoTrackingDataContext.KensaMsts
                                                on new { t1.KensaItemCd, t1.HpId } equals new { t2.KensaItemCd, t2.HpId }
                                               join t3 in NoTrackingDataContext.KensaInfs on new { t1.HpId, t1.PtId, t1.IraiCd } equals new { t3.HpId, t3.PtId, t3.IraiCd }
                                               //join t4 in NoTrackingDataContext.PtInfs on new { t1.PtId, t1.HpId } equals new { t4.PtId, t4.HpId }
                                               select new ListKensaInfDetailItem(
                                                   t1.SeqNo,
                                                   t2.KensaName ?? string.Empty,
                                                   t1.KensaItemCd ?? string.Empty,
                                                   t1.ResultVal ?? string.Empty,
                                                   t1.ResultType ?? string.Empty,
                                                   t1.AbnormalKbn ?? string.Empty,
                                                   t1.CmtCd1 ?? string.Empty,
                                                   t1.CmtCd2 ?? string.Empty,
                                                   sex == 1 ? t2.MaleStd ?? string.Empty : t2.FemaleStd ?? string.Empty,
                                                  sex == 1 ? t2.MaleStdLow ?? string.Empty : t2.FemaleStdLow ?? string.Empty,
                                                  sex == 1 ? t2.MaleStdHigh ?? string.Empty : t2.FemaleStdHigh ?? string.Empty,
                                                   t2.Unit ?? string.Empty,
                                                   t3.Nyubi ?? string.Empty,
                                                   t3.Yoketu ?? string.Empty,
                                                   t3.Bilirubin ?? string.Empty,
                                                   t3.SikyuKbn,
                                                   t3.TosekiKbn
                                               )).ToList();

                var firstItem = kensaInfDetails.Where(x => x.IraiCd == item).First();

                result.Add(new ListKensaInfDetailModel(
                        ptId,
                        item,
                        firstItem.RaiinNo,
                        firstItem.IraiDate,
                        kensaInfDetailByIraiCds
                    ));
            }
            return (result, total);
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
