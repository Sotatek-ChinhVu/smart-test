using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetList
{
    public class GetListReceSeikyuInputData : IInputData<GetListReceSeikyuOutputData>
    {
        public GetListReceSeikyuInputData(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn)
        {
            HpId = hpId;
            SinDate = sinDate;
            SinYm = sinYm;
            IsIncludingUnConfirmed = isIncludingUnConfirmed;
            PtNumSearch = ptNumSearch;
            NoFilter = noFilter;
            IsFilterMonthlyDelay = isFilterMonthlyDelay;
            IsFilterReturn = isFilterReturn;
            IsFilterOnlineReturn = isFilterOnlineReturn;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int SinYm { get; private set; }

        public bool IsIncludingUnConfirmed { get; private set; }

        public long PtNumSearch { get; private set; }

        public bool NoFilter { get; private set; }

        public bool IsFilterMonthlyDelay { get; private set; }

        public bool IsFilterReturn { get; private set; }

        public bool IsFilterOnlineReturn { get; private set; }
    }
}
