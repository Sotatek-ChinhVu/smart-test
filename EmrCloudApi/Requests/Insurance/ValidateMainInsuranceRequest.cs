namespace EmrCloudApi.Requests.Insurance
{
    public class ValidateMainInsuranceRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public int PtBirthday { get; set; }

        public int HokenKbn { get; set; }

        public string HokenSyaNo { get; set; } = string.Empty;

        public bool IsSelectedHokenPattern { get; set; }

        public bool IsSelectedHokenInf { get; set; }

        public bool IsSelectedHokenMst { get; set; }

        public string SelectedHokenInfHoubetu { get; set; } = string.Empty;

        public int SelectedHokenInfHokenNo { get; set; }

        public int SelectedHokenInfHokenEdra { get; set; }

        public bool SelectedHokenInfIsAddNew { get; set; }

        public bool SelectedHokenInfIsJihi { get; set; }

        public int SelectedHokenInfStartDate { get; set; }

        public int SelectedHokenInfEndDate { get; set; }

        public int SelectedHokenInfHokensyaMstIsKigoNa { get; set; }

        public string SelectedHokenInfKigo { get; set; } = string.Empty;

        public string SelectedHokenInfBango { get; set; } = string.Empty;

        public int SelectedHokenInfHonkeKbn { get; set; }

        public int SelectedHokenInfTokureiYm1 { get; set; }

        public int SelectedHokenInfTokureiYm2 { get; set; }

        public bool SelectedHokenInfIsShahoOrKokuho { get; set; }

        public bool SelectedHokenInfIsExpirated { get; set; }

        public bool SelectedHokenInfIsIsNoHoken { get; set; }

        public int SelectedHokenInfConfirmDate { get; set; }

        public bool SelectedHokenInfIsAddHokenCheck { get; set; }

        public string SelectedHokenInfTokki1 { get; set; } = string.Empty;

        public string SelectedHokenInfTokki2 { get; set; } = string.Empty;

        public string SelectedHokenInfTokki3 { get; set; } = string.Empty;

        public string SelectedHokenInfTokki4 { get; set; } = string.Empty;

        public string SelectedHokenInfTokki5 { get; set; } = string.Empty;

        public bool SelectedHokenPatternIsEmptyKohi1 { get; set; }

        public bool SelectedHokenPatternIsEmptyKohi2 { get; set; }

        public bool SelectedHokenPatternIsEmptyKohi3 { get; set; }

        public bool SelectedHokenPatternIsEmptyKohi4 { get; set; }

        public bool SelectedHokenPatternIsExpirated { get; set; }

        public bool SelectedHokenPatternIsEmptyHoken { get; set; }
    }
}
