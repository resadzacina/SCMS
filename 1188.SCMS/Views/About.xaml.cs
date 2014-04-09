using System;
using System.Windows.Browser;

namespace _1188.SCMS
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void FitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("http://www.fit.ba"), "_newWindow");
        }
    }
}