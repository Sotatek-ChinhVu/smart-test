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
    public class GetOrdInfListTreeOutputData : IOutputData
    {

        public List<GroupHokenItem> GroupHokens { get; private set; }

        public GetOrdInfListTreeOutputData(List<GroupHokenItem>  groupHokens)
        {
            GroupHokens = groupHokens;
        }
    }
}
