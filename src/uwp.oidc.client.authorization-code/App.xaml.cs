using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPClient
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += RootFrame_Navigated;

                Window.Current.Content = rootFrame;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += TitleBar_BackRequested;

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var backButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            if (((Frame)sender).CanGoBack)
            {
                backButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = backButtonVisibility;
        }

        private void TitleBar_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;

            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                frame.GoBack();
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}