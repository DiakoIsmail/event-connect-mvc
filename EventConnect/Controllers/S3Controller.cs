using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using EventConnect.Models.Aws.Credentail;
using EventConnect.Services.S3Services;
using Microsoft.AspNetCore.Mvc;
using S3Object = EventConnect.Models.S3Models.S3Object;

// Namespace for the controller
namespace EventConnect.Controllers
{
    // Decorators to define this class as a controller and set its route
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        // Private field for the S3 service
        private readonly IS3Services _storageService;

        // Constructor that takes the S3 service as a parameter
        public S3Controller(IS3Services storageService)
        {
            _storageService = storageService;
        }

        // Endpoint for uploading a file to S3
        [HttpPost("UploadFil/{userId}")]
        public async Task<IActionResult> UploadFile(IFormFile file, string userId)
        {
            // Create a memory stream and copy the file into it
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            // Get the file extension and generate a unique name for the S3 object
            var fileExtension = Path.GetExtension(file.FileName);
            var objName = $"{Guid.NewGuid()}.{fileExtension}";

            // Create the S3 object
            var s3Object = new S3Object()
            {
                BucketName = "image-bucket-event-connect",
                InputStream = memoryStream,
                Name = objName,
            };

            // Upload the file to S3 and return the result
            var result = await _storageService.UploadFileAsync(s3Object, userId);
            return Ok(result);
        }

        // Endpoint for getting a file from S3
        [HttpGet("GetFile/{userId}")]
        public async Task<IActionResult> GetFile(string userId)
        {
            // Get the file from S3 and return the result
            var result = await _storageService.GetFileAsync(userId);
            return Ok(result);
        }

        // Endpoint for deleting a file from S3
        [HttpDelete("DeleteFile/{userId}")]
        public async Task<IActionResult> DeleteFile(string userId)
        {
            // Delete the file from S3 and return the result
            var result = await _storageService.DeleteFileAsync(userId);
            return Ok(result);
        }
    }
}