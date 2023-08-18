using Helper.Common;
using Helper.Constants;
using Reporting.Calculate.Receipt.ViewModels;

namespace Reporting.AccountingCard.Model
{
    public class CoAccountingCardModel
    {
        public List<CoKaikeiInfModel>? KaikeiInfModels { get; } = null;
        public CoPtInfModel? PtInfModel { get; } = null;
        public SinMeiViewModel? SinMeiViewModel { get; } = null;
        public List<CoPtByomeiModel>? PtByomeiModels { get; } = null;
        public List<CoPtKohiModel>? PtKohis { get; } = null;
        public CoAccountingCardModel(
            List<CoKaikeiInfModel> kaikeiInfModels,
            CoPtInfModel ptInfModel, SinMeiViewModel sinMeiViewModel,
            List<CoPtByomeiModel> ptByomeiModels)
        {
            KaikeiInfModels = kaikeiInfModels;
            PtInfModel = ptInfModel;
            SinMeiViewModel = sinMeiViewModel;
            PtByomeiModels = ptByomeiModels;

            PtKohis = new List<CoPtKohiModel>();

            HashSet<int> kohiIds = new HashSet<int>();

            foreach (CoKaikeiInfModel kaikeiInf in kaikeiInfModels)
            {
                foreach (CoPtKohiModel ptKohi in kaikeiInf.PtKohis)
                {
                    int count = kohiIds.Count();

                    kohiIds.Add(ptKohi.HokenId);

                    if (kohiIds.Count() > count)
                    {
                        PtKohis.Add(ptKohi);
                    }
                }
            }

            // 優先順に並べ替えておく
            PtKohis = PtKohis.OrderBy(p => p.SortKey).ToList();
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInfModel?.PtNum ?? 0;
        }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => PtInfModel?.PtId ?? 0;
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInfModel?.Name ?? string.Empty;
        }
        /// <summary>
        /// 患者カナ氏名
        /// </summary>
        public string PtKanaName
        {
            get => PtInfModel?.KanaName ?? string.Empty;
        }

        /// <summary>
        /// 患者性別
        /// </summary>
        public string PtSex
        {
            get
            {
                string ret = "男";

                if (PtInfModel?.Sex == 2)
                {
                    ret = "女";
                }

                return ret;
            }
        }

        /// <summary>
        /// 患者住所
        /// </summary>
        public string PtAddress
        {
            get => PtInfModel?.HomeAddress1 ?? string.Empty + PtInfModel?.HomeAddress2 ?? string.Empty;
        }

        public string PtAddress1
        {
            get => PtInfModel?.HomeAddress1 ?? string.Empty;
        }

        public string PtAddress2
        {
            get => PtInfModel?.HomeAddress2 ?? string.Empty;
        }

        /// <summary>
        /// 患者郵便番号
        /// </summary>
        public string PtPostCd
        {
            get => PtInfModel?.HomePost ?? string.Empty;
        }

