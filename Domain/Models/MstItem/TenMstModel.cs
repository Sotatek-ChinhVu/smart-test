using Helper.Constants;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class TenMstModel
    {
        public TenMstModel(int hpId, string itemCd, string kokuji1, string kokuji2, int sinKouiKbn, string kouiName, string name, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string odrUnitName, string cnvUnitName, int isNodspRece, int yohoKbn, double odrTermVal, double cnvTermVal, string yjCd, string kensaItemCd, int kensaItemSeqNo, int kohatuKbn, double ten, int handanGrpKbn, string ipnNameCd, int isAdopted, int drugKbn, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, string masterSbt, double defaultValue)
        {
            HpId = hpId;
            ItemCd = itemCd;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            SinKouiKbn = sinKouiKbn;
            KouiName = kouiName;
            Name = name;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
            OdrUnitName = odrUnitName;
            CnvUnitName = cnvUnitName;
            IsNodspRece = isNodspRece;
            YohoKbn = yohoKbn;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            KohatuKbn = kohatuKbn;
            Ten = ten;
            HandanGrpKbn = handanGrpKbn;
            IpnNameCd = ipnNameCd;
            IsAdopted = isAdopted;
            DrugKbn = drugKbn;
            CmtCol1 = cmtCol1;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            MasterSbt = masterSbt;
            DefaultValue = defaultValue;
        }


        /// <summary>
        /// 点数マスタ
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get;private set; }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd { get; private set; }

        public string Kokuji1 { get; private set; }

        public string Kokuji2 { get; private set; }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn { get; private set; }

        public string KouiName { get; private set; }

        /// <summary>
        /// 漢字名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// カナ名称１
        /// </summary>
        public string KanaName1 { get; private set; }

        /// <summary>
        /// カナ名称２
        /// </summary>
        public string KanaName2 { get; private set; }

        /// <summary>
        /// カナ名称３
        /// </summary>
        public string KanaName3 { get; private set; }

        /// <summary>
        /// カナ名称４
        /// </summary>
        public string KanaName4 { get; private set; }

        /// <summary>
        /// カナ名称５
        /// </summary>
        public string KanaName5 { get; private set; }

        /// <summary>
        /// カナ名称６
        /// </summary>
        public string KanaName6 { get; private set; }

        /// <summary>
        /// カナ名称７
        /// </summary>
        public string KanaName7 { get; private set; }

        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        public string OdrUnitName { get; private set; }

        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        public string CnvUnitName { get; private set; }

        /// <summary>
        /// レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（レセプト自体に表示しない）
        /// </summary>
        public int IsNodspRece { get; private set; }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int YohoKbn { get; private set; }

        /// <summary>
        /// オーダー単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位からオーダー単位へ換算するための値を表す。
        public double OdrTermVal { get; private set; }

        /// <summary>
        /// 数量換算単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位から数量換算単位へ換算するための値を表す。
        /// </summary>
        public double CnvTermVal { get; private set; }

        /// <summary>
        /// 個別医薬品コード
        ///     薬価基準収載医薬品コードと同様に英数12桁のコードですが、統一名収載品目の個々の商品に対して別々のコードが付与されます。
        ///     銘柄別収載品目（商品名で官報に収載されるもの）については、薬価基準収載医薬品コードと同じコードです。
        /// </summary>
        public string YjCd { get; private set; }

        /// <summary>
        /// 検査項目コード
        ///     KENSA_MST.KENSA_ITEM_CD
        /// </summary>
        public string KensaItemCd { get; private set; }

        /// <summary>
        /// 検査項目コード連番
        ///     KENSA_MST.SEQ_NO
        /// </summary>
        public int KensaItemSeqNo { get; private set; }

        /// <summary>
        /// 後発医薬品区分
        ///     当該医薬品が後発医薬品に該当するか否かを表す。
        ///     　0: 後発医薬品でない
        ///     　1: 先発医薬品がある後発医薬品である
        ///     ※基金マスタの設定、オーダー時はKOHATU_KBN_MSTを見るようにすること
        /// </summary>
        public int KohatuKbn { get; private set; }

        /// <summary>
        /// 点数
        /// </summary>
        public double Ten { get; private set; }

        public int HandanGrpKbn { get; private set; }

        public string IpnNameCd { get; private set; }

        public int IsAdopted { get; private set; }

        public bool IsYoho
        {
            get => YohoKbn == 1 || YohoKbn == 2;
        }

        public int DrugKbn { get; private set; }

        public bool IsDrugUsage
        {
            get => (new[] { 21, 22, 23 }).Contains(SinKouiKbn);
        }
        public bool IsUsageInjection
        {
            get => (new[] { 28, 31, 32, 33, 34 }).Contains(SinKouiKbn);//AIN-4152 SinKoui=28 same as InjectionUsage
        }
        public int CmtCol1 { get; private set; }

        public int CmtCol2 { get; private set; }

        public int CmtCol3 { get; private set; }

        public int CmtCol4 { get; private set; }

        public int CmtColKeta1 { get; private set; }

        public int CmtColKeta2 { get; private set; }

        public int CmtColKeta3 { get; private set; }

        public int CmtColKeta4 { get; private set; }

        public bool Is840Cmt { get => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon; }

        public bool Is820Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment820Pattern);

        public bool Is830Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

        public bool Is831Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

        public bool Is842Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

        public bool Is850Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

        public bool Is851Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

        public bool Is852Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

        public bool Is853Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

        public bool Is880Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

        public bool IsTokuzai
        {
            get =>  MasterSbt.AsString().ToUpper() == "T";
        }

        public string MasterSbt { get; private set; }

        public double DefaultValue { get; private set; }
        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(Name);
        }
    }
}
