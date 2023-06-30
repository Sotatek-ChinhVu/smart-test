using Domain.Models.SinKoui;
using UseCase.SinKoui.GetSinKoui;

namespace EmrCloudApi.Responses.SinKoui
{
    public class GetSinKouiResponse
    {
        public GetSinKouiResponse(List<string> sinYmBindings)
        {
            SinYmBindings = sinYmBindings;
        }

        public List<string> SinYmBindings { get; private set; }
    }
}
