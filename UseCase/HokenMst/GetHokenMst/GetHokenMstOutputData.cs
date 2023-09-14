﻿using UseCase.Core.Sync.Core;

namespace UseCase.HokenMst.GetHokenMst
{
    public class GetHokenMstOutputData : IOutputData
    {
        public GetHokenMstOutputData(Dictionary<string, string> hokenInfModels, Dictionary<string, string> kohiModelWithFutansyanos, GetHokenMstStatus status)
        {
            HokenInfModels = hokenInfModels;
            KohiModelWithFutansyanos = kohiModelWithFutansyanos;
            Status = status;
        }

        public Dictionary<string, string> HokenInfModels { get; private set; }

        public Dictionary<string, string> KohiModelWithFutansyanos { get; private set; }

        public GetHokenMstStatus Status { get; private set; }
    }
}
