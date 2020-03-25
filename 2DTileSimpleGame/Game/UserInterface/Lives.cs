using _2DTileSimpleGame.Graphics;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.UserInterface
{
    class Lives : ILives
    {
        private readonly IResourceManager resourceManager;
        private readonly Stack<Rectangle> heartStack = new Stack<Rectangle>();
        private int heartPositionX = 60;
        private readonly ImageBrush imageHeart;

        public Lives(IResourceManager refResourceManager)
        {
            resourceManager = refResourceManager;
            resourceManager.UserInterfaceImages.TryGetValue(UserInterfaceType.HeartLife, out imageHeart);

        }
        public Rectangle AddHeart()
        {
            heartStack.Push(CreateNewHeart());
            return heartStack.Peek();
        }

        public TextBlock AddLiveText()
        {
            TextBlock textLives = new TextBlock
            {
                Text = "LIVES:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Yellow),
            };
            Canvas.SetLeft(textLives, 10);
            Canvas.SetTop(textLives, 0);
            return textLives;
        }

        public Rectangle RemoveHeart()
        {
            return heartStack.Pop();
        }
        private Rectangle CreateNewHeart()
        {
            Rectangle heart = new Rectangle
            {
                Fill = imageHeart,
            };
            heart.Tag = UserInterfaceType.HeartLife;
            heartStack.Push(heart);
            heartPositionX += 50;
            Canvas.SetLeft(heart, heartPositionX);
            Canvas.SetTop(heart, 3);
            return heart;
        }
        public void ResetLayoutX()
        {
            heartPositionX = 60;
        }
    }
}
