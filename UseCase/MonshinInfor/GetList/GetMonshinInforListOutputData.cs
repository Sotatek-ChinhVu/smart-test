using Domain.Models.MonshinInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.GetList
{
    public class GetMonshinInforListOutputData : IOutputData
    {
        public List<MonshinInforModel> MonshinInforModels { get; private set; }

        public GetMonshinInforListStatus Status { get; private set; }

        public GetMonshinInforListOutputData(List<MonshinInforModel> monshinInforModels, GetMonshinInforListStatus status)
        {
            this.MonshinInforModels = monshinInforModels;
            Status = status;
        }
    }
}
