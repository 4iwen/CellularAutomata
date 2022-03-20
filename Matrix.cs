using Raylib_cs;

class Matrix
{
    private Particle[] particles = {
        new Particle(Material.None, State.None, new Color(0, 0, 0, 255)),
        new Particle(Material.Sand, State.Solid, new Color(255, 255, 0, 255)),
        new Particle(Material.Stone, State.Solid, new Color(55, 55, 55, 255)),
        new Particle(Material.Water, State.Liquid, new Color(55, 55, 222, 255))
    };

    private Particle[,] _matrix;

    public Matrix(uint width, uint height)
    {
        _matrix = new Particle[width, height]; // make a new array

        // fill the matrix with air
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _matrix[x, y] = particles[0];
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
            _matrix[mouseX, mouseY] = particles[3];
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
        if (x + 1 < _matrix.GetLength(0) &&
            x - 1 >= 0 &&
            y + 1 < _matrix.GetLength(1) &&
            y - 1 >= 0
            )
        {
            switch (particle.Material)
            {
                default:
                    break;

                case Material.Sand:
                    if (_matrix[x, y + 1].Type != State.Solid)
                    {
                        _matrix[x, y + 1] = particle;
                        _matrix[x, y] = particles[0];
                    }
                    else if (_matrix[x - 1, y + 1].Type != State.Solid)
                    {
                        _matrix[x - 1, y + 1] = particle;
                        _matrix[x, y] = particles[0];
                    }
                    else if (_matrix[x + 1, y + 1].Type != State.Solid)
                    {
                        _matrix[x + 1, y + 1] = particle;
                        _matrix[x, y] = particles[0];
                    }
                    break;

                case Material.Stone:
                    break;

                case Material.Water:
                    break;
            }
        }
    }
}
