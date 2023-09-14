﻿using Domain.Models.ListSetGenerationMst;
using UseCase.Core.Sync.Core;

namespace UseCase.ListSetGenerationMst.GetListSetGenerationMst
{
    public class GetListSetGenerationMstOutputData : IOutputData
    {
        public GetListSetGenerationMstOutputData(List<ListSetGenerationMstModel> listSetGenerationMsts, GetListSetGenerationMstStatus status)
        {
            ListSetGenerationMsts = listSetGenerationMsts;
            Status = status;
        }

        public List<ListSetGenerationMstModel> ListSetGenerationMsts { get; private set; }
        public GetListSetGenerationMstStatus Status { get; private set; }
    }
}
