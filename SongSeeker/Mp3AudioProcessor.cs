using NAudio.Wave;
using SongSeeker.Interfaces;

namespace SongSeeker;

public class AudioProcessor : IAudioProcessor
{
    public float[] ReadAudioSamples(string filepath)
    {
        using (var audiofile = new AudioFileReader(filepath))
        {
            var samples = new float[audiofile.Length];
            audiofile.Read(samples, 0, samples.Length);
            return samples;
        }
    }
}