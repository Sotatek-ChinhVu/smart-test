using Helper.Common;

namespace Reporting.Karte3.Model
{
    class CoKarte3Model
    {
        public CoKarte3Model(CoPtInfModel ptInf, CoPtHokenInfModel ptHokenInf, List<CoSinKouiModel> sinKouis, List<CoKaikeiInfModel> kaikeiInfs, HashSet<string> houbetuNos)
        {
            PtInf = ptInf;
            PtHokenInf = ptHokenInf;
            SinKouis = sinKouis;
            KaikeiInfs = kaikeiInfs;
            HoubetuNos = houbetuNos;
        }

        /// <summary>
        /// 患者情報
        /// </summary>
        CoPtInfModel PtInf { get; set; }
        /// <summary>
        /// 患者保険情報
        /// </summary>
        CoPtHokenInfModel PtHokenInf { get; set; }
        /// <summary>
        /// 診療行為
        /// </summary>
        public List<CoSinKouiModel> SinKouis { get; set; }
        /// <summary>
        /// 会計情報
        /// </summary>
        public List<CoKaikeiInfModel> KaikeiInfs { get; set; }
        /// <summary>
        /// 公費情報
        /// </summary>
        public HashSet<string> HoubetuNos { get; set; }
        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf.PtNum;
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name;
        }
        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInf.Birthday;
        }
        public string BirthDayW
        {
            get
            {
                string ret = string.Empty;

                if (BirthDay > 0)
                {
                    ret = CIUtil.SDateToShowWDate3(BirthDay).Ymd;
                }

                return ret;
            }
        }
        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get => PtInf.Sex;
        }
        /// <summary>
        /// 性別（男・女）
        /// </summary>
        public string PtSex
        {
            get
            {
                string ret = "男";

                if (Sex == 2)
                {
                    ret = "女";
                }

                return ret;
            }
        }

        public int GetTotalTensu(int sinDate)
        {
            int ret = 0;

            List<CoKaikeiInfModel> kaikeiInf = KaikeiInfs.FindAll(p => p.SinDate == sinDate);

            if (kaikeiInf.Any())
            {
                ret = kaikeiInf.First().Tensu;
            }

            return ret;
        }

        public int GetTotalPtFutan(int sinDate)
        {
            int ret = 0;

            List<CoKaikeiInfModel> kaikeiInf = KaikeiInfs.FindAll(p => p.SinDate == sinDate);

            if (kaikeiInf.Any())
            {
                ret = kaikeiInf.First().PtFutan;
            }

            return ret;
        }
        /// <summary>
        /// 保険の種類
        /// </summary>
        public string HokenSyu
        {
            get
            {
                #region sub method
                string _getHeiyo()
                {
                    string heiyo = string.Empty;
                    switch (HoubetuNos.Count)
                    {
                        case 0:
                            heiyo += "単独";
                            break;
                        case 1:
                            heiyo = "２者併用";
                            break;
                        case 2:
                            heiyo = "３者併用";
                            break;
                        case 3:
                            heiyo = "４者併用";
                            break;
                        case 4:
                            heiyo = "５者併用";
                            break;
                    }
                    return heiyo;
                }

                string _getKohiHeiyo()
                {
                    string heiyo = string.Empty;
                    switch (HoubetuNos.Count)
                    {
                        case 1:
                            heiyo = "公費単独";
                            break;
                        case 2:
                            heiyo = "公費併用";
                            break;
                        case 3:
                            heiyo = "公費３者併用";
                            break;
                        case 4:
                            heiyo = "公費４者併用";
                            break;
                    }
                    return heiyo;
                }

                string _getHonke()
                {
                    string honke = "・本人";
                    if (PtHokenInf.HonkeKbn == 2)
                    {
                        honke = "・家族";
                    }
                    return honke;
                }
                #endregion

                string ret = string.Empty;

                if (PtHokenInf != null)
                {
                    if (PtHokenInf.HokenSbtKbn == 0)
                    {
                        ret = _getKohiHeiyo();
                    }
                    else if (PtHokenInf.HokenSbtKbn == 8)
                    {
                        ret = "自費";
                    }
                    else if (PtHokenInf.HokenSbtKbn == 9)
                    {
                        ret = "自レ";
                    }
                    else if (new int[] { 11, 12, 13 }.Contains(PtHokenInf.HokenKbn))
                    {
                        ret = "労災";
                    }
                    else if (PtHokenInf.HokenKbn == 14)
                    {
                        ret = "自賠";
                    }
                    else
                    {
                        string honke = string.Empty;

                        if (PtHokenInf.HokenKbn == 1)
                        {
                            //if (CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 70))
                            //{
                            //    ret = "高齢";
                            //}
                            //else
                            //{
                            ret = "社保";
                            honke = _getHonke();
                            //}
                            ret += _getHeiyo();
                        }
                        else if (PtHokenInf.HokenKbn == 2)
                        {
                            if (PtHokenInf.Houbetu == "39")
                            {
                                ret = "後期";
                            }
                            //else if (CIUtil.AgeChk(PtInf.Birthday, KaikeiInf.SinDate, 70))
                            //{
                            //    ret = "高齢";
                            //}
                            else if (PtHokenInf.Houbetu == "67")
                            {
                                ret = "退職";
                                honke = _getHonke();
                            }
                            else
                            {
                                ret = "国保";
                                honke = _getHonke();
                            }

                            ret += _getHeiyo();
                        }

                        ret += honke;
                    }
                }
                return ret;
            }
        }
    }
}
