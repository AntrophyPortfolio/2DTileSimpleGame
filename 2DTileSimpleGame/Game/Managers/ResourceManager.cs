using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _2DTileSimpleGame.Graphics
{
    class ResourceManager : IResourceManager
    {
        public Dictionary<FruitType, ImageBrush> FruitImages { get; } = new Dictionary<FruitType, ImageBrush>();

        public Dictionary<BlockType, ImageBrush> BlockImages { get; } = new Dictionary<BlockType, ImageBrush>();

        public Dictionary<CharacterType, ImageBrush> CharacterImages { get; } = new Dictionary<CharacterType, ImageBrush>();

        public Dictionary<MediaType, MediaPlayer> MediaFiles { get; } = new Dictionary<MediaType, MediaPlayer>();
        public Dictionary<UserInterfaceType, ImageBrush> UserInterfaceImages { get; } = new Dictionary<UserInterfaceType, ImageBrush>();
        public Dictionary<ButtonType, ImageSource> ButtonImages { get; } = new Dictionary<ButtonType, ImageSource>();

        public Random RandomGenerator { get; } = new Random();
        public ResourceManager()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            LoadSoundFiles();
            LoadFruitImages();
            LoadBlockImages();
            LoadCharacterImages();
            LoadUserInterfaceImages();
            LoadButtonImages();
        }

        private void LoadUserInterfaceImages()
        {
            UserInterfaceImages.Add(UserInterfaceType.HeartLife, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/UI/Heart.png", UriKind.Relative)) });
        }
        private void LoadButtonImages()
        {
            ButtonImages.Add(ButtonType.BackgroundImage, new BitmapImage(new Uri(@"Resources/Images/UI/PauseMenuBackground.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.QuitToMainMenu, new BitmapImage(new Uri(@"Resources/Images/UI/QuitToMenu.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.Resume, new BitmapImage(new Uri(@"Resources/Images/UI/Resume.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.SaveGame, new BitmapImage(new Uri(@"Resources/Images/UI/SaveGame.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.LoadGame, new BitmapImage(new Uri(@"Resources/Images/UI/LoadGame.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.Select, new BitmapImage(new Uri(@"Resources/Images/UI/SelectButton.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.Back, new BitmapImage(new Uri(@"Resources/Images/UI/BackButton.png", UriKind.Relative)));
            ButtonImages.Add(ButtonType.Confirm, new BitmapImage(new Uri(@"Resources/Images/UI/ConfirmButton.png", UriKind.Relative)));
        }

        private void LoadSoundFiles()
        {
            MediaPlayer musicMedia = new MediaPlayer();
            musicMedia.Open(new Uri(@"Game/Resources/SoundEffects/Music.wav", UriKind.Relative));
            MediaPlayer footstepsMedia = new MediaPlayer();
            footstepsMedia.Open(new Uri(@"Game/Resources/SoundEffects/Footsteps.wav", UriKind.Relative));
            MediaPlayer eatMedia = new MediaPlayer();
            eatMedia.Open(new Uri(@"Game/Resources/SoundEffects/EatFruitSound.wav", UriKind.Relative));
            MediaPlayer ouchMedia = new MediaPlayer();
            ouchMedia.Open(new Uri(@"Game/Resources/SoundEffects/Ouch.wav", UriKind.Relative));
            MediaPlayer showPortalMedia = new MediaPlayer();
            showPortalMedia.Open(new Uri(@"Game/Resources/SoundEffects/OpenPortalSound.wav", UriKind.Relative));

            MediaFiles.Add(MediaType.Music, musicMedia);
            MediaFiles.Add(MediaType.Footsteps, footstepsMedia);
            MediaFiles.Add(MediaType.Eat, eatMedia);
            MediaFiles.Add(MediaType.Ouch, ouchMedia);
            MediaFiles.Add(MediaType.ShowPortal, showPortalMedia);
        }

        private void LoadCharacterImages()
        {
            CharacterImages.Add(CharacterType.Player, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Characters/Player.png", UriKind.Relative)) });
            CharacterImages.Add(CharacterType.Enemy, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Characters/Enemy1.png", UriKind.Relative)) });
        }

        private void LoadBlockImages()
        {
            BlockImages.Add(BlockType.Dirt, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/Dirt.png", UriKind.Relative)) });
            BlockImages.Add(BlockType.FinishLocked, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/FinishLocked.jpg", UriKind.Relative)) });
            BlockImages.Add(BlockType.FinishUnlocked, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/FinishUnlocked.jpg", UriKind.Relative)) });
            BlockImages.Add(BlockType.Edge, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/Edge.jpg", UriKind.Relative)) });
            BlockImages.Add(BlockType.Wall, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/Wall.jpg", UriKind.Relative)) });
            BlockImages.Add(BlockType.Water, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Blocks/Water.jpg", UriKind.Relative)) });
        }

        private void LoadFruitImages()
        {
            FruitImages.Add(FruitType.Apple, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Fruits/Apple.png", UriKind.Relative)) });
            FruitImages.Add(FruitType.Banana, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Fruits/Banana.png", UriKind.Relative)) });
            FruitImages.Add(FruitType.Pineapple, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Fruits/Pineapple.png", UriKind.Relative)) });
            FruitImages.Add(FruitType.Watermelone, new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Game/Resources/Images/Fruits/Watermelone.png", UriKind.Relative)) });
        }
    }
}
