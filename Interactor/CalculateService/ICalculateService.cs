namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        public Task<string> CallCalculate(string apiUrl, string jsonContent);
    }
}
