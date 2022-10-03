using Microsoft.VisualBasic;
using System.Reflection;
using System.Text;

namespace Helper.Common
{
    public class RomajiString
    {
        private static RomajiString? _instance;
        public static RomajiString Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RomajiString();
                }
                return _instance;
            }
        }

        private readonly List<string> _japaneseCharacterList = new List<string>();
        private RomajiString()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly == null)
            {
                return;
            }
            var resourceStream = assembly.GetManifestResourceStream(@"Helper.Resources.JapaneseCharacters.txt");
            if (resourceStream == null)
            {
                return;
            }
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string? splitMe = reader.ReadLine();
                    if (splitMe == null)
                    {
                        continue;
                    }
                    _japaneseCharacterList.Add(splitMe);
                }
            }
        }

        public string GetJapaneseCharacters()
        {
            return string.Join(",", _japaneseCharacterList);
        }

        public string RomajiToKana(string value)
        {
            string result = value.ToLower();
            string roma = string.Empty;
            string kata = string.Empty;

            foreach (string row in _japaneseCharacterList)
            {
                var split = row.Split('@');
                roma = split[0];
                kata = split[2];

                result = result.Replace(roma, kata);
            }

            return result;
        }
    }
}
