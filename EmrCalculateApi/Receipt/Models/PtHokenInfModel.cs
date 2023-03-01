using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class PtHokenInfModel
    {
        public PtHokenInf PtHokenInf { get; } = null;

        public PtHokenInfModel(PtHokenInf ptHokenInf)
        {
            PtHokenInf = ptHokenInf;
        }

        /// <summary>
        /// 患者保険情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtHokenInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtHokenInf.PtId; }
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenId
        {
            get { return PtHokenInf.HokenId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtHokenInf.SeqNo; }
        }

        /// <summary>
        /// 保険番号
        ///  保険マスタに登録された保険番号
        /// </summary>
        public int HokenNo
        {
            get { return PtHokenInf.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        ///  保険マスタに登録された保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return PtHokenInf.HokenEdaNo; }
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get { return PtHokenInf.HokensyaNo ?? string.Empty; }
        }

        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get { return PtHokenInf.Kigo ?? string.Empty; }
        }

        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get { return PtHokenInf.Bango ?? string.Empty; }
        }

        /// <summary>
        /// 本人家族区分
        ///  1:本人
        ///  2:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return PtHokenInf.HonkeKbn; }
        }

        /// <summary>
        /// 保険区分
        ///  0:自費 
        ///  1:社保 
        ///  2:国保
        ///  
        ///  11:労災(短期給付)
        ///  12:労災(傷病年金)
        ///  13:アフターケア
        ///  14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get { return PtHokenInf.HokenKbn; }
        }

        /// <summary>
        /// 保険者番号の桁数
        ///  0桁: [0]自費保険
        ///  4桁: [01]社保
        ///  6桁: [100]国保
        ///  8桁: 保険者番号の前2桁
        ///   [67]退職国保
        ///   [39]後期高齢
        ///   [01]協会健保、[02]船員、[03,04]日雇、
        ///   [06]組合健保、[07]自衛官、
        ///   [31..34]共済組合
        ///    [31]国家公務員共済組合
        ///    [32]地方公務員等共済組合
        ///    [33]警察共済組合
        ///    [34]公立学校共済組合
        ///　    日本私立学校振興・共済事業団
        ///   [63,72..75]特定共済組合
        /// </summary>
        public string Houbetu
        {
            get { return PtHokenInf.Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 被保険者名
        /// </summary>
        public string HokensyaName
        {
            get { return PtHokenInf.HokensyaName ?? string.Empty; }
        }

        /// <summary>
        /// 被保険者郵便番号
        /// </summary>
        public string HokensyaPost
        {
            get { return PtHokenInf.HokensyaPost ?? string.Empty; }
        }

        /// <summary>
        /// 被保険者住所
        /// </summary>
        public string HokensyaAddress
        {
            get { return PtHokenInf.HokensyaAddress ?? string.Empty; }
        }

        /// <summary>
        /// 被保険者電話番号
        /// </summary>
        public string HokensyaTel
        {
            get { return PtHokenInf.HokensyaTel ?? string.Empty; }
        }

        /// <summary>
        /// 継続区分
        ///  1:任意継続
        /// </summary>
        public int KeizokuKbn
        {
            get { return PtHokenInf.KeizokuKbn; }
        }

        /// <summary>
        /// 資格取得日
        ///  yyyymmdd 
        /// </summary>
        public int SikakuDate
        {
            get { return PtHokenInf.SikakuDate; }
        }

        /// <summary>
        /// 交付日
        ///  yyyymmdd 
        /// </summary>
        public int KofuDate
        {
            get { return PtHokenInf.KofuDate; }
        }

        /// <summary>
        /// 適用開始日
        ///  yyyymmdd 
        /// </summary>
        public int StartDate
        {
            get { return PtHokenInf.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        ///  yyyymmdd 
        /// </summary>
        public int EndDate
        {
            get { return PtHokenInf.EndDate; }
        }

        /// <summary>
        /// 負担率
        ///  ※未使用
        /// </summary>
        public int Rate
        {
            get { return PtHokenInf.Rate; }
        }

        /// <summary>
        /// 一部負担限度額
        ///  ※未使用
        /// </summary>
        public int Gendogaku
        {
            get { return PtHokenInf.Gendogaku; }
        }

        /// <summary>
        /// 高額療養費区分
        ///  70歳以上
        ///   0:一般
        ///   3:上位(～2018/07)
        ///   4:低所Ⅱ
        ///   5:低所Ⅰ
        ///   6:特定収入(～2008/12)
        ///   26:現役Ⅲ
        ///   27:現役Ⅱ
        ///   28:現役Ⅰ
        ///  70歳未満
        ///   0:限度額認定証なし
        ///   17:上位[A] (～2014/12)
        ///   18:一般[B] (～2014/12)
        ///   19:低所[C] (～2014/12)
        ///   26:区ア／標準報酬月額83万円以上
        ///   27:区イ／標準報酬月額53..79万円
        ///   28:区ウ／標準報酬月額28..50万円
        ///   29:区エ／標準報酬月額26万円以下
        ///   30:区オ／低所得者
        /// </summary>
        public int KogakuKbn
        {
            get { return PtHokenInf.KogakuKbn; }
        }

        /// <summary>
        /// 高額療養費処理区分
        ///  1:高額委任払い 
        ///  2:適用区分一般
        /// </summary>
        public int KogakuType
        {
            get { return PtHokenInf.KogakuType; }
        }

        /// <summary>
        /// 限度額特例対象年月１
        ///  yyyymm
        /// </summary>
        public int TokureiYm1
        {
            get { return PtHokenInf.TokureiYm1; }
        }

        /// <summary>
        /// 限度額特例対象年月２
        ///  yyyymm
        /// </summary>
        public int TokureiYm2
        {
            get { return PtHokenInf.TokureiYm2; }
        }

        /// <summary>
        /// 多数回該当適用開始年月
        ///  yyyymm
        /// </summary>
        public int TasukaiYm
        {
            get { return PtHokenInf.TasukaiYm; }
        }

        /// <summary>
        /// 職務上区分
        ///  1:職務上
        ///  2:下船後３月以内 
        ///  3:通勤災害
        /// </summary>
        public int SyokumuKbn
        {
            get { return PtHokenInf.SyokumuKbn; }
        }

        /// <summary>
        /// 国保減免区分
        ///  1:減額 
        ///  2:免除 
        ///  3:支払猶予
        /// </summary>
        public int GenmenKbn
        {
            get { return PtHokenInf.GenmenKbn; }
        }

        /// <summary>
        /// 国保減免割合
        ///  ※不要？
        /// </summary>
        public int GenmenRate
        {
            get { return PtHokenInf.GenmenRate; }
        }

        /// <summary>
        /// 国保減免金額
        ///  ※不要？
        /// </summary>
        public int GenmenGaku
        {
            get { return PtHokenInf.GenmenGaku; }
        }

        /// <summary>
        /// 特記事項１
        /// </summary>
        public string Tokki1
        {
            get { return PtHokenInf.Tokki1 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項２
        /// </summary>
        public string Tokki2
        {
            get { return PtHokenInf.Tokki2 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項３
        /// </summary>
        public string Tokki3
        {
            get { return PtHokenInf.Tokki3 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項４
        /// </summary>
        public string Tokki4
        {
            get { return PtHokenInf.Tokki4 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項５
        /// </summary>
        public string Tokki5
        {
            get { return PtHokenInf.Tokki5 ?? string.Empty; }
        }

        /// <summary>
        /// 労災交付番号
        ///  短期給付: 労働保険番号
        ///  傷病年金: 年金証書番号
        ///  アフターケア: 健康管理手帳番号
        /// </summary>
        public string RousaiKofuNo
        {
            get { return PtHokenInf.RousaiKofuNo ?? string.Empty; }
        }

        /// <summary>
        /// 労災災害区分
        ///  1:業務中の災害 
        ///  2:通勤途上の災害
        /// </summary>
        public int RousaiSaigaiKbn
        {
            get { return PtHokenInf.RousaiSaigaiKbn; }
        }

        /// <summary>
        /// 労災事業所名
        /// </summary>
        public string RousaiJigyosyoName
        {
            get { return PtHokenInf.RousaiJigyosyoName ?? string.Empty; }
        }

        /// <summary>
        /// 労災都道府県名
        /// </summary>
        public string RousaiPrefName
        {
            get { return PtHokenInf.RousaiPrefName ?? string.Empty; }
        }

        /// <summary>
        /// 労災所在地郡市区名
        /// </summary>
        public string RousaiCityName
        {
            get { return PtHokenInf.RousaiCityName ?? string.Empty; }
        }

        /// <summary>
        /// 労災傷病年月日
        ///  yyyymmdd 
        /// </summary>
        public int RousaiSyobyoDate
        {
            get { return PtHokenInf.RousaiSyobyoDate; }
        }

        /// <summary>
        /// 労災傷病コード
        /// </summary>
        public string RousaiSyobyoCd
        {
            get { return PtHokenInf.RousaiSyobyoCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災労働局コード
        /// </summary>
        public string RousaiRoudouCd
        {
            get { return PtHokenInf.RousaiRoudouCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災監督署コード
        /// </summary>
        public string RousaiKantokuCd
        {
            get { return PtHokenInf.RousaiKantokuCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災レセ請求回数
        /// </summary>
        public int RousaiReceCount
        {
            get { return PtHokenInf.RousaiReceCount; }
        }

        /// <summary>
        /// 療養開始日
        /// </summary>
        public int RyoyoStartDate
        {
            get { return PtHokenInf.RyoyoStartDate; }
        }

        /// <summary>
        /// 療養終了日
        /// </summary>
        public int RyoyoEndDate
        {
            get { return PtHokenInf.RyoyoEndDate; }
        }

        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get { return PtHokenInf.JibaiHokenName ?? string.Empty; }
        }

        /// <summary>
        /// 自賠保険担当者
        /// </summary>
        public string JibaiHokenTanto
        {
            get { return PtHokenInf.JibaiHokenTanto ?? string.Empty; }
        }

        /// <summary>
        /// 自賠保険連絡先
        /// </summary>
        public string JibaiHokenTel
        {
            get { return PtHokenInf.JibaiHokenTel ?? string.Empty; }
        }

        /// <summary>
        /// 自賠受傷日
        ///  yyyymmdd 
        /// </summary>
        public int JibaiJyusyouDate
        {
            get { return PtHokenInf.JibaiJyusyouDate; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtHokenInf.IsDeleted; }
        }

        /// <summary>
        /// 作成日時 
        /// </summary>
        public DateTime CreateDate
        {
            get { return PtHokenInf.CreateDate; }
        }

        /// <summary>
        /// 作成者  
        /// </summary>
        public int CreateId
        {
            get { return PtHokenInf.CreateId; }
        }

        /// <summary>
        /// 作成端末   
        /// </summary>
        public string CreateMachine
        {
            get { return PtHokenInf.CreateMachine ?? string.Empty; }
        }

        /// <summary>
        /// 更新日時   
        /// </summary>
        public DateTime UpdateDate
        {
            get { return PtHokenInf.UpdateDate; }
        }

        /// <summary>
        /// 更新者   
        /// </summary>
        public int UpdateId
        {
            get { return PtHokenInf.UpdateId; }
        }

        /// <summary>
        /// 更新端末   
        /// </summary>
        public string UpdateMachine
        {
            get { return PtHokenInf.UpdateMachine ?? string.Empty; }
        }


    }

}
