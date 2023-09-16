﻿using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Enum;
using Microsoft.Extensions.Configuration;
using Reporting.Kensalrai.Service;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;
using UseCase.MainMenu.KensaIraiReport;

namespace Interactor.MainMenu;

public class KensaIraiReportInteractor : IKensaIraiReportInputPort
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly IKensaIraiCoReportService _kensaIraiCoReportService;
    private readonly IConfiguration _configuration;

    public KensaIraiReportInteractor(IKensaIraiRepository kensaIraiRepository, IKensaIraiCoReportService kensaIraiCoReportService, IConfiguration configuration)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _kensaIraiCoReportService = kensaIraiCoReportService;
        _configuration = configuration;
    }

    public KensaIraiReportOutputData Handle(KensaIraiReportInputData inputData)
    {
        try
        {
            var kensaIraiList = AsKensaIraiReportModel(inputData.KensaIraiList);
            List<string> data = _kensaIraiCoReportService.GetIraiFileData(inputData.CenterCd, kensaIraiList);
            var datFileByte = data.SelectMany(item => Encoding.UTF8.GetBytes(item + Environment.NewLine)).ToArray();
            string datFile = Convert.ToBase64String(datFileByte, Base64FormattingOptions.InsertLineBreaks);

            var outputPrint = _kensaIraiCoReportService.GetKensalraiData(inputData.HpId, inputData.SystemDate, inputData.FromDate, inputData.ToDate, inputData.CenterCd, kensaIraiList);
            var waitResult = ReturnPDF(outputPrint);
            waitResult.Wait();
            var pdfFileByte = waitResult.Result;
            string pdfFile = Convert.ToBase64String(pdfFileByte, Base64FormattingOptions.InsertLineBreaks);

            return new KensaIraiReportOutputData(pdfFile, datFile, KensaIraiReportStatus.Successed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }

    #region private function
    private List<Reporting.Kensalrai.Model.KensaIraiModel> AsKensaIraiReportModel(List<KensaIraiModel> source)
    {
        List<Reporting.Kensalrai.Model.KensaIraiModel> kensaIraiList = new();
        foreach (var kensaIrai in source)
        {
            kensaIraiList.Add(new Reporting.Kensalrai.Model.KensaIraiModel(
                kensaIrai.SinDate,
                kensaIrai.RaiinNo,
                kensaIrai.IraiCd,
                kensaIrai.PtId,
                kensaIrai.PtNum,
                kensaIrai.Name,
                kensaIrai.KanaName,
                kensaIrai.Sex,
                kensaIrai.Birthday,
                kensaIrai.TosekiKbn,
                kensaIrai.SikyuKbn,
                kensaIrai.KaId,
                AsKensaIraiDetailReportModel(kensaIrai.KensaIraiDetails)));
        }
        return kensaIraiList;
    }

    private List<Reporting.Kensalrai.Model.KensaIraiDetailModel> AsKensaIraiDetailReportModel(List<KensaIraiDetailModel> source)
    {
        List<Reporting.Kensalrai.Model.KensaIraiDetailModel> kensaIraiDetails = new();
        foreach (var kensaDetail in source)
        {
            KensaMst kensaMst = new();
            kensaMst.KensaItemCd = kensaDetail.KensaItemCd;
            kensaMst.CenterItemCd1 = kensaDetail.CenterItemCd;
            kensaMst.KensaKana = kensaDetail.KensaKana;
            kensaMst.KensaName = kensaDetail.KensaName;
            kensaMst.ContainerCd = (int)kensaDetail.ContainerCd;
            kensaIraiDetails.Add(new Reporting.Kensalrai.Model.KensaIraiDetailModel(true, kensaDetail.RpNo, kensaDetail.RpEdaNo, kensaDetail.RowNo, kensaDetail.SeqNo, kensaMst));
        }
        return kensaIraiDetails;
    }

    private async Task<byte[]> ReturnPDF(object data)
    {
        StringContent jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        string functionName = "common-reporting";

        using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}{functionName}", jsonContent))
        {
            response.EnsureSuccessStatusCode();
            using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
            {
                var byteData = streamingData.ToArray();
                return byteData;
            }
        }
    }
    #endregion
}
