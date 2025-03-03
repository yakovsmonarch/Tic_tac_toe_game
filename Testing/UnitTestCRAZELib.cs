using CRAZELib.Engine.ArrTwoEngine;
using CRAZELib.Enums;

namespace Testing
{
    public class UnitTestCRAZELib
    {
        [Fact]
        public void TestParseCZN()
        {
            const string czn = "cz1/3/3 z 3";
            var crazeEngineYakov = new YakovCrazeEngine(czn, CrazeForce.Super);
            int[,] sourceArray = crazeEngineYakov.CloneArrPos();

            bool IsTrueDimensionArray = sourceArray.GetLength(0) == 3 && sourceArray.GetLength(1) == 3;
            Assert.True(IsTrueDimensionArray);

            int[,] destArray = new int[3, 3]
            {
                { 2, 1, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            bool isEqual = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (sourceArray[i, j] != destArray[i, j])
                    {
                        isEqual = false;
                        break;
                    }
                }
            }
            Assert.True(isEqual);
        }
    }
}