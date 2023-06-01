namespace Project002;

public class GameManager
{
    private readonly Player _player;
    //private readonly Bandits _bandits;
    private readonly Background _bg;
    private readonly UIManager _ui;
    public Matrix _translation;
    private readonly Archianomaly _archianomaly1, _archianomaly2, _archianomaly3, _archianomaly4;
    public GameManager()
    {
        _bg = new();
        _ui = new UIManager();
        var texture = Globals.Content.Load<Texture2D>("Particle/Shot");
        ProjectileManager.Init(texture);
        _ui.Init(texture);
        ExperienceManager.Init(Globals.Content.Load<Texture2D>("exp"));
        //_electra = new Anomaly(Globals.Content.Load<Texture2D>("Particle/ElectraSheet"), new Vector2(Globals.Bounds.X /2 +200 , Globals.Bounds.Y / 2+200));
        _archianomaly1 = new(Globals.Content.Load<Texture2D>("Particle/ElectraSheet"), new Vector2(_bg.MapSize.X - 600, _bg.MapSize.Y / 2 + 200));
        _archianomaly2 = new(Globals.Content.Load<Texture2D>("Particle/ElectraSheet"), new Vector2(_bg.MapSize.X /3  - 100, _bg.MapSize.Y / 2));
        _archianomaly3 = new(Globals.Content.Load<Texture2D>("Particle/ElectraSheet"), new Vector2( 200, _bg.MapSize.Y - 200));
        _archianomaly4 = new(Globals.Content.Load<Texture2D>("Particle/ElectraSheet"), new Vector2(_bg.MapSize.X / 4 + 600, _bg.MapSize.Y / 2 -600));


        _player = new(Globals.Content.Load<Texture2D>("Player/MoveSheets"), Globals.Content.Load<Texture2D>("Player/AimingSheet"));
        
        _player.SetBounds(_bg.MapSize, _bg.TileSize);
        
        //_bandits = new(Globals.Content.Load<Texture2D>("Player/MoveSheets"), new Vector2(Globals.Bounds.X / 2, Globals.Bounds.Y / 2));
        BoarsManager.Init();
        BanditsManager.Init();
    }

    private void CalculateTranslation()
    {
        var dx = (Globals.Bounds.X / 2) - _player.Position.X;
        dx = MathHelper.Clamp(dx, -_bg.MapSize.X + Globals.Bounds.X + (_bg.TileSize.X / 2), _bg.TileSize.X / 2);
        var dy = (Globals.Bounds.Y / 2) - _player.Position.Y;
        dy = MathHelper.Clamp(dy, -_bg.MapSize.Y + Globals.Bounds.Y + (_bg.TileSize.Y / 2), _bg.TileSize.Y / 2);
        _translation = Matrix.CreateTranslation(dx, dy, 0f);
    }

    public void Restart()
    {
        ProjectileManager.Reset();
        BoarsManager.Reset();
        BanditsManager.Reset();
        ExperienceManager.Reset();
        _player.Reset();        
    }

    public void Update()
    {
        InputManager.Update();
        ExperienceManager.Update(_player);
        _player.Update(BoarsManager.Boars);
        if (_player.Dead == true) { Restart(); }
        BoarsManager.Update(_player, _bg.MapSize);
        BanditsManager.Update(_player, _bg.MapSize);
        ProjectileManager.Update(BoarsManager.Boars, _player, BanditsManager.Bandits); //TODO: Переделать на макс спавн
        //_electra.Update(_player);
        _archianomaly1.Update(_player);
        _archianomaly2.Update(_player);
        _archianomaly3.Update(_player);
        _archianomaly4.Update(_player);
        CalculateTranslation();
        _ui.Update(_player);
        if (_player.Dead) Restart();
        
    }

    public void Draw()
    {
        Globals.SpriteBatch.Begin(transformMatrix: _translation);

        _bg.Draw();
        ExperienceManager.Draw();
        ProjectileManager.Draw();
        _player.Draw();
        //_electra.Draw();
        _archianomaly1.Draw();
        _archianomaly2.Draw();
        _archianomaly3.Draw();
        _archianomaly4.Draw();
        BoarsManager.Draw();
        BanditsManager.Draw();
        _ui.Draw(_player);
        Globals.SpriteBatch.End();
        
    }
}
