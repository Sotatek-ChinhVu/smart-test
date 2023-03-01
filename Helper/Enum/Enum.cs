namespace Helper.Enum
{
    public enum CheckSpecialType
    {
        /// <summary>
        /// 年齢制限
        /// </summary>
        AgeLimit = 1,

        /// <summary>
        /// 有効期限
        /// </summary>
        Expiration = 2,

        /// <summary>
        /// 算定回数
        /// </summary>
        CalculationCount = 3,

        /// <summary>
        /// コメント
        /// </summary>
        ItemComment = 4,

        /// <summary>
        /// 項目重複
        /// </summary>
        Duplicate = 5,
    }

    public enum CheckAgeType
    {
        MaxAge,
        MinAge
    }

    public enum WindowType
    {
        MedicalExamination,
        ReceptionInInsertMode,
        ReceptionInUpdateMode,
        ReceptionInYoyakuMode,
        SearchPatient,
        MonshinInput,
        Accounting,
        AccountingCard,
        FamilyInfo,
        AccountDueList,
        Scan,
        ApprovalInfo,
        DiseaseRegistration,
        SpecialNote,
        Reservation,
        DocumentManagement,
        VisitingList,
        KarteDaicho,
        Booking,
        PrintRaiinInf,
        RaiinView
    }

    public enum RelationshipEnum
    {
        GF1,
        GM1,
        GF2,
        GM2,
        FA,
        MO,
        MA,
        BB,
        BS,
        LB,
        LS,
        SO,
        DA,
        GC,
        BR,
        OT
    }

    public enum InfoType
    {
        PtHeaderInfo = 0,
        SumaryInfo,
        NotificationInfo,
        Popup
    }
}
