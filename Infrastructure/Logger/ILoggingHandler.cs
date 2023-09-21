namespace Infrastructure.Logger
{
    public interface ILoggingHandler : IDisposable
    {
        Task WriteLogStartAsync(string message = "");

        Task WriteLogExceptionAsync(Exception exception, string message = "");

        Task WriteLogEndAsync(string message = "");

        Task WriteLogMessageAsync(string message);
    }
}
