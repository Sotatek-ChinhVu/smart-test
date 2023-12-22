using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;
using Point = Reporting.GrowthCurve.Model.Point;

namespace Reporting.GrowthCurve.Service
{
    public class GrowthCurveService
    {
        protected Point RootAxis = new Point(1750, 3500);
        protected int partXCount = 12;
        private int intervalX = 12;
        private int intervalY = 5;
        private double heightMaxY = 19;
        private double heightMinY = 40;
        private int weightMinY = 0;
        private long maxX = 12;

        protected List<Line> horizotationLines = new List<Line>();
        protected List<Line> verticalLines = new List<Line>();
        protected List<Point> kanweightPoints = new List<Point>();
        protected List<Point> kanheightPoints = new List<Point>();
        protected List<GcStdInf> GcStdInfCollection = new List<GcStdInf>();
        protected readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        protected readonly Dictionary<string, string> _singleFieldData;
        protected readonly Dictionary<string, string> _extralData;
        protected readonly Dictionary<int, List<ListTextObject>> _listTextData;
        protected readonly Dictionary<string, bool> _visibleFieldData;
        protected readonly Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> _listDrawLineData;
        protected readonly Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> _listDrawTextData;
        protected readonly Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> _listDrawBoxData;
        protected readonly Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> _listDrawCircleData;
        protected readonly Dictionary<int, List<ListDrawTextObject>> _listCreateLabelsTextData;
        protected readonly Dictionary<int, List<ListDrawTextObject>> _listCreateStdInfPercentTextData;
        protected readonly Dictionary<int, List<ListDrawTextObject>> _listCreateStdInfSDTextData;
        protected readonly Dictionary<int, List<ListDrawTextObject>> _listDrawLegendLabelTextData;
        protected readonly Dictionary<int, List<ListDrawLineObject>> _listDrawHorizotationLineData;
        protected readonly Dictionary<int, List<ListDrawLineObject>> _listDrawVerticalLineData;
        protected readonly Dictionary<int, List<ListDrawLineObject>> _listCreateStdInfPercentLineData;
        protected readonly Dictionary<int, List<ListDrawLineObject>> _listCreateStdInfSDLineData;
        protected readonly Dictionary<int, List<ListDrawLineObject>> _listDrawRectangleCanvasLineData;
        protected readonly Dictionary<int, List<ListDrawBoxObject>> _listDrawKanLineBoxData;
        protected readonly Dictionary<int, List<ListDrawBoxObject>> _listDrawLegendLabelBoxData;
        protected readonly Dictionary<int, List<ListDrawCircleObject>> _listDrawKanLineCircleData;
        protected readonly Dictionary<int, List<ListDrawCircleObject>> _listDrawLegendLabelCircleData;
        List<ListDrawTextObject> listDrawTextPerPage = new();
        List<ListDrawLineObject> listDrawLinePerPage = new();
        List<ListDrawBoxObject> listDrawBoxPerPage = new();
        List<ListDrawCircleObject> listDrawCirclePerPage = new();
        protected GrowthCurveConfig GrowthCurveConfig;

        public GrowthCurveService()
        {
            _singleFieldData = new();
            _visibleFieldData = new();
            _setFieldData = new();
            _listTextData = new();
            _listDrawLineData = new();
            _listDrawTextData = new();
            _listDrawBoxData = new();
            _listDrawCircleData = new();
            _listCreateLabelsTextData = new();
            _listCreateStdInfPercentTextData = new();
            _listCreateStdInfSDTextData = new();
            _listDrawLegendLabelTextData = new();
            _listDrawHorizotationLineData = new();
            _listDrawVerticalLineData = new();
            _listCreateStdInfPercentLineData = new();
            _listCreateStdInfSDLineData = new();
            _listDrawRectangleCanvasLineData = new();
            _listDrawKanLineBoxData = new();
            _listDrawLegendLabelBoxData = new();
            _listDrawKanLineCircleData = new();
            _listDrawLegendLabelCircleData = new();
            _extralData = new();
            GrowthCurveConfig = new();
        }

        protected long CanvasWidth { get; set; } = 12500;

        protected long CanvasHeight { get; set; } = 16000;

        protected long PartWidth
        {
            get
            {
                return Convert.ToInt64(CanvasWidth / partXCount);
            }
        }

        protected long PartHeight
        {
            get
            {
                double partHeight = intervalY * CalculateAdjY();
                return Convert.ToInt64(partHeight);
            }
        }

        protected int PartYCount
        {
            get
            {
                return Convert.ToInt32(CanvasHeight / (intervalY * CalculateAdjY()));
            }
        }

