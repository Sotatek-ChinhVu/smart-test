using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.Reception;
using Helper.Constants;

namespace Domain.Models.HistoryOrder
{
    public class HistoryOrderModel
    {
        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }
        
        public int HokenPid { get; private set; }
        
        public string HokenTitle { get; private set; }
        
        public string HokenRate { get; private set; }
        
        public int HokenType { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public int JikanKbn { get; private set; }

        public int KaId { get; private set; }

        public int TantoId { get; private set; }

        public string KaName { get; private set; }

        public string TantoName { get; private set; }

        public int SanteiKbn { get; private set; }

        public int TagNo { get; private set; }

        public string SinryoTitle { get; private set; }

        public string SanteiKbnDisplay { get => _jihiSanteiDict.FirstOrDefault(x => x.Key == SanteiKbn).Value; }

        public string SyosaisinDisplay { get => SyosaiConst.ReceptionShinDict.FirstOrDefault(x => x.Key == SyosaisinKbn).Value; }

        public string JikanDisplay { get => JikanConst.JikanKotokuDict.FirstOrDefault(x => x.Key == JikanKbn).Value; }

        private readonly Dictionary<int, string> _jihiSanteiDict = new Dictionary<int, string>()
        {
            {0,"－" },
            {2,"自費" }
        };

        public List<OrdInfModel> OrderInfList { get; private set; }

        public KarteInfModel KarteInfModel { get; private set; }


        public HistoryOrderModel(ReceptionModel receptionModel, List<OrdInfModel> orderList, KarteInfModel karteInfModel)
        {
            RaiinNo = receptionModel.RaiinNo;
            SinDate = receptionModel.SinDate;
            HokenPid = receptionModel.HokenPid;
            //HokenTitle = receptionModel
            //HokenRate = receptionModel.Hoken
            //HokenType = receptionModel.HO
            SyosaisinKbn = receptionModel.SyosaisinKbn;
            JikanKbn = receptionModel.KaId;
            TantoId = receptionModel.TantoId;

            OrderInfList = orderList;
            KarteInfModel = karteInfModel;
        }
    }
}
