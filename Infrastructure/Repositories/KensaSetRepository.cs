using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaInfDetail;
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
                x.UpdateDate,
                x.UpdateId
                )).ToList();
        }

        public List<KensaSetDetailModel> GetListKensaSetDetail(int hpId, int setId)
        {
            return NoTrackingDataContext.KensaSetDetails.Where(x => x.HpId == hpId && x.SetId == setId && x.IsDeleted == DeleteTypes.None).Select(x => new KensaSetDetailModel(
                x.HpId,
                x.SetId,
                x.SetEdaNo,
                x.KensaItemCd,
                x.KensaItemSeqNo,
                x.SortNo,
                x.IsDeleted
                )).ToList();
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
                        return false;
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
            return true;
        }


        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
