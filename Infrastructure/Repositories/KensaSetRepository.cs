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
                        }

                        foreach (var item in kensaSetDetails)
                        {
                            // Create kensaSetDetail
                            if (item.SetId == 0 && item.SetEdaNo == 0)
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
                                    IsDeleted = 0,
                                });
                            }

                            // Update kensaSetDetail
                            else
                            {
                                var kensaSetDetail = TrackingDataContext.KensaSetDetails.FirstOrDefault(x => x.HpId == item.HpId && x.SetId == item.SetId && x.SetEdaNo == item.SetEdaNo);
                                if (kensaSetDetail == null)
                                {
                                    transaction.Rollback();
                                }
                                kensaSetDetail.SortNo = item.SortNo;
                                kensaSetDetail.IsDeleted = item.IsDeleted;
                                kensaSetDetail.UpdateId = userId;
                                kensaSetDetail.UpdateMachine = CIUtil.GetComputerName();
                                kensaSetDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();

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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
