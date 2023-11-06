using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta9000.DB;
using Reporting.Statistics.Sta9000.Mapper;
using Reporting.Statistics.Sta9000.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace Reporting.Statistics.Sta9000.Service;

public class Sta9000CoReportService : ISta9000CoReportService
{
    #region Constant
    private struct ReportInf
    {
        public int MaxRow { get; set; }
        public string FormFileName { get; set; }

        public ReportInf(int maxRow, string formFileName)
        {
            MaxRow = maxRow;
            FormFileName = formFileName;
        }
    }

    private readonly List<ReportInf> reportInfs = new()
        {
            new ReportInf(43, "sta9000a.rse"),  //患者一覧表
            new ReportInf(43, "sta9000b.rse"),  //患者一覧表（処方・病名一覧）
            new ReportInf(21, "sta9000c.rse"),  //宛名ラベル
            new ReportInf(43, "sta9000d.rse")   //患者来院歴一覧
        };

    public struct PutColumn
    {
        public int DataType { get; set; }
        public string ColName { get; set; }
        public string JpName { get; set; }

        public PutColumn(int dataType, string colName, string jpName)
        {
            DataType = dataType;
            ColName = colName;
            JpName = jpName;
        }
    }

    //カラム一覧
    private readonly List<PutColumn> putColumns = new()
        {
            #region 患者情報
            new PutColumn(0, "PtNum", "患者番号"),
            new PutColumn(0, "PtName", "氏名"),
            new PutColumn(0, "KanaName", "カナ氏名"),
            new PutColumn(0, "Sex", "性別"),
            new PutColumn(0, "Birthday", "生年月日(西暦)"),
            new PutColumn(0, "BirthdayW", "生年月日(和暦)"),
            new PutColumn(0, "BirthdayWS", "生年月日(和暦西暦)"),
            new PutColumn(0, "Age", "年齢"),
            new PutColumn(0, "AgeMonth", "年齢(月)"),
            new PutColumn(0, "DeathDate", "死亡日"),
            new PutColumn(0, "HomePostMark", "郵便マーク"),
            new PutColumn(0, "HomePost", "郵便番号"),
            new PutColumn(0, "HomeAddress", "住所"),
            new PutColumn(0, "HomeAddress1", "住所１"),
            new PutColumn(0, "HomeAddress2", "住所２"),
            new PutColumn(0, "Tel", "電話番号"),
            new PutColumn(0, "Tel1", "電話番号１"),
            new PutColumn(0, "Tel2", "電話番号２"),
            new PutColumn(0, "Mail", "メールアドレス"),
            new PutColumn(0, "Setainusi", "世帯主名"),
            new PutColumn(0, "Zokugara", "世帯主との続柄"),
            new PutColumn(0, "Job", "職業"),
            new PutColumn(0, "RenrakuName", "連絡先名称"),
            new PutColumn(0, "RenrakuPost", "連絡先郵便番号"),
            new PutColumn(0, "RenrakuAddress1", "連絡先住所１"),
            new PutColumn(0, "RenrakuAddress2", "連絡先住所２"),
            new PutColumn(0, "RenrakuTel", "連絡先電話番号"),
            new PutColumn(0, "RenrakuMemo", "連絡先備考"),
            new PutColumn(0, "OfficeName", "勤務先名称"),
            new PutColumn(0, "OfficePost", "勤務先郵便番号"),
            new PutColumn(0, "OfficeAddress1", "勤務先住所１"),
            new PutColumn(0, "OfficeAddress2", "勤務先住所２"),
            new PutColumn(0, "OfficeTel", "勤務先電話番号"),
            new PutColumn(0, "OfficeMemo", "勤務先備考"),
            new PutColumn(0, "IsRyosyuDetail", "領収書明細"),
            new PutColumn(0, "PrimaryDoctor", "主治医"),
            new PutColumn(0, "IsTester", "テスト患者区分"),
            new PutColumn(0, "CreateDate", "患者登録日時"),
            new PutColumn(0, "FirstVisitDate", "初回来院日"),
            new PutColumn(0, "LastVisitDate", "最終来院日"),
            new PutColumn(0, "PtCmt", "患者コメント"),
            new PutColumn(0, "AdjFutan", "調整額"),
            new PutColumn(0, "AdjRate", "調整率"),
            new PutColumn(0, "AutoSantei", "自動算定"),
            #endregion
            #region 保険情報
            new PutColumn(1, "HokenPid", "保険組合せID"),
            new PutColumn(1, "HokenKbn", "保険区分"),
            new PutColumn(1, "HokenKbnName", "保険区分名称"),
            new PutColumn(1, "HokenSbtCd", "保険種別コード"),
            new PutColumn(1, "HokenSbtCdName", "保険種別コード名称"),
            new PutColumn(1, "HokenMemo", "保険メモ"),
            new PutColumn(1, "StartDate", "適用開始日"),
            new PutColumn(1, "EndDate", "適用終了日"),
            new PutColumn(1, "HokenId", "保険ID"),
            new PutColumn(1, "HokenNo", "保険番号"),
            new PutColumn(1, "HokenEdaNo", "保険番号枝番"),
            new PutColumn(1, "HokensyaNo", "保険者番号"),
            new PutColumn(1, "Kigo", "記号"),
            new PutColumn(1, "Bango", "番号"),
            new PutColumn(1, "EdaNo", "枝番"),
            new PutColumn(1, "HonkeKbn", "本人家族区分"),
            new PutColumn(1, "Houbetu", "法別番号"),
            new PutColumn(1, "HokensyaName", "被保険者名"),
            new PutColumn(1, "HokensyaPost", "被保険者郵便番号"),
            new PutColumn(1, "HokensyaAddress", "被保険者住所"),
            new PutColumn(1, "HokensyaTel", "被保険者電話番号"),
            new PutColumn(1, "KeizokuKbn", "継続区分"),
            new PutColumn(1, "SikakuDate", "資格取得日"),
            new PutColumn(1, "KofuDate", "交付日"),
            new PutColumn(1, "KogakuKbn", "高額療養費区分"),
            new PutColumn(1, "KogakuType", "高額療養費処理区分"),
            new PutColumn(1, "TokureiYm1", "限度額特例対象年月１"),
            new PutColumn(1, "TokureiYm2", "限度額特例対象年月２"),
            new PutColumn(1, "TasukaiYm", "多数回該当適用開始年月"),
            new PutColumn(1, "SyokumuKbn", "職務上区分"),
            new PutColumn(1, "GenmenKbn", "国保減免区分"),
            new PutColumn(1, "GenmenRate", "国保減免割合"),
            new PutColumn(1, "GenmenGaku", "国保減免金額"),
            new PutColumn(1, "Tokki1", "特記事項１"),
            new PutColumn(1, "Tokki2", "特記事項２"),
            new PutColumn(1, "Tokki3", "特記事項３"),
            new PutColumn(1, "Tokki4", "特記事項４"),
            new PutColumn(1, "Tokki5", "特記事項５"),
            new PutColumn(1, "RousaiKofuNo", "労災交付番号"),
            new PutColumn(1, "RousaiSaigaiKbn", "労災災害区分"),
            new PutColumn(1, "RousaiJigyosyoName", "労災事業所名"),
            new PutColumn(1, "RousaiPrefName", "労災都道府県名"),
            new PutColumn(1, "RousaiCityName", "労災所在地郡市区名"),
            new PutColumn(1, "RousaiSyobyoDate", "労災傷病年月日"),
            new PutColumn(1, "RousaiSyobyoCd", "労災傷病コード"),
            new PutColumn(1, "RousaiRoudouCd", "労災労働局コード"),
            new PutColumn(1, "RousaiKantokuCd", "労災監督署コード"),
            new PutColumn(1, "RousaiReceCount", "労災レセ請求回数"),
            new PutColumn(1, "RyoyoStartDate", "労災療養開始日"),
            new PutColumn(1, "RyoyoEndDate", "労災療養終了日"),
            new PutColumn(1, "JibaiHokenName", "自賠保険会社名"),
            new PutColumn(1, "JibaiHokenTanto", "自賠保険担当者"),
            new PutColumn(1, "JibaiHokenTel", "自賠保険連絡先"),
            new PutColumn(1, "JibaiJyusyouDate", "自賠受傷日"),
            new PutColumn(1, "Kohi1Id", "公費１ID"),
            new PutColumn(1, "Kohi1PrefNo", "公費１都道府県番号"),
            new PutColumn(1, "Kohi1HokenNo", "公費１保険番号"),
            new PutColumn(1, "Kohi1HokenEdaNo", "公費１保険番号枝番"),
            new PutColumn(1, "Kohi1FutansyaNo", "公費１負担者番号"),
            new PutColumn(1, "Kohi1JyukyusyaNo", "公費１受給者番号"),
            new PutColumn(1, "Kohi1TokusyuNo", "公費１特殊受給者番号"),
            new PutColumn(1, "Kohi1SikakuDate", "公費１資格取得日"),
            new PutColumn(1, "Kohi1KofuDate", "公費１交付日"),
            new PutColumn(1, "Kohi1StartDate", "公費１適用開始日"),
            new PutColumn(1, "Kohi1EndDate", "公費１適用終了日"),
            new PutColumn(1, "Kohi1Rate", "公費１負担率"),
            new PutColumn(1, "Kohi1GendoGaku", "公費１一部負担限度額"),
            new PutColumn(1, "Kohi2Id", "公費２ID"),
            new PutColumn(1, "Kohi2PrefNo", "公費２都道府県番号"),
            new PutColumn(1, "Kohi2HokenNo", "公費２保険番号"),
            new PutColumn(1, "Kohi2HokenEdaNo", "公費２保険番号枝番"),
            new PutColumn(1, "Kohi2FutansyaNo", "公費２負担者番号"),
            new PutColumn(1, "Kohi2JyukyusyaNo", "公費２受給者番号"),
            new PutColumn(1, "Kohi2TokusyuNo", "公費２特殊受給者番号"),
            new PutColumn(1, "Kohi2SikakuDate", "公費２資格取得日"),
            new PutColumn(1, "Kohi2KofuDate", "公費２交付日"),
            new PutColumn(1, "Kohi2StartDate", "公費２適用開始日"),
            new PutColumn(1, "Kohi2EndDate", "公費２適用終了日"),
            new PutColumn(1, "Kohi2Rate", "公費２負担率"),
            new PutColumn(1, "Kohi2GendoGaku", "公費２一部負担限度額"),
            new PutColumn(1, "Kohi3Id", "公費３ID"),
            new PutColumn(1, "Kohi3PrefNo", "公費３都道府県番号"),
            new PutColumn(1, "Kohi3HokenNo", "公費３保険番号"),
            new PutColumn(1, "Kohi3HokenEdaNo", "公費３保険番号枝番"),
            new PutColumn(1, "Kohi3FutansyaNo", "公費３負担者番号"),
            new PutColumn(1, "Kohi3JyukyusyaNo", "公費３受給者番号"),
            new PutColumn(1, "Kohi3TokusyuNo", "公費３特殊受給者番号"),
            new PutColumn(1, "Kohi3SikakuDate", "公費３資格取得日"),
            new PutColumn(1, "Kohi3KofuDate", "公費３交付日"),
            new PutColumn(1, "Kohi3StartDate", "公費３適用開始日"),
            new PutColumn(1, "Kohi3EndDate", "公費３適用終了日"),
            new PutColumn(1, "Kohi3Rate", "公費３負担率"),
            new PutColumn(1, "Kohi3GendoGaku", "公費３一部負担限度額"),
            new PutColumn(1, "Kohi4Id", "公費４ID"),
            new PutColumn(1, "Kohi4PrefNo", "公費４都道府県番号"),
            new PutColumn(1, "Kohi4HokenNo", "公費４保険番号"),
            new PutColumn(1, "Kohi4HokenEdaNo", "公費４保険番号枝番"),
            new PutColumn(1, "Kohi4FutansyaNo", "公費４負担者番号"),
            new PutColumn(1, "Kohi4JyukyusyaNo", "公費４受給者番号"),
            new PutColumn(1, "Kohi4TokusyuNo", "公費４特殊受給者番号"),
            new PutColumn(1, "Kohi4SikakuDate", "公費４資格取得日"),
            new PutColumn(1, "Kohi4KofuDate", "公費４交付日"),
            new PutColumn(1, "Kohi4StartDate", "公費４適用開始日"),
            new PutColumn(1, "Kohi4EndDate", "公費４適用終了日"),
            new PutColumn(1, "Kohi4Rate", "公費４負担率"),
            new PutColumn(1, "Kohi4GendoGaku", "公費４一部負担限度額"),
            #endregion
            #region 病名情報
            new PutColumn(2, "ByomeiCd", "基本病名コード"),
            new PutColumn(2, "SyusyokuCd1", "修飾語コード１"),
            new PutColumn(2, "SyusyokuCd2", "修飾語コード２"),
            new PutColumn(2, "SyusyokuCd3", "修飾語コード３"),
            new PutColumn(2, "SyusyokuCd4", "修飾語コード４"),
            new PutColumn(2, "SyusyokuCd5", "修飾語コード５"),
            new PutColumn(2, "SyusyokuCd6", "修飾語コード６"),
            new PutColumn(2, "SyusyokuCd7", "修飾語コード７"),
            new PutColumn(2, "SyusyokuCd8", "修飾語コード８"),
            new PutColumn(2, "SyusyokuCd9", "修飾語コード９"),
            new PutColumn(2, "SyusyokuCd10", "修飾語コード１０"),
            new PutColumn(2, "SyusyokuCd11", "修飾語コード１１"),
            new PutColumn(2, "SyusyokuCd12", "修飾語コード１２"),
            new PutColumn(2, "SyusyokuCd13", "修飾語コード１３"),
            new PutColumn(2, "SyusyokuCd14", "修飾語コード１４"),
            new PutColumn(2, "SyusyokuCd15", "修飾語コード１５"),
            new PutColumn(2, "SyusyokuCd16", "修飾語コード１６"),
            new PutColumn(2, "SyusyokuCd17", "修飾語コード１７"),
            new PutColumn(2, "SyusyokuCd18", "修飾語コード１８"),
            new PutColumn(2, "SyusyokuCd19", "修飾語コード１９"),
            new PutColumn(2, "SyusyokuCd20", "修飾語コード２０"),
            new PutColumn(2, "SyusyokuCd21", "修飾語コード２１"),
            new PutColumn(2, "Byomei", "病名"),
            new PutColumn(2, "StartDate", "開始日"),
            new PutColumn(2, "TenkiKbn", "転帰区分"),
            new PutColumn(2, "TenkiDate", "転帰日"),
            new PutColumn(2, "SyubyoKbn", "主病名区分"),
            new PutColumn(2, "SikkanKbn", "慢性疾患区分"),
            new PutColumn(2, "SikkanKbnName", "慢性疾患区分名称"),
            new PutColumn(2, "NanbyoCd", "難病外来コード"),
            new PutColumn(2, "HosokuCmt", "補足コメント"),
            new PutColumn(2, "HokenRid", "保険レセプトID"),
            new PutColumn(2, "TogetuByomei", "当月病名区分"),
            new PutColumn(2, "IsNodspRece", "レセプト非表示区分"),
            new PutColumn(2, "IsNodspKarte", "カルテ非表示区分"),
            #endregion
            #region 来院情報
            new PutColumn(3, "SinDate", "診療日"),
            new PutColumn(3, "RaiinNo", "来院番号"),
            new PutColumn(3, "OyaRaiinNo", "親来院番号"),
            new PutColumn(3, "Status", "状態"),
            new PutColumn(3, "IsYoyaku", "予約フラグ"),
            new PutColumn(3, "YoyakuTime", "予約時間"),
            new PutColumn(3, "YoyakuId", "予約者ID"),
            new PutColumn(3, "UketukeSbt", "受付種別"),
            new PutColumn(3, "UketukeTime", "受付時間"),
            new PutColumn(3, "UketukeId", "受付者ID"),
            new PutColumn(3, "UketukeNo", "受付番号"),
            new PutColumn(3, "SinStartTime", "診察開始時間"),
            new PutColumn(3, "SinEndTime", "診察終了時間"),
            new PutColumn(3, "KaikeiTime", "精算時間"),
            new PutColumn(3, "KaikeiId", "精算者ID"),
            new PutColumn(3, "KaName", "診療科"),
            new PutColumn(3, "TantoId", "担当医ID"),
            new PutColumn(3, "TantoName", "担当医"),
            new PutColumn(3, "HokenPid", "保険組合せID"),
            new PutColumn(3, "SyosaisinKbn", "初再診区分"),
            new PutColumn(3, "SyosaisinKbnS", "初再診区分略称"),
            new PutColumn(3, "JikanKbn", "時間枠区分"),
            new PutColumn(3, "RaiinCmt", "来院コメント"),
            new PutColumn(3, "RaiinBiko", "来院備考"),
            new PutColumn(3, "IsFirstVisitDate", "初回来院日フラグ"),
            #endregion
            #region 診療情報(オーダー)
            new PutColumn(4, "SinDate", "診療日"),
            new PutColumn(4, "RaiinNo", "来院番号"),
            new PutColumn(4, "OyaRaiinNo", "親来院番号"),
            new PutColumn(4, "Status", "状態"),
            new PutColumn(4, "UketukeSbt", "受付種別"),
            new PutColumn(4, "KaName", "診療科"),
            new PutColumn(4, "TantoId", "担当医ID"),
            new PutColumn(4, "TantoName", "担当医"),
            new PutColumn(4, "UketukeNo", "受付番号"),
            new PutColumn(4, "SortNo", "並び順"),
            new PutColumn(4, "RpNo", "剤番号"),
            new PutColumn(4, "RpEdaNo", "剤枝番"),
            new PutColumn(4, "HokenPid", "保険組合せID"),
            new PutColumn(4, "HokenPname", "保険組合せ名称"),
            new PutColumn(4, "OdrKouiKbn", "オーダー行為区分"),
            new PutColumn(4, "RpName", "剤名称"),
            new PutColumn(4, "InoutKbn", "院内院外区分"),
            new PutColumn(4, "SikyuKbn", "至急区分"),
            new PutColumn(4, "SyohoSbt", "処方種別"),
            new PutColumn(4, "SanteiKbn", "算定区分"),
            new PutColumn(4, "TosekiKbn", "透析区分"),
            new PutColumn(4, "DaysCnt", "日数回数"),
            new PutColumn(4, "RowNo", "行番号"),
            new PutColumn(4, "SinKouiKbn", "診療行為区分"),
            new PutColumn(4, "ItemCd", "診療行為コード"),
            new PutColumn(4, "ItemName", "診療行為名称"),
            new PutColumn(4, "Suryo", "数量"),
            new PutColumn(4, "UnitName", "単位名称"),
            new PutColumn(4, "UnitSBT", "単位種別"),
            new PutColumn(4, "TermVal", "単位換算値"),
            new PutColumn(4, "KohatuKbn", "後発医薬品区分"),
            new PutColumn(4, "SyohoKbn", "処方せん記載区分"),
            new PutColumn(4, "SyohoLimitKbn", "処方せん記載制限区分"),
            new PutColumn(4, "DrugKbn", "薬剤区分"),
            new PutColumn(4, "YohoKbn", "用法区分"),
            new PutColumn(4, "Kokuji1", "告示等識別区分（１）"),
            new PutColumn(4, "Kokuji2", "告示等識別区分（２）"),
            new PutColumn(4, "IsNodspRece", "レセ非表示区分"),
            new PutColumn(4, "IpnCd", "一般名コード"),
            new PutColumn(4, "IpnName", "一般名"),
            new PutColumn(4, "JissiKbn", "実施区分"),
            new PutColumn(4, "JissiDate", "実施日時"),
            new PutColumn(4, "JissiId", "実施者"),
            new PutColumn(4, "JissiMachine", "実施端末"),
            new PutColumn(4, "ReqCd", "検査依頼コード"),
            new PutColumn(4, "Bunkatu", "分割調剤"),
            new PutColumn(4, "CmtName", "コメント名称"),
            new PutColumn(4, "CmtOpt", "コメント文"),
            new PutColumn(4, "FontColor", "文字色"),
            #endregion
            #region 診療情報(算定)
            new PutColumn(5, "SinDate", "診療日"),
            new PutColumn(5, "RaiinNo", "来院番号"),
            new PutColumn(5, "OyaRaiinNo", "親来院番号"),
            new PutColumn(5, "Status", "状態"),
            new PutColumn(5, "UketukeSbt", "受付種別"),
            new PutColumn(5, "KaName", "診療科"),
            new PutColumn(5, "TantoId", "担当医ID"),
            new PutColumn(5, "TantoName", "担当医"),
            new PutColumn(5, "UketukeNo", "受付番号"),
            new PutColumn(5, "SinId", "診療識別"),
            new PutColumn(5, "RpNo", "剤番号"),
            new PutColumn(5, "SeqNo", "連番"),
            new PutColumn(5, "RowNo", "行番号"),
            new PutColumn(5, "SanteiKbn", "算定区分"),
            new PutColumn(5, "HokenPid", "保険組合せID"),
            new PutColumn(5, "HokenId", "保険ID"),
            new PutColumn(5, "SubTotalTen", "点数小計"),
            new PutColumn(5, "SubTotalZei", "内税小計"),
            new PutColumn(5, "SubTotalCount", "回数小計"),
            new PutColumn(5, "ItemCd", "診療行為コード"),
            new PutColumn(5, "OdrItemCd", "オーダー診療行為コード"),
            new PutColumn(5, "ItemName", "診療行為名称"),
            new PutColumn(5, "Suryo", "数量"),
            new PutColumn(5, "UnitName", "単位名称"),
            new PutColumn(5, "Ten", "点数"),
            new PutColumn(5, "Zei", "内税"),
            new PutColumn(5, "IsNodspRece", "レセ非表示区分"),
            new PutColumn(5, "IsNodspPaperRece", "紙レセ非表示区分"),
            new PutColumn(5, "IsNodspRyosyu", "領収証非表示区分"),
            new PutColumn(5, "InoutKbn", "院外処方区分"),
            new PutColumn(5, "EntenKbn", "円点区分"),
            new PutColumn(5, "JihiSbt", "自費種別"),
            new PutColumn(5, "KazeiKbn", "課税区分"),
            #endregion
            #region カルテ情報
            new PutColumn(6, "SinDate", "診療日"),
            new PutColumn(6, "RaiinNo", "来院番号"),
            new PutColumn(6, "OyaRaiinNo", "親来院番号"),
            new PutColumn(6, "Status", "状態"),
            new PutColumn(6, "UketukeSbt", "受付種別"),
            new PutColumn(6, "KaName", "診療科"),
            new PutColumn(6, "TantoId", "担当医ID"),
            new PutColumn(6, "TantoName", "担当医"),
            new PutColumn(6, "UketukeNo", "受付番号"),
            new PutColumn(6, "KarteKbn", "カルテ区分"),
            new PutColumn(6, "Text", "テキスト"),
            #endregion
            #region 検査情報
            new PutColumn(7, "IraiDate", "依頼日"),
            new PutColumn(7, "CenterCd", "センターコード"),
            new PutColumn(7, "KensaItemCd", "検査項目コード"),
            new PutColumn(7, "KensaName", "検査項目名称"),
            new PutColumn(7, "ResultValue", "結果値"),
            new PutColumn(7, "UnitName", "単位名称"),
            new PutColumn(7, "StandardVal", "基準値"),
            new PutColumn(7, "AbnormalKbn", "異常値区分")
        #endregion
        };
    #endregion

