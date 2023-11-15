using Domain.Models.DrugDetail;
using Domain.Models.DrugInfor;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Interfaces;
using System.Text.RegularExpressions;
using System.Text;

namespace Interactor.DrugInfor.CommonDrugInf;

public class GetCommonDrugInf : IGetCommonDrugInf
{
    private readonly IDrugInforRepository _drugInforRepository;
    private readonly IDrugDetailRepository _drugDetailRepository;
    private readonly IAmazonS3Service _amazonS3Service;

    const string CON_KAKO1 = ").";
    const string CON_KAKO2 = "].";
    const string CON_KAKO3 = ".";

    public GetCommonDrugInf(IDrugInforRepository drugInforRepository, IAmazonS3Service amazonS3Service, IDrugDetailRepository drugDetailRepository)
    {
        _drugInforRepository = drugInforRepository;
        _amazonS3Service = amazonS3Service;
        _drugDetailRepository = drugDetailRepository;
    }

    #region GetDrugInfor
    public DrugInforModel GetDrugInforModel(int hpId, int sinDate, string itemCd)
    {
        var data = _drugInforRepository.GetDrugInfor(hpId, sinDate, itemCd);
        var listPicHou = new List<string>();
        var listPicZai = new List<string>();

        if (!string.IsNullOrEmpty(data.OtherPicZai))
        {
            data.PathPicZai = _amazonS3Service.GetAccessBaseS3() + data.OtherPicZai;
        }
        else
        {
            data.PathPicZai = GetPathImagePic(data.YjCode, data.DefaultPathPicZai, data.CustomPathPicZai, listPicZai);
        }

        if (!string.IsNullOrEmpty(data.OtherPicHou))
        {
            data.PathPicHou = _amazonS3Service.GetAccessBaseS3() + data.OtherPicHou;
        }
        else
        {
            data.PathPicHou = GetPathImagePic(data.YjCode, data.DefaultPathPicHou, data.CustomPathPicHou, listPicHou);
        }

        //set list image
        data.ListPicHou = listPicHou;
        data.ListPicZai = listPicZai;
        return data;
    }

    private string GetPathImagePic(string yjCode, string filePath, string customPath, List<string> listPic)
    {
        var tasks = new List<Task<(bool vavlid, string key)>>();

        string _picStr = " ABCDEFGHIJZ";
        for (int i = 0; i < _picStr.Length - 1; i++)
        {
            if (!string.IsNullOrEmpty(yjCode))
            {
                string imgFile = (filePath + yjCode + _picStr[i].AsString()).Trim() + ".jpg";
                tasks.Add(_amazonS3Service.S3FilePathIsExists(imgFile));
            }
        }
        string customImage = customPath + yjCode + "Z.jpg";
        tasks.Add(_amazonS3Service.S3FilePathIsExists(customImage));

        var rs = Task.WhenAll(tasks).Result;
        listPic.AddRange(rs.Where(x => x.vavlid).Select(x => _amazonS3Service.GetAccessBaseS3() + x.key));
        if (listPic.Count > 0)
        {
            // Image default 
            return listPic[0] ?? string.Empty;
        }
        else
        {
            //Image default Empty
            return string.Empty;
        }
    }
    #endregion

    #region ShowProductInf
    public string ShowProductInf(int hpId, int sinDate, string itemCd, int level, string drugName, string yJCode)
    {
        var drugDetailModel = _drugDetailRepository.GetDataDrugSeletedTree(0, level, drugName, itemCd, yJCode);
        var drugMenu = _drugDetailRepository.GetDrugMenu(hpId, sinDate, itemCd).ToList();
        string result = ShowProductInf(drugDetailModel, drugMenu);
        return result;
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
        rs = rs.Replace(":", "</td><td>");
        rs = rs.Replace("!", "</td></tr><tr><td>");
        rs = rs.Replace(CON_KAI, "<br>");
        rs = rs.Replace(CON_PAI, "<br>");
        rs = rs.Replace(CON_SYU, "</td></tr></table>");
        rs = rs.Replace("<td></td>", "<td>&nbsp;</td>");
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

    #endregion

    #region ShowKanjaMuke
    public string ShowKanjaMuke(string itemCd, int level, string drugName, string yJCode)
    {
        var drugDetailModel = _drugDetailRepository.GetDataDrugSeletedTree(1, level, drugName, itemCd, yJCode);
        string result = ShowKanjaMuke(drugDetailModel);
        return result;
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
    #endregion

    #region ShowMdbByomei
    public string ShowMdbByomei(string itemCd, int level, string drugName, string yJCode)
    {
        var drugDetailModel = _drugDetailRepository.GetDataDrugSeletedTree(2, level, drugName, itemCd, yJCode);
        string result = ShowMdbByomei(drugDetailModel);
        return result;
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
    #endregion

    public void ReleaseResources()
    {
        _drugInforRepository.ReleaseResource();
    }
}
