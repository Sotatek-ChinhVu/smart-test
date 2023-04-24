using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GrpKarteHistoryItem
    {
        public GrpKarteHistoryItem(int karteKbn, string kbnName, string kbnShortName, int canImage, int sortNo, List<KarteInfHistoryItem> karteData)
        {
            KarteKbn = karteKbn;
            KbnName = kbnName;
            KbnShortName = kbnShortName;
            CanImage = canImage;
            SortNo = sortNo;
            KarteData = karteData;
        }

        [JsonPropertyName("karteKbn")]
        public int KarteKbn { get; private set; }

        [JsonPropertyName("kbnName")]
        public string KbnName { get; private set; }

        [JsonPropertyName("kbnShortName")]
        public string KbnShortName { get; private set; }

        [JsonPropertyName("canImage")]
        public int CanImage { get; private set; }

        [JsonPropertyName("sortNo")]
        public int SortNo { get; private set; }

        [JsonPropertyName("karteData")]
        public List<KarteInfHistoryItem> KarteData { get; private set; }
    }
}
