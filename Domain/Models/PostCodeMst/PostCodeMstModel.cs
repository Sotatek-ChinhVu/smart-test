namespace Domain.Models.PostCodeMst
{
    public class PostCodeMstModel
    {
        public PostCodeMstModel(long id, int hpId, string postCd, string prefKana, string cityKana, string postalTermKana, string prefName, string cityName, string banti, int isDeleted)
        {
            Id = id;
            HpId = hpId;
            PostCd = postCd;
            PrefKana = prefKana;
            CityKana = cityKana;
            PostalTermKana = postalTermKana;
            PrefName = prefName;
            CityName = cityName;
            Banti = banti;
            IsDeleted = isDeleted;
        }

        public long Id { get; private set; }
        public int HpId { get; private set; }
        public string PostCd { get; private set; }
        public string PrefKana { get; private set; }
        public string CityKana { get; private set; }
        public string PostalTermKana { get; private set; }
        public string PrefName { get; private set; }
        public string CityName { get; private set; }
        public string Banti { get; private set; }
        public int IsDeleted { get; private set; }

        public string Address
        {
            get => PrefName + CityName + Banti;
        }
    }
}
