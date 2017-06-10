using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05___201331428_308921915
{
    public partial class GameBoard : Form
    {
        private readonly Random r_Random = new Random();
        private Button[] m_BlackButtons = new Button[4];
        private int[] m_generatedColorsPicked = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; // to indicate whether the color is already picked in a row
        private readonly Button[] r_GeneratedColors = new Button[4];
        private readonly List<GameBoardButton[]> r_GuessButtonsMatrix = new List<GameBoardButton[]>();
        private readonly Size r_GuessButtonSize = new Size(30, 30);
        private readonly Size r_ResultButtonSize = new Size(10, 10);
        private readonly Size r_ArrowButtonSize = new Size(35, 15);
        private readonly int r_NumOfGuessButtonsInARow = 4;
        private readonly int r_SpaceBetweenButtonsX = 5;
        private int m_NumberOfChances;
        private int m_ChancesLeft;
        private List<int>m_UsedArrows = new List<int>();

        private enum eColor
        {
            NULL,
            Black,
            Yellow,
            Gray
        }

        public GameBoard(int i_NumOfChances, GuessForm i_Form)
        {
            i_Form.Close();
            this.BackColor = Color.LightGray;
            m_UsedArrows.Capacity = m_NumberOfChances = m_ChancesLeft = i_NumOfChances;
            InitializeControls();
        }

        private void InitializeControls()
        {
            InitializeBlackButtons();
            generateRandomColors();
            for (int i = 0; i < m_NumberOfChances; ++i) // Depends on number of Guesses Picked
            {
                r_GuessButtonsMatrix.Add(new GameBoardButton[9]);
                //for (int j = 0; j < r_GuessButtonsMatrix[i].Length; j++)
                //{
                //    r_GuessButtonsMatrix[i][j].LineNumber = i;
                //}
            }

            InitializeGuessButtons();
            InitializeArrowButtons();
            InitializeResultsButtons();
            this.Controls.AddRange(m_BlackButtons);
            for (int i = 0; i < m_NumberOfChances; i++)
            {
                this.Controls.AddRange(r_GuessButtonsMatrix[i]);
            }
            this.ClientSize = new Size(230, 200 + r_GuessButtonsMatrix[0][1].Size.Height * (m_NumberOfChances - 4) + (5 * (m_NumberOfChances - 4)));
            this.ShowDialog();
        }

        private void InitializeBlackButtons()
        {
            const int blackButtonCoordY = 10;

            m_BlackButtons[0] = new Button();
            m_BlackButtons[0].BackColor = Color.Black;
            m_BlackButtons[0].ClientSize = r_GuessButtonSize;
            m_BlackButtons[0].Location = new Point(10, 10);
            for (int i = 1; i < m_BlackButtons.Length; ++i)
            {
                m_BlackButtons[i] = new Button();
                m_BlackButtons[i].BackColor = Color.Black;
                m_BlackButtons[i].ClientSize = r_GuessButtonSize;
                m_BlackButtons[i].Location = new Point(this.m_BlackButtons[i-1].Location.X + this.m_BlackButtons[i-1].ClientSize.Width + r_SpaceBetweenButtonsX
                    , blackButtonCoordY);
            }
        }

        private void InitializeGuessButtons()
        {
            int coordYSpaceBetweenButtons = 15;
            int previousXCoordLocation = this.m_BlackButtons[0].Location.X;
            int previousYCoordLocation = this.m_BlackButtons[0].Location.Y;

            for (int i = 0; i < r_GuessButtonsMatrix.Count; ++i)
            {
                r_GuessButtonsMatrix[i][0] = new GameBoardButton(i);
                var currentFirstRowButton = this.r_GuessButtonsMatrix[i][0];
                if (i > 0)
                {
                    currentFirstRowButton.Enabled = false;
                }
                currentFirstRowButton.BackColor = Color.LightGray;
                currentFirstRowButton.ClientSize = r_GuessButtonSize;
                currentFirstRowButton.Location = new Point(previousXCoordLocation,
                   previousYCoordLocation + r_GuessButtonSize.Height + coordYSpaceBetweenButtons);
                for (int j = 1; j < r_NumOfGuessButtonsInARow; ++j)
                {
                    r_GuessButtonsMatrix[i][j] = new GameBoardButton(i);
                    var currentButton = this.r_GuessButtonsMatrix[i][j];
                    var previousButton = this.r_GuessButtonsMatrix[i][j - 1];
                    currentButton.BackColor = Color.LightGray;
                    currentButton.ClientSize = r_GuessButtonSize;
                    if (i > 0)
                    {
                        currentButton.Enabled = false;
                    }
                    currentButton.Location = new Point(previousButton.Location.X
                        + r_GuessButtonSize.Width + r_SpaceBetweenButtonsX, currentFirstRowButton.Location.Y);
                    if (j == 1)
                    {
                        r_GuessButtonsMatrix[i][j - 1].Click += new EventHandler(guess_Click);
                        r_GuessButtonsMatrix[i][j].Click += new EventHandler(guess_Click);
                    }
                    else
                    {
                        r_GuessButtonsMatrix[i][j].Click += new EventHandler(guess_Click);
                    }
                }

                previousXCoordLocation = currentFirstRowButton.Location.X;
                previousYCoordLocation = currentFirstRowButton.Location.Y; 
                coordYSpaceBetweenButtons = 5;
            }
        }

        private void InitializeArrowButtons()
        {
            for (int i = 0; i < r_GuessButtonsMatrix.Count; ++i)
            {
                int arrowButtonYCoord = this.r_GuessButtonsMatrix[i][3].ClientSize.Height / 4;
                int previousButtonXCoord = this.r_GuessButtonsMatrix[i][3].Location.X;
                int previoysButtonYCoord = this.r_GuessButtonsMatrix[i][3].Location.Y;
                this.r_GuessButtonsMatrix[i][4] = new GameBoardButton(i);
                var CurrentButton = this.r_GuessButtonsMatrix[i][4];
                CurrentButton.Click += new EventHandler(arrow_Click);
                CurrentButton.Text = "-->>";
                CurrentButton.BackColor = Color.LightGray;
                CurrentButton.ClientSize = r_ArrowButtonSize;
                CurrentButton.Location = new Point(previousButtonXCoord + r_GuessButtonSize.Width + r_SpaceBetweenButtonsX
                    , previoysButtonYCoord + arrowButtonYCoord);
                CurrentButton.Enabled = false;
            }
        }

        private void InitializeResultsButtons()
        {
            for (int i = 0; i < r_GuessButtonsMatrix.Count; ++i)
            {
                int tinyButtoncoordY = this.r_GuessButtonsMatrix[i][3].Location.Y + 2;
                int previousButtonCoordX = this.r_GuessButtonsMatrix[i][4].Location.X;

                                        /* Upper Buttons */
                /* top left */
                this.r_GuessButtonsMatrix[i][5] = new GameBoardButton();
                this.r_GuessButtonsMatrix[i][5].BackColor = Color.LightGray;
                this.r_GuessButtonsMatrix[i][5].ClientSize = r_ResultButtonSize;
                this.r_GuessButtonsMatrix[i][5].Location = new Point(previousButtonCoordX + r_ArrowButtonSize.Width + r_SpaceBetweenButtonsX
                    , tinyButtoncoordY);
                r_GuessButtonsMatrix[i][5].Enabled = false;
                /* top right */
                previousButtonCoordX = this.r_GuessButtonsMatrix[i][5].Location.X;
                r_GuessButtonsMatrix[i][6] = new GameBoardButton();
                r_GuessButtonsMatrix[i][6].BackColor = Color.LightGray;
                r_GuessButtonsMatrix[i][6].ClientSize = r_ResultButtonSize;
                r_GuessButtonsMatrix[i][6].Location = new Point(previousButtonCoordX + r_ResultButtonSize.Width + r_SpaceBetweenButtonsX
                    , tinyButtoncoordY);
                r_GuessButtonsMatrix[i][6].Enabled = false;
                /* Lower Buttons */
                /* Bottom Left */
                r_GuessButtonsMatrix[i][7] = new GameBoardButton();
                r_GuessButtonsMatrix[i][7].BackColor = Color.LightGray;
                r_GuessButtonsMatrix[i][7].ClientSize = r_ResultButtonSize;
                r_GuessButtonsMatrix[i][7].Location = new Point(this.r_GuessButtonsMatrix[i][5].Location.X,
                    this.r_GuessButtonsMatrix[i][5].Location.Y + r_ResultButtonSize.Height + r_SpaceBetweenButtonsX);
                r_GuessButtonsMatrix[i][7].Enabled = false;
                /* Bottom Right */
                r_GuessButtonsMatrix[i][8] = new GameBoardButton();
                r_GuessButtonsMatrix[i][8].BackColor = Color.LightGray;
                r_GuessButtonsMatrix[i][8].ClientSize = r_ResultButtonSize;
                r_GuessButtonsMatrix[i][8].Location = new Point(this.r_GuessButtonsMatrix[i][6].Location.X,
                    this.r_GuessButtonsMatrix[i][7].Location.Y);
                r_GuessButtonsMatrix[i][8].Enabled = false;
            }
        }

        public void SetNumberOfChances(int i_ChancesSize)
        {
            m_NumberOfChances = i_ChancesSize;
        }

        private void arrow_Click(object sender, EventArgs e)
        {
            int correctGuesses = 0;
            List<int> buttonColoring = new List<int>();
            buttonColoring.Capacity = 4;
            int counter = 5;
            int location = (sender as GameBoardButton).LineNumber;
            for (int j = 0; j < r_NumOfGuessButtonsInARow; ++j)
            {
                int lastIndex = 0;
                Color colorToCheck = new Color();
                colorToCheck = r_GuessButtonsMatrix[location][j].BackColor;

                lastIndex = Array.FindLastIndex(r_GuessButtonsMatrix[location], x => x.BackColor == colorToCheck);
                if (lastIndex > j)
                {
                    MessageBox.Show("No Duplicates!!");
                    return;
                }
            } 

            m_ChancesLeft--;
            for (int i = 0; i < r_GeneratedColors.Length; i++)
            { 
                for (int j = 0; j < r_GeneratedColors.Length; j++)
                {
                    if (r_GuessButtonsMatrix[location][i].BackColor == r_GeneratedColors[j].BackColor && i == j)
                    {
                        buttonColoring.Add((int)eColor.Black);
                        break;
                    }
                    else if (r_GuessButtonsMatrix[location][i].BackColor == r_GeneratedColors[j].BackColor)
                    {
                        buttonColoring.Add((int)eColor.Yellow);
                        break;
                    }
                    else if (j == r_GeneratedColors.Length - 1)
                    {
                        buttonColoring.Add((int)eColor.Gray);
                    }
                }
            }

            // disable current line and enable the next one
            DisableCurrentEnableNext(location);
            buttonColoring.Sort();
            foreach (int color in buttonColoring)
            {
                switch (color)
                {
                    case (int)eColor.Black:
                        r_GuessButtonsMatrix[location][counter++].BackColor = Color.Black;
                        correctGuesses++;
                        break;
                    case (int)eColor.Yellow:
                        r_GuessButtonsMatrix[location][counter++].BackColor = Color.Yellow;
                        break;
                    case (int)eColor.Gray:
                        r_GuessButtonsMatrix[location][counter++].BackColor = Color.LightGray;
                        break;
                }
            }
            
            r_GuessButtonsMatrix[location][4].Enabled = false;
            if (correctGuesses == 4 || m_ChancesLeft - 1 < 0)
            {
                CheckWinOrLose(correctGuesses);
                DisableButtons();
            }
        }

        private void guess_Click(object sender, EventArgs e)
        {
            ColorPickForm colorPicker = new ColorPickForm(this, sender);
            colorPicker.ShowDialog();
            checkIfGoodToCompare((sender as GameBoardButton).LineNumber);

        }

        private void generateRandomColors()
        {
            int generatedColor;
            for (int i = 0; i < r_NumOfGuessButtonsInARow; i++)
            {
                r_GeneratedColors[i] = new Button();
            }

            foreach (Button button in r_GeneratedColors)
            {
                do
                {
                    generatedColor = r_Random.Next(0, 8);
                } while (m_generatedColorsPicked[generatedColor] == 1);
                m_generatedColorsPicked[generatedColor]++;
                switch (generatedColor)
                { 
                    case 0:
                        button.BackColor = Color.Purple;
                        break;
                    case 1:
                        button.BackColor = Color.Red;
                        break;
                    case 2:
                        button.BackColor = Color.Green;
                        break;
                    case 3:
                        button.BackColor = Color.Aqua;
                        break;
                    case 4:
                        button.BackColor = Color.Blue;
                        break;
                    case 5:
                        button.BackColor = Color.Yellow;
                        break;
                    case 6:
                        button.BackColor = Color.Crimson;
                        break;
                    case 7:
                        button.BackColor = Color.White;
                        break;
                }
            }
        }

        private void checkIfGoodToCompare(int i_LineNumber)
        {
            int counter = 0;
            for (int i = 0; i < r_NumOfGuessButtonsInARow; i++)
            {
                if (r_GuessButtonsMatrix[i_LineNumber][i].BackColor != Color.LightGray)
                {
                    counter++;
                    if (counter == 4)
                    {
                        // m_UsedArrows.Add(i);
                        r_GuessButtonsMatrix[i_LineNumber][4].Enabled = true;
                    }
                }
            }
        }

        private void DisableButtons()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                var btn = Controls[i] as Button;
                if (btn != null)
                {
                    btn.Enabled = false;
                }
            }
        }

        private void showResult()
        {
            for (int i = 0; i < r_GeneratedColors.Length; i++)
            {
                m_BlackButtons[i].BackColor = r_GeneratedColors[i].BackColor;
            }
        }

        private void DisableCurrentEnableNext(int i_Location)
        {
            for (int i = 0; i < r_NumOfGuessButtonsInARow; i++)
            {
                r_GuessButtonsMatrix[i_Location][i].Enabled = false;
                if (i_Location + 1 < m_NumberOfChances)
                {
                    r_GuessButtonsMatrix[i_Location + 1][i].Enabled = true;
                }
            }
        }

        private void CheckWinOrLose(int i_CorrectGuesses)
        {
            bool startAgain = false;
            string message;
            message = i_CorrectGuesses == 4 ? "You Guessed Right!" : "Game Over!";
            showResult();
            MessageBox.Show(message);
            DialogResult newGame = MessageBox.Show("Would you like to start Again?","Question", MessageBoxButtons.YesNo);
            startAgain = newGame == DialogResult.Yes ? true : false;
            this.Dispose();
            NewGame.StartNewGame(startAgain);
        }

        private class GameBoardButton : Button
        {
            int m_LineNumber;
            public int LineNumber
            {
                get { return m_LineNumber; }
                set { m_LineNumber = value; }
            }

            public GameBoardButton(int i_LineNumber)
            {
                m_LineNumber = i_LineNumber;
            }

            public GameBoardButton()
            {

            }
        }
    }
}