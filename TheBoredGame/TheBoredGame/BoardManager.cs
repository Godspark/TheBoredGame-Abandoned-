using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.IO;

namespace TheBoredGame
{
    class BoardManager
    {
        //Enumerators
        public enum TileType
        {
            BadGridTile = 1
        };
        public enum ObjectType
        {
            Empty_Space = 0, Axe_Fighter = 1, Bowman = 2, Captain = 3, Champion_Knight = 4, Marksman = 5, Spear_Warrior = 6, Sword_Knight = 7, Swordsman = 8
        };
        public enum Command
        {
            None, Move, Attack
        };

        //Attributes
        SideInfoManager sideInfoManager;

        Dictionary<int, Dictionary<int, BoardObject>> boardObjects;
        Dictionary<ObjectType, Texture2D> objectTextures;
        Unit selectedUnit;
        
        Dictionary<TileType, Texture2D> tileTextures;
        Dictionary<int, Dictionary<int, Tile>> tiles;
        //Tile activeTile;

        Player player1, player2, currentPlayer, nobody;

        Highlight highlight;

        Texture2D dotTexture;

        Command selectedCommand;

        int boardSizeColumns, boardSizeRows;


        Boolean autoSelectUnits = true;
        //Constructor
        public BoardManager(SideInfoManager sideInfoManager)
        {
            this.sideInfoManager = sideInfoManager;
            tiles = new Dictionary<int, Dictionary<int, Tile>>();
            boardObjects = new Dictionary<int, Dictionary<int, BoardObject>>();
            player1 = new Player("Player 1");
            player2 = new Player("Player 2");
            nobody = new Player("Nobody");
            player1.EnemyPlayer = player2;
            player2.EnemyPlayer = player1;
            currentPlayer = player1;
            selectedCommand = Command.None;
        }

        //Loading, updating and drawing methods
        public void LoadContent(ContentManager contentManager)
        {
            tileTextures = new Dictionary<TileType, Texture2D>();
            tileTextures[TileType.BadGridTile] = contentManager.Load<Texture2D>("BadGridTile");

            highlight = new Highlight();
            highlight.LoadContent(contentManager);

            objectTextures = new Dictionary<ObjectType, Texture2D>();
            objectTextures[ObjectType.Axe_Fighter] = contentManager.Load<Texture2D>("Axe Fighter");
            objectTextures[ObjectType.Bowman] = contentManager.Load<Texture2D>("Bowman");
            objectTextures[ObjectType.Captain] = contentManager.Load<Texture2D>("Captain");
            objectTextures[ObjectType.Champion_Knight] = contentManager.Load<Texture2D>("Champion Knight");
            objectTextures[ObjectType.Marksman] = contentManager.Load<Texture2D>("Marksman");
            objectTextures[ObjectType.Spear_Warrior] = contentManager.Load<Texture2D>("Spear Warrior");
            objectTextures[ObjectType.Sword_Knight] = contentManager.Load<Texture2D>("Sword Knight");
            objectTextures[ObjectType.Swordsman] = contentManager.Load<Texture2D>("Swordsman");
            
            dotTexture = contentManager.Load<Texture2D>("redDot");
        }
        public void GenerateMap()
        {
            //Make a list of tiles. When foreachly drawn they produce current map.
            try
            {
                //Reading text-file
                StreamReader streamReader = new StreamReader("Content/Maps/map1.txt");
                String wholeString = streamReader.ReadToEnd();
                String[] wholeStringSplits = wholeString.Split('.');

                //Finding dimensions
                String wholeDimensions = wholeStringSplits[0];
                String[] dimensions = wholeDimensions.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);
                boardSizeColumns = Int32.Parse(dimensions[0]);
                boardSizeRows = Int32.Parse(dimensions[1]);

                //Generating map
                for (int count = 0; count < boardSizeColumns; count++)
                {
                    tiles[count] = new Dictionary<int, Tile>();
                }

                String wholeMap = wholeStringSplits[1];
                String[] singleTiles = wholeMap.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);

                int currentColumnTiles = 0;
                int currentRowTiles = 0;

                foreach (String tileString in singleTiles)
                {
                    int tileType = Int32.Parse(tileString);
                    Tile tile;
                    tile = new Tile(tileTextures[(TileType)tileType], currentColumnTiles, currentRowTiles);
                    tiles[currentColumnTiles][currentRowTiles] = tile;

                    currentColumnTiles++;
                    if (currentColumnTiles > boardSizeColumns - 1)
                    {
                        currentColumnTiles = 0;
                        currentRowTiles++;
                    }
                }

                //Generating objects
                for (int count = 0; count < boardSizeColumns; count++)
                {
                    boardObjects[count] = new Dictionary<int, BoardObject>();
                }

