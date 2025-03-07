namespace SongSeeker.Interfaces;

public interface IAudioProcessor
{
    float[] ReadAudioSamples(string filepath);
}