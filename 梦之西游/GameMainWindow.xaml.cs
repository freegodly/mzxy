using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using 梦之西游.Logic.Static;

namespace 梦之西游
{
    /// <summary>
    /// GameMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameMainWindow :NavigationWindow
    {
    
       
        public GameMainWindow()
        {
            InitializeComponent();
            this.Cursor = Super.getCursor(0);
            this.NavigationService.Navigate(new Uri("PageHome.xaml",UriKind.Relative));
        }

        private void NavigationWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void FullScreen()
        {
            //获取全屏时的窗体尺寸
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.WindowStyle = WindowStyle.None;
            this.Left = 0;
            this.Top = 0;
            //获取全屏时的窗体尺寸
            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double ScreenHeight = SystemParameters.PrimaryScreenHeight;
            App.MW.GameWindowTransform.ScaleX = ScreenWidth / 800;
            App.MW.GameWindowTransform.ScaleY = ScreenHeight / 600;
            App.MW.GameWindowTransform.CenterX = 400;
            App.MW.GameWindowTransform.CenterY = 300;
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (App.MW != null && e.Content!=null&&e.Content.Equals(App.MW))
            {
              //  FullScreen(); 
            }
        }
    }
}
