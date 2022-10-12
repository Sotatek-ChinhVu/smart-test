﻿using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RaiinInfConst;

namespace UseCase.OrdInfs.ValidationTodayOrd
{
    public class ValidationTodayOrdOutputData : IOutputData
    {
        public ValidationTodayOrdOutputData(ValidationTodayOrdStatus status, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validations, RaiinInfTodayOdrValidationStatus validationRaiinInf, KarteValidationStatus validationKarte)
        {
            Status = status;
            Validations = validations;
            ValidationRaiinInf = validationRaiinInf;
            ValidationKarte = validationKarte;
        }

        public ValidationTodayOrdStatus Status { get; private set; }
        public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> Validations { get; private set; }
        public RaiinInfTodayOdrValidationStatus ValidationRaiinInf { get; private set; }
        public KarteValidationStatus ValidationKarte { get; private set; }
    }
}
