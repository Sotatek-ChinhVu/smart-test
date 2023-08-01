﻿using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.ReceSeikyu
{
    public class ReceSeikyuModel
    {
        public ReceSeikyuModel(int sinDate, int hpId, long ptId, string ptName, int sinYm, int receListSinYm, int hokenId, string hokensyaNo, int seqNo, int seikyuYm, int seikyuKbn, int preHokenId, string cmt, long ptNum, int hokenKbn, string houbetu, int hokenStartDate, int hokenEndDate, bool isModified, int originSeikyuYm, int originSinYm, bool isAddNew, int isDeleted, bool isChecked, List<RecedenHenJiyuuModel> listRecedenHenJiyuuModel)
        {
            SinDay = sinDate;
            HpId = hpId;
            PtId = ptId;
            PtName = ptName;
            SinYm = sinYm;
            ReceListSinYm = receListSinYm;
            HokenId = hokenId;
            HokensyaNo = hokensyaNo;
            SeqNo = seqNo;
            SeikyuYm = seikyuYm;
            SeikyuKbn = seikyuKbn;
            PreHokenId = preHokenId;
            Cmt = cmt;
            PtNum = ptNum;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            HokenStartDate = hokenStartDate;
            HokenEndDate = hokenEndDate;
            IsModified = isModified;
            OriginSeikyuYm = originSeikyuYm;
            OriginSinYm = originSinYm;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
            IsChecked = isChecked;
            ListRecedenHenJiyuuModel = listRecedenHenJiyuuModel;
        }

        public ReceSeikyuModel(long ptId, int sinYm, int hokenId, long ptNum, int seikyuKbn)
        {
            PtId = ptId;
            PtName = string.Empty;
            SinYm = sinYm;
            HokenId = hokenId;
            HokensyaNo = string.Empty;
            SeikyuKbn = seikyuKbn;
            Cmt = string.Empty;
            PtNum = ptNum;
            Houbetu = string.Empty;
            ListRecedenHenJiyuuModel = new();
        }

        public ReceSeikyuModel(long ptId, int seikyuKbn, int seikyuYm, int sinYm, int hokenId)
        {
            PtId = ptId;
            SeikyuYm = seikyuYm;
            SinYm = sinYm;
            HokenId = hokenId;
            SeikyuKbn = seikyuKbn;
            HokensyaNo = string.Empty;
            Cmt = string.Empty;
            Houbetu = string.Empty;
            PtName = string.Empty;
            ListRecedenHenJiyuuModel = new();
        }

        public ReceSeikyuModel()
        {
            PtName = string.Empty;
            HokensyaNo = string.Empty;
            Cmt = string.Empty;
            Houbetu = string.Empty;
            ListRecedenHenJiyuuModel = new();
        }

        public ReceSeikyuModel UpdateReceSeikyuModel(int isDeleted)
        {
            IsDeleted = isDeleted;
            return this;
        }

        public ReceSeikyuModel UpdateReceSeikyuModel(int preHokenId, string cmt)
        {
            PreHokenId = preHokenId;
            Cmt = cmt;
            return this;
        }

        public int SinDay { get; private set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId { get; private set; }

        public string PtName { get; private set; }

        public int ReceListSinYm { get; private set; }

        public int SinYm { get; private set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId { get; private set; }

        public string HokensyaNo { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm { get; private set; }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn { get; private set; }

        /// <summary>
        /// 前回請求保険ID
        /// 
        /// </summary>
        public int PreHokenId { get; private set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt { get; private set; }

        public bool CheckDefaultValue()
        {
            return HpId == 0 && SinYm == 0;
        }

        private bool _IsChecked;

        public bool IsChecked
        {
            private set
            {
                _IsChecked = value;
                if (!value)
                {
                    SeikyuYm = 999999;
                }
                else if (IsCompletedSeikyu)
                {
                    SeikyuYm = OriginSeikyuYm;
                }
                else if (SeikyuYm == 0 || SeikyuYm == 999999)
                {
                    SeikyuYm = ReceListSinYm;
                }
            }
            get => _IsChecked;
        }

        public bool IsEnableCheckBox
        {
            get => SinYm != ReceListSinYm;
        }

        public bool IsCompletedSeikyu
        {
            get => OriginSeikyuYm != 999999;
        }

        public bool IsNotCompletedSeikyu
        {
            get => OriginSeikyuYm == 999999;
        }

        public string SeikyuYmDisplay
        {
            get
            {
                if (SeikyuYm == 999999 || SeikyuYm == 0 || SeikyuYm / 100000 == 0)
                {
                    return string.Empty;
                }
                string sSinYm = SeikyuYm.AsString();
                return sSinYm.Insert(4, "/");
            }
        }

        public long PtNum { get; private set; }

        public string HenHokenName
        {
            get
            {
                if (true)
                {
                    if (ListRecedenHenJiyuuModel != null && ListRecedenHenJiyuuModel.Any())
                    {
                        var model = ListRecedenHenJiyuuModel.FirstOrDefault();
                        if (model is null) return string.Empty;
                        else return model.HokenSentaku;
                    }
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 返戻事由
        /// 
        /// </summary>
        public string HenreiJiyuu
        {
            get
            {
                if (ListRecedenHenJiyuuModel != null && ListRecedenHenJiyuuModel.Count > 0)
                {
                    var model = ListRecedenHenJiyuuModel.FirstOrDefault();
                    if (model is null) return string.Empty;
                    else return model.HenreiJiyuu;
                }
                return string.Empty;
            }
        }

        public int HokenKbn { get; private set; }

        public string Houbetu { get; private set; }

        public string HokenSentaku
        {
            get
            {
                string result = string.Empty;
                if (HokenId == 0)
                {
                    return string.Empty;
                }
                result += HokenId.ToString().PadLeft(2, '0');
                result += " ";
                if (Houbetu == null)
                {
                    return string.Empty;
                }

                switch (HokenKbn)
                {
                    case 0:
                        if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                        {
                            result += "自費";
                        }
                        else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                        {
                            result += "自費レセ";
                        }
                        else
                        {
                            result += "自費";
                        }
                        break;
                    case 1:
                        if (Houbetu == HokenConstant.HOUBETU_NASHI)
                        {
                            result += "公費";
                        }
                        else
                        {
                            result += "社保";
                        }
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result += "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result += "退職";
                        }
                        else
                        {
                            result += "国保";
                        }
                        break;
                    case 11:
                        result += "労災（短期給付）";
                        break;
                    case 12:
                        result += "労災（傷病年金）";
                        break;
                    case 13:
                        result += "労災（アフターケア）";
                        break;
                    case 14:
                        result += "自賠責";
                        break;
                }

                if (this.HokenKbn != 0)
                {
                    result += "(";
                    if (this.HokenKbn == 1)
                    {
                        result += "本人";
                    }
                    else
                    {
                        result += "家族";
                    }
                    result += ")";
                }

                string sBuff;
                if (HokenStartDate > 0)
                {
                    sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenStartDate));
                }
                else
                {
                    sBuff = string.Format("{0, -11}", " ");
                }

                sBuff += " ～ ";

                if (HokenEndDate > 0 && HokenEndDate < 99999999)
                {
                    sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenEndDate));
                }
                else
                {
                    sBuff += string.Format("{0, -11}", " ");
                }

                return result + " " + sBuff;
            }
        }

        public int HokenStartDate { get; private set; }

        public int HokenEndDate { get; private set; }

        public bool IsModified { get; private set; }

        public int OriginSeikyuYm { get; private set; }

        public int OriginSinYm { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RecedenHenJiyuuModel> ListRecedenHenJiyuuModel { get; private set; }

        public bool IsDefaultValue => CheckDefaultValue();

        public string SeikyuKbnDisplay
        {
            get => SeikyuKbns.ContainsKey(SeikyuKbn) ? SeikyuKbns[SeikyuKbn] : string.Empty;
        }

        private Dictionary<int, string> SeikyuKbns => new Dictionary<int, string>()
        {
            { 1,"月遅れ" },
            { 2,"返戻" },
            { 3,"電算返戻" }
        };

        public void SetSeikyuYm(int value)
        {
            this.SeikyuYm = value;
        }
    }
}
