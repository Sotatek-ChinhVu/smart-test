using Microsoft.VisualBasic;
using System.Reflection;
using System.Text;

namespace Helper.Common
{
    public class HenkanJ
    {
        private static HenkanJ? _instance;
        public static HenkanJ Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HenkanJ();
                }
                return _instance;
            }
        }

        private readonly List<string> _japaneseCharacterList = new List<string>();
        private HenkanJ()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly == null)
            {
                return;
            }
            var resourceStream = assembly.GetManifestResourceStream(@"Helper.Resources.Fullsize-Halfsize.txt");
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

        public string ToHalfsize(string value)
        {
            string result = value.ToLower();
            string fullsize = string.Empty;
            string halfsize = string.Empty;

            foreach (string row in _japaneseCharacterList)
            {
                var split = row.Split('@');
                fullsize = split[0];
                halfsize = split[1];

                result = result.Replace(fullsize, halfsize);
            }

            return result;
        }

        public string ToFullsize(string value)
        {
            string result = value.ToLower();
            string fullsize = string.Empty;
            string halfsize = string.Empty;

            foreach (string row in _japaneseCharacterList)
            {
                var split = row.Split('@');
                fullsize = split[0];
                halfsize = split[1];

                result = result.Replace(halfsize, fullsize);
            }

            return result;
        }
    }
}
