using RollerCoaster.Services.Abstractions;
using SkiaSharp;

namespace RollerCoaster.Services.Implementations;

public class ImageCellPainter: IImageCellPainter
{
    public Task<Stream> DrawCell(Stream image, int width, int height) 
    {
        using var bm = SKBitmap.Decode(image);
        using var canvas = new SKCanvas(bm);
        
        using var paint = new SKPaint();
        paint.Style = SKPaintStyle.Stroke;
        paint.Color = SKColors.Black;
        paint.StrokeWidth = 3;

        for (float currentX = 0; currentX < bm.Width; currentX += (float) bm.Width / width)
            canvas.DrawLine(currentX, 0, currentX, bm.Height - 1, paint);    
        for (float currentY = 0; currentY < bm.Height; currentY += (float) bm.Height / height)
            canvas.DrawLine(0, currentY, bm.Width - 1, currentY, paint);
        
        using var data = bm.Encode(SKEncodedImageFormat.Png, 100);

        var stream = new MemoryStream();
        data.SaveTo(stream);
        stream.Position = 0;

        return Task.FromResult((Stream) stream);
    }
}