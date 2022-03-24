using Raylib_cs;

class Matrix
{
    private static Random random = new Random();

    public Particle[] palette = {
        new Particle(Material.None, State.None, new Color(0, 0, 0, 255), 0, 0),
        new Particle(Material.Sand, State.Solid, new Color(255, 255, 0, 255), 0.9),
        new Particle(Material.Stone, State.Solid, new Color(55, 55, 55, 255), 1, 0),
        new Particle(Material.Water, State.Liquid, new Color(55, 55, 222, 255), 0.8)
    };

    public int BrushIndex { get; private set; } = 1;

    private Particle[,] _matrix;

    public int BrushSize { get; private set; } = 10;

    public Matrix(uint width, uint height)
    {
        _matrix = new Particle[width, height]; // make a new array

        // fill the matrix with air
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _matrix[x, y] = palette[0];
            }
        }
    }

    public void RenderMatrix()
    {
        for (int x = 0; x < _matrix.GetLength(0); x++)
        {
            for (int y = 0; y < _matrix.GetLength(1); y++)
            {
                // get width and height for each particle
                int particleSizeX = Raylib.GetScreenWidth() / _matrix.GetLength(0); 
                int particleSizeY = Raylib.GetScreenHeight() / _matrix.GetLength(1);

                // draw a particle
                Raylib.DrawRectangle(
                    particleSizeX * x,
                    particleSizeY * y,
                    particleSizeX,
                    particleSizeY,
                    _matrix[x, y].Color
                    );
            }
        }
    }

    public void Step()
    {
        // check if left mouse button is pressed
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
        {
            int mouseX = Math.Clamp(
                Raylib.GetMouseX() / (Raylib.GetScreenWidth() / _matrix.GetLength(0)),
                0,
                _matrix.GetLength(0) - 1
                );

            int mouseY = Math.Clamp(
                Raylib.GetMouseY() / (Raylib.GetScreenHeight() / _matrix.GetLength(1)),
                0,
                _matrix.GetLength(1) - 1
                );
             
            // place sand
            for (int x = -(BrushSize / 2); x < (BrushSize / 2); x++)
            {
                for (int y = -(BrushSize / 2); y < (BrushSize / 2); y++)
                {
                    if (mouseX + x < _matrix.GetLength(0) &&
                        mouseX + x >= 0 &&
                        mouseY + y < _matrix.GetLength(1) &&
                        mouseY + y >= 0 &&
                        _matrix[mouseX + x, mouseY + y].ZIndex < palette[BrushIndex].ZIndex)
                        _matrix[mouseX + x, mouseY + y] = palette[BrushIndex];
                }
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
        {
            if (BrushIndex > 0)
                BrushIndex--;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
        {
            if (BrushIndex < palette.Length - 1)
                BrushIndex++;
        }

        for (int x = 0; x < _matrix.GetLength(0); x++)
        {
            for (int y = _matrix.GetLength(1) - 1; y >= 0; y--)
            {
                Move(x, y, _matrix[x, y]);
            }
        }
    }

    private void Move(int x, int y, Particle particle)
    {
        int step = particle.TilePerFrame;

        switch (particle.Material)
        {
            default:
                break;

            case Material.Sand:
                if (y + step < _matrix.GetLength(1) - 1 &&
                    _matrix[x, y + step].Type != State.Solid)
                {                    
                    _matrix[x, y + step] = particle;
                    _matrix[x, y] = palette[0];
                }
                else
                {
                    if(random.NextDouble() > 0.5)
                    {
                        if (y + step < _matrix.GetLength(1) &&
                            x - step >= 0 &&
                            _matrix[x - step, y + step].Type != State.Solid)
                        {
                            _matrix[x - step, y + step] = particle;
                            _matrix[x, y] = palette[0];
                        }
                    }
                    else
                    {
                        if (y + step < _matrix.GetLength(1) &&
                            x + step < _matrix.GetLength(0) &&
                            _matrix[x + step, y + step].Type != State.Solid)
                        {
                            _matrix[x + step, y + step] = particle;
                            _matrix[x, y] = palette[0];
                        }
                    }
                }
                break;

            case Material.Water:
                if (y + step < _matrix.GetLength(1) - 1 &&
                    _matrix[x, y + step].Type != State.Solid)
                {
                    _matrix[x, y + step] = particle;
                    _matrix[x, y] = palette[0];
                }
                else
                {
                    if (random.NextDouble() > 0.5)
                    {
                        if (y + step < _matrix.GetLength(1) &&
                            x - step >= 0 &&
                            _matrix[x - step, y + step].Type != State.Solid)
                        {
                            _matrix[x - step, y + step] = particle;
                            _matrix[x, y] = palette[0];
                        }
                    }
                    else
                    {
                        if (y + step < _matrix.GetLength(1) &&
                            x + step < _matrix.GetLength(0) &&
                            _matrix[x + step, y + step].Type != State.Solid)
                        {
                            _matrix[x + step, y + step] = particle;
                            _matrix[x, y] = palette[0];
                        }
                    }
                }
                break;
        }
    }
}
