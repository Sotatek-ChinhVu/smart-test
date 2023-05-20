﻿using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CreateUKEFile
{
    public class CreateUKEFileInputData : IInputData<CreateUKEFileOutputData>
    {
        public CreateUKEFileInputData(int hpId, ModeTypeCreateUKE modeType, int seikyuYm, int seikyuYmOutput, bool chkHenreisai, bool chkTogetsu, bool includeOutDrug, bool includeTester, int kaId, int doctorId, int sort, int userId)
        {
            HpId = hpId;
            ModeType = modeType;
            SeikyuYm = seikyuYm;
            SeikyuYmOutput = seikyuYmOutput;
            ChkHenreisai = chkHenreisai;
            ChkTogetsu = chkTogetsu;
            IncludeOutDrug = includeOutDrug;
            IncludeTester = includeTester;
            KaId = kaId;
            DoctorId = doctorId;
            Sort = sort;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public ModeTypeCreateUKE ModeType { get; private set; }

        public int SeikyuYm { get; private set; }

        public int SeikyuYmOutput { get; private set; }

        public bool ChkHenreisai { get; private set; }

        public bool ChkTogetsu { get; private set; }

        public bool IncludeOutDrug { get; private set; }

        public bool IncludeTester { get; private set; }

        public int KaId { get; private set; }

        public int DoctorId { get; private set; }

        public int Sort { get; private set; }

        public int UserId { get; private set; }
    }
}
