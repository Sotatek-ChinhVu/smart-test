using System.Runtime.InteropServices;
using System.Text;

namespace Helper.Common
{
    public static class HenkanJ
    {
        private const uint LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const uint LCMAP_HALFWIDTH = 0x00400000;
        private const uint LCMAP_FULLWIDTH = 0x00800000;

        public static string HankToZen(string fullWidth)
        {
            try
            {
                StringBuilder sb = new StringBuilder(256);
                LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_HALFWIDTH, fullWidth, -1, sb, sb.Capacity);
                //This function does not convert \ → ￥
                //Replace by hand
                sb = sb.Replace("\\", "￥");
                //Win7の仕様変更（結合できない濁点[゛]が[?]に変換される）
                sb = sb.Replace("゛", "?");
                //Win7の仕様変更（結合できない半濁点[゜]が[?]に変換される）
                sb = sb.Replace("゜", "?");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string ZenToHank(string halfWidth)
        {
            try
            {
                StringBuilder sb = new StringBuilder(256);
                LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_FULLWIDTH, halfWidth, -1, sb, sb.Capacity);
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int LCMapString(uint Locale, uint dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest);
    }
}
