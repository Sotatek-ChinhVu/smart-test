using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Constants;
using Helper.Extension;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Receipt.Models
{

    public class SyobyoDataModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateReceipt;

        private PtByomei PtByomei { get;} = null;
        private int _sinYm;
        private int _outputYm;

        IEmrLogger _emrLogger;

        public SyobyoDataModel(PtByomei ptByomei, int sinYm, int outputYm, IEmrLogger emrLogger)
        {
            PtByomei = ptByomei;
            _sinYm = sinYm;
            _outputYm = outputYm;

            _emrLogger = emrLogger;
        }

        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "SY"; }
        }
        /// <summary>
        /// 病名コード
        /// </summary>
        public string ByomeiCd
        {
            get { return (PtByomei.ByomeiCd ?? string.Empty) == "" ? "0000999" : PtByomei.ByomeiCd; }
        }
        /// <summary>
        /// 診療開始日
        /// </summary>
        public int StartDate
        {
            get { return PtByomei.StartDate; }
        }
        public int StartDateDsp
        {
            get { return CIUtil.SDateToWDate(PtByomei.StartDate); }
        }
        /// <summary>
        /// 診療開始日（和暦）
        /// </summary>
        public string WStartDate
        {
            get
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(PtByomei.StartDate);
                string ret = $"{ wareki.Gengo}{wareki.Year, 2}年{wareki.Month, 2}月{wareki.Day,2}日";
                return ret;
            }
        }
        /// <summary>
        /// レセ電転帰区分
        /// </summary>
        public int ReceTenkiKbn
        {
            get
            {
                int ret = ReceTenkiKbnConst.Continued;
                if (PtByomei.TogetuByomei == 1)
                {
                    ret = ReceTenkiKbnConst.Continued;
                }
                else if (PtByomei.TenkiKbn == TenkiKbnConst.Other)
                {
                    // その他は、継続で出力
                    ret = ReceTenkiKbnConst.Continued;
                }
                else if (PtByomei.TenkiDate / 100 > _sinYm)
                {
                    ret = ReceTenkiKbnConst.Continued;
                }
                else
                {
                    switch(PtByomei.TenkiKbn)
                    {
                        case TenkiKbnConst.Continued:
                            ret = ReceTenkiKbnConst.Continued;
                            break;
                        case TenkiKbnConst.Cured:
                            ret = ReceTenkiKbnConst.Cured;
                            break;
                        case TenkiKbnConst.Canceled:
                            ret = ReceTenkiKbnConst.Canceled;
                            break;
                        case TenkiKbnConst.Dead:
                            ret = ReceTenkiKbnConst.Dead;
                            break;
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 紙レセ転帰区分
        /// </summary>
        public int KamiReceTenkiKbn
        {
            get
            {
                int ret = ReceTenkiKbnConst.Continued;
                if (PtByomei.TogetuByomei == 1)
                {
                    ret = ReceTenkiKbnConst.Continued;
                }
                else if (PtByomei.TenkiDate / 100 > _sinYm)
                {
                    ret = ReceTenkiKbnConst.Continued;
                }
                else
                {
                    switch (PtByomei.TenkiKbn)
                    {
                        case TenkiKbnConst.Continued:
                            ret = ReceTenkiKbnConst.Continued;
                            break;
                        case TenkiKbnConst.Cured:
                            ret = ReceTenkiKbnConst.Cured;
                            break;
                        case TenkiKbnConst.Canceled:
                            ret = ReceTenkiKbnConst.Canceled;
                            break;
                        case TenkiKbnConst.Dead:
                            ret = ReceTenkiKbnConst.Dead;
                            break;
                        case TenkiKbnConst.Other:
                            ret = ReceTenkiKbnConst.Other;
                            break;
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 転帰日
        /// </summary>
        public int TenkiDate
        {
            get { return PtByomei.TenkiDate; }
        }

        /// <summary>
        /// 紙レセ用転帰区分
        /// </summary>
        public string ReceTenki
        {
            get
            {
                string ret = "";
                switch(KamiReceTenkiKbn)
                {
                    case ReceTenkiKbnConst.Cured:
                        ret = "治ゆ";
                        break;
                    case ReceTenkiKbnConst.Dead:
                        ret = "死亡";
                        break;
                    case ReceTenkiKbnConst.Canceled:
                        ret = "中止";
                        break;
                    case ReceTenkiKbnConst.Other:
                        ret = "他　";
                        break;
                }
                return ret;
            }
        }
        /// <summary>
        /// 和暦転帰日（転帰設定がない場合は空文字）
        /// </summary>
        public string WTenkiDate
        {
            get
            {
                string ret = "";
                if(TenkiDate > 0 && KamiReceTenkiKbn > ReceTenkiKbnConst.Continued)
                {
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(PtByomei.TenkiDate);
                    ret = $"{ wareki.Gengo}{wareki.Year,2}年{wareki.Month,2}月{wareki.Day,2}日";                    
                }
                return ret;
            }
        }
        /// <summary>
        /// 並び順
        /// </summary>
        public long SortNo
        {
            get => PtByomei.SortNo;
        }
        /// <summary>
        /// 修飾語コード
        /// </summary>
        public string SyusyokuCd
        {
            get
            {
                string ret = "";

                ret = PtByomei.SyusyokuCd1.AsString().Trim() +
                      PtByomei.SyusyokuCd2.AsString().Trim() +
                      PtByomei.SyusyokuCd3.AsString().Trim() +
                      PtByomei.SyusyokuCd4.AsString().Trim() +
                      PtByomei.SyusyokuCd5.AsString().Trim() +
                      PtByomei.SyusyokuCd6.AsString().Trim() +
                      PtByomei.SyusyokuCd7.AsString().Trim() +
                      PtByomei.SyusyokuCd8.AsString().Trim() +
                      PtByomei.SyusyokuCd9.AsString().Trim() +
                      PtByomei.SyusyokuCd10.AsString().Trim() +
                      PtByomei.SyusyokuCd11.AsString().Trim() +
                      PtByomei.SyusyokuCd12.AsString().Trim() +
                      PtByomei.SyusyokuCd13.AsString().Trim() +
                      PtByomei.SyusyokuCd14.AsString().Trim() +
                      PtByomei.SyusyokuCd15.AsString().Trim() +
                      PtByomei.SyusyokuCd16.AsString().Trim() +
                      PtByomei.SyusyokuCd17.AsString().Trim() +
                      PtByomei.SyusyokuCd18.AsString().Trim() +
                      PtByomei.SyusyokuCd19.AsString().Trim() +
                      PtByomei.SyusyokuCd20.AsString().Trim();

                return ret;
            }
        }
        /// <summary>
        /// 病名
        /// </summary>
        public string Byomei
        {
            get { return PtByomei.Byomei ?? string.Empty; }
        }
        /// <summary>
        /// 紙レセ用病名
        /// </summary>
        public string ReceByomei
        {
            get
            {
                string ret = "";
                if(SyubyoKbn == 1)
                {
                    ret += "(主)";
                }
                ret += PtByomei.Byomei ?? string.Empty;
                if (string.IsNullOrEmpty(HosokuCmt) == false)
                {
                    ret += $"({HosokuCmt})";
                }
                return ret;
            }
        }

        /// <summary>
        /// 主病名コード
        /// </summary>
        public int SyubyoKbn
        {
            get { return PtByomei.SyubyoKbn; }
        }
        /// <summary>
        /// 補足コメント
        /// </summary>
        public string HosokuCmt
        {
            get { return PtByomei.HosokuCmt ?? string.Empty; }
        }
        /// <summary>
        /// SYレコード
        /// </summary>
        public string SYRecord
        {
            get
            {
                // 診療開始日
                int _getStartDate()
                {
                    int result = StartDate;
                    if (_outputYm < 202005)
                    {
                        result = StartDateDsp;
                    }
                    return result;
                }

                string ret = "";

                // レコード識別情報
                ret = RecId;
                // 傷病名コード
                ret += "," + ByomeiCd;
                // 診療開始日
                //ret += "," + StartDateDsp.ToString();
                ret += "," + _getStartDate().ToString();

                // 転帰区分
                if (PtByomei.TenkiDate / 100 > _sinYm || PtByomei.TogetuByomei == 1)
                {
                    // 転帰日が当月末以降、または当月病名の場合は継続
                    ret += ",1";
                }
                else
                {
                    ret += "," + ReceTenkiKbn.ToString();
                }

                // 修飾語コード
                if (ByomeiCd == "0000999")
                {
                    // 未コード化傷病名の場合、修飾語コードは記録しない
                    ret += ",";                    
                }
                else
                {
                    ret += "," + SyusyokuCd;
                }

                // 傷病名称
                if (ByomeiCd == "0000999")
                {
                    // 未コード化傷病名の場合、記録する
                    string byomei = CIUtil.ToWide(Byomei);
                    string refByomei = "";
                    string badByomei = "";

                    if (CIUtil.IsUntilJISKanjiLevel2(byomei, ref refByomei, ref badByomei) == false)
                    {
                        _emrLogger.WriteLogMsg( this, "SYRecord",
                            string.Format("Byomei is include bad charcter PtId:{0} Byomei:{1} badCharcters:{2}",
                                PtByomei.PtId, byomei, badByomei));

                        byomei = refByomei;
                    }

                    ret += "," + byomei;
                }
                else
                {
                    ret += ",";
                }

                // 主傷病
                if (SyubyoKbn > 0)
                {
                    ret += ",01";
                }
                else
                {
                    ret += ",";
                }
                // 補足コメント
                string hosokuCmt = CIUtil.ToWide(HosokuCmt);
                string refHosokuCmt = "";
                string badHosokuCmt = "";

                if (CIUtil.IsUntilJISKanjiLevel2(hosokuCmt, ref refHosokuCmt, ref badHosokuCmt) == false)
                {
                    _emrLogger.WriteLogMsg( this, "SYRecord",
                        string.Format("HosokuCmt is include bad charcter PtId:{0} HosokuCmt:{1} badCharcters:{2}",
                            PtByomei.PtId, hosokuCmt, badHosokuCmt));

                    hosokuCmt = refHosokuCmt;
                }

                ret += "," + hosokuCmt;

                return ret;
            }
        }

        public string RousaiSYRecord
        {
            get
            {
                // 診療開始日
                int _getStartDate()
                {
                    int result = StartDate;
                    if(_outputYm < 202005)
                    {
                        result = StartDateDsp;
                    }
                    return result;
                }


                string ret = "";

                // レコード識別情報
                ret = RecId;
                // 傷病名コード
                ret += "," + ByomeiCd;
                // 診療開始日
                //ret += "," + StartDateDsp.ToString();
                ret += "," + _getStartDate().ToString();

                // 予備
                ret += ",";

                // 修飾語コード
                if (ByomeiCd == "0000999")
                {
                    // 未コード化傷病名の場合、修飾語コードは記録しない
                    ret += ",";
                }
                else
                {
                    ret += "," + SyusyokuCd;
                }

                // 傷病名称
                if (ByomeiCd == "0000999")
                {
                    // 未コード化傷病名の場合、記録する
                    string byomei = CIUtil.ToWide(Byomei);
                    string refByomei = "";
                    string badByomei = "";

                    if (CIUtil.IsUntilJISKanjiLevel2(byomei, ref refByomei, ref badByomei) == false)
                    {
                        _emrLogger.WriteLogMsg( this, "SYRecord",
                            string.Format("Byomei is include bad charcter PtId:{0} Byomei:{1} badCharcters:{2}",
                                PtByomei.PtId, byomei, badByomei));

                        byomei = refByomei;
                    }

                    ret += "," + byomei;
                }
                else
                {
                    ret += ",";
                }

                // 主傷病
                if (SyubyoKbn > 0)
                {
                    ret += ",01";
                }
                else
                {
                    ret += ",";
                }
                // 補足コメント
                string hosokuCmt = CIUtil.ToWide(HosokuCmt);
                string refHosokuCmt = "";
                string badHosokuCmt = "";

                if (CIUtil.IsUntilJISKanjiLevel2(hosokuCmt, ref refHosokuCmt, ref badHosokuCmt) == false)
                {
                    _emrLogger.WriteLogMsg( this, "SYRecord",
                        string.Format("HosokuCmt is include bad charcter PtId:{0} HosokuCmt:{1} badCharcters:{2}",
                            PtByomei.PtId, hosokuCmt, badHosokuCmt));

                    hosokuCmt = refHosokuCmt;
                }

                ret += "," + hosokuCmt;

                return ret;
            }
        }
    }
}
