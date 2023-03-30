using System.ComponentModel;
using System.Net;

namespace Interactor.CalculateService
{
    public interface ICalcultateCustomerService
    {
        /// <summary>
        /// Call Httpclient Customer Calculation app
        /// </summary>
        /// <typeparam name="T">TData get from res HttpClient</typeparam>
        /// <param name="type">type calculate</param>
        /// <param name="input">object json</param>
        /// <returns></returns>
        Task<CalcultateCustomerResponse<T>> RunCaculationPostAsync<T>(TypeCalculate type, object input);
    }

    public class CalcultateCustomerResponse<T>
    {
        public CalcultateCustomerResponse(T data, HttpStatusCode status, bool isSuccess)
        {
            Data = data;
            Status = status;
            IsSuccess = isSuccess;
        }

        public T Data { get; private set; }

        public HttpStatusCode Status { get; private set; }

        public bool IsSuccess { get; private set; }
    }

    public enum TypeCalculate
    {
        [Description("ReceFutan/ReceFutanCalculateMain")]
        ReceFutanCalculateMain,
        [Description("Calculate/RunCalculateMonth")]
        RunCalculateMonth,
        [Description("Receden/GetRecedenData")]
        GetRecedenData
    }
}
