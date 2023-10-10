using Domain.Common;
using Domain.Models.KensaSetDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaSet
{
    public interface IKensaSetRepository : IRepositoryBase
    {
        public bool UpdateKensaSet(int hpId, int userId, int setId, string setName, int sortNo, int isDeleted, List<KensaSetDetailModel> kensaSetDetails);
    }
}
    