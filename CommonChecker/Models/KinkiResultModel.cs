using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class KinkiResultModel : OrderInforResultModel
    {
        public string ItemCd { get; set; } = string.Empty;

        public string KinkiItemCd { get; set; } = string.Empty;

        public string AYjCd { get; set; } = string.Empty;

        public string BYjCd { get; set; } = string.Empty;

        public string SubAYjCd { get; set; } = string.Empty;

        public string SubBYjCd { get; set; } = string.Empty;

        public string CommentCode { get; set; } = string.Empty;

        public string SayokijyoCode { get; set; } = string.Empty;

        public string Kyodo { get; set; } = string.Empty;

        public bool IsNeedToReplace { get; set; }

        public string IndexWord { get; set; } = string.Empty;

        public int Sbt { get; set; }

        public string SeibunCd { get; set; } = string.Empty;
    }
}
