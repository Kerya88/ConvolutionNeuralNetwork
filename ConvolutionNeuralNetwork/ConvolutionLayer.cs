namespace ConvolutionNeuralNetwork
{
    public class ConvolutionLayer
    {
        private int FilterCount;
        private int FilterSide;
        private int FilterDepth;
        private int[][][,] FilterWeights;
        private int[] Offsets;
        private int StepSize;
        private int AlligmentSize;
        private int InputWidth;
        private int InputHight;
        private int AlligmentWidth;
        private int AlligmentHight;
        private int OutputWidth;
        private int OutputHight;

        public ConvolutionLayer(int inputWidth, int inputHight, int filterCount = 1, int filterSide = 3, int stepSize = 1, int alligmentSize = 1, int filterDepth = 1)
        {
            if ((inputWidth - filterSide + 2 * alligmentSize) % stepSize == 0 || (inputHight - filterSide + 2 * alligmentSize) % stepSize == 0)
            {
                OutputWidth = (inputWidth - filterSide + 2 * alligmentSize) / stepSize + 1;
                OutputHight = (inputHight - filterSide + 2 * alligmentSize) / stepSize + 1;
            }
            else
            {
                throw new Exception("Заданы неверные параметры для инициализации сверточного слоя: размерность выходного представления не является целым числом");
            }

            FilterCount = filterCount;
            FilterSide = filterSide;
            FilterDepth = filterDepth;
            StepSize = stepSize;
            AlligmentSize = alligmentSize;
            InputWidth = inputWidth;
            InputHight = inputHight;
            AlligmentWidth = inputWidth + 2 * alligmentSize;
            AlligmentHight = inputHight + 2 * alligmentSize;
        }

        public void InitWeights(int[][][,]? filterWeights = null, int[]? offsets = null)
        {
            FilterWeights = new int[FilterCount][][,];

            if (filterWeights == null) 
            {
                Random rnd = new();

                for (int i = 0; i < FilterCount; i++)
                {
                    FilterWeights[i] = new int[FilterDepth][,];

                    for (int j = 0; j < FilterDepth; j++)
                    {
                        FilterWeights[i][j] = new int[FilterSide, FilterSide];

                        for (var m = 0; m < FilterSide; m++)
                        {
                            for (var n = 0; n < FilterSide; n++)
                            {
                                FilterWeights[i][j][m, n] = rnd.Next(4);
                            }
                        }
                    }
                }
            }
            else
            {
                Array.Copy(filterWeights, FilterWeights, FilterCount);
            }

            Offsets = new int[FilterCount];
            if (offsets == null)
            {
                for (var i = 0; i < FilterCount; i++)
                {
                    Offsets[i] = 1;
                }
            }
            else
            {
                Array.Copy(offsets, Offsets, FilterCount);
            }
        }

        public int[][,] ConvolutionWithSomeFilters(int[][,] inputData)
        {
            var outputData = new int[FilterCount][,];

            for (int filterNum = 0; filterNum < FilterCount; filterNum++)
            {
                outputData[filterNum] = this.ConvolutionSomeLayers(inputData, filterNum);
            }

            return outputData;
        }

        public int[,] ConvolutionSomeLayers(int[][,] inputData, int filterNum = 0)
        {
            var layer = 0;
            var layersSum = new int[OutputHight, OutputWidth];

            foreach (var oneLayer in inputData)
            {
                var alligmentData = new int[AlligmentHight, AlligmentWidth];

                for (var i = 0; i < AlligmentHight; i++)
                {
                    for (var j = 0; j < AlligmentWidth; j++)
                    {
                        if (i < AlligmentSize || i >= InputHight + AlligmentSize)
                        {
                            alligmentData[i, j] = 0;
                        }
                        else
                        {
                            if (j < AlligmentSize || j >= InputWidth + AlligmentSize)
                            {
                                alligmentData[i, j] = 0;
                            }
                            else
                            {
                                alligmentData[i, j] = oneLayer[i - 1, j - 1];
                            }
                        }
                    }
                }

                var convLayer = new int[OutputHight, OutputWidth];
                var q = 0;
                var p = 0;

                for (var i = 0; i < AlligmentHight - FilterSide; i += StepSize)
                {
                    for (var j = 0; j < AlligmentWidth - FilterSide; j += StepSize)
                    {
                        var sum = 0;

                        for (var m = i; m < FilterSide; m++)
                        {
                            for (var n = j; n < FilterSide; n++)
                            {
                                sum += alligmentData[i, j] * FilterWeights[filterNum][layer][m, n];
                            }
                        }

                        convLayer[q, p] = sum;
                        p++;
                    }
                    q++;
                }

                for (int i = 0; i < OutputHight; i++)
                {
                    for (int j = 0; j < OutputWidth; j++)
                    {
                        layersSum[i, j] += convLayer[i, j];
                    }
                }

                layer++;
            }

            for (int i = 0; i < OutputHight; i++)
            {
                for (int j = 0; j < OutputWidth; j++)
                {
                    layersSum[i, j] += layersSum[i, j] + Offsets[filterNum];
                }
            }

            return layersSum;
        }
    }
}
