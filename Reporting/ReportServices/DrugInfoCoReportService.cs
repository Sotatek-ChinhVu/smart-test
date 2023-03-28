using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.DrugInfo.Model;
using Reporting.Interface;
using System.Drawing;

namespace Reporting.ReportServices
{
    public class DrugInfoCoReportService : RepositoryBase, IDrugInfoCoReportService
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ICoDrugInfFinder _coDrugInfFinder;

        public DrugInfoCoReportService(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository, ICoDrugInfFinder coDrugInfFinder) : base(tenantProvider)
        {
            _systemConfRepository = systemConfRepository;
            _coDrugInfFinder = coDrugInfFinder;
            OrderInfoModels = new();
        }

        int ConfigType = 0;
        int SelectedFormType = 0;
        DrugInfoModel basicInfo = new DrugInfoModel();
        List<OrderInfoModel> OrderInfoModels { get; set; }
        List<DrugInfoModel> DrugInfoModels = new List<DrugInfoModel>();

        public (ReportType, List<DrugInfoModel>) SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo)
        {
            basicInfo = _coDrugInfFinder.GetBasicInfo(hpId, ptId, sinDate);
            //   LoadPathConf();

            ConfigType = (int)_systemConfRepository.GetSettingValue(92004, 1, hpId); // 0,1 - 1 Pic; 2 - 2 Pics; 3- No Pic

            SelectedFormType = (int)_systemConfRepository.GetSettingValue(92004, 17, hpId);

            OrderInfoModels = _coDrugInfFinder.GetOrderByRaiinNo(raiinNo);

            foreach (var orderInfoModel in OrderInfoModels)
            {
                SetupPrintData(hpId, orderInfoModel);
            }

            var reportType = SelectFormType(ConfigType, SelectedFormType);

            return (reportType, DrugInfoModels);
        }

        private ReportType SelectFormType(int configType, int configFormType)
        {
            if (configFormType == 1)
            {
                switch (configType)
                {
                    case 0:
                    case 1:
                        return ReportType.DrgInfType2_1;

                    case 2:
                        return ReportType.DrgInfType2_2;

                    case 3:
                        return ReportType.DrgInfType2_3;

                }
            }
            else
            {
                switch (configType)
                {
                    case 0:
                    case 1:
                        return ReportType.DrgInf1;

                    case 2:
                        return ReportType.DrgInf2;

                    case 3:
                        return ReportType.DrgInf3;

                }
            }

            return ReportType.DrgInf1;
        }

        private void SetupPrintData(int hpId, OrderInfoModel orderInfoModel)
        {

            var usage = orderInfoModel.OrderInfDetailCollection.Where(o => o.YohoKbn == 1).FirstOrDefault();
            var jikochu = orderInfoModel.OrderInfDetailCollection.Where(o => o.SinKouiKbn == 28).FirstOrDefault();
            if (usage == null && jikochu == null) return;
            var drugs = orderInfoModel.OrderInfDetailCollection.Where(o => new[] { 20, 30 }.Contains(o.SinKouiKbn)).ToList();
            foreach (var drug in drugs)
            {
                DrugInfoModel drugInfoModel = new DrugInfoModel();
                drugInfoModel.DrgName = drug.ItemName;

                //Main usage
                drugInfoModel.Usage = jikochu != null ? "自己注射" : usage.ItemName;
                //2st usage
                var subUsage = orderInfoModel.OrderInfDetailCollection.Where(o => o.YohoKbn == 2).FirstOrDefault();
                if (subUsage != null)
                {
                    drugInfoModel.Usage2 = subUsage.ItemName;
                }

                drugInfoModel.DrgKbn = orderInfoModel.OdrKouiKbn;//TODO

                string yjCd = _coDrugInfFinder.GetYJCode(drug.ItemCd);

                var singleDosageMstCollection = _coDrugInfFinder.GetSingleDosageMstCollection(hpId, drug.UnitName);

                drugInfoModel.UnitName = drug.UnitName;
                //
                if (drug.Suryo > 0 && !string.IsNullOrEmpty(drug.UnitName))
                {
                    drugInfoModel.Amount = drug.Suryo.AsString() + drug.UnitName;
                }

                if (usage != null)
                {
                    drugInfoModel.UsageSpan = usage.Suryo + usage.UnitName;
                    string itemCd = usage.ItemCd;

                    var tenMst = _coDrugInfFinder.GetTenMstModel(itemCd);

                    if (tenMst == null) return;

                    int usageAll = tenMst.Rise + tenMst.Morning + tenMst.DayTime + tenMst.Evening + tenMst.Sleep;

                    double wkFloat = 0;

                    if (usageAll > 0)
                    {
                        // 総回数で割る
                        wkFloat = drug.Suryo / usageAll;
                    }

                    //Rise
                    if (tenMst.Rise == 0)//0は空白
                    {
                        drugInfoModel.UsageSign1 = "";
                    }
                    else if (singleDosageMstCollection == null || singleDosageMstCollection.Count == 0)
                    {
                        drugInfoModel.UsageSign1 = "●";// 剤形マスタ未登録は●
                    }
                    else
                    {
                        string usageSignVal = ((wkFloat * tenMst.Rise * 100) / 100).AsString();
                        drugInfoModel.UsageSign1 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                    }

                    //Morning
                    if (tenMst.Morning == 0)
                    {
                        drugInfoModel.UsageSign2 = "";
                    }
                    else if (singleDosageMstCollection == null || singleDosageMstCollection.Count == 0)
                    {
                        drugInfoModel.UsageSign2 = "●";// 剤形マスタ未登録は●
                    }
                    else
                    {
                        string usageSignVal = ((wkFloat * tenMst.Morning * 100) / 100).AsString();
                        drugInfoModel.UsageSign2 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                    }

                    //DayTime
                    if (tenMst.DayTime == 0)
                    {
                        drugInfoModel.UsageSign3 = "";
                    }
                    else if (singleDosageMstCollection == null || singleDosageMstCollection.Count == 0)
                    {
                        drugInfoModel.UsageSign3 = "●";// 剤形マスタ未登録は●
                    }
                    else
                    {
                        string usageSignVal = ((wkFloat * tenMst.DayTime * 100) / 100).AsString();
                        drugInfoModel.UsageSign3 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                    }

                    //Evening
                    if (tenMst.Evening == 0)
                    {
                        drugInfoModel.UsageSign4 = "";
                    }
                    else if (singleDosageMstCollection == null || singleDosageMstCollection.Count == 0)
                    {
                        drugInfoModel.UsageSign4 = "●";// 剤形マスタ未登録は●
                    }
                    else
                    {
                        string usageSignVal = ((wkFloat * tenMst.Evening * 100) / 100).AsString();
                        drugInfoModel.UsageSign4 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                    }

                    //Sleep
                    if (tenMst.Sleep == 0)
                    {
                        drugInfoModel.UsageSign5 = "";
                    }
                    else if (singleDosageMstCollection == null || singleDosageMstCollection.Count == 0)
                    {
                        drugInfoModel.UsageSign5 = "●";// 剤形マスタ未登録は●
                    }
                    else
                    {
                        string usageSignVal = ((wkFloat * tenMst.Sleep * 100) / 100).AsString();
                        drugInfoModel.UsageSign5 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                    }
                }

                drugInfoModel.IntAge = basicInfo.IntAge;
                drugInfoModel.Sex = basicInfo.Sex;

                //ConfigType=3: No image
                if (ConfigType != 3)
                {
                    SetupPrintImage(hpId, drug, drugInfoModel, yjCd);
                }

                if (SelectedFormType == 1)
                {
                    SetupDrugDocumentType2(ConfigType, hpId, drug, drugInfoModel);
                }
                else
                {
                    SetupDrugDocument(ConfigType, hpId, drug, drugInfoModel);
                }
                if ((int)_systemConfRepository.GetSettingValue(92004, 18, hpId) == 0) //PrintDrugCommentSetting
                {
                    SetupDrugComment(orderInfoModel, drug, drugInfoModel);
                }
                else
                {
                    drugInfoModel.DrgComment = string.Empty;
                }
                if (jikochu != null)
                {
                    SetupUsageComment(orderInfoModel, jikochu, drugInfoModel);
                }
                else
                {
                    SetupUsageComment(orderInfoModel, usage, drugInfoModel);
                }
                DrugInfoModels.Add(drugInfoModel);
            }

        }

        private void SetupPrintImage(int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel, string yjCd)
        {

            var images = _coDrugInfFinder.GetProductImages(hpId, orderInfDetailModel.ItemCd);
            string YJCode = yjCd;
            if (images == null || images.Count == 0)
            {
                //Pic House
                //   GetDefaultImage(drugInfoModel, YJCode, 1);
                //Pic Zai
                //   GetDefaultImage(drugInfoModel, YJCode, 0);
                return;
            }

            var picHou = images.Where(i => i.ImageType == 1).FirstOrDefault();
            if (picHou != null)
            {
                drugInfoModel.PicHou = picHou.FileName ?? string.Empty;
            }
            else
            {
                //  GetDefaultImage(drugInfoModel, YJCode, 1);
            }

            var picZai = images.Where(i => i.ImageType == 0).FirstOrDefault();
            if (picZai != null)
            {
                drugInfoModel.PicZai = picZai.FileName ?? string.Empty;
            }
            else
            {
                //  GetDefaultImage(drugInfoModel, YJCode, 0);
            }

        }

        //private void GetDefaultImage(DrugInfoModel drugInfoModel, string yjCd, int imageType)
        //{
        //    if (imageType == 0)
        //    {
        //        //Pic Zai
        //        for (int i = 0; i < _picStr.Length - 1; i++)
        //        {
        //            string imgFile = (_defaultPicZai + yjCd + _picStr[i].AsString()).Trim() + ".jpg";
        //            if (CIUtil.IsFileExisting(imgFile))
        //            {
        //                drugInfoModel.PicZai = (yjCd + _picStr[i].AsString()).Trim() + ".jpg";
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //Pic House
        //        for (int i = 0; i < _picStr.Length - 1; i++)
        //        {
        //            string imgFile = (_defaultPicHou + yjCd + _picStr[i].AsString()).Trim() + ".jpg";
        //            if (CIUtil.IsFileExisting(imgFile))
        //            {
        //                drugInfoModel.PicHou = (yjCd + _picStr[i].AsString()).Trim() + ".jpg";
        //                break;
        //            }
        //        }
        //    }

        //}

        private void SetupDrugDocumentType2(int reportType, int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel)
        {

            int age = drugInfoModel.IntAge;
            int gender = drugInfoModel.Sex == "M" ? 1 : 2;
            var drugInfs = _coDrugInfFinder.GetDrugInfo(hpId, orderInfDetailModel.ItemCd, age, gender);

            if (drugInfs == null) return;

            int nLen = 60;

            if (reportType == 3)
            {
                nLen = 80;
            }

            List<DocumentLine> tText = new List<DocumentLine>();

            tText.AddRange(GetListDocumentLine(drugInfs.Where(item => item.InfKbn == 1).ToList(), 1, nLen));
            tText.AddRange(GetListDocumentLine(drugInfs.Where(item => item.InfKbn != 1).ToList(), 0, nLen));

            drugInfoModel.Tyui = tText;

        }


        private List<DocumentLine> GetListDocumentLine(List<DrugInf> drugInfs, int infKbn, int nLen)
        {
            List<DocumentLine> tText = new List<DocumentLine>();

            string wsBuf = "";
            string sSymbol = "";
            if (infKbn == 1)
            {
                sSymbol = "●";
            }
            else
            {
                sSymbol = "☆";
            }

            foreach (var drugInf in drugInfs)
            {
                var listItem = drugInf.DrugInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var item in listItem)
                {
                    wsBuf += item;
                }
            }

            if (string.IsNullOrEmpty(wsBuf))
            {
                return tText;
            }

            wsBuf = sSymbol + wsBuf;

            int tPos = 0;
            var width = CIUtil.MecsStringWidth(wsBuf);
            while (tPos <= width)
            {
                string sBuf1 = CIUtil.CiCopyStrWidth(wsBuf, tPos + 1, nLen, 0);
                if (!string.IsNullOrEmpty(sBuf1))
                {
                    DocumentLine documentLine = new DocumentLine();
                    documentLine.Text = sBuf1;
                    if (infKbn == 1)
                    {
                        documentLine.Color = Color.Blue;
                    }
                    else
                    {
                        documentLine.Color = Color.Black;
                    }

                    tText.Add(documentLine);
                }

                tPos = tPos + nLen;
            }

            return tText;
        }

        private void SetupDrugDocument(int reportType, int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel)
        {
            int age = drugInfoModel.IntAge;
            int gender = drugInfoModel.Sex == "M" ? 1 : 2;
            var drugInfs = _coDrugInfFinder.GetDrugInfo(hpId, orderInfDetailModel.ItemCd, age, gender);

            if (drugInfs == null) return;

            int nLen = 72;

            if (reportType == 3)
            {
                nLen = 86;
            }

            List<DocumentLine> tText = new List<DocumentLine>();

            foreach (var drugInf in drugInfs)
            {
                string wsBuf = drugInf.DrugInfo ?? string.Empty;
                string sSymbol = "";
                if (drugInf.InfKbn == 1)
                {
                    sSymbol = "●";
                }
                else
                {
                    sSymbol = "☆";
                }

                var listItem = wsBuf.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var item in listItem)
                {

                    if (CIUtil.MecsStringWidth(item) > nLen)
                    {
                        int tPos = 0;
                        while (tPos <= CIUtil.MecsStringWidth(item))
                        {
                            string sBuf1 = CIUtil.CiCopyStrWidth(item, tPos + 1, nLen, 0);
                            if (tPos == 0)
                            {
                                sBuf1 = sSymbol + sBuf1;
                            }
                            if (!string.IsNullOrEmpty(sBuf1))
                            {
                                DocumentLine documentLine = new DocumentLine();
                                documentLine.Text = sBuf1;
                                if (drugInf.InfKbn == 1)
                                {
                                    documentLine.Color = Color.Blue;
                                }
                                else
                                {
                                    documentLine.Color = Color.Black;
                                }

                                tText.Add(documentLine);
                            }

                            tPos = tPos + nLen;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            DocumentLine documentLine = new DocumentLine();
                            documentLine.Text = sSymbol + item;
                            if (drugInf.InfKbn == 1)
                            {
                                documentLine.Color = Color.Blue;
                            }
                            else
                            {
                                documentLine.Color = Color.Black;
                            }

                            tText.Add(documentLine);
                        }
                    }
                }
            }

            drugInfoModel.Tyui = tText;

        }

        private void SetupDrugComment(OrderInfoModel orderInfoModel, OrderInfDetailModel drug, DrugInfoModel drugInfoModel)
        {
            var details = orderInfoModel.OrderInfDetailCollection;
            int rowNo = drug.RowNo;
            string drgComment = "";
            int i = 0;

            while (rowNo < details.Count)
            {
                rowNo++;
                var comment = details.Where(d => (string.IsNullOrEmpty(d.ItemCd) || d.SinKouiKbn == 99) && d.RowNo == rowNo).FirstOrDefault();
                if (comment == null)
                {
                    break;
                }
                if (i == 0)
                {
                    drgComment = comment.ItemName;
                }
                else
                {
                    drgComment = drgComment + Environment.NewLine + "※" + comment.ItemName;
                }
                i++;
            }
            if (i > 1)
            {
                drgComment = "※" + drgComment;
            }
            drugInfoModel.DrgComment = drgComment;
        }

        private void SetupUsageComment(OrderInfoModel orderInfoModel, OrderInfDetailModel usage, DrugInfoModel drugInfoModel)
        {
            var details = orderInfoModel.OrderInfDetailCollection;
            int rowNo = usage.RowNo;
            string usageComment = "";
            int i = 0;
            while (rowNo < details.Count)
            {
                rowNo++;
                var comment = details.Where(d => (string.IsNullOrEmpty(d.ItemCd) || d.SinKouiKbn == 99) && d.RowNo == rowNo).FirstOrDefault();
                if (comment == null)
                {
                    continue;
                }
                if (i == 0)
                {
                    usageComment = comment.ItemName;
                }
                else
                {
                    usageComment = usageComment + Environment.NewLine + comment.ItemName;
                }
                i++;
            }
            drugInfoModel.UsageComment = usageComment;
        }
    }
}
