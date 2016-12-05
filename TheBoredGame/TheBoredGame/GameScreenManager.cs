using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class GameScreenManager
    {
        public enum ScreenTransition
        {
            MenuToCombat, CombatToMenu, Start, MenuToExit, CombatToExit, NoChange, CombatToWin, WinToMenu
        }

        //List<GameScreen> gameScreens;
        MenuScreen menuScreen;
        CombatScreen combatScreen;
        WinScreen winScreen;

        GameScreen activeGameScreen;
        Game1 game1;

        public GameScreenManager(Game1 game1)
        {
            this.game1 = game1;
            menuScreen = new MenuScreen();
            menuScreen.LoadContent(game1.Content);
            activeGameScreen = menuScreen;
        }

        public void Update(GameTime gameTime)
        {
            switch (activeGameScreen.Update(gameTime, this))
            {
                case ScreenTransition.MenuToCombat:
                    combatScreen = new CombatScreen();
                    //Can put in a loadingscreen here, to wait for texture loading.
                    combatScreen.LoadContent(game1.Content);
                    activeGameScreen = combatScreen;
                    break;
                case ScreenTransition.CombatToExit:
                    game1.Exit();
                    break;
                case ScreenTransition.MenuToExit:
                    game1.Exit();
                    break;
                case ScreenTransition.CombatToWin:
                    winScreen = new WinScreen();
                    //Can put in a loadingscreen here, to wait for texture loading.
                    winScreen.LoadContent(game1.Content);
                    activeGameScreen = winScreen;
                    break;
                case ScreenTransition.WinToMenu:
                    activeGameScreen = menuScreen;
                    break;

            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            activeGameScreen.Draw(gameTime, spriteBatch, backgroundColor); 
        }
    }
}
