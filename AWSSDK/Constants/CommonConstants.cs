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

        public static string RemoveDeleteString(string path)
        {
            char slash = '/';
            string deleteString = "delete-";

            int deleteStringLength = deleteString.Length;
            char[] pathChars = path.ToCharArray();
            if (path.Length >= deleteStringLength && path.Substring(0, deleteStringLength) == deleteString)
            {
                for (int i = 0; i < deleteStringLength; i++)
                {
                    pathChars[i] = '\0';
                }
            }
            for (int i = 0; i < pathChars.Length; i++)
            {
                if (pathChars[i] == slash)
                {
                    int remainingLength = path.Length - (i + 1);
                    if (remainingLength >= deleteStringLength && path.Substring(i + 1, deleteStringLength) == deleteString)
                    {
                        for (int j = 0; j < deleteStringLength; j++)
                        {
                            pathChars[i + 1 + j] = '\0';
                        }
                    }
                }
            }
            string modifiedPath = new string(pathChars).Replace("\0", "");
            return modifiedPath;
        }

        public static bool CheckCondition(string path)
        {
            char slash = '/';
            string deleteString = "delete-";

            int deleteStringLength = deleteString.Length;
            char[] pathChars = path.ToCharArray();

            if (path.Length >= deleteStringLength && path.Substring(0, deleteStringLength) == deleteString)
            {
                return true;
            }

            for (int i = 0; i < pathChars.Length; i++)
            {
                if (pathChars[i] == slash)
                {
                    int remainingLength = path.Length - (i + 1);
                    if (remainingLength >= deleteStringLength && path.Substring(i + 1, deleteStringLength) == deleteString)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
