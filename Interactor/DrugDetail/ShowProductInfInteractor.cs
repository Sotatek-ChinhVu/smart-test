using Domain.Models.DrugDetail;
using Helper.Common;
using Helper.Extension;
using System.Text;
using System.Text.RegularExpressions;
using UseCase.DrugDetailData.ShowProductInf;

namespace Interactor.DrugDetailData
{
    public class ShowProductInfInteractor : IShowProductInfInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;
        const string CON_KAKO1 = ").";
        const string CON_KAKO2 = "].";
        const string CON_KAKO3 = ".";

        public ShowProductInfInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public ShowProductInfOutputData Handle(ShowProductInfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidHpId);
                }

                if (inputData.SinDate <= 0)
                {
                    return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidSinDate);
                }
                if (inputData.Level < 0)
                {
                    return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidLevel);
                }

                var drugDetailModel = _drugInforRepository.GetDataDrugSeletedTree(0, inputData.Level, inputData.DrugName, inputData.ItemCd, inputData.YJCode);
                var drugMenu = _drugInforRepository.GetDrugMenu(inputData.HpId, inputData.SinDate, inputData.ItemCd).ToList();
                string result = ShowProductInf(drugDetailModel, drugMenu);
                return new ShowProductInfOutputData(result, ShowProductInfStatus.Successed);
            }
            catch
            {
                return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.Failed);
            }
        }

        private string ShowProductInf(DrugDetailModel drugDetailModel, List<DrugMenuItemModel> drugMenus)
        {

            var straightDrugMenus = new List<DrugMenuItemModel>();
            GetAllDrugMenus(ref straightDrugMenus, drugMenus[0].Children);
            StringBuilder stringBuilder = new StringBuilder();
            //First row of file to check this item is created
            stringBuilder.Append("<!--" + drugDetailModel.DrugInfName + "-->");
            stringBuilder.Append("<!DOCTYPE HTML PUBLIC ' -//W3C//DTD HTML 4.0 Transitional//EN'>");

            stringBuilder.Append("<html>");
            stringBuilder.Append("<head>");
            stringBuilder.Append("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
            stringBuilder.Append("<style type='text/css'><!--");
            //
            for (int i = 0; i <= drugDetailModel.MaxLevel; i++)
            {
                stringBuilder.Append("td.level-" + i.AsString() + " {width: " + (20 * i).AsString() + "px;clear:both}");
            }

            stringBuilder.Append("table {font-family: メイリオ; font-size: 10pt}");
            stringBuilder.Append("--></style></head><body>");

            stringBuilder.Append("<div id='header' style='display:none'>");

            stringBuilder.Append("</div>");

            //SyohinModel
            if (drugDetailModel.SyohinInf != null)
            {
                stringBuilder.Append("<table><tr><td class = level-0></td><td><p style='font-weight: bold; font-size:12pt;'>【商品情報】</p></td></tr></table>");
                stringBuilder.Append("<table><tr><td class = level-1></td><td>商品名：</td><td>　" + drugDetailModel.SyohinInf.ProductName + "</td></tr>");
                stringBuilder.Append("       <tr><td class = level-1></td><td>製剤名：</td><td>　" + drugDetailModel.SyohinInf.PreparationName + "</td></tr>");
                stringBuilder.Append("       <tr><td class = level-1></td><td>規格単位：</td><td>　" + drugDetailModel.SyohinInf.Unit + "</td></tr>");
                stringBuilder.Append("       <tr><td class = level-1></td><td>製造_輸入会社名：</td><td>　" + drugDetailModel.SyohinInf.Maker + "</td></tr>");
                stringBuilder.Append("       <tr><td class = level-1></td><td>販売会社名：</td><td>　" + drugDetailModel.SyohinInf.Vender + "</td></tr></table>");
            }

            //Kikaku Items
            foreach (var kikakuItem in drugDetailModel.KikakuCollection)
            {
                int level = kikakuItem.DbLevel;
                string kikakuText = kikakuItem.DrugMenuName;
                if (level <= 0)
                {
                    int countMenuItem = straightDrugMenus.Count;

                    for (int i = 0; i < countMenuItem; i++)
                    {
                        string menuName = straightDrugMenus[i].MenuName;
                        if (level == straightDrugMenus[i].DbLevel && kikakuText == straightDrugMenus[i].RawDrugMenuName)
                        {
                            kikakuText = "<a name='" + menuName + "'><p style='font-weight: bold; font-size:12pt; margin:10px 0 0 0'>" + kikakuText + "</p></a>";
                            break;
                        }
                    }
                }

                if (CheckHyo(kikakuText))
                {
                    string sTmp = MakeTable(kikakuText);
                    if (kikakuItem.SeqNo == 1)
                    {
                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + sTmp);
                    }
                    else
                    {
                        stringBuilder.Append("</td></tr></table>");
                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + sTmp);
                    }
                }
                else
                {
                    if (kikakuItem.SeqNo == 1)
                    {
                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + kikakuText);
                    }
                    else
                    {
                        stringBuilder.Append("</td></tr></table>");
                        if (SetKouban(ref kikakuText))
                        {
                            stringBuilder.Append("<table><tr><td class = level-" + (level) + "></td>" + kikakuText);
                        }
                        else
                        {
                            stringBuilder.Append("<table><tr><td class = level-" + (level) + "></td><td>" + kikakuText);
                        }
                    }
                }
            }

            stringBuilder.Append("</td></tr></table>");
            //stringBuilder.Append("<br>");

            bool bSiyoFlg = false;

            //Tenpu Items
            DrugMenuItemModel? currentTenpuItem = null;
            foreach (var tenpuItem in drugDetailModel.TenpuCollection)
            {
                int level = tenpuItem.DbLevel;
                string tenpuText = tenpuItem.DrugMenuName;
                if (level == 0)
                {
                    if (bSiyoFlg)
                    {
                        bSiyoFlg = false;
                    }
                    else
                    {
                        bSiyoFlg = (tenpuItem.DrugMenuName == "【使用上の注意】" || tenpuItem.DrugMenuName == "【使用上注意】");

                    }
                }

                if (level == 0 || (bSiyoFlg && level == 1))
                {
                    int countMenuItem = straightDrugMenus.Count;

                    for (int i = 0; i < countMenuItem; i++)
                    {

                        if (level == straightDrugMenus[i].DbLevel && tenpuText == straightDrugMenus[i].RawDrugMenuName)
                        {
                            string menuName = straightDrugMenus[i].MenuName;
                            if (level == 0)
                            {
                                tenpuText = "<a name='" + menuName + "'><p style='font-weight: bold;font-size:12pt;margin:20px 0 0 0'>" + tenpuText + "</p></a>";
                            }
                            else if (level == 1)
                            {
                                tenpuText = "<a name='" + menuName + "'><p style='font-weight: bold;font-size:12pt;margin:10px 0 0 0'>" + tenpuText + "</p></a>";
                            }
                            else
                            {
                                tenpuText = "<a name='" + menuName + "'><p style='font-weight: bold;font-size:12pt;'>" + tenpuText + "</p></a>";
                            }
                        }
                    }
                }

                if (CheckHyo(tenpuText))
                {
                    string sTmp = MakeTable(tenpuText);
                    if (tenpuItem.SeqNo == 1)
                    {
                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + sTmp);
                    }
                    else
                    {
                        stringBuilder.Append("</td></tr></table>");

                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + sTmp);
                    }
                }
                else
                {
                    if (tenpuItem.SeqNo == 1)
                    {
                        stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + tenpuText);
                    }
                    else
                    {
                        stringBuilder.Append("</td></tr></table>");

                        if (SetKouban(ref tenpuText))
                        {
                            stringBuilder.Append("<table><tr><td class = level-" + level + "></td>" + tenpuText);
                        }
                        else
                        {
                            if (currentTenpuItem == null || currentTenpuItem.DrugMenuName != tenpuItem.DrugMenuName)
                            {
                                stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>" + tenpuText);
                            }
                            else
                            {
                                stringBuilder.Append("<table><tr><td class = level-" + level + "></td><td>");
                            }

                        }
                    }
                }
                if (level == 0 || (bSiyoFlg && level == 1))
                {
                    currentTenpuItem = tenpuItem;
                }
            }

            stringBuilder.Append("</td></tr></table>");
            stringBuilder.Append("<div id='footer'>");
            stringBuilder.Append("<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>" +
                      "<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>" +
                      "<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>");
            stringBuilder.Append("</div>");
            //End of html file
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");

            return stringBuilder.ToString();
        }

        private void GetAllDrugMenus(ref List<DrugMenuItemModel> straightDrugMenus, List<DrugMenuItemModel> drugMenus)
        {
            if (drugMenus.Count > 0)
            {
                foreach (var item in drugMenus)
                {
                    straightDrugMenus.Add(item);
                    GetAllDrugMenus(ref straightDrugMenus, item.Children);
                }
            }
        }
        private bool CheckHyo(string stmp)
        {
            string CON_KAISI = "（表開始）";
            string CON_DAI = "［表題］";
            string CON_SYU = "（表終了）";
            string CON_CHU = "［表脚注］!";
            string CON_KAI = "▼";
            string CON_PAI = "Π";
            string CON_MAE = "［前文］!";
            int val = -1;
            if (stmp.IndexOf("!") >= 0)
                val = stmp.IndexOf("!");
            if (stmp.IndexOf(CON_KAISI) >= 0)
                val = stmp.IndexOf(CON_KAISI);
            if (stmp.IndexOf(CON_DAI) >= 0)
                val = stmp.IndexOf(CON_DAI);
            if (stmp.IndexOf(CON_SYU) >= 0)
                val = stmp.IndexOf(CON_SYU);
            if (stmp.IndexOf(CON_CHU) >= 0)
                val = stmp.IndexOf(CON_CHU);
            if (stmp.IndexOf(CON_KAI) >= 0)
                val = stmp.IndexOf(CON_KAI);
            if (stmp.IndexOf(CON_PAI) >= 0)
                val = stmp.IndexOf(CON_PAI);
            if (stmp.IndexOf(CON_MAE) >= 0)
                val = stmp.IndexOf(CON_MAE);
            return (val >= 0);
        }

        private string MakeTable(string sstr)
        {
            string CON_KAISI = "（表開始）";
            string CON_DAI = "［表題］";
            string CON_SYU = "（表終了）";
            string CON_CHU = "［表脚注］!";
            string CON_KAI = "▼";
            string CON_PAI = "Π";
            string CON_MAE = "［前文］!";
            string CON_ATO = "［後文］!";

            string rs = sstr;
            int num = sstr.IndexOf(CON_MAE);
            int num2 = sstr.IndexOf(CON_ATO);
            if (num >= 0)
            {
                rs = rs.Replace(CON_MAE, "");
            }
            else if (num2 >= 0)
            {
                rs = rs.Replace(CON_ATO, "");

            }
            else if (rs.IndexOf(CON_KAISI) >= 0)
            {
                if (sstr.IndexOf(CON_DAI) >= 0)
                {
                    rs = rs.Replace(CON_DAI + "!", "<table border='1' cellspacing='0' cellpadding='5'><caption>");
                    rs = rs.Replace(CON_KAISI, "</caption>");
                }
                else
                {
                    rs = rs.Replace(CON_KAISI, "<table border='1' cellspacing='0' cellpadding='5'><caption>");
                }
            }
            rs = rs.Replace(CON_CHU, "");
            //replace 1st occurrence
            var regex = new Regex(Regex.Escape("!"));
            rs = regex.Replace(rs, "<tr><td>", 1);
            //rs = rs.Replace("!", "<tr><td>");
            rs = rs.Replace(":", "</td><td>");
            rs = rs.Replace("!", "</td></tr><tr><td>");
            rs = rs.Replace(CON_KAI, "<br>");
            rs = rs.Replace(CON_PAI, "<br>");
            rs = rs.Replace(CON_SYU, "</td></tr></table>");
            rs = rs.Replace("<td></td>", "<td>&nbsp;</td>");
            //TODO
            return rs;
        }

        private bool SetKouban(ref string sstr)
        {
            bool res = false;
            int ipos1;
            int ipos2;
            int ipos3;
            string tmpstr;
            string cStr = CIUtil.CDCopy(sstr, 1, 5);
            ipos1 = cStr.IndexOf(CON_KAKO1) + 1;
            ipos2 = cStr.IndexOf(CON_KAKO2) + 1;
            ipos3 = cStr.IndexOf(CON_KAKO3) + 1;
            if (CIUtil.StrToIntDef(CIUtil.CiCopyStrWidth(sstr, ipos3 + 1, 1), -1) != -1)
            {
                ipos3 = 0;
            }

            //   項目番号と内容を分ける

            if (
                (new[] { 2, 3, 4, }.Contains(ipos1) && (ipos2 == 0) && (ipos3 >= 3))////「).」の場合
                || ((ipos1 == 0) && (new[] { 3, 4 }.Contains(ipos2)) && (ipos3 >= 4))//　「].」の場合
                || ((ipos1 == 0) && (ipos2 == 0) && (new[] { 2, 3 }.Contains(ipos3)))
                )
            {
                int MecsStringWidth = CIUtil.MecsStringWidth(sstr); //TODO - MecsStringWidth(sstr)
                tmpstr = "<td valign='top'>" + CIUtil.CiCopyStrWidth(sstr, 1, ipos3) + "</td><td>" + CIUtil.CiCopyStrWidth(sstr, ipos3 + 1, MecsStringWidth - ipos3);
                res = true;
                sstr = tmpstr;
            }
            return res;
        }
    }
}
