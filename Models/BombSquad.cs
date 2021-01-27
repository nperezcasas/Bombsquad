using System;
using System.Collections.Generic;

namespace BOMBSQUAD.Models
{
    public class BombSquad
    {
        internal const int HINT_RADIUS = 3;
        internal const int BOMB_DENSITY = 3;
        internal const int HORIZONTAL_CELLS = 16;

        public const int VERTICAL_CELLS = 8;
        public const string BACK_COLOR = "skyblue";
        public const string ALT_BACK_COLOR = "steelblue";
        public const string COLOR = "black";
        public const string ALT_COLOR = "white";

        public int ArmedCells { get; private set; }
        public bool GameOver { get; internal set; }
        public Dictionary<string, Cell> Cells { get; private set; } = new Dictionary<string, Cell> { };

        internal Cell ClickedCell { get; set; }
        private int UnflaggedBombCount
        {
            get
            {
                int return_value = 0;
                foreach (Cell cell in Cells.Values)
                    if (cell.Armed && !cell.Flagged)
                        return_value++;
                return return_value;
            }
        }

        public int FlagCount
        {
            get
            {
                int return_value = 0;
                foreach (Cell cell in Cells.Values)
                    if (cell.Flagged)
                        return_value++;
                return return_value;
            }
        }
        public double Score
        {
            get
            {
                return FlagCount / ArmedCells * 100; // For some reason the score doesn't report.  
            }
        }

        private void RefreshCells()
        {
            ArmedCells = 0;
            Cells = new Dictionary<string, Cell> { };
            for (int row = 0; row <= HORIZONTAL_CELLS; ++row)
            {
                for (int col = 0; col <= VERTICAL_CELLS;  ++col)
                {
                    Cell cell = new Cell(row, col, this);  
                    Cells.Add(cell.Address, cell);
                    if (cell.Armed)
                    {
                        ArmedCells++;
                    }
                }
            }
        }

        private void SetInitialHint()
        {
            Random rand = new Random();
            int rand_row = rand.Next(1, BombSquad.VERTICAL_CELLS);
            int rand_column = rand.Next(1, BombSquad.HORIZONTAL_CELLS);
            Cell initial_cell = Cells["R" + rand_row + "C" + rand_column];
            if (!initial_cell.Armed)
                initial_cell.Clicked = true;
            else
                foreach(Cell cell in initial_cell.AdjacentCells)
                {
                    if(!cell.Armed)
                    {
                        cell.Clicked = true;
                        break;
                    }
                }

        }

        public void Reset()
        {
            GameOver = false;

            RefreshCells();

            SetInitialHint();
        }

        public void ClickCell(string address)
        {
            bool keyExists = Cells.ContainsKey(address);
            if (keyExists)
            {
                ClickedCell = Cells[address];
                ClickedCell.Click();
            }
        }

        public void FlagCell(string address)
        {
            bool keyExists = Cells.ContainsKey(address);
            if (keyExists)
            {
                ClickedCell = Cells[address];
                ClickedCell.ToggleFlag();
            }

            if(UnflaggedBombCount == 0)
            {
                GameOver = true;
            }
        }

    }
}
