﻿using Domain.Models.InsuranceInfor;
using Domain.Models.KarteInf;
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

        public List<OrdInfModel> OrderInfList { get; private set; }

        public List<KarteInfModel> KarteInfModels { get; private set; }

        public string SanteiKbnDisplay { get => _jihiSanteiDict.FirstOrDefault(x => x.Key == SanteiKbn).Value; }

        public string SyosaisinDisplay { get => SyosaiConst.ReceptionShinDict.FirstOrDefault(x => x.Key == SyosaisinKbn).Value; }

        public string JikanDisplay { get => JikanConst.JikanKotokuDict.FirstOrDefault(x => x.Key == JikanKbn).Value; }

        private readonly Dictionary<int, string> _jihiSanteiDict = new Dictionary<int, string>()
        {
            {0,"－" },
            {2,"自費" }
        };

        public List<FileInfModel> ListKarteFile { get; private set; }

        public int Status { get; private set; }

        public HistoryOrderModel(ReceptionModel receptionModel, InsuranceModel insuranceModel, List<OrdInfModel> orderList, List<KarteInfModel> karteInfModels, string kaName, string tantoName, int tagNo, string sinryoTitle, List<FileInfModel> listKarteFile)
        {
            RaiinNo = receptionModel.RaiinNo;
            SinDate = receptionModel.SinDate;
            SyosaisinKbn = receptionModel.SyosaisinKbn;
            KaId = receptionModel.KaId;
            TantoId = receptionModel.TantoId;
            JikanKbn = receptionModel.JikanKbn;
            HokenPid = receptionModel.HokenPid;
            HokenTitle = insuranceModel.HokenName;
            HokenRate = insuranceModel.DisplayRateOnly;
            HokenType = insuranceModel.GetHokenPatternType();
            KaName = kaName;
            TantoName = tantoName;
            TagNo = tagNo;
            SinryoTitle = sinryoTitle;
            OrderInfList = orderList;
            KarteInfModels = karteInfModels;
            ListKarteFile = listKarteFile;
            Status = receptionModel.Status;
        }
    }
}
