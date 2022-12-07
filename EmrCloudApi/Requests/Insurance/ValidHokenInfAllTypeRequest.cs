using Domain.Models.Insurance;

namespace EmrCloudApi.Requests.Insurance
{
    public class ValidHokenInfAllTypeRequest
    {
        public int HpId { get; private set; }

        public int HokenKbn { get; private set; }

        public int SinDate { get; private set; }

        public bool IsSelectedHokenInf { get; private set; }

        public string SelectedHokenInfRodoBango { get; private set; } = string.Empty;

        public List<RousaiTenkiModel> ListRousaiTenki { get; private set; } = new List<RousaiTenkiModel>();

        public int SelectedHokenInfRousaiSaigaiKbn { get; private set; }

        public int SelectedHokenInfRousaiSyobyoDate { get; private set; }

        public string SelectedHokenInfRousaiSyobyoCd { get; private set; } = string.Empty;

        public int SelectedHokenInfRyoyoStartDate { get; private set; }

        public int SelectedHokenInfRyoyoEndDate { get; private set; }

        public int SelectedHokenInfStartDate { get; private set; }

        public int SelectedHokenInfEndDate { get; private set; }

        public bool SelectedHokenInfIsAddNew { get; private set; }

        public string SelectedHokenInfNenkinBango { get; private set; } = string.Empty;

        public string SelectedHokenInfKenkoKanriBango { get; private set; } = string.Empty;

        public int SelectedHokenInfConfirmDate { get; private set; }

        public bool SelectedHokenInfHokenMasterModelIsNull { get; private set; }

        public bool SelectedHokenInf { get; private set; }

        public string SelectedHokenInfTokki1 { get; private set; } = string.Empty;

        public string SelectedHokenInfTokki2 { get; private set; } = string.Empty;

        public string SelectedHokenInfTokki3 { get; private set; } = string.Empty;

        public string SelectedHokenInfTokki4 { get; private set; } = string.Empty;

        public string SelectedHokenInfTokki5 { get; private set; } = string.Empty;

        public string SelectedHokenInfHoubetu { get; private set; } = string.Empty;

        public bool SelectedHokenInfIsJihi { get; private set; }

        public string HokenSyaNo { get; private set; } = string.Empty;

        public string SelectedHokenInfKigo { get; private set; } = string.Empty;

        public string SelectedHokenInfBango { get; private set; } = string.Empty;

        public int SelectedHokenInfTokureiYm1 { get; private set; }

        public int SelectedHokenInfTokureiYm2 { get; private set; }

        public bool SelectedHokenInfisShahoOrKokuho { get; private set; }

        public bool SelectedHokenInfisExpirated { get; private set; }

        public int SelectedHokenInfconfirmDate { get; private set; }

        public int SelectedHokenInfHokenNo { get; private set; }

        public int SelectedHokenInfHokenEdraNo { get; private set; }

        public bool IsSelectedHokenMst { get; private set; }

        public int SelectedHokenInfHonkeKbn { get; private set; }

        public int PtBirthday { get; private set; }
    }
}
