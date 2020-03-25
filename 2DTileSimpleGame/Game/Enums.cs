namespace _2DTileSimpleGame.Graphics
{
    public enum BlockType
    {
        Dirt,
        Water,
        Wall,
        Edge,
        FinishUnlocked,
        FinishLocked
    }
    public enum FruitType
    {
        Apple,
        Banana,
        Watermelone,
        Pineapple
    }
    public enum CharacterType
    {
        Player,
        Enemy
    }
    public enum MediaType
    {
        Music,
        Footsteps,
        Eat,
        Ouch,
        ShowPortal
    }
    public enum CardinalPoint
    {
        North = 0,
        NorthEast = 45,
        East = 90,
        SouthEast = 135,
        South = 180,
        SouthWest = 225,
        West = 270,
        NorthWest = 315
    }
    public enum UserInterfaceType
    {
        HeartLife
    }
    public enum ButtonType
    {
        Resume,
        QuitToMainMenu,
        SaveGame,
        LoadGame,
        BackgroundImage,
        Select,
        Back,
        Confirm
    }
    public enum ClassType
    {
        GameFruit,
        GameBlock,
        GameCharacter
    }
}
