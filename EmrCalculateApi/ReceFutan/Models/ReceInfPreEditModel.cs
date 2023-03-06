using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class ReceInfPreEditModel
    {
        public ReceInfPreEdit ReceInfPreEdit { get; }

        public ReceInfPreEditModel(ReceInfPreEdit receInfPreEdit)
        {
            ReceInfPreEdit = receInfPreEdit;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceInfPreEdit.HpId; }
            set
            {
                if (ReceInfPreEdit.HpId == value) return;
                ReceInfPreEdit.HpId = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceInfPreEdit.SeikyuYm; }
            set
            {
                if (ReceInfPreEdit.SeikyuYm == value) return;
                ReceInfPreEdit.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceInfPreEdit.PtId; }
            set
            {
                if (ReceInfPreEdit.PtId == value) return;
                ReceInfPreEdit.PtId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceInfPreEdit.SinYm; }
            set
            {
                if (ReceInfPreEdit.SinYm == value) return;
                ReceInfPreEdit.SinYm = value;
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceInfPreEdit.HokenId; }
            set
            {
                if (ReceInfPreEdit.HokenId == value) return;
                ReceInfPreEdit.HokenId = value;
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
            get { return ReceInfPreEdit.ReceSbt; }
            set
            {
                if (ReceInfPreEdit.ReceSbt == value) return;
                ReceInfPreEdit.ReceSbt = value;
            }
        }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        public string Houbetu
        {
            get { return ReceInfPreEdit.Houbetu; }
            set
            {
                if (ReceInfPreEdit.Houbetu == value) return;
                ReceInfPreEdit.Houbetu = value;
            }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return ReceInfPreEdit.Kohi1Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfPreEdit.Kohi1Houbetu == value) return;
                ReceInfPreEdit.Kohi1Houbetu = value;
            }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return ReceInfPreEdit.Kohi2Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfPreEdit.Kohi2Houbetu == value) return;
                ReceInfPreEdit.Kohi2Houbetu = value;
            }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return ReceInfPreEdit.Kohi3Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfPreEdit.Kohi3Houbetu == value) return;
                ReceInfPreEdit.Kohi3Houbetu = value;
            }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return ReceInfPreEdit.Kohi4Houbetu ?? string.Empty; }
            set
            {
                if (ReceInfPreEdit.Kohi4Houbetu == value) return;
                ReceInfPreEdit.Kohi4Houbetu = value;
            }
        }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        public int? HokenReceTensu
        {
            get { return ReceInfPreEdit.HokenReceTensu; }
            set
            {
                if (ReceInfPreEdit.HokenReceTensu == value) return;
                ReceInfPreEdit.HokenReceTensu = value;
            }
        }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        public int? HokenReceFutan
        {
            get { return ReceInfPreEdit.HokenReceFutan; }
            set
            {
                if (ReceInfPreEdit.HokenReceFutan == value) return;
                ReceInfPreEdit.HokenReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        public int? Kohi1ReceTensu
        {
            get { return ReceInfPreEdit.Kohi1ReceTensu; }
            set
            {
                if (ReceInfPreEdit.Kohi1ReceTensu == value) return;
                ReceInfPreEdit.Kohi1ReceTensu = value;
            }
        }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        public int? Kohi1ReceFutan
        {
            get { return ReceInfPreEdit.Kohi1ReceFutan; }
            set
            {
                if (ReceInfPreEdit.Kohi1ReceFutan == value) return;
                ReceInfPreEdit.Kohi1ReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi1ReceKyufu
        {
            get { return ReceInfPreEdit.Kohi1ReceKyufu; }
            set
            {
                if (ReceInfPreEdit.Kohi1ReceKyufu == value) return;
                ReceInfPreEdit.Kohi1ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        public int? Kohi2ReceTensu
        {
            get { return ReceInfPreEdit.Kohi2ReceTensu; }
            set
            {
                if (ReceInfPreEdit.Kohi2ReceTensu == value) return;
                ReceInfPreEdit.Kohi2ReceTensu = value;
            }
        }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        public int? Kohi2ReceFutan
        {
            get { return ReceInfPreEdit.Kohi2ReceFutan; }
            set
            {
                if (ReceInfPreEdit.Kohi2ReceFutan == value) return;
                ReceInfPreEdit.Kohi2ReceFutan = value;
            }
        }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi2ReceKyufu
        {
            get { return ReceInfPreEdit.Kohi2ReceKyufu; }
            set
            {
                if (ReceInfPreEdit.Kohi2ReceKyufu == value) return;
                ReceInfPreEdit.Kohi2ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        public int? Kohi3ReceTensu
        {
            get { return ReceInfPreEdit.Kohi3ReceTensu; }
            set
            {
                if (ReceInfPreEdit.Kohi3ReceTensu == value) return;
                ReceInfPreEdit.Kohi3ReceTensu = value;
            }
        }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        public int? Kohi3ReceFutan
        {
            get { return ReceInfPreEdit.Kohi3ReceFutan; }
            set
            {
                if (ReceInfPreEdit.Kohi3ReceFutan == value) return;
                ReceInfPreEdit.Kohi3ReceFutan = value;
            }
        }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi3ReceKyufu
        {
            get { return ReceInfPreEdit.Kohi3ReceKyufu; }
            set
            {
                if (ReceInfPreEdit.Kohi3ReceKyufu == value) return;
                ReceInfPreEdit.Kohi3ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        public int? Kohi4ReceTensu
        {
            get { return ReceInfPreEdit.Kohi4ReceTensu; }
            set
            {
                if (ReceInfPreEdit.Kohi4ReceTensu == value) return;
                ReceInfPreEdit.Kohi4ReceTensu = value;
            }
        }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        public int? Kohi4ReceFutan
        {
            get { return ReceInfPreEdit.Kohi4ReceFutan; }
            set
            {
                if (ReceInfPreEdit.Kohi4ReceFutan == value) return;
                ReceInfPreEdit.Kohi4ReceFutan = value;
            }
        }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi4ReceKyufu
        {
            get { return ReceInfPreEdit.Kohi4ReceKyufu; }
            set
            {
                if (ReceInfPreEdit.Kohi4ReceKyufu == value) return;
                ReceInfPreEdit.Kohi4ReceKyufu = value;
            }
        }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        public int? HokenNissu
        {
            get { return ReceInfPreEdit.HokenNissu; }
            set
            {
                if (ReceInfPreEdit.HokenNissu == value) return;
                ReceInfPreEdit.HokenNissu = value;
            }
        }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        public int? Kohi1Nissu
        {
            get { return ReceInfPreEdit.Kohi1Nissu; }
            set
            {
                if (ReceInfPreEdit.Kohi1Nissu == value) return;
                ReceInfPreEdit.Kohi1Nissu = value;
            }
        }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        public int? Kohi2Nissu
        {
            get { return ReceInfPreEdit.Kohi2Nissu; }
            set
            {
                if (ReceInfPreEdit.Kohi2Nissu == value) return;
                ReceInfPreEdit.Kohi2Nissu = value;
            }
        }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        public int? Kohi3Nissu
        {
            get { return ReceInfPreEdit.Kohi3Nissu; }
            set
            {
                if (ReceInfPreEdit.Kohi3Nissu == value) return;
                ReceInfPreEdit.Kohi3Nissu = value;
            }
        }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        public int? Kohi4Nissu
        {
            get { return ReceInfPreEdit.Kohi4Nissu; }
            set
            {
                if (ReceInfPreEdit.Kohi4Nissu == value) return;
                ReceInfPreEdit.Kohi4Nissu = value;
            }
        }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        public string Tokki
        {
            get { return ReceInfPreEdit.Tokki; }
            set
            {
                if (ReceInfPreEdit.Tokki == value) return;
                ReceInfPreEdit.Tokki = value;
            }
        }

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        public string Tokki1
        {
            get { return ReceInfPreEdit.Tokki1; }
            set
            {
                if (ReceInfPreEdit.Tokki1 == value) return;
                ReceInfPreEdit.Tokki1 = value;
            }
        }

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        public string Tokki2
        {
            get { return ReceInfPreEdit.Tokki2; }
            set
            {
                if (ReceInfPreEdit.Tokki2 == value) return;
                ReceInfPreEdit.Tokki2 = value;
            }
        }

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        public string Tokki3
        {
            get { return ReceInfPreEdit.Tokki3; }
            set
            {
                if (ReceInfPreEdit.Tokki3 == value) return;
                ReceInfPreEdit.Tokki3 = value;
            }
        }

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        public string Tokki4
        {
            get { return ReceInfPreEdit.Tokki4; }
            set
            {
                if (ReceInfPreEdit.Tokki4 == value) return;
                ReceInfPreEdit.Tokki4 = value;
            }
        }

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        public string Tokki5
        {
            get { return ReceInfPreEdit.Tokki5; }
            set
            {
                if (ReceInfPreEdit.Tokki5 == value) return;
                ReceInfPreEdit.Tokki5 = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceInfPreEdit.CreateDate; }
            set
            {
                if (ReceInfPreEdit.CreateDate == value) return;
                ReceInfPreEdit.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceInfPreEdit.CreateId; }
            set
            {
                if (ReceInfPreEdit.CreateId == value) return;
                ReceInfPreEdit.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceInfPreEdit.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceInfPreEdit.CreateMachine == value) return;
                ReceInfPreEdit.CreateMachine = value;
            }
        }

    }

}
