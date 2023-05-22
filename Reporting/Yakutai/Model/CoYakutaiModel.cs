﻿using Reporting.CommonMasters.Config;

namespace Reporting.Yakutai.Model
{
    public class CoYakutaiModel
    {
        private readonly ISystemConfig _systemConfig;

        public CoYakutaiModel(ISystemConfig systemConfig)
        {
            _systemConfig = systemConfig;
        }
        // 医療機関情報
        public CoHpInfModel HpInfModel;

        // 患者情報
        public CoPtInfModel PtInfModel;

        // 来院情報
        public CoRaiinInfModel RaiinInfModel;

        // オーダー情報
        public List<CoOdrInfModel> OdrInfModels;

        // オーダー情報詳細
        public List<CoOdrInfDetailModel> OdrInfDetailModels;

        // 1回量表示単位マスタ
        public List<CoSingleDoseMstModel> SingleDoseMstModels;
        public CoYakutaiModel(CoHpInfModel hpInf, CoPtInfModel ptInf, CoRaiinInfModel raiinInf,
            List<CoOdrInfModel> odrInfs, List<CoOdrInfDetailModel> odrDtls, List<CoSingleDoseMstModel> singleDoses)
        {
            HpInfModel = hpInf;
            PtInfModel = ptInf;
            RaiinInfModel = raiinInf;
            OdrInfModels = odrInfs;
            OdrInfDetailModels = odrDtls;
            SingleDoseMstModels = singleDoses;

            YohoComments = new List<string>();
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInfModel.PtNum;
        }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => PtInfModel.PtId;
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInfModel.Name ?? "";
        }
        /// <summary>
        /// 患者カナ氏名
        /// </summary>
        public string PtKanaName
        {
            get => PtInfModel.KanaName ?? "";
        }
        /// <summary>
        /// 診療科名
        /// </summary>
        public string KaName
        {
            get => RaiinInfModel != null ? RaiinInfModel.KaName : "";
        }
        /// <summary>
        /// 担当医氏名
        /// </summary>
        public string TantoName
        {
            get => RaiinInfModel != null ? RaiinInfModel.TantoName : "";
        }
        /// <summary>
        /// 受付番号
        /// </summary>
        public int UketukeNo
        {
            get => RaiinInfModel != null ? RaiinInfModel.UketukeNo : 0;
        }
        /// <summary>
        /// 医療機関名
        /// </summary>
        public string HpName
        {
            get => HpInfModel != null ? HpInfModel.HpName : "";
        }
        /// <summary>
        /// 医療機関電話番号
        /// </summary>
        public string HpTel
        {
            get => HpInfModel != null ? HpInfModel.Tel : "";
        }

