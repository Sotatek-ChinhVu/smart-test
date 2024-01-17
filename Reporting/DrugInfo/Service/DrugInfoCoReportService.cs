using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.DrugInfo.DB;
using Reporting.DrugInfo.Model;
using System.Text;

namespace Reporting.DrugInfo.Service;

public class DrugInfoCoReportService : RepositoryBase, IDrugInfoCoReportService
{
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly ICoDrugInfFinder _coDrugInfFinder;
    private readonly IAmazonS3Service _amazonS3Service;
    private int configType = 0;
    private int selectedFormType = 0;
    private DrugInfoModel basicInfo = new DrugInfoModel();
    private List<OrderInfoModel> orderInfoModels { get; set; }
    private readonly List<DrugInfoModel> drugInfoList = new();

    public DrugInfoCoReportService(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository, ICoDrugInfFinder coDrugInfFinder, IAmazonS3Service amazonS3Service) : base(tenantProvider)
    {
        _systemConfRepository = systemConfRepository;
        _coDrugInfFinder = coDrugInfFinder;
        orderInfoModels = new();
        _amazonS3Service = amazonS3Service;
        _defaultPicHou = string.Empty;
        _defaultPicZai = string.Empty;
    }

    private string _defaultPicHou;
    private string _defaultPicZai;
    public DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo)
    {
        try
        {
            basicInfo = _coDrugInfFinder.GetBasicInfo(hpId, ptId, sinDate);
            var partItem = _coDrugInfFinder.GetDefaultPathPicture();
            _defaultPicHou = partItem.PathPicHou;
            _defaultPicZai = partItem.PathPicZai;

            configType = (int)_systemConfRepository.GetSettingValue(92004, 1, hpId); // 0,1 - 1 Pic; 2 - 2 Pics; 3- No Pic

            selectedFormType = (int)_systemConfRepository.GetSettingValue(92004, 17, hpId);

            orderInfoModels = _coDrugInfFinder.GetOrderByRaiinNo(raiinNo);

            // get common data to setup print data
            List<string> itemCdList = new();
            foreach (var odrInf in orderInfoModels)
            {
                itemCdList.AddRange(odrInf.OrderInfDetailCollection.Select(item => item.ItemCd).Distinct().ToList());
            }
            int age = basicInfo.IntAge;
            int gender = basicInfo.Sex == "M" ? 1 : 2;
            (List<DrugInf> drugInfList,
             List<TenMst> tenMstList,
             List<M34DrugInfoMain> m34DrugInfoMainList,
             List<M34IndicationCode> m34IndicationCodeList,
             List<M34Precaution> m34PrecautionList,
             List<M34PrecautionCode> m34PrecautionCodeList) commonDrugData = _coDrugInfFinder.GetQueryDrugList(hpId, itemCdList, age, gender);
            var allSystemConfig = _systemConfRepository.GetAllSystemConfig(hpId);
            foreach (var orderInfoModel in orderInfoModels)
            {
                SetupPrintData(hpId, orderInfoModel, commonDrugData.drugInfList, commonDrugData.tenMstList, commonDrugData.m34DrugInfoMainList, commonDrugData.m34IndicationCodeList, commonDrugData.m34PrecautionList, commonDrugData.m34PrecautionCodeList, allSystemConfig);
            }
            return new DrugInfoData(
                       selectedFormType,
                       configType,
                       drugInfoList);
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
            _coDrugInfFinder.ReleaseResource();
            _amazonS3Service.Dispose();
        }
    }

    private void SetupPrintData(int hpId, OrderInfoModel orderInfoModel, List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList, List<SystemConfModel> allSystemConfigList)
    {
        var usage = orderInfoModel.OrderInfDetailCollection.FirstOrDefault(o => o.YohoKbn == 1);
        var jikochu = orderInfoModel.OrderInfDetailCollection.FirstOrDefault(o => o.SinKouiKbn == 28);
        if (usage == null && jikochu == null) return;
        var drugs = orderInfoModel.OrderInfDetailCollection.Where(o => new[] { 20, 30 }.Contains(o.SinKouiKbn)).ToList();
        foreach (var drug in drugs)
        {
            DrugInfoModel drugInfoModel = new DrugInfoModel();
            drugInfoModel.DrgName = drug.ItemName;

            //Main usage
            drugInfoModel.Usage = jikochu != null ? "自己注射" : usage?.ItemName ?? string.Empty;
            //2st usage
            var subUsage = orderInfoModel.OrderInfDetailCollection.FirstOrDefault(o => o.YohoKbn == 2);
            if (subUsage != null)
            {
                drugInfoModel.Usage2 = subUsage.ItemName;
            }

            //TODO
            drugInfoModel.DrgKbn = orderInfoModel.OdrKouiKbn;

            int sinDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
            string yjCd = tenMstList.FirstOrDefault(t => t.ItemCd == drug.ItemCd && t.StartDate <= sinDate && t.EndDate >= sinDate)?.YjCd ?? string.Empty;

            var singleDosageMstCollection = _coDrugInfFinder.GetSingleDosageMstCollection(hpId, drug.UnitName);

            drugInfoModel.UnitName = drug.UnitName;
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
                    string usageSignVal = (wkFloat * tenMst.Rise * 100 / 100).AsString();
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
                    string usageSignVal = (wkFloat * tenMst.Morning * 100 / 100).AsString();
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
                    string usageSignVal = (wkFloat * tenMst.DayTime * 100 / 100).AsString();
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
                    string usageSignVal = (wkFloat * tenMst.Evening * 100 / 100).AsString();
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
                    string usageSignVal = (wkFloat * tenMst.Sleep * 100 / 100).AsString();
                    drugInfoModel.UsageSign5 = CIUtil.Copy(usageSignVal, 1, 4);// 剤形マスタ登録は用時の数値
                }
            }

            drugInfoModel.IntAge = basicInfo.IntAge;
            drugInfoModel.Sex = basicInfo.Sex;

            //ConfigType=3: No image
            if (configType != 3)
            {
                SetupPrintImage(hpId, drug, drugInfoModel, yjCd);
            }

            if (selectedFormType == 1)
            {
                SetupDrugDocumentType2(configType, hpId, drug, drugInfoModel, drugInfList, tenMstList, m34DrugInfoMainList, m34IndicationCodeList, m34PrecautionList, m34PrecautionCodeList, allSystemConfigList);
            }
            else
            {
                SetupDrugDocument(configType, hpId, drug, drugInfoModel, drugInfList, tenMstList, m34DrugInfoMainList, m34IndicationCodeList, m34PrecautionList, m34PrecautionCodeList, allSystemConfigList);
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
                SetupUsageComment(orderInfoModel, usage ?? new(), drugInfoModel);
            }

            drugInfoModel.OrderDate = basicInfo.OrderDate;
            drugInfoModel.HpName = basicInfo.HpName;
            drugInfoModel.Address1 = basicInfo.Address1;
            drugInfoModel.Address2 = basicInfo.Address2;
            drugInfoModel.Phone = basicInfo.Phone;
            drugInfoModel.PtNo = basicInfo.PtNo;
            drugInfoModel.PtName = basicInfo.PtName;
            drugInfoModel.Sex = basicInfo.Sex;
            drugInfoModel.IntAge = basicInfo.IntAge;

            drugInfoList.Add(drugInfoModel);
        }

    }

    private void SetupPrintImage(int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel, string yjCd)
    {
        var images = _coDrugInfFinder.GetProductImages(hpId, orderInfDetailModel.ItemCd);
        string YJCode = yjCd;
        if (images == null || images.Count == 0)
        {
            //Pic House
            GetDefaultImage(drugInfoModel, YJCode, 1);
            //Pic Zai
            GetDefaultImage(drugInfoModel, YJCode, 0);
            return;
        }

        var picHou = images.FirstOrDefault(i => i.ImageType == 1);
        if (picHou != null)
        {
            drugInfoModel.PicHou = picHou.FileName ?? string.Empty;
        }
        else
        {
            GetDefaultImage(drugInfoModel, YJCode, 1);
        }

        var picZai = images.FirstOrDefault(i => i.ImageType == 0);
        if (picZai != null)
        {
            drugInfoModel.PicZai = picZai.FileName ?? string.Empty;
        }
        else
        {
            GetDefaultImage(drugInfoModel, YJCode, 0);
        }

    }

    readonly string _picStr = " ABCDEFGHIJZ";
    private void GetDefaultImage(DrugInfoModel drugInfoModel, string yjCode, int imageType)
    {
        List<string> listPic = new();
        var tasks = new List<Task<(bool vavlid, string key)>>();
        if (imageType == 0)
        {
            //Pic Zai
            for (int i = 0; i < _picStr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(yjCode))
                {
                    string imgFile = (_defaultPicZai + yjCode + _picStr[i].AsString()).Trim() + ".jpg";
                    tasks.Add(_amazonS3Service.S3FilePathIsExists(imgFile));
                }
            }
        }
        else
        {
            //Pic House
            for (int i = 0; i < _picStr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(yjCode))
                {
                    string imgFile = (_defaultPicHou + yjCode + _picStr[i].AsString()).Trim() + ".jpg";
                    tasks.Add(_amazonS3Service.S3FilePathIsExists(imgFile));
                }
            }
        }
        var rs = Task.WhenAll(tasks).Result;
        listPic.AddRange(rs.Where(x => x.vavlid).Select(x => _amazonS3Service.GetAccessBaseS3() + x.key));
        if (imageType == 0)
        {
            drugInfoModel.PicZai = listPic.FirstOrDefault() ?? string.Empty;
        }
        else
        {
            drugInfoModel.PicHou = listPic.FirstOrDefault() ?? string.Empty;
        }
    }

    /// <summary>
    /// SetupDrugDocument to view data
    /// </summary>
    /// <param name="reportType"></param>
    /// <param name="hpId"></param>
    /// <param name="orderInfDetailModel"></param>
    /// <param name="drugInfoModel"></param>
    /// <param name="drugInfList"></param>
    /// <param name="tenMstList"></param>
    /// <param name="m34DrugInfoMainList"></param>
    /// <param name="m34IndicationCodeList"></param>
    /// <param name="m34PrecautionList"></param>
    /// <param name="m34PrecautionCodeList"></param>
    /// <param name="allSystemConfigList"></param>
    private void SetupDrugDocumentType2(int reportType, int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel, List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList, List<SystemConfModel> allSystemConfigList)
    {
        int age = drugInfoModel.IntAge;
        int gender = drugInfoModel.Sex == "M" ? 1 : 2;
        var drugInfs = _coDrugInfFinder.GetDrugInfo(hpId, orderInfDetailModel.ItemCd, age, gender, drugInfList, tenMstList, m34DrugInfoMainList, m34IndicationCodeList, m34PrecautionList, m34PrecautionCodeList, allSystemConfigList);

        // check null drugInfs
        if (drugInfs == null || !drugInfs.Any()) return;

        int nLen = 60;

        if (reportType == 3)
        {
            nLen = 80;
        }

        List<DocumentLine> tText = new();

        tText.AddRange(GetListDocumentLine(drugInfs.Where(item => item.InfKbn == 1).ToList(), 1, nLen));
        tText.AddRange(GetListDocumentLine(drugInfs.Where(item => item.InfKbn != 1).ToList(), 0, nLen));

        drugInfoModel.Tyui = tText;

    }

    private List<DocumentLine> GetListDocumentLine(List<DrugInf> drugInfs, int infKbn, int nLen)
    {
        List<DocumentLine> tText = new();

        StringBuilder wsBuf = new();
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
            var listItem = drugInf.DrugInfo?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();
            foreach (var item in listItem)
            {
                wsBuf.Append(item);
            }
        }

        if (string.IsNullOrEmpty(wsBuf.ToString()))
        {
            return tText;
        }

        wsBuf.Append(sSymbol + wsBuf);

        int tPos = 0;
        var width = CIUtil.MecsStringWidth(wsBuf.ToString());
        while (tPos <= width)
        {
            string sBuf1 = CIUtil.CiCopyStrWidth(wsBuf.ToString(), tPos + 1, nLen, 0);
            if (!string.IsNullOrEmpty(sBuf1))
            {
                DocumentLine documentLine = new();
                documentLine.Text = sBuf1;
                if (infKbn == 1)
                {
                    documentLine.Color = ColorConstant.Blue;
                }
                else
                {
                    documentLine.Color = ColorConstant.Black;
                }

                tText.Add(documentLine);
            }

            tPos = tPos + nLen;
        }

        return tText;
    }

    /// <summary>
    /// SetupDrugDocument to view data
    /// </summary>
    /// <param name="reportType"></param>
    /// <param name="hpId"></param>
    /// <param name="orderInfDetailModel"></param>
    /// <param name="drugInfoModel"></param>
    /// <param name="drugInfList"></param>
    /// <param name="tenMstList"></param>
    /// <param name="m34DrugInfoMainList"></param>
    /// <param name="m34IndicationCodeList"></param>
    /// <param name="m34PrecautionList"></param>
    /// <param name="m34PrecautionCodeList"></param>
    /// <param name="allSystemConfigList"></param>
    private void SetupDrugDocument(int reportType, int hpId, OrderInfDetailModel orderInfDetailModel, DrugInfoModel drugInfoModel, List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList, List<SystemConfModel> allSystemConfigList)
    {
        int age = drugInfoModel.IntAge;
        int gender = drugInfoModel.Sex == "M" ? 1 : 2;
        var drugInfs = _coDrugInfFinder.GetDrugInfo(hpId, orderInfDetailModel.ItemCd, age, gender, drugInfList, tenMstList, m34DrugInfoMainList, m34IndicationCodeList, m34PrecautionList, m34PrecautionCodeList, allSystemConfigList);

        // check null drugInfs
        if (drugInfs == null || !drugInfs.Any()) return;

        int nLen = 72;

        if (reportType == 3)
        {
            nLen = 86;
        }

        List<DocumentLine> tText = new();

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
                                documentLine.Color = ColorConstant.Blue;
                            }
                            else
                            {
                                documentLine.Color = ColorConstant.Black;
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
                        DocumentLine documentLine = new();
                        documentLine.Text = sSymbol + item;
                        if (drugInf.InfKbn == 1)
                        {
                            documentLine.Color = ColorConstant.Blue;
                        }
                        else
                        {
                            documentLine.Color = ColorConstant.Black;
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
        string drgComment = string.Join(Environment.NewLine, details
            .Skip(rowNo)
            .TakeWhile(d => string.IsNullOrEmpty(d.ItemCd) || d.SinKouiKbn == 99)
            .Select((d, i) => i == 0 ? d.ItemName : "※" + d.ItemName));
        if (drgComment.Count(c => c == '※') > 1)
        {
            drgComment = "※" + drgComment;
        }
        drugInfoModel.DrgComment = drgComment;
    }

    private void SetupUsageComment(OrderInfoModel orderInfoModel, OrderInfDetailModel usage, DrugInfoModel drugInfoModel)
    {
        var details = orderInfoModel.OrderInfDetailCollection;
        int rowNo = usage.RowNo;
        var usageComments = details
            .SkipWhile(d => d.RowNo < rowNo)
            .Where(d => string.IsNullOrEmpty(d.ItemCd) || d.SinKouiKbn == 99)
            .Select(d => d.ItemName);
        string usageComment = string.Join(Environment.NewLine, usageComments);
        drugInfoModel.UsageComment = usageComment;
    }
}
