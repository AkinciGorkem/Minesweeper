using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Button[,] buttons = new Button[9, 9];
        private int[,] mines = new int[9, 9];

        public Form1()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            GenerateButtons();
            PlaceMines();
            this.Size = new Size(9 * 30 + 20, 9 * 30 + 40);
            this.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void GenerateButtons()
        {
            int buttonSize = 30;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].SetBounds(i * buttonSize, j * buttonSize, buttonSize, buttonSize);
                    buttons[i, j].Click += new EventHandler(Button_Click);
                    buttons[i, j].BackColor = Color.FromArgb(30, 30, 30);
                    buttons[i, j].ForeColor = Color.White;
                    this.Controls.Add(buttons[i, j]);
                }
        }

        private void PlaceMines()
        {
            Random random = new Random();
            int minesPlaced = 0;

            while (minesPlaced < 10)
            {
                int x = random.Next(0, 9);
                int y = random.Next(0, 9);

                if (mines[x, y] != 1)
                {
                    mines[x, y] = 1;
                    minesPlaced++;
                }
            }
        }

        void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int buttonX = button.Left / 30;
            int buttonY = button.Top / 30;

            if (mines[buttonX, buttonY] == 1)
            {
                button.BackColor = Color.Red;
                DialogResult dialogResult = MessageBox.Show("Game Over! Do you want to start a new game?", "Minesweeper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    this.Controls.Clear();
                    mines = new int[9, 9];
                    StartGame();
                }
                else
                    Application.Exit();
            }
            else
            {
                int adjacentMines = CountAdjacentMines(buttonX, buttonY);
                button.Text = adjacentMines.ToString();
                button.Enabled = false;
                button.BackColor = Color.Gray;
            }
        }

        int CountAdjacentMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    int adjacentX = x + i;
                    int adjacentY = y + j;
                    if (adjacentX >= 0 && adjacentX < 9 && adjacentY >= 0 && adjacentY < 9)
                        if (mines[adjacentX, adjacentY] == 1)
                            count++;
                }
            return count;
        }
    }
}