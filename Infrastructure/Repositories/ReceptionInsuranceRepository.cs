using Domain.Constant;
using Domain.Models.ReceptionInsurance;
using Entity.Tenant;
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
    public class ReceptionInsuranceRepository: IReceptionInsuranceRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionInsuranceRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<ReceptionInsuranceModel> GetReceptionInsurance(int hpId, long ptId, int sinDate, bool isShowExpiredReception)
        {
            var listData = new List<ReceptionInsuranceModel>();
            var listhokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None)
                           .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                           .ThenByDescending(x => x.HokenId)
                           .ToList();
            var listKohi = _tenantDataContext.PtKohis.Where(entity => entity.HpId == hpId && entity.PtId == ptId)
                          .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                          .ThenByDescending(x => x.HokenId).ToList();
            
            if(listhokenInf.Count > 0)
            {
                foreach (var item in listhokenInf)
                {
                    var HokenMasterModel = _tenantDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                    var isReceKisaiOrNoHoken = false;
                    var isExpirated = IsExpirated(item.StartDate, item.EndDate, sinDate);
                    if (HokenMasterModel != null)
                    {
                       isReceKisaiOrNoHoken = IsReceKisai(HokenMasterModel) || IsNoHoken(HokenMasterModel, item.HokenKbn, item.Houbetu ?? string.Empty);
                    }
                    if(!isReceKisaiOrNoHoken && (isShowExpiredReception || isExpirated))
                    {
                        var newItemHokenInfModel = new ReceptionInsuranceModel(
                                            item.HokenKbn,
                                            item.Kigo ?? string.Empty,
                                            item.Bango ?? string.Empty,
                                            item.StartDate,
                                            item.EndDate,
                                            GetConfirmDate(item.HokenId, HokenGroupConstant.HokenGroupHokenPattern),
                                            item.EdaNo ?? string.Empty,
                                            item.HokensyaNo ?? string.Empty,
                                            item.RousaiKofuNo ?? string.Empty,
                                            sinDate,
                                            1,
                                            0,
                                            "",
                                            "",
                                            item.HokenId,
                                            GetConfirmState(item.HokenKbn, item.Houbetu ?? string.Empty, hpId, ptId, sinDate, item.HokenId, 1, HokenMasterModel)
                                            );

                        listData.Add(newItemHokenInfModel);
                    }    
                }
            }
            
            if(listKohi.Count > 0)
            {
                foreach (var item in listKohi)
                {
                    var HokenMasterModel = _tenantDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                    var isExpirated = IsExpirated(item.StartDate, item.EndDate, sinDate);
                    if(isShowExpiredReception || isExpirated)
                    {
                        var newItemKohiModel = new ReceptionInsuranceModel(
                                            0,
                                            "",
                                            "",
                                            item.StartDate,
                                            item.EndDate,
                                            GetConfirmDate(item.HokenId, HokenGroupConstant.HokenGroupKohi),
                                            "",
                                            "",
                                            "",
                                            sinDate,
                                            0,
                                            1,
                                            item.FutansyaNo ?? string.Empty,
                                            item.JyukyusyaNo ?? string.Empty,
                                            item.HokenId,
                                            GetConfirmState(0, item.Houbetu ?? string.Empty, hpId, ptId, sinDate, item.HokenId, 2, HokenMasterModel)
                                            );

                        listData.Add(newItemKohiModel);
                    }    
                }
            }   

            return listData;
        }
        private int GetConfirmDate(int hokenId, int typeHokenGroup)
        {
            var validHokenCheck = _tenantDataContext.PtHokenChecks.Where(x => x.IsDeleted == 0 && x.HokenId == hokenId && x.HokenGrp == typeHokenGroup)
                .OrderByDescending(x => x.CheckDate)
                .ToList();
            if (!validHokenCheck.Any())
            {
                return 0;
            }
            return CIUtil.DateTimeToInt(validHokenCheck[0].CheckDate);
        }

        private int GetConfirmState(int hokenKbn, string houbetu, int hpId, long ptId,int sinDate, int hokenId, int hokenMstOrKohi, HokenMst? hokenMaster)
        {
            if (hokenMaster != null)
            {
                if(hokenMstOrKohi == 1)
                {
                    var IsReceKisaiOrNoHoken = IsReceKisai(hokenMaster) || IsNoHoken(hokenMaster, hokenKbn, houbetu);
                    // Jihi 100% or NoHoken
                    if (IsReceKisaiOrNoHoken)
                    {
                        return 1;
                    }
                }

                // HokenChecks
                var hokenChecks = _tenantDataContext.PtHokenChecks
                                    .Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == 0 
                                                && x.HokenGrp == 1 && x.HokenId == hokenId && x.IsDeleted == 0)
                                    .OrderByDescending(x => x.CheckDate)
                                    .ToList();

                if (hokenChecks.Count == 0)
                {
                    return 0;
                }

                var now = CIUtil.IntToDate(sinDate);
                if (hokenChecks.Any(hk => hk.CheckDate.Year == now.Year && hk.CheckDate.Month == now.Month && hk.CheckDate.Day == now.Day))
                {
                    return 2;
                }
                int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
                foreach (var ptHokenCheck in hokenChecks)
                {
                    int currentConfirmYM = Int32.Parse(CIUtil.Copy(CIUtil.DateTimeToInt(ptHokenCheck.CheckDate).ToString(), 1, 6));
                    if (currentConfirmYM == SinYM)
                    {
                        return 3;
                    }
                }
                return 0;

            }
            else
                return 1;
        }
        
        private bool IsReceKisai(HokenMst HokenMasterModel)
        {

            if (HokenMasterModel != null)
            {
                return HokenMasterModel.ReceKisai == 3;
            }
            return false;

        }

        private bool IsNoHoken(HokenMst HokenMasterModel, int hokenKbn, string houbetu)
        {
            
                if (HokenMasterModel != null)
                {
                    return HokenMasterModel.HokenSbtKbn == 0;
                }
                return hokenKbn == 1 && houbetu == HokenConstant.HOUBETU_NASHI;
        }

        private bool IsExpirated(int startDate, int endDate, int sinDate)
        {
            return !(startDate <= sinDate && endDate >= sinDate);
        }
    }
}
