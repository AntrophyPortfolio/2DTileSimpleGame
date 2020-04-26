using System.Windows.Shapes;

namespace _2DTileSimpleGame.UserInterface
{
    /// <summary>
    /// Provides user interface for the game.
    /// </summary>
    interface IUserInterface
    {
        /// <summary>
        /// Creates a basic UI layout that will be seen on the user screen.
        /// </summary>
        /// <param name="numberOfLives">Numer of lives player has.</param>
        void CreateUserInterface(int numberOfLives);
        /// <summary>
        /// Removes a life.
        /// </summary>
        /// <returns></returns>
        Rectangle RemoveLife();
        /// <summary>
        /// Updates remaining fruit textblock.
        /// </summary>
        void UpdateLevel();
        /// <summary>
        /// Resets offset X of the hearts after new level creation.
        /// </summary>
        void ResetHeartsLayoutX();
        /// <summary>
        /// Shows pause game menu when ESC pressed.
        /// </summary>
        void ShowPauseGameControls();
        /// <summary>
        /// Hides pause game menu when ESC pressed again.
        /// </summary>
        void HidePauseGameControls();
        /// <summary>
        /// Shows death message with button to go back to menu.
        /// </summary>
        void ShowDeathMessage();
    }
}
