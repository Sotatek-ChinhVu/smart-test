using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public interface IFlowSheetRepository
    {
        List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo);
        List<RaiinListTag> GetRaiinListTags(int hpId, long ptId);
        List<RaiinListCmt> GetRaiinListCmts(int hpId, long ptId);
        List<RaiinListInfModel> GetRaiinListInfModels(int hpId, long ptId);
        List<RaiinListMstModel> GetRaiinListMsts(int hpId);
        List<HolidayModel> GetHolidayMst(int hpId);
        List<RaiinDateModel> GetListRaiinNo(int hpId, long ptId, int sinDate);
    }
}
