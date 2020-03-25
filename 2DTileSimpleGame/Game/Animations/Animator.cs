using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.Animations
{
    class Animator : IAnimator
    {
        public void AnimationFlashing(Rectangle obj, int intervalMillis, int numberOfRepeats, double opacityFrom, double opacityTo, bool removeAfter)
        {
            DoubleAnimation db = new DoubleAnimation
            {
                From = opacityFrom,
                To = opacityTo,
                Duration = TimeSpan.FromMilliseconds(intervalMillis),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(numberOfRepeats)
            };
            if (removeAfter)
            {
                db.Completed += delegate
                {
                    Canvas.SetLeft(obj, 2000);
                    Canvas.SetTop(obj, 2000);
                };
            }
            obj.BeginAnimation(Rectangle.OpacityProperty, db);
        }
        public void AnimationRescaling(Rectangle obj, double fromSize, double toSize, bool autoReverse, int durationMillis)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = fromSize,
                To = toSize,
                Duration = TimeSpan.FromMilliseconds(durationMillis),
                AutoReverse = autoReverse
            };
            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = fromSize,
                To = toSize,
                Duration = TimeSpan.FromMilliseconds(durationMillis),
                AutoReverse = autoReverse
            };
            obj.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            obj.BeginAnimation(Rectangle.HeightProperty, heightAnimation);
        }
    }
}
