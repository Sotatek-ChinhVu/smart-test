using Domain.Models.PatientGroupMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PatientGroupMstRepository : IPatientGroupMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantDataContext;
        public PatientGroupMstRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<PatientGroupMstModel> GetAll()
        {
            var groupMstList = _tenantNoTrackingDataContext.PtGrpNameMsts.Where(p => p.IsDeleted == 0).ToList();
            var groupDetailList = _tenantNoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == 0).ToList();

            List<PatientGroupMstModel> result = new List<PatientGroupMstModel>();
            foreach (var groupMst in groupMstList)
            {
                int groupId = groupMst.GrpId;
                var groupDetailListByGroupId = groupDetailList.Where(p => p.GrpId == groupId).ToList();

                result.Add(ConvertToModel(groupMst, groupDetailListByGroupId));
            }

            return result;
        }

        private PatientGroupMstModel ConvertToModel(PtGrpNameMst groupMst, List<PtGrpItem> groupDetailList)
        {
            return new PatientGroupMstModel
                (
                    groupMst.GrpId,
                    groupMst.SortNo,
                    groupMst.GrpName ?? string.Empty,
                    groupDetailList.Select(g => new PatientGroupDetailModel(g.GrpId, g.GrpCode, g.SeqNo, g.SortNo, g.GrpCodeName)).ToList()
                );
        }

        public bool SaveListPatientGroup(int hpId, int userId, List<PatientGroupMstModel> patientGroupMstModels)
        {
            bool status = false;
            try
            {
                // list PatientGroupDetailModel
                List<PatientGroupDetailModel> listPatientGroupDetailModels = new();
                foreach (var model in patientGroupMstModels)
                {
                    listPatientGroupDetailModels.AddRange(model.Details);
                }

                // list get in datatacontext
                var groupMstLists = _tenantDataContext.PtGrpNameMsts.Where(mst => mst.IsDeleted == 0).ToList();
                var groupDetailLists = _tenantDataContext.PtGrpItems.Where(detail => detail.IsDeleted == 0).ToList();
                var listGroupMstIds = groupMstLists.Select(mst => mst.GrpId).ToList();

                // Add new PtGrpNameMst
                var listGroupModelAddNews = patientGroupMstModels.Where(model => !listGroupMstIds.Contains(model.GroupId))
                        .Select(model => ConvertToPtGrpNameMst(hpId, userId, 0, new PtGrpNameMst(), model, true)).ToList();
                if (listGroupModelAddNews != null && listGroupModelAddNews.Count > 0)
                {
                    _tenantDataContext.PtGrpNameMsts.AddRange(listGroupModelAddNews);
                }

                // Add new PtGrpItem
                var listGroupDetailMstAddNews = listPatientGroupDetailModels.Where(model => model.SeqNo == 0)
                        .Select(model => ConvertToPtGrpItem(hpId, userId, 0, new PtGrpItem(), model, true)).ToList();
                if (listGroupModelAddNews != null && listGroupModelAddNews.Count > 0)
                {
                    _tenantDataContext.PtGrpItems.AddRange(listGroupDetailMstAddNews);
                }

                // Update PtGrpNameMst
                foreach (var mst in groupMstLists)
                {
                    var model = patientGroupMstModels.FirstOrDefault(model => model.GroupId == mst.GrpId);
                    if (model != null)
                    {
                        mst.GrpName = model.GroupName;
                    }
                    else
                    {
                        mst.IsDeleted = 1;
                    }
                    mst.UpdateId = userId;
                    mst.UpdateDate = DateTime.UtcNow;
                }

                // Update PtGrpItems 
                int sortNo = 1;
                foreach (var mst in groupDetailLists)
                {
                    var model = listPatientGroupDetailModels.FirstOrDefault(model => model.GroupID == mst.GrpId && model.SeqNo == mst.SeqNo);
                    if (model != null)
                    {
                        mst.GrpCode = model.GroupCode;
                        mst.GrpCodeName = model.GroupDetailName;
                        mst.SortNo = sortNo;
                        sortNo++;
                    }
                    else
                    {
                        mst.IsDeleted = 1;
                    }
                    mst.UpdateId = userId;
                    mst.UpdateDate = DateTime.UtcNow;
                }

                _tenantDataContext.SaveChanges();
                status = true;
                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }

        private PtGrpNameMst ConvertToPtGrpNameMst(int hpId, int userId, int isDelete, PtGrpNameMst entity, PatientGroupMstModel model, bool? isAddNew = false)
        {
            entity.HpId = hpId;
            entity.GrpId = model.GroupId;
            entity.SortNo = model.SortNo;
            entity.GrpName = model.GroupName;
            entity.IsDeleted = isDelete;
            if (isAddNew == true)
            {
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateId = userId;
            }
            entity.UpdateDate = DateTime.UtcNow;
            entity.CreateId = userId;
            return entity;
        }

        private PtGrpItem ConvertToPtGrpItem(int hpId, int userId, int isDelete, PtGrpItem entity, PatientGroupDetailModel model, bool? isAddNew = false)
        {
            entity.HpId = hpId;
            entity.GrpId = model.GroupID;
            entity.GrpCode = model.GroupCode;
            entity.SeqNo = model.SeqNo;
            entity.GrpCodeName = model.GroupDetailName;
            entity.SortNo = model.SortNo;
            entity.IsDeleted = isDelete;
            if (isAddNew == true)
            {
                entity.SeqNo = 0;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateId = userId;
            }
            entity.UpdateDate = DateTime.UtcNow;
            entity.CreateId = userId;
            return entity;
        }
    }
}
