namespace Project002;
public class Artefacts : Sprite
{
    public float Lifespan { get; private set; }
    private const float LIFE = 5f;

    public Artefacts(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
        Lifespan = LIFE;
    }

    public void Update()
    {

    }

    public void Collect()
    {
        Lifespan = 0;
    }
}

