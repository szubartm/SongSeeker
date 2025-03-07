using SongSeeker;

namespace Tester;

public class Program
{
    private static void Main(string[] args)
    {
        var audioFilePath = "music1.mp3";
        var outputImagePath = "spectrogram.png";

        var audioProcessor = new Mp3AudioProcessor();
        var spectrogramGenerator = new SpectrogramGenerator();
        var imageSaver = new SpectrogramImageSaver();
        var pipeline = new SpectrogramPipeline(audioProcessor, spectrogramGenerator, imageSaver);

        var windowSize = 2048;
        var hopSize = 512;
        var imageWidth = 1024;
        var imageHeight = 512;
        var sampleRate = 44100;

        pipeline.GenerateSpectrogram(audioFilePath, outputImagePath, windowSize, hopSize, imageWidth, imageHeight,
            sampleRate);

        Console.WriteLine("Spectrogram saved to " + outputImagePath);
    }
}