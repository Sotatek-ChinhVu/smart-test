using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IAmazonS3Service _amazonS3Service;

    public FileController(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    [HttpPost("Upload")]
    public async Task<Response<string>> UploadAsync([FromQuery] string fileName)
    {
        var accessUrl = await _amazonS3Service.UploadAnObjectAsync(fileName, Request.Body);
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

    [HttpGet("GetAll")]
    public async Task<Response<List<string>>> Get([FromQuery] string key)
    {
        var data = await _amazonS3Service.GetListObjectAsync(key);


        return new Response<List<string>>
        {
            Status = data != null ? 1 : 0,
            Data = data ?? new List<string>(),
            Message = data != null ? ResponseMessage.Success : ResponseMessage.NotFound
        };
    }
}
