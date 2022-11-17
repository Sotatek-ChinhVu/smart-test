using Domain.Enum;
using Domain.Models.ApprovalInfo;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApprovalInfo
{
    public interface IApprovalInfRepository
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);
    }
}
