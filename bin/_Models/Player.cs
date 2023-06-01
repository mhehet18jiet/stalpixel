namespace Project002;

public class Player : MovingSprite
{
    public Weapon Weapon { get; set; }
    public int HP { get; private set; }
    private Weapon _weapon1;
    private Weapon _weapon2;
    private Vector2 _minPos, _maxPos;
    public Vector2 _deltaRange;
    public bool Dead { get; private set; }
    public int Experience { get; private set; }
    public int Heal { get; private set; }
    public int Arts { get; private set; }

    private readonly AnimationManager _anims = new();
    private readonly AnimationManager _aiming = new();
    private SoundEffect _takeDamdge;
    private SoundEffect _death;
    private SoundEffect _reload;
    private SoundEffect _healing;
    

    public Player(Texture2D texture, Texture2D aimTexture) : base(texture, GetStartPosition())
    {
        Reset();
        #region LoadAnimation
        _anims.AddAnimation(new Vector2(0, -1), new(texture, 6, 8, 0.1f, 1));
        _anims.AddAnimation(new Vector2(1, -1), new(texture, 6, 8, 0.1f, 2));
        _anims.AddAnimation(new Vector2(1, 0), new(texture, 6, 8, 0.1f, 3));
        _anims.AddAnimation(new Vector2(1, 1), new(texture, 6, 8, 0.1f, 4));
        _anims.AddAnimation(new Vector2(0, 1), new(texture, 6, 8, 0.1f, 5));
        _anims.AddAnimation(new Vector2(-1, -1), new(texture, 6, 8, 0.1f, 6));
        _anims.AddAnimation(new Vector2(-1, 0), new(texture, 6, 8, 0.1f, 7));
        _anims.AddAnimation(new Vector2(-1, 1), new(texture, 6, 8, 0.1f, 8));

        _aiming.AddAnimation(new Vector2(0, -1), new(aimTexture, 1, 8, 0.1f, 1));
        _aiming.AddAnimation(new Vector2(1, -1), new(aimTexture, 1, 8, 0.1f, 2));
        _aiming.AddAnimation(new Vector2(1, 0), new(aimTexture, 1, 8, 0.1f, 3));
        _aiming.AddAnimation(new Vector2(1, 1), new(aimTexture, 1, 8, 0.1f, 4));
        _aiming.AddAnimation(new Vector2(0, 1), new(aimTexture, 1, 8, 0.1f, 5));
        _aiming.AddAnimation(new Vector2(-1, 1), new(aimTexture, 1, 8, 0.1f, 6));
        _aiming.AddAnimation(new Vector2(-1, 0), new(aimTexture, 1, 8, 0.1f, 7));
        _aiming.AddAnimation(new Vector2(-1, -1), new(aimTexture, 1, 8, 0.1f, 8));
        #endregion
        _reload = Globals.Content.Load<SoundEffect>("Sound/akm_draw_full");
        SoundEffect.MasterVolume = 0.1f;
        _healing = Globals.Content.Load<SoundEffect>("Sound/morphin");
        _takeDamdge = Globals.Content.Load<SoundEffect>("Sound/heartbeat");
        _death = Globals.Content.Load<SoundEffect>("Sound/death_1");

    }

    public void SetBounds(Point mapSize, Point tileSize)
    {
        _minPos = new(-tileSize.X / 2, -tileSize.Y / 2);
        _maxPos = new(mapSize.X - (tileSize.X / 2) - 35, mapSize.Y - (tileSize.Y / 2) - 59);
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

    private static Vector2 GetStartPosition()
    {
        return new(Globals.Bounds.X / 2, Globals.Bounds.Y / 2);
    }

    public void GetExperience(int exp)
    {
        Experience += exp;

        if (Experience > 10)
        {
            HP += 15;
            Experience = 0;
        }
    }

    public void GetHeal(int kit)
    {
        Heal += kit;
    }

    public void GetArts(int arts)
    {
        Arts += arts;
    }

    private void GiveMeHeal(int heal)
    {
        HP+= heal;
        Heal--;
    }

    public void Reset()
    {
        _weapon1 = new Rifle();
        _weapon2 = new Shotgun();
        HP = 100;
        Dead = false;
        Weapon = _weapon1;
        Position = GetStartPosition();
        Experience = 0;
    }

    public void SwapWeapon()
    {
        Weapon = (Weapon == _weapon1) ? _weapon2 : _weapon1;
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        _takeDamdge.Play();
        if (HP < 0)
        {
            Dead = true;
            _death.Play();
        }
    }

    public void CheckDeath(List<Boars> boars)
    {
        foreach (var b in boars)
        {
            if (b.HP <= 0) continue;
            if ((Position - b.Position).Length() < 50)
            {
                TakeDamage(b.Damage);
                if (HP < 0)
                {
                    Dead = true;//-hp
                    break;
                }
            }                        
        }
    }


    public void Update(List<Boars> boars)
    {
        _deltaRange =  Position - GetStartPosition();
        var toMouse = InputManager.MousePosition - new Vector2(Position.X + 17, Position.Y + 29) + _deltaRange;
        Rotation = (float)Math.Atan2(toMouse.Y, toMouse.X);
        if (!InputManager.MouseRightDown)
        { 
            if (InputManager.Direction != Vector2.Zero)
            {
                var dir = Vector2.Normalize(InputManager.Direction);
                Position += dir * Speed * Globals.TotalSeconds;
                Position = Vector2.Clamp(Position, _minPos, _maxPos);
            }
            _anims.Update(InputManager.Direction);
        }
        else
        {
            _aiming.Update(GetKey(Rotation));
            if (InputManager.MouseLeftDown)
            {
                Weapon.Fire(this);
            }
        }

        Weapon.Update();

        if (InputManager.SpacePressed)
        {
            SwapWeapon();
        }

        if (InputManager.QPressed)
        {
            if (Heal > 0)
            { 
                GiveMeHeal(25);
                _healing.Play();
            }
        }

        if (InputManager.ReloadKey)
        {
            Weapon.Reload();
            _reload.Play();

        }

        CheckDeath(boars);
    }

    public new void Draw()
    {
        if (InputManager.MouseRightDown)
            _aiming.Draw(Position);
        else
            _anims.Draw(Position);
    }
}
