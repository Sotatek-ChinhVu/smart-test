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

    internal static class CharacterConstants
    {
        public static readonly (char Start, char End)[] RomajiRanges = new (char Start, char End)[0];
    }
}