    #region Private properties

    private List<CoSta9000PrintData> printDatas;
    private List<CoPtInfModel> ptInfs;
    private CoHpInfModel hpInf;
    private List<CoDrugOdrModel> drugOdrs;
    private List<CoPtByomeiModel> ptByomeis;
    private List<CoPtHokenModel> ptHokens;
    private List<CoRaiinInfModel> raiinInfs;
    private List<CoOdrInfModel> odrInfs;
    private List<CoSinKouiModel> sinKouis;
    private List<CoKarteInfModel> karteInfs;
    private List<CoKensaModel> kensaInfs;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData = new();
    private readonly Dictionary<string, string> _extralData = new();
    private readonly List<Dictionary<string, CellModel>> _tableFieldData = new();
    private readonly ICoSta9000Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly ICoSta9000Finder _coSta9000Finder;
    public Sta9000CoReportService(ICoSta9000Finder finder, IReadRseReportFileService readRseReportFileService, ICoSta9000Finder coSta9000Finder)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _coSta9000Finder = coSta9000Finder;
    }

    private int reportType;
    private int sortOrder;
    private int sortOrder2;
    private int sortOrder3;
    private CoSta9000PtConf? ptConf;
    private CoSta9000HokenConf? hokenConf;
    private CoSta9000ByomeiConf? byomeiConf;
    private CoSta9000RaiinConf? raiinConf;
    private CoSta9000SinConf? sinConf;
    private CoSta9000KarteConf? karteConf;
    private CoSta9000KensaConf? kensaConf;
    private int nowDate;
    private int currentPage;
    private bool hasNextPage;
    private int outputDataType;
    private List<string> objectRseList;

    public CommonReportingRequestModel GetSta9000ReportingData(int hpId, int reportType, int sortOrder, int sortOrder2, int sortOrder3,
            CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
            CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
            CoSta9000KensaConf? kensaConf)
    {
        try
        {
            this.reportType = new int[] { 0, 1, 2, 3 }.Contains(reportType) ? reportType : 0;
            this.sortOrder = sortOrder;
            this.sortOrder2 = sortOrder2;
            this.sortOrder3 = sortOrder3;
            this.ptConf = ptConf;
            this.hokenConf = hokenConf;
            this.byomeiConf = byomeiConf;
            this.raiinConf = raiinConf;
            this.sinConf = sinConf;
            this.karteConf = karteConf;
            this.kensaConf = kensaConf;

            // get data to print
            outputDataType = 0;
            hasNextPage = true;
            string formFileName = reportInfs[reportType].FormFileName;
            int maxRow = reportInfs[reportType].MaxRow;
            GetFieldNameList(formFileName);
            _extralData.Add("maxRow", maxRow.ToString());
            currentPage = 1;

            if (GetData(hpId))
            {
                while (hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            return new Sta9000Mapper(_singleFieldData, _tableFieldData, _extralData, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    public (string, CoPrintExitCode, List<string>) OutPutFile(int hpId, List<string> outputColumns, bool isPutColName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf,
            CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf,
            CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3)
    {
        this.reportType = new int[] { 0, 1, 2, 3 }.Contains(reportType) ? reportType : 0;
        this.sortOrder = sortOrder;
        this.sortOrder2 = sortOrder2;
        this.sortOrder3 = sortOrder3;
        this.ptConf = ptConf;
        this.hokenConf = hokenConf;
        this.byomeiConf = byomeiConf;
        this.raiinConf = raiinConf;
        this.sinConf = sinConf;
        this.karteConf = karteConf;
        this.kensaConf = kensaConf;
        string printExitMessage = string.Empty;
        outputDataType = 0;
        #region SubMethod
        string RecordData(CoSta9000PrintData csvData, List<string> columns)
        {
            List<string> colDatas = new List<string>();

            foreach (var colName in columns)
            {
                //患者グループ（区分名称）
                if (colName.StartsWith("PtGrpCdName_"))
                {
                    var grpId = Regex.Replace(colName, @"[^0-9]", "");
                    if (grpId != string.Empty)
                    {
                        colDatas.Add("\"" + csvData.PtGrps.Find(p => p.GrpId == int.Parse(grpId)).GrpCodeName + "\"");
                    }
                }
                //患者グループ（区分コード）
                else if (colName.StartsWith("PtGrpCd_"))
                {
                    var grpId = Regex.Replace(colName, @"[^0-9]", "");
                    if (grpId != string.Empty)
                    {
                        colDatas.Add("\"" + csvData.PtGrps.Find(p => p.GrpId == int.Parse(grpId)).GrpCode + "\"");
                    }
                }
                else
                {
                    object? value = null;
                    switch (outputDataType)
                    {
                        #region 保険情報
                        case 1:
                            var propertyInfo = typeof(CoPtHokenModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.PtHoken != null)
                            {
                                value = propertyInfo.GetValue(csvData.PtHoken) ?? new();
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData) ?? new();
                                break;
                            }
                            break;
                        #endregion
                        #region 病名情報
                        case 2:
                            propertyInfo = typeof(CoPtByomeiModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.PtByomei != null)
                            {
                                value = propertyInfo.GetValue(csvData.PtByomei) ?? new();
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData) ?? new();
                                break;
                            }
                            break;
                        #endregion
                        #region 来院情報
                        case 3:
                            if (colName.StartsWith("RaiinKbn_"))
                            {
                                var grpId = Regex.Replace(colName, @"[^0-9]", "");
                                if (grpId != string.Empty)
                                {
                                    value = csvData.RaiinInf?.RaiinKbns.Find(p => p.GrpId == int.Parse(grpId)).KbnName;
                                    break;
                                }
                            }
                            else
                            {
                                propertyInfo = typeof(CoRaiinInfModel).GetProperty(colName);
                                if (propertyInfo != null && csvData.RaiinInf != null)
                                {
                                    value = propertyInfo.GetValue(csvData.RaiinInf);
                                    break;
                                }
                                propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                                if (propertyInfo != null)
                                {
                                    value = propertyInfo.GetValue(csvData);
                                    break;
                                }
                            }
                            break;
                        #endregion
                        #region 算定情報(オーダー)
                        case 4:
                            propertyInfo = typeof(CoOdrInfModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.OdrInf != null)
                            {
                                value = propertyInfo.GetValue(csvData.OdrInf);
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData);
                                break;
                            }
                            break;
                        #endregion
                        #region 算定情報(算定)
                        case 5:
                            propertyInfo = typeof(CoSinKouiModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.SinKoui != null)
                            {
                                value = propertyInfo.GetValue(csvData.SinKoui);
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData);
                                break;
                            }
                            break;
                        #endregion
                        #region カルテ情報
                        case 6:
                            propertyInfo = typeof(CoKarteInfModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.KarteInf != null)
                            {
                                value = propertyInfo.GetValue(csvData.KarteInf);
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData);
                                break;
                            }
                            break;
                        #endregion
                        #region 検査情報
                        case 7:
                            propertyInfo = typeof(CoKensaModel).GetProperty(colName);
                            if (propertyInfo != null && csvData.KensaInf != null)
                            {
                                value = propertyInfo.GetValue(csvData.KensaInf);
                                break;
                            }
                            propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                            if (propertyInfo != null)
                            {
                                value = propertyInfo.GetValue(csvData);
                                break;
                            }
                            break;
                        #endregion
                        default:
                            value = typeof(CoSta9000PrintData).GetProperty(colName)?.GetValue(csvData);
                            break;
                    }
                    colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
                }
            }

            return string.Join(",", colDatas);
        }
        #endregion

        try
        {
            if (!GetData(hpId, ptIds)) return (string.Empty, CoPrintExitCode.EndNoData, new());

            var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
            if (csvDatas.Count == 0) return (string.Empty, CoPrintExitCode.EndNoData, new());


            List<string> retDatas = new List<string>();

            //出力フィールドのリストに患者グループを追加
            var ptGrps = csvDatas.First().PtGrps;
            foreach (var ptGrp in ptGrps)
            {
                putColumns.Add(new PutColumn(0, string.Format("PtGrpCd_{0}", ptGrp.GrpId), string.Format("{0}(区分コード)", ptGrp.GrpName)));
                putColumns.Add(new PutColumn(0, string.Format("PtGrpCdName_{0}", ptGrp.GrpId), string.Format("{0}(区分名称)", ptGrp.GrpName)));
            }
            //出力フィールドのリストに来院区分を追加
            if (outputDataType == 3)
            {
                var raiinKbns = csvDatas.Find(c => c.RaiinInf != null)?.RaiinInf?.RaiinKbns;
                if (raiinKbns != null)
                {
                    foreach (var raiinKbn in raiinKbns)
                    {
                        putColumns.Add(new PutColumn(3, string.Format("RaiinKbn_{0}", raiinKbn.GrpId), string.Format("来院区分({0})", raiinKbn.GrpName)));
                    }
                }
            }

            //データ種類で出力フィールドを絞り込み
            var curPutColumns = putColumns.Where(p => p.DataType == 0 || p.DataType == outputDataType).ToList();

            //出力フィールドの絞り込み
            List<string> wrkTitles = new List<string>();
            List<string> wrkColumns = new List<string>();

            if (outputColumns?.Count >= 1)
            {
                foreach (var outputColumn in outputColumns)
                {
                    int index = curPutColumns.FindIndex(p => p.ColName == outputColumn);
                    if (index == -1) continue;

                    wrkTitles.Add(curPutColumns[index].JpName);
                    wrkColumns.Add(curPutColumns[index].ColName);
                }
            }
            else
            {
                wrkTitles = curPutColumns.Select(p => p.JpName).ToList();
                wrkColumns = curPutColumns.Select(p => p.ColName).ToList();
            }

            if (wrkColumns.Count == 0)
            {
                printExitMessage = "出力フィールドなし";
                return (string.Empty, CoPrintExitCode.EndNoData, new());
            }

            //タイトル行
            retDatas.Add("\"" + string.Join("\",\"", wrkTitles) + "\"");
            if (isPutColName)
            {
                retDatas.Add("\"" + string.Join("\",\"", wrkColumns) + "\"");
            }

            //データ
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData, wrkColumns));
            }


            return (string.Empty, CoPrintExitCode.EndSuccess, retDatas);
        }
        catch (Exception ex)
        {
            //Log.WriteLogError(ModuleName, this, nameof(outPutFile), ex);
            Console.WriteLine(ex);
            return (string.Empty, CoPrintExitCode.EndError, new());
        }
    }

    private bool GetData(int hpId, List<long> ptIds)
    {

        void MakePrintData()
        {

            var ptInfs = _coSta9000Finder.GetPtInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf, ptIds);

            printDatas = new List<CoSta9000PrintData>();

            //ソート順
            ptInfs = (ptInfs ?? new())
                    .OrderBy(p => sortOrder == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ThenBy(p => sortOrder2 == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ThenBy(p => sortOrder3 == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ToList();

            var primaryDoctorIdList = ptInfs.Select(item => item.PrimaryDoctor).Distinct().ToList();
            var primaryDoctorDic = _finder.GetUserSNameByUserIdDictionary(hpId, primaryDoctorIdList);

            foreach (var ptInf in ptInfs)
            {
                CoSta9000PrintData printData = new CoSta9000PrintData();

                printData.PtNum = ptInf.PtNum.ToString();
                printData.PtName = ptInf.PtName.TrimEnd() + (reportType == 2 ? "　様" : string.Empty);
                printData.KanaName = ptInf.KanaName;
                printData.Sex = ptInf.Sex;
                // todo: anh.vu3
                //printData.Birthday = outputFileType == CoFileType.Csv ? ptInf.Birthday.ToString() : CIUtil.SDateToShowSDate(ptInf.Birthday);
                printData.Birthday = CIUtil.SDateToShowSDate(ptInf.Birthday);
                printData.BirthdayW = CIUtil.SDateToShowWDate(ptInf.Birthday);
                printData.BirthdayWS = CIUtil.SDateToShowSWDate(ptInf.Birthday);
                printData.AgeBaseDate = 0;
                if (ptConf == null || ptConf.AgeBaseDate == 0)
                {
                    //年齢の基準日の指定がない
                    printData.AgeBaseDate = nowDate;
                }
                else
                {
                    //年齢の基準日の指定がある
                    printData.AgeBaseDate = ptConf.AgeBaseDate;
                }
                //死亡日より基準日のほうが大きい場合、死亡日を使う
                if (ptInf.IsDead == 1 && ptInf.DeathDate > 0 && ptInf.DeathDate < printData.AgeBaseDate)
                {
                    printData.AgeBaseDate = ptInf.DeathDate;
                }

                // todo: anh.vu3
                //printData.DeathDate = outputFileType == CoFileType.Csv ? ptInf.DeathDate.ToString() : CIUtil.SDateToShowSDate(ptInf.DeathDate);
                printData.DeathDate = CIUtil.SDateToShowSDate(ptInf.DeathDate);
                printData.HomePostMark = ptInf.HomePost.AsString() == string.Empty ? string.Empty : "〒";
                printData.HomePost = ptInf.HomePost;
                printData.HomeAddress1 = ptInf.HomeAddress1;
                printData.HomeAddress2 = ptInf.HomeAddress2;
                printData.Tel1 = ptInf.Tel1;
                printData.Tel2 = ptInf.Tel2;
                printData.Mail = ptInf.Mail;
                printData.Setainusi = ptInf.Setainusi;
                printData.Zokugara = ptInf.Zokugara;
                printData.Job = ptInf.Job;
                printData.RenrakuName = ptInf.RenrakuName;
                printData.RenrakuPost = ptInf.RenrakuPost;
                printData.RenrakuAddress1 = ptInf.RenrakuAddress1;
                printData.RenrakuAddress2 = ptInf.RenrakuAddress2;
                printData.RenrakuTel = ptInf.RenrakuTel;
                printData.RenrakuMemo = ptInf.RenrakuMemo;
                printData.OfficeName = ptInf.OfficeName;
                printData.OfficePost = ptInf.OfficePost;
                printData.OfficeAddress1 = ptInf.OfficeAddress1;
                printData.OfficeAddress2 = ptInf.OfficeAddress2;
                printData.OfficeTel = ptInf.OfficeTel;
                printData.OfficeMemo = ptInf.OfficeMemo;
                printData.IsRyosyuDetail = ptInf.IsRyosyuDetail.ToString();
                printData.PrimaryDoctor = primaryDoctorDic.ContainsKey(ptInf.PrimaryDoctor) ? primaryDoctorDic[ptInf.PrimaryDoctor] : string.Empty;
                printData.IsTester = ptInf.IsTester.ToString();
                printData.CreateDate = ptInf.CreateDate.ToString("yyyy/MM/dd");

                // todo: anh.vu3
                //printData.FirstVisitDate = outputFileType == CoFileType.Csv ? ptInf.FirstVisitDate.ToString() : CIUtil.SDateToShowSDate(ptInf.FirstVisitDate);
                //printData.LastVisitDate = outputFileType == CoFileType.Csv ? ptInf.LastVisitDate.ToString() : CIUtil.SDateToShowSDate(ptInf.LastVisitDate);
                printData.FirstVisitDate = CIUtil.SDateToShowSDate(ptInf.FirstVisitDate);
                printData.LastVisitDate = CIUtil.SDateToShowSDate(ptInf.LastVisitDate);
                printData.PtCmt = ptInf.PtCmt;
                printData.AdjFutan = ptInf.AdjFutan;
                printData.AdjRate = ptInf.AdjRate;
                printData.AutoSantei = ptInf.AutoSantei;
                printData.PtGrps = ptInf.PtGrps;

                bool isAddData = false;
                if (reportType == 1)
                {
                    var curDrugOdrs = drugOdrs
                        .Where(d => d.PtId == ptInf.PtId)
                        .OrderBy(d => d.ItemCd).ThenBy(d => d.ItemName)
                        .ToList();

                    var curByomeis = ptByomeis
                        .Where(b => b.PtId == ptInf.PtId)
                        .OrderBy(b => b.ByomeiCd).ThenBy(b => b.Byomei).ThenBy(b => b.StartDate)
                        .ToList();

                    //処方オーダー
                    for (int i = 0; i < curDrugOdrs.Count; i++)
                    {
                        if (isAddData)
                        {
                            printData = new CoSta9000PrintData();
                            printData.PtGrps = printDatas.Last().PtGrps
                                .Select(p => new CoPtInfModel.PtGrp(p.GrpId, p.GrpName, string.Empty, string.Empty)).ToList();
                        }

                        var drugOdr = curDrugOdrs[i];

                        if (i == 0 || drugOdr.ItemCd != curDrugOdrs[i - 1].ItemCd)
                        {
                            printData.ItemCd = drugOdr.ItemCd;
                            printData.ItemName = drugOdr.ItemName;
                        }
                        printData.Suryo = drugOdr.Suryo.ToString("#,0.00");
                        printData.UnitName = drugOdr.UnitName;

                        // todo: anh.vu3
                        //printData.SinDate = outputFileType == CoFileType.Csv ? drugOdr.SinDate.ToString() : CIUtil.SDateToShowSDate(drugOdr.SinDate);
                        printData.SinDate = CIUtil.SDateToShowSDate(drugOdr.SinDate);

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                    //患者病名
                    for (int i = 0; i < curByomeis.Count; i++)
                    {
                        if (isAddData)
                        {
                            printData = new CoSta9000PrintData();
                            printData.PtGrps = printDatas.Last().PtGrps
                                .Select(p => new CoPtInfModel.PtGrp(p.GrpId, p.GrpName, string.Empty, string.Empty)).ToList();
                        }

                        var ptByomei = curByomeis[i];

                        if (i == 0 || ptByomei.ByomeiCd != curByomeis[i - 1].ByomeiCd)
                        {
                            printData.ItemCd = ptByomei.ByomeiCd;
                            printData.ItemName = ptByomei.Byomei;
                        }
                        printData.SinDate = ptByomei.StartDate;
                        printData.TenkiDate = ptByomei.TenkiDate;
                        printData.TenkiKbn = ptByomei.TenkiKbn;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #region 保険情報
                else if (outputDataType == 1)
                {
                    var curHokens = ptHokens
                        .Where(p => p.PtId == ptInf.PtId)
                        .OrderBy(p => p.HokenPid)
                        .ToList();

                    foreach (var curHoken in curHokens)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.PtHoken = curHoken;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 病名情報
                else if (outputDataType == 2)
                {
                    var curByomeis = ptByomeis
                        .Where(b => b.PtId == ptInf.PtId)
                        .OrderBy(b => b.ByomeiCd).ThenBy(b => b.Byomei).ThenBy(b => b.StartDate)
                        .ToList();

                    foreach (var curByomei in curByomeis)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.PtByomei = curByomei;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 来院情報
                else if (outputDataType == 3 || reportType == 3)
                {
                    var curRaiinInfs = raiinInfs
                        .Where(r => r.PtId == ptInf.PtId)
                        .OrderBy(r => r.SinDate).ThenBy(r => r.UketukeTime).ThenBy(r => r.RaiinNo)
                        .ToList();

                    foreach (var curRaiinInf in curRaiinInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.RaiinInf = curRaiinInf;
                        printData.AgeBaseDate = curRaiinInf.RaiinInf.SinDate;
                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 算定情報(オーダー)
                else if (outputDataType == 4)
                {
                    var curOdrInfs = odrInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo)
                        .ThenBy(c => c.OdrKouiKbn).ThenBy(c => c.SortNo).ThenBy(c => c.RowNo)
                        .ToList();

                    foreach (var curOdrInf in curOdrInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.OdrInf = curOdrInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 算定情報(算定)
                else if (outputDataType == 5)
                {
                    var curSinKouis = sinKouis
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo)
                        .ThenBy(c => c.SinId).ThenBy(c => c.RpNo).ThenBy(c => c.SeqNo).ThenBy(c => c.RowNo)
                        .ToList();

                    foreach (var curSinKoui in curSinKouis)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.SinKoui = curSinKoui;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region カルテ情報
                else if (outputDataType == 6)
                {
                    var curKarteInfs = karteInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo).ThenBy(c => c.KarteKbn)
                        .ToList();

                    foreach (var curKarteInf in curKarteInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.KarteInf = curKarteInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 検査情報
                else if (outputDataType == 7)
                {
                    var curKensaInfs = kensaInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SortKey)
                        .ToList();

                    foreach (var curKensaInf in curKensaInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.KensaInf = curKensaInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion

                if (!isAddData)
                {
                    printDatas.Add(printData);
                }
            }

            #region 患者来院歴一覧のソート順
            if (reportType == 3)
            {
                //患者情報と来院情報のデータを合わせてから並び替える
                printDatas = printDatas
                    .OrderBy(p => sortOrder == 1 ? p.KanaName :
                    sortOrder == 2 ? p.RaiinInf?.SinDate :
                    sortOrder == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ThenBy(p => sortOrder2 == 1 ? p.KanaName :
                    sortOrder2 == 2 ? p.RaiinInf?.SinDate :
                    sortOrder2 == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ThenBy(p => sortOrder3 == 1 ? p.KanaName :
                    sortOrder3 == 2 ? p.RaiinInf?.SinDate :
                    sortOrder3 == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ToList();
            }
            #endregion
        }

        //データ取得
        ptInfs = _finder.GetPtInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf, ptIds);
        if ((ptInfs?.Count ?? 0) == 0)
        {
            return false;
        }

        nowDate = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();
        hpInf = _finder.GetHpInf(hpId, nowDate);

        //処方・病名一覧用データ取得
        if (reportType == 1)
        {
            drugOdrs = _finder.GetDrugOrders(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
            ptByomeis = _finder.GetPtByomeis(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        }
        //患者来院歴一覧用データ取得
        else if (reportType == 3)
        {
            raiinInfs = _finder.GetRaiinInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        }
        //CSV出力用データ取得
        switch (outputDataType)
        {
            case 1:
                ptHokens = _finder.GetPtHokens(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 2:
                ptByomeis = _finder.GetPtByomeis(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 3:
                raiinInfs = _finder.GetRaiinInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 4:
                odrInfs = _finder.GetOdrInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 5:
                sinKouis = _finder.GetSinKouis(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 6:
                karteInfs = _finder.GetKarteInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 7:
                kensaInfs = _finder.GetKensaInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf);
                break;
        }

        //印刷用データの作成
        MakePrintData();

        return printDatas.Count > 0;
    }

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            printDatas = new List<CoSta9000PrintData>();

            //ソート順
            ptInfs = (ptInfs ?? new())
                    .OrderBy(p => sortOrder == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ThenBy(p => sortOrder2 == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ThenBy(p => sortOrder3 == 1 ? p.KanaName :
                    p.PtNum.ToString().PadLeft(10, '0'))
                    .ToList();

            var primaryDoctorIdList = ptInfs.Select(item => item.PrimaryDoctor).Distinct().ToList();
            var primaryDoctorDic = _finder.GetUserSNameByUserIdDictionary(hpId, primaryDoctorIdList);

            foreach (var ptInf in ptInfs)
            {
                CoSta9000PrintData printData = new CoSta9000PrintData();

                printData.PtNum = ptInf.PtNum.ToString();
                printData.PtName = ptInf.PtName.TrimEnd() + (reportType == 2 ? "　様" : string.Empty);
                printData.KanaName = ptInf.KanaName;
                printData.Sex = ptInf.Sex;
                // todo: anh.vu3
                //printData.Birthday = outputFileType == CoFileType.Csv ? ptInf.Birthday.ToString() : CIUtil.SDateToShowSDate(ptInf.Birthday);
                printData.Birthday = CIUtil.SDateToShowSDate(ptInf.Birthday);
                printData.BirthdayW = CIUtil.SDateToShowWDate(ptInf.Birthday);
                printData.BirthdayWS = CIUtil.SDateToShowSWDate(ptInf.Birthday);
                printData.AgeBaseDate = 0;
                if (ptConf == null || ptConf.AgeBaseDate == 0)
                {
                    //年齢の基準日の指定がない
                    printData.AgeBaseDate = nowDate;
                }
                else
                {
                    //年齢の基準日の指定がある
                    printData.AgeBaseDate = ptConf.AgeBaseDate;
                }
                //死亡日より基準日のほうが大きい場合、死亡日を使う
                if (ptInf.IsDead == 1 && ptInf.DeathDate > 0 && ptInf.DeathDate < printData.AgeBaseDate)
                {
                    printData.AgeBaseDate = ptInf.DeathDate;
                }

                // todo: anh.vu3
                //printData.DeathDate = outputFileType == CoFileType.Csv ? ptInf.DeathDate.ToString() : CIUtil.SDateToShowSDate(ptInf.DeathDate);
                printData.DeathDate = CIUtil.SDateToShowSDate(ptInf.DeathDate);
                printData.HomePostMark = ptInf.HomePost.AsString() == string.Empty ? string.Empty : "〒";
                printData.HomePost = ptInf.HomePost;
                printData.HomeAddress1 = ptInf.HomeAddress1;
                printData.HomeAddress2 = ptInf.HomeAddress2;
                printData.Tel1 = ptInf.Tel1;
                printData.Tel2 = ptInf.Tel2;
                printData.Mail = ptInf.Mail;
                printData.Setainusi = ptInf.Setainusi;
                printData.Zokugara = ptInf.Zokugara;
                printData.Job = ptInf.Job;
                printData.RenrakuName = ptInf.RenrakuName;
                printData.RenrakuPost = ptInf.RenrakuPost;
                printData.RenrakuAddress1 = ptInf.RenrakuAddress1;
                printData.RenrakuAddress2 = ptInf.RenrakuAddress2;
                printData.RenrakuTel = ptInf.RenrakuTel;
                printData.RenrakuMemo = ptInf.RenrakuMemo;
                printData.OfficeName = ptInf.OfficeName;
                printData.OfficePost = ptInf.OfficePost;
                printData.OfficeAddress1 = ptInf.OfficeAddress1;
                printData.OfficeAddress2 = ptInf.OfficeAddress2;
                printData.OfficeTel = ptInf.OfficeTel;
                printData.OfficeMemo = ptInf.OfficeMemo;
                printData.IsRyosyuDetail = ptInf.IsRyosyuDetail.ToString();
                printData.PrimaryDoctor = primaryDoctorDic.ContainsKey(ptInf.PrimaryDoctor) ? primaryDoctorDic[ptInf.PrimaryDoctor] : string.Empty;
                printData.IsTester = ptInf.IsTester.ToString();
                printData.CreateDate = ptInf.CreateDate.ToString("yyyy/MM/dd");

                // todo: anh.vu3
                //printData.FirstVisitDate = outputFileType == CoFileType.Csv ? ptInf.FirstVisitDate.ToString() : CIUtil.SDateToShowSDate(ptInf.FirstVisitDate);
                //printData.LastVisitDate = outputFileType == CoFileType.Csv ? ptInf.LastVisitDate.ToString() : CIUtil.SDateToShowSDate(ptInf.LastVisitDate);
                printData.FirstVisitDate = CIUtil.SDateToShowSDate(ptInf.FirstVisitDate);
                printData.LastVisitDate = CIUtil.SDateToShowSDate(ptInf.LastVisitDate);
                printData.PtCmt = ptInf.PtCmt;
                printData.AdjFutan = ptInf.AdjFutan;
                printData.AdjRate = ptInf.AdjRate;
                printData.AutoSantei = ptInf.AutoSantei;
                printData.PtGrps = ptInf.PtGrps;

                bool isAddData = false;
                if (reportType == 1)
                {
                    var curDrugOdrs = drugOdrs
                        .Where(d => d.PtId == ptInf.PtId)
                        .OrderBy(d => d.ItemCd).ThenBy(d => d.ItemName)
                        .ToList();

                    var curByomeis = ptByomeis
                        .Where(b => b.PtId == ptInf.PtId)
                        .OrderBy(b => b.ByomeiCd).ThenBy(b => b.Byomei).ThenBy(b => b.StartDate)
                        .ToList();

                    //処方オーダー
                    for (int i = 0; i < curDrugOdrs.Count; i++)
                    {
                        if (isAddData)
                        {
                            printData = new CoSta9000PrintData();
                            printData.PtGrps = printDatas.Last().PtGrps
                                .Select(p => new CoPtInfModel.PtGrp(p.GrpId, p.GrpName, string.Empty, string.Empty)).ToList();
                        }

                        var drugOdr = curDrugOdrs[i];

                        if (i == 0 || drugOdr.ItemCd != curDrugOdrs[i - 1].ItemCd)
                        {
                            printData.ItemCd = drugOdr.ItemCd;
                            printData.ItemName = drugOdr.ItemName;
                        }
                        printData.Suryo = drugOdr.Suryo.ToString("#,0.00");
                        printData.UnitName = drugOdr.UnitName;

                        // todo: anh.vu3
                        //printData.SinDate = outputFileType == CoFileType.Csv ? drugOdr.SinDate.ToString() : CIUtil.SDateToShowSDate(drugOdr.SinDate);
                        printData.SinDate = CIUtil.SDateToShowSDate(drugOdr.SinDate);

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                    //患者病名
                    for (int i = 0; i < curByomeis.Count; i++)
                    {
                        if (isAddData)
                        {
                            printData = new CoSta9000PrintData();
                            printData.PtGrps = printDatas.Last().PtGrps
                                .Select(p => new CoPtInfModel.PtGrp(p.GrpId, p.GrpName, string.Empty, string.Empty)).ToList();
                        }

                        var ptByomei = curByomeis[i];

                        if (i == 0 || ptByomei.ByomeiCd != curByomeis[i - 1].ByomeiCd)
                        {
                            printData.ItemCd = ptByomei.ByomeiCd;
                            printData.ItemName = ptByomei.Byomei;
                        }
                        printData.SinDate = ptByomei.StartDate;
                        printData.TenkiDate = ptByomei.TenkiDate;
                        printData.TenkiKbn = ptByomei.TenkiKbn;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #region 保険情報
                else if (outputDataType == 1)
                {
                    var curHokens = ptHokens
                        .Where(p => p.PtId == ptInf.PtId)
                        .OrderBy(p => p.HokenPid)
                        .ToList();

                    foreach (var curHoken in curHokens)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.PtHoken = curHoken;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 病名情報
                else if (outputDataType == 2)
                {
                    var curByomeis = ptByomeis
                        .Where(b => b.PtId == ptInf.PtId)
                        .OrderBy(b => b.ByomeiCd).ThenBy(b => b.Byomei).ThenBy(b => b.StartDate)
                        .ToList();

                    foreach (var curByomei in curByomeis)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.PtByomei = curByomei;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 来院情報
                else if (outputDataType == 3 || reportType == 3)
                {
                    var curRaiinInfs = raiinInfs
                        .Where(r => r.PtId == ptInf.PtId)
                        .OrderBy(r => r.SinDate).ThenBy(r => r.UketukeTime).ThenBy(r => r.RaiinNo)
                        .ToList();

                    foreach (var curRaiinInf in curRaiinInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.RaiinInf = curRaiinInf;
                        printData.AgeBaseDate = curRaiinInf.RaiinInf.SinDate;
                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 算定情報(オーダー)
                else if (outputDataType == 4)
                {
                    var curOdrInfs = odrInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo)
                        .ThenBy(c => c.OdrKouiKbn).ThenBy(c => c.SortNo).ThenBy(c => c.RowNo)
                        .ToList();

                    foreach (var curOdrInf in curOdrInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.OdrInf = curOdrInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 算定情報(算定)
                else if (outputDataType == 5)
                {
                    var curSinKouis = sinKouis
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo)
                        .ThenBy(c => c.SinId).ThenBy(c => c.RpNo).ThenBy(c => c.SeqNo).ThenBy(c => c.RowNo)
                        .ToList();

                    foreach (var curSinKoui in curSinKouis)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.SinKoui = curSinKoui;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region カルテ情報
                else if (outputDataType == 6)
                {
                    var curKarteInfs = karteInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SinDate).ThenBy(c => c.RaiinNo).ThenBy(c => c.KarteKbn)
                        .ToList();

                    foreach (var curKarteInf in curKarteInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.KarteInf = curKarteInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion
                #region 検査情報
                else if (outputDataType == 7)
                {
                    var curKensaInfs = kensaInfs
                        .Where(c => c.PtId == ptInf.PtId)
                        .OrderBy(c => c.SortKey)
                        .ToList();

                    foreach (var curKensaInf in curKensaInfs)
                    {
                        if (isAddData)
                        {
                            printData = printDatas.Last().Clone();
                        }
                        printData.KensaInf = curKensaInf;

                        printDatas.Add(printData);
                        isAddData = true;
                    }
                }
                #endregion

                if (!isAddData)
                {
                    printDatas.Add(printData);
                }
            }

            #region 患者来院歴一覧のソート順
            if (reportType == 3)
            {
                //患者情報と来院情報のデータを合わせてから並び替える
                printDatas = printDatas
                    .OrderBy(p => sortOrder == 1 ? p.KanaName :
                    sortOrder == 2 ? p.RaiinInf?.SinDate :
                    sortOrder == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ThenBy(p => sortOrder2 == 1 ? p.KanaName :
                    sortOrder2 == 2 ? p.RaiinInf?.SinDate :
                    sortOrder2 == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ThenBy(p => sortOrder3 == 1 ? p.KanaName :
                    sortOrder3 == 2 ? p.RaiinInf?.SinDate :
                    sortOrder3 == 3 ? p.RaiinInf?.JikanKbnCd.ToString() :
                    p.PtNum.PadLeft(10, '0'))
                    .ToList();
            }
            #endregion
        }

        //データ取得
        ptInfs = _finder.GetPtInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf);
        if ((ptInfs?.Count ?? 0) == 0)
        {
            return false;
        }

        nowDate = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();
        hpInf = _finder.GetHpInf(hpId, nowDate);

        //処方・病名一覧用データ取得
        if (reportType == 1)
        {
            drugOdrs = _finder.GetDrugOrders(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
            ptByomeis = _finder.GetPtByomeis(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        }
        //患者来院歴一覧用データ取得
        else if (reportType == 3)
        {
            raiinInfs = _finder.GetRaiinInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
        }
        //CSV出力用データ取得
        switch (outputDataType)
        {
            case 1:
                ptHokens = _finder.GetPtHokens(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 2:
                ptByomeis = _finder.GetPtByomeis(ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 3:
                raiinInfs = _finder.GetRaiinInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 4:
                odrInfs = _finder.GetOdrInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 5:
                sinKouis = _finder.GetSinKouis(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 6:
                karteInfs = _finder.GetKarteInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf);
                break;
            case 7:
                kensaInfs = _finder.GetKensaInfs(hpId, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf);
                break;
        }

        //印刷用データの作成
        MakePrintData();

        return printDatas.Count > 0;
    }

    private void UpdateDrawForm()
    {
        if (printDatas.Count == 0)
        {
            hasNextPage = false;
            return;
        }

        #region Header
        void UpdateFormHeader()
        {
            //医療機関名
            _extralData.Add("HeaderR_0_0_" + currentPage, hpInf.HpName);
            //作成日時
            _extralData.Add("HeaderR_0_1_" + currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");
            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / reportInfs[reportType].MaxRow);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);
        }
        #endregion

        #region Body1
        void UpdateFormBody()
        {
            int ptIndex = (currentPage - 1) * reportInfs[reportType].MaxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < reportInfs[reportType].MaxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
               
                var printData = printDatas[ptIndex];
                string baseListName = string.Empty;

                //患者グループ タイトル
                if (rowNo == 0)
                {
                    foreach (var ptGrp in printData.PtGrps)
                    {
                        SetFieldData(string.Format("tPtGrpName{0}", ptGrp.GrpId), ptGrp.GrpName);
                    }
                }

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta9000PrintData).GetProperty(colName).GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }
                //患者グループ
                foreach (var ptGrp in printData.PtGrps)
                {
                    AddListData(ref data, string.Format("PtGrpCd{0}", ptGrp.GrpId), ptGrp.GrpCode);
                    AddListData(ref data, string.Format("PtGrpCdName{0}", ptGrp.GrpId), ptGrp.GrpCodeName);
                }
                //処方・病名
                if (reportType == 1)
                {
                    AddListData(ref data, "ItemCd", printData.ItemCd);
                    AddListData(ref data, "ItemName", printData.ItemName);
                    AddListData(ref data, "Suryo", printData.Suryo);
                    AddListData(ref data, "UnitName", printData.UnitName);
                    AddListData(ref data, "SinDate", printData.SinDate);
                    AddListData(ref data, "TenkiDate", printData.TenkiDate);
                    AddListData(ref data, "TenkiKbn", printData.TenkiKbn);
                }

                //5行毎に区切り線を引く
                if ((rowNo + 1) % 5 == 0)
                {
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }
                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }
        #endregion

        #region Body2
        void UpdateFormBody2()
        {
            int ptIndex = (currentPage - 1) * reportInfs[reportType].MaxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName + "_01")).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < reportInfs[reportType].MaxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[ptIndex];

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta9000PrintData).GetProperty(colName).GetValue(printData);
                    SetFieldData(string.Format("{0}_{1:D2}", colName, rowNo), value == null ? string.Empty : value.ToString() ?? string.Empty);
                }
                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }
        #endregion

        #region Body3
        void UpdateFormBody3()
        {
            int rowIndex = (currentPage - 1) * reportInfs[reportType].MaxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < reportInfs[reportType].MaxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[rowIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    object value = null;
                    System.Reflection.PropertyInfo propertyInfo;
                    propertyInfo = typeof(CoRaiinInfModel).GetProperty(colName);
                    if (propertyInfo != null && printData.RaiinInf != null)
                    {
                        value = propertyInfo.GetValue(printData.RaiinInf);
                    }
                    propertyInfo = typeof(CoSta9000PrintData).GetProperty(colName);
                    if (propertyInfo != null && value == null)
                    {
                        value = propertyInfo.GetValue(printData);
                    }

                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString());

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //5行毎に区切り線を引く
                if ((rowNo + 1) % 5 == 0)
                {
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                rowIndex++;
                if (rowIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }
        #endregion

        if (reportType == 2)
        {
            //宛名ラベル
            UpdateFormBody2();
        }
        else if (reportType == 3)
        {
            //患者来院歴一覧
            UpdateFormHeader();
            UpdateFormBody3();
        }
        else
        {
            //患者一覧表
            UpdateFormHeader();
            UpdateFormBody();
        }
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, new CellModel(value));
        }
    }

    private void GetFieldNameList(string fileName)
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta9000, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }
}
