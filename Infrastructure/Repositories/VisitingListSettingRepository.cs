using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class VisitingListSettingRepository : IVisitingListSettingRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public VisitingListSettingRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public void Save(List<UserConfModel> userConfModels, List<SystemConfModel> systemConfModels)
    {
        ModifyUserConfs(userConfModels);
        ModifySystemConfs(systemConfModels);
        _tenantDataContext.SaveChanges();
    }

    private void ModifyUserConfs(List<UserConfModel> confModels)
    {
        if (confModels.Count == 0)
        {
            return;
        }

        var userId = confModels[0].UserId;
        var existingConfigs = _tenantDataContext.UserConfs
            .Where(u => u.UserId == userId
                && u.GrpCd >= UserConfCommon.GroupCodes.Font
                && u.GrpCd <= UserConfCommon.GroupCodes.SelectTodoSetting).ToList();
        var configsToInsert = new List<UserConf>();

        foreach (var model in confModels)
        {
            var configToUpdate = existingConfigs.FirstOrDefault(c => c.GrpCd == model.GrpCd);
            if (configToUpdate is not null)
            {
                if (configToUpdate.Val != model.Val || configToUpdate.Param != model.Param)
                {
                    configToUpdate.Val = model.Val;
                    configToUpdate.Param = model.Param;
                    configToUpdate.UpdateDate = DateTime.UtcNow;
                    configToUpdate.UpdateId = TempIdentity.UserId;
                    configToUpdate.UpdateMachine = TempIdentity.ComputerName;
                }
            }
            else
            {
                configsToInsert.Add(new UserConf
                {
                    HpId = TempIdentity.HpId,
                    UserId = model.UserId,
                    GrpCd = model.GrpCd,
                    GrpItemCd = model.GrpItemCd,
                    GrpItemEdaNo = model.GrpItemEdaNo,
                    Val = model.Val,
                    Param = model.Param,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                });
            }
        }

        _tenantDataContext.UserConfs.AddRange(configsToInsert);
    }

    private void ModifySystemConfs(List<SystemConfModel> confModels)
    {
        var existingConfigs = _tenantDataContext.SystemConfs
            .Where(s => s.GrpCd == SystemConfGroupCodes.ReceptionTimeColor
                || s.GrpCd == SystemConfGroupCodes.ReceptionStatusColor).ToList();
        var configsToInsert = new List<SystemConf>();

        foreach (var model in confModels)
        {
            var configToUpdate = existingConfigs.FirstOrDefault(c => c.GrpCd == model.GrpCd && c.GrpEdaNo == model.GrpEdaNo);
            if (configToUpdate is not null)
            {
                if (configToUpdate.Param != model.Param)
                {
                    configToUpdate.Param = model.Param;
                    configToUpdate.UpdateDate = DateTime.UtcNow;
                    configToUpdate.UpdateId = TempIdentity.UserId;
                    configToUpdate.UpdateMachine = TempIdentity.ComputerName;
                }
            }
            else
            {
                configsToInsert.Add(new SystemConf
                {
                    HpId = TempIdentity.HpId,
                    GrpCd = model.GrpCd,
                    GrpEdaNo = model.GrpEdaNo,
                    Val = model.Val,
                    Param = model.Param,
                    Biko = model.Biko,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                });
            }
        }

        _tenantDataContext.SystemConfs.AddRange(configsToInsert);

        var configsToDelete = existingConfigs.Where(c => !confModels.Exists(m => m.GrpCd == c.GrpCd && m.GrpEdaNo == c.GrpEdaNo));
        _tenantDataContext.SystemConfs.RemoveRange(configsToDelete);
    }
}
