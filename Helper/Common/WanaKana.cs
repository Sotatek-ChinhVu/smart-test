using System.Text.RegularExpressions;

namespace Helper.Common
{
    public static class WanaKana
    {
        /// <summary>
        /// Check charcatering is Romaji
        /// </summary>
        /// <param name="input"></param>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public static bool IsRomaji(char input, Regex? allowed = null)
        {
            if (!IsCharInRange(input, CharacterConstants.RomajiRanges))
            {
                return IsMatch(input, allowed);
            }

            return true;
        }

        /// <summary>
        /// Check string is Romaji
        /// </summary>
        /// <param name="input"></param>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public static bool IsRomaji(string input, Regex? allowed = null)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.All((char c) => IsRomaji(c, allowed));
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static bool IsCharInRange(char input, char start, char end)
        {
            if (input >= start)
            {
                return input <= end;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private static bool IsCharInRange(char input, (char Start, char End) range)
        {
            return IsCharInRange(input, range.Start, range.End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        private static bool IsCharInRange(char input, params (char Start, char End)[] ranges)
        {
            return ranges.Any(((char Start, char End) r) => IsCharInRange(input, r));
        }

        /// <summary>
        /// Match regex
        /// </summary>
        /// <param name="input"></param>
        /// <param name="allowed"></param>
        /// <returns></returns>
        private static bool IsMatch(char input, Regex? allowed)
        {
            return allowed?.IsMatch(input.ToString()) ?? false;
        }

        /// <summary>
        /// Romaji string to Kana string
        /// </summary>
        /// <param name="romajiStr">input string romaji . eg : 'amiru'</param>
        /// <returns></returns>
        public static string RomajiToKana(string romajiStr)
        {
            romajiStr = romajiStr.ToLower();

            string roma = string.Empty;
            string kata = string.Empty;
            foreach (string row in DataRomajiConvert.Sources)
            {
                string[] split = row.Split('@');
                roma = split[0];
                kata = split[2];
                romajiStr = romajiStr.Replace(roma, kata);
            }
            return romajiStr;
        }

        public static bool IsHiragana(char input)
        {
            if (input != 'ー')
                return IsCharInRange(input, 'ぁ', 'ゖ');
            return true;
        }

        public static bool IsHiragana(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return input.All(new Func<char, bool>(IsHiragana));
            return false;
        }

        public static bool IsKatakana(char input)
        {
            return IsCharInRange(input, 'ァ', 'ー');
        }

        public static bool IsKatakana(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return input.All(new Func<char, bool>(IsKatakana));
            return false;
        }

        public static bool IsKana(char input)
        {
            if (!IsHiragana(input))
                return IsKatakana(input);
            return true;
        }

        public static bool IsKana(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return input.All(new Func<char, bool>(IsKana));
            return false;
        }
    }

    internal static class DataRomajiConvert
    {
        public static List<string> Sources = new List<string>()
        {
            "kya@きゃ@キャ",
            "kyu@きゅ@キュ",
            "kyo@きょ@キョ",
            "sha@しゃ@シャ",
            "shu@しゅ@シュ",
            "syu@しゅ@シュ",
            "sho@しょ@ショ",
            "cha@ちゃ@チャ",
            "chu@ちゅ@チュ",
            "cyu@ちゅ@チュ",
            "cho@ちょ@チョ",
            "nya@にゃ@ニャ",
            "nyu@にゅ@ニュ",
            "nyo@にょ@ニョ",
            "hya@ひゃ@ヒャ",
            "hyu@ひゅ@ヒュ",
            "hyo@ひょ@ヒョ",
            "mya@みゃ@ミャ",
            "myu@みゅ@ミュ",
            "myo@みょ@ミョ",
            "rya@りゃ@リャ",
            "ryu@りゅ@リュ",
            "ryo@りょ@リョ",
            "gya@ぎゃ@ギャ",
            "gyu@ぎゅ@ギュ",
            "gyo@ぎょ@ギョ",
            "ja@じゃ@ジャ",
            "ju@じゅ@ジュ",
            "jyu@じゅ@ジュ",
            "jo@じょ@ジョ",
            "dya@ぢゃ@ヂャ",
            "dyu@ぢゅ@ヂュ",
            "dyo@ぢょ@ヂョ",
            "bya@びゃ@ビャ",
            "byu@びゅ@ビュ",
            "byo@びょ@ビョ",
            "pya@ぴゃ@ピャ",
            "pyu@ぴゅ@ピュ",
            "pyo@ぴょ@ピョ",
            "ka@か@カ",
            "ki@き@キ",
            "ku@く@ク",
            "ke@け@ケ",
            "ko@こ@コ",
            "ta@た@タ",
            "chi@ち@チ",
            "ti@ち@チ",
            "tsu@つ@ツ",
            "tu@つ@ツ",
            "te@て@テ",
            "to@と@ト",
            "sa@さ@サ",
            "shi@し@シ",
            "si@し@シ",
            "ci@し@シ",
            "su@す@ス",
            "se@せ@セ",
            "so@そ@ソ",
            "na@な@ナ",
            "ni@に@ニ",
            "nu@ぬ@ヌ",
            "ne@ね@ネ",
            "no@の@ノ",
            "ha@は@ハ",
            "hi@ひ@ヒ",
            "hu@ふ@フ",
            "fu@ふ@フ",
            "he@へ@ヘ",
            "ho@ほ@ホ",
            "ma@ま@マ",
            "mi@み@ミ",
            "mu@む@ム",
            "me@め@メ",
            "mo@も@モ",
            "ya@や@ヤ",
            "yu@ゆ@ユ",
            "ye@いぇ@イェ",
            "yo@よ@ヨ",
            "ra@ら@ラ",
            "ri@り@リ",
            "ru@る@ル",
            "re@れ@レ",
            "ro@ろ@ロ",
            "wa@わ@ワ",
            "wi@うぃ@ウィ",
            "we@うぇ@ウェ",
            "wo@を@ヲ",
            "ga@が@ガ",
            "gi@ぎ@ギ",
            "gu@ぐ@グ",
            "ge@げ@ゲ",
            "go@ご@ゴ",
            "za@ざ@ザ",
            "zi@じ@ジ",
            "ji@じ@ジ",
            "zu@ず@ズ",
            "ze@ぜ@ゼ",
            "zo@ぞ@ゾ",
            "da@だ@ダ",
            "di@ぢ@ヂ",
            "du@づ@ヅ",
            "de@で@デ",
            "do@ど@ド",
            "ba@ば@バ",
            "bi@び@ビ",
            "bu@ぶ@ブ",
            "be@べ@ベ",
            "bo@ぼ@ボ",
            "pa@ぱ@パ",
            "pi@ぴ@ピ",
            "pu@ぷ@プ",
            "pe@ぺ@ペ",
            "po@ぽ@ポ",
            "la@ぁ@ァ",
            "li@ぃ@ィ",
            "lu@ぅ@ゥ",
            "le@ぇ@ェ",
            "lo@ぉ@ォ",
            "a@あ@ア",
            "i@い@イ",
            "u@う@ウ",
            "e@え@エ",
            "o@お@オ",
            "n@ん@ン",
            "k@っ@ッ",
            "t@っ@ッ",
            "c@っ@ッ",
            "s@っ@ッ",
            "h@っ@ッ",
            "f@っ@ッ",
            "m@っ@ッ",
            "y@っ@ッ",
            "r@っ@ッ",
            "w@っ@ッ",
            "g@っ@ッ",
            "z@っ@ッ",
            "d@っ@ッ",
            "b@っ@ッ",
            "p@っ@ッ",
            "-@-@ー"
        };
    } 

    internal static class CharacterConstants
    {
        public const char LatinLowercaseStart = 'a';

        public const char LatinLowercaseEnd = 'z';

        public const char LatinUppercaseStart = 'A';

        public const char LatinUppercaseEnd = 'Z';

        public const char ZenkakuLowercaseStart = 'ａ';

        public const char ZenkakuLowercaseEnd = 'ｚ';

        public const char ZenkakuUppercaseStart = 'Ａ';

        public const char ZenkakuUppercaseEnd = 'Ｚ';

        public const char HiraganaStart = 'ぁ';

        public const char HiraganaEnd = 'ゖ';

        public const char KatakanaStart = 'ァ';

        public const char KatakanaEnd = 'ー';

        public const char KanjiStart = '一';

        public const char KanjiEnd = '龯';

        public const char ProlongedSoundMark = 'ー';

        public const char KanaSlashDot = '・';

        public static readonly (char Start, char End) ZenkakuNumbers;

        public static readonly (char Start, char End) ZenkakuUppercase;

        public static readonly (char Start, char End) ZenkakuLowercase;

        public static readonly (char Start, char End) ZenkakuPunctuation1;

        public static readonly (char Start, char End) ZenkakuPunctuation2;

        public static readonly (char Start, char End) ZenkakuPunctuation3;

        public static readonly (char Start, char End) ZenkakuPunctuation4;

        public static readonly (char Start, char End) ZenkakuSymbolsCurrency;

        public static readonly (char Start, char End) HiraganaCharacters;

        public static readonly (char Start, char End) KatakanaCharacters;

        public static readonly (char Start, char End) HankakuKatakana;

        public static readonly (char Start, char End) KatakanaPunctuation;

        public static readonly (char Start, char End) KanaPunctuation;

        public static readonly (char Start, char End) CJKSymbolsPunctuation;

        public static readonly (char Start, char End) CommonCJK;

        public static readonly (char Start, char End) RareCJK;

        public static readonly (char Start, char End)[] KanaRanges;

        public static readonly (char Start, char End)[] JapanesePunctuationRanges;

        public static readonly (char Start, char End)[] JapaneseRanges;

        public static readonly (char Start, char End) ModernEnglish;

        public static readonly (char Start, char End)[] HepburnMacronRanges;

        public static readonly (char Start, char End)[] SmartQuoteRanges;

        public static readonly (char Start, char End)[] RomajiRanges;

        public static readonly (char Start, char End)[] EnglishPunctuationRanges;

        static CharacterConstants()
        {
            ZenkakuNumbers = ('０', '９');
            ZenkakuUppercase = ('Ａ', 'Ｚ');
            ZenkakuLowercase = ('ａ', 'ｚ');
            ZenkakuPunctuation1 = ('！', '／');
            ZenkakuPunctuation2 = ('：', '？');
            ZenkakuPunctuation3 = ('［', '\uff3f');
            ZenkakuPunctuation4 = ('｛', '｠');
            ZenkakuSymbolsCurrency = ('￠', '￮');
            HiraganaCharacters = ('\u3040', 'ゟ');
            KatakanaCharacters = ('゠', 'ヿ');
            HankakuKatakana = ('ｦ', 'ﾟ');
            KatakanaPunctuation = ('・', 'ー');
            KanaPunctuation = ('｡', '･');
            CJKSymbolsPunctuation = ('\u3000', '〿');
            CommonCJK = ('一', '\u9fff');
            RareCJK = ('㐀', '\u4dbf');
            KanaRanges = new (char, char)[4] { HiraganaCharacters, KatakanaCharacters, KanaPunctuation, HankakuKatakana };
            JapanesePunctuationRanges = new (char, char)[8] { CJKSymbolsPunctuation, KanaPunctuation, KatakanaPunctuation, ZenkakuPunctuation1, ZenkakuPunctuation2, ZenkakuPunctuation3, ZenkakuPunctuation4, ZenkakuSymbolsCurrency };
            ModernEnglish = ('\0', '\u007f');
            HepburnMacronRanges = new (char, char)[5]
            {
                ('Ā', 'ā'),
                ('Ē', 'ē'),
                ('Ī', 'ī'),
                ('Ō', 'ō'),
                ('Ū', 'ū')
            };
            SmartQuoteRanges = new (char, char)[2]
            {
                ('‘', '’'),
                ('“', '”')
            };
            JapaneseRanges = new (char, char)[5] { ZenkakuUppercase, ZenkakuLowercase, ZenkakuNumbers, CommonCJK, RareCJK };
            JapaneseRanges = JapaneseRanges.Concat(KanaRanges).ToArray();
            JapaneseRanges = JapaneseRanges.Concat(JapanesePunctuationRanges).ToArray();
            RomajiRanges = new (char, char)[1] { ModernEnglish };
            RomajiRanges = RomajiRanges.Concat(HepburnMacronRanges).ToArray();
            EnglishPunctuationRanges = new (char, char)[4]
            {
                (' ', '/'),
                (':', '?'),
                ('[', '`'),
                ('{', '~')
            };
            EnglishPunctuationRanges = EnglishPunctuationRanges.Concat(SmartQuoteRanges).ToArray();
        }
    }
}
