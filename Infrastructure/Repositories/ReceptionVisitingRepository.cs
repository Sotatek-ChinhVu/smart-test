using Domain.Models.Insurance;
using Domain.Models.ReceptionVisitingModel;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionVisitingRepository : IReceptionVisitingRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionVisitingRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }
        public IEnumerable<ReceptionVisitingModel> GetReceptionVisiting(long raiinNo)
        {
            var listDataRaiinInf = _tenantDataContext.RaiinInfs.Where(x => x.RaiinNo == raiinNo).ToList();

            var listVisitingModel = new List<ReceptionVisitingModel>();
            if (listDataRaiinInf.Count > 0)
            {
                foreach (var item in listDataRaiinInf)
                {
                    var itemModelDorai = new ReceptionVisitingModel(
                                            item.PtId,
                                            item.UketukeNo,
                                            item.KaId,
                                            item.UketukeTime ?? String.Empty,
                                            item.Status,
                                            item.YoyakuId,
                                            item.TantoId
                                         );

                    listVisitingModel.Add(itemModelDorai);
                }

            }

            return listVisitingModel;
        }

    }
}
