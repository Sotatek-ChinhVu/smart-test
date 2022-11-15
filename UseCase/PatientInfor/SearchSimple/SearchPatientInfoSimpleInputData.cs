﻿using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchSimple
{
    public class SearchPatientInfoSimpleInputData : IInputData<SearchPatientInfoSimpleOutputData>
    {
        public string Keyword { get; private set; }

        public bool ContainMode { get; private set; }

        public int HpId { get; private set; }

        public SearchPatientInfoSimpleInputData(string keyword, bool containMode, int hpId)
        {
            Keyword = keyword;
            ContainMode = containMode;
            HpId = hpId;
        }
    }
}
