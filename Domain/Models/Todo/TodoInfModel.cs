namespace Domain.Models.Todo;

public class TodoInfModel
{
    public TodoInfModel(int todoNo, int todoEdaNo, long ptId, int sinDate, long raiinNo, int todoKbnNo, int todoGrpNo, int tanto, int term, string cmt1, string cmt2, int isDone, int isDeleted)
    {
        TodoNo = todoNo;
        TodoEdaNo = todoEdaNo;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        TodoKbnNo = todoKbnNo;
        TodoGrpNo = todoGrpNo;
        Tanto = tanto;
        Term = term;
        Cmt1 = cmt1;
        Cmt2 = cmt2;
        IsDone = isDone;
        IsDeleted = isDeleted;
        PatientName = string.Empty;
        PrimaryDoctorName = string.Empty;
        KaSname = string.Empty;
        TodoKbnName = string.Empty;
        TantoName = string.Empty;
        UpdaterName = string.Empty;
        CreaterName = string.Empty;
        TodoGrpName = string.Empty;
        Houbetu = string.Empty;
        HokensyaNo = string.Empty;
        Sex = string.Empty;
        GroupColor = string.Empty;
    }

    public TodoInfModel(int hpId, long ptId, long ptNum, string patientName, int sinDate, string primaryDoctorName, string kaSname, string todoKbnName, string cmt1, DateTime createDate, string createrName, string tantoName, string cmt2, DateTime updateDate, string updaterName, string todoGrpName, int term, int hokenPid, string houbetu, int hokenKbn, string hokensyaNo, int hokenId, int tantoId, int todoNo, int todoEdaNo, long raiinNo, int todoKbnNo, int todoGrpNo, int isDone, int status, int gender, string groupColor, int createId)
    {
        HpId = hpId;
        PtId = ptId;
        PtNum = ptNum;
        SinDate = sinDate;
        PatientName = patientName;
        PrimaryDoctorName = primaryDoctorName;
        KaSname = kaSname;
        TodoKbnName = todoKbnName;
        TantoName = tantoName;
        Cmt2 = cmt2;
        CreateDate = createDate;
        UpdaterName = updaterName;
        Cmt1 = cmt1;
        CreaterName = createrName;
        TodoGrpName = todoGrpName;
        Term = term;
        HokenPid = hokenPid;
        Houbetu = houbetu;
        HokenKbn = hokenKbn;
        HokensyaNo = hokensyaNo;
        HokenId = hokenId;
        UpdateDate = updateDate;
        Tanto = tantoId;
        TodoNo = todoNo;
        TodoEdaNo = todoEdaNo;
        RaiinNo = raiinNo;
        TodoKbnNo = todoKbnNo;
        TodoGrpNo = todoGrpNo;
        IsDone = isDone;
        Status = status;
        Sex = string.Empty;
        switch (gender)
        {
            case 1:
                Sex = "男";
                break;
            case 2:
                Sex = "女";
                break;
        }
        GroupColor = groupColor;
        CreateId = createId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string PatientName { get; private set; }

    public string PrimaryDoctorName { get; private set; }

    public string KaSname { get; private set; }

    public string TodoKbnName { get; private set; }

    public string TantoName { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public string UpdaterName { get; private set; }

    public string CreaterName { get; private set; }

    public string TodoGrpName { get; private set; }

    public int TodoNo { get; private set; }

    public int TodoEdaNo { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int TodoKbnNo { get; private set; }

    public int TodoGrpNo { get; private set; }

    public int Tanto { get; private set; }

    public int Term { get; private set; }

    public int HokenPid { get; private set; }

    public int HokenKbn { get; private set; }

    public string Cmt1 { get; private set; }

    public string Cmt2 { get; private set; }

    public int IsDone { get; private set; }

    public int IsDeleted { get; private set; }

    public string HokensyaNo { get; private set; }

    public int HokenId { get; private set; }

    public string Houbetu { get; private set; }

    public int Status { get; private set; }

    public string Sex { get; private set; }

    public string GroupColor { get; private set; }

    public int CreateId { get; private set; }
}