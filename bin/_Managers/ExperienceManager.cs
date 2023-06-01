namespace Project002;

public static class ExperienceManager
{
    private static Texture2D _textureStar;
    private static Texture2D _medKit;
    private static Texture2D _medium;
    public static List<Experience> Experience { get; } = new();
    public static List<MedKit> MedKit { get; } = new();
    public static List<Artefacts> Arts { get; } = new();

    private static SpriteFont _font;
    private static Vector2 _starPosition;
    private static Vector2 _kitPosition;
    private static Vector2 _textPosition;
    private static Vector2 _artsPosition;
    private static Vector2 _kitValue;
    private static Vector2 _ArtsValue;
    private static string _playerExp;
    private static string _playerHeal;
    private static string _playerArts;



    public static void Init(Texture2D tex)
    {
        _textureStar = tex;
        _medKit = Globals.Content.Load<Texture2D>("UI/Heal_1");
        _font = Globals.Content.Load<SpriteFont>("font");
        _medium = Globals.Content.Load<Texture2D>("UI/Geliy");
        _starPosition = new(Globals.Bounds.X - (2 * _textureStar.Width), 0);
    }

    public static void Reset()
    {
        Experience.Clear();
        MedKit.Clear();
        Arts.Clear();
    }

    public static void AddExperience(Vector2 pos)
    {
        Experience.Add(new(_textureStar, pos));
    }

    public static void AddMedKit(Vector2 pos)
    {
        MedKit.Add(new(_medKit, pos));
    }
    public static void AddArts(Vector2 pos)
    {
        Arts.Add(new(_medium, pos));
    }

    public static void Update(Player player)
    {
        foreach (var e in Experience)
        {
            e.Update();

            if ((e.Position - player.Position).Length() < 50)
            {
                e.Collect();
                player.GetExperience(1);
            }
        }

        foreach (var kit in MedKit)
        {
            kit.Update();

            if ((kit.Position - player.Position).Length() < 50)
            {
                kit.Collect();
                player.GetHeal(1);
            }
        }
        foreach (var a in Arts)
        {
            a.Update();

            if ((a.Position - player.Position).Length() < 50)
            {
                a.Collect();
                player.GetArts(1);
            }
        }

        _starPosition = new(player.Position.X - Globals.Bounds.X / 2 + 50, player.Position.Y + Globals.Bounds.Y / 2 - 220);
        _kitPosition = new(player.Position.X - Globals.Bounds.X / 2 + 50, player.Position.Y + Globals.Bounds.Y / 2 - 80);
        _artsPosition = new(player.Position.X - Globals.Bounds.X / 2 + 50, player.Position.Y + Globals.Bounds.Y / 2 - 150);

        Experience.RemoveAll((e) => e.Lifespan <= 0);
        MedKit.RemoveAll((e) => e.Lifespan <= 0);
        Arts.RemoveAll((e) => e.Lifespan <=0);

        _playerExp = player.Experience.ToString();
        _playerHeal = player.Heal.ToString();
        _playerArts = player.Arts.ToString();

        var x = _font.MeasureString(_playerExp).X / 2;
        _textPosition = new(_starPosition.X + 2*_textureStar.Width, _starPosition.Y + 5);
        _kitValue = new(_kitPosition.X + 4*_medKit.Width, _kitPosition.Y+5);
        _ArtsValue = new(_artsPosition.X + 4 * _medium.Width, _artsPosition.Y + 5);
    }

    public static void Draw()
    {
        Globals.SpriteBatch.Draw(_textureStar, _starPosition, null, Color.White * 0.75f, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);
        Globals.SpriteBatch.Draw(_medKit, _kitPosition, null, Color.White * 0.75f, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        Globals.SpriteBatch.Draw(_medium, _artsPosition, null, Color.White * 0.75f, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        Globals.SpriteBatch.DrawString(_font, _playerExp, _textPosition, Color.White);
        Globals.SpriteBatch.DrawString(_font, _playerHeal, _kitValue, Color.White);
        Globals.SpriteBatch.DrawString(_font, _playerArts, _ArtsValue, Color.White);

        foreach (var e in Experience)
        {
            e.Draw();
        }

        foreach(var kit in MedKit)
        {
            kit.Draw();
        }
        foreach (var a in Arts)
        {
            a.Draw();
        }
    }
}
