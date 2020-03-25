using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace _2DTileSimpleGame.Graphics
{
    /// <summary>
    /// Provides resources for the game from different files (sound effects, images, textures).
    /// </summary>
    interface IResourceManager
    {
        /// <summary>
        /// Provides a dictionary of images for different types of fruits.
        /// </summary>
        Dictionary<FruitType, ImageBrush> FruitImages { get; }
        /// <summary>
        /// Provides a dictionary of images for different types of blocks (floor).
        /// </summary>
        Dictionary<BlockType, ImageBrush> BlockImages { get; }
        /// <summary>
        /// Provides a dictionary of images for different types of characters (enemy / player).
        /// </summary>
        Dictionary<CharacterType, ImageBrush> CharacterImages { get; }
        /// <summary>
        /// Provides a dictionary of media files for different sound effects.
        /// </summary>
        Dictionary<MediaType, MediaPlayer> MediaFiles { get; }
        /// <summary>
        /// Provides a dictionary of images for use in user interface.
        /// </summary>
        Dictionary<UserInterfaceType, ImageBrush> UserInterfaceImages { get; }
        /// <summary>
        /// Provides a dictionary of images for use as a button visual in menu.
        /// </summary>
        Dictionary<ButtonType, ImageSource> ButtonImages { get; }
        /// <summary>
        /// Provides a single instance of the class Random to be used in all classes that need it.
        /// </summary>
        Random RandomGenerator { get; }
    }
}
