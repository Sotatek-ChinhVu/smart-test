﻿using Domain.Models.MonshinInf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class MonshinInforRepository : RepositoryBase, IMonshinInforRepository
    {
        public MonshinInforRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId, int sinDate, bool isDeleted)
        {
            var monshinList = NoTrackingDataContext.MonshinInfo
                .Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate <= sinDate && (isDeleted || x.IsDeleted == 0))
                .OrderByDescending(x => x.SinDate)
                .ThenByDescending(x => x.RaiinNo)
                .Select(x => new MonshinInforModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.SinDate,
                x.Text ?? string.Empty,
                x.Rtext ?? string.Empty,
                x.GetKbn,
                x.IsDeleted))
                .ToList();
            return monshinList;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public bool SaveList(List<MonshinInforModel> monshinInforModels, int userId)
        {
            try
            {
                foreach (var model in monshinInforModels)
                {
                    var monshinInfor = NoTrackingDataContext.MonshinInfo.
                    FirstOrDefault(x => x.HpId == model.HpId
                        && x.PtId == model.PtId
                        && x.RaiinNo == model.RaiinNo
                        && x.SinDate == model.SinDate
                        && x.IsDeleted == 0
                        && x.GetKbn == 0);

                    //Update monshin when text change
                    if (monshinInfor != null && !string.IsNullOrEmpty(model.Text.Trim()))
                    {
                        TrackingDataContext.MonshinInfo.Update(new MonshinInfo()
                        {
                            HpId = monshinInfor.HpId,
                            PtId = monshinInfor.PtId,
                            RaiinNo = monshinInfor.RaiinNo,
                            SeqNo = monshinInfor.SeqNo,
                            SinDate = monshinInfor.SinDate,
                            Text = model.Text,
                            Rtext = monshinInfor.Rtext,
                            GetKbn = 0,
                            IsDeleted = monshinInfor.IsDeleted,
                            CreateId = monshinInfor.CreateId,
                            CreateDate = DateTime.SpecifyKind(monshinInfor.CreateDate, DateTimeKind.Utc),
                            CreateMachine = monshinInfor.CreateMachine,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }

                    //Delete Monshin when text is empty
                    else if (monshinInfor != null && string.IsNullOrEmpty(model.Text.Trim()))
                    {
                        TrackingDataContext.MonshinInfo.Update(new MonshinInfo()
                        {
                            HpId = monshinInfor.HpId,
                            PtId = monshinInfor.PtId,
                            RaiinNo = monshinInfor.RaiinNo,
                            SeqNo = monshinInfor.SeqNo,
                            SinDate = monshinInfor.SinDate,
                            Text = monshinInfor.Text,
                            Rtext = monshinInfor.Rtext,
                            GetKbn = 0,
                            IsDeleted = 1,
                            CreateId = monshinInfor.CreateId,
                            CreateDate = DateTime.SpecifyKind(monshinInfor.CreateDate, DateTimeKind.Utc),
                            CreateMachine = monshinInfor.CreateMachine,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }

                    //Insert monshin when not found in monshininf
                    else if (monshinInfor == null && !string.IsNullOrEmpty(model.Text.Trim()))
                    {
                        TrackingDataContext.MonshinInfo.Add(new MonshinInfo()
                        {
                            HpId = model.HpId,
                            PtId = model.PtId,
                            RaiinNo = model.RaiinNo,
                            SinDate = model.SinDate,
                            Text = model.Text,
                            GetKbn = 0,
                            IsDeleted = 0,
                            CreateId = userId,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }
                }
                TrackingDataContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
