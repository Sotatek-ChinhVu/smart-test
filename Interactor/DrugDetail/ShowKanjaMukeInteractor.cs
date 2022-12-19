using Domain.Models.DrugDetail;
using System.Text;
using UseCase.DrugDetailData.ShowKanjaMuke;

namespace Interactor.DrugDetailData
{
    public class ShowKanjaMukeInteractor : IShowKanjaMukeInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;

        public ShowKanjaMukeInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public ShowKanjaMukeOutputData Handle(ShowKanjaMukeInputData inputData)
        {
            try
            {
                if (inputData.Level < 0)
                {
                    return new ShowKanjaMukeOutputData(string.Empty, ShowKanjaMukeStatus.InvalidLevel);
                }

                var drugDetailModel = _drugInforRepository.GetDataDrugSeletedTree(1, inputData.Level, inputData.DrugName, inputData.ItemCd, inputData.YJCode);
                string result = ShowKanjaMuke(drugDetailModel);
                return new ShowKanjaMukeOutputData(result, ShowKanjaMukeStatus.Successed);
            }
            catch
            {
                return new ShowKanjaMukeOutputData(string.Empty, ShowKanjaMukeStatus.Failed);
            }
        }

        private string ShowKanjaMuke(DrugDetailModel drugDetailModel)
        {
            int propertyCode = 0;
            StringBuilder stringBuilder = new StringBuilder();
            //Create header
            stringBuilder.Append("<html>");
            stringBuilder.Append("<head>");
            stringBuilder.Append("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
            stringBuilder.Append("<style type='text/css'><!--");
            stringBuilder.Append("table {font-family: メイリオ; font-size: 10pt}");
            stringBuilder.Append("--></style></head>");
            stringBuilder.Append("<head>");
            stringBuilder.Append("	<style type='text/css'>");
            stringBuilder.Append("	<!--");
            stringBuilder.Append("		td.0 {width: 20px;clear:both}");
            stringBuilder.Append("		td.1 {width: 40px;clear:both}");
            stringBuilder.Append("		td.2 {width: 60px;clear:both}");
            stringBuilder.Append("	-->");
            stringBuilder.Append("	</style>");
            stringBuilder.Append("</head>");
            stringBuilder.Append("<body>");
            stringBuilder.Append("<div id='header' style='display:none'>");
            //string imgSpace = MakeHeaderPrint();
            //stringBuilder.Append(imgSpace);
            stringBuilder.Append("</div>");
            //Create body
            stringBuilder.Append("<table><tr><td></td><td><p style='font-weight: bold; font-size:12pt;'>【薬剤識別】</p></td></tr></table>");

            if (drugDetailModel.YakuInf != null)
            {
                stringBuilder.Append("<table><tr><td class = 0></td><td>剤形：　　　　　　" +
                               drugDetailModel.YakuInf.Form +
                               "</td></tr></table>");
                stringBuilder.Append("<table><tr><td class = 0></td><td>色調：　　　　　　" +
                          drugDetailModel.YakuInf.Color +
                          "</td></tr></table>");

                stringBuilder.Append("<table><tr><td class = 0></td><td>本体記号：　　　" +
                           drugDetailModel.YakuInf.Mark +
                            "</td></tr></table>");
                stringBuilder.Append("<br>");
                stringBuilder.Append("<table><tr><td></td><td><p style='font-weight: bold; font-size:12pt;'>【効能・効果】</p></td></tr></table>");
                stringBuilder.Append("<table><tr><td class = 0></td><td>詳しい説明</td></tr></table>");

                if (drugDetailModel.YakuInf.KonoDetailCmt != "")
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                             drugDetailModel.YakuInf.KonoDetailCmt +
                             "</td></tr></table>");
                }
                else
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
                }

                stringBuilder.Append("<table><tr><td class = 0></td><td>簡単な説明</td></tr></table>");

                if (drugDetailModel.YakuInf.KonoSimpleCmt != "")
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                             drugDetailModel.YakuInf.KonoSimpleCmt +
                              "</td></tr></table>");
                }
                else
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
                }

                stringBuilder.Append("<br>");
                stringBuilder.Append("<table><tr><td></td><td><p style='font-weight: bold; font-size:12pt;'>【副作用】</p></td></tr></table>");
                stringBuilder.Append("<table><tr><td class = 0></td><td>主な副作用</td></tr></table>");

                if (drugDetailModel.YakuInf.FukusayoCmt != "")
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                             drugDetailModel.YakuInf.FukusayoCmt +
                              "</td></tr></table>");
                }
                else
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
                }
            }

            stringBuilder.Append("<table><tr><td class = 0></td><td>直ちに対処すべき副作用</td></tr></table>");

            if (drugDetailModel.FukuInf == null || drugDetailModel.FukuInf.Count == 0)
            {
                stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
            }
            else
            {
                foreach (var fukuModel in drugDetailModel.FukuInf)
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                                fukuModel.FukusayoCd +
                                "</td></tr></table>");
                }
            }

            stringBuilder.Append("<table><tr><td class = 0></td><td>重大な副作用の初期症状</td></tr></table>");

            if (drugDetailModel.SyokiInf == null)
            {
                stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
            }
            else
            {
                if (drugDetailModel.SyokiInf.FukusayoInitCmt != "")
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                            drugDetailModel.SyokiInf.FukusayoInitCmt +
                            "</td></tr></table>");
                }
                else
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
                }
            }

            stringBuilder.Append("<br>");
            stringBuilder.Append("<table><tr><td></td><td><p style='font-weight: bold; font-size:12pt;'>【相互作用】</p></td></tr></table>");

            if (drugDetailModel.SougoInf == null || drugDetailModel.SougoInf.Count == 0)
            {
                stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
            }
            else
            {
                foreach (var sougoModel in drugDetailModel.SougoInf)
                {
                    stringBuilder.Append("<table><tr><td class = 0></td><td>" +
                              sougoModel.InteractionPatCd +
                               "</td></tr></table>");
                }
            }

            stringBuilder.Append("<br>");
            stringBuilder.Append("<table><tr><td></td><td><p style='font-weight: bold; font-size:12pt;'>【注意事項】</p></td></tr></table>");

            foreach (var chuiModel in drugDetailModel.ChuiInf)
            {
                if (propertyCode != chuiModel.PropertyCd)
                {
                    stringBuilder.Append("<table><tr><td class = 0></td><td>" +
                              chuiModel.Property +
                                "</td></tr></table>");
                    propertyCode = chuiModel.PropertyCd;
                }
                if (chuiModel.PrecautionCmt != "")
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td>" +
                               chuiModel.PrecautionCmt +
                               "</td></tr></table>");
                }
                else
                {
                    stringBuilder.Append("<table><tr><td class = 1></td><td><br></td></tr></table>");
                }
            }

            //End of html file
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");

            return stringBuilder.ToString();
        }
    }
}
