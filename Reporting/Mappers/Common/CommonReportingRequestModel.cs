using System.Text.Json.Serialization;

namespace Reporting.Mappers.Common
{
    public class CommonReportingRequestModel
    {
        [JsonPropertyName("reportType")]
        public int ReportType { get; set; }

        [JsonPropertyName("jobName")]
        public string JobName { get; set; } = string.Empty;

        [JsonPropertyName("fileNamePageMap")]
        public Dictionary<string, string> FileNamePageMap { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("reportConfigModel")]
        public ReportConfigModel ReportConfigModel { get; set; } = new();

        [JsonPropertyName("singleFieldList")]
        public Dictionary<string, string> SingleFieldList { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("tableFieldData")]
        public List<Dictionary<string, CellModel>> TableFieldData { get; set; } = new List<Dictionary<string, CellModel>>();

        [JsonPropertyName("systemConfigList")]
        public Dictionary<string, string> SystemConfigList { get; set; } = new();

        [JsonPropertyName("extralData")]
        public Dictionary<string, string> ExtralData { get; set; } = new();

        [JsonPropertyName("listTextData")]
        public Dictionary<int, List<ListTextObject>> ListTextData { get; set; } = new();

        [JsonPropertyName("setFieldData")]
        public Dictionary<int, Dictionary<string, string>> SetFieldData { get; set; } = new();

        [JsonPropertyName("reportConfigPerPage")]
        public Dictionary<int, ReportConfigModel> ReportConfigPerPage { get; set; } = new();

        [JsonPropertyName("drawTextData")]
        public Dictionary<int, List<ListDrawTextObject>>? DrawTextData { get; internal set; }
    }

    public class CellModel
    {
        private bool value;

        public CellModel(string value)
        {
            Value = value;
            IsUnderline = false;
        }

        public CellModel(bool value)
        {
            this.value = value;
        }

        public CellModel(string value, bool isUnderline)
        {
            Value = value;
            IsUnderline = isUnderline;
        }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("isUnderline")]
        public bool IsUnderline { get; set; }

        [JsonPropertyName("isBold")]
        public bool IsBold { get; set; }

        [JsonPropertyName("isItalic")]
        public bool IsItalic { get; set; }
    }

    public class ReportConfigModel
    {
        [JsonPropertyName("rowCountFieldName")]
        public string RowCountFieldName { get; set; } = string.Empty;

        [JsonPropertyName("visibleFieldList")]
        public Dictionary<string, bool> VisibleFieldList { get; set; } = new Dictionary<string, bool>();

        [JsonPropertyName("visibleAtPrint")]
        public Dictionary<string, bool> VisibleAtPrint { get; set; } = new Dictionary<string, bool>();

        [JsonPropertyName("wrapFieldList")]
        public Dictionary<string, bool> WrapFieldList { get; set; } = new Dictionary<string, bool>();
    }

    public class ListTextObject
    {
        public ListTextObject(string listName, int col, int row, string data)
        {
            ListName = listName;
            Col = col;
            Row = row;
            Data = data;
        }

        [JsonPropertyName("listName")]
        public string ListName { get; set; }

        [JsonPropertyName("col")]
        public int Col { get; set; }

        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }

    public class ListDrawTextObject
    {
        public ListDrawTextObject(double startX, double startY, double width, double heigth, long hanFontHeight, string text)
        {
            StartX = startX;
            StartY = startY;
            Width = width;
            Heigth = heigth;
            HanFontHeight = hanFontHeight;
            Text = text;
        }

        [JsonPropertyName("startX")]
        public double StartX { get; set; }

        [JsonPropertyName("startY")]
        public double StartY { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("heigth")]
        public double Heigth { get; set; }

        [JsonPropertyName("hanFontHeight")]
        public double HanFontHeight { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
