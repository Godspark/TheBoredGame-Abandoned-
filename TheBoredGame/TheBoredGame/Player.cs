using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class Player
    {
        List<Unit> currentUnits;
        private Player enemyPlayer;
        String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Player EnemyPlayer
        {
            get { return enemyPlayer; }
            set { enemyPlayer = value; }
        }

        public Player(String name)
        {
            currentUnits = new List<Unit>();
            this.name = name;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Unit unit in currentUnits)
            {
                unit.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            foreach (Unit unit in currentUnits)
            {
                unit.Draw(gameTime, spriteBatch, backgroundColor);
            }
        }

        public void AssignUnit(Unit unit)
        {
            currentUnits.Add(unit);
        }
        public void RemoveUnit(Unit unit)
        {
            currentUnits.Remove(unit);
        }
        public Unit PreviousUnit(Unit selectedUnit)
        {
            int currentIndex = currentUnits.IndexOf(selectedUnit);
            currentIndex -= 1;
            if (currentIndex < 0)
                currentIndex = currentUnits.Count - 1;
            return currentUnits[currentIndex];
        }
        public Unit FirstUnit()
        {
            return currentUnits[0];
        }
        public Unit NextUnit(Unit selectedUnit)
        {
            int currentIndex = currentUnits.IndexOf(selectedUnit);
            currentIndex += 1;
            if (currentIndex >= currentUnits.Count)
                currentIndex = 0;
            return currentUnits[currentIndex];
        }
        public Boolean HasMoreUnits()
        {
            return currentUnits.Count > 0;
        }

        public void ResetUnitTurnCounters()
        {
            foreach (Unit unit in currentUnits)
            {
                unit.ResetExhaustionCounter();
            }
        }

        public int UnitCount()
        {
            return currentUnits.Count;
        }
    }
}
