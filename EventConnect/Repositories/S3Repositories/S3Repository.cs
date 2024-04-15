using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using EventConnect.Models.Aws.Credentail;
using EventConnect.Models.S3Models;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using S3Object = EventConnect.Models.S3Models.S3Object;

// Namespace for the S3 repositories
namespace EventConnect.Repositories.S3Repositories
{
    // Class implementing the IS3Repository interface
    public class S3Repository:IS3Repository
    {
        // Private fields for the configuration, AWS credentials, and S3 configuration
        private readonly IConfiguration _configuration;
        private readonly AwsCredentials _credentials;
        private readonly AmazonS3Config _config;

        // Constructor that takes the configuration as a parameter
        public S3Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            
            // Initialize the AWS credentials from the configuration
            _credentials = new AwsCredentials()
            {
                AwsKey = _configuration["AwsConfiguration:AWSAccessKey"],
                AwsSecretKey = _configuration["AwsConfiguration:AWSSecretKey"],
            };

            // Initialize the S3 configuration
            _config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.EUNorth1
            };
        }

        // Method for uploading a file to S3
        public async Task<S3ResponseDto> UploadFileAsync(TransferUtilityUploadRequest uploadRequest)
        {
            // Initialize the response object
            var response = new S3ResponseDto();
            try
            {
                // Create the AWS credentials
                var credentials = new BasicAWSCredentials(_credentials.AwsKey, _credentials.AwsSecretKey);
       
                // Create the S3 client
                using var client = new AmazonS3Client(credentials, _config);

                // Create the transfer utility
                var transferUtility = new TransferUtility(client);

                // Upload the file to S3
                await transferUtility.UploadAsync(uploadRequest);

                // Set the response status code and message
                response.StatusCode = 200;
                response.Message = $"{uploadRequest.Key}";
            }
            catch (AmazonS3Exception exception)
            {
                // If an S3 exception occurs, set the response status code and message
                response.StatusCode = (int)exception.StatusCode;
                response.Message = exception.Message;
            }
            catch (Exception e)
            {
                // If a general exception occurs, set the response status code to 500 and the message to the exception message
                response.StatusCode = 500;
                response.Message = e.Message;
            }

            // Return the response
            return response;
        }

        // Method for getting a file from S3
        public async Task<object> GetFileAsync(string fileName)
        {
            // Create the AWS credentials
            var credentials = new BasicAWSCredentials(_credentials.AwsKey, _credentials.AwsSecretKey);
            try
            {
                // Create the S3 client
                using var client = new AmazonS3Client(credentials, _config);

                // Create the get object request
                var request = new GetObjectRequest
                {
                    BucketName =  _configuration["AwsConfiguration:AWSBucketName"],
                    Key = fileName
                };

                // Get the object from S3
                var response = await client.GetObjectAsync(request);

                // Return the response stream
                return response.ResponseStream;
            }
            catch (AmazonS3Exception e)
            {
                // If an S3 exception occurs, log the message and return null
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // Method for deleting a file from S3
        public async Task<string> DeleteFileAsync(string fileName)
        {
            // Create the AWS credentials
            var credentials = new BasicAWSCredentials(_credentials.AwsKey, _credentials.AwsSecretKey);
            try
            {
                // Create the S3 client
                using var client = new AmazonS3Client(credentials, _config);

                // Create the delete object request
                var request = new DeleteObjectRequest()
                {
                    BucketName =  _configuration["AwsConfiguration:AWSBucketName"],
                    Key = fileName
                };

                // Delete the object from S3
                var response = await client.DeleteObjectAsync(request);

                // Log the response and return a success message
                Console.WriteLine(response);
                return "File deleted successfully.";
            }
            catch (AmazonS3Exception e)
            {
                // If an S3 exception occurs, log the message and return null
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}