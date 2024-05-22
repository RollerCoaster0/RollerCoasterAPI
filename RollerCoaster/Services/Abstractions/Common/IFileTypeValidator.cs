namespace RollerCoaster.Services.Abstractions.Common;

public interface IFileTypeValidator
{
    bool ValidateImageFileType(IFormFile file);
}