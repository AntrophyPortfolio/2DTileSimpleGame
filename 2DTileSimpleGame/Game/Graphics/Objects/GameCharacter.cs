using _2DTileSimpleGame.Graphics;
using System;
using System.Windows.Media.Media3D;

namespace _2DTileSimpleGame
{
    [Serializable]
    class GameCharacter : IGameObject
    {
        public double Size { get; } = (System.Windows.SystemParameters.WorkArea.Height - (2 * System.Windows.SystemParameters.CaptionHeight)) / 16;
        public Enum Tag { get; private set; }
        public Point3D Coordinates { get; set; }
        public ClassType ClassType { get; } = ClassType.GameCharacter;

        public GameCharacter(CharacterType type, Point3D refCoordinates)
        {
            Tag = type;
            Coordinates = refCoordinates;
        }
    }
}
