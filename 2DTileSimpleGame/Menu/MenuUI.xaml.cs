using _2DTileSimpleGame.Graphics;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Path = System.IO.Path;

namespace _2DTileSimpleGame.MenuUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuUI : Window
    {
        public double MusicVolume { get; private set; }
        public double EffectsVolume { get; private set; }
        public MediaPlayer BackgroundMusic = new MediaPlayer();
        private readonly IResourceManager resourceManager = new ResourceManager();
        public MenuUI()
        {
            double monitorResolutionCanvasRatio = System.Windows.SystemParameters.WorkArea.Height - (2*System.Windows.SystemParameters.CaptionHeight);
            Application.Current.MainWindow.Height = monitorResolutionCanvasRatio;
            Application.Current.MainWindow.Width = monitorResolutionCanvasRatio;
            InitializeComponent();
            SliderEffectsVolume.Value = 0.2;
            SliderEffectsVolume.Maximum = 1;
            SliderEffectsVolume.Minimum = 0;
            SliderMusicVolume.Value = 0.02;
            SliderMusicVolume.Minimum = 0;
            SliderMusicVolume.Maximum = 0.2;
            resourceManager.MediaFiles.TryGetValue(MediaType.Music, out BackgroundMusic);
            BackgroundMusic.Volume = 0.02;
            BackgroundMusic.Play();
            BackgroundMusic.MediaEnded += delegate { BackgroundMusic.Position = TimeSpan.Zero; BackgroundMusic.Play(); };
            this.Activated += delegate { BackOnClick(null, null); };
        }
        private void BackgroundVideo_Ended(object sender, RoutedEventArgs e)
        {
            backgroundVideo.Position = TimeSpan.Zero;
            backgroundVideo.Play();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        private void SettingsOnClick(object sender, RoutedEventArgs e)
        {
            PlayGameButton.Visibility = Visibility.Hidden;
            LoadGameButton.Visibility = Visibility.Hidden;
            QuitGameButton.Visibility = Visibility.Hidden;
            SettingsButton.SetValue(Grid.RowProperty, 0);
            SettingsButton.IsEnabled = false;
            BackButton.Visibility = Visibility.Visible;
            SliderMusicVolume.Visibility = Visibility.Visible;
            TextSliderMusicVolume.Visibility = Visibility.Visible;
            SliderEffectsVolume.Visibility = Visibility.Visible;
            TextSliderEffectsVolume.Visibility = Visibility.Visible;
        }
        private void LoadMainMenu()
        {
            PlayGameButton.Visibility = Visibility.Visible;
            LoadGameButton.Visibility = Visibility.Visible;
            SettingsButton.Visibility = Visibility.Visible;
            SettingsButton.IsEnabled = true;
            LoadGameButton.IsEnabled = true;
            ListBoxLoad.Visibility = Visibility.Hidden;
            SettingsButton.SetValue(Grid.RowProperty, 2);
            LoadGameButton.SetValue(Grid.RowProperty, 1);
            ButtonGrid.RowDefinitions[1].Height = new GridLength(17, GridUnitType.Star);
            ButtonGrid.RowDefinitions[2].Height = new GridLength(17, GridUnitType.Star);

            QuitGameButton.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Hidden;
            TextSliderMusicVolume.Visibility = Visibility.Hidden;
            SliderMusicVolume.Visibility = Visibility.Hidden;
            SliderEffectsVolume.Visibility = Visibility.Hidden;
            TextSliderEffectsVolume.Visibility = Visibility.Hidden;
        }
        private void BackOnClick(object sender, RoutedEventArgs e)
        {
            LoadMainMenu();
            LoadGameButton.Margin = new Thickness(0, 0, 0, 0);
            SettingsButton.Margin = new Thickness(0, 0, 0, 0);

            SelectButton.Visibility = Visibility.Hidden;
        }
        private void SliderMusicVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BackgroundMusic.Volume = e.NewValue;
            MusicVolume = e.NewValue;
        }
        private void PlayGameOnClick(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(this, null, false);
            gameWindow.Show();
            this.Visibility = Visibility.Hidden;
            backgroundVideo.Stop();
        }
        private void QuitGameOnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void SliderEffectsVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            resourceManager.MediaFiles.TryGetValue(MediaType.Eat, out MediaPlayer eat);
            eat.Volume = e.NewValue;
            eat.Play();
            eat.Position = TimeSpan.Zero;
            EffectsVolume = e.NewValue;
        }

        private void LoadGameOnClick(object sender, RoutedEventArgs e)
        {
            PlayGameButton.Visibility = Visibility.Hidden;
            LoadGameButton.Visibility = Visibility.Visible;
            SelectButton.Visibility = Visibility.Visible;
            QuitGameButton.Visibility = Visibility.Hidden;
            LoadGameButton.SetValue(Grid.RowProperty, 0);
            ButtonGrid.RowDefinitions[1].Height = new GridLength(25, GridUnitType.Star);
            ButtonGrid.RowDefinitions[2].Height = new GridLength(9, GridUnitType.Star);
            LoadGameButton.IsEnabled = false;
            SettingsButton.Visibility = Visibility.Hidden;
            BackButton.Visibility = Visibility.Visible;

            ListBoxLoad.Items.Clear();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var item in files)
            {
                if (item.Contains(".dat"))
                {
                    ListBoxLoad.Items.Add(Path.GetFileName(item).Replace(".dat", ""));
                }
            }
            ListBoxLoad.MouseDoubleClick += SelectOnClick;
            ListBoxLoad.Visibility = Visibility.Visible;

        }
        private void SelectOnClick(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(this, ListBoxLoad.SelectedItem.ToString(), true);
            gameWindow.Show();
            this.Visibility = Visibility.Hidden;
        }
    }
}
