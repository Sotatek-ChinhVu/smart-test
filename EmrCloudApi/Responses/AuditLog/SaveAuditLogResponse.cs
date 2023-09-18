namespace EmrCloudApi.Responses.AuditLog;

public class SaveAuditLogResponse
{
    public SaveAuditLogResponse(bool value)
    {
        Value = value;
    }

    public bool Value { get; private set; }
}
