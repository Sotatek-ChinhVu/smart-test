﻿using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf.Get
{
    public class GetSystemGenerationConfInputData : IInputData<GetSystemGenerationConfOutputData>
    {
        public GetSystemGenerationConfInputData(int hpId, int grpCd, int grpEdaNo, int presentDate, int defaultValue, string defaultParam)
        {
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            PresentDate = presentDate;
            DefaultValue = defaultValue;
            DefaultParam = defaultParam;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }

        public int PresentDate { get; private set; }

        public int DefaultValue { get; private set; }

        public string DefaultParam { get; private set; }
    }
}
