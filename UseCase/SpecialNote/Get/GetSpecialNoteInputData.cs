﻿using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Get
{
    public class GetSpecialNoteInputData : IInputData<GetSpecialNoteOutputData>
    {
        public GetSpecialNoteInputData(int hpId, long ptId, int sex)
        {
            HpId = hpId;
            PtId = ptId;
            Sex = sex;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int Sex { get; private set; }
    }
}
