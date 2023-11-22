using Domain.SuperAdminModels.Tenant;

namespace SuperAdminAPI.Reponse.Tenant.Dto;

public class TenantDto
{
    public TenantDto(TenantModel model)
    {
        TenantId = model.TenantId;
        Hospital = model.Hospital;
        Status = model.Status;
        AdminId = model.AdminId;
        Password = model.Password;
        SubDomain = model.SubDomain;
        Db = model.Db;
        Size = model.Size;
        Type = model.Type;
        EndPointDb = model.EndPointDb;
        EndSubDomain = model.EndSubDomain;
        Action = model.Action;
        ScheduleDate = model.ScheduleDate;
        ScheduleTime = model.ScheduleTime;
        CreateDate = model.CreateDate;
        RdsIdentifier = model.RdsIdentifier;
        StorageFull = model.StorageFull;
    }

    public int TenantId { get; private set; }

    public string Hospital { get; private set; }

    public byte Status { get; private set; }

    public int AdminId { get; private set; }

    public string Password { get; private set; }

    public string SubDomain { get; private set; }

    public string Db { get; private set; }

    public int Size { get; private set; }

    public byte Type { get; private set; }

    public string EndPointDb { get; private set; }

    public string EndSubDomain { get; private set; }

    public int Action { get; private set; }

    public int ScheduleDate { get; private set; }

    public int ScheduleTime { get; private set; }

    public DateTime CreateDate { get; private set; }

    public string RdsIdentifier { get; private set; }

    public double StorageFull { get; private set; }
}
