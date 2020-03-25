using _2DTileSimpleGame.Game;
using _2DTileSimpleGame.Game.Manager;
using _2DTileSimpleGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2DTileSimpleGame.Physics
{
    class Collider : ICollider
    {
        private readonly IGameManager gameManager;
        public Collider(IGameManager refGameManager)
        {
            gameManager = refGameManager;
        }
        public IGraphicsComponent GetObjectUnder(IGraphicsComponent body, params Enum[] tags)
        {
            List<Enum> listTags = tags.ToList();

            IGraphicsComponent[,,] gameMesh = gameManager.GameMesh;

            for (int i = 0; i < gameMesh.GetLength(1); i++)
            {
                for (int j = 0; j < gameMesh.GetLength(0); j++)
                {
                    if (gameMesh[i, j, 1] != null)
                    {
                        if (listTags.Contains(gameMesh[i, j, 1].ObjectContext.Tag))
                        {
                            double xObject = gameMesh[i, j, 1].CanvasPositionX;
                            double yOBject = gameMesh[i, j, 1].CanvasPositionY;
                            if (CalculateDistance(body.CanvasPositionX, xObject, body.CanvasPositionY, yOBject) < 30)
                            {
                                return gameMesh[i, j, 1];
                            }
                        }
                    }
                }
            }
            return null;
        }
        public bool IsPathBlocked(IGraphicsComponent body, double offsetX, double offsetY)
        {
            IGraphicsComponent[,,] gameMesh = gameManager.GameMesh;
            for (int i = 0; i < gameMesh.GetLength(1); i++)
            {
                for (int j = 0; j < gameMesh.GetLength(0); j++)
                {
                    IGraphicsComponent block = gameMesh[i, j, 0];
                    if (!block.ObjectContext.Tag.Equals(BlockType.Dirt) && !block.ObjectContext.Tag.Equals(BlockType.FinishLocked))
                    {
                        double xBlock = block.CanvasPositionX;
                        double yBlock = block.CanvasPositionY;
                        double xPlayer = body.CanvasPositionX + offsetX;
                        double yPlayer = body.CanvasPositionY + offsetY;

                        double distance = CalculateDistance(xBlock, xPlayer, yBlock, yPlayer); ;
                        if (distance < body.ObjectContext.Size - 10)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private double CalculateDistance(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
