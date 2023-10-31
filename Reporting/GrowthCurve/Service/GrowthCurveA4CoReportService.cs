using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.GrowthCurve.DB;
using Reporting.GrowthCurve.Mapper;
using Reporting.GrowthCurve.Model;
using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Service;

public class GrowthCurveA4CoReportService : GrowthCurveService, IGrowthCurveA4CoReportService
{
    private readonly string fileName = "GrowthCurve_A4.rse";
    private readonly ISpecialNoteFinder _specialNoteFinder;
    private List<PointModel> kanWeightPointCollection = new();
    private List<PointModel> kanHeightPointCollection = new();
    public GrowthCurveA4CoReportService(ISpecialNoteFinder specialNoteFinder)
    {
        _specialNoteFinder = specialNoteFinder;
        CanvasWidth = 17800;
        CanvasHeight = 24600;
    }

    public CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig)
    {
        GrowthCurveConfig = growthCurveConfig;

        SetParamByScope(growthCurveConfig.Scope);
        List<GcStdInfModel> gcStdInfCollection = _specialNoteFinder.GetStdPoint(hpId);
        GetKanInfRst(growthCurveConfig.PtId, growthCurveConfig.BirthDay);
        foreach (var item in gcStdInfCollection)
        {
            AddGcStdInfCollection(item.GcStdMst);
        }

        //KanLine
        foreach (var kanweight in kanWeightPointCollection)
        {
            AddKanWeightPoint(kanweight.X, kanweight.Y);
        }

        foreach (var kanheight in kanHeightPointCollection)
        {
            AddKanHeightPoint(kanheight.X, kanheight.Y);
        }

        for (int i = 1; i < PartYCount; i++)
        {
            AddHorizotationLine(i);
        }

        for (int i = 1; i < partXCount; i++)
        {
            AddVerticalLine(i);
        }
        UpdateDrawForm();

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

        _extralData.Add("totalPage", "1");
        return new GrowthCurveMapper(_setFieldData, _listTextData, _extralData, fileName, _singleFieldData, _visibleFieldData, _listDrawLineData, _listDrawTextData, _listDrawBoxData, _listDrawCircleData, "成長曲線_A4").GetData();
    }

    private void UpdateDrawForm()
    {
        //Fill info
        FillBaseInfo();
        DrawRectangleCanvas();
        DrawHorizotationLines();
        DrawVerticalLines();
        DrawCanvasBound();
    }

    private void DrawCanvasBound()
    {
        DrawLegendLabel();

        CreateLabels();
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

    private void GetKanInfRst(long ptId, int birthDay)
    {
        var kensaInfDetails = _specialNoteFinder.GetKensaInf(ptId, -1, -1, "");
        kanWeightPointCollection = new();
        kanHeightPointCollection = new();

        //weigth
        var weightInfo = kensaInfDetails.Where(item => item.KensaItemCd == IraiCodeConstant.WEIGHT_CODE).OrderBy(item => item.IraiDate).ToList();
        foreach (var weightItem in weightInfo)
        {
            int age = 0;
            int month = 0;
            int day = 0;
            CIUtil.SDateToDecodeAge(birthDay, weightItem.IraiDate, ref age, ref month, ref day);
            bool lessThan18 = age <= 18;

            if (lessThan18)
            {
                double pointX = age * 12 + month + ((double)day / 30);
                kanWeightPointCollection.Add(new PointModel(pointX, (weightItem?.ResultVal ?? "0").AsDouble()));
            }
        }

        //height
        var heightInfo = kensaInfDetails.Where(item => item.KensaItemCd == IraiCodeConstant.HEIGHT_CODE).OrderBy(item => item.IraiDate).ToList();
        foreach (var heightItem in heightInfo)
        {
            int age = 0;
            int month = 0;
            int day = 0;
            CIUtil.SDateToDecodeAge(birthDay, heightItem.IraiDate, ref age, ref month, ref day);

            bool lessThan18 = age <= 18;

            if (lessThan18)
            {
                double pointX = age * 12 + month + ((double)day / 30);
                kanHeightPointCollection.Add(new PointModel(pointX, (heightItem?.ResultVal ?? "0").AsDouble()));
            }
        }
    }
}
