using Domain.Models.MonshinInf;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Repositories
{
    public class MonshinInforRepository : IMonshinInforRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public MonshinInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId)
        {
            var monshinList = _tenantDataContextNoTracking.MonshinInfo
                .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .OrderByDescending(x => x.SinDate)
                .ThenByDescending(x => x.RaiinNo)
                .Select(x => new MonshinInforModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.SinDate,
                x.Text))
                .ToList();
            return monshinList;
        }

        public bool SaveList(List<MonshinInforModel> monshinInforModels)
        {
            var executionStrategy = _tenantDataContextTracking.Database.CreateExecutionStrategy();

            var result = executionStrategy.Execute(
                () =>
                {
                    using (var transaction = _tenantDataContextTracking.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var model in monshinInforModels)
                            {
                                var monshinInfor = _tenantDataContextNoTracking.MonshinInfo.
                                Where(x => x.HpId == model.HpId
                                    && x.PtId == model.PtId
                                    && x.RaiinNo == model.RaiinNo
                                    && x.SinDate == model.SinDate
                                    && x.IsDeleted == 0).FirstOrDefault();

                                if (monshinInfor != null && !string.IsNullOrEmpty(model.Text.Trim()))
                                {
                                    _tenantDataContextTracking.MonshinInfo.Update(new MonshinInfo()
                                    {
                                        HpId = monshinInfor.HpId,
                                        PtId = monshinInfor.PtId,
                                        RaiinNo = monshinInfor.RaiinNo,
                                        SeqNo = monshinInfor.SeqNo,
                                        SinDate = monshinInfor.SinDate,
                                        Text = model.Text,
                                        Rtext = monshinInfor.Rtext,
                                        GetKbn = monshinInfor.GetKbn,
                                        IsDeleted = monshinInfor.IsDeleted,
                                        CreateId = monshinInfor.CreateId,
                                        CreateDate = DateTime.SpecifyKind(monshinInfor.CreateDate, DateTimeKind.Utc),
                                        CreateMachine = monshinInfor.CreateMachine,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName
                                    });
                                }else if (monshinInfor != null && string.IsNullOrEmpty(model.Text.Trim()))
                                {
                                    _tenantDataContextTracking.MonshinInfo.Update(new MonshinInfo()
                                    {
                                        HpId = monshinInfor.HpId,
                                        PtId = monshinInfor.PtId,
                                        RaiinNo = monshinInfor.RaiinNo,
                                        SeqNo = monshinInfor.SeqNo,
                                        SinDate = monshinInfor.SinDate,
                                        Text = monshinInfor.Text,
                                        Rtext = monshinInfor.Rtext,
                                        GetKbn = monshinInfor.GetKbn,
                                        IsDeleted = 1,
                                        CreateId = monshinInfor.CreateId,
                                        CreateDate = DateTime.SpecifyKind(monshinInfor.CreateDate, DateTimeKind.Utc),
                                        CreateMachine = monshinInfor.CreateMachine,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName
                                    });
                                }
                                else if(monshinInfor == null && !string.IsNullOrEmpty(model.Text.Trim()))
                                {
                                    _tenantDataContextTracking.MonshinInfo.Add(new MonshinInfo()
                                    {
                                        HpId = model.HpId,
                                        PtId = model.PtId,
                                        RaiinNo = model.RaiinNo,
                                        SinDate = model.SinDate,
                                        Text = model.Text,
                                        GetKbn = 0,
                                        IsDeleted = 0,
                                        CreateId = TempIdentity.UserId,
                                        CreateDate = DateTime.UtcNow,
                                        CreateMachine = TempIdentity.ComputerName,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName
                                    });
                                }
                            }
                            _tenantDataContextTracking.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                });
            return result;
        }
    }
}
