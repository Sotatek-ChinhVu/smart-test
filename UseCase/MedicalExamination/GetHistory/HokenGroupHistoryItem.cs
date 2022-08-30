using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class HokenGroupHistoryItem
    {
        public HokenGroupHistoryItem(int hokenPid, string hokenTitle, List<GroupOdrGHistoryItem> groupOdrHistories)
        {
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
            GroupOdrHistories = groupOdrHistories;
        }

        public int HokenPid { get; private set; }
        public string HokenTitle { get; private set; }
        public List<GroupOdrGHistoryItem> GroupOdrHistories { get; private set; }
    }
}
