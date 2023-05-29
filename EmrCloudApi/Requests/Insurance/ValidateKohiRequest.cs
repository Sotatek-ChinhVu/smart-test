namespace EmrCloudApi.Requests.Insurance
{
    public class ValidateKohiRequest
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

        public bool SelectedKohiIsAddNew { get; set; }

        public bool SelectedHokenPatternIsExpirated { get; set; }

        #region info hokenMst
        public int KohiMasterIsFutansyaNoCheck { get; set; }

        public int KohiMasterIsJyukyusyaNoCheck { get; set; }

        public int KohiMasterIsTokusyuNoCheck { get; set; }

        public int KohiMasterStartDate { get; set; }

        public int KohiMasterEndDate { get; set; }

        public string KohiMasterDisplayTextMaster { get; set; } = string.Empty;

        public int KohiMasterJyukyuCheckDigit { get; set; }

        public int KohiMasterCheckDigit { get; set; }

        public string KohiMasterHoubetu { get; set; } = string.Empty;

        public int KohiMasterAgeStart { get; set; }

        public int KohiMasterAgeEnd { get; set; }
        #endregion
    }
}
