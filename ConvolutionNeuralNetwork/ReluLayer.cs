namespace ConvolutionNeuralNetwork
{
    public class ReluLayer
    {
        public int[][,] Relu(int[][,] inputData)
        {
            var layerCount = inputData.Length;
            var outputData = new int[layerCount][,];

            for (int i = 0; i < inputData.Length; i++)
            {
                var reluLayer = new int[inputData[i].GetUpperBound(0) + 1, inputData[i].GetUpperBound(1) + 1];

                for (int n = 0; n < inputData[i].GetUpperBound(0) + 1; n++)
                {
                    for (int m = 0; m < inputData[i].GetUpperBound(1) + 1; m++)
                    {
                        reluLayer[n, m] = inputData[i][n, m] > 0 ? inputData[i][n, m] : 0;
                    }
                }

                outputData[i] = reluLayer;
            }

            return outputData;
        }
    }
}
