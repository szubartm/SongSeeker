using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using SongSeeker.Interfaces;

namespace SongSeeker;

public class SpectrogramGenerator : ISpectrogramGenerator
{
    public float[,] CreateSpectrogram(float[] samples, int windowSize, int hopSize, int sampleRate)
    {
        var numWindows = (samples.Length - windowSize) / hopSize;
        var fftSize = windowSize / 2 + 1;

        var spectrogram = new float[numWindows, fftSize];
        var hammingWindow = Window.Hamming(windowSize).Select(w => (float)w).ToArray();

        Parallel.For(0, numWindows, i =>
        {
            var window = new Complex[windowSize];
            for (var j = 0; j < windowSize; j++)
                window[j] = new Complex(samples[i * hopSize + j] * hammingWindow[j], 0);

            Fourier.Forward(window, FourierOptions.Default);

            for (var j = 0; j < fftSize; j++)
            {
                var magnitude = (float)window[j].Magnitude;
                var dB = 20 * (float)Math.Log10(magnitude + 1e-10);
                spectrogram[i, j] = dB;
            }
        });

        return spectrogram;
    }
}