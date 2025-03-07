using SongSeeker.Interfaces;

namespace SongSeeker;

public class SpectrogramPipeline(
    IAudioProcessor audioProcessor,
    ISpectrogramGenerator spectrogramGenerator,
    IImageSaver imageSaver)
{
    public void GenerateSpectrogram(string audioFilePath, string outputImagePath, int windowSize, int hopSize,
        int imageWidth, int imageHeight, int sampleRate)
    {
        var samples = audioProcessor.ReadAudioSamples(audioFilePath);
        var spectrogram = spectrogramGenerator.CreateSpectrogram(samples, windowSize, hopSize, sampleRate);
        imageSaver.SaveImage(spectrogram, outputImagePath, imageWidth, imageHeight, sampleRate, hopSize);
    }
}