using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class ReceInfEditModel
    {
        public ReceInfEdit ReceInfEdit { get; }

        public ReceInfEditModel(ReceInfEdit receInfEdit)
        {
            ReceInfEdit = receInfEdit;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceInfEdit.HpId; }
            set
            {
                if (ReceInfEdit.HpId == value) return;
                ReceInfEdit.HpId = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceInfEdit.SeikyuYm; }
            set
            {
                if (ReceInfEdit.SeikyuYm == value) return;
                ReceInfEdit.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceInfEdit.PtId; }
            set
            {
                if (ReceInfEdit.PtId == value) return;
                ReceInfEdit.PtId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceInfEdit.SinYm; }
            set
            {
                if (ReceInfEdit.SinYm == value) return;
                ReceInfEdit.SinYm = value;
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceInfEdit.HokenId; }
            set
            {
                if (ReceInfEdit.HokenId == value) return;
                ReceInfEdit.HokenId = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return ReceInfEdit.SeqNo; }
            set
            {
                if (ReceInfEdit.SeqNo == value) return;
                ReceInfEdit.SeqNo = value;
            }
        }

        /// <summary>
        /// レセプト種別
        /// 11x2: 本人
        ///                     11x4: 未就学者          
        ///                     11x6: 家族          
        ///                     11x8: 高齢一般・低所          
        ///                     11x0: 高齢７割          
        ///                     12x2: 公費          
        ///                     13x8: 後期一般・低所          
        ///                     13x0: 後期７割          
        ///                     14x2: 退職本人          
        ///                     14x4: 退職未就学者          
        ///                     14x6: 退職家族          
        /// </summary>
        public string ReceSbt
        {
            get { return ReceInfEdit.ReceSbt; }
            set
            {
                if (ReceInfEdit.ReceSbt == value) return;
                ReceInfEdit.ReceSbt = value;
            }
        }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        public string Houbetu
        {
            get { return ReceInfEdit.Houbetu; }
            set
            {
                if (ReceInfEdit.Houbetu == value) return;
                ReceInfEdit.Houbetu = value;
            }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return ReceInfEdit.Kohi1Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfEdit.Kohi1Houbetu == value) return;
                ReceInfEdit.Kohi1Houbetu = value;
            }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return ReceInfEdit.Kohi2Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfEdit.Kohi2Houbetu == value) return;
                ReceInfEdit.Kohi2Houbetu = value;
            }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return ReceInfEdit.Kohi3Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfEdit.Kohi3Houbetu == value) return;
                ReceInfEdit.Kohi3Houbetu = value;
            }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return ReceInfEdit.Kohi4Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfEdit.Kohi4Houbetu == value) return;
                ReceInfEdit.Kohi4Houbetu = value;
            }
        }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        public int? HokenReceTensu
        {
            get { return ReceInfEdit.HokenReceTensu; }
            set
            {
                if (ReceInfEdit.HokenReceTensu == value) return;
                ReceInfEdit.HokenReceTensu = value;
            }
        }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        public int? HokenReceFutan
        {
            get { return ReceInfEdit.HokenReceFutan; }
            set
            {
                if (ReceInfEdit.HokenReceFutan == value) return;
                ReceInfEdit.HokenReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        public int? Kohi1ReceTensu
        {
            get { return ReceInfEdit.Kohi1ReceTensu; }
            set
            {
                if (ReceInfEdit.Kohi1ReceTensu == value) return;
                ReceInfEdit.Kohi1ReceTensu = value;
            }
        }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        public int? Kohi1ReceFutan
        {
            get { return ReceInfEdit.Kohi1ReceFutan; }
            set
            {
                if (ReceInfEdit.Kohi1ReceFutan == value) return;
                ReceInfEdit.Kohi1ReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi1ReceKyufu
        {
            get { return ReceInfEdit.Kohi1ReceKyufu; }
            set
            {
                if (ReceInfEdit.Kohi1ReceKyufu == value) return;
                ReceInfEdit.Kohi1ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        public int? Kohi2ReceTensu
        {
            get { return ReceInfEdit.Kohi2ReceTensu; }
            set
            {
                if (ReceInfEdit.Kohi2ReceTensu == value) return;
                ReceInfEdit.Kohi2ReceTensu = value;
            }
        }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        public int? Kohi2ReceFutan
        {
            get { return ReceInfEdit.Kohi2ReceFutan; }
            set
            {
                if (ReceInfEdit.Kohi2ReceFutan == value) return;
                ReceInfEdit.Kohi2ReceFutan = value;
            }
        }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi2ReceKyufu
        {
            get { return ReceInfEdit.Kohi2ReceKyufu; }
            set
            {
                if (ReceInfEdit.Kohi2ReceKyufu == value) return;
                ReceInfEdit.Kohi2ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        public int? Kohi3ReceTensu
        {
            get { return ReceInfEdit.Kohi3ReceTensu; }
            set
            {
                if (ReceInfEdit.Kohi3ReceTensu == value) return;
                ReceInfEdit.Kohi3ReceTensu = value;
            }
        }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        public int? Kohi3ReceFutan
        {
            get { return ReceInfEdit.Kohi3ReceFutan; }
            set
            {
                if (ReceInfEdit.Kohi3ReceFutan == value) return;
                ReceInfEdit.Kohi3ReceFutan = value;
            }
        }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi3ReceKyufu
        {
            get { return ReceInfEdit.Kohi3ReceKyufu; }
            set
            {
                if (ReceInfEdit.Kohi3ReceKyufu == value) return;
                ReceInfEdit.Kohi3ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        public int? Kohi4ReceTensu
        {
            get { return ReceInfEdit.Kohi4ReceTensu; }
            set
            {
                if (ReceInfEdit.Kohi4ReceTensu == value) return;
                ReceInfEdit.Kohi4ReceTensu = value;
            }
        }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        public int? Kohi4ReceFutan
        {
            get { return ReceInfEdit.Kohi4ReceFutan; }
            set
            {
                if (ReceInfEdit.Kohi4ReceFutan == value) return;
                ReceInfEdit.Kohi4ReceFutan = value;
            }
        }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi4ReceKyufu
        {
            get { return ReceInfEdit.Kohi4ReceKyufu; }
            set
            {
                if (ReceInfEdit.Kohi4ReceKyufu == value) return;
                ReceInfEdit.Kohi4ReceKyufu = value;
            }
        }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        public int? HokenNissu
        {
            get { return ReceInfEdit.HokenNissu; }
            set
            {
                if (ReceInfEdit.HokenNissu == value) return;
                ReceInfEdit.HokenNissu = value;
            }
        }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        public int? Kohi1Nissu
        {
            get { return ReceInfEdit.Kohi1Nissu; }
            set
            {
                if (ReceInfEdit.Kohi1Nissu == value) return;
                ReceInfEdit.Kohi1Nissu = value;
            }
        }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        public int? Kohi2Nissu
        {
            get { return ReceInfEdit.Kohi2Nissu; }
            set
            {
                if (ReceInfEdit.Kohi2Nissu == value) return;
                ReceInfEdit.Kohi2Nissu = value;
            }
        }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        public int? Kohi3Nissu
        {
            get { return ReceInfEdit.Kohi3Nissu; }
            set
            {
                if (ReceInfEdit.Kohi3Nissu == value) return;
                ReceInfEdit.Kohi3Nissu = value;
            }
        }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        public int? Kohi4Nissu
        {
            get { return ReceInfEdit.Kohi4Nissu; }
            set
            {
                if (ReceInfEdit.Kohi4Nissu == value) return;
                ReceInfEdit.Kohi4Nissu = value;
            }
        }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        public string Tokki
        {
            get { return ReceInfEdit.Tokki; }
            set
            {
                if (ReceInfEdit.Tokki == value) return;
                ReceInfEdit.Tokki = value;
            }
        }

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        public string Tokki1
        {
            get { return ReceInfEdit.Tokki1; }
            set
            {
                if (ReceInfEdit.Tokki1 == value) return;
                ReceInfEdit.Tokki1 = value;
            }
        }

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        public string Tokki2
        {
            get { return ReceInfEdit.Tokki2; }
            set
            {
                if (ReceInfEdit.Tokki2 == value) return;
                ReceInfEdit.Tokki2 = value;
            }
        }

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        public string Tokki3
        {
            get { return ReceInfEdit.Tokki3; }
            set
            {
                if (ReceInfEdit.Tokki3 == value) return;
                ReceInfEdit.Tokki3 = value;
            }
        }

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        public string Tokki4
        {
            get { return ReceInfEdit.Tokki4; }
            set
            {
                if (ReceInfEdit.Tokki4 == value) return;
                ReceInfEdit.Tokki4 = value;
            }
        }

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        public string Tokki5
        {
            get { return ReceInfEdit.Tokki5; }
            set
            {
                if (ReceInfEdit.Tokki5 == value) return;
                ReceInfEdit.Tokki5 = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return ReceInfEdit.IsDeleted; }
            set
            {
                if (ReceInfEdit.IsDeleted == value) return;
                ReceInfEdit.IsDeleted = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceInfEdit.CreateDate; }
            set
            {
                if (ReceInfEdit.CreateDate == value) return;
                ReceInfEdit.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceInfEdit.CreateId; }
            set
            {
                if (ReceInfEdit.CreateId == value) return;
                ReceInfEdit.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceInfEdit.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceInfEdit.CreateMachine == value) return;
                ReceInfEdit.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return ReceInfEdit.UpdateDate; }
            set
            {
                if (ReceInfEdit.UpdateDate == value) return;
                ReceInfEdit.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return ReceInfEdit.UpdateId; }
            set
            {
                if (ReceInfEdit.UpdateId == value) return;
                ReceInfEdit.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return ReceInfEdit.UpdateMachine ?? string.Empty; }
            set
            {
                if (ReceInfEdit.UpdateMachine == value) return;
                ReceInfEdit.UpdateMachine = value;
            }
        }

    }

}
