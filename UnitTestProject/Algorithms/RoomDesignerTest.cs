using MyClassLibrary.Algorithms;
using Xunit;

namespace UnitTestProject.Algorithms
{
    public class RoomDesignerTest
    {

        [Theory]
        [InlineData(1, 1, 1, 1, 1, 1)]
        [InlineData(2, 2, 1, 1, 1, 1)]
        [InlineData(2, 2, 1, 1, 2, 2)]
        [InlineData(2, 2, 1, 1, 3, 1)]
        [InlineData(3, 3, 1, 1, 1, 3)]
        [InlineData(3, 3, 1, 1, 2, 8)]
        [InlineData(3, 3, 2, 3, 1, 1)]
        [InlineData(3, 3, 2, 1, 1, 2)]
        public void Test(int roomWidth, int roomHeight, int rectsWidth, int rectsHeight, int numRects, int numCombinations)
        {
            var roomDesigner = new RoomDesigner(roomWidth, roomHeight);
            for (var i = 0; i < numRects; i++)
            {
                roomDesigner.AddSpaceObject(rectsWidth, rectsHeight);
            }

            Assert.Equal(numCombinations, roomDesigner.CalculateNumberOfUsefulCombinations());
        }
        
    }
}