﻿using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetDefaultSelectPattern
{
    public class GetDefaultSelectPatternInputData : IInputData<GetDefaultSelectPatternOutputData>
    {
        public GetDefaultSelectPatternInputData(int hpId, long ptId, int sinDate, List<int> historyPids, int selectedHokenPid)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HistoryPids = historyPids;
            SelectedHokenPid = selectedHokenPid;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public List<int> HistoryPids { get; private set; }

        public int SelectedHokenPid { get; private set; }
    }
}