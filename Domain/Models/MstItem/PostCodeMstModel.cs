using Helper.Constants;
using System.Text.Json.Serialization;

namespace Domain.Models.MstItem
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

        [JsonConstructor]
        public PostCodeMstModel(long id, int hpId, string postCd, string prefKana, string cityKana, string postalTermKana, string prefName, string cityName, string banti, int isDeleted, ModelStatus postCodeStatus)
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
            PostCodeStatus = postCodeStatus;
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

        public ModelStatus PostCodeStatus { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(PostCd) && string.IsNullOrEmpty(CityName) && string.IsNullOrEmpty(PrefName) && string.IsNullOrEmpty(Banti);
        }
    }
}
