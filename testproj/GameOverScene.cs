using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace MonoRider
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOverScene : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager CM_Play;
        //ContentManager CM_GO;
        Texture2D background;
        bool debug = false;
        int midPoint;
        private SpriteFont font;
        //bool moveRight = false;
        //bool moveLeft = false;
        //bool moveCenter = false;
        Random Ranum = new Random();

        enum GamePlayState
        {
            kStatePlay,
            kStateGO
        };
        GamePlayState currentState = GamePlayState.kStatePlay;

        public GameOverScene()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            CM_Play = new ContentManager(Content.ServiceProvider);
            CM_Play.RootDirectory = "Content";
            this.IsMouseVisible = true;
            //currentState = GamePlayState.kStatePlay;
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = CM_Play.Load<SpriteFont>("test");
            midPoint = (GraphicsDevice.Viewport.Width / 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            CM_Play.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Start drawing
            if (debug)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                RasterizerState state = new RasterizerState();
                state.FillMode = FillMode.WireFrame;
                spriteBatch.GraphicsDevice.RasterizerState = state;
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                // Sprite effects! https://msdn.microsoft.com/en-us/library/bb203872(v=xnagamestudio.40).aspx
                spriteBatch.DrawString(font, "Game over!", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Black);
            }

            // Stop drawing

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
