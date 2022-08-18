﻿using Domain.Models.RaiinListMst;

namespace Domain.Models.FlowSheet
{
    public interface IFlowSheetRepository
    {
        List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo);

        List<RaiinListMstModel> GetRaiinListMsts(int hpId);

        List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo);

        void Upsert(long rainNo, long ptId, int sinDate, int tagNo, int cmtKbn, string text, int seqNo);
    }
}
