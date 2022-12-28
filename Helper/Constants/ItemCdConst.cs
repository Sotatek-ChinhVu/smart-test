namespace Helper.Constants
{
    public static class ItemCdConst
    {
        /// <summary>
        /// 初再診
        /// </summary>
        public const string SyosaiKihon = "@SHIN";
        /// <summary>
        /// 時間枠
        /// </summary>
        public const string JikanKihon = "@JIKAN";

        /// <summary>
        /// 分割調剤
        /// </summary>
        public const string Con_TouyakuOrSiBunkatu = "@BUNKATU";

        /// <summary>
        /// リフィル
        /// </summary>
        public const string Con_Refill = "@REFILL";

        /// <summary>
        /// 情報通信機器
        /// </summary>
        public const string Con_Jouhou = "@JOUHOU";

        #region 初診関連

        #region 初診関連-基本
        /// <summary>
        /// 初診料
        /// </summary>
        public const string Syosin = "111000110";
        /// <summary>
        /// 初診料（同一日複数科受診時の２科目）
        /// </summary>
        public const string Syosin2 = "111011810";
        /// <summary>
        /// 初診料（新型コロナウイルス感染症・診療報酬上臨時的取扱）
        /// </summary>
        public const string SyosinCorona = "111013850";
        /// <summary>
        /// 初診料（労災）
        /// </summary>
        public const string SyosinRousai = "101110010";
        /// <summary>
        /// 初診（同一日２科目）
        /// </summary>
        public const string Syosin2Rousai = "101110040";
        /// <summary>
        /// 初診料（情報通信機器を用いた場合）
        /// </summary>
        public const string SyosinJouhou = "111014210";
        /// <summary>
        /// 初診料（同一日複数科受診時の２科目）（情報通信機器を用いた場合）
        /// </summary>
        public const string Syosin2Jouhou = "111014510";
        #endregion

        #region 初診関連-時間外加算
        /// <summary>
        /// 時間外加算（初診）
        /// </summary>
        public const string SyosinJikangai = "111000570";
        /// <summary>
        /// 休日加算（初診）
        /// </summary>
        public const string SyosinKyujitu = "111000670";
        /// <summary>
        /// 深夜加算（初診）
        /// </summary>
        public const string SyosinSinya = "111000770";
        /// <summary>
        /// 夜間・早朝等加算（初診）
        /// </summary>
        public const string SyosinYasou = "111012470";
        #endregion

        #region 初診関連-妊婦加算

        /// <summary>
        /// 妊婦加算（初診）
        /// </summary>
        public const string SyosinNinpu = "111013370";
        /// <summary>
        /// 妊婦時間外加算（初診）
        /// </summary>
        public const string SyosinNinpuJikangai = "111012970";
        /// <summary>
        /// 妊婦休日加算（初診）
        /// </summary>
        public const string SyosinNinpuKyujitu = "111013070";
        /// <summary>
        /// 妊婦深夜加算（初診）
        /// </summary>
        public const string SyosinNinpuSinya = "111013170";
        /// <summary>
        /// 妊婦時間外特例医療機関加算（初診）
        /// </summary>
        public const string SyosinNinpuJikangaiToku = "111013270";
        /// <summary>
        /// 妊婦夜間加算（産科又は産婦人科初診）
        /// </summary>
        public const string SyosinNinpuYakanToku = "111013470";
        /// <summary>
        /// 妊婦休日加算（産科又は産婦人科初診）
        /// </summary>
        public const string SyosinNinpuKyujituToku = "111013570";
        /// <summary>
        /// 妊婦深夜加算（産科又は産婦人科初診）
        /// </summary>
        public const string SyosinNinpuSinyaToku = "111013670";
        #endregion

        #region 初診関連-乳幼児加算
        /// <summary>
        /// 乳幼児加算（初診）
        /// </summary>
        public const string SyosinNyu = "111000370";
        /// <summary>
        /// 乳幼児時間外加算（初診）
        /// </summary>
        public const string SyosinNyuJikangai = "111011970";
        /// <summary>
        /// 乳幼児休日加算（初診）
        /// </summary>
        public const string SyosinNyuKyujitu = "111012070";
        /// <summary>
        /// 乳幼児深夜加算（初診）
        /// </summary>
        public const string SyosinNyuSinya = "111012170";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（初診）
        /// </summary>
        public const string SyosinNyuJikangaiToku = "111012270";
        #endregion

        #region 初診関連-乳幼児（小児科）
        /// <summary>
        /// 乳幼児夜間加算（小児科初診）
        /// </summary>
        public const string SyosinSyouniNyuYakan = "111011570";
        /// <summary>
        /// 乳幼児夜間加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string SyosinSyouniGairaiYakan = "113007070";
        /// <summary>
        /// 乳幼児休日加算（小児科初診）
        /// </summary>
        public const string SyosinSyouniNyuKyujitu = "111011670";
        /// <summary>
        /// 乳幼児休日加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string SyosinSyouniGairaiKyujitu = "113007170";
        /// <summary>
        /// 乳幼児深夜加算（小児科初診）
        /// </summary>
        public const string SyosinSyouniNyuSinya = "111011770";
        /// <summary>
        /// 乳幼児深夜加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string SyosinSyouniGairaiSinya = "113007270";
        #endregion
        /// <summary>
        /// 乳幼児感染予防策加算（初診料・診療報酬上臨時的取扱）
        /// </summary>
        public const string SyosinNyuyojiKansen = "111013970";
        public const string NyuyojiKansenCancel = "X100005";

        /// <summary>
        /// 機能強化加算（初診）
        /// </summary>
        public const string SyosinKinoKyoka = "111013770";
        /// <summary>
        /// 機能強化加算（初診）取消し
        /// </summary>
        public const string KinoKyokaCancel = "X00045";

        /// <summary>
        /// 医科外来等感染症対策実施加算（初診料）
        /// </summary>
        public const string SyosinKansenTaisaku = "111014070";
        public const string KansenTaisakuCancel = "X100006";        // 暫定

        /// <summary>
        /// 医科外来等感染症対策実施加算（初診料）
        /// </summary>
        public const string Syosin2RuiKansen = "111014170";

        /// <summary>
        /// 医科外来等感染症対策実施加算シリーズ自動算定設定コード
        /// </summary>
        public const string KansenTaisaku = "111014070";
        /// <summary>
        /// 外来感染対策向上加算（初診）
        /// </summary>
        public const string SyosinKansenKojo = "111014870";
        /// <summary>
        /// 外来感染対策向上加算取消し
        /// </summary>
        public const string KansenKojoCancel = "X100007";
        /// <summary>
        /// 連携強化加算（初診）
        /// </summary>
        public const string SyosinRenkeiKyoka = "111014970";
        /// <summary>
        /// 連携強化加算取消し
        /// </summary>
        public const string RenkeiKyokaCancel = "X100008";
        /// <summary>
        /// サーベイランス強化加算（初診）
        /// </summary>
        public const string SyosinSurveillance = "111015070";
        /// <summary>
        /// サーベイランス強化加算取消し
        /// </summary>
        public const string SurveillanceCancel = "X100009";
        /// <summary>
        /// 電子的保健医療情報活用加算（初診）
        /// </summary>
        public const string SyosinDensiHoken = "111015170";
        /// <summary>
        /// 電子的保健医療情報活用加算（初診）（診療情報等の取得が困難等）
        /// </summary>
        public const string SyosinDensiHokenKonnan = "111015270";
        /// <summary>
        /// 医療情報・システム基盤整備体制充実加算１（初診）
        /// </summary>
        public const string SyosinIryoJyohoKiban1 = "111015970";
        /// <summary>
        /// 医療情報・システム基盤整備体制充実加算２（初診）
        /// </summary>
        public const string SyosinIryoJyohoKiban2 = "111016070";
        #endregion

        #region 再診関連

        #region 再診関連-基本
        /// <summary>
        /// 再診料
        /// </summary>
        public const string Saisin = "112007410";
        public const string SaisinRousai = "101120010";
        /// <summary>
        /// 同日再診
        /// </summary>
        public const string SaisinDojitu = "112008350";
        public const string SaisinDojituRousai = "101120060";
        /// <summary>
        /// 電話等再診料
        /// </summary>
        public const string SaisinDenwa = "112007950";
        public const string SaisinDenwaRousai = "101120050";
        /// <summary>
        /// 同日電話等再診
        /// </summary>
        public const string SaisinDenwaDojitu = "112008850";
        public const string SaisinDenwaDojituRousai = "101120070";
        /// <summary>
        /// 再診料（同一日複数科受診時の２科目）
        /// </summary>
        public const string Saisin2 = "112015810";
        public const string Saisin2Rousai = "101120040";
        /// <summary>
        /// 電話等再診料（同一日複数科受診時の２科目）
        /// </summary>
        public const string SaisinDenwa2 = "112015950";
        public const string SaisinDenwa2Rousai = "101120080";
        /// <summary>
        /// 電話等再診料（３０年３月以前継続）
        /// </summary>
        public const string SaisinDenwaKeizoku = "112023350";
        /// <summary>
        /// 同日電話等再診料（３０年３月以前継続）
        /// </summary>
        public const string SaisinDenwaDojituKeizoku = "112023450";
        /// <summary>
        /// 再診料（情報通信機器を用いた場合）
        /// </summary>
        public const string SaisinJouhou = "112024210";
        /// <summary>
        /// 同日再診料（情報通信機器）
        /// </summary>
        public const string SaisinJouhouDojitu = "112024950";
        /// <summary>
        /// 再診料（同一日複数科受診時の２科目）（情報通信機器
        /// </summary>
        public const string Saisin2Jouhou = "112025210";
        #endregion

        #region 再診関連-時間外加算
        /// <summary>
        /// 時間外加算（初診）
        /// </summary>
        public const string SaisinJikangai = "112001110";
        /// <summary>
        /// 休日加算（初診）
        /// </summary>
        public const string SaisinKyujitu = "112001210";
        /// <summary>
        /// 深夜加算（初診）
        /// </summary>
        public const string SaisinSinya = "112001310";
        /// <summary>
        /// 夜間・早朝等加算（初診）
        /// </summary>
        public const string SaisinYasou = "112015570";
        #endregion

        #region 再診関連-妊婦加算
        /// <summary>
        /// 妊婦加算（再診）
        /// </summary>
        public const string SaisinNinpu = "112022070";
        /// <summary>
        /// 妊婦時間外加算（再診）（入院外）
        /// </summary>
        public const string SaisinNinpuJikangai = "112021370";
        /// <summary>
        /// 妊婦休日加算（再診）（入院外）
        /// </summary>
        public const string SaisinNinpuKyujitu = "112021470";
        /// <summary>
        /// 妊婦深夜加算（再診）（入院外）
        /// </summary>
        public const string SaisinNinpuSinya = "112021570";
        /// <summary>
        /// 妊婦時間外特例医療機関加算（再診）（入院外）
        /// </summary>
        public const string SaisinNinpuJikangaiToku = "112021670";
        /// <summary>
        /// 妊婦休日加算（産科又は産婦人科再診）（入院外）
        /// </summary>
        public const string SaisinNinpuKyujituToku = "112022270";
        /// <summary>
        /// 妊婦深夜加算（産科又は産婦人科再診）（入院外）
        /// </summary>
        public const string SaisinNinpuSinyaToku = "112022370";
        /// <summary>
        /// 妊婦夜間加算（産科又は産婦人科再診）（入院外）
        /// </summary>
        public const string SaisinNinpuYakanToku = "112022170";
        #endregion

        #region 再診関連-乳幼児
        /// <summary>
        /// 乳幼児加算（再診）
        /// </summary>
        public const string SaisinNyu = "112000970";
        /// <summary>
        /// 乳幼児時間外加算（再診）（入院外）
        /// </summary>
        public const string SaisinNyuJikangai = "112014770";
        /// <summary>
        /// 乳幼児休日加算（再診）（入院外）
        /// </summary>
        public const string SaisinNyuKyujitu = "112014870";
        /// <summary>
        /// 乳幼児深夜加算（再診）（入院外）
        /// </summary>
        public const string SaisinNyuSinya = "112014970";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（入院外）
        /// </summary>
        public const string SaisinNyuJikangaiToku = "112015070";
        #endregion

        #region 再診関連-乳幼児（小児科）
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（入院外）
        /// </summary>
        public const string SaisinSyouniNyuYakan = "112014170";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string SaisinSyouniGairaiYakan = "113007370";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（入院外）
        /// </summary>
        public const string SaisinSyouniNyuKyujitu = "112014270";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string SaisinSyouniGairaiKyujitu = "113007470";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（入院外）
        /// </summary>
        public const string SaisinSyouniNyuSinya = "112014370";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string SaisinSyouniGairaiSinya = "113007570";
        #endregion

        #region 再診関連-外来管理加算
        /// <summary>
        /// 外来管理加算
        /// </summary>
        public const string GairaiKanriKasan = "112011010";
        /// <summary>
        /// 外来管理加算（労災）
        /// </summary>
        public const string GairaiKanriKasanRousai = "101120020";
        /// <summary>
        /// 外来管理加算取消
        /// </summary>
        public const string GairaiKanriKasanCancel = "@1206000";
        #endregion

        #region 再診関連-時間外対応加算
        /// <summary>
        /// 時間外対応加算１
        /// </summary>
        public const string JikangaiTaiou1 = "112016070";
        /// <summary>
        /// 時間外対応加算２
        /// </summary>
        public const string JikangaiTaiou2 = "112015670";
        /// <summary>
        /// 時間外対応加算３
        /// </summary>
        public const string JikangaiTaiou3 = "112016170";
        /// <summary>
        /// 時間外対応加算取消
        /// </summary>
        public const string JikangaitaiouCancel = "X00021";
        #endregion

        #region 再診関連-明細書発行体制等加算
        /// <summary>
        /// 明細書発行体制等加算
        /// </summary>
        public const string MeisaiHakko = "112015770";
        /// <summary>
        /// 明細書発行体制等加算取消し
        /// </summary>
        public const string MeisaiHakkoCancel = "X00022";
        #endregion

        #region 再診関連-認知症地域包括診療加算
        /// <summary>
        /// 認知症地域包括診療加算１
        /// </summary>
        public const string SaisinNintiTiikiHoukatu1 = "112021870";
        /// <summary>
        /// 認知症地域包括診療加算２
        /// </summary>
        public const string SaisinNintiTiikiHoukatu2 = "112017570";
        #endregion

        #region 再診関連-地域包括診療加算
        /// <summary>
        /// 地域包括診療加算１
        /// </summary>
        public const string SaisinTiikiHoukatu1 = "112021770";
        /// <summary>
        /// 地域包括診療加算２
        /// </summary>
        public const string SaisinTiikiHoukatu2 = "112017270";

        #endregion

        /// <summary>
        /// 乳幼児感染予防策加算（初診料・診療報酬上臨時的取扱）
        /// </summary>
        public const string SaisinNyuyojiKansen = "112023970";
        /// <summary>
        /// 医科外来等感染症対策実施加算（再診料・外来診療料）
        /// </summary>
        public const string SaisinKansenTaisaku = "112024070";
        /// <summary>
        /// 二類感染症患者入院診療加算（電話等再診料・診療報酬上臨時的取扱）
        /// </summary>
        public const string Saisin2RuiKansen = "112024170";
        /// <summary>
        /// 外来感染対策向上加算（再診）
        /// </summary>
        public const string SaisinKansenKojo = "112024370";
        /// <summary>
        /// 連携強化加算（再診）
        /// </summary>
        public const string SaisinRenkeiKyoka = "112024470";
        /// <summary>
        /// サーベイランス強化加算（再診）
        /// </summary>
        public const string SaisinSurveillance = "112024570";
        #endregion

        /// <summary>
        /// オンライン診療料
        /// </summary>
        public const string OnlineSinryo = "112023210";

        #region 医学管理関連

        #region 医学管理関連-小児かかりつけ診療料
        /// <summary>
        /// 小児かかりつけ診療料
        /// </summary>
        public const string IgakuSyouniKakarituke = "@134000";
        /// <summary>
        /// 小児かかりつけ診療料（処方箋を交付）初診時	
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinKofuAri = "113019710";
        /// <summary>
        /// 小児かかりつけ診療料（処方箋を交付しない）初診時
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinKofuNasi = "113019910";
        /// <summary>
        /// 小児かかりつけ診療料（処方箋を交付）再診時
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinKofuAri = "113019810";
        /// <summary>
        /// 小児かかりつけ診療料（処方箋を交付しない）再診時
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinKofuNasi = "113020010";
        /// <summary>
        /// 小児かかりつけ診療料１
        /// </summary>
        public const string IgakuSyouniKakarituke1 = "@134001";
        /// <summary>
        /// 小児かかりつけ診療料１（処方箋を交付）初診時	
        /// </summary>
        public const string IgakuSyouniKakarituke1SyosinKofuAri = "113037210";
        /// <summary>
        /// 小児かかりつけ診療料１（処方箋を交付しない）初診時
        /// </summary>
        public const string IgakuSyouniKakarituke1SyosinKofuNasi = "113037410";
        /// <summary>
        /// 小児かかりつけ診療料１（処方箋を交付）再診時
        /// </summary>
        public const string IgakuSyouniKakarituke1SaisinKofuAri = "113037310";
        /// <summary>
        /// 小児かかりつけ診療料１（処方箋を交付しない）再診時
        /// </summary>
        public const string IgakuSyouniKakarituke1SaisinKofuNasi = "113037510";
        /// <summary>
        /// 小児かかりつけ診療料２
        /// </summary>
        public const string IgakuSyouniKakarituke2 = "@134002";
        /// <summary>
        /// 小児かかりつけ診療料２（処方箋を交付）初診時	
        /// </summary>
        public const string IgakuSyouniKakarituke2SyosinKofuAri = "113037610";
        /// <summary>
        /// 小児かかりつけ診療料２（処方箋を交付しない）初診時
        /// </summary>
        public const string IgakuSyouniKakarituke2SyosinKofuNasi = "113037810";
        /// <summary>
        /// 小児かかりつけ診療料２（処方箋を交付）再診時
        /// </summary>
        public const string IgakuSyouniKakarituke2SaisinKofuAri = "113037710";
        /// <summary>
        /// 小児かかりつけ診療料２（処方箋を交付しない）再診時
        /// </summary>
        public const string IgakuSyouniKakarituke2SaisinKofuNasi = "113037910";


        /// <summary>
        /// 時間外加算（初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinJikangai = "113026670";
        /// <summary>
        /// 休日加算（初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinKyujitu = "113026770";
        /// <summary>
        /// 深夜加算（初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinSinya = "113026870";

        /// <summary>
        /// 乳幼児時間外加算（初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinNyuJikangai = "113020170";
        /// <summary>
        /// 乳幼児休日加算（小児科初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinNyuKyujitu = "113020670";
        /// <summary>
        /// 乳幼児深夜加算（小児科初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinNyuSinya = "113020770";
        /// <summary>
        /// 乳幼児夜間加算（小児科初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinNyuYakan = "113020570";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（初診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSyosinNyuJikangaiToku = "113020470";

        /// <summary>
        /// 時間外加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinJikangai = "113027070";
        /// <summary>
        /// 休日加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinKyujitu = "113027170";
        /// <summary>
        /// 深夜加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinSinya = "113027270";

        /// <summary>
        /// 乳幼児時間外加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinNyuJikangai = "113020870";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinNyuKyujitu = "113021370";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinNyuSinya = "113021470";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinNyuYakan = "113021270";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinNyuJikangaiToku = "113021170";
        /// <summary>
        /// 時間外特例医療機関加算（初診）（小児かかりつけ診療料）
        /// </summary>        
        public const string IgakuSyouniKakaritukeSyosinJikangaiToku = "113026970";
        /// <summary>
        /// 時間外特例医療機関加算（再診）（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeSaisinJikangaiToku = "113027370";

        /// <summary>
        /// 小児抗菌薬適正使用支援加算（小児かかりつけ診療料）
        /// </summary>
        public const string IgakuSyouniKakaritukeKokin = "113027870";
        /// <summary>
        /// 機能強化加算（初診）（小児かかりつけ診療料） 
        /// </summary>
        public const string IgakuSyouniKakaritukeKinoKyoka = "113028970";

        #endregion

        #region 医学管理関連-小児科外来診療料
        /// <summary>
        /// 小児科外来診療料（処方箋を交付）初診時
        /// </summary>
        public const string IgakuSyouniGairaiSyosinKofuAri = "113003510";
        /// <summary>
        /// 小児科外来診療料（処方箋を交付しない）初診時
        /// </summary>
        public const string IgakuSyouniGairaiSyosinKofuNasi = "113003710";
        /// <summary>
        /// ※時間外に緊急投与の必要が有ったため院内処方した
        /// </summary>
        public const string IgakuSyouniGairaiJikangaiComment = "@00006";
        /// <summary>
        /// 乳幼児時間外加算（初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSyosinJikangai = "113009670";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSyosinJikangaiToku = "111010770";
        /// <summary>
        /// 乳幼児夜間加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSyosinSyouniYakan = "113007070";
        /// <summary>
        /// 乳幼児休日加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSyosinSyouniKyujitu = "113007170";
        /// <summary>
        /// 乳幼児深夜加算（小児科初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSyosinSyouniSinya = "113007270";

        /// 小児科外来診療料（処方箋を交付）再診時
        /// </summary>
        public const string IgakuSyouniGairaiSaisinKofuAri = "113003610";
        /// <summary>
        /// 小児科外来診療料（処方箋を交付しない）再診時
        /// </summary>
        public const string IgakuSyouniGairaiSaisinKofuNasi = "113003810";

        /// <summary>
        /// <summary>
        /// 乳幼児時間外加算（再診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSaisinJikangai = "113009770";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSaisinJikangaiToku = "112006070";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSaisinSyouniYakan = "113007370";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSaisinSyouniKyujitu = "113007470";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiSaisinSyouniSinya = "113007570";


        /// <summary>
        /// 小児抗菌薬適正使用支援加算（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiKokin = "113024670";
        /// <summary>
        /// 機能強化加算（初診）（小児科外来診療料）
        /// </summary>
        public const string IgakuSyouniGairaiKinoKyoka = "113028870";
        #endregion
        /// <summary>
        /// 乳幼児感染予防策加算（小児科外来診療料等・診療報酬上臨時的取扱）
        /// </summary>
        public const string IgakuNyuyojiKansen = "113033270";

        /// <summary>
        /// 小児科外来診療料取消し
        /// </summary>
        public const string IgakuSyouniGairaiCancel = "X00027";

        #region 医学管理関連-小児カウンセリング
        /// <summary>
        /// 小児特定疾患カウンセリング料（１回目）
        /// </summary>
        public const string IgakuSyouniCounseling1 = "113000810";
        /// <summary>
        /// 小児特定疾患カウンセリング料（２回目）
        /// </summary>
        public const string IgakuSyouniCounseling2 = "113009910";
        /// <summary>
        /// 小児特定疾患カウンセリング料（公認心理師）
        /// </summary>
        public const string IgakuSyouniCounselingKounin = "113029410";
        #endregion

        #region 医学管理関連-認知症地域包括診療料関連

        /// <summary>
        /// 認知症地域包括診療料１
        /// </summary>
        public const string IgakuNintiTiikiHoukatu1 = "113025710";
        /// <summary>
        /// 認知症地域包括診療料２
        /// </summary>
        public const string IgakuNintiTiikiHoukatu2 = "113018410";
        /// <summary>
        /// 時間外加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuJikangai = "113018570";
        /// <summary>
        /// 休日加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuKyujitu = "113018670";
        /// <summary>
        /// 深夜加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuSinya = "113018770";
        /// <summary>
        /// 時間外特例医療機関加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuJikangaiToku = "113019170";
        /// <summary>
        /// 乳幼児時間外加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuJikangai = "113018870";
        /// <summary>
        /// 乳幼児休日加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuKyujitu = "113018970";
        /// <summary>
        /// 乳幼児深夜加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuSinya = "113019070";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuJikangaiToku = "113019270";
        /// <summary>
        /// 夜間・早朝等加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuYasou = "113019670";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuSyouniYakan = "113019370";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuSyouniKyujitu = "113019470";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNyuSyouniSinya = "113019570";
        /// <summary>
        /// 妊婦時間外加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuJikangai = "113025870";
        /// <summary>
        /// 妊婦休日加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuKyujitu = "113025970";
        /// <summary>
        /// 妊婦深夜加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuSinya = "113026070";
        /// <summary>
        /// 妊婦時間外特例医療機関加算（再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuJikangaiToku = "113026170";
        /// <summary>
        /// 妊婦夜間加算（産科又は産婦人科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuSankaYakan = "113026270";
        /// <summary>
        /// 妊婦休日加算（産科又は産婦人科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuSankaKyujitu = "113026370";
        /// <summary>
        /// 妊婦深夜加算（産科又は産婦人科再診）（認知症地域包括診療料）
        /// </summary>
        public const string IgakuNintiTiikiHoukatuNinpuSankaSinya = "113026470";

        #endregion

        #region 医学管理関連-地域包括診療料関連

        /// <summary>
        /// 地域包括診療料１
        /// </summary>
        public const string IgakuTiikiHoukatu1 = "113024810";
        /// <summary>
        /// 地域包括診療料２
        /// </summary>
        public const string IgakuTiikiHoukatu2 = "113015810";

        /// <summary>
        /// 時間外加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuJikangai = "113016270";
        /// <summary>
        /// 休日加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuKyujitu = "113016370";
        /// <summary>
        /// 深夜加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuSinya = "113016470";
        /// <summary>
        /// 時間外特例医療機関加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuJikangaiToku = "113016870";
        /// <summary>
        /// 乳幼児時間外加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuJikangai = "113016570";
        /// <summary>
        /// 乳幼児休日加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuKyujitu = "113016670";
        /// <summary>
        /// 乳幼児深夜加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuSinya = "113016770";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuJikangaiToku = "113016970";
        /// <summary>
        /// 夜間・早朝等加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuYasou = "113017370";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuSyouniYakan = "113017070";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuSyouniKyujitu = "113017170";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNyuSyouniSinya = "113017270";
        /// <summary>
        /// 妊婦時間外加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuJikangai = "113024970";
        /// <summary>
        /// 妊婦休日加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuKyujitu = "113025070";
        /// <summary>
        /// 妊婦深夜加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuSinya = "113025170";
        /// <summary>
        /// 妊婦時間外特例医療機関加算（再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuJikangaiToku = "113025270";
        /// <summary>
        /// 妊婦夜間加算（産科又は産婦人科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuSankaYakan = "113025370";
        /// <summary>
        /// 妊婦休日加算（産科又は産婦人科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuSankaKyujitu = "113025470";
        /// <summary>
        /// 妊婦深夜加算（産科又は産婦人科再診）（地域包括診療料）
        /// </summary>
        public const string IgakuTiikiHoukatuNinpuSankaSinya = "113025570";

        /// <summary>
        /// 再診時療養指導管理料
        /// </summary>
        public const string IgakuSaisinRyoyo = "101130190";
        /// <summary>
        /// 再診時療養指導管理料取消し
        /// </summary>
        public const string IgakuSaisinRyoyoCancel = "@13991";
        #endregion

        /// <summary>
        /// 慢性疼痛疾患管理料
        /// </summary>
        public const string IgakuManseiTotu = "113006510";

        #region 医学管理関連-慢性維持透析患者
        /// <summary>
        /// 慢性維持透析患者外来医学管理料
        /// </summary>
        public const string IgakuManseiIji = "113002510";
        /// <summary>
        /// 腎代替療法実績加算
        /// </summary>
        public const string IgakuManseiIjiJinDaitai = "113024270";
        #endregion

        #region 医学管理関連-ニコチン依存症

        /// <summary>
        /// ニコチン依存症管理料（初回）
        /// </summary>
        public const string IgakuNico1 = "113008310";
        /// <summary>
        /// ニコチン依存症管理料１（初回）（診療報酬上臨時的取扱）
        /// </summary>
        public const string IgakuNico1Rinsyojyo = "113033450";
        /// <summary>
        /// ニコチン依存症管理料（２回目から４回目まで）
        /// </summary>
        public const string IgakuNico2_4 = "113008410";
        /// <summary>
        /// ニコチン依存症管理料（５回目）
        /// </summary>
        public const string IgakuNico5 = "113008510";
        /// <summary>
        /// ニコチン依存症管理料１（５回目）（診療報酬上臨時的取扱）
        /// </summary>
        public const string IgakuNico5Rinsyojyo = "113033550";
        /// <summary>
        /// ニコチン依存症管理料１（２回目から４回目まで）（情報通信機器）
        /// </summary>
        public const string IgakuNico2_4Tusin = "113031610";
        /// <summary>
        /// ニコチン依存症管理料２
        /// </summary>
        public const string IgakuNico2 = "113031710";
        #endregion

        #region 医学管理関連-喘息治療管理料
        /// <summary>
        /// 喘息治療管理料１（１月目）
        /// </summary>
        public const string IgakuZensoku1_1 = "113005710";
        /// <summary>
        /// 喘息治療管理料１（２月目以降）
        /// </summary>
        public const string IgakuZensoku1_2 = "113004910";
        #endregion

        #region 医学管理関連-重度喘息患者治療管理加算
        /// <summary>
        /// 重度喘息患者治療管理加算（１月目）
        /// </summary>
        public const string IgakuJyudoZensoku1 = "113008070";
        /// <summary>
        /// 重度喘息患者治療管理加算（２月目以降６月目まで）
        /// </summary>
        public const string IgakuJyudoZensoku2_6 = "113008170";
        #endregion

        /// <summary>
        /// 皮膚科特定疾患療養指導料（Ⅰ）
        /// </summary>
        public const string SiHifuToku1 = "113000910";
        /// <summary>
        /// 皮膚科特定疾患指導管理料（１）（情報通信機器）
        /// </summary>
        public const string SiHifuToku1JyohoTusin = "113034510";

        /// <summary>
        /// 皮膚科特定疾患療養指導料（Ⅱ）
        /// </summary>
        public const string SiHifuToku2 = "113002310";
        /// <summary>
        /// 皮膚科特定疾患指導管理料（２）（情報通信機器）
        /// </summary>
        public const string SiHifuToku2JyohoTusin = "113034610";

        /// <summary>
        /// オンライン医学管理料
        /// </summary>
        public const string IgakuOnlineIgakuKanri = "113023890";

        /// <summary>
        /// 特定疾患療養管理料（診療所）
        /// </summary>
        public const string IgakuTokusitu = "113001810";
        /// <summary>
        /// 特定疾患療養管理料（診療所・情報通信機器）
        /// </summary>
        public const string IgakuTokusitu1 = "113034010";
        /// <summary>
        /// 特定薬剤治療管理料１
        /// </summary>
        public const string IgakuTokuYaku = "113000410";
        /// <summary>
        /// 特定薬剤治療管理料１（第４月目以降）
        /// </summary>
        public const string IgakuTokuYaku4 = "113000510";
        /// <summary>
        /// 小児科療養指導料
        /// </summary>
        public const string IgakuSyouniRyoyo = "113002210";
        /// <summary>
        /// てんかん指導料
        /// </summary>
        public const string IgakuTenkan = "113002850";
        /// <summary>
        /// 難病外来指導管理料
        /// </summary>
        public const string IgakuNanbyo = "113002910";

        #region 医学管理関連-糖尿病透析予防指導管理料
        /// <summary>
        /// 糖尿病透析予防指導管理料
        /// </summary>
        public const string IgakuTounyou = "113013610";
        /// <summary>
        /// 糖尿病透析予防指導管理料（特定地域）
        /// </summary>
        public const string IgakuTounyouToku = "113015610";
        #endregion

        #region 医学管理関連-生活習慣病管理料
        #region ～2022/3/31
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付）（脂質異常症を主病）
        /// </summary>
        public const string IgakuSeikatuKofuAriSisitu = "113005810";
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付）（糖尿病を主病）
        /// </summary>
        public const string IgakuSeikatuKofuAriTounyou = "113005910";
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付しない）（脂質異常症を主病）
        /// </summary>
        public const string IgakuSeikatuKofuNasiSisitu = "113006010";
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付しない）（糖尿病を主病）
        /// </summary>
        public const string IgakuSeikatuKofuNasiTounyou = "113006110";
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付）（高血圧症を主病）
        /// </summary>
        public const string IgakuSeikatuKofuAriKouketuatu = "113003910";
        /// <summary>
        /// 生活習慣病管理料（処方箋を交付しない）（高血圧症を主病）
        /// </summary>
        public const string IgakuSeikatuKofuNasiKouketuatu = "113004010";
        #endregion
        #region 2022/4/1～
        /// <summary>
        /// 生活習慣病管理料（脂質異常症を主病）
        /// </summary>
        public const string IgakuSeikatuSisitu = "113041710";
        /// <summary>
        /// 生活習慣病管理料（高血圧症を主病）
        /// </summary>
        public const string IgakuSeikatuKouketuatu = "113041810";
        /// <summary>
        /// 生活習慣病管理料（糖尿病を主病）
        /// </summary>
        public const string IgakuSeikatuTounyou = "113041910";
        #endregion
        #endregion

        #region 医学管理関連-外来リハビリテーション診療料
        /// <summary>
        /// 外来リハビリテーション診療料１
        /// </summary>
        public const string IgakuGairaiRiha1 = "113013910";
        /// <summary>
        /// 外来リハビリテーション診療料２
        /// </summary>
        public const string IgakuGairaiRiha2 = "113014010";
        #endregion

        /// <summary>
        /// 外来放射線照射診療料
        /// </summary>
        public const string IgakuGairaiHosya = "113014110";

        /// <summary>
        /// 救急救命管理料
        /// </summary>
        public const string IgakuKyukyuKyumei = "113002610";

        /// <summary>
        /// 退院後訪問指導料
        /// </summary>
        public const string IgakuKTaiinHoumon = "113022910";

        /// <summary>
        /// 施設基準不適合減算（医学管理等）（１００分の７０）
        /// </summary>
        public const string IgakuSisetuKijyun = "113023770";

        /// <summary>
        /// 外来感染対策向上加算（医学管理等）
        /// </summary>
        public const string IgakuKansenKojo = "113033790";
        /// <summary>
        /// 連携強化加算（医学管理等）
        /// </summary>
        public const string IgakuRenkeiKyoka = "113033890";
        /// <summary>
        /// サーベイランス強化加算（医学管理等）
        /// </summary>
        public const string IgakuSurveillance = "113033990";

        #region 外来腫瘍化学療法診療料
        /// <summary>
        /// 外来腫瘍化学療法診療料１（抗悪性腫瘍剤を投与）
        /// </summary>
        public const string IgakuGairaiSyuyo1 = "113038010";
        /// <summary>
        /// 外来腫瘍化学療法診療料１（抗悪性腫瘍剤投与その他必要な治療管理）
        /// </summary>
        public const string IgakuGairaiSyuyo1Sonota = "113038110";
        /// <summary>
        /// 外来腫瘍化学療法診療料２（抗悪性腫瘍剤を投与）
        /// </summary>
        public const string IgakuGairaiSyuyo2 = "113038210";
        /// <summary>
        /// 外来腫瘍化学療法診療料２（抗悪性腫瘍剤投与その他必要な治療管理）
        /// </summary>
        public const string IgakuGairaiSyuyo2Sonota = "113038310";

        /// <summary>
        /// 小児加算（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoSyoni = "113041470";
        /// <summary>
        /// 連携充実加算（外来腫瘍化学療法診療料１・イ）
        /// </summary>
        public const string IgakuGairaiSyuyoRenkeiJujitu = "113041570";
        /// <summary>
        /// バイオ後続品導入初期加算（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoBio = "113041670";


        /// <summary>
        /// 時間外加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoJikangaiSyosin = "113038570";
        /// <summary>
        /// 休日加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoKyujituSyosin = "113038670";
        /// <summary>
        /// 深夜加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoSinyaSyosin = "113038770";
        /// <summary>
        /// 時間外特例医療機関加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoJikangaiTokuSyosin = "113038970";
        /// <summary>
        /// 乳幼児加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyosin = "113038470";
        /// <summary>
        /// 乳幼児時間外加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuJikangaiSyosin = "113038870";
        /// <summary>
        /// 乳幼児休日加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuKyujituSyosin = "113043670";
        /// <summary>
        /// 乳幼児深夜加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSinyaSyosin = "113043770";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuJikangaiTokuSyosin = "113039070";
        /// <summary>
        /// 乳幼児夜間加算（小児科初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniYakanSyosin = "113039170";
        /// <summary>
        /// 乳幼児休日加算（小児科初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniKyujituSyosin = "113039270";
        /// <summary>
        /// 乳幼児深夜加算（小児科初診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniSinyaSyosin = "113039370";

        /// <summary>
        /// 時間外加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoJikangaiSaisin = "113039570";
        /// <summary>
        /// 休日加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoKyujituSaisin = "113039670";
        /// <summary>
        /// 深夜加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoSinyaSaisin = "113039770";
        /// <summary>
        /// 時間外特例医療機関加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoJikangaiTokuSaisin = "113039970";
        /// <summary>
        /// 乳幼児加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSaisin = "113039470";
        /// <summary>
        /// 乳幼児時間外加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuJikangaiSaisin = "113039870";
        /// <summary>
        /// 乳幼児休日加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuKyujituSaisin = "113043870";
        /// <summary>
        /// 乳幼児深夜加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSinyaSaisin = "113043970";
        /// <summary>
        /// 乳幼児時間外特例医療機関加算（再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuJikangaiTokuSaisin = "113040070";
        /// <summary>
        /// 乳幼児夜間加算（小児科再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniYakanSaisin = "113040170";
        /// <summary>
        /// 乳幼児休日加算（小児科再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniKyujituSaisin = "113040270";
        /// <summary>
        /// 乳幼児深夜加算（小児科再診）（外来腫瘍化学療法診療料）
        /// </summary>
        public const string IgakuGairaiSyuyoNyuSyouniSinyaSaisin = "113040370";
        #endregion

        #region アレルギー性鼻炎免疫療法治療管理料
        /// <summary>
        /// アレルギー性鼻炎免疫療法治療管理料（１月目）
        /// </summary>
        public const string IgakuBienMeneki1 = "113036810";
        /// <summary>
        /// アレルギー性鼻炎免疫療法治療管理料（２月目以降）
        /// </summary>
        public const string IgakuBienMeneki2 = "113036910";
        #endregion

        #region 情報通信機器

        /// <summary>
        /// 特定疾患療養管理料（情報通信機器）
        /// </summary>
        public const string IgakuTokusituJyohoTusin = "113029010";
        /// <summary>
        /// 小児科療養指導料（情報通信機器）
        /// </summary>
        public const string IgakuSyouniRyoyoJyohoTusin = "113029510";
        /// <summary>
        /// てんかん指導料（情報通信機器）
        /// </summary>
        public const string IgakuTenkanJyohoTusin = "113029610";
        /// <summary>
        /// 難病外来指導管理料（情報通信機器）
        /// </summary>
        public const string IgakuNanbyoJyohoTusin = "113029710";
        /// <summary>
        /// 糖尿病透析予防指導管理料（情報通信機器）
        /// </summary>
        public const string IgakuTounyouJyohoTusin = "113030910";
        /// <summary>
        /// 地域包括診療料（情報通信機器）
        /// </summary>
        public const string IgakuTiikiHoukatuJyohoTusin = "113031310";
        /// <summary>
        /// 認知症地域包括診療料（情報通信機器）
        /// </summary>
        public const string IgakuNintiTiikiJyohoTusin = "113031410";
        /// <summary>
        /// 生活習慣病管理料（情報通信機器）
        /// </summary>
        public const string IgakuSeikatuJyohoTusin = "113031510";
        /// <summary>
        /// 慢性疾患の診療（新型コロナウイルス感染症・診療報酬上臨時的取扱） 
        /// </summary>
        public const string IgakuManseiCorona = "113032850";
        #endregion

        /// <summary>
        /// 院内トリアージ実施料（診療報酬上臨時的取扱）
        /// </summary>
        public const string IgakuTriageRinsyo = "113032950";
        /// <summary>
        /// 医科外来等感染症対策実施加算（医学管理等）
        /// </summary>
        public const string IgakuKansenTaisaku = "113033370";
        /// <summary>
        /// 二類感染症患者入院診療加算（外来診療・診療報酬上臨時的取扱）
        /// </summary>
        public const string Igaku2RuiKansen = "113033650";
        /// <summary>
        /// 二類感染症患者入院診療加算（電話等診療・臨取）（重点措置）
        /// </summary>
        public const string Igaku2RuiKansenJyuten = "113044350";
        #endregion

        #region 薬剤情報提供料
        public const string YakuzaiJoho = "120002370";
        public const string YakuzaiJohoTeiyo = "113701310";
        #endregion

        #region ZAN
        public static string ZanGigi = "@ZANGIGI";
        public static string ZanTeiKyo = "@ZANTEIKYO";
        #endregion

        #region 乳幼児育児栄養指導料
        public const string SiIkuji = "111000470";
        /// <summary>
        /// 乳幼児育児栄養指導料（情報通信機器）
        /// </summary>
        public const string SiIkujiJyohoTusin = "113037110";
        #endregion

        #region 在宅関連
        /// <summary>
        /// 手技料なし（在宅）
        /// </summary>
        public const string ZaiSyuginasi = "X00001";
        /// <summary>
        /// 往診
        /// </summary>
        public const string ZaiOusin = "114000110";
        /// <summary>
        /// 特別往診
        /// </summary>
        public const string ZaiOusinTokubetu = "114001610";
        /// <summary>
        /// 患家診療時間加算（往診）
        /// </summary>
        public const string ZaiOusinKanka = "114000970";
        /// <summary>
        /// 患家診療時間加算（特別往診）
        /// </summary>
        public const string ZaiOusinKankaTokubetu = "114002470";
        /// <summary>
        /// 滞在時間加算（１号地域）
        /// </summary>
        public const string ZaiOusinTaizai1Go = "114002870";
        /// <summary>
        /// 往診往復時間加算（２号地域）
        /// </summary>
        public const string ZaiOusinOufuku2Go = "114002970";
        /// <summary>
        /// 患家診療時間加算（在宅患者訪問診療料（１）・（２））
        /// </summary>
        public const string ZaiHoumonKanka = "114001470";

        #region 在宅関連-在宅患者訪問診療料
        /// <summary>
        /// 在宅患者訪問診療料（１）１（同一建物居住者以外）
        /// </summary>
        public const string ZaiHoumon1_1DouIgai = "114001110";
        /// <summary>
        /// 在宅患者訪問診療料（１）１（同一建物居住者）
        /// </summary>
        public const string ZaiHoumon1_1Dou = "114030310";
        /// <summary>
        /// 在宅患者訪問診療料（１）２（同一建物居住者以外）
        /// </summary>
        public const string ZaiHoumon1_2DouIgai = "114042110";
        /// <summary>
        /// 在宅患者訪問診療料（１）２（同一建物居住者）
        /// </summary>
        public const string ZaiHoumon1_2Dou = "114042210";
        /// <summary>
        /// 在宅患者訪問診療料（２）イ
        /// </summary>
        public const string ZaiHoumon2i = "114042810";
        /// <summary>
        /// 在宅患者訪問診療料（２）ロ（他の保険医療機関から紹介された患者）
        /// </summary>
        public const string ZaiHoumon2ro = "114046310";
        #endregion

        #region 在宅関連-在宅患者訪問看護指示料
        /// <summary>
        /// 在宅患者訪問看護･指導料(保健師、助産師、看護師･週３日目まで)
        /// </summary>
        public const string ZaiHoumonKangoHoken_3 = "114004510";
        /// <summary>
        /// 在宅患者訪問看護・指導料（准看護師）（週３日目まで）
        /// </summary>
        public const string ZaiHoumonKangoJyunkan_3 = "114004610";
        /// <summary>
        /// 在宅患者訪問看護･指導料(保健師、助産師、看護師･週４日目以降)
        /// </summary>
        public const string ZaiHoumonKangoHoken4_ = "114010610";
        /// <summary>
        /// 在宅患者訪問看護・指導料（准看護師）（週４日目以降）
        /// </summary>
        public const string ZaiHoumonKangoJyunkan4_ = "114010710";
        /// <summary>
        /// 在宅患者訪問看護･指導料(緩和、褥瘡、人工肛門ケア等専門看護師
        /// </summary>
        public const string ZaiHoumonKangoKanwa = "114020110";

        #endregion
        #region 在宅関連-在がん医総
        /// <summary>
        /// 在がん医総（在支診等）（処方箋あり）
        /// </summary>
        public const string ZaiZaiganSyohoAri = "114007610";
        /// <summary>
        /// 在がん医総（在支診等）（処方箋なし）
        /// </summary>
        public const string ZaiZaiganSyohoNasi = "114007710";
        /// <summary>
        /// 在がん医総（機能強化した在支診等）（病床あり）（処方箋あり）
        /// </summary>
        public const string ZaiZaiganByoAriSyohoAri = "114019510";
        /// <summary>
        /// 在がん医総（機能強化した在支診等）（病床あり）（処方箋なし）
        /// </summary>
        public const string ZaiZaiganByoAriSyohoNasi = "114019610";
        /// <summary>
        /// 在がん医総（機能強化した在支診等）（病床なし）（処方箋あり）
        /// </summary>
        public const string ZaiZaiganByoNasiSyohoAri = "114019710";
        /// <summary>
        /// 在がん医総（機能強化した在支診等）（病床なし）（処方箋なし）
        /// </summary>
        public const string ZaiZaiganByoNasiSyohoNasi = "114019810";
        /// <summary>
        /// 在がん医総算定開始日
        /// </summary>
        public const string ZaiZaiganStart = "X00013";
        /// <summary>
        /// 在がん医総取消ダミー
        /// </summary>
        public const string ZaiZaiganCancel = "X100004";
        #endregion
        /// <summary>
        /// 開放型病院共同指導料（１）
        /// </summary>
        public const string ZaiKaihouSido1 = "180010510";
        /// <summary>
        /// 死亡診断加算（在宅患者訪問診療料（１）１・（２）イ）
        /// </summary>
        public const string SiboSindanSyoHoumon = "114018670 ";
        /// <summary>
        /// 死亡診断加算
        /// </summary>
        public const string SiboSindanSyoZaigan = "114019970";

        #region 在宅関連-在医総・施医総
        /// <summary>
        /// 在医総管・施医総管（在支診等以外）（１００分の８０）減算
        /// </summary>
        public const string ZaiZaiisoFuteki = "114041870";
        /// <summary>
        /// 在医総管・施医総管（１００分の８０）減算取消し
        /// </summary>
        public const string ZaiZaiisoFutekiCancel = "X00043";
        /// <summary>
        /// 処方箋無交付加算（在医総管・施医総管）
        /// </summary>
        public const string ZaiZaiisoMukofu = "114034370";
        /// <summary>
        /// 処方せん無交付加算取消し
        /// </summary>
        public const string ZaiZaiisoMukofuCancel = "X100001";
        #endregion
        /// <summary>
        /// 在宅患者訪問点滴注射管理指導料
        /// </summary>
        public const string ZaiHoumonTenteki = "114011410";

        #region 在宅関連-在宅ターミナルケア加算
        /// <summary>
        /// 在宅ターミナルケア加算（ロ）（在支診等）
        /// </summary>
        public const string ZaiTerminalRoZai = "114042570";
        /// <summary>
        /// 在宅ターミナルケア加算（ロ）（在支診等以外）
        /// </summary>
        public const string ZaiTerminalRoZaiGai = "114042670";
        /// <summary>
        /// 在宅ターミナルケア加算（ロ）（機能強化した在支診等）（病床あり）
        /// </summary>
        public const string ZaiTerminalRoKyokaAri = "114042370";
        /// <summary>
        /// 在宅ターミナルケア加算（ロ）（機能強化した在支診）（病床なし）
        /// </summary>
        public const string ZaiTerminalRoKyokaNasi = "114042470";
        /// <summary>
        /// 在宅ターミナルケア加算(イ)(機能強化した在支診等) (病床あり)
        /// </summary>
        public const string ZaiTerminalIKyokaAri = "114018170";
        /// <summary>
        /// 在宅ターミナルケア加算(イ)(機能強化した在支診) (病床なし)
        /// </summary>
        public const string ZaiTerminalIKyokaNasi = "114018270";
        /// <summary>
        /// 在宅ターミナルケア加算（イ）（在支診等）
        /// </summary>  
        public const string ZaiTerminalIZai = "114018370";
        /// <summary>
        /// 在宅ターミナルケア加算（イ）（在支診等以外）
        /// </summary>        
        public const string ZaiTerminalIZaiGai = "114018470";
        /// <summary>
        /// 在宅ターミナルケア加算（２）（在支診等以外）
        /// </summary>
        public const string ZaiTerminal2ZaiGai = "114043270";
        /// <summary>
        /// 在宅ターミナルケア加算（２）（機能強化した在支診等）（病床あり）
        /// </summary>
        public const string ZaiTerminal2KyokaAri = "114042970";
        /// <summary>
        /// 在宅ターミナルケア加算（２）（機能強化した在支診）（病床なし）
        /// </summary>
        public const string ZaiTerminal2KyokaNasi = "114043070";
        /// <summary>
        /// 在宅ターミナルケア加算（２）（在支診等）
        /// </summary>
        public const string ZaiTerminal2Zai = "114043170";
        /// <summary>
        /// 在宅ターミナルケア加算（在宅、特養等・看取り介護加算等算定除く）
        /// </summary>
        public const string ZaiTerminalSonota = "114044370";
        /// <summary>
        /// 在宅ターミナルケア加算（特養等（看取り介護加算等算定））
        /// </summary>
        public const string ZaiTerminalTokuyo = "114044470";
        #endregion

        #region 在宅関連-酸素療法加算
        /// <summary>
        /// 酸素療法加算（在宅患者訪問診療料（１）１）
        /// </summary>
        public const string ZaiSansoRyohoKasan1 = "114042770";
        ///<summary>
        ///酸素療法加算（在宅患者訪問診療料（２）イ）
        ///</summary>
        public const string ZaiSansoRyohoKasan2 = "114043670";

        #endregion
        /// <summary>
        /// 看取り加算（在宅患者訪問診療料（１）１・（２）イ・往診料）
        /// </summary>
        public const string ZaiMitoriKasan = "114018570";
        /// <summary>
        /// 訪問看護指示料
        /// </summary>
        public const string ZaiHoumonKango = "114008010";

        #region 在宅緩和ケア充実診療所・病院加算
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（在がん医総）
        /// </summary>
        public const string ZaiKanwaCareZaigan = "114040270";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（施医総管）（１０人～）	
        /// </summary>
        public const string ZaiKanwaCareSiiso10_ = "114039570";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（施医総管）（１人）	
        /// </summary>
        public const string ZaiKanwaCareSiiso1 = "114039370";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（施医総管）（２人～９人）	
        /// </summary>
        public const string ZaiKanwaCareSiiso2_9 = "114039470";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（在医総管）（１人）	
        /// </summary>
        public const string ZaiKanwaCareZaiiso1 = "114034570";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（在医総管）（２人～９人）
        /// </summary>
	    public const string ZaiKanwaCareZaiiso2_9 = "114034670";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（在医総管）（１０人～）
        /// </summary>
        public const string ZaiKanwaCareZaiiso10 = "114034770";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算（往診）
        /// </summary>
        public const string ZaiKanwaCareOusin = "114029670";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算(在宅患者訪問診療料(１)１)	
        /// </summary>
        public const string ZaiKanwaCareHoumon1 = "114030470";
        /// <summary>
        /// 在宅緩和ケア充実診療所・病院加算(在宅患者訪問診療料(２)イ)
        /// </summary>
        public const string ZaiKanwaCareHoumon2 = "114043370";
        #endregion

        #region 在宅療養実績加算
        /// <summary>
        /// 在宅療養実績加算１（在がん医総）
        /// </summary>
        public const string ZaiRyoyoJisseki1Zaigan = "114040370";
        /// <summary>
        /// 在宅療養実績加算２（在がん医総）
        /// </summary>
        public const string ZaiRyoyoJisseki2Zaigan = "114040470";
        #endregion

        /// <summary>
        /// 在宅自己注射指導管理料（情報通信機器）
        /// </summary>
        public const string ZaiJikochuJyohoTusin = "114049910";
        /// <summary>
        /// 在宅患者緊急時等カンファレンス料
        /// </summary>
        public const string ZaiConference = "114015410";
        /// <summary>
        /// 医科外来等感染症対策実施加算（在宅医療）
        /// </summary>
        public const string ZaiKansenTaisaku = "114051070";

        /// <summary>
        /// 外来感染対策向上加算（医学管理等）
        /// </summary>
        public const string ZaitakuKansenKojo = "114055090";
        /// <summary>
        /// 連携強化加算（医学管理等）
        /// </summary>
        public const string ZaitakuRenkeiKyoka = "114055190";
        /// <summary>
        /// サーベイランス強化加算（医学管理等）
        /// </summary>
        public const string ZaitakuSurveillance = "114055290";
        #endregion

        #region 検査関連
        /// <summary>
        /// 手技料なし（検査）
        /// </summary>
        public const string KensaSyuginasi = "X00007";

        #region 検査関連-まるめ項目

        #region 検査関連-まるめ項目-生化学（Ⅰ）
        /// <summary>
        /// 生化学検査（Ｉ）（５～７項目）
        /// </summary>
        public const string KensaMarumeSeika5_7 = "@60010";
        /// <summary>
        /// 生化学検査（Ｉ）（８・９項目）
        /// </summary>
        public const string KensaMarumeSeika8_9 = "@60011";
        /// <summary>
        /// 生化学検査（Ｉ）（１０項目以上）
        /// </summary>
        public const string KensaMarumeSeika10 = "@60012";
        #endregion

        #region 検査関連-まるめ項目-内分泌学検査
        /// <summary>
        /// 内分泌学検査（３～５項目）　
        /// </summary>
        public const string KensaMarumeNaibunpitu3_5 = "@60020";
        /// <summary>
        /// 内分泌学検査（６・７項目）
        /// </summary>
        public const string KensaMarumeNaibunpitu6_7 = "@60021";
        /// <summary>
        /// 内分泌学検査（８項目以上）　　
        /// </summary>
        public const string KensaMarumeNaibunpitu8 = "@60022";
        #endregion

        #region 検査関連-まるめ項目-肝炎ウイルス関連検査
        /// <summary>
        /// 肝炎ウイルス関連検査（３項目）
        /// </summary>
        public const string KensaMarumeKanen3 = "@60030";
        /// <summary>
        /// 肝炎ウイルス関連検査（４項目）
        /// </summary>
        public const string KensaMarumeKanen4 = "@60031";
        /// <summary>
        /// 肝炎ウイルス関連検査（５項目以上）
        /// </summary>
        public const string KensaMarumeKanen5 = "@60032";
        #endregion

        #region 検査関連-まるめ項目-腫瘍マーカー精密検査
        /// <summary>
        /// 腫瘍マーカー精密検査（２項目）
        /// </summary>
        public const string KensaMarumeSyuyou2 = "@60050";
        /// <summary>
        /// 腫瘍マーカー精密検査（３項目）
        /// </summary>
        public const string KensaMarumeSyuyou3 = "@60051";
        /// <summary>
        /// 腫瘍マーカー精密（４項目以上）
        /// </summary>
        public const string KensaMarumeSyuyou4 = "@60052";
        #endregion

        #region 検査関連-まるめ項目-出血・凝固検査
        /// <summary>
        /// 出血・凝固検査（３・４項目）
        /// </summary>
        public const string KensaMarumeSyukketu3_4 = "@60060";
        /// <summary>
        /// 出血・凝固検査（５項目以上）
        /// </summary>
        public const string KensaMarumeSyukketu5 = "@60061";
        #endregion

        #region 検査関連-まるめ項目-自己抗体検査
        /// <summary>
        /// 自己抗体検査（２項目）
        /// </summary>
        public const string KensaMarumeJikoKoutai2 = "@60033";
        /// <summary>
        /// 自己抗体検査（３項目以上）
        /// </summary>
        public const string KensaMarumeJikoKoutai3 = "@60034";
        #endregion

        #region 検査関連-まるめ項目-ウイルス抗体価測定検査
        /// <summary>
        /// ウイルス抗体価測定検査上限
        /// </summary>
        public const string KensaMarumeVirus8 = "@60221";
        #endregion

        #region 検査関連-まるめ項目-グロブリンクラス別ウイルス抗体価
        /// <summary>
        /// ウイルス抗体価精密測定検査上限
        /// </summary>
        public const string KensaMarumeGlobulin2 = "@60223";
        #endregion

        #region 検査関連-まるめ項目-悪性腫瘍遺伝子検査
        /// <summary>
        /// 悪性腫瘍組織検査（２項目）
        /// 2020/04~
        /// 悪性腫瘍組織検査（処理が容易なもの）（２項目）
        /// </summary>
        public const string KensaMarumeAkusei2 = "@60080";
        /// <summary>
        /// 悪性腫瘍組織検査（３項目以上）
        /// 2020/04~
        /// 悪性腫瘍組織検査（処理が容易なもの）（３項目）
        /// </summary>
        public const string KensaMarumeAkusei3 = "@60081";
        /// <summary>
        /// 2020/04~
        /// 悪性腫瘍組織検査（処理が容易なもの）（４項目以上）
        /// </summary>
        public const string KensaMarumeAkusei4 = "@60082";

        /// <summary>
        /// 悪性腫瘍組織検査（処理が複雑なもの）（２項目）
        /// </summary>
        public const string KensaMarumeAkuseiFukuzatu2 = "@60090";
        /// <summary>
        /// 悪性腫瘍組織検査（処理が複雑なもの）（３項目）
        /// </summary>
        public const string KensaMarumeAkuseiFukuzatu3 = "@60091";

        /// <summary>
        /// 悪性腫瘍遺伝子検査１（２項目）
        /// </summary>
        public const string KensaMarumeAkuseiKetueki1_2 = "@60101";
        /// <summary>
        /// 悪性腫瘍遺伝子検査１（３項目）
        /// </summary>
        public const string KensaMarumeAkuseiKetueki1_3 = "@60102";
        /// <summary>
        /// 悪性腫瘍遺伝子検査２（２項目）
        /// </summary>
        public const string KensaMarumeAkuseiKetueki2_2 = "@60103";
        #endregion

        #endregion

        /// <summary>
        /// 特異的ＩｇＥ半定量・定量
        /// </summary>
        public const string KensaIge = "160056110";
        /// <summary>
        /// ＨＲＴ
        /// </summary>
        public const string KensaHrt = "160162950";
        /// <summary>
        /// ＨＲＴ（９種類以上）
        /// </summary>
        public const string KensaHrt9 = "160167650";
        /// <summary>
        /// 外来迅速検体検査加算
        /// </summary>
        public const string KensaGairaiJinsoku = "160177770";

        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２核酸検出（検査委託）
        /// </summary>
        public const string KensaSARSCov2Itaku = "160223350";
        public const string KensaSARSCov2Itaku20211231 = "160229450";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２核酸検出（検査委託以外）
        /// </summary>
        public const string KensaSARSCov2ItakuGai = "160223450";
        public const string KensaSARSCov2ItakuGai20211231 = "160229550";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２抗原検出
        /// </summary>
        public const string KensaSARSCov2Kogen = "160223550";
        public const string KensaSARSCov2Kogen20211231 = "160229850";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２抗原検出（定量）
        /// </summary>
        public const string KensaSARSCov2KogenTeiryo = "160224250";
        public const string KensaSARSCov2KogenTeiryo20211231 = "160229950";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザウイルス抗原同時検出
        /// </summary>
        public const string KensaSARSInfulKogenDouji = "160226450";
        public const string KensaSARSInfulKogenDouji20211231 = "160230050";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザ核酸同時検出（検査委託）
        /// </summary>        
        public const string KensaSARSInfulItaku = "160224750";
        public const string KensaSARSInfulItaku20211231 = "160229650";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザ核酸同時検出（検査委託以外）
        /// </summary>        
        public const string KensaSARSInfulItakugai = "160224850";
        public const string KensaSARSInfulItakugai20211231 = "160229750";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス核酸同時検出（検査委託） 
        /// </summary>
        public const string KensaSARSRsVirusItaku = "160234550";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス核酸同時検出（検査委託以外） 
        /// </summary>
        public const string KensaSARSRsVirusItakuIgai = "160234650";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス抗原同時検出（定性）
        /// </summary>
        public const string KensaSARSRsVirusKougen = "160234850";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ核酸同時検出（委託）
        /// </summary>
        public const string KensaSARSInfluRsVirusItaku = "160235250";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ核酸同時検出（委託外）
        /// </summary>        
        public const string KensaSARSInfluRsVirusItakuIgai = "160235350";
        /// <summary>
        /// ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ抗原同時検出（定性）
        /// </summary>
        public const string KensaSARSInfluRsVirusKougen = "160235450";
        /// <summary>
        /// ウイルス・細菌核酸多項目同時検出（検査委託）
        /// </summary>
        public const string KensaVirusKakusaninSARSItaku = "160224050";
        /// <summary>
        /// ウイルス・細菌核酸多項目同時検出（検査委託以外）
        /// </summary>
        public const string KensaVirusKakusaninSARSItakuGai = "160224150";

        #region 検査関連-採血料

        /// <summary>
        /// Ｂ－Ｖ
        /// </summary>
        public const string KensaBV = "160095710";
        /// <summary>
        /// Ｂ－Ｃ
        /// </summary>
        public const string KensaBC = "160095810";
        /// <summary>
        /// Ｂ－Ａ
        /// </summary>
        public const string KensaBA = "160101210";
        /// <summary>
        /// 採血料取消し
        /// </summary>
        public const string KensaSaiketuCancel = "@60203";
        #endregion

        #region 検査関連-判断料

        /// <summary>
        /// 尿・糞便等検査判断料
        /// </summary>
        public const string KensaHandanNyou = "160061710";
        /// <summary>
        /// 血液学的検査判断料
        /// </summary>
        public const string KensaHandanKetueki = "160061810";
        /// <summary>
        /// 生化学的検査（１）判断料
        /// </summary>
        public const string KensaHandanSeika1 = "160061910";
        /// <summary>
        /// 生化学的検査（２）判断料
        /// </summary>
        public const string KensaHandanSeika2 = "160062010";
        /// <summary>
        /// 免疫学的検査判断料
        /// </summary>
        public const string KensaHandanMeneki = "160062110";
        /// <summary>
        /// 微生物学的検査判断料
        /// </summary>
        public const string KensaHandanBiseibutu = "160062210";
        /// <summary>
        /// 呼吸機能検査等判断料
        /// </summary>
        public const string KensaHandanKokyu = "160146910";
        /// <summary>
        /// 脳波検査判断料１
        /// </summary>
        public const string KensaHandanNoha1 = "160207610";
        /// <summary>
        /// 脳波検査判断料２
        /// </summary>
        public const string KensaHandanNoha2 = "160147610";
        /// <summary>
        /// 神経・筋検査判断料
        /// </summary>
        public const string KensaHandanSinkei = "160147710";
        /// <summary>
        /// ラジオアイソトープ検査判断料
        /// </summary>
        public const string KensaHandanRadio = "160147910";
        /// <summary>
        /// 遺伝子関連・染色体検査判断料
        /// </summary>
        public const string KensaHandanIdensi = "160218110";
        /// <summary>
        /// 病理判断料
        /// </summary>
        public const string KensaHandanByori = "160062310";

        /// <summary>
        /// 尿・糞便等検査判断料取消し
        /// </summary>
        public const string KensaHandanNyouCancel = "@60204";
        /// <summary>
        /// 血液学的検査判断料取消し
        /// </summary>
        public const string KensaHandanKetuekiCancel = "@60205";
        /// <summary>
        /// 生化学的検査（１）判断料取消し
        /// </summary>
        public const string KensaHandanSeika1Cancel = "@60206";
        /// <summary>
        /// 生化学的検査（２）判断料取消し
        /// </summary>
        public const string KensaHandanSeika2Cancel = "@60207";
        /// <summary>
        /// 免疫学的検査判断料取消し
        /// </summary>
        public const string KensaHandanMenekiCancel = "@60208";
        /// <summary>
        /// 微生物学的検査判断料取消し
        /// </summary>
        public const string KensaHandanBiseibutuCancel = "@60209";
        /// <summary>
        /// 呼吸機能検査等判断料取消し
        /// </summary>
        public const string KensaHandanKokyuCancel = "@60211";
        /// <summary>
        /// 脳波検査判断料取消し
        /// </summary>
        public const string KensaHandanNohaCancel = "@60213";
        /// <summary>
        /// 神経・筋検査判断料取消し
        /// </summary>
        public const string KensaHandanSinkeiCancel = "@60214";
        /// <summary>
        /// ラジオアイソトープ検査判断料取消し
        /// </summary>
        public const string KensaHandanRadioCancel = "@60215";
        /// <summary>
        /// 遺伝子関連・染色体検査判断料取消し
        /// </summary>
        public const string KensaHandanIdensiCancel = "@60216";
        /// <summary>
        /// 病理学的検査判断料取消し
        /// </summary>
        public const string KensaHandanByoriCancel = "@60210";

        #endregion

        /// <summary>
        /// 検査逓減
        /// </summary>
        public const string KensaTeigen = "160000190";
        /// <summary>
        /// 検査逓減取消し
        /// </summary>
        public const string KensaTeigenCancel = "X00014";

        #region 検査関連-内視鏡検査

        /// <summary>
        /// 時間外加算（内視鏡検査）
        /// </summary>
        public const string KensaNaisiJikangai = "160204070";
        /// <summary>
        /// 休日加算（内視鏡検査）
        /// </summary>
        public const string KensaNaisiKyujitu = "160203970";
        /// <summary>
        /// 深夜加算（内視鏡検査）
        /// </summary>
        public const string KensaNaisiSinya = "160204170";
        /// <summary>
        /// 内視鏡検査時間外加算取消し
        /// </summary>
        public const string KensaNaisiJikanCancel = "X00028";
        #endregion

        /// <summary>
        /// 施設基準不適合減算（検査）（１００分の８０）
        /// </summary>
        public const string KensaSisetuFutekigo = "160174470";

        #endregion

        #region 画像関連
        /// <summary>
        /// 手技料なし（画像）
        /// </summary>
        public const string GazoSyuginasi = "X00006";
        /// <summary>
        /// 新生児加算（画像診断）
        /// </summary>
        public const string GazoSinseijiKasan = "170017170";
        /// <summary>
        /// 乳幼児加算（画像診断）
        /// </summary>
        public const string GazoNyuyojiKasan = "170017270";
        /// <summary>
        /// 幼児加算（画像診断）
        /// </summary>
        public const string GazoYojiKasan = "170034170";

        /// <summary>
        /// 他医撮影の写真診断（乳房撮影）
        /// </summary>
        public const string GazoTaiNyu = "170027450";
        /// <summary>
        /// 医撮影の写真診断（単純撮影・イ）
        /// </summary>
        public const string GazoTaiTanjyunI = "170001250";
        /// <summary>
        ///  他医撮影の写真診断（単純撮影・ロ）
        /// </summary>
        public const string GazoTaiTanjyunRO = "170001350";
        /// <summary>
        ///  他医撮影の写真診断（特殊撮影）
        /// </summary>
        public const string GazoTaiTokusyu = "170001450";
        /// <summary>
        /// 他医撮影の写真診断（造影剤使用撮影）
        /// </summary>
        public const string GazoTaiZouei = "170001550";
        /// <summary>
        /// 他医間接撮影の写真診断（単純撮影・イ）
        /// </summary>
        public const string GazoTaiKansetuTanjyunI = "170001650";
        /// <summary>
        /// 他医間接撮影の写真診断（単純撮影・ロ）
        /// </summary>
        public const string GazoTaiKansetuTanjyunRO = "170001750";
        /// <summary>
        /// 医間接撮影の写真診断（造影剤使用撮影）
        /// </summary>
        public const string GazoTaiKansetuZouei = "170001850";
        /// <summary>
        /// 他医撮影のコンピューター断層診断
        /// </summary>
        public const string GazoTaiComputer = "170019950";

        /// <summary>
        /// 電子媒体保存撮影（フィルム扱い）
        /// </summary>
        public const string GazoDensibaitaiHozon = "840000100";
        /// <summary>
        /// 時間外緊急院内画像診断加算
        /// </summary>
        public const string GazoKinga = "170016010";
        /// <summary>
        /// 時間外加算取消し（画像）
        /// </summary>
        public const string GazoKingaCancel = "@70041";
        /// <summary>
        /// フィルム料（乳幼児）加算
        /// </summary>
        public const string GazoFilmNyu = "799990070";
        /// <summary>
        /// フィルム算定なし
        /// </summary>
        public const string GazoNoFilm = "X00003";
        /// <summary>
        /// ２回目以降減算（ＣＴ、ＭＲＩ）
        /// </summary>
        public const string GazoCtMriGensan = "170022290";
        /// <summary>
        /// ２回目以降減算（ＣＴ、ＭＲＩ）取消し
        /// </summary>
        public const string GazoCtMriGensanCancel = "X00026";
        /// <summary>
        /// 小児鎮静下ＭＲＩ撮影加算
        /// </summary>
        public const string GazoSyoniTinseiMRI = "170036170";

        public static List<string> GazoTaisatuei = new List<string>()
        {
            // 他医撮影の写真診断（乳房撮影）
            GazoTaiNyu,
            // 他医撮影の写真診断（単純撮影・イ）
            GazoTaiTanjyunI,
            //  他医撮影の写真診断（単純撮影・ロ）
            GazoTaiTanjyunRO,
            //  他医撮影の写真診断（特殊撮影）
            GazoTaiTokusyu,
            // 他医撮影の写真診断（造影剤使用撮影）
            GazoTaiZouei,
            // 他医間接撮影の写真診断（単純撮影・イ）
            GazoTaiKansetuTanjyunI,
            // 他医間接撮影の写真診断（単純撮影・ロ）
            GazoTaiKansetuTanjyunRO,
            // 医間接撮影の写真診断（造影剤使用撮影）
            GazoTaiKansetuZouei,
            // 他医撮影のコンピューター断層診断
            GazoTaiComputer
        };
        public static List<string> ZaitakuTokushu = new List<string>()
        {
            ZaiOusin,
            ZaiOusinTokubetu,
            ZaiHoumon1_1Dou,
            ZaiHoumon1_1DouIgai,
            ZaiHoumon1_2Dou,
            ZaiHoumon1_2DouIgai,
            ZaiHoumon2i,
            ZaiHoumon2ro
        };
        #endregion

        #region 投薬関連

        #region 投薬関連-逓減

        /// <summary>
        /// （精減）
        /// </summary>
        public const string TouyakuYakuGenKousei = "820000166";
        /// <summary>
        /// （減）
        /// </summary>
        public const string TouyakuYakuGenNaifuku = "820000047";
        /// <summary>
        /// 薬剤料逓減（８０／１００）（向精神薬多剤投与）
        /// </summary>
        public const string TouyakuTeigenKousei = "630010005";
        /// <summary>
        /// 薬剤料逓減（９０／１００）（内服薬）
        /// </summary>
        public const string TouyakuTeigenNaifuku = "630010002";

        #endregion

        #region 投薬関連-処方料

        /// <summary>
        /// 処方料（その他）
        /// </summary>
        public const string TouyakuSyohoSonota = "120001210";
        /// <summary>
        /// 処方料（向精神薬多剤投与）
        /// </summary>
        public const string TouyakuSyohoKousei = "120003610";
        /// <summary>
        /// 処方料（７種類以上内服薬又は向精神薬長期処方）
        /// 2020/04~
        /// 処方料（７種類以上内服薬）
        /// </summary>
        public const string TouyakuSyohoNaifuku = "120002610";
        /// <summary>
        /// 2020/04~
        /// 処方料（向精神薬長期処方）
        /// </summary>
        public const string TouyakuSyohoKouseiChoki = "120004410";
        /// <summary>
        /// 向精神薬調整連携加算（処方料）
        /// </summary>
        public const string TouyakuKouseiRenkeiSyoho = "120004470";
        /// <summary>
        /// 麻薬等加算（処方料）
        /// </summary>
        public const string TouyakuMayakuSyoho = "120001310";
        /// <summary>
        /// 処方料取消し
        /// </summary>
        public const string TouyakuSyohoCancel = "@20080";
        #endregion

        #region 投薬関連-調剤料

        /// <summary>
        /// 調剤料（内服薬・浸煎薬・屯服薬）
        /// </summary>
        public const string TouyakuChozaiNaiTon = "120000710";
        /// <summary>
        /// 調剤料（外用薬）
        /// </summary>
        public const string TouyakuChozaiGai = "120001010";
        /// <summary>
        /// 麻薬等加算（処方料）
        /// </summary>
        public const string TouyakuMayakuChozai = "120000110";
        /// <summary>
        /// 調剤料取消し
        /// </summary>
        public const string TouyakuChozaiCancel = "@20090";
        /// <summary>
        /// 処方料（７種類以上内服薬）
        /// </summary>
        public const string TouyakuSyohoryoNaifuku = "@20011";
        /// <summary>
        /// 処方料（向精神薬長期処方）
        /// </summary>
        public const string TouyakuSyohoryoKouseiChoki = "@20012";
        #endregion

        #region 投薬関連-調剤基本料

        /// <summary>
        /// 調基（その他）
        /// </summary>
        public const string TouyakuChoKi = "120001810";
        /// <summary>
        /// 調基（その他）取消し
        /// </summary>
        public const string TouyakuChoKiCancel = "@20110";
        #endregion

        #region 投薬関連-処方箋料

        /// <summary>
        /// 処方箋料（その他）
        /// 2022/04 ～
        /// 処方箋料（リフィル以外・その他）
        /// </summary>
        public const string TouyakuSyohosenSonota = "120002910";
        /// <summary>
        /// 処方箋料（向精神薬多剤投与）
        /// 2022/04 ～
        /// 処方箋料（リフィル以外・向精神薬多剤投与）
        /// </summary>
        public const string TouyakuSyohosenKouSei = "120003710";
        /// <summary>
        /// 処方箋料（７種類以上内服薬又は向精神薬長期処方）
        /// 2020/04 ~　
        /// 処方箋料（７種類以上内服薬）
        /// 2022/04 ～
        /// 処方箋料（リフィル以外・７種類以上内服薬）
        /// </summary>
        public const string TouyakuSyohosenNaifukuKousei = "120002710";
        /// <summary>
        /// 2020/04 ~
        /// 処方箋料（向精神薬長期処方）
        /// 2022/04 ～
        /// 処方箋料（リフィル以外・向精神薬長期処方）
        /// </summary>
        public const string TouyakuSyohosenKouseiChoki = "120004610";
        /// <summary>
        /// 処方箋料（リフィル処方箋・その他）
        /// </summary>
        public const string TouyakuSyohosenSonotaRefill = "120005010";
        /// <summary>
        /// 処方箋料（リフィル処方箋・向精神薬多剤投与）
        /// </summary>
        public const string TouyakuSyohosenKouSeiRefill = "120004710";
        /// <summary>
        /// 処方箋料（リフィル処方箋・７種類以上内服薬）
        /// </summary>
        public const string TouyakuSyohosenNaifukuKouseiRefill = "120004810";
        /// <summary>
        /// 処方箋料（リフィル処方箋・向精神薬長期処方）
        /// </summary>
        public const string TouyakuSyohosenKouseiChokiRefill = "120004910";
        /// <summary>
        /// 処方せん料取消し
        /// </summary>
        public const string TouyakuSyohosenCancel = "@20100";
        /// <summary>
        /// 処方せん（乳幼児）加算
        /// </summary>
        public const string TouyakuSyohosenNyuyojiKasan = "120002470";

        #region 投薬関連-処方箋料-一般名処方加算

        /// <summary>
        /// 一般名処方加算１（処方箋料）
        /// </summary>
        public const string TouyakuIpnName1 = "120004270";
        /// <summary>
        /// 一般名処方加算２（処方箋料）
        /// </summary>
        public const string TouyakuIpnName2 = "120003570";
        /// <summary>
        /// 一般名処方加算取消し
        /// </summary>
        public const string TouyakuIpnNameCancel = "X00023";

        #endregion

        #endregion

        #region 投薬関連-外来後発医薬品使用体制加算
        /// <summary>
        /// 外来後発医薬品使用体制加算取消し
        /// </summary>
        public const string TouyakuGairaiKohatuCancel = "X100002";
        /// <summary>
        /// 外来後発医薬品使用体制加算１
        /// </summary>
        public const string TouyakuGairaiKohatu1 = "120004370";
        /// <summary>
        /// 外来後発医薬品使用体制加算２
        /// </summary>
        public const string TouyakuGairaiKohatu2 = "120004070";
        /// <summary>
        /// 外来後発医薬品使用体制加算３
        /// </summary>
        public const string TouyakuGairaiKohatu3 = "120004170";
        #endregion

        #region 投薬関連-特定疾患処方管理加算

        /// <summary>
        /// 特定疾患処方管理加算１（処方料）
        /// </summary>
        public const string TouyakuTokuSyo1Syoho = "120002270";
        /// <summary>
        /// 特定疾患処方管理加算２（処方料）
        /// </summary>
        public const string TouyakuTokuSyo2Syoho = "120003170";
        /// <summary>
        /// 特定疾患処方管理加算１（処方箋料）
        /// </summary>
        public const string TouyakuTokuSyo1Syohosen = "120002570";
        /// <summary>
        /// 特定疾患処方管理加算２（処方箋料）
        /// </summary>
        public const string TouyakuTokuSyo2Syohosen = "120003270";

        #endregion

        #endregion

        #region 注射関連

        #region 注射関連-手技なし
        /// <summary>
        /// 手技料なし（31注射）
        /// </summary>
        public const string ChusyaSyuginasi31 = "@30996";

        /// <summary>
        /// 手技料なし（32注射）
        /// </summary>
        public const string ChusyaSyuginasi32 = "@30997";

        /// <summary>
        /// 手技料なし（33注射）
        /// </summary>
        public const string ChusyaSyuginasi33 = "@30999";

        #endregion

        #region 注射関連-点滴
        /// <summary>
        /// 点滴注射（その他）（入院外）
        /// </summary>
        public const string ChusyaTenteki = "130009310";
        /// <summary>
        /// 点滴注射（乳幼児）
        /// </summary>
        public const string ChusyaTentekiNyu100 = "130003710";
        /// <summary>
        /// 点滴注射
        /// </summary>
        public const string ChusyaTenteki500 = "130003810";
        /// <summary>
        /// 訪問点滴
        /// </summary>
        public const string ChusyaHoumonTenteki = "@30033";
        /// <summary>
        /// 点滴相殺（レセ非表示）
        /// </summary>
        public const string ChusyaTentekiSosatuNoDsp = "@300001";
        /// <summary>
        /// 点滴相殺（点数調整）
        /// </summary>
        public const string ChusyaTentekiSosatuTenAdj = "@300002";
        #endregion

        /// <summary>
        /// 皮内、皮下及び筋肉内注射
        /// </summary>
        public const string ChusyaHikaKin = "130000510";
        /// <summary>
        /// 静脈内注射
        /// </summary>
        public const string ChusyaJyomyaku = "130003510";
        /// <summary>
        /// 自己注射
        /// </summary>
        public const string ChusyaJikocyu = "@30024";

        /// <summary>
        /// 生物学的製剤注射加算
        /// </summary>
        public const string ChusyaSeibutuKasan = "130000110";
        /// <summary>
        /// 麻薬注射加算
        /// </summary>
        public const string ChusyaMayakuKasan = "130000310";

        #endregion

        #region その他

        #region その他関連-目標設定等支援・管理料

        /// <summary>
        /// 目標設定等支援・管理料（初回）
        /// </summary>
        public const string SonotaMokuhyo1 = "180046110";
        /// <summary>
        /// 目標設定等支援・管理料（２回目以降）
        /// </summary>
        public const string SonotaMokuhyo2 = "180046210";

        #endregion
        /// <summary>
        /// 通院・在宅精神療法（２０歳未満）加算
        /// </summary>
        public const string SonotaTuuinSeisin20Kasan = "180020570";
        /// <summary>
        /// 児童思春期精神科専門管理加算（１６歳未満）～2022/3/31
        /// 児童思春期精神科専門管理加算（１６歳未満）（２年以内）2022/4/1～
        /// </summary>
        public const string SonotaJidoSisyunkiKasan16 = "180047270";
        /// <summary>
        /// 児童思春期精神科専門管理加算（１６歳未満）（（１）以外）
        /// </summary>
        public const string SonotaJidoSisyunkiKasan16_Sonota = "180067870";
        /// <summary>
        /// 児童思春期精神科専門管理加算（２０歳未満）
        /// </summary>
        public const string SonotaJidoSisyunkiKasan20 = "180047370";
        /// <summary>
        /// 疾患別等専門プログラム加算（精神科ショート・ケア、小規模なもの）
        /// </summary>
        public const string SonotaSikkanbetu = "180056570";
        /// <summary>
        /// 精神科オンライン在宅管理料
        /// </summary>
        public const string SonotaSeisinOnline = "180058270";
        /// <summary>
        /// 労災電子化加算
        /// </summary>
        public const string SonotaRosaiDensika = "101801000";
        /// <summary>
        /// 労災電子化加算取り消し
        /// </summary>
        public const string SonotaRosaiDensikaCancel = "X100003";
        /// <summary>
        /// その他　労災（１．５倍）
        /// </summary>
        public const string SonotaRosaiSisiKasan = "101800010";

        /// <summary>
        /// 医科外来等感染症対策実施加算（精神科訪問看護・指導料）
        /// </summary>
        public const string SonotaKansenTaisaku = "180064870";

        /// <summary>
        /// 外来感染対策向上加算（精神科訪問看護・指導料）
        /// </summary>
        public const string SonotaKansenKojo = "180068770";
        /// <summary>
        /// 連携強化加算（精神科訪問看護・指導料）
        /// </summary>
        public const string SonotaRenkeiKyoka = "180072670";
        /// <summary>
        /// サーベイランス強化加算（精神科訪問看護・指導料）
        /// </summary>
        public const string SonotaSurveillance = "180072770";

        #region 救急医療管理加算１（診療報酬上臨時的取扱）
        /// <summary>
        /// 救急医療管理加算１（診療報酬上臨時的取扱）（外来診療）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinji = "180065250";
        /// <summary>
        /// 救急医療管理加算１（診療報酬上臨時的取扱）（往診等・中和抗体薬）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiCovOhsin = "180065650";
        /// <summary>
        /// 救急医療管理加算１（診療報酬上臨時的取扱）（往診等・中和抗体薬）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa = "180065750";
        /// <summary>
        /// 救急医療管理加算１（診療報酬上臨時的取扱）（ＣＯＶ・外来診療）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiCovGairai = "180065850";
        /// <summary>
        /// 救急医療管理加算１（診療報酬上臨時的取扱）（外来・中和抗体薬）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa = "180065950";
        /// <summary>
        /// 乳幼児加算（救急医療管理加算・臨時的取扱）（外来診療・往診等）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan = "180066170";
        /// <summary>
        /// 小児加算（救急医療管理加算・臨時的取扱）（外来診療・往診等）
        /// </summary>
        public const string SonotaKyukyuIryoKanriKasanRinjiSyoniKasan = "180066270";
        #endregion

        #endregion

        #region 処置
        /// <summary>
        /// 手技料なし（処置）
        /// </summary>
        public const string SyotiSyuginasi = "X00002";
        /// <summary>
        /// ３歳未満乳幼児加算（処置）（１１０）
        /// </summary>
        public const string SyotiNyuYojiKasan1 = "140049170";
        /// <summary>
        /// ３歳未満乳幼児加算（処置）（５５）
        /// </summary>
        public const string SyotiNyuYojiKasan2 = "140049470";
        /// <summary>
        /// ６歳未満乳幼児加算（処置）（１１０）
        /// </summary>
        public const string SyotiYojiKasan1 = "140031870";
        /// <summary>
        /// ６歳未満乳幼児加算（処置）（８３）
        /// </summary>
        public const string SyotiYojiKasan2 = "140049370";
        /// <summary>
        /// ６歳未満乳幼児加算（処置）（５５）
        /// </summary>
        public const string SyotiYojiKasan3 = "140057170";

        /// <summary>
        /// 時間外加算２（イに該当を除く）（処置）
        /// </summary>
        public const string SyotiJikangai = "140000190";
        /// <summary>
        /// 休日加算２（イに該当を除く）（処置）
        /// </summary>
        public const string SyotiKyujitu = "140000290";
        /// <summary>
        /// 深夜加算２（イに該当を除く）（処置）
        /// </summary>
        public const string SyotiSinya = "140000390";
        /// <summary>
        /// 処置時間外加算取り消し
        /// </summary>
        public const string SyotiTimeKasanCancel = "@40021";

        /// <summary>
        /// 処置　労災（１．５倍）
        /// </summary>
        public const string SyotiRosaiSisiKasan = "101400020";
        /// <summary>
        /// 処置　労災（２倍）
        /// </summary>
        public const string SyotiRosaiSisiKasan2 = "101400010";

        /// <summary>
        /// 消炎鎮痛等処置（湿布処置）
        /// </summary>
        public const string SyotiSyoenSippu = "140002210";

        /// <summary>
        /// 腰部固定帯加算
        /// </summary>
        public const string SyotiYobuKoteitaiKasan = "140037490";

        /// <summary>
        /// 胸部固定帯加算
        /// </summary>
        public const string SyotiKyobuKoteitaiKasan = "140040110";

        /// <summary>
        /// 腰部固定帯加算（固定用伸縮性包帯）
        /// </summary>
        public const string SyotiKeibuKoteitaiKasan = "140052610";
        /// <summary>
        /// 耳鼻咽喉科乳幼児処置加算の追加
        /// </summary>
        public const string SyotiJibiNyuyojiKasan = "140061990";
        public const string SyotiJibiNyuyojiKasanCancel = "X100010";
        #endregion

        #region 手術
        /// <summary>
        /// 手技料なし（手術）
        /// </summary>
        public const string SyujyutuSyuginasi = "X00005";
        /// <summary>
        /// 時間外加算２（手術）
        /// </summary>
        public const string SyujyutuJikangai = "150000490";
        /// <summary>
        /// 休日加算２（手術）
        /// </summary>
        public const string SyujyutuKyujitu = "150000590";
        /// <summary>
        /// 深夜加算２（手術）
        /// </summary>
        public const string SyujyutuSinya = "150000690";

        /// <summary>
        /// 時間外加算（麻酔）
        /// </summary>
        public const string MasuiJikangai = "150231790";
        /// <summary>
        /// 休日加算（麻酔）
        /// </summary>
        public const string MasuiKyujitu = "150231890";
        /// <summary>
        /// 深夜加算（麻酔）
        /// </summary>
        public const string MasuiSinya = "150231990";

        /// <summary>
        /// 時間外加算取消し（手術）
        /// </summary>
        public const string SyujyutuTimeKasanCancel = "@50200";

        /// <summary>
        /// 極低出生体重児加算（手術）
        /// </summary>
        public const string SyujyutuMijyuku = "150306890";
        /// <summary>
        /// 新生児加算（手術）
        /// </summary>
        public const string SyujyutuSinseiji = "150000190";
        /// <summary>
        /// 乳幼児加算（手術）（３歳未満）
        /// </summary>
        public const string SyujyutuNyuyoji = "150000290";
        /// <summary>
        /// 幼児加算（手術）（３歳以上６歳未満）
        /// </summary>
        public const string SyujyutuYoji = "150342890";
        /// <summary>
        /// 未熟児加算（麻酔）
        /// </summary>
        public const string MasuiMijyuku = "150286690";
        /// <summary>
        /// 新生児加算（麻酔）
        /// </summary>
        public const string MasuiSinseiji = "150231590";
        /// <summary>
        /// 乳児加算（麻酔）
        /// </summary>
        public const string MasuiNyuji = "150231690";
        /// <summary>
        /// 幼児加算（麻酔）
        /// </summary>
        public const string MasuiYoji = "150265390";

        /// <summary>
        /// 手術　労災（１．５倍）
        /// </summary>
        public const string SyujyutuRosaiSisiKasan = "101500020";
        /// <summary>
        /// 手術　労災（２倍）
        /// </summary>
        public const string SyujyutuRosaiSisiKasan2 = "101500010";

        /// <summary>
        /// 創外固定器加算
        /// </summary>
        public const string SyujyutuSougaiKoteikiSiyoKasan = "150266970";

        #region 体外受精・顕微受精管理料
        /// <summary>
        /// 体外受精・顕微受精管理料（体外受精）
        /// </summary>
        public const string SyujyutuTaigaiJusei = "150433010";
        /// <summary>
        /// 体外受精・顕微受精管理料（顕微受精）（１個）
        /// </summary>
        public const string SyujyutuKenbiJusei1 = "150433110";
        /// <summary>
        /// 体外受精・顕微受精管理料（顕微受精）（２個から５個まで）
        /// </summary>
        public const string SyujyutuKenbiJusei2_5 = "150433210";
        /// <summary>
        /// 体外受精・顕微受精管理料（顕微受精）（６個から９個まで）
        /// </summary>
        public const string SyujyutuKenbiJusei6_9 = "150433310";
        /// <summary>
        /// 体外受精・顕微受精管理料（顕微受精）（１０個以上）
        /// </summary>
        public const string SyujyutuKenbiJusei10 = "150433410";
        /// <summary>
        /// 体外受精及び顕微受精同時実施管理料（１個）
        /// </summary>
        public const string SyujyutuJuseiDouji1 = "150436630";
        /// <summary>
        /// 体外受精及び顕微受精同時実施管理料（２個から５個まで）
        /// </summary>
        public const string SyujyutuJuseiDouji2_5 = "150436830";
        /// <summary>
        /// 体外受精及び顕微受精同時実施管理料（６個から９個まで）
        /// </summary>
        public const string SyujyutuJuseiDouji6_9 = "150436930";
        /// <summary>
        /// 体外受精及び顕微受精同時実施管理料（１０個以上）
        /// </summary>
        public const string SyujyutuJuseiDouji10 = "150436730";
        #endregion
        #region 受精卵・胚培養管理料
        /// <summary>
        /// 受精卵・胚培養管理料（１個）
        /// </summary>
        public const string SyujyutuJuseiranHaiBaiyo1 = "150434110";
        /// <summary>
        /// 受精卵・胚培養管理料（２個から５個まで）
        /// </summary>
        public const string SyujyutuJuseiranHaiBaiyo2_5 = "150434210";
        /// <summary>
        /// 受精卵・胚培養管理料（６個から９個まで）
        /// </summary>
        public const string SyujyutuJuseiranHaiBaiyo6_9 = "150434310";
        /// <summary>
        /// 受精卵・胚培養管理料（１０個以上）
        /// </summary>
        public const string SyujyutuJuseiranHaiBaiyo10 = "150434410";
        #endregion
        #region 胚凍結保存管理料（胚凍結保存管理料（導入時））
        /// <summary>
        /// 胚凍結保存管理料（胚凍結保存管理料（導入時））（１個）
        /// </summary>
        public const string SyujyutuHaiHozonDonyu1 = "150434910";
        /// <summary>
        /// 胚凍結保存管理料（胚凍結保存管理料（導入時））（２個から５個）
        /// </summary>
        public const string SyujyutuHaiHozonDonyu2_5 = "150435010";
        /// <summary>
        /// 胚凍結保存管理料（胚凍結保存管理料（導入時））（６個から９個）
        /// </summary>
        public const string SyujyutuHaiHozonDonyu6_9 = "150435110";
        /// <summary>
        /// 胚凍結保存管理料（胚凍結保存管理料・導入時・１０個以上）
        /// </summary>
        public const string SyujyutuHaiHozonDonyu10 = "150435210";
        #endregion

        #endregion

        /// <summary>
        /// 酸素補正率１．３（１気圧）
        /// </summary>
        public const string SansoHoseiRitu = "770020070";
        /// <summary>
        /// 救急医療管理加算（入院外）
        /// </summary>
        public const string KyuKyuIryoKanriKasan = "101800890";
        /// <summary>
        /// 急性増悪
        /// </summary>
        public const string KyuseiZoaku = "X00012";

        /// <summary>
        /// 消費税
        /// </summary>
        public const string Syohizei = "@900001";
        /// <summary>
        /// 内税
        /// </summary>
        public const string Uchizei = "@900002";
        /// <summary>
        /// 軽減税率分税
        /// </summary>
        public const string Keigenzei = "@900003";
        /// <summary>
        /// 内税軽減税率分税
        /// </summary>
        public const string UchiKeigenzei = "@900004";

        /// <summary>
        /// 実日数カウント
        /// </summary>
        public const string JituNissuCount = "@Z";
        /// <summary>
        /// 実日数カウント取り消し
        /// </summary>
        public const string JituNissuCountCancel = "@ZZ";
        /// <summary>
        /// 在がん医総訪問日コメント取消し
        /// </summary>
        public const string HoumonCommentCancel = "X00025";
        /// <summary>
        /// 向精神薬多剤投与減算対象外
        /// </summary>
        public const string KouseiTaisyoGai = "X0029";
        #region 労災読み替え加算

        /// <summary>
        /// 外来管理加算（読み替え加算）（検査）
        /// </summary>
        public const string RosaiYomikaeKensa = "101600150";
        /// <summary>
        /// 外来管理加算（読み替え加算）（処置）
        /// </summary>
        public const string RosaiYomikaeSyoti = "101400040";
        /// <summary>
        /// 外来管理加算（読み替え加算）（麻酔）
        /// </summary>
        public const string RosaiYomikaeMasui = "101500210";
        /// <summary>
        /// 外来管理加算（読み替え加算）（その他）
        /// </summary>
        public const string RosaiYomikaeSonota = "101800380";

        #endregion

        /// <summary>
        /// 明細なし
        /// </summary>
        public const string NoMeisai = "XNOODR";

        /// <summary>
        /// 算定しない項目
        /// </summary>
        public const string NoSantei = "9999999999";
        /// <summary>
        /// 残量破棄（注射）（数量切り上げ）
        /// </summary>
        public const string ChusyaZanryoHaki = "X00040";
        /// <summary>
        /// 残量破棄（注射）（数量切り上げ）
        /// </summary>
        public const string ZaitakuZanryoHaki = "X00035";
        /// <summary>
        /// 残量破棄（処置）（数量切り上げ）
        /// </summary>
        public const string SyotiZanryoHaki = "X00036";
        /// <summary>
        /// 残量破棄（手術）（数量切り上げ）
        /// </summary>
        public const string SyujyutuZanryoHaki = "X00037";
        /// <summary>
        /// 内服用法ダミー
        /// </summary>
        public const string YohoNaifukuDummy = "YZZZZZZZZ1";
        /// <summary>
        /// 頓服用法ダミー
        /// </summary>
        public const string YohoTonpukuDummy = "YZZZZZZZZ2";
        /// <summary>
        /// 外用用法ダミー
        /// </summary>
        public const string YohoGaiyoDummy = "YZZZZZZZZ3";

        #region コメント

        /// <summary>
        /// フリーコメント
        /// </summary>
        public const string CommentFree = "810000001";
        /// <summary>
        /// 前回実施日
        /// </summary>
        public const string CommentZenkaiJissi = "840000087";
        /// <summary>
        /// 初回実施日
        /// </summary>
        public const string CommentSyokaiJissi = "840000085";
        /// <summary>
        /// 初回算定日
        /// </summary>
        public const string CommentSyokaiSantei = "840000097";
        /// <summary>
        /// 実施日数
        /// </summary>
        public const string CommentJissiCnt = "840000096";
        /// <summary>
        /// 小児特定第１回目カウンセリング　　　年　　月　　日
        /// </summary>
        public const string CommentSyouniCounseling = "840000104";
        /// <summary>
        /// 小児特定第１回目カウンセリング 令和　　年 月 日
        /// </summary>
        public const string CommentSyouniCounselingReiwa = "840000604";
        /// <summary>
        /// 第１回目カウセンリング実施年月日（小児特定疾患カウンセリング料）
        /// </summary>
        public const string CommentSyouniCounselingSeireki = "850100051";
        /// <summary>
        /// 初回算定年月日（ニコチン依存症管理料）
        /// </summary>
        public const string CommentNicoSyokai = "850100064";
        /// <summary>
        /// ＜実施日列挙＞
        /// コメント文に列挙する診療行為のコードを記録
        /// </summary>
        public const string CommentJissiRekkyoDummy = "@800001";
        /// <summary>
        /// ＜実施日列挙項目名付き＞
        /// コメント文に列挙する診療行為のコードを記録
        /// </summary>
        public const string CommentJissiRekkyoItemNameDummy = "@800002";
        /// <summary>
        /// ＜実施日数＞
        /// コメント文に実施日数カウントする診療行為のコードを記録
        /// </summary>
        public const string CommentJissiNissuDummy = "@800003";
        /// <summary>
        /// ＜前回初回日項目名つき＞
        /// </summary>
        public const string CommentJissiNissuItemNameDummy = "@800004";
        /// <summary>
        /// ＜実施日列挙（前月末・翌月頭含む）＞
        /// 数量　0: 当月分のみ、1: 前月末含む、2:翌月初週含む、3:前月末と翌月初週含む
        /// コメント文に列挙する診療行為のコードを記録
        /// </summary>
        public const string CommentJissiRekkyoZengoDummy = "@800005";
        /// <summary>
        /// 発症　　　月　　日
        /// </summary>
        public const string CommentHassyo = "840000045";

        /// <summary>
        /// 急性増悪　　　月　　日
        /// </summary>
        public const string CommentKyuseizoaku = "840000101";

        /// <summary>
        /// 治療開始日　　　月　　日
        /// </summary>
        public const string CommentChiryo = "840000076";

        /// <summary>
        /// 初診時間内のコメント
        /// </summary>
        public const string CommentSyosinJikanNai = "@811001";

        /// <summary>
        /// 同日再診のコメント
        /// </summary>
        public const string CommentSaisinDojitu = "@812001";
        /// <summary>
        /// 電話再診のコメント
        /// </summary>
        public const string CommentDenwaSaisin = "@812002";
        /// <summary>
        /// 同日電話再診のコメント
        /// </summary>
        public const string CommentDenwaSaisinDojitu = "@812003";
        /// <summary>
        /// 妊婦
        /// </summary>
        public const string CommentNinpu = "820100348";
        /// <summary>
        /// 初回算定年月日（ＣＴ撮影）；
        /// </summary>
        public const string CommentCTSyokai = "850100198";
        /// <summary>
        /// 初回算定年月日（ＭＲＩ撮影）；
        /// </summary>
        public const string CommentMRISyokai = "850100199";

        /// <summary>
        /// 月途中まで乳幼児
        /// </summary>
        public const string CommentTukiTocyuNyu = "820100005";
        /// <summary>
        /// ２つ目の診療科（初診料）；
        /// </summary>
        public const string CommentSyosin2Kame = "830100002";
        /// <summary>
        /// ２つ目の診療科（再診料）；
        /// </summary>
        public const string CommentSaisin2Kame = "830100003";
        /// <summary>
        /// オンライン診療の適切な実施に関する指針に沿った適切な診療である（初診料）
        /// </summary>
        public const string CommentOnlineSinryoSyosin = "820100990";
        /// <summary>
        /// オンライン診療の適切な実施に関する指針に沿った適切な処方である（初診料）
        /// </summary>
        public const string CommentOnlineSyohoSyosin = "820100816";
        /// <summary>
        /// オンライン診療の適切な実施に関する指針に沿った適切な診療である（再診料）
        /// </summary>
        public const string CommentOnlineSinryoSaisin = "820100817";
        /// <summary>
        /// オンライン診療の適切な実施に関する指針に沿った適切な処方である（再診料）
        /// </summary>
        public const string CommentOnlineSyohoSaisin = "820100818";

        public const string CommentMasterCdStart = "CO";

        public const string Comment840Pattern = "840";
        public const string Comment842Pattern = "842";

        public const string Comment830Pattern = "830";
        public const string Comment831Pattern = "831";

        public const string Comment820Pattern = "820";

        public const string Comment850Pattern = "850";
        public const string Comment851Pattern = "851";
        public const string Comment852Pattern = "852";

        public const string Comment853Pattern = "853";
        public const string Comment880Pattern = "880";

        #region Jihi

        /// <summary>
        /// Jihi
        /// </summary>
        public const string ItemJihi = "J";

        #endregion

        /// <summary>
        /// 単純撮影（撮影部位）胸部（肩を除く）
        /// </summary>
        public const string CommentTanjyunSatueiKyobu = "820181220";
        /// <summary>
        /// 往診を行った年月日
        /// </summary>
        public const string CommentOusinJissi = "850100093";
        /// <summary>
        /// 訪問診療年月日（在宅がん医療総合診療料）
        /// </summary>
        public const string CommentZaiganHoumon = "850100111";
        /// <summary>
        /// 訪問看護年月日（在宅がん医療総合診療料）
        /// </summary>
        public const string CommentZaiganKango = "850100112";
        /// <summary>
        /// 湿布薬の１日用量又は投与日数（薬剤等・処方箋料）；
        /// </summary>
        public const string CommentSippuYoryo = "830100204";
        /// <summary>
        /// ７０枚を超えて湿布薬を投与した理由；
        /// </summary>
        public const string CommentSippu70 = "830000052";
        #endregion

        #region Jihi

        /// <summary>
        /// Jihi
        /// </summary>
        public const string ItemJihi = "J";

        #endregion

        /// <summary>
        /// 妊婦加算
        /// </summary>
        public static List<string> ninpuKasanls =
        new List<string>
            {
                ItemCdConst.SyosinNinpu,
                ItemCdConst.SyosinNinpuJikangai,
                ItemCdConst.SyosinNinpuKyujitu,
                ItemCdConst.SyosinNinpuSinya,
                ItemCdConst.SyosinNinpuJikangaiToku,
                ItemCdConst.SyosinNinpuKyujituToku,
                ItemCdConst.SyosinNinpuSinyaToku,
                ItemCdConst.SyosinNinpuYakanToku,

                ItemCdConst.SaisinNinpu,
                ItemCdConst.SaisinNinpuJikangai,
                ItemCdConst.SaisinNinpuKyujitu,
                ItemCdConst.SaisinNinpuSinya,
                ItemCdConst.SaisinNinpuJikangaiToku,
                ItemCdConst.SaisinNinpuKyujituToku,
                ItemCdConst.SaisinNinpuSinyaToku,
                ItemCdConst.SaisinNinpuYakanToku,

                ItemCdConst.IgakuTiikiHoukatuNinpuJikangai,
                ItemCdConst.IgakuTiikiHoukatuNinpuJikangaiToku,
                ItemCdConst.IgakuTiikiHoukatuNinpuKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNinpuSinya,
                ItemCdConst.IgakuTiikiHoukatuNinpuSankaKyujitu,
                ItemCdConst.IgakuTiikiHoukatuNinpuSankaSinya,
                ItemCdConst.IgakuTiikiHoukatuNinpuSankaYakan,

                ItemCdConst.IgakuNintiTiikiHoukatuNinpuJikangai,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuJikangaiToku,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaKyujitu,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaSinya,
                ItemCdConst.IgakuNintiTiikiHoukatuNinpuSankaYakan,
            };

        public const string NicotineruTTS10 = "620003490";
        public const string NicotineruTTS20 = "620003491";
        public const string NicotineruTTS30 = "620003492";
        public const string Cyanpics0_5 = "620006776";
        public const string Cyanpics1 = "620006777";

        /// <summary>
        /// ニコチネル系薬剤
        /// </summary>
        public static List<string> nicotineruls =
            new List<string>
            {
                ItemCdConst.NicotineruTTS10,
                ItemCdConst.NicotineruTTS20,
                ItemCdConst.NicotineruTTS30,
                ItemCdConst.Cyanpics0_5,
                ItemCdConst.Cyanpics1
            };
        /// <summary>
        /// 同日再診チェック項目
        /// </summary>
        public static List<string> doujituSaisinCheckitemCds = new List<string>
                    {
                        ItemCdConst.Syosin,
                        ItemCdConst.SyosinCorona,
                        ItemCdConst.SyosinJouhou,
                        ItemCdConst.Saisin,
                        ItemCdConst.SaisinDenwa,
                        ItemCdConst.SaisinDojitu,
                        ItemCdConst.SaisinDenwaDojitu,
                        ItemCdConst.SaisinDenwaKeizoku,
                        ItemCdConst.ZaiHoumon1_1Dou,
                        ItemCdConst.ZaiHoumon1_1DouIgai,
                        ItemCdConst.ZaiHoumon1_2Dou,
                        ItemCdConst.ZaiHoumon1_2DouIgai,
                        ItemCdConst.ZaiHoumon2i,
                        ItemCdConst.ZaiHoumon2ro,
                        ItemCdConst.ZaiKaihouSido1,
                        ItemCdConst.SyosinRousai,
                        ItemCdConst.SaisinRousai,
                        ItemCdConst.SaisinDenwaRousai,
                        ItemCdConst.SaisinDojituRousai,
                        ItemCdConst.SaisinDenwaDojituRousai,
                        ItemCdConst.SaisinJouhou,
                        ItemCdConst.SaisinJouhouDojitu
                    };
    }
}