using Domain.Models.InsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.DeleteHokenMaster
{
    public class DeleteHokenMasterOutputData : IOutputData
    {
        public string Message { get; private set; }

        public DeleteHokenMasterStatus Status { get; private set; }

        public DeleteHokenMasterOutputData(DeleteHokenMasterStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
