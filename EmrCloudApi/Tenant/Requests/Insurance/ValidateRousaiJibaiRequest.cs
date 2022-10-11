using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class ValidateRousaiJibaiRequest
    {
        public int HpId { get; set; }

        public int HokenKbn { get; set; }

        public int SinDate { get; set; }

        public bool IsSelectedHokenInf { get; set; }

        public string SelectedHokenInfRodoBango { get; set; } = string.Empty;

        public List<RousaiTenkiModel> ListRousaiTenki { get; set; } = new List<RousaiTenkiModel>();

        public int SelectedHokenInfRousaiSaigaiKbn { get; set; }

        public int SelectedHokenInfRousaiSyobyoDate { get; set; }

        public string SelectedHokenInfRousaiSyobyoCd { get; set; } = string.Empty;

        public int SelectedHokenInfRyoyoStartDate { get; set; }

        public int SelectedHokenInfRyoyoEndDate { get; set; }

        public int SelectedHokenInfStartDate { get; set; }

        public int SelectedHokenInfEndDate { get; set; }

        public bool SelectedHokenInfIsAddNew { get; set; }

        public string SelectedHokenInfNenkinBango { get; set; } = string.Empty;

        public string SelectedHokenInfKenkoKanriBango { get; set; } = string.Empty;

        public int SelectedHokenInfConfirmDate { get; set; }
    }
}
