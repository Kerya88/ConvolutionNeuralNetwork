namespace ConvolutionNeuralNetwork
{
    public class MaxPoolingLayer
    {
        private int PoolSide;
        private int StepSize;

        public MaxPoolingLayer(int poolSide = 2, int stepSize = 2)
        {
            PoolSide = poolSide;
            StepSize = stepSize;
        }

        public int[][,] Pool(int[][,] inputData)
        {
            var layerCount = inputData.Length;
            var outputData = new int[layerCount][,];

            var inputHight = outputData[0].GetUpperBound(1) + 1;
            var inputWidth = outputData[0].GetUpperBound(1) + 1;
            var outputHight = (inputHight - PoolSide) / StepSize + 1;
            var outputWidth = (inputWidth - PoolSide) / StepSize + 1;

            var layerNum = 0;
            foreach (var layer in inputData) 
            {
                var pooled = new int[outputHight, outputWidth];

                var q = 0;
                var p = 0;

                for (var i = 0; i < inputHight - PoolSide; i += StepSize)
                {
                    for (var j = 0; j < inputWidth - PoolSide; j += StepSize)
                    {
                        var max = 0;

                        for (var m = i; m < PoolSide; m++)
                        {
                            for (var n = j; n < PoolSide; n++)
                            {
                                max = max > layer[m,n] ? max : layer[m,n];
                            }
                        }

                        pooled[q, p] = max;
                        p++;
                    }
                    q++;
                }

                outputData[layerNum] = pooled;
                layerNum++;
            }

            return outputData;
        }
    }
}
