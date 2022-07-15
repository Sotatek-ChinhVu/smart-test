using Domain.Models.OrdInfs;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetListTrees
{
    public class GroupHokenItem
    {

        public int  HokenPid { get; private set; }
        public string HokenTitle { get; private set; }
        public List<GroupOdrItem> GroupOdrItems { get; private set; }

        public GroupHokenItem(List<GroupOdrItem> groupOdrItem, int hokenPid, string hokenTitle)
        {
            GroupOdrItems = groupOdrItem;
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
        }
    }
}
