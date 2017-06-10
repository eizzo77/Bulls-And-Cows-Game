using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05___201331428_308921915
{
    public partial class GuessForm : Form
    {
        private Button m_NumOfChances = new Button();
        private Button m_StartButton = new Button();
        private int m_ChancesPicked = 4;
        private GameBoard m_GameBoard;
        private bool m_IsClosed = false;

        public GuessForm()
        {
            this.Text = "Bulls and Cows";
            this.Size = new Size(300, 170);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightGray;
            InitializeControls();
        }

        private void InitializeControls()
        {
            int startButtonWidth = this.ClientSize.Width - 90;
            int startButtonHeight = this.ClientSize.Height - 30;
            m_NumOfChances.Text = "Number Of Chances: " + m_ChancesPicked.ToString();
            m_NumOfChances.Size = new Size(270, 25);
            m_NumOfChances.Location = new Point(7, 12);
            m_NumOfChances.BackColor = Color.LightGray;
            m_NumOfChances.Click += new EventHandler(chances_Click);
            m_StartButton.Text = "Start";
            m_StartButton.Size = new Size(80, 20);
            m_StartButton.Location = new Point(startButtonWidth,startButtonHeight);
            m_StartButton.BackColor = Color.LightGray;
            m_StartButton.Click += new EventHandler(start_Click);
            this.Controls.AddRange(new Control[] { m_NumOfChances, m_StartButton });
            this.FormClosing += new FormClosingEventHandler(form_Closing);

        }

        private void chances_Click(object sender, EventArgs e)
        {
            if (m_ChancesPicked + 1 > 10)
            {
                m_ChancesPicked = 4;
            }
            else
            {
                m_ChancesPicked++;
            }

            m_NumOfChances.Text = "Number Of Chances: " + m_ChancesPicked.ToString(); 
        }


        private void form_Closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel && m_IsClosed == false)
            {
                m_IsClosed = true;
                this.Dispose();
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            m_GameBoard = new GameBoard(m_ChancesPicked, this);
        }
    }
}
