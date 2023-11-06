﻿using Domain.Models.PatientGroupMst;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class PatientGroupMstRepository : RepositoryBase, IPatientGroupMstRepository
    {
        public PatientGroupMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<PatientGroupMstModel> GetAll()
        {
            var groupMstList = NoTrackingDataContext.PtGrpNameMsts.Where(p => p.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
            var groupDetailList = NoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();

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
                    groupDetailList.Select(g => new PatientGroupDetailModel(g.GrpId, g.GrpCode, g.SeqNo, g.SortNo, g.GrpCodeName ?? string.Empty)).ToList()
                );
        }

        public bool SaveListPatientGroup(int hpId, int userId, List<PatientGroupMstModel> patientGroupMstModels)
        {
            bool status = false;
            try
            {
                // list get in datatacontext
                var groupMstLists = TrackingDataContext.PtGrpNameMsts.Where(mst => mst.IsDeleted == 0 && mst.HpId == hpId).ToList();
                var groupDetailLists = TrackingDataContext.PtGrpItems.Where(detail => detail.IsDeleted == 0 && detail.HpId == hpId).ToList();

                List<long> listDetailSeqNoModel = new();
                List<PtGrpNameMst> listAddNewGroupMsts = new();
                List<PtGrpItem> listAddNewGroupItemMsts = new();

                foreach (var model in patientGroupMstModels)
                {
                    var groupMst = groupMstLists.FirstOrDefault(x => x.GrpId == model.GroupId);
                    if (groupMst != null)
                    {
                        groupMst.GrpName = model.GroupName;
                        int sortNo = 1;
                        foreach (var detail in model.Details)
                        {
                            var groupDetail = groupDetailLists.FirstOrDefault(x => x.SeqNo == detail.SeqNo && x.GrpId == model.GroupId);
                            if (groupDetail != null)
                            {
                                groupDetail.GrpCode = detail.GroupCode;
                                groupDetail.GrpCodeName = detail.GroupDetailName;
                                groupDetail.SortNo = sortNo;
                                groupDetail.UpdateId = userId;
                                groupDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            }
                            else
                            {
                                listAddNewGroupItemMsts.Add(ConvertToPtGrpItem(hpId, userId, sortNo, 0, new PtGrpItem(), detail, true));
                            }
                            sortNo++;
                        }
                    }
                    else
                    {
                        listAddNewGroupMsts.Add(ConvertToPtGrpNameMst(hpId, userId, 0, new PtGrpNameMst(), model, true));
                        int sortNo = 1;
                        foreach (var detail in model.Details)
                        {
                            listAddNewGroupItemMsts.Add(ConvertToPtGrpItem(hpId, userId, sortNo, 0, new PtGrpItem(), detail, true));
                            sortNo++;
                        }
                    }
                    listDetailSeqNoModel.AddRange(model.Details.Select(x => x.SeqNo).ToList());
                }

                TrackingDataContext.PtGrpNameMsts.AddRange(listAddNewGroupMsts);
                TrackingDataContext.PtGrpItems.AddRange(listAddNewGroupItemMsts);

                // delete Group
                var listGroupDeletes = groupMstLists.Where(mst => !patientGroupMstModels.Select(x => x.GroupId).ToList().Contains(mst.GrpId)).ToList();
                if (listGroupDeletes != null && listGroupDeletes.Count > 0)
                {
                    foreach (var item in listGroupDeletes)
                    {
                        item.IsDeleted = 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }

                // delete Detail
                var listDetailDeletes = groupDetailLists.Where(mst => !listDetailSeqNoModel.Contains(mst.SeqNo)).ToList();
                if (listDetailDeletes != null && listDetailDeletes.Count > 0)
                {
                    foreach (var item in listDetailDeletes)
                    {
                        item.IsDeleted = 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }

                TrackingDataContext.SaveChanges();
                status = true;
                return status;
            }
            catch (Exception)
            {
                throw;
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
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.CreateId = userId;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            return entity;
        }

        private PtGrpItem ConvertToPtGrpItem(int hpId, int userId, int sortNo, int isDelete, PtGrpItem entity, PatientGroupDetailModel model, bool? isAddNew = false)
        {
            entity.HpId = hpId;
            entity.GrpId = model.GroupID;
            entity.GrpCode = model.GroupCode;
            entity.SeqNo = model.SeqNo;
            entity.GrpCodeName = model.GroupDetailName;
            entity.SortNo = sortNo;
            entity.IsDeleted = isDelete;
            if (isAddNew == true)
            {
                entity.SeqNo = 0;
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.CreateId = userId;
            }
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            return entity;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
