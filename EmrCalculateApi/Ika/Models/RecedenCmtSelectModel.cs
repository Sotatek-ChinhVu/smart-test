using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class RecedenCmtSelectModel
    {
        public RecedenCmtSelect RecedenCmtSelect { get; } = null;

        public RecedenCmtSelectModel(
            RecedenCmtSelect recedenCmtSelect, int cmtsbt, 
            int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3, int cmtCol4, int cmtColKeta4, 
            string name)
        {
            RecedenCmtSelect = recedenCmtSelect;
            CmtSbt = cmtsbt;
            CmtCol1 = cmtCol1;
            CmtColKeta1 = cmtColKeta1;
            CmtCol2 = cmtCol2;
            CmtColKeta2 = cmtColKeta2;
            CmtCol3 = cmtCol3;
            CmtColKeta3 = cmtColKeta3;
            CmtCol4 = cmtCol4;
            CmtColKeta4 = cmtColKeta4;

            Name = name;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RecedenCmtSelect.HpId; }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return RecedenCmtSelect.ItemCd; }
        }

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return RecedenCmtSelect.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return RecedenCmtSelect.EndDate; }
        }

        /// <summary>
        /// 順番
        /// 
        /// </summary>
        public int SortNo
        {
            get { return RecedenCmtSelect.SortNo; }
        }

        /// <summary>
        /// コメントコード
        /// コメントマスターの請求コード
        /// </summary>
        public string CommentCd
        {
            get { return RecedenCmtSelect.CommentCd; }
        }

        /// <summary>
        /// 有効区分
        /// 0:有効、1:無効
        /// </summary>
        public int IsInvalid
        {
            get { return RecedenCmtSelect.IsInvalid; }
        }

        /// <summary>
        /// 項番
        /// 記載要領別表Ⅰの「項番」列の値
        /// </summary>
        public int ItemNo
        {
            get { return RecedenCmtSelect.ItemNo; }
        }

        /// <summary>
        /// 区分
        /// 記載要領別表Ⅰの「区分」列の値
        /// </summary>
        public string KbnNo
        {
            get { return RecedenCmtSelect.KbnNo; }
        }

        /// <summary>
        /// 枝番
        /// 項番内に複数の条件がある場合、条件ごとに連番
        /// </summary>
        public int EdaNo
        {
            get { return RecedenCmtSelect.EdaNo; }
        }

        /// <summary>
        /// 患者の状態
        /// 記載要領別表Ⅰに収載されている患者の状態コード
        /// </summary>
        public int PtStatus
        {
            get { return RecedenCmtSelect.PtStatus; }
        }

        /// <summary>
        /// 条件区分
        /// 00:「01,02,03」以外
        /// 01:対象の診療行為の算定が条件であって、
        /// 　それ以外の条件がない場合
        /// 02:対象の診療行為の算定が条件であって、
        /// 　入院又は入院外のいずれかで算定した場合
        /// 03:対象の診療行為の算定が条件であって、
        /// 　複数回算定した場合
        /// </summary>
        public int CondKbn
        {
            get { return RecedenCmtSelect.CondKbn; }
        }

        /// <summary>
        /// 非算定理由コメント
        /// 0:「1」以外のコメント
        /// 1:対象の診療行為を算定しなかった場合であって、
        /// 条件に合致する場合に記録するコメント
        /// </summary>
        public int NotSanteiKbn
        {
            get { return RecedenCmtSelect.NotSanteiKbn; }
        }

        /// <summary>
        /// 入外区分
        /// 条件区分が「02」の場合、いずれの条件か表す
        /// 1:入院
        /// 2:入院外
        /// </summary>
        public int NyugaiKbn
        {
            get { return RecedenCmtSelect.NyugaiKbn; }
        }

        /// <summary>
        /// 算定回数
        /// 条件区分が「03」の場合、コメントコードの記録が必要となる対象の診療行為の算定回数を表す
        /// </summary>
        public int SanteiCnt
        {
            get { return RecedenCmtSelect.SanteiCnt; }
        }

        /// <summary>
        /// コメント種別
        /// コメントマスターの場合、コメントの種類を表す。
        /// 0:下記以外
        /// 1:初回日
        /// 2:前回日
        /// 3:実施日
        /// 4:手術日
        /// 5:発症日
        /// 6:治療開始日
        /// 7:発症日または治療開始日
        /// 8:急性憎悪
        /// 9:初回診断
        /// 10:診療時間
        /// 11:疾患名
        /// 20:撮影部位
        /// 21:撮影部位（胸部）
        /// 22:撮影部位（腹部）
        /// </summary>
        public int CmtSbt { get; set; } = 0;
        /// <summary>
        /// コメント位置１
        /// </summary>
        public int CmtCol1 { get; set; } = 0;
        /// <summary>
        /// コメント桁数１
        /// </summary>
        public int CmtColKeta1 { get; set; } = 0;
        /// <summary>
        /// コメント位置２
        /// </summary>
        public int CmtCol2 { get; set; } = 0;
        /// <summary>
        /// コメント桁数２
        /// </summary>
        public int CmtColKeta2 { get; set; } = 0;
        /// <summary>
        /// コメント位置３
        /// </summary>
        public int CmtCol3 { get; set; } = 0;
        /// <summary>
        /// コメント桁数３
        /// </summary>
        public int CmtColKeta3 { get; set; } = 0;
        /// <summary>
        /// コメント位置４
        /// </summary>
        public int CmtCol4 { get; set; } = 0;
        /// <summary>
        /// コメント桁数４
        /// </summary>
        public int CmtColKeta4 { get; set; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        public int CmtCol(int index)
        {
            int ret = 0;

            switch(index)
            {
                case 1: ret = CmtCol1; break;
                case 2: ret = CmtCol2; break;
                case 3: ret = CmtCol3; break;
                case 4: ret = CmtCol4; break;
            }

            return ret;
        }
        public int CmtColKeta(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1: ret = CmtColKeta1; break;
                case 2: ret = CmtColKeta2; break;
                case 3: ret = CmtColKeta3; break;
                case 4: ret = CmtColKeta4; break;
            }

            return ret;
        }
        public List<int> CmtCols
        {
            get
            {
                List<int> results = new List<int>();

                for(int i = 1; i <= 4; i++)
                {
                    if(CmtCol(i) > 0)
                    {
                        results.Add(CmtCol(i));
                    }
                    else
                    {
                        break;
                    }
                }

                return results;
            }
        }
        public List<int> CmtColKetas
        {
            get
            {
                List<int> results = new List<int>();

                for (int i = 1; i <= 4; i++)
                {
                    if (CmtCol(i) > 0)
                    {
                        results.Add(CmtColKeta(i));
                    }
                    else
                    {
                        break;
                    }
                }

                return results;
            }
        }
    }

}
