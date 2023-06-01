using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace Project002;

public static class BoarsManager
{
    public static List<Boars> Boars { get; } = new();
    private static float _spawnCooldown;
    private static float _spawnTime;
    private static Random _random;
    private static int _padding;
    private static Texture2D _tex;
    

    public static void Init()
    {
        _tex = Globals.Content.Load<Texture2D>("Enemy/PigSheets");
        _spawnCooldown = 3f;
        _spawnTime = _spawnCooldown;
        _random = new();
        _padding = 60;        
    }

        

    public static void Reset()
    {
        Boars.Clear();
        _spawnTime = _spawnCooldown;
    }

    private static Vector2 RandomPosition(Point bounds)
    {
        float w = bounds.X;
        float h = bounds.Y;
        Vector2 pos = new();

        if (_random.NextDouble() <  w / (w + h))
        {
            pos.X = (int)(_random.NextDouble() * w);
            pos.Y = (int)(_random.NextDouble() < 0.5 ? -_padding : h + _padding);
        }
        else
        {
            pos.Y = (int)(_random.NextDouble() * h);
            pos.X = (int)(_random.NextDouble() < 0.5 ? -_padding : w + _padding);
        }

        return pos;
    }

    public static void AddBoars(Point bounds)
    {
        Boars.Add(new(_tex, RandomPosition(bounds)));
    }

    public static void Update(Player player, Point bounds)
    {
        _spawnTime -= Globals.TotalSeconds;
        while(_spawnTime <= 0)
        {
            _spawnTime += _spawnCooldown;
            AddBoars(bounds);
        }

        foreach (var b in Boars)
        {
            b.Update(player);
        }
        Boars.RemoveAll((b) => b.HP <= 0);
    }

    public static void Draw()
    {
        foreach (var b in Boars)
        {
            b.Draw();
        }
    }
}
