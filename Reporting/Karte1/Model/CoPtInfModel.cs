using Entity.Tenant;
using Helper.Common;

namespace Reporting.Karte1.Model
{
    public class CoPtInfModel
    {
        public PtInf PtInf { get; } = null;
        public PtMemo PtMemo { get; } = null;
        public PtCmtInf PtCmtInf { get; } = null;
        public CoPtInfModel(PtInf ptInf, PtMemo ptMemo, PtCmtInf ptCmt, int sinDate)
        {
            PtInf = ptInf;
            PtMemo = ptMemo;
            PtCmtInf = ptCmt;
            SinDate = sinDate;
        }


        public int SinDate { get; set; }

        /// <summary>
        /// 患者情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号       
        /// </summary>
        public long PtId
        {
            get { return PtInf.PtId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return PtInf.SeqNo; }
        }

        /// <summary>
        /// 患者番号
        ///  医療機関が患者特定するための番号
        /// </summary>
        public long PtNum
        {
            get { return PtInf.PtNum; }
        }

        /// <summary>
        /// カナ氏名
        /// </summary>
        public string KanaName
        {
            get { return PtInf.KanaName; }
        }

        /// <summary>
        /// 氏名
        /// </summary>
        public string Name
        {
            get { return PtInf.Name; }
        }

        /// <summary>
        /// 性別
        ///  1:男 
        ///  2:女
        /// </summary>
        public int Sex
        {
            get { return PtInf.Sex; }
        }
        public string PtSex
        {
            get
            {
                string ret = "男";

                if (PtInf.Sex == 2)
                {
                    ret = "女";
                }

                return ret;
            }
        }

        /// <summary>
        /// 生年月日
        ///  yyyymmdd 
        /// </summary>
        public int Birthday
        {
            get { return PtInf.Birthday; }
        }
        /// <summary>
        /// 年齢
        /// </summary>
        public int Age
        {
            get { return CIUtil.SDateToAge(Birthday, CIUtil.DateTimeToInt(DateTime.Now)); }
        }

        /// <summary>
        /// 死亡区分
        ///  0:生存 
        ///  1:死亡 
        ///  2:消息不明
        /// </summary>
        public int IsDead
        {
            get { return PtInf.IsDead; }
        }

        /// <summary>
        /// 死亡日
        ///  yyyymmdd  
        /// </summary>
        public int DeathDate
        {
            get { return PtInf.DeathDate; }
        }

        /// <summary>
        /// 自宅郵便番号
        ///  区切り文字("-") を除く   
        /// </summary>
        public string HomePost
        {
            get { return PtInf.HomePost; }
        }

        /// <summary>
        /// 自宅住所１
        /// </summary>
        public string HomeAddress1
        {
            get { return PtInf.HomeAddress1; }
        }

        /// <summary>
        /// 自宅住所２
        /// </summary>
        public string HomeAddress2
        {
            get { return PtInf.HomeAddress2; }
        }

        /// <summary>
        /// 自宅住所
        /// </summary>
        public string HomeAddress
        {
            get { return PtInf.HomeAddress1 + PtInf.HomeAddress2; }
        }

        /// <summary>
        /// 電話番号１
        /// </summary>
        public string Tel1
        {
            get { return PtInf.Tel1 ?? ""; }
        }

        /// <summary>
        /// 電話番号２
        /// </summary>
        public string Tel2
        {
            get { return PtInf.Tel2 ?? ""; }
        }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel
        {
            get
            {
                string ret = Tel1;

                if (ret == "")
                {
                    ret = Tel2;
                }

                if (ret == "")
                {
                    ret = RenrakuTel;
                }

                return ret;
            }
        }

        /// <summary>
        /// E-Mailアドレス
        /// </summary>
        public string Mail
        {
            get { return PtInf.Mail; }
        }

        /// <summary>
        /// 世帯主名
        /// </summary>
        public string Setanusi
        {
            get { return PtInf.Setanusi; }
        }

        /// <summary>
        /// 続柄
        /// </summary>
        public string Zokugara
        {
            get { return PtInf.Zokugara; }
        }

        /// <summary>
        /// 職業
        /// </summary>
        public string Job
        {
            get { return PtInf.Job; }
        }

        /// <summary>
        /// 連絡先名称
        /// </summary>
        public string RenrakuName
        {
            get { return PtInf.RenrakuName; }
        }

