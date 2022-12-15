using Domain.Models.DrugDetail;
using Domain.Models.DrugInfor;
using Helper.Common;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.DrugDetailData;

namespace Interactor.DrugDetailData
{
    public class ShowProductInfInteractor : IGetDrugDetailDataInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;
        public ShowProductInfInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public GetDrugDetailDataOutputData Handle(GetDrugDetailDataInputData inputData)
        {
            if(string.IsNullOrEmpty(inputData.ItemCd))
            {
                return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidItemCd);
            }    

            if(string.IsNullOrEmpty(inputData.YJCode))
            {
                return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidYJCode);
            }    

            var data = _drugInforRepository.GetDataDrugSeletedTree(inputData.SelectedIndexOfMenuLevel, inputData.Level, inputData.DrugName, inputData.ItemCd, inputData.YJCode);

            return new GetDrugDetailDataOutputData(data, GetDrugDetailDataStatus.Successed);
        }

        private void ShowProductInf(int menuIndex, DrugDetailModel data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //var menuList = DrugMenuItemCollection[0].Childrens.ToList();
            using (var tw = new StreamWriter(new FileStream(strFIPATH, FileMode.Create), Encoding.UTF8))
            {
                //First row of file to check this item is created
                stringBuilder.AppendLine("<!--" + data.DrugInfName + "-->");
                stringBuilder.AppendLine("<!DOCTYPE HTML PUBLIC ' -//W3C//DTD HTML 4.0 Transitional//EN'>");

                stringBuilder.AppendLine("<html>");
                stringBuilder.AppendLine("<head>");
                stringBuilder.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
                stringBuilder.AppendLine("<style type='text/css'><!--");
                //
                for (int i = 0; i <= data.MaxLevel; i++)
                {
                    stringBuilder.AppendLine("td." + i.AsString() + " {width: " + (20 * i).AsString() + "px;clear:both}");
                }

                stringBuilder.AppendLine("table {font-family: メイリオ; font-size: 10pt}");
                stringBuilder.AppendLine("--></style></head><body>");

                stringBuilder.AppendLine("<div id='header' style='display:none'>");

                stringBuilder.AppendLine("</div>");

                //SyohinModel
                if (data.SyohinInf != null)
                {
                    tw.WriteLine("<table><tr><td class = 0></td><td><p style='font-weight: bold; font-size:12pt;'>【商品情報】</p></td></tr></table>");
                    tw.WriteLine("<table><tr><td class = 1></td><td>商品名：</td><td>　" + data.SyohinInf.ProductName + "</td></tr>");
                    tw.WriteLine("       <tr><td class = 1></td><td>製剤名：</td><td>　" + data.SyohinInf.PreparationName + "</td></tr>");
                    tw.WriteLine("       <tr><td class = 1></td><td>規格単位：</td><td>　" + data.SyohinInf.Unit + "</td></tr>");
                    tw.WriteLine("       <tr><td class = 1></td><td>製造_輸入会社名：</td><td>　" + data.SyohinInf.Maker + "</td></tr>");
                    tw.WriteLine("       <tr><td class = 1></td><td>販売会社名：</td><td>　" + data.SyohinInf.Vender + "</td></tr></table>");
                }

                //Kikaku Items
                foreach (var kikakuItem in data.KikakuCollection)
                {
                    int level = kikakuItem.DbLevel;
                    string kikakuText = kikakuItem.DrugMenuName;
                    if (level <= 0)
                    {
                        int countMenuItem = menuList.Count;

                        for (int i = 0; i < countMenuItem; i++)
                        {
                            string menuName = menuList[i].MenuName;
                            if (level == menuList[i].DbLevel && kikakuText == menuList[i].RawDrugMenuName)
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
                            tw.Write("<table><tr><td class = " + level + "></td><td>" + sTmp);
                        }
                        else
                        {
                            tw.WriteLine("</td></tr></table>");
                            tw.Write("<table><tr><td class = " + level + "></td><td>" + sTmp);
                        }
                    }
                    else
                    {
                        if (kikakuItem.SeqNo == 1)
                        {
                            tw.Write("<table><tr><td class = " + level + "></td><td>" + kikakuText);
                        }
                        else
                        {
                            tw.WriteLine("</td></tr></table>");
                            if (SetKouban(ref kikakuText))
                            {
                                tw.Write("<table><tr><td class = " + (level) + "></td>" + kikakuText);
                            }
                            else
                            {
                                tw.Write("<table><tr><td class = " + (level) + "></td><td>" + kikakuText);
                            }
                        }
                    }
                }

                tw.WriteLine("</td></tr></table>");
                //tw.WriteLine("<br>");

                bool bSiyoFlg = false;

                //Tenpu Items
                DrugMenuItem currentTenpuItem = null;
                foreach (var tenpuItem in TenpuCollection)
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
                        int countMenuItem = menuList.Count;

                        for (int i = 0; i < countMenuItem; i++)
                        {

                            if (level == menuList[i].DbLevel && tenpuText == menuList[i].RawDrugMenuName)
                            {
                                string menuName = menuList[i].MenuName;
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
                            tw.Write("<table><tr><td class = " + level + "></td><td>" + sTmp);
                        }
                        else
                        {
                            tw.WriteLine("</td></tr></table>");

                            tw.Write("<table><tr><td class = " + level + "></td><td>" + sTmp);
                        }
                    }
                    else
                    {
                        if (tenpuItem.SeqNo == 1)
                        {
                            tw.Write("<table><tr><td class = " + level + "></td><td>" + tenpuText);
                        }
                        else
                        {
                            tw.WriteLine("</td></tr></table>");

                            if (SetKouban(ref tenpuText))
                            {
                                tw.Write("<table><tr><td class = " + level + "></td>" + tenpuText);
                            }
                            else
                            {
                                if (currentTenpuItem == null || currentTenpuItem.DrugMenuName != tenpuItem.DrugMenuName)
                                {
                                    tw.Write("<table><tr><td class = " + level + "></td><td>" + tenpuText);
                                }
                                else
                                {
                                    tw.Write("<table><tr><td class = " + level + "></td><td>");
                                }

                            }
                        }
                    }
                    if (level == 0 || (bSiyoFlg && level == 1))
                    {
                        currentTenpuItem = tenpuItem;
                    }
                }

                tw.WriteLine("</td></tr></table>");
                tw.WriteLine("<div id='footer'>");
                tw.WriteLine("<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>" +
                          "<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>" +
                          "<br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br>");
                tw.WriteLine("</div>");
                //End of html file
                tw.WriteLine("</body>");
                tw.WriteLine("</html>");
                var temp = tw.ToString();
            }

            if (menuIndex > 0)
            {
                DrugInformationSource = path + @"\" + strFIPATH + "#" + (menuList[menuIndex - 1].MenuName.AsInteger()).AsString();
            }
            else
            {
                DrugInformationSource = path + @"\" + strFIPATH;
                string[] lines = System.IO.File.ReadAllLines(DrugInformationSource);

                // Display the file contents by using a foreach loop.
                string str = "";
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    str += line + Environment.NewLine;
                }
            }
        }
    }
}
