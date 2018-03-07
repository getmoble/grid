using System;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Grid.Providers.Blob
{
    public class BlobUploadService
    {
        private static string RMSContainer = "rms";
        private static string IMSContainer = "ims";
        private static string HRMSContainer = "hrms";
        private static string PMSContainer = "pms";
        private static string RecruitContainer = "recruit";
        private static string InterviewRoundContainer = "interview";
        private static string ProfileContainer = "profile";
        private static string ArticleContainer = "articles";

        
        private static string account = "";
        private static string key = "";

        public BlobUploadService(BlobSettingsModel blobSettings)
        {
            account = blobSettings.AccountName;
            key = blobSettings.AccountKey;
        }

        public static CloudStorageAccount GetConnectionString()
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={account};AccountKey={key}";
            return CloudStorageAccount.Parse(connectionString);
        }

        public string UploadProfileImage(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ProfileContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadHRDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(HRMSContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadProjectDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(PMSContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadRecruitDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(RecruitContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadInterviewRoundDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(InterviewRoundContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadArticleAttachment(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ArticleContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadITDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(IMSContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }

        public string UploadRMSDocument(HttpPostedFileBase imageToUpload)
        {
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return "";
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(RMSContainer);
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                return cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string UploadAccessLogDocument(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                var cloudStorageAccount = GetConnectionString();
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("accesslog");
                cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Container);

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageToUpload.FileName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }
    }
}