namespace DevExpress.Template
{
    partial class Karte1TemplatePage2
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.byomei_name = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrShape4 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape3 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape2 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.groupHeaderPage2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.objectDataSource2 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 26.70895F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new System.Drawing.Font("MS Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.000133849F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(652.8578F, 26.70895F);
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.byomei_name,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell10,
            this.xrTableCell15});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // byomei_name
            // 
            this.byomei_name.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.byomei_name.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Byomei]")});
            this.byomei_name.Multiline = true;
            this.byomei_name.Name = "byomei_name";
            this.byomei_name.StylePriority.UseBorders = false;
            this.byomei_name.Text = "byomei_name";
            this.byomei_name.Weight = 2.2338548630934492D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.Text = "上・外";
            this.xrTableCell8.Weight = 0.62630163339468159D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ByomeiStartDateWFormat]")});
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.Text = "xrTableCell9";
            this.xrTableCell9.Weight = 1.4718284608786676D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ByomeiTenkiDateWFormat]")});
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.Text = "xrTableCell11";
            this.xrTableCell11.Weight = 1.4594014444698895D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrShape4,
            this.xrShape3,
            this.xrShape2,
            this.xrShape1,
            this.xrLabel1});
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.Weight = 1.8269303493530014D;
            // 
            // xrShape4
            // 
            this.xrShape4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrShape4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[TenkiSonota] == true")});
            this.xrShape4.LocationFloat = new DevExpress.Utils.PointFloat(107.5256F, 4.999987F);
            this.xrShape4.Name = "xrShape4";
            this.xrShape4.SizeF = new System.Drawing.SizeF(18.41937F, 14.99998F);
            this.xrShape4.StylePriority.UseBorders = false;
            // 
            // xrShape3
            // 
            this.xrShape3.Angle = 359;
            this.xrShape3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrShape3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[TenkiChusiMaru] == true")});
            this.xrShape3.LocationFloat = new DevExpress.Utils.PointFloat(78.521F, 4.999999F);
            this.xrShape3.Name = "xrShape3";
            this.xrShape3.SizeF = new System.Drawing.SizeF(17.77615F, 14.99999F);
            this.xrShape3.StylePriority.UseBorders = false;
            // 
            // xrShape2
            // 
            this.xrShape2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrShape2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[TenkiSiboMaru] == true")});
            this.xrShape2.LocationFloat = new DevExpress.Utils.PointFloat(45.30832F, 4.999987F);
            this.xrShape2.Name = "xrShape2";
            this.xrShape2.SizeF = new System.Drawing.SizeF(18.2319F, 14.99998F);
            this.xrShape2.StylePriority.UseBorders = false;
            // 
            // xrShape1
            // 
            this.xrShape1.Angle = 336;
            this.xrShape1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrShape1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[TenkiTiyuMaru] == True")});
            this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(9F, 4.999998F);
            this.xrShape1.Name = "xrShape1";
            this.xrShape1.SizeF = new System.Drawing.SizeF(18.53918F, 14.99998F);
            this.xrShape1.StylePriority.UseBorders = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("MS Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(131.9449F, 26.70895F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = " 治ゆ・死亡・中⽌・他";
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "年 ⽉ ⽇";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 1.42125289189933D;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.groupHeaderPage2});
            this.TopMargin.HeightF = 84.375F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTable1.Font = new System.Drawing.Font("MS Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52.00895F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(652.8579F, 32.36605F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell3,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.StylePriority.UseTextAlignment = false;
            this.xrTableRow1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "傷 病 名";
            this.xrTableCell1.Weight = 1.6242519638930493D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "職\r\n務";
            this.xrTableCell2.Weight = 0.44438463721375704D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "開 始";
            this.xrTableCell4.Weight = 1.0600205583534736D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "終 了";
            this.xrTableCell3.Weight = 1.0540130600527802D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "転 帰";
            this.xrTableCell5.Weight = 1.3194495585952837D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "期 間 満 了\r\n予 定 ⽇";
            this.xrTableCell6.Weight = 1.0264597836357059D;
            // 
            // groupHeaderPage2
            // 
            this.groupHeaderPage2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel70,
            this.xrLabel71,
            this.xrLabel72,
            this.xrLabel73,
            this.xrLine2,
            this.xrLine3});
            this.groupHeaderPage2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.groupHeaderPage2.Name = "groupHeaderPage2";
            this.groupHeaderPage2.SizeF = new System.Drawing.SizeF(652.9992F, 28.56055F);
            // 
            // xrLabel70
            // 
            this.xrLabel70.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel70.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(3.666821F, 5.041644F);
            this.xrLabel70.Multiline = true;
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(82.29167F, 19.51892F);
            this.xrLabel70.StylePriority.UseBorders = false;
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.StylePriority.UseTextAlignment = false;
            this.xrLabel70.Text = "患 者 番 号";
            this.xrLabel70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel71
            // 
            this.xrLabel71.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PtNum]")});
            this.xrLabel71.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(85.95851F, 5F);
            this.xrLabel71.Multiline = true;
            this.xrLabel71.Name = "xrLabel71";
            this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel71.SizeF = new System.Drawing.SizeF(100F, 19.56055F);
            this.xrLabel71.StylePriority.UseBorders = false;
            this.xrLabel71.StylePriority.UseFont = false;
            this.xrLabel71.StylePriority.UseTextAlignment = false;
            this.xrLabel71.Text = "txt_PtNum";
            this.xrLabel71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel72
            // 
            this.xrLabel72.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel72.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PtName]")});
            this.xrLabel72.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(342.8335F, 4.041557F);
            this.xrLabel72.Multiline = true;
            this.xrLabel72.Name = "xrLabel72";
            this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel72.SizeF = new System.Drawing.SizeF(214.8333F, 20.519F);
            this.xrLabel72.StylePriority.UseBorders = false;
            this.xrLabel72.StylePriority.UseFont = false;
            this.xrLabel72.StylePriority.UseTextAlignment = false;
            this.xrLabel72.Text = "txt_PtNum";
            this.xrLabel72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel73
            // 
            this.xrLabel73.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel73.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(300.1252F, 4.083241F);
            this.xrLabel73.Multiline = true;
            this.xrLabel73.Name = "xrLabel73";
            this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel73.SizeF = new System.Drawing.SizeF(42.70828F, 20.47732F);
            this.xrLabel73.StylePriority.UseBorders = false;
            this.xrLabel73.StylePriority.UseFont = false;
            this.xrLabel73.StylePriority.UseTextAlignment = false;
            this.xrLabel73.Text = "⽒名";
            this.xrLabel73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine2
            // 
            this.xrLine2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(3.666821F, 24.56054F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(257.2917F, 2F);
            this.xrLine2.StylePriority.UseBorders = false;
            // 
            // xrLine3
            // 
            this.xrLine3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(300.375F, 25.00004F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(257.2917F, 2F);
            this.xrLine3.StylePriority.UseBorders = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 25F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSourceType = null;
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // objectDataSource2
            // 
            this.objectDataSource2.DataSource = typeof(global::DevExpress.Models.Karte1ExportModel);
            this.objectDataSource2.Name = "objectDataSource2";
            // 
            // Karte1TemplatePage2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.objectDataSource2});
            this.DataMember = "ListByomeiModelsPage2";
            this.DataSource = this.objectDataSource2;
            this.Margins = new System.Drawing.Printing.Margins(100, 95, 84, 25);
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private XtraReports.UI.XRPanel groupHeaderPage2;
        private XtraReports.UI.XRLabel xrLabel70;
        private XtraReports.UI.XRLabel xrLabel71;
        private XtraReports.UI.XRLabel xrLabel72;
        private XtraReports.UI.XRLabel xrLabel73;
        private XtraReports.UI.XRLine xrLine2;
        private XtraReports.UI.XRLine xrLine3;
        private XtraReports.UI.XRTable xrTable1;
        private XtraReports.UI.XRTableRow xrTableRow1;
        private XtraReports.UI.XRTableCell xrTableCell1;
        private XtraReports.UI.XRTableCell xrTableCell2;
        private XtraReports.UI.XRTableCell xrTableCell4;
        private XtraReports.UI.XRTableCell xrTableCell3;
        private XtraReports.UI.XRTableCell xrTableCell5;
        private XtraReports.UI.XRTableCell xrTableCell6;
        private DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private XtraReports.UI.XRTable xrTable2;
        private XtraReports.UI.XRTableRow xrTableRow2;
        public XtraReports.UI.XRTableCell byomei_name;
        private XtraReports.UI.XRTableCell xrTableCell8;
        private XtraReports.UI.XRTableCell xrTableCell9;
        private XtraReports.UI.XRTableCell xrTableCell11;
        private XtraReports.UI.XRTableCell xrTableCell10;
        private XtraReports.UI.XRTableCell xrTableCell15;
        private XtraReports.UI.XRShape xrShape1;
        private XtraReports.UI.XRShape xrShape4;
        private XtraReports.UI.XRShape xrShape3;
        private XtraReports.UI.XRShape xrShape2;
        private XtraReports.UI.XRLabel xrLabel1;
        private DataAccess.ObjectBinding.ObjectDataSource objectDataSource2;
    }
}
