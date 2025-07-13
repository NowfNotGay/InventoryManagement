using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Method;
public class CloudDinaryHelper
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;

    public CloudDinaryHelper(IConfiguration configuration)
    {
        
        _configuration = configuration.GetSection("CloudinarySettings");
        var account = new Account(_configuration["CloudName"], _configuration["ApiKey"], _configuration["ApiSecret"]);
        _cloudinary = new Cloudinary(account);

    }

    public async Task<(string Url, string PublicId)> UploadImageAsync(IFormFile file, string PublicId = null, string folderImg = "default_folder")
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folderImg,
            UniqueFilename = false,
            PublicId = PublicId,
            Overwrite = true
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return (result.SecureUrl.ToString(), result.PublicId); // PublicId cần để xóa
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result == "ok";
    }


}
