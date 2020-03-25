using _2DTileSimpleGame.Game.Manager;

namespace _2DTileSimpleGame.Game
{
    /// <summary>
    /// Controls what the game does at certain points of the game and takes
    /// actions when any events occur.
    /// </summary>
    interface IGameManager
    {
        /// <summary>
        /// The 3D array of the world. Z Axis of the world is 
        /// either 0 (floor) or 1 (item above the ground).
        /// </summary>
        IGraphicsComponent[,,] GameMesh { get; set; }
        /// <summary>
        /// Returns number of level the player is currently in.
        /// </summary>
        int CurrentLevel { get; }
        /// <summary>
        /// Returns number of fruits the player still needs to pick up.
        /// </summary>
        int FruitsCounter { get; }
        /// <summary>
        /// Randomly generates a 3D world and starts a new game,
        /// starting at 2 enemies and 3 fruits.
        /// </summary>
        void StartGame();
        /// <summary>
        /// Quits the current game and returns back to main menu.
        /// </summary>
        void QuitToMenu();
        /// <summary>
        /// Loads a saved game.
        /// </summary>
        /// <param name="filename">Name of the file that contains
        /// game world of the level.</param>
        void LoadGame(string filename);
        /// <summary>
        /// Saves the current game.
        /// </summary>
        /// <param name="filename">Name of the file that contains
        /// game world of the level.</param>
        void SaveGame(string filename);
        /// <summary>
        /// Re-enables all timers and hides pause menu.
        /// </summary>
        void UnPauseGame();
        /// <summary>
        /// Stops all timers and shows a pause menu.
        /// </summary>
        void PauseGame();
    }
}
