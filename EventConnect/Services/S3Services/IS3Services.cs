using EventConnect.Models.Aws.Credentail;
using EventConnect.Models.S3Models;

namespace EventConnect.Services.S3Services;

public interface IS3Services
{
    Task<S3ResponseDto> UploadFileAsync(S3Object s3Object, string userId);
    Task<Object> GetFileAsync( string userId);
    Task<string> DeleteFileAsync(string userId);
}