        /// <summary>
        /// 患者生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInfModel?.Birthday ?? 0;
        }

        public int Age
        {
            get => PtInfModel?.Age ?? 0;
        }
        /// <summary>
        /// 保険の種類
        /// </summary>
        public string HokenSbt
        {
            get => KaikeiInfModels?.First()?.HokenSyu ?? string.Empty;
        }

        /// <summary>
        /// 保険の種類（１つでも異なる保険があれば空文字を返す）
        /// </summary>
        public string HokenSbtAll
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null)
                {
                    foreach (CoKaikeiInfModel kaikeiInfModel in KaikeiInfModels)
                    {
                        if (ret == "")
                        {
                            ret = kaikeiInfModel.HokenSyu;
                        }
                        else if (ret != kaikeiInfModel.HokenSyu)
                        {
                            ret = "";
                            break;
                        }
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 負担率
        /// </summary>
        public int? FutanRate
        {
            //get => KaikeiInfModels.First().FutanRate;
            get
            {
                int? ret = null;

                if (new int[] { 1, 2 }.Contains(HokenKbn))
                {

                    ret = GetHokenRate(KaikeiInfModels?.First().Rate ?? 0, KaikeiInfModels?.First()?.HokenSbtKbn ?? 0, KaikeiInfModels?.First()?.KogakuKbn ?? 0, KaikeiInfModels?.First()?.Houbetu ?? string.Empty);

                }
                else if (HokenKbn == 0)
                {
                    // 自費
                    ret = 100;
                }
                else
                {
                    // 労災・自賠
                    //ret = null;
                }


                if (PtKohis != null)
                {
                    for (int i = 0; i < PtKohis.Count(); i++)
                    {
                        if (PtKohis[i].HokenMst.FutanKbn == 0)
                        {
                            ret = 0;
                        }
                        else if (ret == null || ret > PtKohis[i].FutanRate)
                        {
                            ret = PtKohis[i].FutanRate;
                        }
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 主保険負担率計算
        /// </summary>
        /// <param name="futanRate">負担率</param>
        /// <param name="hokenSbtKbn">保険種別区分</param>
        /// <param name="kogakuKbn">高額療養費区分</param>
        /// <param name="houbetu">法別番号</param>
        /// <returns></returns>
        private int GetHokenRate(int futanRate, int hokenSbtKbn, int kogakuKbn, string houbetu)
        {
            int wrkRate = futanRate;

            switch (hokenSbtKbn)
            {
                case 0:
                    //主保険なし
                    break;
                case 1:
                    //主保険
                    if (IsPreSchool())
                    {
                        //６歳未満未就学児
                        wrkRate = 20;
                    }
                    else if (IsElder() && houbetu != "39")
                    {
                        wrkRate =
                            IsElder20per() ? wrkRate = 20 :  //前期高齢
                            IsElderExpat() ? wrkRate = 20 :  //75歳以上海外居住者
                            wrkRate = 10;
                    }

                    if (IsElder() || houbetu == "39")
                    {
                        if ((kogakuKbn == 3 && KaikeiInfModels?.Last()?.SinDate < KaiseiDate.d20180801) ||
                            (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && KaikeiInfModels?.Last()?.SinDate >= KaiseiDate.d20180801))
                        {
                            //後期７割 or 高齢７割
                            wrkRate = 30;
                        }
                        else if (houbetu == "39" && kogakuKbn == 41 &&
                            KaikeiInfModels?.Last()?.SinDate >= KaiseiDate.d20221001)
                        {
                            //後期８割
                            wrkRate = 20;
                        }
                    }
                    break;
                default:
                    break;
            }

            return wrkRate;
        }
        /// <summary>
        /// 未就学かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsPreSchool()
        {
            return !CIUtil.IsStudent(PtInfModel?.Birthday ?? 0, KaikeiInfModels?.Last()?.SinDate ?? 0);
        }

        /// <summary>
        /// 70歳以上かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsElder()
        {
            return CIUtil.AgeChk(PtInfModel?.Birthday ?? 0, KaikeiInfModels?.Last()?.SinDate ?? 0, 70);
        }
        /// <summary>
        /// 前期高齢2割かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsElder20per()
        {
            return CIUtil.Is70Zenki_20per(PtInfModel?.Birthday ?? 0, KaikeiInfModels?.Last()?.SinDate ?? 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsElderExpat()
        {
            //75歳以上で海外居住者の方は後期高齢者医療には加入せず、
            //協会、健保組合に加入することになり、高齢受給者証を提示した場合、
            //H26.5診療分からは所得に合わせ2割または3割負担となる。
            return CIUtil.AgeChk(PtInfModel?.Birthday ?? 0, KaikeiInfModels?.Last()?.SinDate ?? 0, 75) && KaikeiInfModels?.Last()?.SinDate >= 20140501;
        }

        /// <summary>
        /// 主保険負担率
        /// </summary>
        public int? HokenRate
        {
            get => KaikeiInfModels != null ? KaikeiInfModels?.FirstOrDefault()?.HokenRate ?? 0 : 0;
        }

        /// <summary>
        /// 実日数
        /// </summary>
        public int Nissu
        {
            get
            {
                int ret = 0;

                var groupsums = (
                    from kaikeiInf in KaikeiInfModels
                    group kaikeiInf by kaikeiInf.SinDate into A
                    select new { A.Key, sum = A.Sum(a => (int)a.Nissu) }
                    ).ToList();

                foreach (var groupsum in groupsums)
                {
                    if (groupsum.sum > 0)
                    {
                        ret++;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 診療点数
        /// </summary>
        public int SinryoTensu
        {
            get
            {
                int ret = 0;

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.Sum(p => p.Tensu);
                }

                return ret;
            }
        }

        /// <summary>
        /// 患者負担額
        /// </summary>
        public int PtFutan
        {
            get
            {
                int ret = 0;

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.Sum(p => p.PtFutan);
                }

                return ret;
            }
        }

        /// <summary>
        /// 保険区分
        ///		0:自費 
        ///		1:社保 
        ///		2:国保
        ///		
        ///		11:労災(短期給付)
        ///		12:労災(傷病年金)
        ///		13:アフターケア
        ///		14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get
            {
                int ret = 0;

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().HokenKbn;
                }

                return ret;
            }
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().HokensyaNo;
                }

                return ret;
            }
        }
        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().Kigo;
                }

                return ret;
            }
        }
        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().Bango;
                }

                return ret;
            }
        }
        public string EdaNo
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().EdaNo;
                }

                return ret;
            }
        }
        /// <summary>
        /// 記号番号
        /// </summary>
        public string KigoBango
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().KigoBango;
                }

                return ret;
            }
        }
        /// <summary>
        /// 労災の交付番号
        /// </summary>
        public string RousaiKofuNo
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().RousaiKofuNo;
                }

                return ret;
            }
        }
        /// <summary>
        /// 労災事業所名
        /// </summary>
        public string RousaiJigyosyoName
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().RousaiJigyosyoName;
                }

                return ret;

            }
        }
        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().JibaiHokenName;
                }

                return ret;
            }
        }
        /// <summary>
        /// 自賠担当者
        /// </summary>
        public string JibaiTanto
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().JibaiTanto;
                }

                return ret;
            }
        }
        /// <summary>
        /// 本人家族区分
        /// </summary>
        public string HonKe
        {
            get
            {
                string ret = "";

                if (KaikeiInfModels != null && KaikeiInfModels.Any())
                {
                    ret = KaikeiInfModels.First().Honke;
                }

                return ret;
            }
        }
        /// <summary>
        /// 公費負担者番号
        /// </summary>
        public string KohiFutansyaNo(int index)
        {
            string ret = "";

            //if (KaikeiInfModels != null && KaikeiInfModels.Any())
            //{
            //    ret = KaikeiInfModels.First().KohiFutansyaNo(index);
            //}
            if (PtKohis != null && index >= 0 && index < PtKohis.Count())
            {
                ret = PtKohis[index].FutansyaNo;
            }

            return ret;
        }
        /// <summary>
        /// 公費受給者番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiJyukyusyaNo(int index)
        {
            string ret = "";

            //if (KaikeiInfModels != null && KaikeiInfModels.Any())
            //{
            //    ret = KaikeiInfModels.First().KohiJyukyusyaNo(index);
            //}
            if (PtKohis != null && index >= 0 && index < PtKohis.Count())
            {
                ret = PtKohis[index].JyukyusyaNo;
            }

            return ret;
        }

    }
}
