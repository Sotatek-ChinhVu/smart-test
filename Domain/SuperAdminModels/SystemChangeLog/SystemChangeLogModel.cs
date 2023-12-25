namespace Domain.SuperAdminModels.SystemChangeLog;

public class SystemChangeLogModel
{
    public SystemChangeLogModel(string fileName, string version, int isPg, int isDb, int isMaster, int isRun, int isNote, int isDrugPhoto, int status, string errMessage)
    {
        FileName = fileName;
        Version = version;
        IsPg = isPg;
        IsDb = isDb;
        IsMaster = isMaster;
        IsRun = isRun;
        IsNote = isNote;
        IsDrugPhoto = isDrugPhoto;
        Status = status;
        ErrMessage = errMessage;
    }

    public string FileName { get; private set; }

    public string Version { get; private set; }

    public int IsPg { get; private set; }

    public int IsDb { get; private set; }

    public int IsMaster { get; private set; }

    public int IsRun { get; private set; }

    public int IsNote { get; private set; }

    public int IsDrugPhoto { get; private set; }

    public int Status { get; private set; }

    public string ErrMessage { get; private set; }
}
