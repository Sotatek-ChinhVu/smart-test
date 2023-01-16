using Domain.Models.PtGroupMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class GroupNameMstRepository : RepositoryBase, IGroupNameMstRepository
    {
        public GroupNameMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        private readonly TenantDataContext _tenantNoTrackingDataContext;

        public void ReleaseResource()
        {
            DisposeDataContext();
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<GroupNameMstModel> GetListGroupNameMst(int hpId)
        {
            List<GroupNameMstModel> result = new List<GroupNameMstModel>();
            var grpNameDatabases = _tenantNoTrackingDataContext.PtGrpNameMsts.Where(x => x.HpId == hpId).ToList();
            foreach (var item in grpNameDatabases)
            {
                if(item.IsDeleted == DeleteTypes.Deleted)
                {
                    //Not need include GrpItems
                    result.Add(new GroupNameMstModel(item.GrpId, item.SortNo, item.GrpName ?? string.Empty, item.IsDeleted, new List<GroupItemModel>()));
                }
                else
                {
                    var grpItems = _tenantNoTrackingDataContext.PtGrpItems.Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId
                                                                        && x.GrpId == item.GrpId)
                                                                .OrderBy(x => x.SortNo)
                                                                .Select(x => new GroupItemModel(
                                                                            x.GrpId,
                                                                            x.GrpCode,
                                                                            x.SeqNo, 
                                                                            x.GrpCodeName ?? string.Empty, 
                                                                            x.SortNo,
                                                                            x.IsDeleted)
                                                                ).ToList();

                    result.Add(new GroupNameMstModel(item.GrpId, item.SortNo, item.GrpName ?? string.Empty, item.IsDeleted, grpItems));
                }
            }
            return result;
        }

        /// <summary>
        /// FE logic
        /// GrpName : filter & only pass record want save to db
        /// GrpIdtem : when delete in screen Remove real 
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
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;

                var grpItemDeletes = TrackingDataContext.PtGrpItems.Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId
                                        && x.GrpId == item.GrpId);

                foreach(var itemGrp in grpItemDeletes)
                {
                    itemGrp.IsDeleted = DeleteTypes.Deleted;
                    itemGrp.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    itemGrp.UpdateId = userId;
                }
            }

            foreach(var item in groupNameMsts) //Add or Update
            {
                if (item.IsDeleted == DeleteTypes.Deleted)
                    continue;

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
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId
                    });
                    TrackingDataContext.PtGrpItems.AddRange(Mapper.Map<GroupItemModel, PtGrpItem>(item.GroupItems, (src, dest) =>
                    {
                        dest.CreateId = userId;
                        dest.HpId = hpId;
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateId = userId;
                        return dest;
                    }));
                }
                else
                {
                    itemAct.GrpName = item.GrpName;
                    itemAct.SortNo = item.SortNo;
                    itemAct.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    itemAct.UpdateId = userId;

                    var itemInDatabases = TrackingDataContext.PtGrpItems
                                    .Where(x => x.IsDeleted == DeleteTypes.None && x.HpId == hpId
                                        && x.GrpId == item.GrpId).ToList();

                    var itemRemoves = itemInDatabases.Where(x => !item.GroupItems.Any(o => o.GrpCode == x.GrpCode));
                    foreach(var itemRemove in itemRemoves)
                    {
                        itemRemove.IsDeleted = DeleteTypes.Deleted;
                        itemRemove.UpdateDate = DateTime.UtcNow;
                        itemRemove.UpdateId = userId;
                    }

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
                                SortNo = itemGrp.SortNo,
                                CreateId = userId,
                                HpId = hpId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateId = userId
                            });
                        }
                        else
                        {
                            itemGrpAct.GrpCodeName = itemGrp.GrpCodeName;
                            itemGrpAct.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            itemGrpAct.UpdateId = userId;
                        }
                    }
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }
    }
}
