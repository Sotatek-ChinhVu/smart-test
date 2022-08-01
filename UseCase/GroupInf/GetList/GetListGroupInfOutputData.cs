using Domain.Models.GroupInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.GroupInf.GetList
{
    public class GetListGroupInfOutputData : IOutputData
    {
        public List<GroupInfModel> ListGroup { get; private set; }
        public GetListGroupInfSatus Status { get; private set; }
        public GetListGroupInfOutputData(List<GroupInfModel> data, GetListGroupInfSatus status)
        {
            ListGroup = data;
            Status = status;
        }
    }
}
