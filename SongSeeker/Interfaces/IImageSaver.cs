namespace SongSeeker.Interfaces;

public interface IImageSaver
{
    void SaveImage(float[,] data, string outputPath, int imageWidth, int imageHeight, int sampleRate, int hopSize);
}