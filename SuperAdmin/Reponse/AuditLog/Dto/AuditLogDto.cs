using Domain.SuperAdminModels.Logger;

namespace SuperAdminAPI.Reponse.AuditLog.Dto;

public class AuditLogDto
{
    public AuditLogDto(AuditLogModel model)
    {
        LogId = model.LogId;
        TenantId = model.TenantId;
        Domain = model.Domain;
        ThreadId = model.ThreadId;
        LogType = model.LogType;
        HpId = model.HpId;
        UserId = model.UserId;
        LoginKey = model.LoginKey;
        DepartmentId = model.DepartmentId;
        LogDate = model.LogDate;
        EventCd = model.EventCd;
        PtId = model.PtId;
        SinDay = model.SinDay;
        RaiinNo = model.RaiinNo;
        Path = model.Path;
        RequestInfo = model.RequestInfo;
        ClientIP = model.ClientIP;
        Desciption = model.Desciption;
    }

    public long LogId { get; private set; }

    public int TenantId { get; private set; }

    public string Domain { get; private set; }

    public string ThreadId { get; private set; }

    public string LogType { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string LoginKey { get; private set; }

    public int DepartmentId { get; private set; }

    public DateTime LogDate { get; private set; }

    public string EventCd { get; private set; }

    public long PtId { get; private set; }

    public int SinDay { get; private set; }

    public long RaiinNo { get; private set; }

    public string Path { get; private set; }

    public string RequestInfo { get; private set; }

    public string ClientIP { get; private set; }

    public string Desciption { get; private set; }
}