        /// <summary>
        /// 連絡先郵便番号
        /// </summary>
        public string RenrakuPost
        {
            get { return PtInf.RenrakuPost; }
        }

        /// <summary>
        /// 連絡先住所１
        /// </summary>
        public string RenrakuAddress1
        {
            get { return PtInf.RenrakuAddress1; }
        }

        /// <summary>
        /// 連絡先住所２
        /// </summary>
        public string RenrakuAddress2
        {
            get { return PtInf.RenrakuAddress2; }
        }

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        public string RenrakuTel
        {
            get { return PtInf.RenrakuTel ?? ""; }
        }

        /// <summary>
        /// 連絡先メモ
        /// </summary>
        public string RenrakuMemo
        {
            get { return PtInf.RenrakuMemo; }
        }

        /// <summary>
        /// 勤務先名称
        /// </summary>
        public string OfficeName
        {
            get { return PtInf.OfficeName; }
        }

        /// <summary>
        /// 勤務先郵便番号
        /// </summary>
        public string OfficePost
        {
            get { return PtInf.OfficePost; }
        }

        /// <summary>
        /// 勤務先住所１
        /// </summary>
        public string OfficeAddress1
        {
            get { return PtInf.OfficeAddress1; }
        }

        /// <summary>
        /// 勤務先住所２
        /// </summary>
        public string OfficeAddress2
        {
            get { return PtInf.OfficeAddress2; }
        }

        /// <summary>
        /// 勤務先電話番号
        /// </summary>
        public string OfficeTel
        {
            get { return PtInf.OfficeTel; }
        }

        /// <summary>
        /// 勤務先備考
        /// </summary>
        public string OfficeMemo
        {
            get { return PtInf.OfficeMemo; }
        }

        /// <summary>
        /// 領収証明細発行区分
        ///  0:不要 
        ///  1:要
        /// </summary>
        public int IsRyosyoDetail
        {
            get { return PtInf.IsRyosyoDetail; }
        }

        /// <summary>
        /// 主治医コード
        /// </summary>
        public int PrimaryDoctor
        {
            get { return PtInf.PrimaryDoctor; }
        }

        /// <summary>
        /// テスト患者区分
        ///  1:テスト患者
        /// </summary>
        public int IsTester
        {
            get { return PtInf.IsTester; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDelete
        {
            get { return PtInf.IsDelete; }
        }

        /// <summary>
        /// MAIN_HOKEN_PID
        /// </summary>
        public int MainHokenPid
        {
            get { return PtInf.MainHokenPid; }
        }

        public string PtMemoText
        {
            get { return PtMemo?.Memo; }
        }

        public List<string> PtMemoList
        {
            get
            {
                List<string> ret = new List<string>();

                if (PtMemo != null && string.IsNullOrEmpty(PtMemo.Memo) == false)
                {
                    string[] del = { "\r\n", "\r", "\n" };
                    string[] tmps = PtMemo.Memo.Split(del, StringSplitOptions.None);

                    foreach (string tmp in tmps)
                    {
                        ret.Add(tmp);
                    }
                }

                return ret;
            }
        }

        public string PtCmtText
        {
            get { return PtCmtInf?.Text; }
        }

        public List<string> PtCmtList
        {
            get
            {
                List<string> ret = new List<string>();

                if (PtCmtInf != null && string.IsNullOrEmpty(PtCmtInf.Text) == false)
                {
                    string[] del = { "\r\n", "\r", "\n" };
                    string[] tmps = PtCmtInf.Text.Split(del, StringSplitOptions.None);

                    foreach (string tmp in tmps)
                    {
                        ret.Add(tmp);
                    }
                }

                return ret;
            }
        }
        public bool IsPreSchool()
        {
            return !CIUtil.IsStudent(Birthday, SinDate);
        }

        public bool IsElder()
        {
            return CIUtil.AgeChk(Birthday, SinDate, 70);
        }

        public bool IsElder20per()
        {
            return CIUtil.Is70Zenki_20per(Birthday, SinDate);
        }

        public bool IsElderExpat()
        {
            //75歳以上で海外居住者の方は後期高齢者医療には加入せず、
            //協会、健保組合に加入することになり、高齢受給者証を提示した場合、
            //H26.5診療分からは所得に合わせ2割または3割負担となる。
            return CIUtil.AgeChk(Birthday, SinDate, 75) && SinDate >= 20140501;
        }
    }

}