                String wholeUnits = wholeStringSplits[2];
                String[] singleUnits = wholeUnits.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);

                int currentColumnObjects = 0;
                int currentRowObjects = 0;

                foreach (String unitString in singleUnits)
                {
                    String[] belongsToAndUnitNumberString = unitString.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    String belongsToString = belongsToAndUnitNumberString[0];
                    Player belongsTo = nobody;
                    if (belongsToString == "p1")
                        belongsTo = player1;
                    else if (belongsToString == "p2")
                        belongsTo = player2;

                    int objectNumber = Int32.Parse(belongsToAndUnitNumberString[1]);
                    BoardObject boardObject;
                    if (objectNumber == 0)
                        boardObject = new EmptySpace(currentColumnObjects, currentRowObjects, belongsTo);
                    else
                    {
                        boardObject = new Unit(objectTextures[(ObjectType)objectNumber], dotTexture, (ObjectType)objectNumber, 2, currentColumnObjects, currentRowObjects, belongsTo);
                        belongsTo.AssignUnit((Unit)boardObject);
                    }

                    boardObjects[currentColumnObjects][currentRowObjects] = boardObject;

                    currentColumnObjects++;
                    if (currentColumnObjects > boardSizeColumns - 1)
                    {
                        currentColumnObjects = 0;
                        currentRowObjects++;
                    }
                }
            }
            catch
            {
                //Error while loading board
            }

            selectedUnit = currentPlayer.FirstUnit();
            //activeTile = tiles[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex];
            highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
            highlight.DrawHighlight = true;
        }

        public void Update(GameTime gameTime)
        {
            player1.Update(gameTime);
            player2.Update(gameTime);

            if (selectedCommand == Command.Move)
            {
                //activeTile = tiles[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex];

                if (selectedUnit.BoardRowIndex <= 0)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Up, Highlight.HighlightTexture.NothingDirection);
                else if (boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex - 1].BelongsTo == currentPlayer.EnemyPlayer)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Up, Highlight.HighlightTexture.FightDirection);
                else
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Up, Highlight.HighlightTexture.UpMoveArrow);

                if (selectedUnit.BoardRowIndex >= boardSizeRows)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Down, Highlight.HighlightTexture.NothingDirection);
                else if (boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex + 1].BelongsTo == currentPlayer.EnemyPlayer)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Down, Highlight.HighlightTexture.FightDirection);
                else
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Down, Highlight.HighlightTexture.DownMoveArrow);

                if (selectedUnit.BoardColumnIndex <= 0)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Left, Highlight.HighlightTexture.NothingDirection);
                else if (boardObjects[selectedUnit.BoardColumnIndex - 1][selectedUnit.BoardRowIndex].BelongsTo == currentPlayer.EnemyPlayer)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Left, Highlight.HighlightTexture.FightDirection);
                else
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Left, Highlight.HighlightTexture.LeftMoveArrow);

                if (selectedUnit.BoardColumnIndex >= boardSizeColumns)
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Right, Highlight.HighlightTexture.NothingDirection);
                else if (boardObjects[selectedUnit.BoardColumnIndex + 1][selectedUnit.BoardRowIndex].BelongsTo == currentPlayer.EnemyPlayer)
                     highlight.ChangeActiveHighlightTexture(Highlight.Area.Right, Highlight.HighlightTexture.FightDirection);
                else
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Right, Highlight.HighlightTexture.RightMoveArrow);
                    
            }
            highlight.Update(gameTime);
        }
        public Player CheckWinConditions(GameTime gameTime)
        {
            if (!player1.HasMoreUnits())
                return player2;
            else if (!player2.HasMoreUnits())
                return player1;
            else
                return null;
        }
        public void DrawMap(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            //Draw each tile
            foreach (KeyValuePair<int, Dictionary<int, Tile>> tileColumns in tiles)
            {
                foreach (KeyValuePair<int, Tile> tile in tileColumns.Value)
                {
                    tile.Value.Draw(gameTime, spriteBatch, backgroundColor);
                }
            }

            //Draw an arrow towards each possible move location
            if (selectedCommand == Command.Move)
            {
                highlight.DrawMoveArrows(gameTime, spriteBatch, backgroundColor);
            }
        }
        public void DrawUnits(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            //Draw highlight
            highlight.Draw(gameTime, spriteBatch, backgroundColor);

            //Draw each unit
            player1.Draw(gameTime, spriteBatch, backgroundColor);
            player2.Draw(gameTime, spriteBatch, backgroundColor);
        }

        //Key input methods
        public void Move()
        {
            if (selectedUnit.IsExhausted())
                return;
            if (selectedCommand == Command.Move)
                selectedCommand = Command.None;
            else
                selectedCommand = Command.Move;
        }
        public void MoveUp()
        {
            switch (selectedCommand)
            {
                case Command.None:
                    //Do nothing
                    break;
                case Command.Move:
                    if (selectedUnit.BoardRowIndex <= 0) // If top of board, then return.
                    {
                        selectedUnit.BoardRowIndex = 0;
                        return;
                    }

                    BoardObject ifTestObject = boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex - 1];
                    if (ifTestObject.BelongsTo == currentPlayer.EnemyPlayer) //If collides with enemy unit, instakill, then move
                    {
                        sideInfoManager.AddInfoText(selectedUnit.Name + " killed " + ((Unit)ifTestObject).Name);
                        ifTestObject.BelongsTo.RemoveUnit((Unit)(ifTestObject));
                    }
                    else if (ifTestObject.IsCollidable) //Else if collides with object (not enemy), just return.
                        return;

                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex] = new EmptySpace(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex, nobody);
                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex - 1] = selectedUnit;
                    selectedUnit.BoardRowIndex -= 1;
                    highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                    if (!selectedUnit.IncrementExhaustionCounter())
                    {
                        selectedCommand = Command.None;
                        if (autoSelectUnits)
                        {
                            Unit testUnit = currentPlayer.NextUnit(selectedUnit);
                            int testUnitCount = currentPlayer.UnitCount();
                            int numberOfExhaustedCounter = 0;
                            for (int counter = 0; counter < testUnitCount; counter++)
                            {

                                if (testUnit.IsExhausted())
                                {
                                    testUnit = currentPlayer.NextUnit(testUnit);
                                    numberOfExhaustedCounter++;
                                }
                            }

                            if (numberOfExhaustedCounter == testUnitCount)
                            {
                                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                            }
                            else
                            {
                                selectedUnit = testUnit;
                                highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                            }
                        }
                        else
                            highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                    }
                    
                    break;
            }
            
        }
        public void MoveDown()
        {
            switch (selectedCommand)
            {
                case Command.None:
                    //Do nothing
                    break;
                case Command.Move:
                    if (selectedUnit.BoardRowIndex >= boardSizeRows - 1) // If bottom of board, then return.
                    {
                        selectedUnit.BoardRowIndex = boardSizeRows - 1;
                        return;
                    }

                    BoardObject ifTestObject = boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex + 1];
                    if (ifTestObject.BelongsTo == currentPlayer.EnemyPlayer) //If collides with enemy unit, instakill, then move
                    {
                        sideInfoManager.AddInfoText(selectedUnit.Name + " killed " + ((Unit)ifTestObject).Name);
                        ifTestObject.BelongsTo.RemoveUnit((Unit)(ifTestObject));
                    }
                    else if (ifTestObject.IsCollidable) //Else if collides with object (not enemy), just return.
                        return;

                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex] = new EmptySpace(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex, nobody);
                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex + 1] = selectedUnit;
                    selectedUnit.BoardRowIndex += 1;
                    highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);

                    if (!selectedUnit.IncrementExhaustionCounter())
                    {
                        selectedCommand = Command.None;
                        if (autoSelectUnits)
                        {
                            Unit testUnit = currentPlayer.NextUnit(selectedUnit);
                            int testUnitCount = currentPlayer.UnitCount();
                            int numberOfExhaustedCounter = 0;
                            for (int counter = 0; counter < testUnitCount; counter++)
                            {

                                if (testUnit.IsExhausted())
                                {
                                    testUnit = currentPlayer.NextUnit(testUnit);
                                    numberOfExhaustedCounter++;
                                }
                            }

                            if (numberOfExhaustedCounter == testUnitCount)
                            {
                                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                            }
                            else
                            {
                                selectedUnit = testUnit;
                                highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                            }
                        }
                        else
                            highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                    }
                    break;
            }
        }
        public void MoveLeft()
        {
            switch (selectedCommand)
            {
                case Command.None:
                    //Do nothing
                    break;
                case Command.Move:
                    if (selectedUnit.BoardColumnIndex <= 0) // If at very left of board, then return.
                    {
                        selectedUnit.BoardColumnIndex = 0;
                        return;
                    }
                    BoardObject ifTestObject = boardObjects[selectedUnit.BoardColumnIndex - 1][selectedUnit.BoardRowIndex];
                    if (ifTestObject.BelongsTo == currentPlayer.EnemyPlayer) //If collides with enemy unit, instakill, then move
                    {
                        sideInfoManager.AddInfoText(selectedUnit.Name + " killed " + ((Unit)ifTestObject).Name);
                        ifTestObject.BelongsTo.RemoveUnit((Unit)(ifTestObject));
                    }
                    else if (ifTestObject.IsCollidable) //Else if collides with object (not enemy), just return.
                        return;

                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex] = new EmptySpace(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex, nobody);
                    boardObjects[selectedUnit.BoardColumnIndex - 1][selectedUnit.BoardRowIndex] = selectedUnit;
                    selectedUnit.BoardColumnIndex -= 1;
                    highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                    if (!selectedUnit.IncrementExhaustionCounter())
                    {
                        selectedCommand = Command.None;
                        if (autoSelectUnits)
                        {
                            Unit testUnit = currentPlayer.NextUnit(selectedUnit);
                            int testUnitCount = currentPlayer.UnitCount();
                            int numberOfExhaustedCounter = 0;
                            for (int counter = 0; counter < testUnitCount; counter++)
                            {

                                if (testUnit.IsExhausted())
                                {
                                    testUnit = currentPlayer.NextUnit(testUnit);
                                    numberOfExhaustedCounter++;
                                }
                            }

                            if (numberOfExhaustedCounter == testUnitCount)
                            {
                                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                            }
                            else
                            {
                                selectedUnit = testUnit;
                                highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                            }
                        }
                        else
                            highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                    }
                    break;
            }
        }
        public void MoveRight()
        {
            switch (selectedCommand)
            {
                case Command.None:
                    //Do nothing
                    break;
                case Command.Move:
                    if (selectedUnit.BoardColumnIndex >= boardSizeColumns - 1) // If at very right of board, then return.
                    {
                        selectedUnit.BoardColumnIndex = boardSizeColumns - 1;
                        return;
                    }

                    BoardObject ifTestObject = boardObjects[selectedUnit.BoardColumnIndex + 1][selectedUnit.BoardRowIndex];
                    if (ifTestObject.BelongsTo == currentPlayer.EnemyPlayer) //If collides with enemy unit, instakill, then move
                    {
                        sideInfoManager.AddInfoText(selectedUnit.Name + " killed " + ((Unit)ifTestObject).Name);
                        ifTestObject.BelongsTo.RemoveUnit((Unit)(ifTestObject));
                    }
                    else if (ifTestObject.IsCollidable) //Else if collides with object (not enemy), just return.
                        return;

                    boardObjects[selectedUnit.BoardColumnIndex][selectedUnit.BoardRowIndex] = new EmptySpace(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex, nobody);
                    boardObjects[selectedUnit.BoardColumnIndex + 1][selectedUnit.BoardRowIndex] = selectedUnit;
                    selectedUnit.BoardColumnIndex += 1;
                    highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                    highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                    if (!selectedUnit.IncrementExhaustionCounter())
                    {
                        selectedCommand = Command.None;
                        if (autoSelectUnits)
                        {
                            Unit testUnit = currentPlayer.NextUnit(selectedUnit);
                            int testUnitCount = currentPlayer.UnitCount();
                            int numberOfExhaustedCounter = 0;
                            for (int counter = 0; counter < testUnitCount; counter++)
                            {

                                if (testUnit.IsExhausted())
                                {
                                    testUnit = currentPlayer.NextUnit(testUnit);
                                    numberOfExhaustedCounter++;
                                }
                            }

                            if (numberOfExhaustedCounter == testUnitCount)
                            {
                                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                            }
                            else
                            {
                                selectedUnit = testUnit;
                                highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
                            }
                        }
                        else
                            highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
                    }
                    break;
            }
        }
        public void NextUnit()
        {
            selectedUnit = currentPlayer.NextUnit(selectedUnit);
            highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
            if (selectedUnit.IsExhausted())
                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
            else
                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.YellowHighlight);
        }
        public void PreviousUnit()
        {
            selectedUnit = currentPlayer.PreviousUnit(selectedUnit);
            
            highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
            if (selectedUnit.IsExhausted())
                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.RedHighlight);
            else
                highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.YellowHighlight);
        }
        public void EndTurn()
        {
            currentPlayer.ResetUnitTurnCounters();

            if (currentPlayer == player1)
                currentPlayer = player2;
            else
                currentPlayer = player1;
            selectedCommand = Command.None;

            selectedUnit = currentPlayer.FirstUnit();
            highlight.ChangePosition(selectedUnit.BoardColumnIndex, selectedUnit.BoardRowIndex);
            highlight.ChangeActiveHighlightTexture(Highlight.Area.Highlight, Highlight.HighlightTexture.YellowHighlight);
            

            sideInfoManager.AddInfoText(currentPlayer.Name + " turn");
        }

    }
}
