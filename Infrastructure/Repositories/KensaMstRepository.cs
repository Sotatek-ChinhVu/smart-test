using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class KensaMstRepository : RepositoryBase, IKensaMstFinder
    {
        public KensaMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<KensaMstModel> GetParrentKensaMstModels(int hpId, string keyWord)
        {
            var result = new List<KensaMstModel>();
            string itemCd = "";
            var kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId);
            var kensaInTenMst = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);

            if (string.IsNullOrEmpty(keyWord))
            {
                kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId &&
                                                                                      p.IsDelete == DeleteTypes.None &&
                                                                                      (string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd));

                kensaInTenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                                  !string.IsNullOrEmpty(p.KensaItemCd) &&
                                                                                  p.IsDeleted == DeleteTypes.None);
            }
            else
            {
                string bigKeyWord = keyWord.ToUpper()
                                           .Replace("ｧ", "ｱ")
                                           .Replace("ｨ", "ｲ")
                                           .Replace("ｩ", "ｳ")
                                           .Replace("ｪ", "ｴ")
                                           .Replace("ｫ", "ｵ")
                                           .Replace("ｬ", "ﾔ")
                                           .Replace("ｭ", "ﾕ")
                                           .Replace("ｮ", "ﾖ")
                                           .Replace("ｯ", "ﾂ");

                //get kensa in KensaMst
                kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId &&
                                                                                          p.IsDelete == DeleteTypes.None &&
                                                                                          (string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd) &&
                                                                                          (keyWord == "ﾊﾞｲﾀﾙ" ? p.KensaItemCd.Contains("V") :
                                                                                          (p.KensaName.ToUpper().Contains(bigKeyWord) ||
                                                                                           p.KensaKana.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                                .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ")
                                                                                                                .StartsWith(bigKeyWord))));

                //get kensa in TenMst
                kensaInTenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                                  p.IsDeleted == DeleteTypes.None &&
                                                                                      !string.IsNullOrEmpty(p.KensaItemCd) &&
                                                                                      (keyWord == "IGE" ? p.ItemCd.StartsWith("IGE") :
                                                                                      (p.Name.ToUpper().Contains(bigKeyWord)
                                                                                    || p.KanaName1.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName2.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName3.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName4.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName5.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName6.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName7.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord))));
            }
            if (!string.IsNullOrEmpty(itemCd))
            {
                kensaInKensaMst = kensaInKensaMst.Where(u => u.KensaItemCd == itemCd);
            }
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);

            var tenMstJoinKensaMstQuery = from kensaTenMst in kensaInTenMst
                                          join kensaMst in kensaMsts.Where(p => string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd)
                                          on new { kensaTenMst.KensaItemCd, kensaTenMst.KensaItemSeqNo } equals new { kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo }
                                          select new
                                          {
                                              KensaMst = kensaMst
                                          };

            var allParrentKensaMsts = tenMstJoinKensaMstQuery.Select(p => p.KensaMst).Union(kensaInKensaMst).OrderBy(p => p.KensaKana).Distinct().ToList();

            //get all child kensaMst 
            var allKensaMsts = from parrentKensaMst in allParrentKensaMsts
                               join kensaMst in kensaMsts.Where(p => !string.IsNullOrEmpty(p.OyaItemCd) && p.KensaItemCd != p.OyaItemCd)
                               on parrentKensaMst.KensaItemCd equals kensaMst.OyaItemCd into childKensaMsts
                               select new
                               {
                                   ParrentKensaMst = parrentKensaMst,
                                   ChildKensaMsts = childKensaMsts,
                               };

            var KensaItemCd = kensaMsts.Select(x => x.KensaItemCd).ToList();
            var KensaItemSeqNo = kensaMsts.Select(x => x.KensaItemSeqNo).ToList();

            var tenMsts = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && KensaItemCd.Contains(p.KensaItemCd ?? string.Empty) && KensaItemSeqNo.Contains(p.KensaItemSeqNo));

            var query = from kensaMst in allKensaMsts
                        join tenMst in tenMsts
                        on new { kensaMst.ParrentKensaMst.KensaItemCd, kensaMst.ParrentKensaMst.KensaItemSeqNo }
                        equals new { tenMst.KensaItemCd, tenMst.KensaItemSeqNo } into tempTenMsts
                        select new
                        {
                            ParrentKensaMst = kensaMst.ParrentKensaMst,
                            ChildKensaMsts = kensaMst.ChildKensaMsts,
                            TenMsts = tempTenMsts.Select(x => new TenMstModel(x.SinKouiKbn,
                                                                              x.MasterSbt ?? string.Empty,
                                                                              x.ItemCd,
                                                                              x.KensaItemCd ?? string.Empty,
                                                                              x.KensaItemSeqNo,
                                                                              x.Ten,
                                                                              x.Name ?? string.Empty,
                                                                              x.ReceName ?? string.Empty,
                                                                              x.KanaName1 ?? string.Empty,
                                                                              x.KanaName2 ?? string.Empty,
                                                                              x.KanaName3 ?? string.Empty,
                                                                              x.KanaName4 ?? string.Empty,
                                                                              x.KanaName5 ?? string.Empty,
                                                                              x.KanaName6 ?? string.Empty,
                                                                              x.KanaName7 ?? string.Empty,
                                                                              x.StartDate,
                                                                              x.EndDate,
                                                                              x.DefaultVal,
                                                                              x.OdrUnitName ?? string.Empty,
                                                                              x.SanteiItemCd ?? string.Empty,
                                                                              x.SanteigaiKbn,
                                                                              x.IsNosearch)).ToList()
                        };
             
            foreach (var entity in query)
            {
                result.Add(new KensaMstModel(
                    entity.ParrentKensaMst.KensaItemCd,
                    entity.ParrentKensaMst.KensaItemSeqNo,
                    entity.ParrentKensaMst.CenterCd ?? string.Empty,
                    entity.ParrentKensaMst.KensaName ?? string.Empty,
                    entity.ParrentKensaMst.KensaKana ?? string.Empty,
                    entity.ParrentKensaMst.Unit ?? string.Empty,
                    entity.ParrentKensaMst.MaterialCd,
                    entity.ParrentKensaMst.ContainerCd,
                    entity.ParrentKensaMst.MaleStd ?? string.Empty,
                    entity.ParrentKensaMst.MaleStdLow ?? string.Empty,
                    entity.ParrentKensaMst.MaleStdHigh ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStd ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStdLow ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStdHigh ?? string.Empty,
                    entity.ParrentKensaMst.Formula ?? string.Empty,
                    entity.ParrentKensaMst.Digit,
                    entity.ParrentKensaMst.OyaItemCd ?? string.Empty,
                    entity.ParrentKensaMst.OyaItemSeqNo,
                    entity.ParrentKensaMst.SortNo,
                    entity.ParrentKensaMst.CenterItemCd1 ?? string.Empty,
                    entity.ParrentKensaMst.CenterItemCd2 ?? string.Empty,
                    entity.TenMsts
                    ));
            }

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
