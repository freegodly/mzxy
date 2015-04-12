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

namespace 梦之西游.Controls
{
    /// <summary>
    /// GameMenuShow.xaml 的交互逻辑
    /// </summary>
    public partial class GameMenuShow : UserControl
    {
        public GameMenuShow()
        {
            InitializeComponent();
            GJ.Source = new CroppedBitmap(Super.GetImage("Windowskins", "下方攻击"), new Int32Rect(0, 0, 25, 22));
            BG.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方物品"), new Int32Rect(0, 0, 25, 22));
            GY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方给予"), new Int32Rect(0, 0, 25, 22));
            JY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方交易"), new Int32Rect(0, 0, 25, 22));
            ZD.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方组队"), new Int32Rect(0, 0, 25, 22));
            CW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方生肖"), new Int32Rect(0, 0, 25, 22));
            RW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方任务"), new Int32Rect(0, 0, 25, 22));
            BP.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方帮派"), new Int32Rect(0, 0, 25, 22));
            FS.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方快捷"), new Int32Rect(0, 0, 25, 22));
            HY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方好友"), new Int32Rect(0, 0, 25, 22));
            DZ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方动作"), new Int32Rect(0, 0, 25, 22));
            XT.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方系统"), new Int32Rect(0, 0, 25, 22));
        }

        private void BQ_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void BQ_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BQ_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void GJ_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GJ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方攻击"), new Int32Rect(50, 0, 25, 22));
        }

        private void GJ_MouseEnter(object sender, MouseEventArgs e)
        {
            GJ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方攻击"), new Int32Rect(25, 0, 25, 22));
        }

        private void GJ_MouseLeave(object sender, MouseEventArgs e)
        {
            GJ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方攻击"), new Int32Rect(0, 0, 25, 22));
        }

        private void BG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BG.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方物品"), new Int32Rect(50, 0, 25, 22));
            App.MW.ShowPackageShow();
        }

        private void BG_MouseEnter(object sender, MouseEventArgs e)
        {
            BG.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方物品"), new Int32Rect(25, 0, 25, 22));
        }

        private void BG_MouseLeave(object sender, MouseEventArgs e)
        {
            BG.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方物品"), new Int32Rect(0, 0, 25, 22));
        }

        private void GY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方给予"), new Int32Rect(50, 0, 25, 22));
        }

        private void GY_MouseEnter(object sender, MouseEventArgs e)
        {
            GY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方给予"), new Int32Rect(25, 0, 25, 22));
        }

        private void GY_MouseLeave(object sender, MouseEventArgs e)
        {
            GY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方给予"), new Int32Rect(0, 0, 25, 22));
        }

        private void JY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            JY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方交易"), new Int32Rect(50, 0, 25, 22));
        }

        private void JY_MouseEnter(object sender, MouseEventArgs e)
        {
            JY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方交易"), new Int32Rect(25, 0, 25, 22));
        }

        private void JY_MouseLeave(object sender, MouseEventArgs e)
        {
            JY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方交易"), new Int32Rect(0, 0, 25, 22));
        }

        private void ZD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ZD.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方组队"), new Int32Rect(50, 0, 25, 22));
        }

        private void ZD_MouseEnter(object sender, MouseEventArgs e)
        {
            ZD.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方组队"), new Int32Rect(25, 0, 25, 22));
        }

        private void ZD_MouseLeave(object sender, MouseEventArgs e)
        {
            ZD.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方组队"), new Int32Rect(0, 0, 25, 22));
        }

        private void CW_MouseDown(object sender, MouseButtonEventArgs e)
        {
           CW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方生肖"), new Int32Rect(50, 0, 25, 22));
        }

        private void CW_MouseEnter(object sender, MouseEventArgs e)
        {
            CW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方生肖"), new Int32Rect(25, 0, 25, 22));
        }

        private void CW_MouseLeave(object sender, MouseEventArgs e)
        {
            CW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方生肖"), new Int32Rect(0, 0, 25, 22));
        }

        private void RW_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方任务"), new Int32Rect(50, 0, 25, 22));
        }

        private void RW_MouseEnter(object sender, MouseEventArgs e)
        {
            RW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方任务"), new Int32Rect(25, 0, 25, 22));
        }

        private void RW_MouseLeave(object sender, MouseEventArgs e)
        {
            RW.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方任务"), new Int32Rect(0, 0, 25, 22));
        }

        private void BP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BP.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方帮派"), new Int32Rect(50, 0, 25, 22));
        }

        private void BP_MouseEnter(object sender, MouseEventArgs e)
        {
            BP.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方帮派"), new Int32Rect(25, 0, 25, 22));
        }

        private void BP_MouseLeave(object sender, MouseEventArgs e)
        {
            BP.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方帮派"), new Int32Rect(0, 0, 25, 22));
        }

        private void FS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FS.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方快捷"), new Int32Rect(50, 0, 25, 22));
        }

        private void FS_MouseEnter(object sender, MouseEventArgs e)
        {
            FS.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方快捷"), new Int32Rect(25, 0, 25, 22));
        }

        private void FS_MouseLeave(object sender, MouseEventArgs e)
        {
            FS.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方快捷"), new Int32Rect(0, 0, 25, 22));
        }

        private void HY_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方好友"), new Int32Rect(50, 0, 25, 22));
        }

        private void HY_MouseEnter(object sender, MouseEventArgs e)
        {
            HY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方好友"), new Int32Rect(25, 0, 25, 22));
        }

        private void HY_MouseLeave(object sender, MouseEventArgs e)
        {
            HY.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方好友"), new Int32Rect(0, 0, 25, 22));
        }

        private void DZ_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DZ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方动作"), new Int32Rect(50, 0, 25, 22));
        }

        private void DZ_MouseEnter(object sender, MouseEventArgs e)
        {
            DZ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方动作"), new Int32Rect(25, 0, 25, 22));
        }

        private void DZ_MouseLeave(object sender, MouseEventArgs e)
        {
            DZ.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方动作"), new Int32Rect(0, 0, 25, 22));
        }

        private void XT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            XT.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方系统"), new Int32Rect(50, 0, 25, 22));
            App.MW.ShowSystemSettingShow();
        }

        private void XT_MouseEnter(object sender, MouseEventArgs e)
        {
            XT.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方系统"), new Int32Rect(25, 0, 25, 22));
        }

        private void XT_MouseLeave(object sender, MouseEventArgs e)
        {
            XT.Source = new CroppedBitmap(Super.GetImage("Windowskins","下方系统"), new Int32Rect(0, 0, 25, 22));
        }
    }
}
