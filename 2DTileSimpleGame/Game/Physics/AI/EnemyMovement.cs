using _2DTileSimpleGame.Game.Manager;
using _2DTileSimpleGame.Graphics;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace _2DTileSimpleGame.Physics.AI
{
    class EnemyMovement : IMovable
    {
        public bool IsMoving { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int MovementSpeed { get; } = 2;
        private readonly IGraphicsComponent enemy;
        private readonly ICollider collider;
        private readonly IResourceManager resourceManager;
        private readonly Timer movementWait = new Timer();
        private readonly Timer movementTimerTick = new Timer();

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        public EnemyMovement(IGraphicsComponent refEnemy, ICollider refCollider, IResourceManager refResourceManager)
        {
            collider = refCollider;
            enemy = refEnemy;
            resourceManager = refResourceManager;
            movementWait.Interval = 1000;
            movementWait.Elapsed += delegate
            {
                moveLeft = false;
                moveRight = false;
                moveUp = false;
                moveDown = false;
                DecideNextStep();
                movementTimerTick.Enabled = true;
            };
            movementWait.AutoReset = true;
            movementTimerTick.Interval = 20;
            movementTimerTick.AutoReset = true;
            movementTimerTick.Elapsed += delegate
            {
                DoMovement();
            };
        }
        public void StopMovement()
        {
            movementWait.Stop();
            movementTimerTick.Stop();
        }
        public void StartMovement()
        {
            movementWait.Start();
            movementTimerTick.Start();
        }
        public void Move()
        {
            StartMovement();
        }
        private void DecideNextStep()
        {
            int randomNumber = resourceManager.RandomGenerator.Next(0, 101);
            switch (randomNumber)
            {
                case int n when (n >= 0 && n <= 25):
                    moveLeft = true;
                    break;
                case int n when (n > 25 && n <= 50):
                    moveRight = true;
                    break;
                case int n when (n > 50 && n <= 75):
                    moveUp = true;
                    break;
                case int n when (n > 75 && n <= 100):
                    moveDown = true;
                    break;
            }
        }
        private void DoMovement()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                double x = enemy.CanvasPositionX;
                double y = enemy.CanvasPositionY;

                if (moveLeft)
                {
                    if (!collider.IsPathBlocked(enemy, -5, 0))
                    {
                        x -= MovementSpeed;
                        enemy.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.West);
                    }
                    else
                    {
                        DecideNextStep();
                    }
                }
                if (moveRight)
                {
                    if (!collider.IsPathBlocked(enemy, 5, 0))
                    {
                        x += MovementSpeed;
                    }
                    else
                    {
                        DecideNextStep();
                    }
                    enemy.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.East);
                }
                if (moveUp)
                {
                    if (!collider.IsPathBlocked(enemy, 0, -5))
                    {
                        y -= MovementSpeed;
                    }
                    else
                    {
                        DecideNextStep();
                    }
                    enemy.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.North);
                }
                if (moveDown)
                {
                    if (!collider.IsPathBlocked(enemy, 0, 5))
                    {
                        y += MovementSpeed;
                    }
                    else
                    {
                        DecideNextStep();
                    }
                    enemy.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.South);
                }
                enemy.SetCanvasPositionX(x);
                enemy.SetCanvasPositionY(y);
            });
        }
        
    }
}
