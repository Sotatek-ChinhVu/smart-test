using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public int KarteKbn { get; private set; }
        public string KbnName { get; private set; }
        public string KbnShortName { get; private set; }
        public int CanImage { get; private set; }
        public int SortNo { get; private set; }
        public List<KarteInfHistoryItem> KarteData { get; private set; }
    }
}
