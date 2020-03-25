using _2DTileSimpleGame.Graphics;
using System;
using System.Windows.Media.Media3D;

namespace _2DTileSimpleGame
{
    [Serializable]
    class GameBlock : IGameObject
    {
        public double Size { get; } = (System.Windows.SystemParameters.WorkArea.Height - (2 * System.Windows.SystemParameters.CaptionHeight)) / 16;
        public Enum Tag { get; private set; }
        public Point3D Coordinates { get; set; }
        public ClassType ClassType { get; } = ClassType.GameBlock;
        [NonSerialized]
        private readonly Random rand;

        public GameBlock(IResourceManager resourceManager, Point3D refCoordinates)
        {
            rand = resourceManager.RandomGenerator;
            DecideBlockType();
            Coordinates = refCoordinates;
        }
        public GameBlock(BlockType type, Point3D refCoordinates)
        {
            Tag = type;
            Coordinates = refCoordinates;
        }
        private void DecideBlockType()
        {
            Tag = GenerateBlockType();
        }
        private BlockType GenerateBlockType()
        {
            int randomNumber = rand.Next(0, 101);

            switch (randomNumber)
            {
                case int n when (n >= 0 && n <= 75):
                    return BlockType.Dirt;
                case int n when (n > 75 && n <= 85):
                    return BlockType.Water;
                case int n when (n > 85 && n <= 100):
                    return BlockType.Wall;
                default:
                    throw new FormatException("The block doesn't exist");
            }
        }
    }
}
