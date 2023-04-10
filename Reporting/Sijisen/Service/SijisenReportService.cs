using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Reporting.Byomei.DB;
using Reporting.Common;
using Reporting.Enums;
using Reporting.Mappers;
using Reporting.Mappers.Common;
using Reporting.Sijisen.DB;
using Reporting.Sijisen.Mapper;
using Reporting.Sijisen.Model;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Reporting.Sijisen.Service
{
    public class SijisenReportService : ISijisenReportService
    {
        private readonly ITenantProvider _tenantProvider;
        public SijisenReportService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoSijisenFinder(_tenantProvider.GetNoTrackingDataContext());

                int hpId = Session.HospitalID;

                // 患者情報
                CoPtInfModel ptInf = finder.FindPtInf(hpId, ptId, sinDate);

                // 来院情報
                CoRaiinInfModel raiinInf = finder.FindRaiinInfData(hpId, ptId, sinDate, raiinNo);
                List<CoRaiinInfModel> raiinInfOther = finder.FindOtherRaiinInfData(hpId, ptId, sinDate, raiinNo);


                List<CoCommonOdrInfModel> commonOdrInfs = new List<CoCommonOdrInfModel>();
                List<CoCommonOdrInfDetailModel> commonOdrDtls = new List<CoCommonOdrInfDetailModel>();

                if (formType == (int)CoSijisenFormType.Sijisen)
                {            // オーダー情報
                    List<CoOdrInfModel> odrInfs = finder.FindOdrInf(hpId, ptId, sinDate, raiinNo, odrKouiKbns);

                    // オーダー情報詳細
                    List<CoOdrInfDetailModel> odrInfDtls = finder.FindOdrInfDetail(hpId, ptId, sinDate, raiinNo, odrKouiKbns);

                    commonOdrInfs = CommonOdrInfListFactory(odrInfs);
                    commonOdrDtls = CommonOdrInfDetailListFactory(odrInfDtls);
                }
                else if (formType == (int)CoSijisenFormType.JyusinHyo)
                {

                    // 予約オーダー情報
                    List<CoRsvkrtOdrInfModel> rsvkrtOdrInfs = finder.FindRsvKrtOdrInf(hpId, ptId, sinDate, odrKouiKbns);

                    // 予約オーダー情報詳細
                    List<CoRsvkrtOdrInfDetailModel> rsvkrtOdrInfDtls = finder.FindRsvKrtOdrInfDetail(hpId, ptId, sinDate, odrKouiKbns);

                    commonOdrInfs = CommonOdrInfListFactory(rsvkrtOdrInfs);
                    commonOdrDtls = CommonOdrInfDetailListFactory(rsvkrtOdrInfDtls);

                }


                // 来院区分
                List<CoRaiinKbnInfModel> raiinKbnInfs = finder.FindRaiinKbnInf(hpId, ptId, sinDate, raiinNo);

                if (formType == (int)CoSijisenFormType.JyusinHyo && string.IsNullOrEmpty(SystemConfig.Instance.JyusinHyoRaiinKbn) == false)
                {
                    // 受診票 来院区分指定印刷

                    bool find = false;
                    List<string> raiinKbns = SystemConfig.Instance.JyusinHyoRaiinKbn.Split(',').ToList();
                    foreach (string raiinKbn in raiinKbns)
                    {
                        List<string> splitRaiinKbn = raiinKbn.Split('=').ToList();
                        if (splitRaiinKbn.Count == 2 &&
                            raiinKbnInfs.Any(p => p.GrpId == CIUtil.StrToIntDef(splitRaiinKbn[0], 0) && p.KbnCd == CIUtil.StrToIntDef(splitRaiinKbn[1], 0)))
                        {
                            find = true;
                            break;
                        }
                    }

                    if (!find)
                    {
                        // 印刷しない
                        return new CommonReportingRequestModel();
                    }
                }

                // 最終来院日
                int lastSinDate = finder.GetLastSinDate(hpId, ptId);

                CoSijisenModel coSijisen = new CoSijisenModel(ptInf, raiinInf, commonOdrInfs, commonOdrDtls, raiinKbnInfs, raiinInfOther, lastSinDate);


                // 来院区分マスタ
                var raiinKbnMstModels = finder.FindRaiinKbnMst(hpId);

                if ((commonOdrInfs.Any()) || (formType == (int)CoSijisenFormType.JyusinHyo && printNoOdr))
                {
                    // オーダーあり or
                    // 受診票で、オーダーなしでも印刷の指示あり
                    return new SijisenMapper(formType, coSijisen, raiinKbnMstModels).GetData();
                }
                else
                {
                    return new CommonReportingRequestModel();
                }

            }
        }

        #region Factory Method
        // オーダー情報
        List<CoCommonOdrInfModel> CommonOdrInfListFactory(List<CoOdrInfModel> odrInfs)
        {
            List<CoCommonOdrInfModel> results = new List<CoCommonOdrInfModel>();

            foreach (CoOdrInfModel odrInf in odrInfs)
            {
                results.Add(CommonOdrInfFactory(odrInf));
            }

            return results;
        }
        List<CoCommonOdrInfModel> CommonOdrInfListFactory(List<CoRsvkrtOdrInfModel> odrInfs)
        {
            List<CoCommonOdrInfModel> results = new List<CoCommonOdrInfModel>();

            foreach (CoRsvkrtOdrInfModel odrInf in odrInfs)
            {
                results.Add(CommonOdrInfFactory(odrInf));
            }

            return results;
        }
        List<CoCommonOdrInfDetailModel> CommonOdrInfDetailListFactory(List<CoOdrInfDetailModel> odrDtls)
        {
            List<CoCommonOdrInfDetailModel> results = new List<CoCommonOdrInfDetailModel>();

            foreach (CoOdrInfDetailModel odrDtl in odrDtls)
            {
                results.Add(CommonOdrDtlFactory(odrDtl));
            }

            return results;
        }
        List<CoCommonOdrInfDetailModel> CommonOdrInfDetailListFactory(List<CoRsvkrtOdrInfDetailModel> odrDtls)
        {
            List<CoCommonOdrInfDetailModel> results = new List<CoCommonOdrInfDetailModel>();

            foreach (CoRsvkrtOdrInfDetailModel odrDtl in odrDtls)
            {
                results.Add(CommonOdrDtlFactory(odrDtl));
            }

            return results;
        }

        private CoCommonOdrInfModel CommonOdrInfFactory(CoOdrInfModel odrInf)
        {
            return new CoCommonOdrInfModel(
                hpId: odrInf.HpId, ptId: odrInf.PtId, sinDate: odrInf.SinDate,
                raiinNo: odrInf.RaiinNo, rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
                odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName,
                inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn, syohoSbt: odrInf.SyohoSbt,
                santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo);
        }
        private CoCommonOdrInfModel CommonOdrInfFactory(CoRsvkrtOdrInfModel odrInf)
        {
            return new CoCommonOdrInfModel(
                hpId: odrInf.HpId, ptId: odrInf.PtId, sinDate: odrInf.RsvDate,
                raiinNo: odrInf.RsvkrtNo, rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
                odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName,
                inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn, syohoSbt: odrInf.SyohoSbt,
                santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo);
        }

        private CoCommonOdrInfDetailModel CommonOdrDtlFactory(CoOdrInfDetailModel odrDtl)
        {
            return new CoCommonOdrInfDetailModel(
                hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.SinDate,
                raiinNo: odrDtl.RaiinNo, rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
                sinKouiKbn: odrDtl.SinKouiKbn, itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName,
                suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
                bunkatu: odrDtl.Bunkatu,
                materialName: odrDtl.MaterialName, containerName: odrDtl.ContainerName
                );
        }

        private CoCommonOdrInfDetailModel CommonOdrDtlFactory(CoRsvkrtOdrInfDetailModel odrDtl)
        {
            return new CoCommonOdrInfDetailModel(
                hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.RsvDate,
                raiinNo: odrDtl.RsvkrtNo, rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
                sinKouiKbn: odrDtl.SinKouiKbn, itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName,
                suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
                bunkatu: odrDtl.Bunkatu,
                materialName: odrDtl.MaterialName, containerName: odrDtl.ContainerName
                );
        }
        #endregion
    }
}
