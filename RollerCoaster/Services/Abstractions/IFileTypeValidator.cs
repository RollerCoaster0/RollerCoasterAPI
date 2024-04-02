namespace RollerCoaster;

public interface IFileTypeValidator
{
    bool ValidateImageFileType(IFormFile file);
}