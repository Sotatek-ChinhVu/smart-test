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
        public static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@*";
            char[] password = new char[12];
            Random random = new Random();

            for (int i = 0; i < 12; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }
        public static string GenerateDatabaseName(string subdomain)
        {
            string cleanedSubdomain = subdomain.Replace("-", "");

            return cleanedSubdomain.ToLower();
        }
        public static bool IsSubdomainValid(string subdomain)
        {
            string pattern = @"^(?!\-)[a-zA-Z0-9\-]{1,63}(?<!\-)$";
            return Regex.IsMatch(subdomain, pattern);
        }
    }
}
