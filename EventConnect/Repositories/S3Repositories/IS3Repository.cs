using Amazon.S3.Transfer;
using EventConnect.Models.Aws.Credentail;
using EventConnect.Models.S3Models;

namespace EventConnect.Repositories.S3Repositories;

public interface IS3Repository
{
    public Task<S3ResponseDto> UploadFileAsync(TransferUtilityUploadRequest uploadRequest);
    public Task<Object> GetFileAsync(string fileName );
    public Task<string> DeleteFileAsync(string fileName);
}