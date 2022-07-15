using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Get
{
    public class GetReceptionOutputData : IOutputData
    {
        public ReceptionModel? ReceptionModel { get; private set; }

        public GetReceptionStatus Status { get; private set; }

        public GetReceptionOutputData(ReceptionModel? receptionModel, GetReceptionStatus status)
        {
            ReceptionModel = receptionModel;
            Status = status;
        }
    }
}
