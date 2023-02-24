namespace Helper.Constants;

public static class ReceErrCdConst
{
    //hoken
    public const string ExpiredEndDateHokenErrCd = "E1001";
    public const string ExpiredEndDateHokenErrMsg = "期限切れ保険です。";

    public const string ExpiredStartDateHokenErrCd = "E1002";
    public const string ExpiredStartDateHokenErrMsg = "開始日前の保険です。";

    public const string UnConfirmedHokenErrCd = "E1003";
    public const string UnConfirmedHokenErrMsg = "保険が未確認です。";

    //order
    public const string ExpiredEndDateOdrErrCd = "E3001";
    public const string ExpiredEndDateOdrErrMsg = "期限切れ項目が登録されています。";

    public const string ExpiredStartDateOdrErrCd = "E3002";
    public const string ExpiredStartDateOdrErrMsg = "期限切れ項目が登録されています。";

    public const string FirstExamFeeCheckErrCd = "E3003";
    public const string FirstExamFeeCheckErrMsg = "初診料を同一月に複数回算定しています。";

    public const string SanteiCountCheckErrCd = "E3004";
    public const string SanteiCountCheckErrMsg = "算定回数の上限を超えています。";

    public const string TokuzaiItemCheckErrCd = "E3005";
    public const string TokuzaiItemCheckErrMsg = "未コード化特材が登録されています。";

    public const string RosaiItemCheckErrCd = "E3006";
    public const string RosaiItemCheckErrMsg = "労災項目が登録されています。";

    public const string ItemAgeCheckErrCd = "E3007";
    public const string ItemAgeCheckErrMsg = "当該年齢では算定できない項目です。";

    public const string CommentCheckErrCd = "E3008";
    public const string CommentCheckErrMsg = "コメントが登録されていません。";

    public const string ExceededDosageOdrErrCd = "E4001";
    public const string ExceededDosageOdrErrMsg = "投与日数制限を超えています。";

    public const string DuplicateOdrErrCd = "E4002";
    public const string DuplicateOdrErrMsg = "同種同効薬の重複処方があります。";

    public const string AdditionItemErrCd = "E5013";

    //byomei
    public const string NotExistByomeiErrCd = "E2001";
    public const string NotExistByomeiErrMsg = "病名が登録されていません。";

    public const string CheckReVisitContiByomeiErrCd = "E2002";
    public const string CheckReVisitContiByomeiErrMsg = "再診日に有効病名が存在しません。";

    public const string CheckFirstVisit2003ByomeiErrCd = "E2003";
    public const string CheckFirstVisit2003ByomeiErrMsg = "病名が登録されていません。";

    public const string CheckFirstVisit2004ByomeiErrCd = "E2004";
    public const string CheckFirstVisit2004ByomeiErrMsg = "初診日に継続病名があります。";

    public const string HasNotMainByomeiErrCd = "E2005";
    public const string HasNotMainByomeiErrMsg = "主病名が登録されていません。";

    public const string InvalidByomeiErrCd = "E2006";
    public const string InvalidByomeiErrMsg = "廃止病名が登録されています。";

    public const string FreeTextLengthByomeiErrCd = "E2007";
    public const string FreeTextLengthByomeiErrMsg = "未コード化病名が最大バイト数を超えています。";

    public const string CheckSuspectedByomeiErrCd = "E2008";
    public const string CheckSuspectedByomeiErrMsg = "疑い病名がxxヶ月を超えています。";

    public const string HasNotByomeiWithOdrErrCd = "E2009";
    public const string HasNotByomeiWithOdrErrMsg = "適応病名がありません。";

    //rosai 
    public const string HasNotSaigaiKbnErrCd = "E1101";
    public const string HasNotSaigaiKbnErrMsg = "災害区分がありません。";

    public const string HasNotSyobyoDateErrCd = "E1102";
    public const string HasNotSyobyoDateErrMsg = "傷病年月日がありません。";

    public const string HasNotSyobyoKeikaErrCd = "E1103";
    public const string HasNotSyobyoKeikaErrMsg = "傷病の経過がありません。";

    public const string HokenUsingRosaiItemErrCd = "E1104";
    public const string HokenUsingRosaiItemErrMsg = "健保レセプトで労災項目が算定されています。";

    public const string HasNotRousaiKantokuErrCd = "E1105";
    public const string HasNotRousaiKantokuErrMsg = "監督署コードが正しくありません。";

    public const string BuiOrderByomeiErrCd = "E2011";
    public const string BuiOrderByomeiErrMsg = "部位が一致する病名がありません。";

    public static string TokuzaiItemCd = "777770000";

    public const string ByomeiBuiOrderByomeiChekkuErrCd = "E2010";
    public const string ByomeiBuiOrderByomeiChekkuErrMsg = "オーダー部位と一致する病名がありません。";

    public const string DuplicateByomeiCheckErrCd = "E2012";
    public const string DuplicateByomeiCheckErrMsg = "病名が重複しています。";

    public const string DuplicateSyusyokuByomeiCheckErrCd = "E2013";

    public static List<string> IsFirstVisitCd = new List<string>()
        {
            "101110010",   //初診料
            //"101110040", //初診（同一日２科目）
            "111000110",   //初診料
            //"111011810", //初診料（同一日複数科受診時の２科目）
            "113003510",   //小児科外来診療料（処方箋を交付）初診時
            "113003710",   //小児科外来診療料（処方箋を交付しない）初診時
            "113019710",   //小児かかりつけ診療料（処方箋を交付）初診時
            "113019910"    //小児かかりつけ診療料（処方箋を交付しない）初診時
        };

    public static List<string> IsReVisitCd = new List<string>()
        {
            "101120010",   //再診料
            "101120040",   //再診料（同一日２科目）
            "101120050",   //電話再診料（労災）
            "101120060",   //同日再診料（労災）
            "101120070",   //同日電話再診料（労災）
            "101120080",   //電話再診料（同一日２科目）（労災）
            "112007410",   //再診料
            "112007950",   //電話等再診料
            "112008350",   //同日再診料
            "112008850",   //同日電話等再診料
            "112015810",   //再診料（同一日複数科受診時の２科目）
            "112015950",   //電話等再診料（同一日複数科受診時の２科目）
            "112023350",   //電話等再診料（３０年３月以前継続）
            "112023450",   //同日電話等再診料（３０年３月以前継続）
            "112023550",   //電話等再診料（同一日複数科受診時の２科目）（３０年３月以前継続）
            "113003610",   //小児科外来診療料（処方箋を交付）再診時
            "113003810",   //小児科外来診療料（処方箋を交付しない）再診時
            "113019810",   //小児かかりつけ診療料（処方箋を交付）再診時
            "113020010"    //小児かかりつけ診療料（処方箋を交付しない）再診時
        };
}
