using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetList
{
    public class GetListFlowSheetInputData : IInputData<GetListFlowSheetOutputData>
    {
        public int HpId { get; private set; }
        
        public long PtId { get; private set; }
        
        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public bool IsHolidayOnly { get; private set; }

        public bool IsRaiinListMstOnly { get; private set; }

        public int HolidayFrom { get; private set; }

        public int HolidayTo { get; private set; }

        public GetListFlowSheetInputData(int hpId, long ptId, int sinDate, long raiinNo, bool isHolidayOnly, int holidayFrom, int holidayTo, bool isRaiinListMstOnly)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            IsHolidayOnly = isHolidayOnly;
            HolidayFrom = holidayFrom;
            HolidayTo = holidayTo;
            IsRaiinListMstOnly = isRaiinListMstOnly;
        }
    }
}
