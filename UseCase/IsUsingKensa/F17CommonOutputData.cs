﻿using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.IsUsingKensa
{
    public class F17CommonOutputData : IOutputData
    {
        public F17CommonOutputData(List<string> kensaItemCd, F17CommonStatus status, List<KensaStdMstModel> kensaStdMsts, List<string> itemCd, Dictionary<int, string> materialMsts, Dictionary<int, string> containerMsts, Dictionary<string, string> kensaCenterMsts, Dictionary<string, double> tenOfItem) 
        {
            KensaItemCd = kensaItemCd;
            Status = status;
            KensaStdMsts = kensaStdMsts;
            ItemCd = itemCd;
            MaterialMsts = materialMsts;
            ContainerMsts = containerMsts;
            KensaCenterMsts = kensaCenterMsts;
            TenOfItem = tenOfItem;
        }

        public List<string> KensaItemCd { get; private set; }

        public F17CommonStatus Status { get; private set; }

        public List<KensaStdMstModel> KensaStdMsts { get; private set; }

        public List<string> ItemCd { get; private set; }

        public Dictionary<int, string> MaterialMsts { get; private set; }

        public Dictionary<int, string> ContainerMsts { get; private set; }

        public Dictionary<string, string> KensaCenterMsts { get; private set; }

        public Dictionary<string, double> TenOfItem { get; private set; }
    }
}
