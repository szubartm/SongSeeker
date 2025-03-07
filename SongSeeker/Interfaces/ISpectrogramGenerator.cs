namespace SongSeeker.Interfaces;

public interface ISpectrogramGenerator
{
    float[,] CreateSpectogram(float[] samples, int windowSize, int hopSize, int sampleRate);
}