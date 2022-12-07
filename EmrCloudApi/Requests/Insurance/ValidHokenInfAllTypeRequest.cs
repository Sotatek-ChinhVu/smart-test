using Domain.Models.Insurance;

namespace EmrCloudApi.Requests.Insurance
{
    public class ValidHokenInfAllTypeRequest
    {
        public int HpId { get; set; }

        public int HokenKbn { get; set; }

        public int SinDate { get;  set; }

        public bool IsSelectedHokenInf { get;  set; }

        public string SelectedHokenInfRodoBango { get;  set; } = string.Empty;

        public List<RousaiTenkiModel> ListRousaiTenki { get;  set; } = new List<RousaiTenkiModel>();

        public int SelectedHokenInfRousaiSaigaiKbn { get;  set; }

        public int SelectedHokenInfRousaiSyobyoDate { get;  set; }

        public string SelectedHokenInfRousaiSyobyoCd { get;  set; } = string.Empty;

        public int SelectedHokenInfRyoyoStartDate { get;  set; }

        public int SelectedHokenInfRyoyoEndDate { get;  set; }

        public int SelectedHokenInfStartDate { get;  set; }

        public int SelectedHokenInfEndDate { get;  set; }

        public bool SelectedHokenInfIsAddNew { get;  set; }

        public string SelectedHokenInfNenkinBango { get;  set; } = string.Empty;

        public string SelectedHokenInfKenkoKanriBango { get;  set; } = string.Empty;

        public int SelectedHokenInfConfirmDate { get;  set; }

        public bool SelectedHokenInfHokenMasterModelIsNull { get;  set; }

        public bool SelectedHokenInf { get;  set; }

        public string SelectedHokenInfTokki1 { get;  set; } = string.Empty;

        public string SelectedHokenInfTokki2 { get;  set; } = string.Empty;

        public string SelectedHokenInfTokki3 { get;  set; } = string.Empty;

        public string SelectedHokenInfTokki4 { get;  set; } = string.Empty;

        public string SelectedHokenInfTokki5 { get;  set; } = string.Empty;

        public string SelectedHokenInfHoubetu { get;  set; } = string.Empty;

        public bool SelectedHokenInfIsJihi { get;  set; }

        public string HokenSyaNo { get;  set; } = string.Empty;

        public string SelectedHokenInfKigo { get;  set; } = string.Empty;

        public string SelectedHokenInfBango { get;  set; } = string.Empty;

        public int SelectedHokenInfTokureiYm1 { get;  set; }

        public int SelectedHokenInfTokureiYm2 { get;  set; }

        public bool SelectedHokenInfisShahoOrKokuho { get;  set; }

        public bool SelectedHokenInfisExpirated { get;  set; }

        public int SelectedHokenInfconfirmDate { get;  set; }

        public int SelectedHokenInfHokenNo { get;  set; }

        public int SelectedHokenInfHokenEdraNo { get;  set; }

        public bool IsSelectedHokenMst { get;  set; }

        public int SelectedHokenInfHonkeKbn { get;  set; }

        public int PtBirthday { get;  set; }

        public bool SelectedHokenInfIsAddHokenCheck { get; set; }

        public int SelectedHokenInfHokenChecksCount { get; set; }

        public bool HokenInfIsNoHoken { get; set; }

        public int HokenInfConfirmDate { get; set; }
    }
}
