using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using System.Drawing;

namespace EmrReporting.Reports;

public partial class Karte2Report : XtraReport
{
    public Karte2Report()
    {
        InitializeComponent();
        currentLeftCellCaretLocation = LeftCell.LocationF;
        currentRightCellCaretLocation = RightCell.LocationF;
    }

    private const float normalLineIndentWidth = 10F;
    private const float starLineIndentWidth = normalLineIndentWidth - 20F;
    // Caret (text cursor)
    private PointF currentLeftCellCaretLocation;
    private PointF currentRightCellCaretLocation;

    public void WriteRowHeaderInfo(string sinDate, string generalInfo)
    {
        var rowStartLocation = currentLeftCellCaretLocation.Y > currentRightCellCaretLocation.Y ? currentLeftCellCaretLocation : currentRightCellCaretLocation;
        var sinDateLabel = CreateDefaultLabel();
        sinDateLabel.Text = sinDate;
        AddLabelToLeftCell(sinDateLabel, rowStartLocation.Y);

        var generalInfoLabel = CreateDefaultLabel();
        generalInfoLabel.Text = generalInfo;
        generalInfoLabel.Styles.Style = RedTextStyle;
        AddLabelToRightCell(generalInfoLabel, rowStartLocation.Y);
    }

    public void WriteGroupName(string groupName)
    {
        var label = CreateDefaultLabel();
        label.Text = groupName;
        label.Styles.Style = UnderlineTextStyle;
        AddLabelToRightCell(label, currentRightCellCaretLocation.Y);
    }

    public void WriteActivedOrderCreatedInfo(string info, bool hasStar = false)
    {
        var label = CreateDefaultLabel();
        label.Text = "<color=black>＊</color>";
        //label.Text = hasStar ? "<color=black>＊</color>" + info : info;
        label.AllowMarkupText = hasStar;
        label.Styles.Style = RedTextStyle;
        var rec = BestSizeEstimator.GetBoundsToFitText(label);
        AddLabelToRightCell(label, currentRightCellCaretLocation.Y, hasStar ? starLineIndentWidth : normalLineIndentWidth);
    }

    private void AddLabelToLeftCell(XRLabel label, float locationY, float locationX = 0)
    {
        // Parent must be set before LocationF
        label.Parent = LeftCell;
        label.WidthF = LeftCell.WidthF;
        label.LocationF = new PointF(locationX, locationY);
        AdjustHeightToFitText(label);
        currentLeftCellCaretLocation.Y += label.HeightF;
    }

    private void AddLabelToRightCell(XRLabel label, float locationY, float locationX = 0)
    {
        // Parent must be set before LocationF
        label.Parent = RightCell;
        label.WidthF = RightCell.WidthF;
        label.LocationF = new PointF(locationX, locationY);
        AdjustHeightToFitText(label);
        currentRightCellCaretLocation.Y += label.HeightF;
    }

    private void AdjustHeightToFitText(XRLabel label)
    {
        var rec = BestSizeEstimator.GetBoundsToFitText(label);
        label.HeightF = rec.Height;
    }

    private XRLabel CreateDefaultLabel()
    {
        var label = new XRLabel();
        label.CanGrow = false;
        label.CanShrink = false;
        return label;
    }
}
