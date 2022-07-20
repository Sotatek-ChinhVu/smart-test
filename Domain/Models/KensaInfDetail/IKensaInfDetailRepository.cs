using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaInfDetail
{
    public interface IKensaInfDetailRepository
    {
        List<KensaInfDetailModel> GetListByPtIdAndSinDate(long ptId, int sinDate);
    }
}
