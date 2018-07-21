using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
       
        enum GameState
        {
            MainMenu,
            InGame,
            Settings,
        }

        List<int[]> resolutions = new List<int[]>();

        int playerChoice;

        int menuChoice;

        int resolutionChoice;

        GameState currentState;

        SpriteFont font;

        KeyboardState ks;
        KeyboardState prevKs;

        Texture2D background;

        Song musicTest;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentState = GameState.MainMenu;
            playerChoice = 0;
            menuChoice = 0;
            resolutionChoice = 0;
            resolutions.Add(new[] {1920, 1080});
            resolutions.Add(new[] {1280, 720});
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            musicTest = Content.Load<Song>("musicTest");
            background = Content.Load<Texture2D>("bgTest");
            MediaPlayer.Play(musicTest);
            font = Content.Load<SpriteFont>("mainFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            prevKs = ks;
            ks = Keyboard.GetState();


            if (currentState == GameState.InGame)
            {
                if (ks.IsKeyDown(Keys.Down) && prevKs.IsKeyUp(Keys.Down))
                    playerChoice++;

                if (ks.IsKeyDown(Keys.Up) && prevKs.IsKeyUp(Keys.Up))
                    playerChoice--;

                if (playerChoice < 0)
                    playerChoice = 0;

                if (playerChoice > 5)
                    playerChoice = 5;
            }

            if (currentState == GameState.MainMenu)
            {
                if (ks.IsKeyDown(Keys.Down) && prevKs.IsKeyUp(Keys.Down))
                    menuChoice++;

                if (ks.IsKeyDown(Keys.Up) && prevKs.IsKeyUp(Keys.Up))
                    menuChoice--;

                if (menuChoice < 0)
                    menuChoice = 0;

                if (menuChoice > 1)
                    menuChoice = 1;

                if (menuChoice == 0 && ks.IsKeyDown(Keys.Enter) && prevKs.IsKeyUp(Keys.Enter))
                {
                    currentState = GameState.InGame;
                    prevKs = ks;
                }

                if (menuChoice == 1 && ks.IsKeyDown(Keys.Enter) && prevKs.IsKeyUp(Keys.Enter))
                {
                    currentState = GameState.Settings;
                    prevKs = ks;
                }
            }

            if (currentState == GameState.Settings)
            {
                if (ks.IsKeyDown(Keys.Down) && prevKs.IsKeyUp(Keys.Down))
                    resolutionChoice++;

                if (ks.IsKeyDown(Keys.Up) && prevKs.IsKeyUp(Keys.Up))
                    resolutionChoice--;

                if (resolutionChoice < 0)
                    resolutionChoice = 0;

                if (resolutionChoice > 1)
                    resolutionChoice = 1;

                if (resolutionChoice == 0 && ks.IsKeyDown(Keys.Enter) && prevKs.IsKeyUp(Keys.Enter))
                {
                    graphics.PreferredBackBufferWidth = resolutions[0][0];
                    graphics.PreferredBackBufferHeight = resolutions[0][1];
                    graphics.ApplyChanges();
                    currentState = GameState.MainMenu;
                }

                if (resolutionChoice == 1 && ks.IsKeyDown(Keys.Enter) && prevKs.IsKeyUp(Keys.Enter))
                {
                    graphics.PreferredBackBufferWidth = resolutions[1][0];
                    graphics.PreferredBackBufferHeight = resolutions[1][1];
                    graphics.ApplyChanges();
                    currentState = GameState.MainMenu;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (currentState == GameState.MainMenu)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.DrawString(font, menuChoice.ToString(), new Vector2(100, 100), Color.Red);
            }

            if (currentState == GameState.Settings)
            {
                GraphicsDevice.Clear(Color.Yellow);
                spriteBatch.DrawString(font, resolutionChoice.ToString(), new Vector2(100, 100), Color.Red);
            }

            // TODO: Add your drawing code here
            if (currentState == GameState.InGame)
            {
                spriteBatch.Draw(background, new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.DrawString(font, playerChoice.ToString(), new Vector2(100, 100), Color.Red);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
