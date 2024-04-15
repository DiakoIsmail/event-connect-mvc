using EventConnect.Models.S3Models;
using Amazon.S3;
using Amazon.S3.Transfer;
using EventConnect.Data.Auth;
using EventConnect.Entities;
using EventConnect.Repositories.S3Repositories;
using S3Object = EventConnect.Models.S3Models.S3Object;

// Namespace for the S3 services
namespace EventConnect.Services.S3Services
{
    // Class implementing the IS3Services interface
    public class S3Services: IS3Services
    {
        // Private fields for the S3 and Auth repositories
        private readonly IS3Repository _s3Repository;
        private readonly IAuthRepository _authRepository;

        // Constructor that takes the S3 and Auth repositories as parameters
        public S3Services(IS3Repository s3Repository, IAuthRepository authRepository)
        {
            _s3Repository = s3Repository;
            _authRepository = authRepository;
        }

        // Method for uploading a file to S3
        public async Task<S3ResponseDto> UploadFileAsync(S3Object s3Object,string userId)
        {
            // Initialize the response object
            S3ResponseDto response = new S3ResponseDto();
            try
            {
                // Get the user by ID
                User user =  await _authRepository.GetUserById(userId);
                // If the user's ImageKey length is greater than 5, delete the file from S3
                if (user.ImageKey.Length > 5)
                { 
                    await _s3Repository.DeleteFileAsync(user.ImageKey);
                }

                // Create the upload request
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = s3Object.InputStream,
                    Key = s3Object.Name,
                    BucketName = s3Object.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };
                
                // Upload the file to S3
                response = await _s3Repository.UploadFileAsync(uploadRequest);
            
                // Update the user's ImageKey and save the changes
                user.ImageKey = response.Message;
                await _authRepository.UpdateUser(user);
            }
            catch (Exception e)
            {
                // If an error occurs, set the response status code to 500 and the message to the error message
                response.StatusCode = 500;
                response.Message = e.Message;
            }

            // Return the response
            return response;
        }

        // Method for getting a file from S3
        public async Task<Object> GetFileAsync(string userId)
        { 
            // Get the user by ID
            User user =  await _authRepository.GetUserById(userId);
            // Return the file from S3
            return await _s3Repository.GetFileAsync(user.ImageKey);
        }

        // Method for deleting a file from S3
        public async Task<string> DeleteFileAsync(string userId)
        {
            // Get the user by ID
            User user =  await _authRepository.GetUserById(userId);
            //cheek if the user has an image key
            if (user.ImageKey.Length < 5)
            {
                return "No image to delete";
            }
            
            // Delete the file from S3
            string response = await _s3Repository.DeleteFileAsync(user.ImageKey);
        
            // Update the user's ImageKey to an empty string and save the changes
            user.ImageKey = "";
            await _authRepository.UpdateUser(user);

            // Return the response
            return response;
        }
    }
}