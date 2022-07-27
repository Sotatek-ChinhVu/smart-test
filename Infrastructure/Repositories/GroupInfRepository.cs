using Domain.Models.GroupInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupInfRepository: IGroupInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public GroupInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId)
        {
            var grs = _tenantDataContext.PtGrpNameMsts.Where(x => x.IsDeleted == 0).ToList();
            var listItem = _tenantDataContext.PtGrpItems.Where(x => x.IsDeleted == 0)
                        .Select(x => new PtGrpItemModel(
                            x.HpId,
                            x.GrpId,
                            x.GrpCode,
                            x.SeqNo,
                            x.GrpCodeName,
                            x.SortNo
                            ))
                .ToList();
            listItem.Add(new PtGrpItemModel(0, 0, "", 0, "", 0));
            var ptGroupList = _tenantDataContext.PtGrpInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).ToList();
            // Create a inner join with GroupName and PtGroupInf via GroupId
            var grMstJoinGrpLst = from gr in grs
                                  join ptGr in ptGroupList on gr.GrpId equals ptGr.GroupId
                                  select new
                                  {
                                      GrpId = gr.GrpId,
                                      GrpCode = ptGr.GroupCode,
                                      HpId = gr.HpId,
                                      PtId = ptGr.PtId
                                  };
            //Then inner join with GroupItem via GroupCode & GroupId
            var joinData2 = from data in grMstJoinGrpLst
                            join item in listItem on new { data.GrpId, data.GrpCode } equals new { item.GrpId, item.GrpCode }
                            select new
                            {
                                GroupId = data.GrpId,
                                GroupCode = data.GrpCode,
                                GroupItem = item,
                                PtId = data.PtId
                            };
            //finally, create GroupJoin with GroupName and their result via GroupId to get GroupItemList, GroupItem selected

            var rs = grs.GroupJoin(joinData2,
                    gr => gr.GrpId,
                    t => t.GroupId,
                    (gr, t) => new
                    {
                        GroupName = gr,
                        GroupCodes = t
                    }
                    ).SelectMany(xy => xy.GroupCodes.DefaultIfEmpty(),
                   (g, t) => new
                   {
                       GroupName = new PtGrpNameMstModel(g.GroupName.HpId, g.GroupName.GrpId, g.GroupName.SortNo, g.GroupName.GrpName),
                       GroupItem = t,
                       PtId = ptId
                   }
                   ).Select(gt => new GroupInfModel(gt.GroupName, gt.GroupItem == null ? null : gt.GroupItem.GroupItem)
                   {
                       GroupCode = gt.GroupItem != null ? gt.GroupItem.GroupCode : string.Empty,
                       PtId = gt.PtId,
                       ListItem = listItem.Where(gi => gi.GrpId == gt.GroupName.GrpId || gi.GrpId == 0).OrderBy(gi => gi.SortNo).ToList()
                   }).OrderBy(gt => gt.SortNo).ToList();
            return rs;
        }
    }
}
