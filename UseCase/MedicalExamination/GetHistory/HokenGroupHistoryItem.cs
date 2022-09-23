using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class HokenGroupHistoryItem
    {
        public HokenGroupHistoryItem(int hokenPid, string hokenTitle, List<GroupOdrGHistoryItem> groupOdrItems)
        {
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
            GroupOdrItems = groupOdrItems;
        }

        public int HokenPid { get; private set; }
        public string HokenTitle { get; private set; }
        public List<GroupOdrGHistoryItem> GroupOdrItems { get; private set; }
    }
}
