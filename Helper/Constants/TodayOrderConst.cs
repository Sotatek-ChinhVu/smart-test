namespace Helper.Constants
{
    public class TodayOrderConst
    {
        public enum TodayOrdValidationStatus
        {
            InvalidSpecialItem = 1,
            InvalidSpecialStadardUsage,
            InvalidSpecialSuppUsage,
            InvalidHasUsageButNotDrug,
            InvalidHasUsageButNotInjectionOrDrug,
            InvalidHasDrugButNotUsage,
            InvalidHasInjectionButNotUsage,
            InvalidHasNotBothInjectionAndUsageOf28,
            InvalidSpecialDetailItem,
            InvalidStandardUsageOfDrugOrInjection,
            InvalidSuppUsageOfDrugOrInjection,
            InvalidBunkatu,
            InvalidUsageWhenBuntakuNull,
            InvalidSumBunkatuDifferentSuryo,
            InvalidUsage,
            InvalidSuppUsage,
            InvalidQuantityUnit,
            InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull,
            InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu,
            InvalidPrice,
            InvalidSuryoOfReffill,
            InvalidCmt840,
            InvalidCmt842,
            InvalidCmt842CmtOptMoreThan38,
            InvalidCmt830CmtOpt,
            InvalidCmt830CmtOptMoreThan38,
            InvalidCmt831,
            InvalidCmt850Date,
            InvalidCmt850OtherDate,
            InvalidCmt851,
            InvalidCmt852,
            InvalidCmt853,
            InvalidCmt880,
            Valid
        };

        //public enum OrdInfDetailValidationStatus
        //{
        //    InvalidSpecialItem,
        //    InvalidUsage,
        //    InvalidSuppUsage,
        //    InvalidQuantityUnit,
        //    InvalidSuryoAndYohoKbn,
        //    InvalidSuryoBunkatu,
        //    InvalidPrice,
        //    InvalidSuryoOfReffill,
        //    InvalidCmt840,
        //    InvalidCmt842,
        //    InvalidCmt842CmtOpt38,
        //    InvalidCmt830CmtOpt,
        //    InvalidCmt830CmtOpt38,
        //    InvalidCmt831,
        //    InvalidCmt850Date,
        //    InvalidCmt850OtherDate,
        //    InvalidCmt851,
        //    InvalidCmt852,
        //    InvalidCmt853,
        //    InvalidCmt880,
        //    Valid
        //};
    }
}
