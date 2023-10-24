using System.Text.Json.Serialization;

namespace Domain.Models.SystemConf
{
    public class SystemConfListXmlPathModel
    {
        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }

        public int SeqNo { get; private set; }

        public string Machine { get; private set; }

        public string Path { get; private set; }

        public string Param { get; private set; }

        public string Biko { get; private set; }

        public int CharCd { get; private set; }

        public int IsInvalid { get; private set; }

        public int CreateId { get; private set; }

        public DateTime CreateDate { get; private set; }

        [JsonConstructor]
        public SystemConfListXmlPathModel(int hpId, int grpCd, int grpEdaNo, int seq, string machine, string path, string param, string biko, int charCd, int isInvalid, int createId, DateTime createDate)
        {
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            SeqNo = seq;
            Machine = machine;
            Path = path;
            Param = param;
            Biko = biko;
            CharCd = charCd;
            IsInvalid = isInvalid;
            CreateId = createId;
            CreateDate = createDate;
        }

        public SystemConfListXmlPathModel()
        {
            Machine = string.Empty;
            Path = string.Empty;
            Param = string.Empty;
            Biko = string.Empty;
        }
    }
}