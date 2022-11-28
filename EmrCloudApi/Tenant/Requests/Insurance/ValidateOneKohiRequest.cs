namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class ValidateOneKohiRequest
    {
        public int SinDate { get; set; }

        public int PtBirthday { get; set; }

        public bool IsKohiEmptyModel { get; set; }

        public bool IsSelectedKohiMst { get; set; }

        public string SelectedKohiFutansyaNo { get; set; } = string.Empty;

        public string SelectedKohiJyukyusyaNo { get; set; } = string.Empty;

        public string SelectedKohiTokusyuNo { get; set; } = string.Empty;

        public int SelectedKohiStartDate { get; set; }

        public int SelectedKohiEndDate { get; set; }

        public int SelectedKohiConfirmDate { get; set; }

        public int SelectedKohiHokenNo { get; set; }

        public int SelectedKohiHokenEdraNo { get; set; }

        public bool SelectedKohiIsAddNew { get; set; }
    }
}