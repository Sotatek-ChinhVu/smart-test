namespace EmrCloudApi.Tenant.Requests.ReceptionInsurance
{
    public class ValidPatternExpiratedRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int PatternHokenPid { get; set; }

        public bool PatternIsExpirated { get; set; }

        public bool HokenInfIsJihi { get; set; }

        public bool HokenInfIsNoHoken { get; set; }

        public int PatternConfirmDate { get; set; }

        public int HokenInfStartDate { get; set; }

        public int HokenInfEndDate { get; set; }

        public bool IsHaveHokenMst { get; set; }

        public int HokenMstStartDate { get; set; }

        public int HokenMstEndDate { get; set; }

        public string HokenMstDisplayTextMaster { get; set; } = string.Empty;

        public bool IsEmptyKohi1 { get; set; }

        public bool IsKohiHaveHokenMst1 { get; set; }

        public int KohiConfirmDate1 { get; set; }

        public string KohiHokenMstDisplayTextMaster1 { get; set; } = string.Empty;

        public int KohiHokenMstStartDate1 { get; set; }

        public int KohiHokenMstEndDate1 { get; set; }

        public bool IsEmptyKohi2 { get; set; }

        public bool IsKohiHaveHokenMst2 { get; set; }

        public int KohiConfirmDate2 { get; set; }

        public string KohiHokenMstDisplayTextMaster2 { get; set; } = string.Empty;

        public int KohiHokenMstStartDate2 { get; set; }

        public int KohiHokenMstEndDate2 { get; set; }

        public bool IsEmptyKohi3 { get; set; }

        public bool IsKohiHaveHokenMst3 { get; set; }

        public int KohiConfirmDate3 { get; set; }

        public string KohiHokenMstDisplayTextMaster3 { get; set; } = string.Empty;

        public int KohiHokenMstStartDate3 { get; set; }

        public int KohiHokenMstEndDate3 { get; set; }

        public bool IsEmptyKohi4 { get; set; }

        public bool IsKohiHaveHokenMst4 { get; set; }

        public int KohiConfirmDate4 { get; set; }

        public string KohiHokenMstDisplayTextMaster4 { get; set; } = string.Empty;

        public int KohiHokenMstStartDate4 { get; set; }

        public int KohiHokenMstEndDate4 { get; set; }

        public int PatientInfBirthday { get; set; }

        public int PatternHokenKbn { get; set; }
    }
}
