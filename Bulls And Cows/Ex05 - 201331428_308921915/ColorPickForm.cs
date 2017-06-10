using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05___201331428_308921915
{
    public partial class ColorPickForm : Form
    {
        List<Button[]> m_ColorPickButtons = new List<Button[]>();
        private GameBoard m_GameBoard;
        private Button m_ButtonCalled;
        private bool m_IsUsed;

        public ColorPickForm(GameBoard i_Board, object i_ButtonCalled)
        {
            m_GameBoard = i_Board;
            m_ButtonCalled = (i_ButtonCalled as Button);
            this.BackColor = Color.LightGray;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(150, 80);
            m_ColorPickButtons.Add(new Button[4]);
            m_ColorPickButtons.Add(new Button[4]);
            InitializeColoredButtons();
            this.Controls.AddRange(m_ColorPickButtons[0]);
            this.Controls.AddRange(m_ColorPickButtons[1]);
            this.FormClosing += new FormClosingEventHandler(form_Closing);
           
        }

        private void InitializeColoredButtons()
        {
            int coordY = 2;

            for (int i = 0; i < m_ColorPickButtons.Count; ++i)
            {
                m_ColorPickButtons[i][0] = new Button();
                m_ColorPickButtons[i][0].ClientSize = new Size(30, 30);
                m_ColorPickButtons[i][0].Location = new Point(7, 5 + coordY);
                m_ColorPickButtons[i][0].Click += new EventHandler(color_Click);
                for (int j = 1; j < m_ColorPickButtons[0].Length; ++j)
                {
                    m_ColorPickButtons[i][j] = new Button();
                    m_ColorPickButtons[i][j].ClientSize = new Size(30, 30);
                    m_ColorPickButtons[i][j].Location = new Point(this.m_ColorPickButtons[i][j - 1].Location.X
                        + this.m_ColorPickButtons[i][j - 1].ClientSize.Width + 5, this.m_ColorPickButtons[i][0].Location.Y);
                    m_ColorPickButtons[i][j].Click += new EventHandler(color_Click);
                }

                coordY = this.m_ColorPickButtons[i][0].Location.Y + this.m_ColorPickButtons[i][0].ClientSize.Height;
            }

            m_ColorPickButtons[0][0].BackColor = Color.Purple;
            m_ColorPickButtons[0][1].BackColor = Color.Red;
            m_ColorPickButtons[0][2].BackColor = Color.Green;
            m_ColorPickButtons[0][3].BackColor = Color.Aqua;
            m_ColorPickButtons[1][0].BackColor = Color.Blue;
            m_ColorPickButtons[1][1].BackColor = Color.Yellow;
            m_ColorPickButtons[1][2].BackColor = Color.Crimson;
            m_ColorPickButtons[1][3].BackColor = Color.White;
        }


        private void color_Click(object sender, EventArgs e)
        {
            m_ButtonCalled.BackColor = (sender as Button).BackColor;
            m_IsUsed = true;
            this.Dispose();
        }

        private void form_Closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel && m_IsUsed == false)
            {
                MessageBox.Show("Gotta pick one...");
                e.Cancel = true;
            }
        }
    }
}
