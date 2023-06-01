namespace Project002;

public class Rifle : Weapon
{
    public Rifle()
    {
        cooldown = 0.1f;
        maxAmmo = 30;
        Ammo = maxAmmo;
        reloadTime = 2f;
    }

    protected override void CreateProjectiles(Player player)
    {
        ProjectileData pd = new()
        {
            Position = new Vector2 (player.Position.X + 17, player.Position.Y+34),
            Rotation = player.Rotation,
            Lifespan = 2f,
            Speed = 900,
            Damage = 1
        };

        ProjectileManager.AddProjectile(pd , true);
    }

    protected override void CreateProjectiles(Bandits bandits)
    {
        ProjectileData _pdBandits = new()
        {
            Position = new Vector2(bandits.Position.X + 17, bandits.Position.Y + 34),
            Rotation = bandits.Rotation,
            Lifespan = 2f,
            Speed = 450,
            Damage = 1
        };

        ProjectileManager.AddProjectile(_pdBandits, false);
    }
}
