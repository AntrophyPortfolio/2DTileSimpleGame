using System.Windows.Controls;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.UserInterface
{
    /// <summary>
    /// Provides visual representation of remaining lives.
    /// </summary>
    interface ILives
    {
        /// <summary>
        /// Removes a heart from the stack.
        /// </summary>
        /// <returns>Removed heart.</returns>
        Rectangle RemoveHeart();
        /// <summary>
        /// Adds a heart to the stack.
        /// </summary>
        /// <returns>Added heart.</returns>
        Rectangle AddHeart();
        /// <summary>
        /// Adds a text next to the lives.
        /// </summary>
        /// <returns>Added text.</returns>
        TextBlock AddLiveText();
        /// <summary>
        /// Resets heart offset layout because new game created.
        /// </summary>
        void ResetLayoutX();
    }
}
