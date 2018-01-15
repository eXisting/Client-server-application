using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClientServerView view = new ClientServerView();
            SynchronizerPresenter presenter = new SynchronizerPresenter(view);

            Application.Run(view);
        }
    }
}
