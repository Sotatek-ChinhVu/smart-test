
namespace Domain.Models.MstItem
{
    public class SinKouiModel
    {
        public SinKouiModel(int sinKouiCd, string sinkouiName)
        {
            SinKouiCd = sinKouiCd;
            SinkouiName = sinkouiName;
        }

        public int SinKouiCd { get; set; }

        public string SinkouiName { get; set; }
    }

    public class SinkouiCollection : List<SinKouiModel>
    {
        public SinkouiCollection()
        {
            Add(new SinKouiModel(0, string.Empty));

            Add(new SinKouiModel(10, "初再診"));

            Add(new SinKouiModel(11, "初診"));

            Add(new SinKouiModel(12, "再診"));

            Add(new SinKouiModel(13, "医管"));

            Add(new SinKouiModel(14, "在宅"));

            Add(new SinKouiModel(20, "投薬"));

            Add(new SinKouiModel(21, "内服"));

            Add(new SinKouiModel(22, "頓服"));

            Add(new SinKouiModel(23, "外用"));

            Add(new SinKouiModel(24, string.Empty));

            Add(new SinKouiModel(25, "処方料"));

            Add(new SinKouiModel(26, "麻毒"));

            Add(new SinKouiModel(27, "調基"));

            Add(new SinKouiModel(28, "自己注"));

            Add(new SinKouiModel(30, "注射"));

            Add(new SinKouiModel(31, "皮下筋"));

            Add(new SinKouiModel(32, "静注"));

            Add(new SinKouiModel(33, "点滴"));

            Add(new SinKouiModel(34, "注射他"));

            Add(new SinKouiModel(40, "処置"));

            Add(new SinKouiModel(50, "手術"));

            Add(new SinKouiModel(52, "輸血"));

            Add(new SinKouiModel(54, "麻酔"));

            Add(new SinKouiModel(60, "検査"));

            Add(new SinKouiModel(61, "検体"));

            Add(new SinKouiModel(62, "生体"));

            Add(new SinKouiModel(64, "病理"));

            Add(new SinKouiModel(70, "画像"));

            Add(new SinKouiModel(77, "フィルム"));

            Add(new SinKouiModel(80, "その他"));

            Add(new SinKouiModel(81, "リハ"));

            Add(new SinKouiModel(82, "精神"));

            Add(new SinKouiModel(83, "処方箋"));

            Add(new SinKouiModel(84, "放射"));

            Add(new SinKouiModel(90, string.Empty));

            Add(new SinKouiModel(92, string.Empty));

            Add(new SinKouiModel(93, string.Empty));

            Add(new SinKouiModel(95, "自費"));

            Add(new SinKouiModel(96, "自費"));

            Add(new SinKouiModel(97, string.Empty));

            Add(new SinKouiModel(99, "コメント"));

            Add(new SinKouiModel(100, "コメント"));

            Add(new SinKouiModel(101, "コメント"));
        }
    }
}
