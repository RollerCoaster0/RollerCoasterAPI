namespace RollerCoaster;

public class FileTypeValidator: IFileTypeValidator
{
    private readonly Dictionary<string, List<byte[]>> _fileSignatures = 
        new()
        {
            ["jpeg"] = [
                [0xFF, 0xD8, 0xFF, 0xE0],
                [0xFF, 0xD8, 0xFF, 0xE2],
                [0xFF, 0xD8, 0xFF, 0xE3]
            ],
            ["png"] = [
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]
            ],
            ["jpg"] = [
                [0xFF, 0xD8, 0xFF, 0xE0],
                [0xFF, 0xD8, 0xFF, 0xE1],
                [0xFF, 0xD8, 0xFF, 0xE8],
                [0xFF, 0xD8, 0xFF, 0xEE],
                [0xFF, 0xD8, 0xFF, 0xDB]
            ]
        };

    public bool ValidateImageFileType(IFormFile file)
    {
        using var reader = new BinaryReader(file.OpenReadStream());

        if (!file.FileName.Contains('.'))
            return false;

        string ext = file.FileName.Split(".").Last().ToLower();

        if (!_fileSignatures.ContainsKey(ext))
            return false;
        
        var signatures = _fileSignatures[ext];
        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

        return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
    }
}