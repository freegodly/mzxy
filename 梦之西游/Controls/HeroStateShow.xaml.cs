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
using 梦之西游.Logic.Static;
using System.Windows.Threading;
namespace 梦之西游.Controls
{
    /// <summary>
    /// HeroStateShow.xaml 的交互逻辑
    /// </summary>
    public partial class HeroStateShow : UserControl
    {
        #region 面板属性访问器
        /// <summary>
        /// 设置人物头像
        /// </summary>
        public ImageSource HeroFace
        {
            set { HeroHeadImage.Source = value; }
        }
        /// <summary>
        /// 设置宝宝头像
        /// </summary>
        public ImageSource PetFace
        {
            set { PetHeadImage.Source = value; }
        }
        /// <summary>
        /// 设置宠物生命百分比
        /// </summary>
        public double PetLifePercent
        {
            set { PetLife.Width = value*60; }
        }
        public double PetMagicPercent
        {
            set { PetMagic.Width = value * 60; }
        }
        public double PetExpPercent
        {
            set { PetExp.Width = value * 60; }
        }

        public double HeroLifePercent
        {
            set { HeroLife.Width = value * 60; }
        }
        public double HeroMagicPercent
        {
            set { HeroMagic.Width = value * 60; }
        }
        public double HeroExpPercent
        {
            set { HeroExp.Width = value * 60; }
        }
        public double HeroEnergyPercent
        {
            set { HeroEnergy.Width = value * 60; }
        }
        #endregion

        public HeroStateShow()
        {
            InitializeComponent();
            
        }
        private void PetHead_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.MW.ShowMonsterPropertyShow();
            }
        }
        private void HeroHead_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.MW.ShowHeroPropertyShow();
            }
         
        }

        private void PetHead_MouseLeave(object sender, MouseEventArgs e)
        {
            BackImage.Background = new ImageBrush(Super.GetImage("Windowskins", "maphead")); 
        }

        private void PetHead_MouseEnter(object sender, MouseEventArgs e)
        {

            BackImage.Background = new ImageBrush(Super.GetImage("Windowskins", "maphead2")); 
        }

        private void HeroHead_MouseEnter(object sender, MouseEventArgs e)
        {
            BackImage.Background = new ImageBrush(Super.GetImage("Windowskins", "maphead1"));
           
        }

        private void HeroHead_MouseLeave(object sender, MouseEventArgs e)
        {
            BackImage.Background = new ImageBrush(Super.GetImage("Windowskins", "maphead")); 
        }

       
    }
}
