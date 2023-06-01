using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace Project002;

public class Anomaly
{
    public Vector2 _anomalyPos;
    public Point Bounds { get; private set; }

    public int Damage;
    public float cooldownLeft;
    public int _timeToArtefactSpaw;
    public int _spawnColdown;
    public bool IsActive;
    private readonly SoundEffect _active;
    private readonly SoundEffect _activeDamage;



    private readonly AnimationManager _anim = new();

    public Anomaly (Texture2D texture, Vector2 pos)
    {
        _anomalyPos = pos;
        Damage = 3;
        _spawnColdown = 15;
        cooldownLeft = 15f;
        _timeToArtefactSpaw = _spawnColdown;
        IsActive = false;

        _anim.AddAnimation(true, new(texture, 4, 2, 0.15f, 1)); //сделать обычную анимацию
        _anim.AddAnimation(false, new(texture, 4, 2, 0.15f, 2));

        _active = Globals.Content.Load<SoundEffect>("Sound/electra");
        _activeDamage = Globals.Content.Load<SoundEffect>("Sound/electra_hit");
    }

    private void GetKey(Player player)
    {
        if ((_anomalyPos - player.Position).Length() < 50)
            cooldownLeft = 15f;
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Globals.TotalSeconds;
            IsActive = true;
        }                  
        else if(IsActive)
            IsActive = false;
         
    }

    public void Update (Player  player)//все существа могут активировать аномалию
    {
        if (new Vector2((_anomalyPos.X + 8 - player.Position.X), (_anomalyPos.Y + 8 - player.Position.Y)).Length() < 120)
            _active.Play(); 

        if (new Vector2((_anomalyPos.X+8 - player.Position.X) , (_anomalyPos.Y + 8 - player.Position.Y)).Length() < 30)
        cooldownLeft = 15f;

        if (cooldownLeft > 0)
        {
            cooldownLeft -= Globals.TotalSeconds;
            IsActive = true;
            if (new Vector2((_anomalyPos.X + 8 - player.Position.X), (_anomalyPos.Y + 8 - player.Position.Y)).Length() < 45)
            {
                _activeDamage.Play();
                player.TakeDamage(Damage);
            }
        }
        else if (IsActive)
            IsActive = false;

        //if(_spawnColdown <= 0)
        //{
        //    _spawnColdown = _timeToArtefactSpaw;
        //    //Здесь вызвать метод спавна артефакта
        //}
        _anim.Update(new Vector2((_anomalyPos.X + 8 - player.Position.X), (_anomalyPos.Y + 8 - player.Position.Y)).Length() < 45);
    }
    public void Draw()
    {
        _anim.Draw(_anomalyPos);
    }

}
