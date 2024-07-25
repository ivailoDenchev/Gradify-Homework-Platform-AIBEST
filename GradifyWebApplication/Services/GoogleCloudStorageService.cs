using Google.Cloud.Storage.V1;

namespace GradifyWebApplication.Services
{
    public class GoogleCloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
            _bucketName = "homeworkplatformdrive-429008.appspot.com";
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var objectName = $"{Guid.NewGuid()}_{fileName}";
                await _storageClient.UploadObjectAsync(_bucketName, objectName, null, fileStream);
                return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the file.", ex);
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                if (await FileExistsAsync(fileName))
                {
                    await _storageClient.DeleteObjectAsync(_bucketName, fileName);
                }
                else
                {
                    Console.WriteLine($"File {fileName} not found. Skipping delete operation.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the file.", ex);
            }
        }

        private async Task<bool> FileExistsAsync(string fileName)
        {
            try
            {
                var storageObject = await _storageClient.GetObjectAsync(_bucketName, fileName);
                return storageObject != null;
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the file exists.", ex);
            }
        }
    }
}
