using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ex05___201331428_308921915
{
    public static class NewGame
    {
        static GuessForm m_NewGuessForm;

        public static void StartNewGame(bool i_Decision)
        {
            if (i_Decision == true)
            {
                m_NewGuessForm = new GuessForm();
                m_NewGuessForm.ShowDialog();
            }
        }
    }
}
