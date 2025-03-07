namespace SongSeeker.Interfaces;

public interface ISpectrogramGenerator
{
    float[,] CreateSpectrogram(float[] samples, int windowSize, int hopSize, int sampleRate);
}