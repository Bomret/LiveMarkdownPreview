using System.Windows;
using System.Windows.Controls;

namespace LiveMarkdownPreview.Utils
{
    public static class DisplayHtml
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof (string), typeof (DisplayHtml),
                new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string) obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var browser = o as WebBrowser;
            if (browser == null) return;

            var content = e.NewValue as string ?? "";
            browser.NavigateToString(content);
        }
    }
}