using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Helper.Common;

namespace UseCase.PatientInfor;

public class PatientInfoWithGroup
{
    public Dictionary<int, GroupInfModel> GroupInfList { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public string Birthday { get; private set; }

    public int BirthdayRaw { get; private set; }

    public int Sex { get; private set; }

    public string Age { get; private set; }

    public string Tel1 { get; private set; }

    public string Tel2 { get; private set; }

    public string RenrakuTel { get; private set; }

    public string HomePost { get; private set; }

    public string HomeAddress { get; private set; }

    public string LastVisitDate { get; private set; }

    public PatientInfoWithGroup(PatientInforModel patientInfo, List<GroupInfModel> groupInfList)
    {
        PtId = patientInfo.PtId;
        PtNum = patientInfo.PtNum;
        KanaName = patientInfo.KanaName;
        Name = patientInfo.Name;
        Sex = patientInfo.Sex;

        //Generate birthday
        Birthday = CIUtil.SDateToShowSWDate(patientInfo.Birthday);
        BirthdayRaw = patientInfo.Birthday;

        //Generate age
        int intDate = patientInfo.Birthday;
        int rAge = 0;
        int rMonth = 0;
        int rDay = 0;
        bool isAge = CIUtil.SDateToDecodeAge(intDate, CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyyMMdd")), ref rAge, ref rMonth, ref rDay);
        if (isAge)
        {
            Age = string.Format("{0}歳 {1}ヶ月 {2}日", rAge, rMonth, rDay);
        }
        else
        {
            Age = string.Empty;
        }

        Tel1 = patientInfo.Tel1;
        Tel2 = patientInfo.Tel2;
        RenrakuTel = patientInfo.RenrakuTel;
        HomePost = patientInfo.HomePost;
        HomeAddress = patientInfo.HomeAddress1 + '\u3000' + patientInfo.HomeAddress2;
        LastVisitDate = CIUtil.SDateToShowSDate(patientInfo.LastVisitDate);

        GroupInfList = groupInfList.ToDictionary(item => item.GroupId, item => item);
    }
}