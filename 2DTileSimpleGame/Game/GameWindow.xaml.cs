using _2DTileSimpleGame.Game;
using _2DTileSimpleGame.GameLogic;
using System;
using System.Windows;

namespace _2DTileSimpleGame
{
    /// <summary>
    /// Delegates all needed things to game manager
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow(MenuUI.MenuUI mainMenu, string gameFileName, bool isLoadingGame)
        {
            double monitorResolutionCanvasRatio = System.Windows.SystemParameters.WorkArea.Height - (2 * System.Windows.SystemParameters.CaptionHeight);
            InitializeComponent();
            GameCanvas.Focus();
            GameCanvas.Width = monitorResolutionCanvasRatio;
            GameCanvas.Height = monitorResolutionCanvasRatio;
            IGameManager gameManager = new GameManager(GameCanvas, this, mainMenu);
            this.Closed += delegate { Environment.Exit(0); };
            if (isLoadingGame)
            {
                gameManager.LoadGame(gameFileName);
            }
            else
            {
                gameManager.StartGame();
            }
        }
    }
}
