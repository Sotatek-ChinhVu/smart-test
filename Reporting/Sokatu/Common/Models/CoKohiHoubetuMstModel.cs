namespace Reporting.Sokatu.Common.Models
{
    public class CoKohiHoubetuMstModel
    {
        private string houbetu;
        private string hokenNameCd;

        public int PrefNo { get; set; }

        public string Houbetu
        {
            get => houbetu ?? "";
            set => houbetu = value;
        }

        public string HokenNameCd
        {
            get => hokenNameCd ?? "";
            set => hokenNameCd = value;
        }
    }
}
