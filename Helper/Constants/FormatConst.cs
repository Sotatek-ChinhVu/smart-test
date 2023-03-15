namespace Helper.Constants;

public class FormatConst
{
    /// <summary>
    /// 点数回数のメッセージフォーマット
    /// </summary>
    public const string TenCount = "{0,8}x{1,3}";

    /// <summary>
    /// 算定可否
    /// </summary>
    public const string NotSantei = "算定できません。";
    public const string MaybeNotSantei = "算定できない可能性があります。";
    public const string DontSantei = "算定していません。";

    public const string NotSanteiOnline = "オンライン医学管理料は、{0}、算定できません。";
    public const string MaybeNotSanteiOnline = "オンライン医学管理料は、{0}、算定できない可能性があります。";
    public const string NotSyohoOdr = "'{0}' は、処方オーダーがないため、算定できません。";

    public const string SanteiJyogen = "'{0}' は、上限（{1}回／{2}）に達しているため、";
    public const string SanteiJyogen2 = "'{0}' は、上限（{1}回／{2}）に達するため、";
    public const string SanteiSyosin = "'{0}' は、初診から1カ月以内のため、";
    public const string SanteiNotSyosin = "'{0}' は、初診ではないため、";

    public const string SanteiJyogenMulti = "'{0}' は、{1} が、上限（{2}回／{3}）に達しているため、";
    public const string NotSanteiReason = "'{0}' は、{1}ため、" + NotSantei;
    public const string MaybeNotSanteiReason = "'{0}' は、{1}ため、" + MaybeNotSantei;
    public const string DontSanteiReason = "'{0}' は、{1}ため、" + DontSantei;

    /// <summary>
    /// 包括マスタのメッセージフォーマット
    /// </summary>
    public static string HokatuBase = "'{0}' は、'{1}'({2}) に包括されるため、";

    public static string HokatuNotSantei = HokatuBase + NotSantei;
    public static string HokatuDontSantei = HokatuBase + DontSantei;
    public static string HokatuMaybeNotSantei = HokatuBase + MaybeNotSantei;

    public static readonly string[] HokatuLog =
        {
            HokatuNotSantei,
            HokatuMaybeNotSantei
        };
    public static readonly string[] HokatuLogAuto =
        {
            HokatuDontSantei,
            HokatuMaybeNotSantei
        };

    /// <summary>
    /// 背反マスタのメッセージフォーマット
    /// </summary>
    public static readonly string[] HaihanLog =
        {
            "'{0}' は、'{1}' を、{2}に算定しているため、" + NotSantei,
            "'{0}' は、'{1}' を、{2}に算定しているため、" + MaybeNotSantei,
            "'{0}' と '{1}' は、{2}にどちらか１つしか" + NotSantei,
            "'{0}' と '{1}' は、{2}にどちらか１つしか" + MaybeNotSantei
        };
    public static readonly string[] HaihanLogAuto =
        {
            "'{0}' は、'{1}' を、{2}に算定しているため、" + DontSantei,
            "'{0}' は、'{1}' を、{2}に算定しているため、" + MaybeNotSantei,
            "'{0}' と '{1}' は、{2}にどちらか１つしか" + NotSantei,
            "'{0}' と '{1}' は、{2}にどちらか１つしか" + MaybeNotSantei
        };

    public const string GairaiKanriLog = "'外来管理加算' は、一定の検査、リハビリ、精神、処置、手術、麻酔、放射線のいずれかを実施しているため、";
    public const string GairaiKanriLogNotSantei = GairaiKanriLog + NotSantei;
    public const string GairaiKanriLogDontSantei = GairaiKanriLog + DontSantei;

    public const string GairaiKanriLogDouituRaiin = "'外来管理加算' は、同一来院で算定済みのため、";
    public const string GairaiKanriLogDouituRaiinNotSantei = GairaiKanriLogDouituRaiin + NotSantei;
    public const string GairaiKanriLogDouituRaiinDontSantei = GairaiKanriLogDouituRaiin + DontSantei;

    public const string LowSuryo = "'{0}' は、数量が少なすぎるため、算定できません。（数量 {1}/下限値 {2}）";
    public const string HighSuryo = "'{0}' は、数量が多すぎる可能性があります。（数量 {1}/上限値 {2}）";
    public const string HighSuryo2 = "'{0}' は、数量がきざみ上限を超えるため、上限値で算定しました。（数量 {1}/上限値 {2}）";

    public const string NoMst = "'{0}'({1}) は、マスタに登録がないか期限切れのため、算定できません。";
    public const string OutTermMst = "'{0}'({1}) は、期限切れ({2})のため、算定できない可能性があります。";
    public const string NoSanteiMst = "'{0}'({1}) は、算定用項目がマスタに登録がないか期限切れのため、算定できない可能性があります。";

    public static string DelKensa = "'{0}' は、重複しているため、削除しました。";
    public static string DelKensaDiffBase = "'{0}' は、'{1}' と算定項目が重複するため、削除しました。";

    public const string TyuKasanNoTarget = "'{0}' は、対象となる基本項目が同一Rpに存在しないため、";
    public const string TyuKasanNoTargetNotSantei = TyuKasanNoTarget + NotSantei;
    public const string TyuKasanNoTargetDontSantei = TyuKasanNoTarget + DontSantei;

    public const string TyuKasanNoTargetWarning = "'{0}' は、対象となる基本項目が同一Rpに存在しないため、算定できない可能性があります。";

    public const string NoExist = "'{0}' は、'{1}' が算定されていないため、算定できません。";
    public const string NoExistAuto = "'{0}' は、'{1}' が算定されていないため、算定していません。";
    public const string NoExistWarning = "'{0}' は、'{1}' が算定されていないため、算定できない可能性があります。";

    public const string KasanNoKihon = "'{0}' は、基本項目が同一Rpに存在しないため、算定できません。";
    public const string KasanNoKihonAuto = "'{0}' は、基本項目が同一Rpに存在しないため、算定していません。";
    public const string KasanNoKihonWarning = "'{0}' は、基本項目が同一Rpに存在しないため、算定できない可能性があります。";

    public const string JibaiBunsyoHoken = "'{0}' は、自賠責保険ではないため、算定できません。";

    public const string NoneChusyaYakuzai = "'{0}' は、注射薬がないため、算定できません。";
}
