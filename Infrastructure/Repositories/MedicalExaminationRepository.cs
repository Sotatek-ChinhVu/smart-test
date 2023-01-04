using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using System.Text.RegularExpressions;
using static Helper.Constants.OrderInfConst;

namespace Infrastructure.Repositories
{
    public class MedicalExaminationRepository : RepositoryBase, IMedicalExaminationRepository
    {
        public MedicalExaminationRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        public List<CheckedOrderModel> IgakuTokusitu(int hpId, int sinDate, int hokenId, ReceptionModel receptionModel, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou)
        {
            var checkedOrderModelList = new List<CheckedOrderModel>();
            var igakuTokusituItem = allOdrInfDetail.FirstOrDefault(detail => detail.ItemCd == ItemCdConst.IgakuTokusitu || detail.ItemCd == ItemCdConst.IgakuTokusitu1);

            // 既に入力されている場合は不要
            if (igakuTokusituItem != null)
            {
                return checkedOrderModelList;
            }

            TenMst? tenMstModel = null;
            if (isJouhou)
            {
                if (sinDate >= 20220401)
                {
                    tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTokusitu1, sinDate);
                    if (tenMstModel == null)
                    {
                        return checkedOrderModelList;
                    }
                }
                else
                {
                    return checkedOrderModelList;
                }
            }
            else
            {
                tenMstModel = FindTenMst(hpId, ItemCdConst.IgakuTokusitu, sinDate);
                if (tenMstModel == null)
                {
                    return checkedOrderModelList;
                }
            }

            // 初診の場合は算定不可
            if (receptionModel.SyosaisinKbn == SyosaiConst.Syosin ||
                receptionModel.SyosaisinKbn == SyosaiConst.Syosin2 ||
                receptionModel.SyosaisinKbn == SyosaiConst.Unspecified)
            {
                return checkedOrderModelList;
            }

            // 電話再診の場合は算定不可
            if (receptionModel.SyosaisinKbn == SyosaiConst.SaisinDenwa ||
                receptionModel.SyosaisinKbn == SyosaiConst.SaisinDenwa2)
            {
                return checkedOrderModelList;
            }

            // 初診日から1カ月以内は算定不可
            // 背反設定されている場合は不可

            var byomeiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2002 && p.GrpEdaNo == 4)?.Val ?? 0;
            // 対象疾病の有無
            bool existByoMeiSpecial = ByomeiModelList
                                .Any(b => (byomeiCondition == 1 ? b.SyubyoKbn == 1 : true) &&
                                    b.SikkanKbn == SikkanKbnConst.Special &&
                                    (b.HokenPid == hokenId || b.HokenPid == 0) &&
                                    b.StartDate <= sinDate &&
                                    (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > sinDate));
            if (existByoMeiSpecial)
            {
                var santeiKanren = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 4001 && p.GrpEdaNo == 0)?.Val ?? 0;
                bool santei;
                if (santeiKanren == 0)
                {
                    santei = false;
                }
                else if (santeiKanren == 1 && receptionModel.SyosaisinKbn != SyosaiConst.None)
                {
                    santei = true;
                }
                checkingOrderModel.CheckingContent = FormatSanteiMessage(tenMstModel.Name);
                checkingOrderModel.TenMstItem = tenMstModel;

                var checkingOrderModel = new CheckedOrderModel(CheckingType.MissingCalculate, );


                checkingOrderModelList.Add(checkingOrderModel);

                return checkingOrderModelList;
            }

            bool existByoMeiOther = ByomeiModelList
                            .Any(b => (byomeiCondition == 1 ? b.SyobyoKbn : true) &&
                                b.SikkanKbn == SikkanKbnConst.Other &&
                                (b.HokenNo == _hokenId || b.HokenNo == 0) &&
                                b.StartDate <= _sinDate &&
                                (b.TenkiKbn == TenkiKbnConst.Continued || b.TenkiDate > _sinDate));
            if (existByoMeiOther)
            {
                CheckingOrderModel checkingOrderModel = new CheckingOrderModel();
                checkingOrderModel.CheckingType = CheckingType.MissingCalculate;
                checkingOrderModel.Santei = false;
                checkingOrderModel.CheckingContent = FormatSanteiMessage(tenMstModel.Name);
                checkingOrderModel.TenMstItem = tenMstModel;

                checkingOrderModelList.Add(checkingOrderModel);

                return checkingOrderModelList;
            }
            return checkingOrderModelList;
        }


        public TenMst FindTenMst(int hpId, string itemCd, int sinDate)
        {
            var entity = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd);
            
            return entity ?? new TenMst();
        }

        private string FormatSanteiMessage(string santeiItemName)
        {
            return $"\"{santeiItemName}\"を算定できる可能性があります。";

        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
