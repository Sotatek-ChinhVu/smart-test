using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IAmazonS3Service _amazonS3Service;

    public FileController(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    [HttpPost("Upload")]
    public async Task<Response<string>> UploadAsync([FromQuery] string subFolder, [FromQuery] string fileName)
    {
        var accessUrl = await _amazonS3Service.UploadAnObjectAsync(true, subFolder, fileName, Request.Body);
        if (string.IsNullOrEmpty(accessUrl))
        {
            return new Response<string>
            {
                Status = 0,
                Data = string.Empty,
                Message = ResponseMessage.Failed
            };
        }

        return new Response<string>
        {
            Status = 1,
            Data = accessUrl,
            Message = ResponseMessage.Success
        };
    }

    [HttpGet("Exists")]
    public async Task<Response<bool>> ExistsAsync([FromQuery] string key)
    {
        var exists = await _amazonS3Service.ObjectExistsAsync(key);

        return new Response<bool>
        {
            Status = exists ? 1 : 0,
            Data = exists,
            Message = exists ? ResponseMessage.Success : ResponseMessage.NotFound
        };
    }
}
