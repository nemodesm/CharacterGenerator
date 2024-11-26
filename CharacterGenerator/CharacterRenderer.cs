using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CharacterGenerator;

public class CharacterRenderer : Game
{
    #region SETUP

    public static Color WindowBackgroundColor = Color.White;
    public static Color TextColor = Color.Black;
    private const int ScreenshotSize = 32;
    
    private const float MaxRotAngle = 0.5f;
    private const float RandomJiggle = 1;

    // These values are just to display the character that is being rendered
    // They do not affect the output
    private const int WindowWidth = 800;
    private const int WindowHeight = 800;
    
    #endregion
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    #region Tools

    private const int ScreenshotWidth = ScreenshotSize;
    private const int ScreenshotHeight = ScreenshotSize;
    private const int HalfRenderWidth = ScreenshotWidth / 2;
    private const int HalfRenderHeight = ScreenshotHeight / 2;
    private const float TextScale = .01f * ScreenshotWidth;
    private const float RandomHorizontalOffset = .08f * ScreenshotWidth * RandomJiggle;
    private const float RandomVerticalOffset = .08f * ScreenshotHeight * RandomJiggle;
    private const float RotMult = MaxRotAngle * 2;

    #endregion

    #region Cache
    
    private static readonly float ContentScaling = MathF.Min((float)WindowWidth / ScreenshotWidth, (float)WindowHeight / ScreenshotHeight);
    private readonly Matrix _screenMatrix = Matrix.CreateScale(ContentScaling);
    private readonly Vector2 _screenCenter = new Vector2(HalfRenderWidth, HalfRenderHeight);
    private RenderTarget2D _renderTarget;
    
    #endregion

    #region User Input

    public static uint ScreenshotCount = 2048;
    public static string filePath;

    #endregion

    public CharacterRenderer()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;
        _graphics.PreferredBackBufferWidth = WindowWidth;
        _graphics.PreferredBackBufferHeight = WindowHeight;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, ScreenshotWidth, ScreenshotHeight,false, SurfaceFormat.Color,DepthFormat.Depth24,0,RenderTargetUsage.PreserveContents);

        _font = Content.Load<SpriteFont>("font");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {

        // TODO: Add your update logic here

        base.Update(gameTime);

        if (frameCount >= ScreenshotCount && ScreenshotCount != 0)
        {
            Exit();
        }
    }
    
    int frameCount = 0;
    public static bool useLower;

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(WindowBackgroundColor);
        _spriteBatch.Begin();
        var letter = DrawRandomLetter();
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        //GraphicsDevice.Clear(WindowBackgroundColor);
        _spriteBatch.Begin(transformMatrix: _screenMatrix);
        _spriteBatch.Draw(_renderTarget, Vector2.Zero, null, Color.White);
        _spriteBatch.End();

#if !DEBUG
        using (var stream = File.Create($"{filePath}/{frameCount}.png"))
        {
            _renderTarget.SaveAsPng(stream, ScreenshotWidth, ScreenshotHeight);
        }
        
        using (var stream = File.Create($"{filePath}/{frameCount}.txt"))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(letter);
            }
        }
#endif
        ++frameCount;

        base.Draw(gameTime);
    }

    private string DrawRandomLetter()
    {
        var letter = ((char)((useLower ? 'a' : 'A') + Random.Shared.Next(0, 26))).ToString();

        var scale = new Vector2(Random.Shared.NextSingle() + .5f, Random.Shared.NextSingle() + .5f) * TextScale;
        var origin = (_font.MeasureString(letter) / 2);

        _spriteBatch.DrawString(_font,
            letter,
            position: new Vector2((Random.Shared.NextSingle() - .5f) * RandomHorizontalOffset, (Random.Shared.NextSingle() - .5f) * RandomVerticalOffset) + _screenCenter,
            color: TextColor,
            rotation: (Random.Shared.NextSingle() - .5f) * RotMult, 
            origin: origin,
            scale: scale,
            SpriteEffects.None, 0f);
        
        return letter;
    }
}