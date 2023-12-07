using Entity.Tenant;
using Helper.Extension;

namespace Reporting.Statistics.Sta9000.Models
{
    public class CoPtInfModel
    {
        public PtInf PtInf { get; private set; }

        private readonly string _ptCmt;

        public CoPtInfModel(PtInf ptInf, int firstVisitDate, int lastVisitDate, string ptCmt)
        {
            PtInf = ptInf;
            FirstVisitDate = firstVisitDate;
            LastVisitDate = lastVisitDate;
            _ptCmt = ptCmt;
            AdjFutan = string.Empty;
            _ptCmt = string.Empty;
            AdjRate = string.Empty;
            AutoSantei = string.Empty;
            PtGrps = new();
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => PtInf.PtId;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf.PtNum;
        }

        /// <summary>
        /// 氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name ?? string.Empty;
        }

        /// <summary>
        /// カナ氏名
        /// </summary>
        public string KanaName
        {
            get => PtInf.KanaName ?? string.Empty;
        }

        /// <summary>
        /// 性別
        /// </summary>
        public string Sex
        {
            get
            {
                switch (PtInf.Sex)
                {
                    case 1: return "男";
                    case 2: return "女";
                }
                return "";
            }
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int Birthday
        {
            get => PtInf.Birthday;
        }

        /// <summary>
        /// 死亡日
        /// </summary>
        public int DeathDate
        {
            get => PtInf.DeathDate;
        }

        /// <summary>
        /// 死亡区分
        /// </summary>
        public int IsDead
        {
            get => PtInf.IsDead;
        }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string HomePost
        {
            get =>
                PtInf.HomePost?.Length == 7 ?
                    PtInf.HomePost.Substring(0, 3) + "-" + PtInf.HomePost.Substring(3, 4) :
                PtInf.HomePost ?? string.Empty;
        }

        /// <summary>
        /// 住所１
        /// </summary>
        public string HomeAddress1
        {
            get => PtInf.HomeAddress1 ?? string.Empty;
        }

        /// <summary>
        /// 住所２
        /// </summary>
        public string HomeAddress2
        {
            get => PtInf.HomeAddress2 ?? string.Empty;
        }

        /// <summary>
        /// 電話１
        /// </summary>
        public string Tel1
        {
            get => PtInf.Tel1 ?? string.Empty;
        }

        /// <summary>
        /// 電話２
        /// </summary>
        public string Tel2
        {
            get => PtInf.Tel2 ?? string.Empty;
        }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Mail
        {
            get => PtInf.Mail ?? string.Empty;
        }

        /// <summary>
        /// 世帯主
        /// </summary>
        public string Setainusi
        {
            get => PtInf.Setanusi ?? string.Empty;
        }

        /// <summary>
        /// 世帯主との続柄
        /// </summary>
        public string Zokugara
        {
            get => PtInf.Zokugara ?? string.Empty;
        }

        /// <summary>
        /// 職業
        /// </summary>
        public string Job
        {
            get => PtInf.Job ?? string.Empty;
        }

        /// <summary>
        /// 連絡先名称
        /// </summary>
        public string RenrakuName
        {
            get => PtInf.RenrakuName ?? string.Empty;
        }

        /// <summary>
        /// 連絡先郵便番号
        /// </summary>
        public string RenrakuPost
        {
            get =>
                PtInf.RenrakuPost?.Length == 7 ?
                    PtInf.RenrakuPost.Substring(0, 3) + "-" + PtInf.RenrakuPost.Substring(3, 4) :
                PtInf.RenrakuPost ?? string.Empty;
        }

        /// <summary>
        /// 連絡先住所１
        /// </summary>
        public string RenrakuAddress1
        {
            get => PtInf.RenrakuAddress1 ?? string.Empty;
        }

        /// <summary>
        /// 連絡先住所２
        /// </summary>
        public string RenrakuAddress2
        {
            get => PtInf.RenrakuAddress2 ?? string.Empty;
        }

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        public string RenrakuTel
        {
            get => PtInf.RenrakuTel ?? string.Empty;
        }

        /// <summary>
        /// 連絡先備考
        /// </summary>
        public string RenrakuMemo
        {
            get => PtInf.RenrakuMemo ?? string.Empty;
        }

        /// <summary>
        /// 勤務先名称
        /// </summary>
        public string OfficeName
        {
            get => PtInf.OfficeName ?? string.Empty;
        }

        /// <summary>
        /// 勤務先郵便番号
        /// </summary>
        public string OfficePost
        {
            get =>
                PtInf?.OfficePost?.Length == 7 ?
                    PtInf?.OfficePost.Substring(0, 3) + "-" + PtInf?.OfficePost.Substring(3, 4) :
                PtInf?.OfficePost ?? string.Empty;
        }

        /// <summary>
        /// 勤務先住所１
        /// </summary>
        public string OfficeAddress1
        {
            get => PtInf.OfficeAddress1 ?? string.Empty;
        }

        /// <summary>
        /// 勤務先住所２
        /// </summary>
        public string OfficeAddress2
        {
            get => PtInf.OfficeAddress2 ?? string.Empty;
        }

        /// <summary>
        /// 勤務先電話番号
        /// </summary>
        public string OfficeTel
        {
            get => PtInf.OfficeTel ?? string.Empty;
        }

        /// <summary>
        /// 勤務先備考
        /// </summary>
        public string OfficeMemo
        {
            get => PtInf.OfficeMemo ?? string.Empty;
        }

        /// <summary>
        /// 領収証明細
        /// </summary>
        public int IsRyosyuDetail
        {
            get => PtInf.IsRyosyoDetail;
        }

        /// <summary>
        /// 主治医
        /// </summary>
        public int PrimaryDoctor
        {
            get => PtInf.PrimaryDoctor;
        }

        /// <summary>
        /// テスト患者区分
        /// </summary>
        public int IsTester
        {
            get => PtInf.IsTester;
        }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreateDate
        {
            get => PtInf.CreateDate;
        }

        /// <summary>
        /// 初回来院日
        /// </summary>
        public int FirstVisitDate { get; private set; }

        /// <summary>
        /// 最終来院日
        /// </summary>
        public int LastVisitDate { get; private set; }

        /// <summary>
        /// 患者コメント
        /// </summary>
        public string PtCmt
        {
            get => _ptCmt.AsString().Replace(Environment.NewLine, "⏎");
        }

        /// <summary>
        /// 調整額
        /// </summary>
        public string AdjFutan { get; set; }

        /// <summary>
        /// 調整率
        /// </summary>
        public string AdjRate { get; set; }

        /// <summary>
        /// 自動算定
        /// </summary>
        public string AutoSantei { get; set; }

        /// <summary>
        /// 患者グループ
        /// </summary>
        public struct PtGrp
        {
            public int GrpId { get; set; }
            public string GrpName { get; set; }
            public string GrpCode { get; set; }
            public string GrpCodeName { get; set; }

            public PtGrp(int grpId, string grpName, string grpCode, string grpCodeName)
            {
                GrpId = grpId;
                GrpName = grpName;
                GrpCode = grpCode;
                GrpCodeName = grpCodeName;
            }
        };

        /// <summary>
        /// 患者グループ
        /// </summary>
        public List<PtGrp> PtGrps { get; set; }
    }
}
