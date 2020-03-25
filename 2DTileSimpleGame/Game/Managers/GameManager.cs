using _2DTileSimpleGame.Animations;
using _2DTileSimpleGame.Game;
using _2DTileSimpleGame.Game.Manager;
using _2DTileSimpleGame.Graphics;
using _2DTileSimpleGame.Physics;
using _2DTileSimpleGame.Physics.AI;
using _2DTileSimpleGame.UserInterface;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.GameLogic
{
    class GameManager : IGameManager
    {
        public int CurrentLevel { get; set; } = 0;
        public int FruitsCounter { get; private set; } = 0;
        public IGraphicsComponent[,,] GameMesh { get; set; }
        private int numberOfEnemies = 2;
        private int numberOfFruits = 3;
        private readonly Canvas gameCanvas;
        private readonly IResourceManager resourceManager = new ResourceManager();
        private readonly ICollider collider;
        private readonly ILevelCreator levelCreator;
        private IMovable playerMovement;
        private readonly IUserInterface gameUI;
        private readonly IAnimator animator = new Animator();
        private readonly List<IMovable> enemyAIMovement = new List<IMovable>();
        private readonly GameWindow gameWindow;
        private readonly MenuUI.MenuUI mainMenuWindow;
        private IGraphicsComponent player;
        private Timer globalTimer;
        private readonly Timer recentlyHitTimer = new Timer { Interval = 1000 };
        private bool isRecentlyHit = false;
        private int numberOfLives = 3;
        private bool isLoaded = false;
        private bool showPortalPlayed = false;

        public GameManager(Canvas refCanvas, GameWindow refMainWindow, MenuUI.MenuUI mainMenuWindow)
        {
            gameCanvas = refCanvas;
            collider = new Collider(this);
            levelCreator = new LevelCreator(this, resourceManager, gameCanvas);
            gameWindow = refMainWindow;
            gameWindow.Activated += delegate
            {
                if (!isLoaded)
                {
                    isLoaded = true;
                    StartTimer();
                    DoMovement();
                }
            };
            gameUI = new GameUI(gameCanvas, resourceManager, this);
            this.mainMenuWindow = mainMenuWindow;
            refMainWindow.KeyDown += RefMainWindow_KeyDown;

        }

        private void RefMainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
            {
                if (globalTimer.Enabled)
                {
                    PauseGame();
                    gameUI.ShowPauseGameControls();
                }
                else
                {
                    UnPauseGame();
                }
            }
        }

        public void PauseGame()
        {
            globalTimer.Stop();
            recentlyHitTimer.Stop();
            playerMovement.StopMovement();
            foreach (var item in enemyAIMovement)
            {
                item.StopMovement();
            }
            foreach (var item in resourceManager.MediaFiles)
            {
                if (!item.Key.Equals(MediaType.Music))
                {
                    item.Value.Stop();
                }
            }
        }
        public void UnPauseGame()
        {
            globalTimer.Start();
            recentlyHitTimer.Start();
            playerMovement.StartMovement();
            foreach (var item in enemyAIMovement)
            {
                item.StartMovement();
            }
            gameUI.HidePauseGameControls();
        }

        public void StartGame()
        {
            LoadNewLevel(numberOfFruits, numberOfEnemies);
        }

        public void QuitToMenu()
        {
            gameCanvas.Children.Clear();
            gameWindow.Hide();
            Application.Current.MainWindow.Show();
        }
        private void FixedUpdate(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                resourceManager.MediaFiles.TryGetValue(MediaType.Footsteps, out MediaPlayer soundFootSteps);
                resourceManager.MediaFiles.TryGetValue(MediaType.Eat, out MediaPlayer soundEatFruit);
                resourceManager.MediaFiles.TryGetValue(MediaType.Ouch, out MediaPlayer soundOuch);
                resourceManager.MediaFiles.TryGetValue(MediaType.ShowPortal, out MediaPlayer soundShowPortal);
                soundFootSteps.Volume = mainMenuWindow.EffectsVolume;
                soundOuch.Volume = mainMenuWindow.EffectsVolume;
                soundShowPortal.Volume = mainMenuWindow.EffectsVolume;
                soundEatFruit.Volume = mainMenuWindow.EffectsVolume;

                if (FruitsCounter == 0 && !showPortalPlayed)
                {
                    levelCreator.SpawnFinishPoint(BlockType.FinishUnlocked);
                    soundShowPortal.Position = TimeSpan.Zero;
                    soundShowPortal.Play();
                    showPortalPlayed = true;
                }
                DoMovement();
                if (playerMovement.IsMoving)
                {
                    soundFootSteps.Play();
                }
                else
                {
                    soundFootSteps.Stop();
                }
                IGraphicsComponent fruit = collider.GetObjectUnder(player, FruitType.Apple, FruitType.Banana, FruitType.Pineapple, FruitType.Watermelone);
                if (fruit != null)
                {
                    soundEatFruit.Position = TimeSpan.Zero;
                    soundEatFruit.Play();

                    DeleteFruit(fruit);
                    animator.AnimationRescaling(player.RectangleBody, player.ObjectContext.Size, (player.ObjectContext.Size + player.ObjectContext.Size / 3), true, 200);

                }
                if (collider.GetObjectUnder(player, BlockType.FinishUnlocked) != null)
                {
                    LoadNewLevel(++numberOfFruits, ++numberOfEnemies);
                    showPortalPlayed = false;
                }
                if (collider.GetObjectUnder(player, CharacterType.Enemy) != null && !isRecentlyHit)
                {
                    isRecentlyHit = true;
                    recentlyHitTimer.Start();

                    animator.AnimationFlashing(player.RectangleBody, 200, 5, 1.0, 0.2, false);

                    Rectangle heart = gameUI.RemoveLife();
                    animator.AnimationFlashing(heart, 200, 5, 1.0, 0.2, true);
                    gameUI.RemoveLife();
                    soundOuch.Position = TimeSpan.Zero;
                    soundOuch.Play();
                    numberOfLives--;
                }
                if (numberOfLives == 0)
                {
                    PauseGame();
                    gameUI.ShowDeathMessage();
                }
            });
        }
        private void DoMovement()
        {
            playerMovement.Move();
            foreach (var enemy in enemyAIMovement)
            {
                enemy.Move();
            }
        }
        private void StartTimer()
        {
            globalTimer = new Timer
            {
                Interval = 20,
                Enabled = true,
                AutoReset = true
            };
            globalTimer.Elapsed += FixedUpdate;
            recentlyHitTimer.Elapsed += delegate
            {
                isRecentlyHit = false;
                recentlyHitTimer.Stop();
            };
        }
        private void DeleteFruit(IGraphicsComponent fruit)
        {
            fruit.SetCanvasPositionX(2000);
            fruit.SetCanvasPositionY(2000);
            gameCanvas.Children.Remove(fruit.RectangleBody);

            FruitsCounter--;
            gameUI.UpdateFruit();
        }
        private void LoadNewLevel(int numberOfFruits, int numberOfEnemies)
        {
            CurrentLevel++;
            gameCanvas.Children.Clear();
            levelCreator.CreateLevel(numberOfFruits, numberOfEnemies);
            player = levelCreator.Player;
            FruitsCounter = numberOfFruits;

            foreach (var character in GameMesh)
            {
                if (character != null)
                {
                    if (character.ObjectContext.Tag.Equals(CharacterType.Enemy))
                    {
                        IMovable enemyMovement = new EnemyMovement(character, collider, resourceManager);
                        enemyAIMovement.Add(enemyMovement);
                    }
                    else if (character.ObjectContext.Tag.Equals(CharacterType.Player))
                    {
                        playerMovement = new PlayerMovement(player, collider, gameCanvas);

                    }
                }
            }
            gameUI.ResetHeartsLayoutX();
            gameUI.CreateUserInterface(numberOfLives);
        }

        public void LoadGame(string filename)
        {
            levelCreator.LoadLevel(filename, out int fruitsCounter, out int levelCounter);
            FruitsCounter = fruitsCounter;
            CurrentLevel = levelCounter;
            player = levelCreator.Player;
            foreach (var character in GameMesh)
            {
                if (character != null)
                {
                    if (character.ObjectContext.Tag.Equals(CharacterType.Enemy))
                    {
                        IMovable enemyMovement = new EnemyMovement(character, collider, resourceManager);
                        enemyAIMovement.Add(enemyMovement);
                    }
                    else if (character.ObjectContext.Tag.Equals(CharacterType.Player))
                    {
                        playerMovement = new PlayerMovement(player, collider, gameCanvas);

                    }
                }
            }
            gameUI.ResetHeartsLayoutX();
            gameUI.CreateUserInterface(numberOfLives);
        }
        public void SaveGame(string name)
        {
            levelCreator.SaveLevel(name);
        }
    }
}
