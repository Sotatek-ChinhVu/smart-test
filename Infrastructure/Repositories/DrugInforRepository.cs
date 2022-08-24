using Domain.Constant;
using Domain.Models.DrugInfor;
using Helper.Common;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DrugInforRepository : IDrugInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public DrugInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd)
        {
            var queryItems = _tenantDataContext.TenMsts.Where(
                     item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                     && item.StartDate <= sinDate && item.EndDate >= sinDate
                     ).ToList();

            var queryDrugInfs = _tenantDataContext.PiProductInfs.ToList();
            var queryM28DrugMsts = _tenantDataContext.M28DrugMst.ToList();
            var queryM34DrugInfoMains = _tenantDataContext.M34DrugInfoMains.ToList();
            //Join
            var joinQuery = from m28DrugMst in queryM28DrugMsts
                            join tenItem in queryItems
                            on m28DrugMst.KikinCd equals tenItem.ItemCd
                            join m34DrugInfoMain in queryM34DrugInfoMains
                            on m28DrugMst.YjCd equals m34DrugInfoMain.YjCd
                            join drugInf in queryDrugInfs
                            on m28DrugMst.YjCd equals drugInf.YjCd
                            select new { m28DrugMst, tenItem, m34DrugInfoMain, drugInf };

            if (!string.IsNullOrEmpty(itemCd))
            {
                joinQuery = joinQuery.Where(
                       item =>
                       item.tenItem.ItemCd == itemCd);
            }
            var item = joinQuery.AsEnumerable().FirstOrDefault();
            string YJCode = "";
            if (item != null && item.tenItem != null)
            {
                YJCode = item.tenItem.YjCd ?? string.Empty;
            }

            // get pic zaikei
            var picZaikei = GetImageDefault(YJCode, itemCd, PicImageConstant.PicZaikei);

            // get pic housou
            var picHousou = GetImageDefault(YJCode, itemCd, PicImageConstant.PicHousou);



            var result = joinQuery.AsEnumerable().Select(d => new DrugInforModel(
                                                        d.tenItem != null ? (d.tenItem.Name ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? d.drugInf.GenericName : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Unit ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Marketer ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Vender ?? string.Empty) : string.Empty,
                                                        d.tenItem != null ? d.tenItem.KohatuKbn : 0,
                                                        d.tenItem != null ? d.tenItem.Ten : 0,
                                                        d.tenItem != null ? (d.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                                        d.m34DrugInfoMain != null ? (d.m34DrugInfoMain.Mark ?? string.Empty) : string.Empty,
                                                        picZaikei,
                                                        picHousou
                                                    )).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            else
            {
                return new DrugInforModel();
            }
        }

        private string GetImageDefault(string yjCode, string itemCd, int imageType)
        {
            string defaultImgPic = string.Empty;

            // get PicZai PicHou
            var defaultPic = "";
            var pathConf = _tenantDataContext.PathConfs
                .FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);
            if (pathConf != null)
            {
                if(imageType == 0)
                {
                    defaultPic = pathConf.Path + @"\zaikei\";
                }
                else
                {
                    defaultPic = pathConf.Path + @"\housou\";
                }
            }
            else
            {

                if (imageType == 0)
                {
                    defaultPic = PathConstant.DrugImageServerPath + @"\zaikei\";
                }
                else
                {
                    defaultPic = PathConstant.DrugImageServerPath + @"\housou\";
                }
            }

            // Get Custom PicZai PicHou
            var customPathPic = "";
            var customPathConf = _tenantDataContext.PathConfs
                .FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeCustomDefault);

            if (customPathConf != null)
            {
                if (imageType == 0)
                {
                    customPathPic = customPathConf.Path + @"\zaikei\";
                }
                else
                {
                    customPathPic = customPathConf.Path + @"\housou\";
                }
            }
            else
            {
                if (imageType == 0)
                {
                    customPathPic = PathConstant.DrugImageServerPath + @"\zaikei\";
                }
                else
                {
                    customPathPic = PathConstant.DrugImageServerPath + @"\housou\";
                }
            }


            // PicZaikei
            // get other Image PicZai
            var listPic = new List<string>();
            var otherImagePic = _tenantDataContext.PiImages.FirstOrDefault(pi => pi.ItemCd == itemCd && pi.ImageType == PicImageConstant.PicZaikei);
            if (otherImagePic != null)
            {
                defaultImgPic = defaultPic + otherImagePic.FileName ?? string.Empty;
            }
            else
            {
                var _picStr = " ABCDEFGHIJZ";
                for (int i = 0; i < _picStr.Length - 1; i++)
                {
                    if (!String.IsNullOrEmpty(yjCode))
                    {
                        string imgFile = (defaultPic + yjCode + _picStr[i]).Trim() + ".jpg";
                        if (CIUtil.IsFileExisting(imgFile))
                        {
                            listPic.Add(imgFile);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(yjCode))
                {
                    string customImage = customPathPic + yjCode + "Z.jpg";
                    if (CIUtil.IsFileExisting(customImage))
                    {
                        listPic.Add(customImage);
                    }
                }

                if (listPic.Count > 0)
                {
                    // Image default 
                    defaultImgPic = listPic[0] ?? string.Empty;
                }
            }
            return defaultImgPic;
        }
    }
}