        public void SetParamByScope(int scope)
        {
            switch (scope)
            {
                case 1:
                    partXCount = 12;
                    intervalX = 1;//1 Month Interval

                    heightMinY = 10;
                    heightMaxY = 85;

                    weightMinY = 2;
                    break;
                case 2:
                    partXCount = 12;
                    intervalX = 2;//2 Month Interval

                    heightMinY = 30;
                    heightMaxY = 140;

                    weightMinY = 0;
                    break;
                case 3:
                    partXCount = 9;
                    intervalX = 4;//4 Month Interval

                    heightMinY = 30;
                    heightMaxY = 140;

                    weightMinY = 0;
                    break;
                case 4:
                case 5:
                case 6:
                    partXCount = scope;
                    intervalX = 12;//1 Year Interval

                    heightMinY = 30;
                    heightMaxY = 140;

                    weightMinY = 0;
                    break;
                default:
                    partXCount = scope;
                    intervalX = 12;//1 Year Interval

                    heightMinY = 40;
                    heightMaxY = 190;

                    weightMinY = 0;
                    break;
            }
        }

        public void AddHorizotationLine(int i)
        {
            var from = new Point(RootAxis.X, RootAxis.Y + i * PartHeight);
            var to = new Point(RootAxis.X + CanvasWidth, RootAxis.Y + i * PartHeight);
            Line line = new Line(from, to);
            horizotationLines.Add(line);
        }

        public void AddVerticalLine(int i)
        {
            var from = new Point((int)(RootAxis.X + i * PartWidth), (int)RootAxis.Y);
            var to = new Point((int)(RootAxis.X + i * PartWidth), (int)(RootAxis.Y + CanvasHeight));
            Line line = new Line(from, to);
            verticalLines.Add(line);
        }

        public void AddGcStdInfCollection(GcStdMst gcStdMst)
        {
            GcStdInf item = new GcStdInf(gcStdMst);
            GcStdInfCollection.Add(item);
        }

        public void AddKanWeightPoint(double x, double y)
        {
            kanweightPoints.Add(new Point(x, y));
        }

        public void AddKanHeightPoint(double x, double y)
        {
            kanheightPoints.Add(new Point(x, y));
        }

        protected void FillBaseInfo()
        {
            if (GrowthCurveConfig.PrintMode == 0)
            {
                SetFieldData("PRN_MODE", "SD値");
            }
            else
            {
                SetFieldData("PRN_MODE", "パーセンタイル");
            }
            SetFieldData("KANID", GrowthCurveConfig.PtNum.ToString());

            SetFieldData("KANNAME", GrowthCurveConfig.PtName);

            string sex;
            if (GrowthCurveConfig.Sex == 1)
            {
                sex = "男";
            }
            else if (GrowthCurveConfig.Sex == 2)
            {
                sex = "女";
            }
            else
            {
                sex = "？";
            }

            SetFieldData("SEX", sex);

            string birthday = CIUtil.SDateToShowWDate2(GrowthCurveConfig.BirthDay);
            SetFieldData("BIRTHDAY", birthday);

            string printDate = DateTime.Now.ToString("yyyy/MM/dd");
            SetFieldData("PRN_DATE", printDate);
        }

        protected void CreateLabels()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawTextPerPage = new();
            for (int i = 1; i < partXCount; i++)
            {
                int textX = i;
                if (GrowthCurveConfig.Scope <= 3)
                {
                    textX = i * intervalX;
                }
                listDrawTextPerPage.Add(new(RootAxis.X + i * PartWidth - 100, RootAxis.Y + CanvasHeight + 50, 1000, 350, 300, textX.AsString()));
            }

            if (GrowthCurveConfig.WeightVisible)
            {
                //kg
                for (int i = 1; i < PartYCount; i++)
                {
                    string textY = string.Empty;
                    if (GrowthCurveConfig.Scope > 1)
                    {
                        if (i % 2 == 0)
                        {
                            textY = (Convert.ToInt32(intervalY * i)).AsString();
                        }
                    }
                    else
                    {
                        textY = (i + weightMinY).AsString();
                    }
                    listDrawTextPerPage.Add(new(RootAxis.X + CanvasWidth + 100, RootAxis.Y + CanvasHeight - PartHeight * i - 220, 1000, 350, 300, textY.AsString()));

                }
                //体重(kg)
                listDrawTextPerPage.Add(new(RootAxis.X + CanvasWidth - 750, RootAxis.Y - 550, 1500, 450, 300, "体重（kg）"));
            }

            if (GrowthCurveConfig.HeightVisible)
            {
                //cm
                for (int i = 1; i < PartYCount; i++)
                {
                    string textY = (Convert.ToInt32(intervalY * i + heightMinY)).AsString();
                    if (GrowthCurveConfig.Scope > 1 && i % 2 != 0)
                    {
                        textY = string.Empty;
                    }
                    listDrawTextPerPage.Add(new(RootAxis.X - 700, RootAxis.Y + CanvasHeight - PartHeight * i - 220, 1000, 350, 300, textY.AsString()));
                }
                //身長(cm)
                listDrawTextPerPage.Add(new(RootAxis.X - 450, RootAxis.Y - 550, 1500, 450, 300, "身長（cm）"));
            }

