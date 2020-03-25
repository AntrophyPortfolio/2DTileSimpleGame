using System;
using System.Windows.Media.Media3D;

namespace _2DTileSimpleGame.Graphics
{
    /// <summary>
    /// Provides different types of game objects located
    /// throughout the game.
    /// </summary>
    interface IGameObject
    {
        /// <summary>
        /// Type of the class that the object originated from.
        /// </summary>
        ClassType ClassType { get; }
        /// <summary>
        /// Custom tag for the object.
        /// </summary>
        Enum Tag { get; }
        /// <summary>
        /// Size of the object on the final Canvas.
        /// </summary>
        double Size { get; }
        /// <summary>
        /// Coordinates of the initial position for the object.
        /// </summary>
        Point3D Coordinates { get; set; }
    }
}