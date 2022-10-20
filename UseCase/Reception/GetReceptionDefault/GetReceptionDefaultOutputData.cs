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
        public GetReceptionDefaultOutputData(ReceptionDefautDataModel data, GetReceptionDefaultStatus status)
        {
            Data = data;
            Status = status;
        }

        public ReceptionDefautDataModel Data { get; set; }

        public GetReceptionDefaultStatus Status { get; private set; }
    }
}
