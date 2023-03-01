using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GroupInf
{
    public interface IGroupInfRepository : IRepositoryBase
    {
        IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId);

        IEnumerable<GroupInfModel> GetAllByPtIdList(List<long> ptIdList);
    }
}
