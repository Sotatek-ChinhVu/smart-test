using Domain.Models.SetGenerationMst;
using Npgsql.Replication.PgOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.GetList
{
    public class SetSendaiGenerationOutputData : IOutputData
    {
        public SetSendaiGenerationOutputData(List<SetSendaiGenerationModel> listData, SetSendaiGenerationStatus status)
        {
            ListData = listData;
            Status = status;
        }

        public List<SetSendaiGenerationModel> ListData { get; private set; }
        public SetSendaiGenerationStatus Status { get; private set; }
    }
}
