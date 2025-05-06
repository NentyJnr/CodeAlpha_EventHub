namespace EventHub.Contracts
{
    public interface IUploadHelper
    {
        Task<string> UploadImage(IFormFile imageFile, bool isForRegistration = false);
    }
}
