using Helper.Common;
using System.Text.Json.Serialization;

namespace Reporting.NameLabel.Models
{
    public class CoNameLabelModel
    {
        CoPtInfModel PtInf { get; } = null;
        
        public CoNameLabelModel(CoPtInfModel ptInf, string ptName, int sinDate)
        {
            PtInf = ptInf;
            PtName = ptName;
            SinDate = sinDate;
        }

        public string PtName { get; }

        public int SinDate { get; }

        [JsonPropertyName("Barcode")]
        public long PtNum
        {
            get { return PtInf != null ? PtInf.PtNum : 0; }
        }

        [JsonPropertyName("KanName")]
        public string Name
        {
            get { return PtInf != null ? PtInf.Name : ""; }
        }

        [JsonPropertyName("KanKana")]
        public string KanaName
        {
            get { return PtInf != null ? PtInf.KanaName : ""; }
        }

        [JsonPropertyName("KanBirthday")]
        public string BirthdayCnv
        {
            get
            {
                string ret = "";

                if (PtInf.BirthDay > 0)
                {
                    ret = CIUtil.SDateToShowSDate(PtInf.BirthDay);
                }

                return ret;
            }
        }

        [JsonPropertyName("KanBirthdayW")]
        public string WBirthdayCnv
        {
            get
            {
                string ret = "";
                if (PtInf.BirthDay > 0)
                {
                    ret = CIUtil.SDateToShowWDate3(PtInf.BirthDay).Ymd;
                }

                return ret;
            }
        }

        [JsonPropertyName("KanSex")]
        public string SexCnv
        {
            get
            {
                string ret = "";
                if (PtInf != null)
                {
                    if (PtInf.Sex == 1)
                    {
                        ret = "男";
                    }
                    else if (PtInf.Sex == 2)
                    {
                        ret = "女";
                    }
                }

                return ret;
            }
        }
    }
}
