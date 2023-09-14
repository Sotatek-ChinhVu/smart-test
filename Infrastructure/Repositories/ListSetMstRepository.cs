using Domain.Models.ListSetMst;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ListSetMstRepository : RepositoryBase, IListSetMstRepository
    {
        public ListSetMstRepository(ITenantProvider _tenantProvider) : base(_tenantProvider)
        {
        }
        public int GetGenerationId(int sinDate)
        {
            ListSetGenerationMst? generation = NoTrackingDataContext.ListSetGenerationMsts.Where
                            (item => item.StartDate <= sinDate)
                            .OrderByDescending(item => item.StartDate)
                            .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }
        public List<ListSetMstModel> GetListSetMst(int hpId, int setKbn, int generationId)
        {
            var list = NoTrackingDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0
                                                        && x.SetKbn == setKbn);

            return (from item in list
                    select new ListSetMstModel(item.HpId,
                                               item.GenerationId,
                                               item.SetId,
                                               item.SetName ?? string.Empty,
                                               item.ItemCd ?? string.Empty,
                                               item.IsTitle,
                                               item.SetKbn,
                                               item.SelectType,
                                               item.Suryo,
                                               item.Level1,
                                               item.Level2,
                                               item.Level3,
                                               item.Level4,
                                               item.Level5,
                                               item.CmtName ?? string.Empty,
                                               item.CmtOpt ?? string.Empty)).ToList();
        }

        public bool UpdateTreeListSetMst(int userId, int hpId, List<ListSetMstUpdateModel> listData)
        {
            foreach (var item in listData)
            {
                // Create
                if (item.SetId == 0)
                {
                    var listSetMst = TrackingDataContext.ListSetMsts.FirstOrDefault(x => x.HpId == item.HpId && x.GenerationId == item.GenerationId
                    && x.SetId == item.SetId && x.SetKbn == item.SetKbn && x.Level1 == item.Level1 && x.Level2 == item.Level2 && x.Level3 == item.Level3 
                    && x.Level4 == item.Level4 && x.Level5 == item.Level5);
                    if(listSetMst != null)
                    {
                        return false;
                    }
                    TrackingDataContext.ListSetMsts.Add(new ListSetMst()
                    {
                        CreateId = userId,
                        UpdateId = userId,
                        HpId = hpId,
                        GenerationId = item.GenerationId,
                        SetName = item.SetName,
                        ItemCd = item.ItemCd,
                        CreateMachine = CIUtil.GetComputerName(),
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        Level1 = item.Level1,
                        Level2 = item.Level2,
                        Level3 = item.Level3,
                        Level4 = item.Level4,
                        Level5 = item.Level5,
                        SetKbn = item.SetKbn,
                        IsDeleted = 0,
                        IsTitle = item.IsTitle,

                    });
                }

                // Update
                else
                {
                    var listSetMst = TrackingDataContext.ListSetMsts.FirstOrDefault(x => x.HpId == item.HpId && x.GenerationId == item.GenerationId
                    && x.SetId == item.SetId && x.SetKbn == item.SetKbn);
                    if(listSetMst == null)
                    {
                        return false;
                    }    
                    listSetMst.UpdateId = userId;
                    listSetMst.SetName = item.SetName;
                    listSetMst.ItemCd = item.ItemCd;
                    listSetMst.Level1 = item.Level1;
                    listSetMst.Level2 = item.Level2;
                    listSetMst.Level3 = item.Level3;
                    listSetMst.Level4 = item.Level4;
                    listSetMst.Level5 = item.Level5;
                    listSetMst.IsDeleted = item.IsDeleted;
                    listSetMst.IsTitle = item.IsTitle;
                    listSetMst.UpdateMachine = CIUtil.GetComputerName();
                    listSetMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
