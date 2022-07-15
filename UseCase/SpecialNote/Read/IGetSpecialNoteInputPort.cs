﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.Read
{
    public interface IGetSpecialNoteInputPort : IInputPort<GetSpecialNoteInputData, GetSpecialNoteOutputData>
    {
    }
}
