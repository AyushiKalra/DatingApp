using System.Diagnostics.CodeAnalysis;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        //accessing our strongly typed class for our cloudinary configuration
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.APIKey,
                config.Value.APISecret
            );
            _cloudinary = new Cloudinary(acc);//Initializes a new instance of the Cloudinary class with Cloudinary account.
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            //IFormFile : Represents a file sent with the HttpRequest.
            var uploadResult = new ImageUploadResult();
            if(file.Length >0)
            {
                /*we are using the using statement here so that the file is going to be disposed of and cleaned out from  
                memory as soon as we are finished with this method because this is going to consume memory.*/
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams{
                    File = new FileDescription(file.FileName, stream),
                    //image will be uploaded as square, hence transforming the image acc to our needs.
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net7"
                    };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}