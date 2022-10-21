using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.Reception.Get;

namespace UseCase.Reception.GetReceptionDefault
{
    public class GetReceptionDefaultOutputData: IOutputData
    {
        public GetReceptionDefaultOutputData(ReceptionModel data, GetReceptionDefaultStatus status)
        {
            Data = data;
            Status = status;
        }

        public ReceptionModel Data { get; private set; }

        public GetReceptionDefaultStatus Status { get; private set; }
    }
}
