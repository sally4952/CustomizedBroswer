using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expolerer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            View = new Microsoft.Web.WebView2.Wpf.WebView2();
            InitializeComponent();
            View.Source = new Uri("https://cn.bing.com/");
            View.ContentLoading += View_ContentLoading;
            View.CoreWebView2InitializationCompleted += (a, b) =>
            {
                View.CoreWebView2.CookieManager.DeleteAllCookies();
                View.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                //this.Title = "浏览器";
            };
            View.NavigationCompleted += View_NavigationCompleted;
            View.NavigationStarting += View_NavigationStarting;
        }

        private void View_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            this.Title = "加载中...";
        }

        private void View_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (View.Source.ToString() != "about:blank")
            {
                LinkTypeBox.Text = View.Source.ToString();
            }
            this.Title = $"浏览器 - {View.CoreWebView2.DocumentTitle}";

            if (View.CanGoForward)
            {
                GoForwardButton.IsEnabled = true;
            }
            else
            {
                GoForwardButton.IsEnabled = false;
            }
            if (View.CanGoBack)
            {
                GoBackButton.IsEnabled = true;
            }
            else
            {
                GoBackButton.IsEnabled = false;
            }
        }

        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            View.Source = new Uri(e.Uri);
        }

        private void View_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            View.Margin = new Thickness(0, 20, 0, 0);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            View.Height = sizeInfo.NewSize.Height - LinkTypeBox.Height;
            LinkTypeBox.Width = sizeInfo.NewSize.Width - 20 - 50 - 20;
            EnterButton.Margin = new Thickness(sizeInfo.NewSize.Width - 50, 0, 0, 0);
            GoBackButton.Margin = new Thickness(0, 0, 0, 0);
            GoForwardButton.Margin = new Thickness(GoBackButton.ActualWidth, 0, 0, 0);
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            var oldLink = View.Source;
            var link = LinkTypeBox.Text;
        TryLink:
            try
            {
                if (UrlHtmlMapping.Mapping.TryGetValue(new Uri(link), out var doc))
                {
                    View.CoreWebView2.NavigateToString(doc);
                    goto End;
                }
                if (link.StartsWith("http://"))
                {
                    link = LinkTypeBox.Text = link.Replace("http://", "https://");
                    goto TryLink;
                }
                View.Source = new Uri(link);
                goto End;
            }
            catch (UriFormatException)
            {
                if (link.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                {
                    goto InvalidLink;
                }
                if (!link.StartsWith("https://") || !link.StartsWith("http://"))
                {
                    link = LinkTypeBox.Text = $"http://{link}";
                }

                goto TryLink;
            }
        End:
            return;
        InvalidLink:
            LinkTypeBox.Text = $"https://cn.bing.com/search?q={System.Web.HttpUtility.UrlEncode(link)}";
            goto TryLink;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (View.CanGoBack)
            {
                View.GoBack();
                GoBackButton.IsEnabled = View.CanGoBack;
                return;
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (View.CanGoForward)
            {
                View.GoForward();
                GoForwardButton.IsEnabled = View.CanGoForward;
                return;
            }
        }
    }
}
