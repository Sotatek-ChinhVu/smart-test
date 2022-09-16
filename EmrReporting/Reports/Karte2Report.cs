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

    // Caret (text cursor)
    private PointF currentLeftCellCaretLocation;
    private PointF currentRightCellCaretLocation;

    public void WriteRowHeaderInfo(string sinDate, string generalInfo)
    {
        var rowStartLocation = currentLeftCellCaretLocation.Y > currentRightCellCaretLocation.Y ? currentLeftCellCaretLocation : currentRightCellCaretLocation;
        var sinDateLabel = CreateDefaultLabel();
        sinDateLabel.Text = sinDate;
        sinDateLabel.LocationF = new PointF(0, rowStartLocation.Y);
        AddLabelToLeftCell(sinDateLabel);

        var generalInfoLabel = CreateDefaultLabel();
        generalInfoLabel.Text = generalInfo;
        generalInfoLabel.LocationF = new PointF(0, rowStartLocation.Y);
        AddLabelToRightCell(generalInfoLabel);
    }

    public void WriteGroupName(string groupName)
    {
        var label = new XRLabel();
        label.Text = groupName;
        label.CanGrow = false;
        label.CanShrink = false;
        label.Styles.Style = UnderlineTextStyle;
        label.Parent = RightCell;
        label.LocationF = new PointF(0, currentRightCellCaretLocation.Y);
        var rec = BestSizeEstimator.GetBoundsToFitText(label);
        label.HeightF = rec.Height;
        currentRightCellCaretLocation.Y += label.HeightF;
    }

    public void WriteGroupNameFit(string groupName)
    {
        var label = new XRLabel();
        label.CanGrow = false;
        label.CanShrink = false;
        label.Styles.Style = UnderlineTextStyle;
        label.Text = groupName;
        label.Parent = RightCell;
        label.LocationF = new PointF(0, currentRightCellCaretLocation.Y);
        var reg = BestSizeEstimator.GetBoundsToFitText(label);
        //label.HeightF = reg.Height;
        label.SizeF = reg.Size;
        currentRightCellCaretLocation.Y += label.HeightF;
    }

    public void WriteActivedOrderCreatedInfo(string info, bool hasStar = false)
    {
        if (hasStar)
        {
            var star = new XRLabel();
        }
        var label = new XRLabel();
        label.Text = info;
        label.Parent = RightCell;
        label.LocationF = currentRightCellCaretLocation;
        currentRightCellCaretLocation.Y += label.HeightF;
    }

    private void AddLabelToLeftCell(XRLabel label)
    {
        label.Parent = LeftCell;
        label.WidthF = LeftCell.WidthF;
        //label.LocationF = currentLeftCellCaretLocation;
        AdjustHeight(label);
        currentLeftCellCaretLocation.Y += label.HeightF;
    }

    private void AddLabelToRightCell(XRLabel label)
    {
        label.Parent = RightCell;
        label.WidthF = RightCell.WidthF;
        //label.LocationF = currentRightCellCaretLocation;
        AdjustHeight(label);
        currentRightCellCaretLocation.Y += label.HeightF;
    }

    private void AdjustHeight(XRLabel label)
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
