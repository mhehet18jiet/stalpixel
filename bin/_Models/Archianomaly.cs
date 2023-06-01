namespace Project002;

public class Archianomaly
{
    private readonly List<Anomaly> _anomaly = new();
    private Random _random;
    public Archianomaly(Texture2D tex, Vector2 pos)
    {

        _anomaly.Add(new Anomaly(tex, pos));
        _anomaly.Add(new Anomaly(tex, new Vector2(pos.X + 50, pos.Y)));
        _anomaly.Add(new Anomaly(tex, new Vector2(pos.X + 50, pos.Y + 50)));
        _anomaly.Add(new Anomaly(tex, new Vector2(pos.X + 50, pos.Y - 50)));
        _anomaly.Add(new Anomaly(tex, new Vector2(pos.X - 50, pos.Y - 50)));
        _anomaly.Add(new Anomaly(tex, new Vector2(pos.X - 100, pos.Y + 50)));
        _random = new();
    }

    public void Update(Player player)
    {
        foreach(var a in _anomaly )
        {
            a.Update(player);
            #region OMG_Цифры
            if (_random.Next(40000)<0.0000000000000000001)
            {
                ExperienceManager.AddArts(new Vector2(a._anomalyPos.X - 25, a._anomalyPos.Y - 25));
            }
            #endregion
        }
    }

    public void Draw()
    {
        foreach (var a in _anomaly)
        {
            a.Draw();
        }
    }


}
