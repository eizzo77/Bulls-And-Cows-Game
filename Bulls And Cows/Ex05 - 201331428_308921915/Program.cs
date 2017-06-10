using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex05___201331428_308921915
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //GameBoard board = new GameBoard();
            //board.ShowDialog(); 
            GuessForm form = new GuessForm();
            form.ShowDialog();
           // ColorPickForm colorForm = new ColorPickForm();
            //colorForm.ShowDialog();
        }
    }
}
