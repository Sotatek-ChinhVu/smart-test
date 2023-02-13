using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientInfor.GetListPatient
{
    public class GetListPatientInfoInputItem
    {
        public GetListPatientInfoInputItem(int hpId, long ptId, long ptNum, string kanaName, string name, int birthday, int lastVisitDate)
        {
            HpId = hpId;
            PtId = ptId;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            Birthday = birthday;
            LastVisitDate = lastVisitDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public string KanaName { get; private set; }

        public string Name { get; private set; }

        public int Birthday { get; private set; }

        public int LastVisitDate { get; private set; }

        public string DisplayBirthday
        {
            get
            {
                int intDate = Birthday;
                string tDate = CIUtil.SDateToShowSWDate(intDate);
                return tDate;

            }
        }

        public string KarteDaichoAge
        {
            get
            {
                if (Birthday <= 0)
                {
                    return string.Empty;
                }
                else
                {
                    return CIUtil.SDateToDecodeAge(Birthday.AsString(), DateTime.UtcNow.ToString("yyyyMMdd").AsInteger().AsString());
                }
            }
        }

        public string LastVisitDateLabel
        {
            get { return CIUtil.SDateToShowSDate(LastVisitDate); }
        }
    }
}
