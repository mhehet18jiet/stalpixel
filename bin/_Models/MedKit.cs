namespace Project002;

public class MedKit : Sprite
{
    public float Lifespan { get; private set; }
    private const float LIFE = 15f;

    public MedKit(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
        Lifespan = LIFE;
    }

    public void Update()
    {
        Lifespan -= Globals.TotalSeconds;
        Scale = 0.33f + (Lifespan / LIFE * 0.66f);
    }

    public void Collect()
    {
        Lifespan = 0;
    }
}
