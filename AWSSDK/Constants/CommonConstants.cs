using System.Text.RegularExpressions;

namespace AWSSDK.Constants
{
    public static class CommonConstants
    {
        static Random random = new Random();

        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RemoveSpecialCharacters(string input)
        {
            string pattern = @"^[a-zA-Z]\w*";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                Console.WriteLine(match.Value.ToLower().Trim());
                return match.Value.ToLower().Trim();
            }
            return string.Empty;
        }
    }
}
