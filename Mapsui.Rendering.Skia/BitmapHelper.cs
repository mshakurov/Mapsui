using System.IO;
using Mapsui.Extensions;
using Mapsui.Utilities;
using SkiaSharp;

namespace Mapsui.Rendering.Skia;

public static class BitmapHelper
{
    public static BitmapInfo? LoadBitmap(Stream stream)
    {
        if (stream.IsSvg())
        {
            return new BitmapInfo { Svg = new SvgWithStream(stream.LoadSvg(), stream) };
        }

        using var skData = SKData.CreateCopy(stream.ToBytes());
        var image = SKImage.FromEncodedData(skData);
        return new BitmapInfo { Bitmap = image };

    }

    public static bool InvalidBitmapInfo(BitmapInfo bitmapInfo)
    {
        return bitmapInfo.IsDisposed || bitmapInfo.Bitmap == null;
    }
}
