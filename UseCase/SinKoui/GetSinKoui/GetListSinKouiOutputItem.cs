using Domain.Models.SinKoui;

namespace UseCase.SinKoui.GetSinKoui
{
    public class GetListSinKouiOutputItem
    {
        public GetListSinKouiOutputItem(KaikeiInfModel kaikeiInfModel)
        {
            SinYmBinding = kaikeiInfModel.SinYmBinding;
        }

        public string SinYmBinding { get; private set; }
    }
}
