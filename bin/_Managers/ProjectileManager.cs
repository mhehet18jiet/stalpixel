using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using static System.Formats.Asn1.AsnWriter;

namespace Project002;

public static class ProjectileManager
{
    private static Texture2D _texture;
    private static SoundEffect _soundShot;
    public static List<Projectile> Projectiles { get; } = new();

    public static void Init(Texture2D tex)
    {
        _texture = tex;
        _soundShot = Globals.Content.Load<SoundEffect>("Sound/shot2");
    }

    public static void Reset()
    {
        Projectiles.Clear();
    }

    public static void AddProjectile(ProjectileData data , bool playerBullet)
    {
        Projectiles.Add(new(_texture, data, playerBullet)); //TODO: MAKE PLAYER BULLET
        _soundShot.Play();
    }

    public static void Update(List<Boars> boars, Player player , List<Bandits> bandits)
    {
        foreach (var p in Projectiles)
        {
            p.Update();
            foreach (var b in boars)
            {
                if (b.HP <= 0) continue;
                if ((p.Position - b.Position).Length() < 40 && p.PlayerBullet)
                {
                    b.TakeDamage(p.Damage);
                    p.Destroy();
                    break;
                }
            }
            if (player.HP <= 0) continue;
            if ( (p.Position - new Vector2( player.Position.X+17, player.Position.Y+29)).Length() < 32 && !p.PlayerBullet)
            {
                player.TakeDamage(p.Damage);
                player.CheckDeath(boars);
                p.Destroy();
                break;
            }

            foreach(var b in bandits)
            {
                if (b.HP <= 0) continue;
                if ((p.Position - new Vector2(b.Position.X + 17, b.Position.Y + 29)).Length() < 32 && p.PlayerBullet)
                {
                    b.TakeDamage(p.Damage);
                    p.Destroy();
                    break;
                }
            }
        }
        Projectiles.RemoveAll((p) => p.Lifespan <= 0);
    }

    public static void Draw()
    {
        foreach (var p in Projectiles)
        {
            //p.Draw();
            Globals.SpriteBatch.Draw(_texture, p.Position, null, Color.White, p.Rotation, Vector2.Zero, 2f, SpriteEffects.None, 1);
        }
    }
}
