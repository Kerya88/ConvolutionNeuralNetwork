using System.Drawing;

namespace ConvolutionNeuralNetwork
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int TrainCount = 10000;
            const int TestCount = 10000;

            var trainFiles = new DirectoryInfo(@"D:\archive\train").GetFiles().Select(x => Image.FromFile(x.FullName));
            var testFiles = new DirectoryInfo(@"D:\archive\test").GetFiles().Select(x => Image.FromFile(x.FullName));

            var trainData = new InputData(trainFiles.Take(TrainCount).ToArray());
            var testData = new InputData(testFiles.Take(TestCount).ToArray());

        }
    }
}
