﻿namespace EmrCloudApi.Requests.DrugDetail
{
    public class ShowProductInfRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int SelectedIndexOfMenuLevel { get; set; }

        public int Level { get; private set; }

        public string DrugName { get; set; } = string.Empty;

        public string YJCode { get; set; } = string.Empty;
    }
}
