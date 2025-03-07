namespace SongSeeker.Interfaces;

public interface IImageSaver
{
    void SaveImage(float[,] data, string outputPath);
}