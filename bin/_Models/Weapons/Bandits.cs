namespace Project002;

public class Bandits : MovingSprite
{
    private readonly Weapon _weapon;
    public int HP { get; private set; }
    private readonly AnimationManager _bandits = new();
    private readonly AnimationManager _banditsAiming = new();
    private Vector2 _chasePaths;

    public Bandits(Texture2D? texture, Vector2 pos) : base(texture, pos)
    {
        Speed = 70;
        HP = 2;
        var aimTexture = Globals.Content.Load<Texture2D>("Enemy/BanditsAimingSheet");
        #region LoadAnimation
        _bandits.AddAnimation(new Vector2(0, -1), new(texture, 6, 8, 0.1f, 1));
        _bandits.AddAnimation(new Vector2(1, -1), new(texture, 6, 8, 0.1f, 2));
        _bandits.AddAnimation(new Vector2(1, 0), new(texture, 6, 8, 0.1f, 3));
        _bandits.AddAnimation(new Vector2(1, 1), new(texture, 6, 8, 0.1f, 4));
        _bandits.AddAnimation(new Vector2(0, 1), new(texture, 6, 8, 0.1f, 5));
        _bandits.AddAnimation(new Vector2(-1, -1), new(texture, 6, 8, 0.1f, 6));
        _bandits.AddAnimation(new Vector2(-1, 0), new(texture, 6, 8, 0.1f, 7));
        _bandits.AddAnimation(new Vector2(-1, 1), new(texture, 6, 8, 0.1f, 8));

        _banditsAiming.AddAnimation(new Vector2(0, -1), new(aimTexture, 1, 8, 0.1f, 1));
        _banditsAiming.AddAnimation(new Vector2(1, -1), new(aimTexture, 1, 8, 0.1f, 2));
        _banditsAiming.AddAnimation(new Vector2(1, 0), new(aimTexture, 1, 8, 0.1f, 3));
        _banditsAiming.AddAnimation(new Vector2(1, 1), new(aimTexture, 1, 8, 0.1f, 4));
        _banditsAiming.AddAnimation(new Vector2(0, 1), new(aimTexture, 1, 8, 0.1f, 5));
        _banditsAiming.AddAnimation(new Vector2(-1, 1), new(aimTexture, 1, 8, 0.1f, 6));
        _banditsAiming.AddAnimation(new Vector2(-1, 0), new(aimTexture, 1, 8, 0.1f, 7));
        _banditsAiming.AddAnimation(new Vector2(-1, -1), new(aimTexture, 1, 8, 0.1f, 8));
        #endregion

        _weapon = new Rifle();
    }

    public Vector2 GetKey(float Rotation)
    {
        #region AngleToTarget
        if ((Rotation >= 0) && (Rotation <= Math.PI / 6) || (Rotation >= -Math.PI / 6) && (Rotation <= 0)) return new Vector2(1, 0);
        if ((Rotation > Math.PI / 6) && (Rotation <= Math.PI / 3)) return new Vector2(1, 1);
        if ((Rotation > Math.PI / 3) && (Rotation <= 4 * Math.PI / 6)) return new Vector2(0, 1);
        if ((Rotation > 4 * Math.PI / 6) && (Rotation <= 5 * Math.PI / 6)) return new Vector2(-1, 1);
        if ((Rotation > 5 * Math.PI / 6) && (Rotation <= Math.PI) || (Rotation >= -Math.PI) && (Rotation <= -5 * Math.PI / 6)) return new Vector2(-1, 0);
        if ((Rotation > -5 * Math.PI / 6) && (Rotation <= -4 * Math.PI / 6)) return new Vector2(-1, -1);
        if ((Rotation > -4 * Math.PI / 6) && (Rotation <= -2 * Math.PI / 6)) return new Vector2(0, -1);
        if ((Rotation > -2 * Math.PI / 6) && (Rotation <= -Math.PI / 6)) return new Vector2(1, -1);
        return new Vector2(0, 1);
        #endregion
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0) ExperienceManager.AddMedKit(Position);
    }

    public void Update(Player player)
    {

        _chasePaths = player.Position - Position;
        Rotation = (float)Math.Atan2(_chasePaths.Y, _chasePaths.X);

        if (_chasePaths.Length() > 400)
        {
            var dir = Vector2.Normalize(_chasePaths);
            Position += dir * Speed * Globals.TotalSeconds;

            _bandits.Update(GetKey(Rotation));
        }
        else
        {
            _banditsAiming.Update(GetKey(Rotation));
            _weapon.Fire(this);
        }
        _weapon.Update(); 
    }
    public new void Draw()
    {
        if (_chasePaths.Length() > 400)
        {
            _bandits.Draw(Position);
        }
        else
        {
            _banditsAiming.Draw(Position);
        }
    }
}
