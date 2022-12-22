using Domain.Models.GroupInf;
using Domain.Models.PtGroupMst;
using Entity.Tenant;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;

namespace Infrastructure.Repositories
{
    public class GroupNameMstRepository : RepositoryBase, IGroupNameMstRepository
    {
        public GroupNameMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        /// <summary>
        /// FE logic
        /// GrpName : filter & only pass record want save to db
        /// GrpIdtem : when delete in screen : not seq no => Remove else . set IsDeleted = 1.
        /// </summary>
        /// <param name="groupNameMsts"></param>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool SaveGroupNameMst(List<GroupNameMstModel> groupNameMsts, int hpId, int userId)
        {
            var grpNameDatabases = TrackingDataContext.PtGrpNameMsts.Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId).ToList();
            var grpNameDeletes = grpNameDatabases.Where(x => !groupNameMsts.Any(o => o.GrpId == x.GrpId));
            foreach(var item in grpNameDeletes)
            {
                item.IsDeleted = DeleteTypes.Deleted;
                item.UpdateDate = DateTime.UtcNow;
                item.UpdateId = userId;

                var grpItemDeletes = TrackingDataContext.PtGrpItems.Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId
                                        && x.GrpId == item.GrpId);

                foreach(var itemGrp in grpItemDeletes)
                {
                    itemGrp.IsDeleted = DeleteTypes.Deleted;
                    itemGrp.UpdateDate = DateTime.UtcNow;
                    itemGrp.UpdateId = userId;
                }
            }

            foreach(var item in groupNameMsts) //Add or Update
            {
                var itemAct = grpNameDatabases.FirstOrDefault(x => x.GrpId == item.GrpId);
                if(itemAct is null)
                {
                    TrackingDataContext.PtGrpNameMsts.Add(new PtGrpNameMst()
                    {
                        HpId = hpId,
                        GrpId = item.GrpId,
                        SortNo = item.SortNo,
                        GrpName = item.GrpName,
                        IsDeleted = DeleteTypes.None,
                        CreateDate = DateTime.UtcNow,
                        CreateId = userId,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId
                    });
                    TrackingDataContext.PtGrpItems.AddRange(Mapper.Map<GroupItemModel, PtGrpItem>(item.GroupItems, (src, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.HpId = hpId;
                        dest.CreateDate = DateTime.UtcNow;
                        dest.UpdateDate = DateTime.UtcNow;
                        dest.UpdateId = userId;
                        return dest;
                    }));
                }
                else
                {
                    itemAct.GrpName = item.GrpName;
                    itemAct.SortNo = item.SortNo;
                    itemAct.UpdateDate = DateTime.UtcNow;
                    itemAct.UpdateId = userId;

                    var itemInDatabases = TrackingDataContext.PtGrpItems
                                    .Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId
                                        && x.GrpId == item.GrpId);

                    foreach (var itemGrp in item.GroupItems)
                    {
                        var itemGrpAct = itemInDatabases.FirstOrDefault(x => x.GrpCode == itemGrp.GrpCode);
                        if(itemGrpAct is null)
                        {
                            TrackingDataContext.PtGrpItems.Add(new PtGrpItem()
                            {
                                GrpId = itemGrp.GrpId,
                                GrpCode = itemGrp.GrpCode,
                                GrpCodeName = itemGrp.GrpCodeName,
                                CreateId = userId,
                                HpId = hpId,
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                                UpdateId = userId,
                            });
                        }
                        else
                        {
                            itemGrpAct.GrpCode = itemGrp.GrpCode;
                            itemGrpAct.GrpCodeName = itemGrp.GrpCodeName;
                            itemGrpAct.UpdateDate = DateTime.UtcNow;
                            itemGrpAct.UpdateId = userId;
                            itemGrpAct.IsDeleted = itemGrp.IsDeleted;
                        }
                    }
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }
    }
}
