using _2DTileSimpleGame.Game;
using _2DTileSimpleGame.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Media3D;
using _2DTileSimpleGame.Game.Manager;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace _2DTileSimpleGame.GameLogic
{
    class LevelCreator : ILevelCreator
    {
        public IGraphicsComponent Player { get; set; }
        private const int numberOfBlocks = 16;
        private readonly IResourceManager resourceManager;
        private readonly IGameManager gameManager;
        private readonly Canvas gameCanvas;
        private IGraphicsComponent[,,] gameMesh;
        public LevelCreator(IGameManager refGameManager, IResourceManager refResourceManager, Canvas refGameCanvas)
        {
            resourceManager = refResourceManager;
            gameCanvas = refGameCanvas;
            gameManager = refGameManager;
        }

        public void SaveLevel(string fileName)
        {
            IGameObject[,,] gameMeshObjects = new IGameObject[numberOfBlocks, numberOfBlocks, 2];
            for (int i = 0; i < gameMeshObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gameMeshObjects.GetLength(1); j++)
                {
                    for (int z = 0; z < gameMeshObjects.GetLength(2); z++)
                    {
                        if (gameMesh[i, j, z] != null)
                        {
                            gameMeshObjects[i, j, z] = gameMesh[i, j, z].ObjectContext;
                        }
                    }
                }
            }
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream($"{fileName}.dat", FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, gameMeshObjects);

            };

            using (FileStream fsInt = new FileStream($"{fileName}.dat", FileMode.Append, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fsInt);
                bw.Write(gameManager.CurrentLevel);
            }
        }
        public void LoadLevel(string fileName, out int numberOfFruits, out int numberOfLevels)
        {
            gameMesh = new IGraphicsComponent[numberOfBlocks, numberOfBlocks, 2];
            numberOfFruits = 0;
            Stack<IGameObject> characters = new Stack<IGameObject>();
            Stack<IGameObject> fruits = new Stack<IGameObject>();

            IGameObject[,,] gameMeshObjects;
            using (FileStream fs = new FileStream($"{fileName}.dat", FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                BinaryFormatter bf = new BinaryFormatter();
                gameMeshObjects = (IGameObject[,,])bf.Deserialize(fs);
                numberOfLevels = br.Read();
            };

            for (int i = 0; i < gameMeshObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gameMeshObjects.GetLength(1); j++)
                {
                    for (int z = 0; z < gameMeshObjects.GetLength(2); z++)
                    {
                        if (gameMeshObjects[i, j, z] != null)
                        {
                            if (gameMeshObjects[i, j, z].ClassType.Equals(ClassType.GameCharacter))
                            {
                                characters.Push(gameMeshObjects[i, j, z]);
                                continue;
                            }
                            if (gameMeshObjects[i, j, z].ClassType.Equals(ClassType.GameFruit))
                            {
                                fruits.Push(gameMeshObjects[i, j, z]);
                                continue;
                            }
                            gameMesh[i, j, z] = new GraphicsComponent(gameMeshObjects[i, j, z], resourceManager, gameCanvas);
                        }
                    }
                }
            }
            foreach (var fruit in fruits)
            {
                gameMesh[(int)fruit.Coordinates.X, (int)fruit.Coordinates.Y, (int)fruit.Coordinates.Z] = new GraphicsComponent(fruit, resourceManager, gameCanvas);
                numberOfFruits++;
            }
            foreach (var item in characters)
            {
                Player = gameMesh[(int)item.Coordinates.X, (int)item.Coordinates.Y, (int)item.Coordinates.Z] = new GraphicsComponent(item, resourceManager, gameCanvas);
            }
            SetGameGrid();
        }

        public void CreateLevel(int refNumberOfFruits, int refNumberOfEnemies)
        {
            IPathFinding<IGraphicsComponent> pathFinder;
            do
            {
                pathFinder = new PathFinding<IGraphicsComponent>();
                CreateFloor();
                SpawnFinishPoint(BlockType.FinishLocked);
                SpawnPlayer();
                SpawnEnemies(refNumberOfEnemies);
                SpawnFruits(refNumberOfFruits);
                InitializePathFinding(pathFinder);
            } while (!pathFinder.PathExists());
            SetGameGrid();
        }
        private void SetGameGrid()
        {
            gameManager.GameMesh = gameMesh;
        }
        public void SpawnFinishPoint(BlockType type)
        {
            if (!type.Equals(BlockType.FinishLocked) && !type.Equals(BlockType.FinishUnlocked))
            {
                throw new ArgumentException("Invalid finish type parameter!");
            }
            IGameObject finishPoint = new GameBlock(type, new Point3D(numberOfBlocks - 2, numberOfBlocks - 2, 1));
            IGraphicsComponent portalObject = new GraphicsComponent(finishPoint, resourceManager, gameCanvas);
            gameMesh[numberOfBlocks - 2, numberOfBlocks - 2, 1] = portalObject;
        }

        private void InitializePathFinding(IPathFinding<IGraphicsComponent> pathFinder)
        {
            pathFinder.InitialUnit = Player;
            pathFinder.ArrayGrid3D = gameMesh;
            pathFinder.ObstaclesAddAll(GetAllItemsWithTags(BlockType.Wall, BlockType.Water, BlockType.Edge).ToArray());
            pathFinder.TargetsAddAll(GetAllItemsWithTags(FruitType.Apple, FruitType.Banana, FruitType.Pineapple, FruitType.Watermelone, CharacterType.Enemy, BlockType.FinishLocked).ToArray());
        }
        private List<IGraphicsComponent> GetAllItemsWithTags(params Enum[] tags)
        {
            List<Enum> enumList = tags.OfType<Enum>().ToList();

            List<IGraphicsComponent> list = new List<IGraphicsComponent>();
            foreach (var item in gameMesh)
            {
                if (item != null)
                {
                    if (enumList.Contains(item.ObjectContext.Tag))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        private void CreateFloor()
        {
            gameMesh = new IGraphicsComponent[numberOfBlocks, numberOfBlocks, 2];
            for (int i = 0; i < gameMesh.GetLength(1); ++i)
            {
                for (int j = 0; j < gameMesh.GetLength(0); ++j)
                {
                    IGameObject block;
                    if (IsSpawnPoint(i, j))
                    {
                        block = new GameBlock(BlockType.Dirt, new Point3D(i, j, 0));
                    }
                    else if (IsFinishPoint(i, j))
                    {
                        block = new GameBlock(BlockType.Dirt, new Point3D(i, j, 0));
                        IGameObject blockFinish = new GameBlock(BlockType.Dirt, new Point3D(i, j, 0));
                        IGraphicsComponent portalObject = new GraphicsComponent(blockFinish, resourceManager, gameCanvas);
                        gameMesh[i, j, 1] = portalObject;
                    }

                    else if (IsEdgeOfMap(i, j))
                    {
                        block = new GameBlock(BlockType.Edge, new Point3D(i, j, 0));
                    }
                    else
                    {
                        block = new GameBlock(resourceManager, new Point3D(i, j, 0));
                    }
                    IGraphicsComponent blockObject = new GraphicsComponent(block, resourceManager, gameCanvas);
                    gameMesh[i, j, 0] = blockObject;
                }
            }
        }
        private void SpawnPlayer()
        {
            IGameObject playerObject = new GameCharacter(CharacterType.Player, new Point3D(1, 1, 1));
            Player = new GraphicsComponent(playerObject, resourceManager, gameCanvas);
            Player.RectangleBody.LayoutTransform = new RotateTransform(90);
            gameMesh[1, 1, 1] = Player;
        }

        private void SpawnFruits(int numberOfFruits)
        {
            for (int i = 0; i < numberOfFruits; i++)
            {
                IGraphicsComponent spawnPoint = GetRandomSpawnPoint(50);

                IGameObject fruit = new GameFruit(resourceManager, new Point3D(spawnPoint.ObjectContext.Coordinates.X, spawnPoint.ObjectContext.Coordinates.Y, 1));
                IGraphicsComponent fruitObject = new GraphicsComponent(fruit, resourceManager, gameCanvas);

                for (int x = 0; x < gameMesh.GetLength(1); x++)
                {
                    for (int y = 0; y < gameMesh.GetLength(0); y++)
                    {
                        if (gameMesh[x, y, 0].Equals(spawnPoint))
                        {
                            gameMesh[x, y, 1] = fruitObject;
                        }
                    }
                }
            }
        }

        private void SpawnEnemies(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                IGraphicsComponent spawnPoint = GetRandomSpawnPoint(500);
                IGameObject enemy = new GameCharacter(CharacterType.Enemy, new Point3D(spawnPoint.ObjectContext.Coordinates.X, spawnPoint.ObjectContext.Coordinates.Y, 1));
                IGraphicsComponent enemyObject = new GraphicsComponent(enemy, resourceManager, gameCanvas);
                for (int x = 0; x < gameMesh.GetLength(1); x++)
                {
                    for (int y = 0; y < gameMesh.GetLength(0); y++)
                    {
                        if (gameMesh[x, y, 0].Equals(spawnPoint))
                        {
                            gameMesh[x, y, 1] = enemyObject;
                        }
                    }
                }
            }
        }
        private IGraphicsComponent GetRandomSpawnPoint(int radius)
        {
            List<IGraphicsComponent> potentialSpawnPoints = GetPotentialSpawnPointsGreaterThanRadius(radius);

            int randomIndex = resourceManager.RandomGenerator.Next(0, potentialSpawnPoints.Count);

            return potentialSpawnPoints[randomIndex];
        }

        private List<IGraphicsComponent> GetPotentialSpawnPointsGreaterThanRadius(int radius)
        {
            List<IGraphicsComponent> potentialSpawnPointList = new List<IGraphicsComponent>();

            for (int i = 0; i < gameMesh.GetLength(1); i++)
            {
                for (int j = 0; j < gameMesh.GetLength(0); j++)
                {
                    IGraphicsComponent block = gameMesh[i, j, 0];
                    if (block.ObjectContext.Tag.Equals(BlockType.Dirt))
                    {
                        if (CalculateDistance(Player.CanvasPositionX, block.CanvasPositionX, Player.CanvasPositionX, block.CanvasPositionY) > radius)
                        {
                            if (!IsAnyObjectAlreadyThere(block))
                            {
                                potentialSpawnPointList.Add(block);
                            }
                        }
                    }
                }
            }
            return potentialSpawnPointList;
        }
        private bool IsAnyObjectAlreadyThere(IGraphicsComponent block)
        {
            for (int i = 0; i < gameMesh.GetLength(1); i++)
            {
                for (int j = 0; j < gameMesh.GetLength(0); j++)
                {
                    if (gameMesh[i, j, 0].Equals(block))
                    {
                        if (gameMesh[i, j, 1] != null)
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
        private bool IsEdgeOfMap(int x, int y)
        {
            return x == 0 || y == 0 || x == (numberOfBlocks - 1) || y == (numberOfBlocks - 1);
        }

        private bool IsFinishPoint(int x, int y)
        {
            return x == numberOfBlocks - 2 && y == numberOfBlocks - 2;
        }

        private bool IsSpawnPoint(int x, int y)
        {
            return x == 1 && y == 1;
        }
    }
}
