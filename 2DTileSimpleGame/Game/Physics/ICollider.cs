using _2DTileSimpleGame.Game.Manager;
using System;

namespace _2DTileSimpleGame.Physics
{
    /// <summary>
    /// Provides a very basic collision system.
    /// </summary>
    interface ICollider
    {
        /// <summary>
        /// Returns visual representation of object that is under body and is one of the few wanted ones in the list.
        /// </summary>
        /// <param name="body">Visual representation of object that is being checked for collision.</param>
        /// <param name="tags">List of tags that are to be checked whether they are below body.</param>
        /// <returns>NULL or object that is below body</returns>
        IGraphicsComponent GetObjectUnder(IGraphicsComponent body, params Enum[] tags);
        /// <summary>
        /// Checks whether there is an object it can't pass through according to the specified offsets.
        /// </summary>
        /// <param name="body">Object that is being checked for collision</param>
        /// <param name="offsetX">Offset of X axis where a check needs to happen.</param>
        /// <param name="offsetY">Offset of Y axis where a check needs to happen.</param>
        /// <returns>True if path is blocked or False if path is not blocked.</returns>
        bool IsPathBlocked(IGraphicsComponent body, double offsetX, double offsetY);
    }
}