            //
            //年齢（月）
            string text = "年齢（月）";
            if (GrowthCurveConfig.Scope > 3)
            {
                text = "年齢（年）";
            }
            listDrawTextPerPage.Add(new(RootAxis.X + CanvasWidth - 750, RootAxis.Y + CanvasHeight + 500, 1500, 450, 300, text));
            _listCreateLabelsTextData.Add(pageIndex, listDrawTextPerPage);
        }

        protected void DrawHorizotationLines()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawLinePerPage = new();
            foreach (var line in horizotationLines)
            {
                listDrawLinePerPage.Add(new(line.From.X, line.From.Y, line.To.X, line.To.Y, 5, "Dash", "Orange"));
            }
            _listDrawHorizotationLineData.Add(pageIndex, listDrawLinePerPage);
        }

        protected void DrawVerticalLines()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawLinePerPage = new();
            foreach (var line in verticalLines)
            {
                listDrawLinePerPage.Add(new(line.From.X, line.From.Y, line.To.X, line.To.Y, 10, "Solid", "Orange"));
            }
            _listDrawVerticalLineData.Add(pageIndex, listDrawLinePerPage);
        }

        protected void CreateStdInfPercent()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawLinePerPage = new();
            listDrawTextPerPage = new();
            List<GcStdInf> gcStdInfCollection = GcStdInfCollection;

            //weight
            if (GrowthCurveConfig.WeightVisible)
            {
                int countPoint = GetCountPointX();

                var sdMInfCollection = gcStdInfCollection.Where(gc => gc.StdKbn == 0 && gc.Sex == GrowthCurveConfig.Sex && gc.Point <= countPoint).ToList();

                for (int i = 0; i < sdMInfCollection.Count - 1; i++)
                {
                    var item1 = sdMInfCollection[i];
                    var item2 = sdMInfCollection[i + 1];

                    Line lineSdAVG = CreateLineWeight(item1.Point, item1.Per50, item2.Point, item2.Per50);
                    double AvgPosY = lineSdAVG.To.Y - 200;

                    if (GrowthCurveConfig.Per50)
                    {
                        //Per50
                        listDrawLinePerPage.Add(new(lineSdAVG.From.X, lineSdAVG.From.Y, lineSdAVG.To.X, lineSdAVG.To.Y, 5, "Solid", "Blue"));

                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdAVG.To.Y - 200;
                            double posX = lineSdAVG.To.X - 800;

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "50%"));
                        }
                    }

                    if (GrowthCurveConfig.Per25)
                    {
                        //Per25
                        Line lineSdP25 = CreateLineWeight(item1.Point, item1.Per25, item2.Point, item2.Per25);
                        listDrawLinePerPage.Add(new(lineSdP25.From.X, lineSdP25.From.Y, lineSdP25.To.X, lineSdP25.To.Y, 5, "Solid", "Blue"));
                        //Per75
                        Line lineSdP75 = CreateLineWeight(item1.Point, item1.Per75, item2.Point, item2.Per75);
                        listDrawLinePerPage.Add(new(lineSdP75.From.X, lineSdP75.From.Y, lineSdP75.To.X, lineSdP75.To.Y, 5, "Solid", "Blue"));
                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP75.To.Y - 200;
                            double posX = lineSdP75.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 600;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "75%"));

                            posY = lineSdP25.To.Y - 200;
                            posX = lineSdP25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 500;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "25%"));
                        }
                    }

                    if (GrowthCurveConfig.Per10)
                    {
                        //Per10
                        Line lineSdP10 = CreateLineWeight(item1.Point, item1.Per10, item2.Point, item2.Per10);
                        listDrawLinePerPage.Add(new(lineSdP10.From.X, lineSdP10.From.Y, lineSdP10.To.X, lineSdP10.To.Y, 5, "Solid", "Blue"));
                        //Per90
                        Line lineSdP90 = CreateLineWeight(item1.Point, item1.Per90, item2.Point, item2.Per90);
                        listDrawLinePerPage.Add(new(lineSdP90.From.X, lineSdP90.From.Y, lineSdP90.To.X, lineSdP90.To.Y, 5, "Solid", "Blue"));
                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP90.To.Y - 200;
                            double posX = lineSdP90.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 1200;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "90%"));

                            posY = lineSdP10.To.Y - 200;
                            posX = lineSdP10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 900;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "10%"));
                        }
                    }

                    if (GrowthCurveConfig.Per3)
                    {
                        //Per3
                        Line lineSdP03 = CreateLineWeight(item1.Point, item1.Per03, item2.Point, item2.Per03);
                        listDrawLinePerPage.Add(new(lineSdP03.From.X, lineSdP03.From.Y, lineSdP03.To.X, lineSdP03.To.Y, 5, "Solid", "Blue"));
                        //Per97
                        Line lineSdP97 = CreateLineWeight(item1.Point, item1.Per97, item2.Point, item2.Per97);
                        listDrawLinePerPage.Add(new(lineSdP97.From.X, lineSdP97.From.Y, lineSdP97.To.X, lineSdP97.To.Y, 5, "Solid", "Blue"));
                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP97.To.Y - 200;
                            double posX = lineSdP97.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 2100;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "97%"));

                            posY = lineSdP03.To.Y - 200;
                            posX = lineSdP03.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 1500;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "3%"));
                        }
                    }
                }
            }

            //height
            if (GrowthCurveConfig.HeightVisible)
            {
                int countPoint = GetCountPointX();

                var sdHInfCollection = gcStdInfCollection.Where(gc => gc.StdKbn == 1 && gc.Sex == GrowthCurveConfig.Sex && gc.Point <= countPoint).ToList();

                for (int i = 0; i < sdHInfCollection.Count - 1; i++)
                {
                    var item1 = sdHInfCollection[i];
                    var item2 = sdHInfCollection[i + 1];

                    Line lineSdAVG = CreateLineHeight(item1.Point, item1.Per50, item2.Point, item2.Per50);
                    double AvgPosY = lineSdAVG.To.Y - 200;

                    if (GrowthCurveConfig.Per50)
                    {
                        //Per50
                        listDrawLinePerPage.Add(new(lineSdAVG.From.X, lineSdAVG.From.Y, lineSdAVG.To.X, lineSdAVG.To.Y, 5, "Solid", "Pink"));
                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdAVG.To.Y - 200;
                            double posX = lineSdAVG.To.X - 800;
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "50%"));
                        }
                    }

                    if (GrowthCurveConfig.Per25)
                    {
                        //Per25
                        Line lineSdP25 = CreateLineHeight(item1.Point, item1.Per25, item2.Point, item2.Per25);
                        listDrawLinePerPage.Add(new(lineSdP25.From.X, lineSdP25.From.Y, lineSdP25.To.X, lineSdP25.To.Y, 5, "Solid", "Pink"));
                        //Per75
                        Line lineSdP75 = CreateLineHeight(item1.Point, item1.Per75, item2.Point, item2.Per75);
                        listDrawLinePerPage.Add(new(lineSdP75.From.X, lineSdP75.From.Y, lineSdP75.To.X, lineSdP75.To.Y, 5, "Solid", "Pink"));
                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP75.To.Y - 200;
                            double posX = lineSdP75.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 500;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "75%"));

                            posY = lineSdP25.To.Y - 200;
                            posX = lineSdP25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 400;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "25%"));
                        }
                    }

                    if (GrowthCurveConfig.Per10)
                    {
                        //Per10
                        Line lineSdP10 = CreateLineHeight(item1.Point, item1.Per10, item2.Point, item2.Per10);
                        listDrawLinePerPage.Add(new(lineSdP10.From.X, lineSdP10.From.Y, lineSdP10.To.X, lineSdP10.To.Y, 5, "Solid", "Pink"));
                        //Per90
                        Line lineSdP90 = CreateLineHeight(item1.Point, item1.Per90, item2.Point, item2.Per90);
                        listDrawLinePerPage.Add(new(lineSdP90.From.X, lineSdP90.From.Y, lineSdP90.To.X, lineSdP90.To.Y, 5, "Solid", "Pink"));
                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP90.To.Y - 200;
                            double posX = lineSdP90.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 800;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "90%"));

                            posY = lineSdP10.To.Y - 200;
                            posX = lineSdP10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 700;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "10%"));
                        }
                    }

                    if (GrowthCurveConfig.Per3)
                    {
                        //Per3
                        Line lineSdP03 = CreateLineHeight(item1.Point, item1.Per03, item2.Point, item2.Per03);
                        listDrawLinePerPage.Add(new(lineSdP03.From.X, lineSdP03.From.Y, lineSdP03.To.X, lineSdP03.To.Y, 5, "Solid", "Pink"));
                        //Per97
                        Line lineSdP97 = CreateLineHeight(item1.Point, item1.Per97, item2.Point, item2.Per97);
                        listDrawLinePerPage.Add(new(lineSdP97.From.X, lineSdP97.From.Y, lineSdP97.To.X, lineSdP97.To.Y, 5, "Solid", "Pink"));
                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP97.To.Y - 200;
                            double posX = lineSdP97.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 1200;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "97%"));

                            posY = lineSdP03.To.Y - 200;
                            posX = lineSdP03.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 1000;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "3%"));
                        }
                    }
                }
            }

            _listCreateStdInfPercentLineData.Add(pageIndex, listDrawLinePerPage);
            _listCreateStdInfPercentTextData.Add(pageIndex, listDrawTextPerPage);
        }

        protected void CreateStdInfSD()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawTextPerPage = new();
            listDrawLinePerPage = new();

            List<GcStdInf> gcStdInfCollection = GcStdInfCollection;
            int sex = GrowthCurveConfig.Sex;

            //weight
            if (GrowthCurveConfig.WeightVisible)
            {
                int countPoint = GetCountPointX();

                var sdMInfCollection = gcStdInfCollection.Where(gc => gc.StdKbn == 0 && gc.Sex == sex && gc.Point <= countPoint).ToList();

                for (int i = 0; i < sdMInfCollection.Count - 1; i++)
                {
                    var item1 = sdMInfCollection[i];
                    var item2 = sdMInfCollection[i + 1];

                    Line lineSdAVG = CreateLineWeight(item1.Point, item1.SdAvg, item2.Point, item2.SdAvg);

                    double AvgPosY = lineSdAVG.To.Y - 200;

                    if (GrowthCurveConfig.SDAvg)
                    {
                        //SDAVG
                        listDrawLinePerPage.Add(new(lineSdAVG.From.X, lineSdAVG.From.Y, lineSdAVG.To.X, lineSdAVG.To.Y, 5, "Solid", "Blue"));

                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = AvgPosY;
                            double posX = lineSdAVG.To.X - 800;

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "平均"));
                        }
                    }

                    if (GrowthCurveConfig.SD1)
                    {
                        //SDM10
                        Line lineSdM10 = CreateLineWeight(item1.Point, item1.SdM10, item2.Point, item2.SdM10);
                        listDrawLinePerPage.Add(new(lineSdM10.From.X, lineSdM10.From.Y, lineSdM10.To.X, lineSdM10.To.Y, 5, "Solid", "Blue"));

                        //SDP10
                        Line lineSdP10 = CreateLineWeight(item1.Point, item1.SdP10, item2.Point, item2.SdP10);
                        listDrawLinePerPage.Add(new(lineSdP10.From.X, lineSdP10.From.Y, lineSdP10.To.X, lineSdP10.To.Y, 5, "Solid", "Blue"));

                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP10.To.Y - 200;
                            double posX = lineSdP10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 1200;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+1"));

                            posY = lineSdM10.To.Y - 200;
                            posX = lineSdM10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 1200;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-1"));

                        }
                    }

                    if (GrowthCurveConfig.SD2)
                    {
                        //SDM20
                        Line lineSdM20 = CreateLineWeight(item1.Point, item1.SdM20, item2.Point, item2.SdM20);
                        listDrawLinePerPage.Add(new(lineSdM20.From.X, lineSdM20.From.Y, lineSdM20.To.X, lineSdM20.To.Y, 5, "Solid", "Blue"));

                        //SDP20
                        Line lineSdP20 = CreateLineWeight(item1.Point, item1.SdP20, item2.Point, item2.SdP20);
                        listDrawLinePerPage.Add(new(lineSdP20.From.X, lineSdP20.From.Y, lineSdP20.To.X, lineSdP20.To.Y, 5, "Solid", "Blue"));

                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP20.To.Y - 200;
                            double posX = lineSdP20.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 2200;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+2"));

                            posY = lineSdM20.To.Y - 200;
                            posX = lineSdM20.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 2200;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-2"));
                        }
                    }

                    if (GrowthCurveConfig.SD25)
                    {
                        //SDM25
                        Line lineSdM25 = CreateLineWeight(item1.Point, item1.SdM25, item2.Point, item2.SdM25);
                        listDrawLinePerPage.Add(new(lineSdM25.From.X, lineSdM25.From.Y, lineSdM25.To.X, lineSdM25.To.Y, 5, "Solid", "Blue"));
                        //list.Add(lineSdM25);

                        //SDP10
                        Line lineSdP25 = CreateLineWeight(item1.Point, item1.SdP25, item2.Point, item2.SdP25);
                        listDrawLinePerPage.Add(new(lineSdP25.From.X, lineSdP25.From.Y, lineSdP25.To.X, lineSdP25.To.Y, 5, "Solid", "Blue"));
                        //list.Add(lineSdP25);
                        if (GrowthCurveConfig.Legend && i == sdMInfCollection.Count - 2)
                        {
                            double posY = lineSdP25.To.Y - 200;
                            double posX = lineSdP25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 3000;
                            }

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+2.5"));

                            posY = lineSdM25.To.Y - 200;
                            posX = lineSdM25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 3000;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-2.5"));
                        }
                    }
                }
            }

            //height
            if (GrowthCurveConfig.HeightVisible)
            {
                int countPoint = GetCountPointX();

                var sdHInfCollection = gcStdInfCollection.Where(gc => gc.StdKbn == 1 && gc.Sex == sex && gc.Point <= countPoint).ToList();

                for (int i = 0; i < sdHInfCollection.Count - 1; i++)
                {
                    var item1 = sdHInfCollection[i];
                    var item2 = sdHInfCollection[i + 1];

                    Line lineSdAVG = CreateLineHeight(item1.Point, item1.SdAvg, item2.Point, item2.SdAvg);
                    double AvgPosY = lineSdAVG.To.Y - 200;

                    if (GrowthCurveConfig.SDAvg)
                    {
                        //SDAVG
                        listDrawLinePerPage.Add(new(lineSdAVG.From.X, lineSdAVG.From.Y, lineSdAVG.To.X, lineSdAVG.To.Y, 5, "Solid", "Pink"));

                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdAVG.To.Y - 200;
                            double posX = lineSdAVG.To.X - 800;

                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "平均"));

                        }
                    }

                    if (GrowthCurveConfig.SD1)
                    {
                        //SDM10
                        Line lineSdM10 = CreateLineHeight(item1.Point, item1.SdM10, item2.Point, item2.SdM10);
                        listDrawLinePerPage.Add(new(lineSdM10.From.X, lineSdM10.From.Y, lineSdM10.To.X, lineSdM10.To.Y, 5, "Solid", "Pink"));

                        //SDP10
                        Line lineSdP10 = CreateLineHeight(item1.Point, item1.SdP10, item2.Point, item2.SdP10);
                        listDrawLinePerPage.Add(new(lineSdP10.From.X, lineSdP10.From.Y, lineSdP10.To.X, lineSdP10.To.Y, 5, "Solid", "Pink"));

                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP10.To.Y - 200;
                            double posX = lineSdP10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 500;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+1"));


                            posY = lineSdM10.To.Y - 200;
                            posX = lineSdM10.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 500;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-1"));

                        }

                    }


                    if (GrowthCurveConfig.SD2)
                    {
                        //SDM20
                        Line lineSdM20 = CreateLineHeight(item1.Point, item1.SdM20, item2.Point, item2.SdM20);
                        listDrawLinePerPage.Add(new(lineSdM20.From.X, lineSdM20.From.Y, lineSdM20.To.X, lineSdM20.To.Y, 5, "Solid", "Pink"));

                        //SDP20
                        Line lineSdP20 = CreateLineHeight(item1.Point, item1.SdP20, item2.Point, item2.SdP20);
                        listDrawLinePerPage.Add(new(lineSdP20.From.X, lineSdP20.From.Y, lineSdP20.To.X, lineSdP20.To.Y, 5, "Solid", "Pink"));

                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP20.To.Y - 200;
                            double posX = lineSdP20.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 1000;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+2"));


                            posY = lineSdM20.To.Y - 200;
                            posX = lineSdM20.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 1000;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-2"));

                        }
                    }


                    if (GrowthCurveConfig.SD25)
                    {
                        //SDM25
                        Line lineSdM25 = CreateLineHeight(item1.Point, item1.SdM25, item2.Point, item2.SdM25);
                        listDrawLinePerPage.Add(new(lineSdM25.From.X, lineSdM25.From.Y, lineSdM25.To.X, lineSdM25.To.Y, 5, "Solid", "Pink"));

                        //SDP25
                        Line lineSdP25 = CreateLineHeight(item1.Point, item1.SdP25, item2.Point, item2.SdP25);
                        listDrawLinePerPage.Add(new(lineSdP25.From.X, lineSdP25.From.Y, lineSdP25.To.X, lineSdP25.To.Y, 5, "Solid", "Pink"));

                        if (GrowthCurveConfig.Legend && i == sdHInfCollection.Count - 2)
                        {
                            double posY = lineSdP25.To.Y - 200;
                            double posX = lineSdP25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY - 1600;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "+2.5"));

                            posY = lineSdM25.To.Y - 200;
                            posX = lineSdM25.To.X - 800;

                            if (GrowthCurveConfig.Scope > 1)
                            {
                                posY = AvgPosY + 1600;
                            }
                            listDrawTextPerPage.Add(new(posX, posY, 1000, 350, 300, "-2.5"));

                        }
                    }

                }
            }

            _listCreateStdInfSDLineData.Add(pageIndex, listDrawLinePerPage);
            _listCreateStdInfSDTextData.Add(pageIndex, listDrawTextPerPage);
        }

        protected void DrawKanLine()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawCirclePerPage = new();
            listDrawBoxPerPage = new();
            if (GrowthCurveConfig.WeightVisible)
            {
                //In case there is only one point, we'll draw a line that mean from = to 
                if (kanweightPoints.Count == 1)
                {
                    kanweightPoints.Add(kanweightPoints[0]);
                }

                if (kanweightPoints.Count >= 2)
                {
                    for (int i = 0; i < kanweightPoints.Count - 1; i++)
                    {
                        var pointFrom = kanweightPoints[i];
                        var pointTo = kanweightPoints[i + 1];
                        var line = CreateLineWeight(pointFrom.X, pointFrom.Y, pointTo.X, pointTo.Y);

                        pointFrom = line.From;

                        if (pointFrom.X > maxX || pointFrom.Y < RootAxis.Y)
                        {
                            break;
                        }
                        pointTo = line.To;
                        if (pointTo.X > maxX)
                        {

                            double y = (maxX - pointFrom.X) / (pointTo.X - pointFrom.X) * (pointTo.Y - pointFrom.Y) + pointFrom.Y;
                            pointTo.X = maxX;
                            pointTo.Y = y;
                        }

                        if (pointTo.Y < RootAxis.Y)
                        {
                            double x = (maxX - pointFrom.X) * (RootAxis.Y - pointFrom.Y) / (pointTo.Y - pointFrom.Y) + pointFrom.X;
                            pointTo.X = x;
                            pointTo.Y = RootAxis.Y;
                        }

                        line.To = pointTo;

                        if (pointFrom.X >= 0 && pointFrom.Y >= 0 && pointTo.X >= 0 && pointTo.Y >= 0)
                        {
                            listDrawLinePerPage.Add(new(pointFrom.X, pointFrom.Y, pointTo.X, pointTo.Y, 40, "Solid", "Blue"));
                        }

                        if (i == 0)
                        {
                            if (pointFrom.X >= 0 && pointFrom.Y >= 0)
                            {
                                listDrawCirclePerPage.Add(new(pointFrom.X, pointFrom.Y, 120, 120, "Blue", "Blue"));
                            }

                            if (line.To.X >= 0 && line.To.Y >= 0)
                            {
                                listDrawCirclePerPage.Add(new(line.To.X - 4, line.To.Y - 4, 120, 120, "Blue", "Blue"));
                            }
                        }
                        else
                        {
                            if (line.To.X >= 0 && line.To.Y >= 0)
                            {
                                listDrawCirclePerPage.Add(new(line.To.X - 4, line.To.Y - 4, 120, 120, "Blue", "Blue"));
                            }
                        }
                    }
                }

            }

            if (GrowthCurveConfig.HeightVisible)
            {
                //Same as LineWeight
                if (kanheightPoints.Count == 1)
                {
                    kanheightPoints.Add(kanheightPoints[0]);
                }

                if (kanheightPoints.Count >= 2)
                {
                    for (int i = 0; i < kanheightPoints.Count - 1; i++)
                    {
                        var pointFrom = kanheightPoints[i];
                        var pointTo = kanheightPoints[i + 1];
                        var line = CreateLineHeight(pointFrom.X, pointFrom.Y, pointTo.X, pointTo.Y);

                        pointFrom = line.From;

                        if (pointFrom.X > maxX || pointFrom.Y < RootAxis.Y)
                        {
                            break;
                        }
                        pointTo = line.To;
                        if (pointTo.X > maxX)
                        {

                            double y = (maxX - pointFrom.X) / (pointTo.X - pointFrom.X) * (pointTo.Y - pointFrom.Y) + pointFrom.Y;
                            pointTo.X = maxX;
                            pointTo.Y = y;
                        }

                        if (pointTo.Y < RootAxis.Y)
                        {
                            double x = (maxX - pointFrom.X) * (RootAxis.Y - pointFrom.Y) / (pointTo.Y - pointFrom.Y) + pointFrom.X;
                            pointTo.X = x;
                            pointTo.Y = RootAxis.Y;
                        }

                        line.To = pointTo;

                        if (pointFrom.X >= 0 && pointFrom.Y >= 0 && pointTo.X >= 0 && pointTo.Y >= 0)
                        {
                            listDrawLinePerPage.Add(new(pointFrom.X, pointFrom.Y, pointTo.X, pointTo.Y, 40, "Solid", "Red"));
                        }

                        if (i == 0)
                        {
                            if (pointFrom.X >= 0 && pointFrom.Y >= 0)
                            {
                                listDrawBoxPerPage.Add(new((int)(line.From.X - 100), (int)(line.From.Y - 100), 200, 200, 0, "Red", "Red"));
                            }
                            if (line.To.X >= 0 && line.To.Y >= 0)
                            {
                                listDrawBoxPerPage.Add(new((int)(line.To.X - 100), (int)(line.To.Y - 100), 200, 200, 0, "Red", "Red"));
                            }
                        }
                        else
                        {
                            if (line.To.X >= 0 && line.To.Y >= 0)
                            {
                                listDrawBoxPerPage.Add(new((int)(line.To.X - 100), (int)(line.To.Y - 100), 200, 200, 0, "Red", "Red"));
                            }
                        }
                    }

                }
            }
            _listDrawKanLineCircleData.Add(pageIndex, listDrawCirclePerPage);
            _listDrawKanLineBoxData.Add(pageIndex, listDrawBoxPerPage);
        }

        protected void DrawRectangleCanvas()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawLinePerPage = new()
            {
                new(RootAxis.X, RootAxis.Y, RootAxis.X + CanvasWidth, RootAxis.Y),
                new(RootAxis.X, RootAxis.Y, RootAxis.X, RootAxis.Y + CanvasHeight),
                //CoRep.
                new(RootAxis.X, RootAxis.Y + CanvasHeight, RootAxis.X + CanvasWidth, RootAxis.Y + CanvasHeight),
                new(RootAxis.X + CanvasWidth, RootAxis.Y, RootAxis.X + CanvasWidth, RootAxis.Y + CanvasHeight)
            };
            _listDrawRectangleCanvasLineData.Add(pageIndex, listDrawLinePerPage);
        }

        protected void DrawLegendLabel()
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            listDrawTextPerPage = new();
            listDrawBoxPerPage = new();
            listDrawCirclePerPage = new();
            maxX = Convert.ToInt64(RootAxis.X) + partXCount * PartWidth;
            if (GrowthCurveConfig.WeightVisible && GrowthCurveConfig.HeightVisible)
            {
                listDrawBoxPerPage.Add(new((int)(RootAxis.X + 400), (int)(RootAxis.Y + 400), 1300, 1000, 0, "White", "Black"));
                listDrawBoxPerPage.Add(new((int)RootAxis.X + 600, (int)RootAxis.Y + 600, 200, 200, 0, "Red", "Red"));
                listDrawCirclePerPage.Add(new(RootAxis.X + 700, RootAxis.Y + 1100, 120, 120, "Blue", "Blue"));
                listDrawTextPerPage.Add(new(RootAxis.X + 900, RootAxis.Y + 550, 1000, 300, 300, "身長"));
                listDrawTextPerPage.Add(new(RootAxis.X + 900, RootAxis.Y + 950, 1000, 300, 300, "体重"));
            }
            else
            {
                listDrawBoxPerPage.Add(new((int)RootAxis.X + 400, (int)RootAxis.Y + 400, 1300, 600, 0, "White", "Black"));
                if (GrowthCurveConfig.HeightVisible)
                {
                    listDrawBoxPerPage.Add(new((int)RootAxis.X + 600, (int)RootAxis.Y + 600, 200, 200, 0, "Red", "Red"));
                    listDrawTextPerPage.Add(new(RootAxis.X + 900, RootAxis.Y + 550, 1000, 300, 300, "身長"));
                }
                else if (GrowthCurveConfig.WeightVisible)
                {
                    listDrawCirclePerPage.Add(new(RootAxis.X + 700, RootAxis.Y + 700, 120, 120, "Blue", "Blue"));
                    listDrawTextPerPage.Add(new(RootAxis.X + 900, RootAxis.Y + 550, 1000, 300, 300, "体重"));
                }
            }
            _listDrawLegendLabelBoxData.Add(pageIndex, listDrawBoxPerPage);
            _listDrawLegendLabelCircleData.Add(pageIndex, listDrawCirclePerPage);
            _listDrawLegendLabelTextData.Add(pageIndex, listDrawTextPerPage);
        }

        private double CalculateAdjY()
        {
            double rs = 1;
            double max = heightMaxY - heightMinY;
            if (max > 0 && CanvasHeight > 0)
            {
                rs = CanvasHeight / max;
            }

            if (rs <= 0) rs = 1;

            return rs;
        }

        private int GetCountPointX()
        {
            int countPoint;
            int scope = GrowthCurveConfig.Scope;
            switch (scope)
            {
                case 1:
                    countPoint = 12;
                    break;
                case 2:
                    countPoint = 24;
                    break;
                case 3:
                    countPoint = 36;
                    break;
                default:
                    countPoint = intervalX * scope;
                    break;
            }
            return countPoint;
        }

        private Line CreateLineWeight(double point1, double value1, double point2, double value2)
        {
            var line = new Line();

            double pY = RootAxis.Y + (PartYCount + weightMinY) * PartHeight;

            var pointModel1 = new Point((int)(RootAxis.X + point1 * PartWidth / intervalX), (int)(pY - value1 * PartHeight));
            var pointModel2 = new Point((int)(RootAxis.X + point2 * PartWidth / intervalX), (int)(pY - value2 * PartHeight));

            if (GrowthCurveConfig.Scope > 1)
            {
                pointModel1 = new Point((int)(RootAxis.X + point1 * PartWidth / intervalX), (int)(pY - value1 * PartHeight / intervalY));
                pointModel2 = new Point((int)(RootAxis.X + point2 * PartWidth / intervalX), (int)(pY - value2 * PartHeight / intervalY));
            }

            line.From = pointModel1;
            line.To = pointModel2;
            return line;
        }

        private Line CreateLineHeight(double point1, double value1, double point2, double value2)
        {
            var lineModel = new Line();

            double pY = RootAxis.Y + (PartYCount + heightMinY / intervalY) * PartHeight;

            var pointModel1 = new Point((int)(RootAxis.X + point1 * PartWidth), (int)(pY - value1 * PartHeight / 5));
            var pointModel2 = new Point((int)(RootAxis.X + point2 * PartWidth), (int)(pY - value2 * PartHeight / 5));

            if (GrowthCurveConfig.Scope > 1)
            {
                pointModel1 = new Point((int)(RootAxis.X + point1 * PartWidth / intervalX), (int)(pY - value1 * PartHeight / intervalY));
                pointModel2 = new Point((int)(RootAxis.X + point2 * PartWidth / intervalX), (int)(pY - value2 * PartHeight / intervalY));
            }

            lineModel.From = pointModel1;
            lineModel.To = pointModel2;
            return lineModel;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
    }
}
