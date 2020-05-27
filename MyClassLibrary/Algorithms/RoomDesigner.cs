using System.Collections.Generic;
using System.Linq;

namespace MyClassLibrary.Algorithms
{
    public class RoomDesigner
    {

        private readonly List<Rect> _spaceObjects = new List<Rect>();

        public int Width { get; }
        public int Height { get; }

        public int Area => Width * Height;

        public RoomDesigner(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void AddSpaceObject(int width, int height)
        {
            _spaceObjects.Add(new Rect(width, height));
        }

        public int CalculateNumberOfUsefulCombinations()
        {
            var sumSpaceObjectArea = _spaceObjects.Sum(rect => rect.Area);

            if (sumSpaceObjectArea > Area)
                return 0;
            if (sumSpaceObjectArea == Area)
                return 1;

            if (_spaceObjects.Count == 1)
            {
                var distinctIndices = new List<int>();
                for (var i = 0; i < Area; i++)
                {
                    var newIndex1 = i % Width;
                    var newIndex2 = i % Height;
                    if (!distinctIndices.Contains(newIndex1) && !distinctIndices.Contains(newIndex2))
                        return 1;
                }
            }
            
            return 0;
        }
    }

    internal struct Rect
    {
        public Rect(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
        public int Area => Width * Height;
    }
}