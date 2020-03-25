using System.Windows.Shapes;

namespace _2DTileSimpleGame.Animations
{
    /// <summary>
    /// Provides simple animations for game world.
    /// </summary>
    interface IAnimator
    {
        /// <summary>
        /// Makes the object on canvas flashing by decreasing and increasing opacity.
        /// </summary>
        /// <param name="obj">Rectangle of object on Canvas.</param>
        /// <param name="intervalMillis">Duration of the animation.</param>
        /// <param name="numberOfRepeats">Number of repeats.</param>
        /// <param name="opacityFrom">Change Opacity from specified value.</param>
        /// <param name="opacityTo">Change opacity value to specified final value.</param>
        /// <param name="removeAfter">Whether you want to remove said object after the animation finishes.</param>
        void AnimationFlashing(Rectangle obj, int intervalMillis, int numberOfRepeats, double opacityFrom, double opacityTo, bool removeAfter);
        /// <summary>
        /// Makes the object on canvas change its size from specified value to final specified value.
        /// </summary>
        /// <param name="obj">Rectangle of object on Canvas.</param>
        /// <param name="fromSize">Change size from specified value.</param>
        /// <param name="toSize">Change size to specified final value.</param>
        /// <param name="autoReverse">Whether the animation should reverse after finishing.</param>
        /// <param name="durationMillis">Duration of the animation.</param>
        void AnimationRescaling(Rectangle obj, double fromSize, double toSize, bool autoReverse, int durationMillis);
    }
}
