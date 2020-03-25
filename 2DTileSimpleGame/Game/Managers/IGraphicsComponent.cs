using _2DTileSimpleGame.Graphics;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.Game.Manager
{
    /// <summary>
    /// Class implementing this interface connects 
    /// data of the object with the visual
    /// representation on the Canvas.
    /// </summary>
    interface IGraphicsComponent
    {
        /// <summary>
        /// Shape Rectangle that will be shown to the end user on screen in the Canvas.
        /// </summary>
        Rectangle RectangleBody { get; set; }
        /// <summary>
        /// Game object that contains data for visual representation on screen.
        /// </summary>
        IGameObject ObjectContext { get; set; }
        /// <summary>
        /// Position of the shape in the canvas representing X axis.
        /// </summary>
        double CanvasPositionX { get; }
        /// <summary>
        /// Position of the shape in the canvas representing Y axis.
        /// </summary>
        double CanvasPositionY { get; }
        /// <summary>
        /// Sets a new position of the shape on the X axis in the canvas using SetLeft
        /// method and updating CanvasPosition property.
        /// </summary>
        /// <param name="positionX">New X axis of the shape on canvas.</param>
        void SetCanvasPositionX(double positionX);
        /// <summary>
        /// Sets a new position of the shape on the Y axis in the canvas using SetTop 
        /// method and updating CanvasPosition property.
        /// </summary>
        /// <param name="positionY">New Y axis of the shape on the canvas.</param>
        void SetCanvasPositionY(double positionY);
    }
}
