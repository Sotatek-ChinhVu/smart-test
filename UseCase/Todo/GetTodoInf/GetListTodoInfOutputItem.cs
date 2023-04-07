using Helper.Common;
using Helper.Constants;

namespace UseCase.Todo.GetTodoInf
{
    public class GetListTodoInfOutputItem
    {

        public GetListTodoInfOutputItem(int hpId, long ptNum, string patientName, int sinDate, string primaryDoctorName, string kaSname, string todoKbnName, string tantoName, string cmt2, DateTime createDate, string updaterName, string cmt1, string createrName, string todoGrpName, int term, string houbetu, string hokensyaNo, int hokenPid, int hokenKbn, int hokenId)
        {
            HpId = hpId;
            PtNum = ptNum;
            PatientName = patientName;
            SinDate = sinDate;
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
            Houbetu = houbetu;
            HokensyaNo = hokensyaNo;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            HokenId = hokenId;
        }

        public int HpId { get; private set; }

        public long PtNum { get; private set; }

        public string PatientName { get; private set; }

        public string PrimaryDoctorName { get; private set; }

        public string KaSname { get; private set; }

        public string TodoKbnName { get; private set; }

        public string TantoName { get; private set; }

        public DateTime CreateDate { get; private set; }

        public string UpdaterName { get; private set; }

        public string CreaterName { get; private set; }

        public string TodoGrpName { get; private set; }

        public int SinDate { get; private set; }

        public string DisplaySinDate
        {
            get { return CIUtil.SDateToShowSDate(SinDate); }
        }

        public string HokenKbnName
        {
            get
            {
                string result = string.Empty;
                if (PtId == 0 && HokenPid == 0 && HpId == 0)
                {
                    return string.Empty;
                }
                if (PtId == 0 && HokenId == 0 && HpId == 0)
                {
                    result = "公費";
                    return result;
                }
                if (Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    result = "公費";
                    return result;
                }
                switch (HokenKbn)
                {
                    case 0:
                        result = "自費";
                        break;
                    case 1:
                        result = "社保";
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result = "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result = "退職";
                        }
                        else
                        {
                            result = "国保";
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                        result = "労災";
                        break;
                    case 14:
                        result = "自賠";
                        break;
                }
                return result;
            }
        }

        public int Term { get; private set; }

        public string Cmt1 { get; private set; }

        public string Cmt2 { get; private set; }

        public string DisplayTerm
        {
            get { return CIUtil.SDateToShowSDate(Term); }
        }

        public string Houbetu { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public int HokenId { get; private set; }

        public int HokenPid { get; private set; }

        public long PtId { get; private set; }
    }
}
