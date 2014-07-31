using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (FooterViewModel))]
    public sealed class FooterViewModel : Screen
    {
        private BindableCollection<string> _cssFiles;
        private string _selectedCssFile;

        [ImportingConstructor]
        public FooterViewModel()
        {
            CssFiles = new BindableCollection<string>
            {
                "bootstrap",
                "normalizr"
            };

            SelectedCssFile = "bootstrap";
        }

        public BindableCollection<string> CssFiles
        {
            get { return _cssFiles; }
            set
            {
                if (Equals(value, _cssFiles)) return;

                _cssFiles = value;
                NotifyOfPropertyChange(() => CssFiles);
            }
        }

        public string SelectedCssFile
        {
            get { return _selectedCssFile; }
            set
            {
                if (Equals(value, _selectedCssFile)) return;

                _selectedCssFile = value;
                NotifyOfPropertyChange(() => SelectedCssFile);
            }
        }
    }
}