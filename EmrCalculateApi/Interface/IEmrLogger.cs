namespace EmrCalculateApi.Interface
{
    public interface IEmrLogger
    {
        void WriteLogError(object className, string functionName, Exception exception);

        void WriteLogEnd(object className, string functionName, string message);

        void WriteLogStart(object className, string functionName, string message);
        void WriteLogMsg(object className, string functionName, string message);
    }
}
