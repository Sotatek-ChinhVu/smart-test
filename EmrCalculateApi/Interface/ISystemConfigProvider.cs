namespace EmrCalculateApi.Interface
{
    public interface ISystemConfigProvider
    {
        int GetJibaiJunkyo();

        int GetChokiFutan();

        int GetChokiDateRange();

        int GetRoundKogakuPtFutan();

        double GetJibaiRousaiRate();

        int GetChokiTokki();

        int GetReceKyufuKisai();

        int GetReceKyufuKisai2();
        int GetReceiptTantoIdTarget();
        int GetReceiptKaIdTarget();

        int GetHokensyuHandling();
        int GetCalcCheckKensaDuplicateLog();
        int GetHoukatuHaihanCheckMode();
        int GetHoukatuHaihanLogputMode();
        int GetHoukatuHaihanSPJyokenLogputMode();
        int GetHoumonKangoSaisinHokatu();
        int GetKensaMarumeBuntenKokuho();
        int GetKensaMarumeBuntenSyaho();
        int GetReceNoDspComment();
        int GetOutDrugYohoDsp();
        int GetSyohoRinjiDays();
        int GetRousaiRecedenLicense();
        string GetRousaiRecedenStartYm();
        int GetAfterCareRecedenLicense();
        string GetAfterCareRecedenStartYm();
        int GetDrugPid();
        int GetSyouniCounselingCheck();
        int GetInDrugYohoComment();
        int GetCalcAutoComment();
        string GetNaraFukusiReceCmtStartDate();
        int GetNaraFukusiReceCmt();
        int GetReceiptCommentTenCount();
        int GetReceiptOutDrgSinId();
        int GetSameRpMerge();
    }
}
