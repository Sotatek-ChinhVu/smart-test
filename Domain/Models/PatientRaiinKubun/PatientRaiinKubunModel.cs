using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientRaiinKubun
{
    public class PatientRaiinKubunModel
    {
        public PatientRaiinKubunModel(int hpId, int groupId, string groupName, int kbnCd, int sortNo)
        {
            HpId = hpId;
            GroupId = groupId;
            GroupName = groupName;
            KbnCd = kbnCd;
            SortNo = sortNo;
        }

        public int HpId { get; private set; }
        
        public int GroupId { get; private set; }

        public string GroupName { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }
    }
}
