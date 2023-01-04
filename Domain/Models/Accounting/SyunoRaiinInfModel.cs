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

        public int Status { get; set; }
        public string KaikeiTime { get; set; }
        public int UketukeSbt { get; set; }
    }
}
