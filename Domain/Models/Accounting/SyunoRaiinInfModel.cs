namespace Domain.Models.Accounting
{
    public class SyunoRaiinInfModel
    {
        public SyunoRaiinInfModel(int status, string kaikeiTime, int uketukeSbt)
        {
            Status = status;
            KaikeiTime = kaikeiTime;
            UketukeSbt = uketukeSbt;
        }

        public SyunoRaiinInfModel()
        {
            KaikeiTime = string.Empty;
        }
        public int Status { get; private set; }
        public string KaikeiTime { get; private set; }
        public int UketukeSbt { get; private set; }
    }
}
