using System.Drawing;
using System.Drawing.Drawing2D;
using SongSeeker.Interfaces;

namespace SongSeeker;

public class SpectrogramImageSaver : IImageSaver
{
    private const int PaddingLeft = 50;
    private const int PaddingBottom = 30;
    private const int PaddingRight = 20;

    public void SaveImage(float[,] data, string outputPath, int imageWidth, int imageHeight, int sampleRate,
        int hopSize)
    {
        var spectrogramWidth = data.GetLength(0);
        var spectrogramHeight = data.GetLength(1);

        var totalWidth = imageWidth + PaddingLeft + PaddingRight;
        var totalHeight = imageHeight + PaddingBottom;

        using (var bitmap = new Bitmap(totalWidth, totalHeight))
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);

            var minDB = data.Cast<float>().Min();
            var maxDB = data.Cast<float>().Max();
            var scale = 1.0f / (maxDB - minDB);

            DrawSpectrogram(graphics, data, imageWidth, imageHeight, spectrogramWidth, spectrogramHeight, minDB, scale);
            DrawFrequencyLabels(graphics, imageHeight, sampleRate);
            DrawTimeLabels(graphics, imageWidth, imageHeight, spectrogramWidth, sampleRate, hopSize);
            DrawColorBar(graphics, imageWidth, imageHeight, minDB, maxDB);

            bitmap.Save(outputPath);
        }
    }

    #region Private helpers

    private void DrawSpectrogram(Graphics graphics, float[,] data, int imageWidth, int imageHeight,
        int spectrogramWidth, int spectrogramHeight, float minDB, float scale)
    {
        for (var x = 0; x < imageWidth; x++)
        for (var y = 0; y < imageHeight; y++)
        {
            var spectrogramX = x * spectrogramWidth / imageWidth;
            var spectrogramY = y * spectrogramHeight / imageHeight;

            var dB = data[spectrogramX, spectrogramY];
            var normalizedValue = (dB - minDB) * scale;

            var color = MagnitudeToColor(normalizedValue);
            graphics.FillRectangle(new SolidBrush(color), x + PaddingLeft, imageHeight - y - 1, 1, 1);
        }
    }

    private void DrawFrequencyLabels(Graphics graphics, int imageHeight, int sampleRate)
    {
        var font = new Font("Arial", 8);
        var brush = new SolidBrush(Color.Black);

        var nyquistFrequency = sampleRate / 2.0f;

        for (var i = 0; i <= 10; i++)
        {
            var frequency = i * nyquistFrequency / 10;
            var y = imageHeight - (int)(i * imageHeight / 10.0f);

            var label = $"{frequency / 1000:0.0} kHz";
            graphics.DrawString(label, font, brush, 5, y - 10);
        }
    }

    private void DrawTimeLabels(Graphics graphics, int imageWidth, int imageHeight, int spectrogramWidth,
        int sampleRate, int hopSize)
    {
        var font = new Font("Arial", 8);
        var brush = new SolidBrush(Color.Black);

        var totalDuration = spectrogramWidth * hopSize / (float)sampleRate;

        for (var i = 0; i <= 10; i++)
        {
            var time = i * totalDuration / 10;
            var x = PaddingLeft + (int)(i * imageWidth / 10.0f);

            var label = $"{time:0.0} s";
            graphics.DrawString(label, font, brush, x - 10, imageHeight + 5);
        }
    }

    private void DrawColorBar(Graphics graphics, int imageWidth, int imageHeight, float minDB, float maxDB)
    {
        var font = new Font("Arial", 8);
        var brush = new SolidBrush(Color.Black);

        for (var y = 0; y < imageHeight; y++)
        {
            var normalizedValue = y / (float)imageHeight;
            var color = MagnitudeToColor(normalizedValue);

            graphics.FillRectangle(new SolidBrush(color), imageWidth + PaddingLeft, imageHeight - y - 1, 20, 1);
        }

        graphics.DrawString($"{maxDB:0} dB", font, brush, imageWidth + PaddingLeft + 25, 5);
        graphics.DrawString($"{minDB:0} dB", font, brush, imageWidth + PaddingLeft + 25, imageHeight - 15);
    }

    private Color MagnitudeToColor(float value)
    {
        var r = Math.Min(1, Math.Max(0, 1.0f * value));
        float g = 0;
        var b = Math.Min(1, Math.Max(0, 1.0f * (1 - value)));

        return Color.FromArgb(
            (int)(255 * r),
            (int)(255 * g),
            (int)(255 * b)
        );
    }

    #endregion
}