using _2DTileSimpleGame.Graphics;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.Game.Manager
{
    class GraphicsComponent : IGraphicsComponent
    {
        public Rectangle RectangleBody { get; set; }
        public IGameObject ObjectContext { get; set; }
        public double CanvasPositionX { get; private set; }
        public double CanvasPositionY { get; private set; }
        private readonly IResourceManager resourceManager;
        private readonly Canvas gameCanvas;
        public GraphicsComponent(IGameObject refGameObject, IResourceManager refResourceManager, Canvas refGameCanvas)
        {
            ObjectContext = refGameObject;
            resourceManager = refResourceManager;
            CreateRectangleBody();
            gameCanvas = refGameCanvas;
            SetCanvasPositionX(refGameObject.Coordinates.X * ObjectContext.Size);
            SetCanvasPositionY(refGameObject.Coordinates.Y * ObjectContext.Size);
            gameCanvas.Children.Add(RectangleBody);
        }
        private void CreateRectangleBody()
        {
            Rectangle rectangle = new Rectangle
            {
                Width = ObjectContext.Size,
                Height = ObjectContext.Size,
                Fill = DetermineImageBrush()
            };
            RectangleBody = rectangle;
        }

        private ImageBrush DetermineImageBrush()
        {
            switch (ObjectContext.ClassType)
            {
                case ClassType.GameFruit:
                    resourceManager.FruitImages.TryGetValue((FruitType)ObjectContext.Tag, out ImageBrush imageFruit);
                    return imageFruit;
                case ClassType.GameBlock:
                    resourceManager.BlockImages.TryGetValue((BlockType)ObjectContext.Tag, out ImageBrush imageBlock);
                    return imageBlock;
                case ClassType.GameCharacter:
                    resourceManager.CharacterImages.TryGetValue((CharacterType)ObjectContext.Tag, out ImageBrush imageCharacter);
                    return imageCharacter;
                default:
                    throw new ArgumentException("The provider object doesn't belong to any game object.");
            }
        }

        public void SetCanvasPositionX(double positionX)
        {
            CanvasPositionX = positionX;
            Canvas.SetLeft(RectangleBody, CanvasPositionX);
        }

        public void SetCanvasPositionY(double positionY)
        {
            CanvasPositionY = positionY;
            Canvas.SetTop(RectangleBody, CanvasPositionY);
        }
    }
}
