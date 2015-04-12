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
using System.Windows.Navigation;
using System.Windows.Shapes;
using 梦之西游.Controls;
using 梦之西游.Logic.Static;
using 梦之西游.Logic.Static;

namespace 梦之西游
{
    /// <summary>
    /// PageHome.xaml 的交互逻辑
    /// </summary>
    public partial class PageHome : Page
    {
        Spirit LogSpirit;
        public PageHome()
        {
            InitializeComponent();
            Super.BackMusic.Volume = 1;
            Super.PlayBackGroundMusic("034music.mp3");
            Dictionary<int, GG.ImageInfo> imagepathtable = new Dictionary<int, GG.ImageInfo>();
            imagepathtable.Add((int)GG.Actions.Stand, new GG.ImageInfo("剑侠_W1", 8, 8, 0, 0, 1440, 832));
            LogSpirit = new Spirit("                         离歌笑", new Point(0, 0), imagepathtable,4,GG.SpiritTypes.NPC);
            BK.Children.Add(LogSpirit);
            Canvas.SetTop(LogSpirit, 120);
            Canvas.SetLeft(LogSpirit,130);
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            image1.Source =Super.GetImage("Windowskins","2"); 
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            image1.Source =Super.GetImage("Windowskins","1"); 
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {

            App.MW = new MainWindow();
            this.NavigationService.Navigate(App.MW);
          
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            image2.Source =Super.GetImage("Windowskins","4"); 
        }

        private void image2_MouseLeave(object sender, MouseEventArgs e)
        {
            image2.Source =Super.GetImage("Windowskins","3"); 
        }

        private void image3_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            image3.Source =Super.GetImage("Windowskins","6"); 
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            image3.Source =Super.GetImage("Windowskins","5"); 
        }
    }
}
