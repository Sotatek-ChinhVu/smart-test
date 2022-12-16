using Domain.Models.DrugDetail;
using Helper.Extension;
using System.Text;
using UseCase.DrugDetailData.ShowMdbByomei;

namespace Interactor.DrugDetailData
{
    public class ShowMdbByomeiInteractor : IShowMdbByomeiInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;

        public ShowMdbByomeiInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public ShowMdbByomeiOutputData Handle(ShowMdbByomeiInputData inputData)
        {
            try
            {
                if (inputData.Level < 0)
                {
                    return new ShowMdbByomeiOutputData(string.Empty, ShowMdbByomeiStatus.InvalidLevel);
                }
                if (inputData.SelectedIndexOfMenuLevel < 0)
                {
                    return new ShowMdbByomeiOutputData(string.Empty, ShowMdbByomeiStatus.InvalidSelectedIndexOfMenuLevel);
                }

                var drugDetailModel = _drugInforRepository.GetDataDrugSeletedTree(inputData.SelectedIndexOfMenuLevel, inputData.Level, inputData.DrugName, inputData.ItemCd, inputData.YJCode);
                string result = ShowMdbByomei(drugDetailModel);
                return new ShowMdbByomeiOutputData(result, ShowMdbByomeiStatus.Successed);
            }
            catch
            {
                return new ShowMdbByomeiOutputData(string.Empty, ShowMdbByomeiStatus.Failed);
            }
        }

        private string ShowMdbByomei(DrugDetailModel drugDetailModel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<html>");
            stringBuilder.Append("<head>");
            stringBuilder.Append("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
            stringBuilder.Append("<style type='text/css'><!--");
            stringBuilder.Append("table {font-family: メイリオ; font-size: 10pt}");
            stringBuilder.Append("--></style>");
            stringBuilder.Append("</head>");
            stringBuilder.Append("<body>");
            stringBuilder.Append("<div id='header' style='display:none'>");

            stringBuilder.Append("</div>");
            if (drugDetailModel.TenMstInf != null)
            {
                foreach (var item in drugDetailModel.TenMstInf)
                {
                    if (item == null) continue;
                    string lineHtml = "<table><tr><td>" + item.Byomei.AsString() + "</td></tr></table>";
                    stringBuilder.Append(lineHtml);
                }
            }

            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");

            return stringBuilder.ToString();
        }
    }
}
