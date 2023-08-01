using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineService.Request;
using OnlineService.Response;

namespace OnlineService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlController : ControllerBase
    {
        private CancellationToken _cancellationToken;

        [HttpPost("Get")]
        public ActionResult<GetXmlResponse> Get(GetXmlRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Start to get xml content");
                _cancellationToken = cancellationToken;
                if (string.IsNullOrEmpty(request.XmlFolderPath) ||
                    !Directory.Exists(request.XmlFolderPath))
                {
                    return new ActionResult<GetXmlResponse>(new GetXmlResponse());
                }

                for (int i = 0; i < 60; i++)
                {
                    if (IsCancel)
                    {
                        break;
                    }

                    var result = MonitorXmlFile(request.XmlFolderPath);
                    if (result != null)
                    {
                        return new ActionResult<GetXmlResponse>(result);
                    }
                    Thread.Sleep(1000);
                    Console.WriteLine("MonitorXmlFile");
                }

                return new ActionResult<GetXmlResponse>(new GetXmlResponse());
            }
            finally
            {
                Console.WriteLine("End to get xml content");
            }
        }

        private bool IsCancel => _cancellationToken.IsCancellationRequested;

        private GetXmlResponse? MonitorXmlFile(string xmlFolderPath)
        {
            var extensionList = new List<string> { "xml" };
            var xmlFileList = Directory
                .EnumerateFiles(xmlFolderPath, "*.*", SearchOption.AllDirectories)
                .Where(s => extensionList.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

            if (xmlFileList.Any())
            {
                GetXmlResponse getXmlResponse = new GetXmlResponse();
                foreach (var xmlFile in xmlFileList)
                {
                    var content = System.IO.File.ReadAllText(xmlFile);
                    getXmlResponse.FileList.Add(new XmlFileInfo()
                    {
                        Content = content,
                        Filename = xmlFile
                    });
                }
                return getXmlResponse;
            }
            return null;
        }
    }
}
