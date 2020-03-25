using _2DTileSimpleGame.Game;
using _2DTileSimpleGame.Graphics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DTileSimpleGame.UserInterface
{
    class GameUI : IUserInterface
    {
        private readonly Canvas gameCanvas;
        private readonly ILives lives;
        private readonly IGameManager gameManager;
        private readonly IResourceManager resourceManager;
        private TextBlock currentLevelText;
        private TextBlock remainingFruitsText;
        private Rectangle menuOverlay;
        private Grid pauseControls;
        private Grid buttonsGrid;

        public GameUI(Canvas refGameCanvas, IResourceManager refResourceManager, IGameManager refGameManager)
        {
            gameCanvas = refGameCanvas;
            lives = new Lives(refResourceManager);
            gameManager = refGameManager;
            resourceManager = refResourceManager;
        }
        public void CreateUserInterface(int numberOfLives)
        {
            ShowLives(numberOfLives);
            ShowMenuLabel();
            ShowRemainingFruits();
            ShowCurrentLevel();
        }

        private void ShowMenuLabel()
        {
            Label menuLabel = new Label
            {
                Content = "Press ESC for pause menu...",
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(Colors.Black)
            };
            Canvas.SetLeft(menuLabel, gameCanvas.Width / 100.8);
            Canvas.SetTop(menuLabel, gameCanvas.Height / 25.2);
            gameCanvas.Children.Add(menuLabel);
        }

        public void ShowPauseGameControls()
        {
            HidePauseGameControls();
            menuOverlay = new Rectangle();
            pauseControls = new Grid();
            InitializePauseMenu();
            menuOverlay.Visibility = Visibility.Visible;
            pauseControls.Visibility = Visibility.Visible;
        }

        private void InitializePauseMenu()
        {
            GreyOutGame();
            LoadBackgroundForButtons();
            AddButtons();
        }

        private void AddButtons()
        {
            buttonsGrid = InitializeButtonGrid();

            Button resumeBtn = CreateButton(ButtonType.Resume, 0, 0);
            resumeBtn.Click += delegate { gameManager.UnPauseGame(); };
            resumeBtn.Margin = new Thickness(0, gameCanvas.Height / 6.72, 0, 0);

            Button saveGameBtn = CreateButton(ButtonType.SaveGame, 1, 0);
            saveGameBtn.Click += delegate { ShowSaveGameWindow(); };

            Button loadGameBtn = CreateButton(ButtonType.LoadGame, 2, 0);
            loadGameBtn.Click += delegate { ShowLoadGameWindow(); };

            Button quitToMenuBtn = CreateButton(ButtonType.QuitToMainMenu, 3, 0);
            quitToMenuBtn.Click += delegate { gameManager.QuitToMenu(); };
            quitToMenuBtn.Margin = new Thickness(0, gameCanvas.Height / 6.72, 0, 0);
            pauseControls.Children.Add(buttonsGrid);
        }

        private void ShowSaveGameWindow()
        {
            HideButtons();

            Button saveGameBtn = CreateButton(ButtonType.SaveGame, 0, 0);
            saveGameBtn.Margin = new Thickness(0, gameCanvas.Height / 8.06, 0, 0);
            saveGameBtn.IsEnabled = false;

            StackPanel userInputPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, gameCanvas.Height / 10.8, 0, 0)
            };

            Label label = new Label
            {
                Content = "Save Name:",
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = gameCanvas.Width / 40.32,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBox textBox = new TextBox
            {
                Text = $"GameLevel{gameManager.CurrentLevel}",
                Background = new SolidColorBrush(Colors.SkyBlue),
                Foreground = new SolidColorBrush(Colors.Black),
                Height = gameCanvas.Height / 27,
                Width = gameCanvas.Width / 5.04,
                FontSize = gameCanvas.Width / 40.32,
                BorderBrush = new SolidColorBrush(Colors.DarkSlateBlue),
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center
            };

            userInputPanel.Children.Add(label);
            userInputPanel.Children.Add(textBox);
            Grid.SetRow(userInputPanel, 1);
            Grid.SetColumn(userInputPanel, 0);
            buttonsGrid.Children.Add(userInputPanel);

            Button okButton = CreateButton(ButtonType.Confirm, 2, 0);
            okButton.Width = gameCanvas.Width / 6.72;
            okButton.Click += delegate
            {
                if (textBox.Text.Length == 0)
                {
                    gameManager.SaveGame($"GameLevel{gameManager.CurrentLevel}");
                    textBox.Text = $"GameLevel{gameManager.CurrentLevel}";
                }
                else
                {
                    gameManager.SaveGame(textBox.Text);
                    ShowPauseGameControls();
                }
            };
            Button backBtn = CreateButton(ButtonType.Back, 3, 0);
            backBtn.Click += delegate { ShowPauseGameControls(); };
            backBtn.Margin = new Thickness(0, 0, 0, gameCanvas.Height / 20.16);
        }

        private void HideButtons()
        {
            foreach (Control item in buttonsGrid.Children)
            {
                item.Visibility = Visibility.Hidden;
            }
        }
        private void ShowLoadGameWindow()
        {
            HideButtons();
            Button loadGameBtn = CreateButton(ButtonType.LoadGame, 0, 0);
            loadGameBtn.Margin = new Thickness(0, gameCanvas.Height / 6.72, 0, 0);
            loadGameBtn.IsEnabled = false;

            Button backBtn = CreateButton(ButtonType.Back, 3, 0);
            backBtn.Click += delegate { ShowPauseGameControls(); };
            backBtn.Margin = new Thickness(0, 0, 0, gameCanvas.Height / 20.16);

            ListBox listBox = new ListBox
            {
                Height = gameCanvas.Height / 9.16,
                SelectionMode = SelectionMode.Single,
                FontSize = gameCanvas.Width / 40.32,
                Background = new SolidColorBrush(Colors.SkyBlue),
                Foreground = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.DarkSlateBlue),
                Padding = new Thickness(gameCanvas.Width / 100.8),
                BorderThickness = new Thickness(5),
                Margin = new Thickness(0, gameCanvas.Height / 10.08, 0, 0)
            };

            Button selectBtn = CreateButton(ButtonType.Select, 2, 0);
            selectBtn.Click += delegate { gameManager.LoadGame(listBox.SelectedItem.ToString()); gameManager.UnPauseGame(); };
            selectBtn.Width = gameCanvas.Width / 6.72;

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var item in files)
            {
                if (item.Contains(".dat"))
                {
                    listBox.Items.Add(System.IO.Path.GetFileName(item).Replace(".dat", ""));
                }
            }
            Grid.SetRow(listBox, 1);
            Grid.SetColumn(listBox, 0);
            buttonsGrid.Children.Add(listBox);
            listBox.MouseDoubleClick += delegate { gameManager.LoadGame(listBox.SelectedItem.ToString()); gameManager.UnPauseGame(); };
            listBox.Visibility = Visibility.Visible;
        }

        private Grid InitializeButtonGrid()
        {
            Grid buttonsGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = gameCanvas.Height / 1.44
            };
            RowDefinition row1 = new RowDefinition
            {
                Height = GridLength.Auto
            };
            RowDefinition row2 = new RowDefinition
            {
                Height = GridLength.Auto
            };
            RowDefinition row3 = new RowDefinition
            {
                Height = GridLength.Auto
            };
            RowDefinition row4 = new RowDefinition
            {
                Height = GridLength.Auto
            };
            ColumnDefinition col1 = new ColumnDefinition
            {
                Width = new GridLength(gameCanvas.Width / 2.52)
            };

            buttonsGrid.RowDefinitions.Add(row1);
            buttonsGrid.RowDefinitions.Add(row2);
            buttonsGrid.RowDefinitions.Add(row3);
            buttonsGrid.RowDefinitions.Add(row4);
            buttonsGrid.ColumnDefinitions.Add(col1);

            return buttonsGrid;
        }

        private Button CreateButton(ButtonType type, int row, int column)
        {
            resourceManager.ButtonImages.TryGetValue(type, out ImageSource imgSource);

            Image img = new Image
            {
                Source = imgSource
            };

            StackPanel stackPnl = new StackPanel();
            stackPnl.Children.Add(img);

            Button newBtn = new Button
            {
                Content = stackPnl,
                Width = gameCanvas.Width / 4,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Style = Application.Current.FindResource("MyButton") as Style,
            };

            Grid.SetRow(newBtn, row);
            Grid.SetColumn(newBtn, column);
            buttonsGrid.Children.Add(newBtn);
            return newBtn;
        }
        private void LoadBackgroundForButtons()
        {
            pauseControls = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            resourceManager.ButtonImages.TryGetValue(ButtonType.BackgroundImage, out ImageSource imgBackground);
            Image img = new Image
            {
                Width = gameCanvas.Width - gameCanvas.Width / 4,
                Height = gameCanvas.Height,
                Source = imgBackground
            };
            gameCanvas.Children.Add(pauseControls);

            Canvas.SetLeft(pauseControls, (gameCanvas.Width / 2) - img.Width / 2);
            Canvas.SetTop(pauseControls, (gameCanvas.Height / 2) - (img.Height / 2));
            pauseControls.Children.Add(img);
        }

        private void GreyOutGame()
        {
            menuOverlay = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.Black),
                Width = gameCanvas.Width,
                Height = gameCanvas.Height,
                Opacity = 0.5
            };
            Canvas.SetLeft(menuOverlay, 0);
            Canvas.SetTop(menuOverlay, 0);
            gameCanvas.Children.Add(menuOverlay);
        }

        public void HidePauseGameControls()
        {
            if (menuOverlay != null && pauseControls != null)
            {
                menuOverlay.Visibility = Visibility.Hidden;
                pauseControls.Visibility = Visibility.Hidden;
            }
        }
        public void UpdateFruit()
        {
            if (gameManager.FruitsCounter == 0)
            {
                remainingFruitsText.Text = "Portal activated!";
                remainingFruitsText.Width = gameCanvas.Width / 3.36;
                Canvas.SetLeft(remainingFruitsText, gameCanvas.Width - remainingFruitsText.Width);
            }
            else
            {
                remainingFruitsText.Text = $"Collect {gameManager.FruitsCounter.ToString()} fruits";
            }
        }
        public void UpdateLevel()
        {
            currentLevelText.Text = $"LEVEL: {gameManager.CurrentLevel.ToString()}";
        }
        private void ShowCurrentLevel()
        {
            currentLevelText = new TextBlock
            {
                Text = $"LEVEL: {gameManager.CurrentLevel.ToString()}",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Yellow),
                FontSize = gameCanvas.Width / 33.6,
                Width = gameCanvas.Width / 6.72
            };
            Canvas.SetLeft(currentLevelText, (gameCanvas.Width - currentLevelText.Width) / 2);
            Canvas.SetTop(currentLevelText, 0);
            gameCanvas.Children.Add(currentLevelText);
        }
        private void ShowRemainingFruits()
        {
            remainingFruitsText = new TextBlock
            {
                Text = $"Collect {gameManager.FruitsCounter.ToString()} fruits",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Yellow),
                FontSize = gameCanvas.Width / 33.6,
                Width = gameCanvas.Width / 4.9
            };
            Canvas.SetLeft(remainingFruitsText, gameCanvas.Width - remainingFruitsText.Width);
            Canvas.SetTop(remainingFruitsText, 0);
            gameCanvas.Children.Add(remainingFruitsText);
        }
        public Rectangle RemoveLife()
        {
            return lives.RemoveHeart();
        }
        private void ShowLives(int numberOfLives)
        {
            TextBlock livesText = lives.AddLiveText();
            livesText.FontSize = gameCanvas.Width / 33.6;
            gameCanvas.Children.Add(livesText);
            for (int i = 0; i < numberOfLives; i++)
            {
                Rectangle heart = lives.AddHeart();
                heart.Width = gameCanvas.Width / 25.2;
                heart.Height = gameCanvas.Height / 25.2;
                gameCanvas.Children.Add(heart);
            }
        }

        public void ResetHeartsLayoutX()
        {
            lives.ResetLayoutX();
        }

        public void ShowDeathMessage()
        {
            GreyOutGame();
            Label textDeath = new Label
            {
                Content = "YOU HAVE DIED!",
                Foreground = new SolidColorBrush(Colors.Red),
                FontSize = gameCanvas.Width / 10.8,
                FontWeight = FontWeights.Bold,
            };

            textDeath.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            textDeath.Width = textDeath.DesiredSize.Width;
            textDeath.Height = textDeath.DesiredSize.Height;

            gameCanvas.Children.Add(textDeath);
            Canvas.SetLeft(textDeath, gameCanvas.Width / 2 - textDeath.Width / 2);
            Canvas.SetTop(textDeath, gameCanvas.Height / 2 - textDeath.Height / 2 - gameCanvas.Height / 10.8);
            resourceManager.ButtonImages.TryGetValue(ButtonType.QuitToMainMenu, out ImageSource imgSource);

            Image img = new Image
            {
                Source = imgSource
            };

            StackPanel stackPnl = new StackPanel();
            stackPnl.Children.Add(img);

            Button quitToMainMenu = new Button
            {
                Content = stackPnl,
                Width = gameCanvas.Width / 4,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Style = Application.Current.FindResource("MyButton") as Style,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            quitToMainMenu.Click += delegate { gameManager.QuitToMenu(); };
            Canvas.SetLeft(quitToMainMenu, gameCanvas.Width / 2 - (quitToMainMenu.Width / 2));
            Canvas.SetTop(quitToMainMenu, gameCanvas.Height / 2);
            gameCanvas.Children.Add(quitToMainMenu);
        }
    }
}
