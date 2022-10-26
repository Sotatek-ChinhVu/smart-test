using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class TenMstModel
    {
        public TenMst TenMst { get; }

        public TenMstModel(TenMst tenMst)
        {
            TenMst = tenMst;
        }

        /// <summary>
        /// 点数マスタ
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId => TenMst.HpId;

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd => TenMst.ItemCd;

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn => TenMst.SinKouiKbn;

        /// <summary>
        /// 漢字名称
        /// </summary>
        public string Name => TenMst.Name ?? string.Empty;

        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        public string OdrUnitName => TenMst.OdrUnitName ?? string.Empty;

        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        public string CnvUnitName => TenMst.CnvUnitName ?? string.Empty;

        /// <summary>
        /// レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（レセプト自体に表示しない）
        /// </summary>
        public int IsNodspRece => TenMst.IsNodspRece;

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int YohoKbn => TenMst.YohoKbn;

        /// <summary>
        /// オーダー単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位からオーダー単位へ換算するための値を表す。
        public double OdrTermVal => TenMst.OdrTermVal;

        /// <summary>
        /// 数量換算単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位から数量換算単位へ換算するための値を表す。
        /// </summary>
        public double CnvTermVal => TenMst.CnvTermVal;

        /// <summary>
        /// 個別医薬品コード
        ///     薬価基準収載医薬品コードと同様に英数12桁のコードですが、統一名収載品目の個々の商品に対して別々のコードが付与されます。
        ///     銘柄別収載品目（商品名で官報に収載されるもの）については、薬価基準収載医薬品コードと同じコードです。
        /// </summary>
        public string YjCd => TenMst.YjCd ?? string.Empty;

        /// <summary>
        /// 検査項目コード
        ///     KENSA_MST.KENSA_ITEM_CD
        /// </summary>
        public string KensaItemCd => TenMst.KensaItemCd ?? string.Empty;

        /// <summary>
        /// 検査項目コード連番
        ///     KENSA_MST.SEQ_NO
        /// </summary>
        public int KensaItemSeqNo => TenMst.KensaItemSeqNo;

        /// <summary>
        /// 後発医薬品区分
        ///     当該医薬品が後発医薬品に該当するか否かを表す。
        ///     　0: 後発医薬品でない
        ///     　1: 先発医薬品がある後発医薬品である
        ///     ※基金マスタの設定、オーダー時はKOHATU_KBN_MSTを見るようにすること
        /// </summary>
        public int KohatuKbn => TenMst.KohatuKbn;

        /// <summary>
        /// 点数
        /// </summary>
        public double Ten => TenMst.Ten;

        /// <summary>
        /// 検査等実施判断グループ区分
        ///     当該診療行為が検査等の場合、判断料・診断料又は判断料･診断料を算定できるグループ区分を表す。
        ///     　0: 1～42以外の診療行為
        ///     (検体検査)
        ///     　　1: 尿・糞便等検査　2: 血液学的検査　3: 生化学的検査（Ⅰ）　4: 生化学的検査（Ⅱ）　5: 免疫学的検査
        ///     (細菌検査)
        ///     　　6: 微生物学的検査
        ///     (検体検査)
        ///     　　8: 基本的検体検査
        ///     (生理検査)
        ///     　　11: 呼吸機能検査　13: 脳波検査　14: 神経・筋検査　15: ラジオアイソトープ検査 16: 眼科学的検査
        ///     (その他検査)
        ///     　　31: 核医学診断（Ｅ１０１－２～Ｅ１０１－５）
        ///     　　32: 核医学診断（それ以外） 33: コンピューター断層診断
        ///     (病理検査)
        ///     　　40: 病理診断 ※
        ///     　　41: 病理診断（組織診断）
        ///     　　42: 病理診断（細胞診断）
        ///     　　※40: 病理診断は41: 病理診断（組織診断）、42: 病理診断（細胞診断）を含む。
        /// </summary>
        public int HandanGrpKbn => TenMst.HandanGrpKbn;

        /// <summary>
        /// 一般名コード
        ///     YJ_CDの頭9桁（例外あり）
        /// </summary>
        public string IpnNameCd => TenMst.IpnNameCd ?? string.Empty;

        /// <summary>
        /// カラム位置１
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol1 => TenMst.CmtCol1;

        /// <summary>
        /// カラム位置２
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol2 => TenMst.CmtCol2;

        /// <summary>
        /// カラム位置３
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol3 => TenMst.CmtCol3;

        /// <summary>
        /// カラム位置４
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol4 => TenMst.CmtCol4;

        /// <summary>
        /// 桁数１
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta1 => TenMst.CmtColKeta1;

        /// <summary>
        /// 桁数２
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta2 => TenMst.CmtColKeta2;

        /// <summary>
        /// 桁数３
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta3 => TenMst.CmtColKeta3;

        /// <summary>
        /// 桁数４
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta4 => TenMst.CmtColKeta4;

        /// <summary>
        /// 上下限年齢下限年齢
        ///     当該診療行為が算定可能な年齢の下限値を表す。
        ///     算定可能な年齢 ≧ 下限年齢
        ///     下限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MinAge => TenMst.MinAge ?? string.Empty;

        /// <summary>
        /// 上下限年齢上限年齢
        ///     当該診療行為が算定可能な年齢の「上限値＋１」を表す。
        ///     算定可能な年齢 ＜ 上限年齢
        ///     上限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MaxAge => TenMst.MaxAge ?? string.Empty;

        /// <summary>
        /// 有効開始年月日
        ///     yyyymmdd
        /// </summary>
        public int StartDate => TenMst.StartDate;

        /// <summary>
        /// 有効終了年月日
        ///     yyyymmdd
        /// </summary>
        public int EndDate => TenMst.EndDate;

        /// <summary>
        /// マスター種別
        ///     S: 診療行為
        ///     Y: 医薬品
        ///     T: 特材
        ///     C: コメント
        ///     R: 労災
        ///     U: 労災特定器材
        ///     D: 労災コメントマスタ
        /// </summary>
        public string MasterSbt => TenMst.MasterSbt ?? string.Empty;

        /// <summary>
        /// 部位区分
        ///     画像診断撮影部位マスターにおいて撮影部位を表す。
        ///     　0: 部位以外
        ///     　1: 頭部
        ///     　2: 躯幹
        ///     　3: 四肢
        ///     　5: 胸部
        ///     　6: 腹部
        ///     　7: 脊椎
        ///     　8: 消化管
        ///      10: 指
        ///      99: その他部位（撮影部位マスターでない場合も含む。）
        /// </summary>
        public int BuiKbn => TenMst.BuiKbn;

        /// <summary>
        /// コード表用区分－区分
        ///     当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。
        ///     　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        ///     　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        public string CdKbn => TenMst.CdKbn ?? string.Empty;

        /// <summary>
        /// コード表用区分－区分番号
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdKbnNo => TenMst.CdKbnno;

        /// <summary>
        /// コード表用区分－区分番号－枝番
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdEdano => TenMst.CdEdano;

        /// <summary>
        /// 告示等識別区分（１）
        ///     当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///     　1: 基本項目（告示）　※基本項目
        ///     　3: 合成項目　　　　　※基本項目
        ///     　5: 準用項目（通知）　※基本項目
        ///     　7: 加算項目　　　　　※加算項目
        ///     　9: 通則加算項目　　　※加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        ///       A: 入院基本料労災乗数項目又は四肢加算（手術）項目
        /// </summary>
        public string Kokuji1 => TenMst.Kokuji1 ?? string.Empty;

        /// <summary>
        /// 告示等識別区分（２）
        ///     当該診療行為について点数表上の取扱いを表す。
        ///     　1: 基本項目（告示）
        ///     　3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///     　7: 加算項目（告示）
        ///     （削）9: 通則加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        /// </summary>
        public string Kokuji2 => TenMst.Kokuji2 ?? string.Empty;

        /// <summary>
        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        /// </summary>
        public int DrugKbn => TenMst.DrugKbn;

        /// <summary>
        /// 請求用名称
        ///     計算後、請求用の名称
        /// </summary>
        public string ReceName => TenMst.ReceName ?? string.Empty;

        /// <summary>
        /// 算定診療行為コード
        ///     算定時の診療行為コード（自己結合でデータ取得）
        /// </summary>
        public string SanteiItemCd => TenMst.SanteiItemCd ?? string.Empty;

        /// <summary>
        /// 自費種別コード
        ///     0: 自費項目以外
        ///     >0: 自費項目
        ///     JIHI_SBT_MST.自費種別
        /// </summary>
        public int JihiSbt => TenMst.JihiSbt;
    }
}
