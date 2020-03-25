using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace _2DTileSimpleGame.Graphics
{
    [Serializable]
    class GameFruit : IGameObject
    {
        public double Size { get; } = (System.Windows.SystemParameters.WorkArea.Height - (2 * System.Windows.SystemParameters.CaptionHeight)) / 16;
        public Enum Tag { get; private set; }
        public Point3D Coordinates { get; set; }
        public ClassType ClassType { get; } = ClassType.GameFruit;
        [NonSerialized]
        private readonly Random rand;

        public GameFruit(IResourceManager resourceManager, Point3D refCoordinates)
        {
            rand = resourceManager.RandomGenerator;
            DecideImageType();
            Coordinates = refCoordinates;
        }
        private void DecideImageType()
        {
            Tag = GenerateFruitType();
        }
        private FruitType GenerateFruitType()
        {
            List<FruitType> fruits = Enum.GetValues(typeof(FruitType)).Cast<FruitType>().ToList();
            int randomIndex = rand.Next(0, fruits.Count - 1);

            return fruits[randomIndex];
        }
    }
}
