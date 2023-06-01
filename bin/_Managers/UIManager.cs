namespace Project002;

public class UIManager
{
    private Texture2D _bulletTexture;
    private Vector2 _posUI;
    private Texture2D _hpTexture;



    public void Init(Texture2D tex)
    {
        _bulletTexture = tex;
        _hpTexture = Globals.Content.Load<Texture2D>("UI/HP");
    }

    public void Draw(Player player)
    {
        Color c = player.Weapon.Reloading ? Color.Red : Color.White;
        

        for (int i = 0; i < player.Weapon.Ammo; i++)
        {
            Vector2 pos = new(_posUI.X - Globals.Bounds.X / 2, _posUI.Y + Globals.Bounds.Y / 4 + i * _bulletTexture.Height * 8);
            Globals.SpriteBatch.Draw(_bulletTexture, pos, null, c * 0.75f, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
        }

        for (int i = 0; i < player.HP / 4; i++)
        {
            Vector2 _pos = new( _posUI.X - Globals.Bounds.X/2 , _posUI.Y - Globals.Bounds.Y / 4 + i * _bulletTexture.Height * 8);
            Globals.SpriteBatch.Draw(_hpTexture, _pos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }

    public void Update(Player player)
    {
        _posUI = player.Position;
    }
}
