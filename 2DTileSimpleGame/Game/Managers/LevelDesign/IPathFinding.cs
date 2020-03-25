namespace _2DTileSimpleGame.GameLogic
{
    /// <summary>
    /// Provides a simple way to know whether all target points of 3D array are connected.
    /// </summary>
    /// <typeparam name="T">Type of the 3D array.</typeparam>
    interface IPathFinding<T>
    {
        /// <summary>
        /// The 3D array containing objects the path needs to found for.
        /// </summary>
        T[,,] ArrayGrid3D { get; set; }
        /// <summary>
        /// Starting point of the search. In most cases the player initial position.
        /// </summary>
        T InitialUnit { get; set; }
        /// <summary>
        /// Adds target points to the targets list.
        /// </summary>
        /// <param name="values">Objects of the array that are to be checked
        /// whether a path between them exists.</param>
        void TargetsAddAll(params T[] values);
        /// <summary>
        /// Adds obstacle points to the obstacle list.
        /// </summary>
        /// <param name="values">Objects of the array that serve as "path block"
        /// that prevent from going into that direction.</param>
        void ObstaclesAddAll(params T[] values);
        /// <summary>
        /// Checks whether path exists between all targets in the specified array.
        /// </summary>
        /// <returns></returns>
        bool PathExists();
    }
}
