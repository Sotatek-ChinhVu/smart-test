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
        public Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> DrawTextData { get; set; } = new();

        [JsonPropertyName("drawLineObject")]
        public Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> DrawLineData { get; set; } = new();

        [JsonPropertyName("drawBoxObject")]
        public Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> DrawBoxData { get; set; } = new();

        [JsonPropertyName("drawCircleObject")]
        public Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> DrawCircleData { get; set; } = new();

        public string DataJsonConverted { get; set; } = string.Empty;
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

    public class ListDrawLineObject
    {
        public ListDrawLineObject(double startX, double startY, double endX, double endY, long width, string style, string color)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            Width = width;
            Style = style;
            Color = color;
        }

        public ListDrawLineObject(double startX, double startY, double endX, double endY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            Width = 10;
            Style = "Solid";
            Color = "Black";
        }

        [JsonPropertyName("startX")]
        public double StartX { get; set; }

        [JsonPropertyName("startY")]
        public double StartY { get; set; }

        [JsonPropertyName("endX")]
        public double EndX { get; set; }

        [JsonPropertyName("endY")]
        public double EndY { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("style")]
        public string Style { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }

    public class ListDrawBoxObject
    {
        public ListDrawBoxObject(long startX, long startY, long width, long heigth, long round, string fillColor, string lineColor)
        {
            StartX = startX;
            StartY = startY;
            Width = width;
            Heigth = heigth;
            Round = round;
            FillColor = fillColor;
            LineColor = lineColor;
        }

        [JsonPropertyName("startX")]
        public long StartX { get; set; }

        [JsonPropertyName("startY")]
        public long StartY { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("heigth")]
        public long Heigth { get; set; }

        [JsonPropertyName("round")]
        public long Round { get; set; }

        [JsonPropertyName("fillColor")]
        public string FillColor { get; set; }

        [JsonPropertyName("lineColor")]
        public string LineColor { get; set; }
    }

    public class ListDrawCircleObject
    {
        public ListDrawCircleObject(double startX, double startY, double width, double heigth, string fillColor, string lineColor)
        {
            StartX = startX;
            StartY = startY;
            Width = width;
            Heigth = heigth;
            FillColor = fillColor;
            LineColor = lineColor;
        }

        [JsonPropertyName("startX")]
        public double StartX { get; set; }

        [JsonPropertyName("startY")]
        public double StartY { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("heigth")]
        public double Heigth { get; set; }

        [JsonPropertyName("fillColor")]
        public string FillColor { get; set; }

        [JsonPropertyName("lineColor")]
        public string LineColor { get; set; }
    }

    public enum ConLineStyle
    {
        None = 0,
        Solid = 1,
        Dash = 2,
        Dot = 3,
        DashDot = 4,
        DashDotDot = 5
    }
}
