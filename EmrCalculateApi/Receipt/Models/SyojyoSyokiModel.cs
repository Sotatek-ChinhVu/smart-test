using EmrCalculateApi.Interface;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;

namespace EmrCalculateApi.Receipt.Models
{
    public class SyojyoSyokiModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateReceipt;

        private SyoukiInf SyoukiInf { get; } = null;

        IEmrLogger _emrLogger;
        public SyojyoSyokiModel(SyoukiInf syoukiInf, IEmrLogger emrLogger)
        {
            SyoukiInf = syoukiInf;

            _emrLogger = emrLogger;
        }

        public long PtId
        {
            get { return SyoukiInf?.PtId ?? 0;  }
        }

        public string RecId
        {
            get { return "SJ"; }
        }

        public int SyoukiKbn
        {
            get { return SyoukiInf.SyoukiKbn; }
        }

        public string Syouki
        {
            get { return SyoukiInf?.Syouki ?? ""; }
        }

        /// <summary>
        /// SJレコード
        /// <paramref name="mode">1:アフターケア(詳記区分なし)</paramref>
        /// </summary>
        public string SJRecord(int mode = 0)
        {
            const int conMaxLength = 1200;

            string ret = "";
            string syokiKbn = $"{SyoukiKbn:D2}";
            if(mode == 1)
            {
                syokiKbn = "";
            }

            string commentData = CIUtil.ToRecedenString(Syouki);

            string refText = "";
            string badText = "";

            if (CIUtil.IsUntilJISKanjiLevel2(commentData, ref refText, ref badText) == false)
            {
                _emrLogger.WriteLogMsg( this, "GetCORecord",
                    string.Format("SyoukiInf is include bad charcter PtId:{0} Comment:{1} badCharcters:{1}",
                        SyoukiInf.PtId, commentData, badText));

                commentData = refText;

            }

            if (commentData != "")
            {
                string[] tmpLines = commentData.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                List<string> lines = new List<string>();

                bool first = true;
                for (int i = 0; i < tmpLines.Count(); i++)
                {
                    if (first && tmpLines[i].Trim() == "")
                    {
                        // 先頭の空行はカット
                    }
                    else
                    {
                        first = false;
                        lines.Add(tmpLines[i]);
                    }
                }

                // 最後の空行はカット
                if (lines.Count() > 0)
                {
                    while (lines[lines.Count() - 1].Trim() == "")
                    {
                        lines.RemoveAt(lines.Count() - 1);
                    }
                }

                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].Trim() == "")
                    {
                        // 改行だけの行
                        if (ret != "")
                        {
                            ret += "\r\n" + RecId + ",,";
                        }
                        else
                        {
                            ret += RecId + "," + $"{syokiKbn}" + ",";
                        }
                    }
                    else
                    {
                        while (lines[i] != "")
                        {
                            string tmp = lines[i];
                            if (tmp.Length > conMaxLength)
                            {
                                tmp = lines[i].Substring(0, conMaxLength);
                            }
                            // コメント情報
                            if (ret != "")
                            {
                                ret += "\r\n" + RecId + ",," + tmp;
                            }
                            else
                            {
                                ret += RecId + "," + $"{syokiKbn}" + "," + tmp;
                            }

                            lines[i] = CIUtil.Copy(lines[i], tmp.Length + 1, lines[i].Length - tmp.Length);
                        }
                    }
                }
                                        
            }

            return ret;
        }
    }
}
