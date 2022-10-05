namespace DevExpress.Templates
{
    partial class Karte2SubReportRichText
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Karte2SubReportRichText));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.HpId = new DevExpress.XtraReports.Parameters.Parameter();
            this.PtId = new DevExpress.XtraReports.Parameters.Parameter();
            this.SinDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.RaiinNo = new DevExpress.XtraReports.Parameters.Parameter();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // HpId
            // 
            this.HpId.Description = "Parameter1";
            this.HpId.Name = "HpId";
            this.HpId.Type = typeof(int);
            this.HpId.ValueInfo = "0";
            // 
            // PtId
            // 
            this.PtId.Description = "Parameter1";
            this.PtId.Name = "PtId";
            this.PtId.Type = typeof(int);
            this.PtId.ValueInfo = "0";
            // 
            // SinDate
            // 
            this.SinDate.Description = "Parameter1";
            this.SinDate.Name = "SinDate";
            this.SinDate.Type = typeof(int);
            this.SinDate.ValueInfo = "0";
            // 
            // RaiinNo
            // 
            this.RaiinNo.Description = "Parameter1";
            this.RaiinNo.Name = "RaiinNo";
            this.RaiinNo.Type = typeof(int);
            this.RaiinNo.ValueInfo = "0";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::DevExpress.Models.RichTextKarte2Model);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrRichText1
            // 
            this.xrRichText1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrRichText1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Html", "[RichText]")});
            this.xrRichText1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(374.1626F, 130.2083F);
            this.xrRichText1.StylePriority.UseBorders = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreport1,
            this.xrRichText1});
            this.Detail.HeightF = 130.2083F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(374.1626F, 0F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("HpId", null, "HpId"));
            this.xrSubreport1.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("PtId", null, "PtId"));
            this.xrSubreport1.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("RaiinNo", null, "RaiinNo"));
            this.xrSubreport1.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("SinDate", null, "SinDate"));
            this.xrSubreport1.ReportSource = new DevExpress.Templates.Karte2HokenSubReport();
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(377.8374F, 23F);
            // 
            // Karte2SubReportRichText
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.FilterString = "[HpId] = ?HpId And [PtId] = ?PtId And [SinDate] = ?SinDate And [RaiinNo] = ?Raiin" +
    "No";
            this.Margins = new System.Drawing.Printing.Margins(49, 49, 0, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.HpId,
            this.PtId,
            this.SinDate,
            this.RaiinNo});
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private XtraReports.Parameters.Parameter HpId;
        private XtraReports.Parameters.Parameter PtId;
        private XtraReports.Parameters.Parameter SinDate;
        private XtraReports.Parameters.Parameter RaiinNo;
        private DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private XtraReports.UI.XRRichText xrRichText1;
        private XtraReports.UI.DetailBand Detail;
        private XtraReports.UI.XRSubreport xrSubreport1;
    }
}
