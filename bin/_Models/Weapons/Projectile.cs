namespace Project002;

public class Projectile : MovingSprite
{
    public Vector2 Direction { get; set; }
    public float Lifespan { get; private set; }
    public int Damage { get; }

    public bool PlayerBullet {get; private set;}

    public Projectile(Texture2D tex, ProjectileData data, bool playerBullet) : base(tex, data.Position)
    {
        Speed = data.Speed;
        Rotation = data.Rotation;
        Direction = new((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        Lifespan = data.Lifespan;
        Damage = data.Damage;
        PlayerBullet = playerBullet;
    }

    public void Destroy()
    {
        Lifespan = 0;
    }

    public void Update()
    {
        Position += Direction * Speed * Globals.TotalSeconds;
        Lifespan -= Globals.TotalSeconds;
    }
}
