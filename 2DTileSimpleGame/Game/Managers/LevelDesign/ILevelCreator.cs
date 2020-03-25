using _2DTileSimpleGame.Game.Manager;
using _2DTileSimpleGame.Graphics;

namespace _2DTileSimpleGame.GameLogic
{
    /// <summary>
    /// Provides randomly generated game world level.
    /// </summary>
    interface ILevelCreator
    {
        /// <summary>
        /// Instance of the object on Canvas that the player controls.
        /// </summary>
        IGraphicsComponent Player { get; set; }
        /// <summary>
        /// Generates a new game level.
        /// </summary>
        /// <param name="numberOfFruits">Number of fruits present for new level.</param>
        /// <param name="numberOfEnemies">Number of enemies present for new level.</param>
        void CreateLevel(int numberOfFruits, int numberOfEnemies);
        /// <summary>
        /// Serializes objects of 3D world level into a .dat file.
        /// </summary>
        /// <param name="fileName">Name of the file the level will be saved to.</param>
        void SaveLevel(string fileName);
        /// <summary>
        /// Deserializes objects from .dat file and loads them as a new level.
        /// </summary>
        /// <param name="fileName">Name of the file containing level.</param>
        /// <param name="numberOfFruits">Returns number of fruits the loaded level contains.</param>
        /// <param name="numberOfLevels">Returns level number the loaded level contains.</param>
        void LoadLevel(string fileName, out int numberOfFruits, out int numberOfLevels);
        /// <summary>
        /// Spawns a finish point for current level.
        /// </summary>
        /// <param name="type">Type of finish point</param>
        void SpawnFinishPoint(BlockType type);
    }
}
