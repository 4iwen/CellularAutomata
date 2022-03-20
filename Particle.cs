using Raylib_cs;

enum State { None, Solid, Liquid, Gas }
enum Material { None, Sand, Stone, Water }

struct Particle
{
    public Material Material { get; private set; }
    public State Type { get; private set; }
    public Color Color { get; private set; }

    public Particle(Material material, State type, Color color)
    {
        Material = material;
        Type = type;
        Color = color;
    }
}