        public string HpAddress
        {
            get => HpInfModel != null ? HpInfModel.Address : "";
        }
        public string HpPostCd
        {
            get => HpInfModel != null ? HpInfModel.PostCd : "";
        }
        public string HpPostCdDsp
        {
            get => HpInfModel != null ? HpInfModel.PostCdDsp : "";
        }
        public string HpFaxNo
        {
            get => HpInfModel != null ? HpInfModel.FaxNo : "";
        }
        public string HpOtherContacts
        {
            get => HpInfModel != null ? HpInfModel.OtherContacts : "";
        }
        public string DrugKbn
        {
            get
            {
                string ret = "";

                switch (DrugKbnCd)
                {
                    case 21:
                        ret = "内服薬";
                        break;
                    case 22:
                        ret = "頓服薬";
                        break;
                    case 23:
                        ret = "外用薬";
                        break;
                }

                return ret;
            }
        }
        public int DrugKbnCd
        {
            get
            {
                int ret = 0;

                if (OdrInfModels != null && OdrInfModels.Any())
                {
                    ret = OdrInfModels.First().OdrKouiKbn;
                }
                return ret;
            }
        }
        /// <summary>
        /// 用法名称
        /// </summary>
        public string Yoho { get; set; }
        /// <summary>
        /// 用法の数量
        /// </summary>
        public double YohoSuryo { get; set; }
        /// <summary>
        /// 用法の単位
        /// </summary>
        public string YohoTani { get; set; }
        /// <summary>
        /// 用法コメント
        /// </summary>
        public List<string> YohoComments { get; set; }
        /// <summary>
        /// 用時設定が均等でない設定の用法が１つでもあればtrueを返す
        /// </summary>
        public bool IsFukinto
        {
            get
            {
                bool ret = false;

                foreach (CoOdrInfDetailModel odrDtl in OdrInfDetailModels.FindAll(p => p.YohoKbn == 1))
                {
                    double tmp = 0;

                    void _checkSetFukuyoryo(double fukuyoryo)
                    {
                        if (ret == false)
                        {
                            if (fukuyoryo > 0)
                            {
                                if (tmp == 0)
                                {
                                    tmp = fukuyoryo;
                                }
                                else if (tmp != fukuyoryo)
                                {
                                    ret = true;
                                }
                            }
                        }
                    }

                    _checkSetFukuyoryo(odrDtl.FukuyoRise);
                    _checkSetFukuyoryo(odrDtl.FukuyoMorning);
                    _checkSetFukuyoryo(odrDtl.FukuyoDaytime);
                    _checkSetFukuyoryo(odrDtl.FukuyoNight);
                    _checkSetFukuyoryo(odrDtl.FukuyoSleep);

                    if (ret)
                    {
                        break;
                    }
                }

                return ret;

            }
        }
        /// <summary>
        /// 1回量に換算するための数量
        /// </summary>
        public double CnvToOnceValue
        {
            get
            {
                double ret = 0;

                CoOdrInfDetailModel odrDtl = OdrInfDetailModels.Find(p => p.YohoKbn == 1);

                if (odrDtl != null)
                {
                    ret =
                        odrDtl.FukuyoRise +
                        odrDtl.FukuyoMorning +
                        odrDtl.FukuyoDaytime +
                        odrDtl.FukuyoNight +
                        odrDtl.FukuyoSleep;
                }

                return ret;

            }
        }
        /// <summary>
        /// １回量均等の場合の値
        /// </summary>
        public double OnceValue
        {
            get
            {
                double ret = 0;

                if (IsFukinto == false)
                {
                    CoOdrInfDetailModel odrDtl = OdrInfDetailModels.Find(p => p.YohoKbn == 1);

                    if (odrDtl != null)
                    {
                        if (odrDtl.FukuyoRise > 0)
                        {
                            ret = odrDtl.FukuyoRise;
                        }
                        else if (odrDtl.FukuyoMorning > 0)
                        {
                            ret = odrDtl.FukuyoMorning;
                        }
                        else if (odrDtl.FukuyoDaytime > 0)
                        {
                            ret = odrDtl.FukuyoDaytime;
                        }
                        else if (odrDtl.FukuyoNight > 0)
                        {
                            ret = odrDtl.FukuyoNight;
                        }
                        else if (odrDtl.FukuyoSleep > 0)
                        {
                            ret = odrDtl.FukuyoSleep;
                        }
                    }
                }
                return ret;

            }
        }
        /// <summary>
        /// 1回量で出力できるかどうか
        /// </summary>
        public bool IsOnceAmount
        {
            get
            {
                bool ret = true;

                if (_systemConfig.YakutaiOnceAmount() == 1 &&
                    CnvToOnceValue > 0 &&
                    SingleDoseMstModels != null &&
                    SingleDoseMstModels.Any() &&
                    IsFukinto == false)
                {
                    foreach (CoOdrInfDetailModel odrDtl in OdrInfDetailModels.FindAll(p => p.DrugKbn > 0))
                    {
                        if (SingleDoseMstModels.Any(p => p.UnitName == odrDtl.UnitNameDsp) == false)
                        {
                            ret = false;
                            break;
                        }
                    }
                }
                else
                {
                    ret = false;
                }

                return ret;
            }
        }
        /// <summary>
        /// 服用量（薬袋の用紙判断に使用）
        /// </summary>
        public double Fukuyoryo
        {
            get
            {
                double ret = 0;

                double yohoSuryo = 1;

                if (OdrInfDetailModels.Any(p => p.YohoKbn == 1))
                {
                    yohoSuryo = OdrInfDetailModels.Find(p => p.YohoKbn == 1).Suryo;
                }

                foreach (CoOdrInfDetailModel odrDtl in OdrInfDetailModels.FindAll(p => p.DrugKbn > 0))
                {
                    if (new List<int> { 21, 22 }.Contains(DrugKbnCd))
                    {
                        // 内服薬・頓服
                        ret += odrDtl.Fukuyoryo * yohoSuryo;
                    }
                    else
                    {
                        ret += odrDtl.Fukuyoryo;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 服用時点別一包化の薬袋かどうか
        /// </summary>
        public bool IsFukuyojiIppo { get; set; } = false;
    }
}
