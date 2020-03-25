using _2DTileSimpleGame.Game.Manager;
using _2DTileSimpleGame.Graphics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace _2DTileSimpleGame.Physics
{
    class PlayerMovement : IMovable
    {
        public int MovementSpeed { get; } = 2;
        public bool IsMoving { get; set; } = false;
        private readonly ICollider collider;
        private readonly Canvas gameCanvas;
        private readonly IGraphicsComponent player;
        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private bool stopMovement;

        public PlayerMovement(IGraphicsComponent refPlayer, ICollider refCollider, Canvas refCanvas)
        {
            player = refPlayer;
            collider = refCollider;
            gameCanvas = refCanvas;

            gameCanvas.KeyUp += OnKeyUpHandler;
            gameCanvas.KeyDown += OnKeyDownHandler;
        }
        public void StopMovement()
        {
            stopMovement = true;
            IsMoving = false;
        }
        public void StartMovement()
        {
            stopMovement = false;
            IsMoving = true;
        }
        public void Move()
        {
            if (!stopMovement)
            {
                double x = player.CanvasPositionX;
                double y = player.CanvasPositionY;

                if (moveLeft && moveUp)
                {
                    if (!collider.IsPathBlocked(player, -5, -5))
                    {
                        x -= MovementSpeed;
                        y -= MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.NorthWest);

                        player.SetCanvasPositionX(x);
                        player.SetCanvasPositionY(y);
                        return;
                    }
                }
                if (moveRight && moveUp)
                {
                    if (!collider.IsPathBlocked(player, 5, -5))
                    {
                        x += MovementSpeed;
                        y -= MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.NorthEast);

                        player.SetCanvasPositionX(x);
                        player.SetCanvasPositionY(y);
                        return;
                    }
                }
                if (moveLeft && moveDown)
                {
                    if (!collider.IsPathBlocked(player, -5, 5))
                    {
                        x -= MovementSpeed;
                        y += MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.SouthWest);

                        player.SetCanvasPositionX(x);
                        player.SetCanvasPositionY(y);
                        return;
                    }
                }
                if (moveRight && moveDown)
                {
                    if (!collider.IsPathBlocked(player, 5, 5))
                    {
                        x += MovementSpeed;
                        y += MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.SouthEast);

                        player.SetCanvasPositionX(x);
                        player.SetCanvasPositionY(y);
                        return;
                    }
                }
                if (moveLeft)
                {
                    if (!collider.IsPathBlocked(player, -5, 0))
                    {
                        x -= MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.West);
                    }
                }
                if (moveRight)
                {
                    if (!collider.IsPathBlocked(player, 5, 0))
                    {
                        x += MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.East);
                    }
                }
                if (moveUp)
                {
                    if (!collider.IsPathBlocked(player, 0, -5))
                    {
                        y -= MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.North);
                    }
                }
                if (moveDown)
                {
                    if (!collider.IsPathBlocked(player, 0, 5))
                    {
                        y += MovementSpeed;

                        player.RectangleBody.LayoutTransform = new RotateTransform((int)CardinalPoint.South);
                    }
                }
                player.SetCanvasPositionX(x);
                player.SetCanvasPositionY(y);
            }
        }
        public void OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (!stopMovement)
            {
                switch (e.Key)
                {
                    case Key.W:
                        moveUp = false;
                        break;
                    case Key.S:
                        moveDown = false;
                        break;
                    case Key.A:
                        moveLeft = false;
                        break;
                    case Key.D:
                        moveRight = false;
                        break;
                }

                if (!(moveUp || moveDown || moveLeft || moveRight))
                {
                    IsMoving = false;
                }
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (!stopMovement)
            {
                if (e.IsRepeat)
                {
                    return;
                }

                switch (e.Key)
                {
                    case Key.W:
                        moveUp = true;
                        IsMoving = true;
                        break;
                    case Key.S:
                        moveDown = true;
                        IsMoving = true;
                        break;
                    case Key.A:
                        moveLeft = true;
                        IsMoving = true;
                        break;
                    case Key.D:
                        moveRight = true;
                        IsMoving = true;
                        break;
                }
                Move();
            }
        }
    }

}
