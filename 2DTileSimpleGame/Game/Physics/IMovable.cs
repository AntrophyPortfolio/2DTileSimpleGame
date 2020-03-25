namespace _2DTileSimpleGame.Physics
{
    /// <summary>
    /// Provides a basic movement functionality.
    /// </summary>
    interface IMovable
    {
        /// <summary>
        /// Returns whether the object is moving or not.
        /// </summary>
        bool IsMoving { get; set; }
        /// <summary>
        /// Returns the speed of mvoement of the object.
        /// </summary>
        int MovementSpeed { get; }
        /// <summary>
        /// Stops the timers in the class resulting in stopping the movement for the object.
        /// </summary>
        void StopMovement();
        /// <summary>
        /// Provides a single tick move for the movement that is not controlled by timers.
        /// </summary>
        void Move();
        /// <summary>
        /// Starts all the movement timers.
        /// </summary>
        void StartMovement();
    }
}
