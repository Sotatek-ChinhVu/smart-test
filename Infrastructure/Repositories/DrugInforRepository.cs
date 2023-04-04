﻿using Domain.Constant;
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
            var queryItems = NoTrackingDataContext.TenMsts.Where(
                     item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                     && item.StartDate <= sinDate && item.EndDate >= sinDate
                     );

            var queryDrugInfs = NoTrackingDataContext.PiProductInfs.AsQueryable();
            var queryM28DrugMsts = NoTrackingDataContext.M28DrugMst.AsQueryable();
            var queryM34DrugInfoMains = NoTrackingDataContext.M34DrugInfoMains.AsQueryable();

            ////Join
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
            string yjCode = "";
            if (item != null && item.tenItem != null)
            {
                yjCode = item.tenItem.YjCd ?? string.Empty;
            }

            // piczai pichou
            string pathServerDefault = _configuration["PathImageDrugFolder"] ?? string.Empty;
            var pathConf = NoTrackingDataContext.PathConfs
                .FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);

            // PicZai
            var defaultPicZai = "";
            var otherPicZai = "";
            var defaultPicHou = "";
            var otherPicHou = "";
            if (pathConf != null)
            {
                defaultPicZai = pathConf.Path + @"/zaikei/";
                defaultPicHou = pathConf.Path + @"/housou/";
            }
            else
            {

                defaultPicZai = pathServerDefault + @"/zaikei/";
                defaultPicHou = pathServerDefault + @"/housou/";
            }
            var customPathPicZai = "";
            var customPathPicHou = "";
            var customPathConf = NoTrackingDataContext.PathConfs.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeCustomDefault);
            if (customPathConf != null)
            {
                customPathPicZai = customPathConf.Path + @"/zaikei/";
                customPathPicHou = customPathConf.Path + @"/housou/";
            }
            else
            {
                customPathPicZai = pathServerDefault + @"/zaikei/";
                customPathPicHou = pathServerDefault + @"/housou/";
            }

            var otherImagePic = NoTrackingDataContext.PiImages.FirstOrDefault(pi => pi.ItemCd == itemCd && pi.ImageType == PicImageConstant.PicZaikei);
            if (otherImagePic != null)
            {
                otherPicZai = defaultPicZai + otherImagePic.FileName ?? string.Empty;
                otherPicHou = defaultPicHou + otherImagePic.FileName ?? string.Empty;
            }

            var result = joinQuery.AsEnumerable().Select(d => new DrugInforModel(
                                                        d.tenItem != null ? (d.tenItem.Name ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.GenericName ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Unit ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Marketer ?? string.Empty) : string.Empty,
                                                        d.drugInf != null ? (d.drugInf.Vender ?? string.Empty) : string.Empty,
                                                        d.tenItem != null ? d.tenItem.KohatuKbn : 0,
                                                        d.tenItem != null ? d.tenItem.Ten : 0,
                                                        d.tenItem != null ? (d.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                                        d.m34DrugInfoMain != null ? (d.m34DrugInfoMain.Mark ?? string.Empty) : string.Empty,
                                                        yjCode,
                                                        "",
                                                        "",
                                                        defaultPicZai,
                                                        customPathPicZai,
                                                        otherPicZai,
                                                        defaultPicHou,
                                                        customPathPicHou,
                                                        otherPicHou,
                                                        new List<string>(),
                                                        new List<string>()
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
