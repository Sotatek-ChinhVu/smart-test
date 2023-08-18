using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor;

public class PatientInfoDto
{
    public PatientInfoDto(PatientInforModel model)
    {
        PtId = model.PtId;
        PtNum = model.PtNum;
        KanaName = model.KanaName;
        Name = model.Name;
        SeqNo = model.SeqNo;
        ReferenceNo = model.ReferenceNo;
        Sex = model.Sex;
        Birthday = model.Birthday;
        LimitConsFlg = model.LimitConsFlg;
        IsDead = model.IsDead;
        DeathDate = model.DeathDate;
        HomePost = model.HomePost;
        HomeAddress1 = model.HomeAddress1;
        HomeAddress2 = model.HomeAddress2;
        Tel1 = model.Tel1;
        Tel2 = model.Tel2;
        Mail = model.Mail;
        Setanusi = model.Setanusi;
        Zokugara = model.Zokugara;
        Job = model.Job;
        RenrakuName = model.RenrakuName;
        RenrakuPost = model.RenrakuPost;
        RenrakuAddress1 = model.RenrakuAddress1;
        RenrakuAddress2 = model.RenrakuAddress2;
        RenrakuTel = model.RenrakuTel;
        RenrakuMemo = model.RenrakuMemo;
        OfficeName = model.OfficeName;
        OfficePost = model.OfficePost;
        OfficeAddress1 = model.OfficeAddress1;
        OfficeAddress2 = model.OfficeAddress2;
        OfficeTel = model.OfficeTel;
        OfficeMemo = model.OfficeMemo;
        IsRyosyoDetail = model.IsRyosyoDetail;
        PrimaryDoctor = model.PrimaryDoctor;
        IsTester = model.IsTester;
        MainHokenPid = model.MainHokenPid;
        Memo = model.Memo;
        FirstVisitDate = model.FirstVisitDate;
        RainCountInt = model.RainCountInt;
        Comment = model.Comment;
        LastVisitDate = model.LastVisitDate;
        IsShowKyuSeiName = model.IsShowKyuSeiName;
    }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public long SeqNo { get; private set; }

    public long ReferenceNo { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int LimitConsFlg { get; private set; }

    public int IsDead { get; private set; }

    public int DeathDate { get; private set; }

    public string HomePost { get; private set; }

    public string HomeAddress1 { get; private set; }

    public string HomeAddress2 { get; private set; }

    public string Tel1 { get; private set; }

    public string Tel2 { get; private set; }

    public string Mail { get; private set; }

    public string Setanusi { get; private set; }

    public string Zokugara { get; private set; }

    public string Job { get; private set; }

    public string RenrakuName { get; private set; }

    public string RenrakuPost { get; private set; }

    public string RenrakuAddress1 { get; private set; }

    public string RenrakuAddress2 { get; private set; }

    public string RenrakuTel { get; private set; }

    public string RenrakuMemo { get; private set; }

    public string OfficeName { get; private set; }

    public string OfficePost { get; private set; }

    public string OfficeAddress1 { get; private set; }

    public string OfficeAddress2 { get; private set; }

    public string OfficeTel { get; private set; }

    public string OfficeMemo { get; private set; }

    public int IsRyosyoDetail { get; private set; }

    public int PrimaryDoctor { get; private set; }

    public int IsTester { get; private set; }

    public int MainHokenPid { get; private set; }

    public string Memo { get; private set; }

    public int FirstVisitDate { get; private set; }

    public int RainCountInt { get; private set; }

    public string Comment { get; private set; }

    public int LastVisitDate { get; private set; }

    public bool IsShowKyuSeiName { get; private set; }

    public string RainCount => RainCountInt + "人";
}
