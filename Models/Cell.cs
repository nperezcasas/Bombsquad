using System;
using System.Collections.Generic;

namespace BOMBSQUAD.Models
{
    public class Cell
    {
        private readonly BombSquad game;
        internal readonly bool Armed;
        public readonly int XCoordinate;
        public readonly int YCoordinate;
        private bool Exploded;
        internal bool Flagged { get; private set; }
        public bool Clicked;


        public string Address
        {
            get
            {
                return "R" + YCoordinate + "C" + XCoordinate;
            }
        }

        public string Content
        {
            get
            {
                if (Exploded == true)
                {
                    return "&#128165;";
                }
                if (Flagged == true)
                {
                    return "&#128681;";
                }
                if (Armed == true)
                {
                    return "";
                }
                if (Clicked == true)
                {
                    return AdjacentBombCount.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        internal List<Cell> AdjacentCells
        {
            get
            {
                List<Cell> adjCells = new List<Cell>();
                
                for (int row = YCoordinate - 1; YCoordinate + 1 >= row; row++)
                {
                    for (int col = XCoordinate - 1; XCoordinate + 1 >= col; col++)
                    {
                        string address = "R" + row + "C" + col;
                        if (game.Cells.ContainsKey(address) && address != Address)
                        {
                            Cell adj = game.Cells[address];
                            adjCells.Add(adj);
                        }
          
                    }
                }
                return adjCells;

            }
        }

        private int AdjacentBombCount
        {
            get
            {
                int return_value = 0;
                foreach (var cell in AdjacentCells)
                { 
                    if (cell.Armed == true)
                    { 
                        return_value++;

                    }
                }
                return return_value;
            }
        }

        internal Cell(int row, int col, BombSquad bombsquad)
        {
            game = bombsquad;
            XCoordinate = row;
            YCoordinate = col;
            Random rand = new Random();
            int num = rand.Next(0, game.Cells.Count);
            int check = num % BombSquad.BOMB_DENSITY;
            if (check == 0)
            {
                Armed = true;
            }
            else
            {
                Armed = false;
            }
        }

        internal void Click()
        {
            if (Flagged == false || this == game.ClickedCell)
            {
                Clicked = true;
                Flagged = false;
                if (Armed == true && Exploded == false)
                {
                    Explode();
                }
                foreach (var cell in AdjacentCells)
                {
                    
                    if (!cell.Armed && !cell.Clicked &&
                        cell.XCoordinate < game.ClickedCell.XCoordinate + BombSquad.HINT_RADIUS &&
                        cell.XCoordinate > game.ClickedCell.XCoordinate - BombSquad.HINT_RADIUS
                        &&
                        cell.YCoordinate < game.ClickedCell.YCoordinate + BombSquad.HINT_RADIUS &&
                        cell.YCoordinate > game.ClickedCell.YCoordinate - BombSquad.HINT_RADIUS)

                    {
                        cell.Click();
                    }
                }
            }
        }

        internal void ToggleFlag()
        {
            if(Exploded == false && Flagged == false && Clicked == false && game.FlagCount < game.ArmedCells)
            {
                Flagged = true;
            }
            else
            {
                Flagged = false;
            }
        }

        private void Explode()
        {
            game.GameOver = true;
            if (Flagged == !true)
            {
                Exploded = true;
            }
            foreach (var cell in game.Cells.Values)
            {
                if (cell.Armed && !cell.Exploded && !cell.Flagged) 
                {
                    cell.Explode(); 
                }
            }
        }

    }
}
