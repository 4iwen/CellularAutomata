using Raylib_cs;

class Window
{
    public int WindowX { get => Raylib.GetScreenWidth(); }
    public int WindowY { get => Raylib.GetScreenHeight(); }
    public string WindowName { get; set; }
    public bool FpsCounter { get; set; }

    public Window(int windowX, int windowY, string windowName, int targetFps = 0, bool fpsCounter = true)
    {
        FpsCounter = fpsCounter;
        WindowName = windowName;
        Raylib.SetTargetFPS(targetFps);
        Raylib.InitWindow(windowX, windowY, windowName);
    }

    public void Init()
    {
        Matrix matrix = new Matrix(160, 120);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.SetWindowTitle(WindowName + (FpsCounter ? " | FPS: " + Raylib.GetFPS().ToString() : ""));

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            matrix.Step();
            matrix.RenderMatrix();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}