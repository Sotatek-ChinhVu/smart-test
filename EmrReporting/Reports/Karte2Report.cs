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

    private const float lineIndentWidth = 50F;
    private readonly float starLineIndentWidth = lineIndentWidth - 20F;
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

    public void WriteText(string text)
    {
        var label = CreateDefaultLabel();
        label.Text = text;
        label.AutoWidth = false;
        label.Styles.Style = NormalTextStyle;
        label.Parent = RightCell;
        var rec = BestSizeEstimator.GetBoundsToFitText(label);
        label.SizeF = rec.Size;
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
        label.Text = hasStar ? "<color=black>＊</color>" + info : info;
        label.AllowMarkupText = hasStar;
        label.Styles.Style = RedTextStyle;
        var locationX = hasStar ? starLineIndentWidth : lineIndentWidth;
        AddLabelToRightCell(label, locationX, currentRightCellCaretLocation.Y);
    }

    public bool TryWriteActivedOrderCreatedInfo(string info, bool hasStar = false)
    {
        var label = CreateDefaultLabel();
        label.Text = info;
        label.Styles.Style = RedTextStyle;
        var success = TryAddLabelToRightCell(label, RightCell.WidthF - lineIndentWidth, lineIndentWidth, currentRightCellCaretLocation.Y);
        if (!success)
        {
            return false;
        }

        if (hasStar)
        {
            var starLabel = CreateDefaultLabel();
            starLabel.Text = "＊";
            starLabel.Styles.Style = NormalTextStyle;
            starLabel.Parent = RightCell;
            var rec = BestSizeEstimator.GetBoundsToFitText(starLabel);
            starLabel.SizeF = rec.Size;
            starLabel.LocationF = new PointF(lineIndentWidth - starLabel.WidthF, currentRightCellCaretLocation.Y);
        }

        return true;
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

    private void AddLabelToRightCell(XRLabel label, float locationX, float locationY)
    {
        TryAddLabelToRightCell(label, RightCell.WidthF, locationX, locationY);
    }

    private void AddLabelToRightCell(XRLabel label, float locationY)
    {
        TryAddLabelToRightCell(label, RightCell.WidthF, 0, locationY);
    }

    private bool TryAddLabelToRightCell(XRLabel label, float width, float locationX, float locationY)
    {
        // The label's parent mus be set first
        // in order to set the label's size and location properly
        //RightCell.Controls.Add(label);
        label.Parent = RightCell;
        label.LocationF = new PointF(locationX, locationY);
        label.WidthF = width;
        AdjustHeightToFitText(label);
        var newCaretLocationY = currentRightCellCaretLocation.Y + label.HeightF;
        if (newCaretLocationY > RightCell.HeightF)
        {
            // Label overflowed, remove it
            RightCell.Controls.Remove(label);
            return false;
        }

        currentRightCellCaretLocation.Y = newCaretLocationY;
        return true;
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
