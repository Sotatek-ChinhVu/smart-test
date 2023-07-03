using Domain.Constant;
using Domain.Models.DrugInfor;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class DrugInforRepository : RepositoryBase, IDrugInforRepository
    {
        private readonly IConfiguration _configuration;

        public DrugInforRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            _configuration = configuration;
        }

        public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd)
        {
            var queryItems = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                                                                    && item.StartDate <= sinDate && item.EndDate >= sinDate);

            ////Join
            var joinQuery = from m28DrugMst in NoTrackingDataContext.M28DrugMst
                            join tenItem in queryItems
                            on m28DrugMst.KikinCd equals tenItem.ItemCd
                            join m34DrugInfoMain in NoTrackingDataContext.M34DrugInfoMains
                            on m28DrugMst.YjCd equals m34DrugInfoMain.YjCd
                            join drugInf in NoTrackingDataContext.PiProductInfs
                            on m28DrugMst.YjCd equals drugInf.YjCd
                            where string.IsNullOrEmpty(itemCd) || tenItem.ItemCd == itemCd
                            select new 
                            { 
                                m28DrugMst, 
                                tenItem, 
                                m34DrugInfoMain, 
                                drugInf 
                            };

            // piczai pichou
            string pathServerDefault = _configuration["PathImageDrugFolder"] ?? string.Empty;

            var pathConfDb = NoTrackingDataContext.PathConfs.Where(p => p.GrpCd == PicImageConstant.GrpCodeDefault || p.GrpCd == PicImageConstant.GrpCodeCustomDefault).ToList();

            var pathConfDf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);

            // PicZai
            string defaultPicZai = "";
            string otherPicZai = "";
            string defaultPicHou = "";
            string otherPicHou = "";
            if (pathConfDf != null)
            {
                defaultPicZai = pathConfDf.Path + "zaikei/";
                defaultPicHou = pathConfDf.Path + "housou/";
            }
            else
            {
                defaultPicZai = pathServerDefault + "zaikei/";
                defaultPicHou = pathServerDefault + "housou/";
            }
            string customPathPicZai = "";
            string customPathPicHou = "";
            var customPathConf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeCustomDefault);
            if (customPathConf != null)
            {
                customPathPicZai = customPathConf.Path + "zaikei/";
                customPathPicHou = customPathConf.Path + "housou/";
            }
            else
            {
                customPathPicZai = pathServerDefault + "zaikei/";
                customPathPicHou = pathServerDefault + "housou/";
            }

            var imagePics = NoTrackingDataContext.PiImages.Where(pi => pi.ItemCd == itemCd && (pi.ImageType == PicImageConstant.PicZaikei || pi.ImageType == PicImageConstant.PicHousou)).ToList();

            var otherImagePicZai = imagePics.FirstOrDefault(pi => pi.ImageType == PicImageConstant.PicZaikei);
            if (otherImagePicZai != null)
            {
                otherPicZai = defaultPicZai + otherImagePicZai.FileName ?? string.Empty;
            }

            var otherImagePicHou = imagePics.FirstOrDefault(pi =>pi.ImageType == PicImageConstant.PicHousou);
            if (otherImagePicHou != null)
            {
                otherPicHou = defaultPicHou + otherImagePicHou.FileName ?? string.Empty;
            }

            var rs = joinQuery.FirstOrDefault();
            if (rs != null)
            {
                return new DrugInforModel(rs.tenItem != null ? (rs.tenItem.Name ?? string.Empty) : string.Empty,
                                          rs.drugInf != null ? (rs.drugInf.GenericName ?? string.Empty) : string.Empty,
                                          rs.drugInf != null ? (rs.drugInf.Unit ?? string.Empty) : string.Empty,
                                          rs.drugInf != null ? (rs.drugInf.Maker ?? string.Empty) : string.Empty,
                                          rs.drugInf != null ? (rs.drugInf.Vender ?? string.Empty) : string.Empty,
                                          rs.tenItem != null ? rs.tenItem.KohatuKbn : 0,
                                          rs.tenItem != null ? rs.tenItem.Ten : 0,
                                          rs.tenItem != null ? (rs.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                          rs.m34DrugInfoMain != null ? (rs.m34DrugInfoMain.Mark ?? string.Empty) : string.Empty,
                                          rs.tenItem != null ? rs.tenItem.YjCd ?? string.Empty : string.Empty,
                                          "",
                                          "",
                                          defaultPicZai,
                                          customPathPicZai,
                                          otherPicZai,
                                          defaultPicHou,
                                          customPathPicHou,
                                          otherPicHou,
                                          new List<string>(),
                                          new List<string>());
            }
            else
            {
                return new DrugInforModel();
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
