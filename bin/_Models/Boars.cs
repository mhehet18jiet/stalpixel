using Microsoft.Xna.Framework.Graphics;

namespace Project002;

public class Boars : MovingSprite
{
    public int HP { get; private set; }
    private readonly AnimationManager _boar = new();
    private Vector2 _chasePaths;
    private float _rotation;
    public int Damage { get; private set; }

    public Boars(Texture2D? tex, Vector2 pos) : base(tex, pos)
    {
        Speed = 100;
        HP = 2;
        Damage = 5;
        _boar.AddAnimation(new Vector2(1, 0), new(tex, 4, 2, 0.15f, 2));
        _boar.AddAnimation(new Vector2(-1, 0), new(tex, 4, 2, 0.15f, 1));

    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0) ExperienceManager.AddExperience(Position);
    }

    public static Vector2 GetKey(float Rotation)
    {
        var _cos = Math.Cos(Rotation);
        if (_cos >= 0) return new Vector2(1, 0);
        else return new Vector2(-1, 0);
    }

    public void Update(Player player)
    {
        _chasePaths = player.Position - Position;
        _rotation = (float)Math.Atan2(_chasePaths.Y, _chasePaths.X);

        if (_chasePaths.Length() > 4)
        {
            var dir = Vector2.Normalize(_chasePaths);
            Position += dir * Speed * Globals.TotalSeconds;

            _boar.Update(GetKey(_rotation));
        }
    }

    public new void  Draw()
    {
        _boar.Draw(Position);
    }
}
