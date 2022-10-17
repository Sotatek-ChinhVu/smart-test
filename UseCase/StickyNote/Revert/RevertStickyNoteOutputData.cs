﻿using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class RevertStickyNoteOutputData : IOutputData
    {
        public RevertStickyNoteOutputData(bool successed, UpdateStickyNoteStatus status)
        {
            Successed = successed;
            Status = status;
        }

        public bool Successed { get; private set; }
        public UpdateStickyNoteStatus Status { get; private set; }
    }
}
