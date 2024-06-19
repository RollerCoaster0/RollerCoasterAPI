namespace RollerCoaster.Services.Abstractions;

public interface IImageCellPainter
{
    Task<Stream> DrawCell(Stream image, int width, int height);
}