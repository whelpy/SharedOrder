using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharedOrder.Model;
using SharedOrder.Presenter;
using SharedOrder.View;

namespace SharedOrder
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

            // открыть соединение с БД
            if (!MainModel.InitDb())
                return;

            var presenter = new MainPresenter();
            presenter.ShowView();

            MainModel.Connection.Close();
        }
    }
}
