using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Document;
using EmrCloudApi.Requests.ReceiptCreation;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using UseCase.Core.Sync;
using UseCase.Document.DownloadDocumentTemplate;
using UseCase.ReceiptCreation.CreateUKEFile;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceiptCreationController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public ReceiptCreationController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.CreateUKEFile)]
        public IActionResult CreateUKEFile([FromBody] CreateUKEFileRequest request)
        {
            var input = new CreateUKEFileInputData(HpId, 
                                                    request.ModeType,
                                                    request.SeikyuYm,
                                                    request.SeikyuYmOutput, 
                                                    request.ChkHenreisai,
                                                    request.ChkTogetsu, 
                                                    request.IncludeOutDrug,
                                                    request.IncludeTester, 
                                                    request.KaId, 
                                                    request.DoctorId,
                                                    request.Sort,
                                                    request.SkipWarningIncludeOutDrug,
                                                    request.SkipWarningIncludeTester, 
                                                    request.SkipWarningKaId, 
                                                    request.SkipWarningDoctorId,
                                                    request.ConfirmCreateUKEFile,
                                                    UserId);
            var output = _bus.Handle(input);
            if (output.Status == CreateUKEFileStatus.Successful)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in output.UKEFiles)
                        {
                            var entry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                zipStream.Write(file.OutputStream, 0, file.OutputStream.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", "Archive.zip");
                }
            }  
            else
                return Ok(output);
        }
    }
}