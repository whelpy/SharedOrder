using SharedOrder.Model;
using SharedOrder.View;

namespace SharedOrder.Presenter
{
    public class MainPresenter : BasePresenter
    {
        private readonly MainForm _view;

        public MainPresenter()
        {
            _view = new MainForm();
        }

        public override void ShowView()
        {
            _view.ShowDialog();
        }
    }
}
