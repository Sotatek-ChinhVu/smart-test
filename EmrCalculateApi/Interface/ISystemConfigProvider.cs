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
    }
}
