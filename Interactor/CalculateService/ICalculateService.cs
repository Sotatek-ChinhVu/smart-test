namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        Task<string> CallCalculate(string apiUrl, object inputData);
        object GetCalculateData(string apiUrl, object inputData);
    }
}
