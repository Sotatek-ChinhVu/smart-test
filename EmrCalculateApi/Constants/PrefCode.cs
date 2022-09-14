namespace EmrCalculateApi.Constants
{
    public static class PrefCode
    {
        public const int Hokkaido = 1;
        public const int Aomori = 2;
        public const int Iwate = 3;
        public const int Miyagi = 4;
        public const int Akita = 5;
        public const int Yamagata = 6;
        public const int Fukushima = 7;
        public const int Ibaraki = 8;
        public const int Tochigi = 9;
        public const int Gunma = 10;
        public const int Saitama = 11;
        public const int Chiba = 12;
        public const int Tokyo = 13;
        public const int Kanagawa = 14;
        public const int Niigata = 15;
        public const int Toyama = 16;
        public const int Ishikawa = 17;
        public const int Fukui = 18;
        public const int Yamanashi = 19;
        public const int Nagano = 20;
        public const int Gifu = 21;
        public const int Shizuoka = 22;
        public const int Aichi = 23;
        public const int Mie = 24;
        public const int Shiga = 25;
        public const int Kyoto = 26;
        public const int Osaka = 27;
        public const int Hyogo = 28;
        public const int Nara = 29;
        public const int Wakayama = 30;
        public const int Tottori = 31;
        public const int Shimane = 32;
        public const int Okayama = 33;
        public const int Hiroshima = 34;
        public const int Yamaguchi = 35;
        public const int Tokushima = 36;
        public const int Kagawa = 37;
        public const int Ehime = 38;
        public const int Kochi = 39;
        public const int Fukuoka = 40;
        public const int Saga = 41;
        public const int Nagasaki = 42;
        public const int Kumamoto = 43;
        public const int Oita = 44;
        public const int Miyazaki = 45;
        public const int Kagoshima = 46;
        public const int Okinawa = 47;

        /// <summary>
        /// 都道府県コードから都道府県名を取得
        /// </summary>
        /// <param name="prefCd"></param>
        /// <returns></returns>
        public static string PrefName(int prefCd)
        {
            prefCd = prefCd >= 51 ? prefCd - 50 : prefCd;

            switch (prefCd)
            {
                case 1: return "北海道";
                case 2: return "青森県";
                case 3: return "岩手県";
                case 4: return "宮城県";
                case 5: return "秋田県";
                case 6: return "山形県";
                case 7: return "福島県";
                case 8: return "茨城県";
                case 9: return "栃木県";
                case 10: return "群馬県";
                case 11: return "埼玉県";
                case 12: return "千葉県";
                case 13: return "東京都";
                case 14: return "神奈川県";
                case 15: return "新潟県";
                case 16: return "富山県";
                case 17: return "石川県";
                case 18: return "福井県";
                case 19: return "山梨県";
                case 20: return "長野県";
                case 21: return "岐阜県";
                case 22: return "静岡県";
                case 23: return "愛知県";
                case 24: return "三重県";
                case 25: return "滋賀県";
                case 26: return "京都府";
                case 27: return "大阪府";
                case 28: return "兵庫県";
                case 29: return "奈良県";
                case 30: return "和歌山県";
                case 31: return "鳥取県";
                case 32: return "島根県";
                case 33: return "岡山県";
                case 34: return "広島県";
                case 35: return "山口県";
                case 36: return "徳島県";
                case 37: return "香川県";
                case 38: return "愛媛県";
                case 39: return "高知県";
                case 40: return "福岡県";
                case 41: return "佐賀県";
                case 42: return "長崎県";
                case 43: return "熊本県";
                case 44: return "大分県";
                case 45: return "宮崎県";
                case 46: return "鹿児島県";
                case 47: return "沖縄県";
            }

            return "";
        }
        public static string PrefShortName(int prefCd)
        {
            prefCd = prefCd >= 51 ? prefCd - 50 : prefCd;

            switch (prefCd)
            {
                case 1: return "北海道";
                case 2: return "青森";
                case 3: return "岩手";
                case 4: return "宮城";
                case 5: return "秋田";
                case 6: return "山形";
                case 7: return "福島";
                case 8: return "茨城";
                case 9: return "栃木";
                case 10: return "群馬";
                case 11: return "埼玉";
                case 12: return "千葉";
                case 13: return "東京";
                case 14: return "神奈川";
                case 15: return "新潟";
                case 16: return "富山";
                case 17: return "石川";
                case 18: return "福井";
                case 19: return "山梨";
                case 20: return "長野";
                case 21: return "岐阜";
                case 22: return "静岡";
                case 23: return "愛知";
                case 24: return "三重";
                case 25: return "滋賀";
                case 26: return "京都";
                case 27: return "大阪";
                case 28: return "兵庫";
                case 29: return "奈良";
                case 30: return "和歌山";
                case 31: return "鳥取";
                case 32: return "島根";
                case 33: return "岡山";
                case 34: return "広島";
                case 35: return "山口";
                case 36: return "徳島";
                case 37: return "香川";
                case 38: return "愛媛";
                case 39: return "高知";
                case 40: return "福岡";
                case 41: return "佐賀";
                case 42: return "長崎";
                case 43: return "熊本";
                case 44: return "大分";
                case 45: return "宮崎";
                case 46: return "鹿児島";
                case 47: return "沖縄";
            }

            return "";
        }

#pragma warning disable S1125 // Boolean literals should not be redundant
        public static bool isHokkaido(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "1", "51" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isAomori(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "2", "52" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isIwate(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "3", "53" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isMiyagi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "4", "54" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isAkita(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "5", "55" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isYamagata(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "6", "56" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isFukushima(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "7", "57" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isIbaraki(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "8", "58" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isTochigi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "9", "59" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isGunma(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "10", "60" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isSaitama(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "11", "61" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isChiba(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "12", "62" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isTokyo(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "13", "63" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKanagawa(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "14", "64" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isNiigata(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "15", "65" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isToyama(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "16", "66" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isIshikawa(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "17", "67" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isFukui(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "18", "68" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isYamanashi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "19", "69" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isNagano(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "20", "70" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isGifu(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "21", "71" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isShizuoka(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "22", "72" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isAichi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "23", "73" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isMie(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "24", "74" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isShiga(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "25", "75" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKyoto(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "26", "76" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isOsaka(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "27", "77" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isHyogo(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "28", "78" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isNara(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "29", "79" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isWakayama(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "30", "80" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isTottori(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "31", "81" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isShimane(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "32", "82" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isOkayama(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "33", "83" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isHiroshima(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "34", "84" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isYamaguchi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "35", "85" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isTokushima(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "36", "86" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKagawa(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "37", "87" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isEhime(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "38", "88" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKochi(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "39", "89" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isFukuoka(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "40", "90" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isSaga(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "41", "91" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isNagasaki(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "42", "92" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKumamoto(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "43", "93" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isOita(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "44", "94" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isMiyazaki(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "45", "95" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isKagoshima(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "46", "96" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
        public static bool isOkinawa(string hokensyaNo)
        {
            return
                (hokensyaNo ?? "").Length < 6 ? false :
                    new string[] { "47", "97" }.Contains(
                        hokensyaNo!.Substring(hokensyaNo!.Length - 6, 2)
                    );
        }
#pragma warning restore S1125 // Boolean literals should not be redundant
    }
}
