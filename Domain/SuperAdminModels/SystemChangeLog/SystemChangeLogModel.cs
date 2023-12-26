namespace Domain.SuperAdminModels.SystemChangeLog;

public class SystemChangeLogModel
{
    public SystemChangeLogModel(int id, string fileName, string version, int isPg, int isDb, int isMaster, int isRun, int isNote, int isDrugPhoto, int status, string errMessage)
    {
        Id = id;
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

    public SystemChangeLogModel(string fileName, string version)
    {
        FileName = fileName;
        Version = version;
        Status = 1;
        ErrMessage = string.Empty;
    }

    public SystemChangeLogModel()
    {
        FileName = string.Empty;
        Version = string.Empty;
        Status = 1;
        ErrMessage = string.Empty;
    }

    /// <summary>
    /// Update IsDrug
    /// </summary>
    /// <param name="isDrugPhoto"></param>
    /// <returns></returns>
    public SystemChangeLogModel UpdateIsDrug(int isDrugPhoto)
    {
        IsDrugPhoto = isDrugPhoto;
        return this;
    }

    /// <summary>
    /// Update IsNote
    /// </summary>
    /// <param name="isNote"></param>
    /// <returns></returns>
    public SystemChangeLogModel UpdateIsNote(int isNote)
    {
        IsNote = isNote;
        return this;
    }

    /// <summary>
    /// Update Status
    /// </summary>
    /// <param name="status"></param>
    /// <param name="errMessage"></param>
    /// <returns></returns>
    public SystemChangeLogModel UpdateStatus(int status, string errMessage)
    {
        Status = status;
        ErrMessage = errMessage;
        return this;
    }

    public int Id { get; private set; }

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
