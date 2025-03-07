using NAudio.Wave;
using SongSeeker.Interfaces;

namespace SongSeeker;

public class Mp3AudioProcessor : IAudioProcessor
{
    public float[] ReadAudioSamples(string filepath)
    {
        using (var reader = new Mp3FileReader(filepath))
        {
            var waveStream = WaveFormatConversionStream.CreatePcmStream(reader);
            var sampleProvider = waveStream.ToSampleProvider();
            var buffer = new float[waveStream.Length / 4];
            var sampleRead = sampleProvider.Read(buffer, 0, buffer.Length);

            if (sampleRead < buffer.Length) Array.Resize(ref buffer, sampleRead);
            return buffer;
        }
    }
}