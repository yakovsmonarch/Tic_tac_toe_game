using TicTacToeLib.Engine.ArrTwoEngine;
using TicTacToeLib.Enums;

namespace Testing
{
    public class UnitTestTicTacToeLib
    {
        [Fact]
        public void TestParseFenTTT()
        {
            const string fenttt = "cz1/3/3 z 3";
            var engineYakov = new YakovEngine(fenttt, Level.Super);
            int[,] valueArr = engineYakov.CloneArrPos();

            bool IsTrueDimensionArray = valueArr.GetLength(0) == 3 && valueArr.GetLength(1) == 3;
            Assert.True(IsTrueDimensionArray);

            int[,] etalonArr = new int[3, 3]
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
                    if (valueArr[i, j] != etalonArr[i, j])
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