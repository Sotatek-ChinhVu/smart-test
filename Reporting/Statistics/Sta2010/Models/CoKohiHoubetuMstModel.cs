namespace Reporting.Statistics.Sta2010.Models
{
    public class CoKohiHoubetuMstModel
    {
        private string houbetu = string.Empty;
        private string hokenNameCd = string.Empty;

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
