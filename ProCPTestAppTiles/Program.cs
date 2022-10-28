using System;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var form = new Form();
            new MapCreator(form, new Point());
            form.Size = Utils.GetCorrectSize(form);
            form.AutoSize = true;
            
            Application.Run(form);
        }
    }
}