using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class CombatScreen : GameScreen
    {
        BoardManager boardManager;
        SideInfoManager sideInfoManager;
        SpriteFont fontKooten;

        public CombatScreen()
        {
            sideInfoManager = new SideInfoManager(900, 0);
            boardManager = new BoardManager(sideInfoManager);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            boardManager.LoadContent(contentManager);
            boardManager.GenerateMap();

            fontKooten = contentManager.Load<SpriteFont>("Kootenay");
            sideInfoManager.SpriteFont = fontKooten;
        }

        public override GameScreenManager.ScreenTransition Update(GameTime gameTime, GameScreenManager gameScreenManager)
        {
            base.Update(gameTime, gameScreenManager); //Do NOT use the return value from here!

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                return GameScreenManager.ScreenTransition.CombatToExit;
            if (previousKeyboardState != currentKeyboardState)
            {
                if (currentKeyboardState.IsKeyDown(Keys.M))
                    boardManager.Move();

                if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickUp))
                    boardManager.MoveUp();
                else if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickDown))
                    boardManager.MoveDown();
                else if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                    boardManager.MoveLeft();
                else if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickRight))
                    boardManager.MoveRight();

                if ((currentKeyboardState.IsKeyDown(Keys.Tab) && (currentKeyboardState.IsKeyDown(Keys.LeftShift) || currentKeyboardState.IsKeyDown(Keys.RightShift))) || currentGamePadState.IsButtonDown(Buttons.DPadLeft))
                    boardManager.PreviousUnit();
                else if (currentKeyboardState.IsKeyDown(Keys.Tab) || currentGamePadState.IsButtonDown(Buttons.DPadRight))
                    boardManager.NextUnit();

                if (currentKeyboardState.IsKeyDown(Keys.Enter))
                    boardManager.EndTurn();

            }

            if (boardManager.CheckWinConditions(gameTime) != null)
                return GameScreenManager.ScreenTransition.CombatToWin;


            boardManager.Update(gameTime);
            sideInfoManager.Update(gameTime);

            return GameScreenManager.ScreenTransition.NoChange;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            boardManager.DrawMap(gameTime, spriteBatch, backgroundColor);
            boardManager.DrawUnits(gameTime, spriteBatch, backgroundColor);
            sideInfoManager.Draw(gameTime, spriteBatch, backgroundColor, Color.Black);
        }

    }
}
