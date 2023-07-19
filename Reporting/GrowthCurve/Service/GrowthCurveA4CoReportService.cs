using Reporting.GrowthCurve.Mapper;
using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service
{
    public class GrowthCurveA4CoReportService : GrowthCurveService, IGrowthCurveCoReportService
    {
        string fileName = "GrowthCurve_A4.rse";
        private int hpId;
        private bool hasNextPage;
        private int currentPage;
        private GrowthCurveConfig _growthCurveConfig;

        public GrowthCurveA4CoReportService()
        {
            CanvasWidth = 17800;
            CanvasHeight = 24600;
        }
         
        public CommonReportingRequestModel GetGrowthCurvePrintData(int hpId, GrowthCurveConfig growthCurveConfig)
        {
            this.hpId = hpId;
            _growthCurveConfig = growthCurveConfig;
            GrowthCurveConfig = _growthCurveConfig;
            
            hasNextPage = true;
            currentPage = 1;

            while (hasNextPage)
            {
                UpdateDrawForm();
                currentPage++;
            }

            _listDrawTextData.Add(1, _listCreateLabelsTextData);
            _listDrawTextData.Add(2, _listCreateStdInfPercentTextData);
            _listDrawTextData.Add(3, _listCreateStdInfSDTextData);
            _listDrawTextData.Add(4, _listDrawLegendLabelTextData);
            _listDrawLineData.Add(1, _listDrawHorizotationLineData);
            _listDrawLineData.Add(2, _listDrawVerticalLineData);
            _listDrawLineData.Add(3, _listCreateStdInfPercentLineData);
            _listDrawLineData.Add(4, _listCreateStdInfSDLineData);
            _listDrawLineData.Add(5, _listDrawRectangleCanvasLineData);
            _listDrawBoxData.Add(1, _listDrawKanLineBoxData);
            _listDrawBoxData.Add(2, _listDrawLegendLabelBoxData);
            _listDrawCircleData.Add(1, _listDrawKanLineCircleData);
            _listDrawCircleData.Add(2, _listDrawLegendLabelCircleData);

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new GrowthCurveMapper(_setFieldData, _listTextData, _extralData, fileName, _singleFieldData, _visibleFieldData, _listDrawLineData, _listDrawTextData, _listDrawBoxData, _listDrawCircleData).GetData();
        }

        private bool UpdateDrawForm()
        {
            //Fill info
            FillBaseInfo();
            DrawRectangleCanvas();
            AddHorizotationLine();
            DrawHorizotationLines();
            DrawVerticalLines();
            DrawCanvasBound();
            return hasNextPage = false;
        }

        private void DrawCanvasBound()
        {
            //DrawRectangleCanvas();

            DrawLegendLabel();

            CreateLabels();
            //
            if (GrowthCurveConfig.PrintMode == 1)
            {
                CreateStdInfPercent();
            }
            else
            {
                CreateStdInfSD();
            }
            DrawKanLine();
        }


    }
}
