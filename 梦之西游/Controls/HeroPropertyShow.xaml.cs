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
using 梦之西游.SpiritInfo;
using 梦之西游.Logic.Static;

namespace 梦之西游.Controls
{
    /// <summary>
    /// HeroPropertyShow.xaml 的交互逻辑
    /// </summary>
    public partial class HeroPropertyShow : UserControl
    {
        Spirit spirit;
        public HeroPropertyShow(Spirit spirit)
        {
            InitializeComponent();
            Update(spirit);
            this.spirit = spirit;
            InitJiaDian();
        }
        
        public void Update(Spirit spirit)
        {
            spirit.SInfo.MaxLife = (int)(spirit.SInfo.TZ * GG.RTZ);
            spirit.SInfo.MaxMagic = (int)(spirit.SInfo.ML * GG.RML);
            spirit.SInfo.Hit=(int)(spirit.SInfo.LL * GG.RMZ);
            spirit.SInfo.Hurt = (int)(spirit.SInfo.LL * GG.RLL);
            spirit.SInfo.Defense = (int)(spirit.SInfo.NL * GG.RNL);
            spirit.SInfo.Speed = (int)(spirit.SInfo.MJ * GG.RMJ);
            spirit.SInfo.Wakan = (int)(spirit.SInfo.MJ * GG.RWakan);

            TLevel.Text = spirit.SInfo.Level.ToString();
            TName.Text = spirit.SInfo.Name;
            TTitle.Text = spirit.SInfo.Title;
            TMoods.Text = spirit.SInfo.Moods.ToString();
            TFaction.Text = spirit.SInfo.Faction;
            TFactionDevote.Text = spirit.SInfo.FactionDevote.ToString();
            TSchool.Text = spirit.SInfo.School;
            TSchoolDevote.Text = spirit.SInfo.SchoolDevote.ToString();
            TLife.Text = spirit.SInfo.NowLife.ToString() + "/" + spirit.SInfo.MaxLife;
            TMagic.Text = spirit.SInfo.NowMagic.ToString() + "/" + spirit.SInfo.MaxMagic;
            TEnergy.Text = spirit.SInfo.NowEnergy.ToString() + "/" + spirit.SInfo.MaxEnergy;
            TEnergy.Text = spirit.SInfo.NowEnergy.ToString() + "/" + spirit.SInfo.MaxEnergy;
            TVigour.Text = spirit.SInfo.NowVigour.ToString() + "/" + spirit.SInfo.MaxVigour;
            TBeef.Text = spirit.SInfo.NowBeef.ToString() + "/" + spirit.SInfo.MaxBeef;

            THit.Text = spirit.SInfo.Hit.ToString();
            THurt.Text = spirit.SInfo.Hurt.ToString();
            TDetense.Text = spirit.SInfo.Defense.ToString();
            TSpeed.Text = spirit.SInfo.Speed.ToString();
            TAvoid.Text = spirit.SInfo.Avoid.ToString();
            TWakan.Text = spirit.SInfo.Wakan.ToString();

            TTZ.Text = spirit.SInfo.TZ.ToString();
            TML.Text = spirit.SInfo.ML.ToString();
            TLL.Text = spirit.SInfo.LL.ToString();
            TNL.Text = spirit.SInfo.NL.ToString();
            TMJ.Text = spirit.SInfo.MJ.ToString();
            TQL.Text = spirit.SInfo.QL.ToString();


            TLevelUpExp.Text = spirit.SInfo.LevelUpExp.ToString();
            TNowExp.Text = spirit.SInfo.NowExp.ToString();

        }

        Point p0;
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            p0 = e.GetPosition(App.MW.GameWindow);
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p1 = e.GetPosition(App.MW.GameWindow);
                App.MW.MoveHeroPropertyShow(p1.X - p0.X, p1.Y - p0.Y);
                p0 = p1;
            }
        }
        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.MW.CloseHeroPropertyShow();
        }

        #region 加点相关
        int T = 0;
        int M = 0;
        int L = 0;
        int N = 0;
        int J = 0;

        private void InitJiaDian()
        {
            if (spirit.SInfo.QL > 0) Active();
            else DisActive();
            T = 0;
            M = 0;
            L = 0;
            N = 0;
            J = 0;
        }

        private void Active()
        {
            ButtnOK.Style = (Style)this.Resources["b2"];
            foreach (Button b in Add.Children)
            {
                b.Style = (Style)this.Resources["ba2"];
            }

        }
        private void DisActive()
        {
            ButtnOK.Style = (Style)this.Resources["b1"];
            foreach (Button b in Add.Children)
            {
                b.Style = (Style)this.Resources["ba1"];
            }
            foreach (Button b in Dec.Children)
            {
                b.Style = (Style)this.Resources["bd1"];
            }
        }

        private void UpUI()
        {
            Update(spirit);
            if (T > 0) DT.Style = (Style)this.Resources["bd2"];
            else DT.Style = (Style)this.Resources["bd1"];
            if (M > 0) DM.Style = (Style)this.Resources["bd2"];
            else DM.Style = (Style)this.Resources["bd1"];
            if (L > 0) DL.Style = (Style)this.Resources["bd2"];
            else DL.Style = (Style)this.Resources["bd1"];
            if (N > 0) DN.Style = (Style)this.Resources["bd2"];
            else DN.Style = (Style)this.Resources["bd1"];
            if (J > 0) DJ.Style = (Style)this.Resources["bd2"];
            else DJ.Style = (Style)this.Resources["bd1"];

            if (spirit.SInfo.QL == 0)
            {
                foreach (Button b in Add.Children)
                {
                    b.Style = (Style)this.Resources["ba1"];
                }
            }
            else
            {
                foreach (Button b in Add.Children)
                {
                    b.Style = (Style)this.Resources["ba2"];
                }
            }
        }

        private void AT_Click(object sender, RoutedEventArgs e)
        {
            if (spirit.SInfo.QL > 0)
            {
                spirit.SInfo.QL--;
                spirit.SInfo.TZ++;
                T++;
            }
            UpUI();
        }

        private void AM_Click(object sender, RoutedEventArgs e)
        {
            if (spirit.SInfo.QL > 0)
            {
                spirit.SInfo.QL--;
                spirit.SInfo.ML++;
                M++;
            }
            UpUI();
        }

        private void AL_Click(object sender, RoutedEventArgs e)
        {
            if (spirit.SInfo.QL > 0)
            {
                spirit.SInfo.QL--;
                spirit.SInfo.LL++;
                L++;
            }
            UpUI();
        }

        private void AN_Click(object sender, RoutedEventArgs e)
        {
            if (spirit.SInfo.QL > 0)
            {
                spirit.SInfo.QL--;
                spirit.SInfo.NL++;
                N++;
            }
            UpUI();
        }
        private void AJ_Click(object sender, RoutedEventArgs e)
        {
            if (spirit.SInfo.QL > 0)
            {
                spirit.SInfo.QL--;
                spirit.SInfo.MJ++;
                J++;
            }
            UpUI();
        }
        private void DT_Click(object sender, RoutedEventArgs e)
        {
            if (T > 0)
            {
                spirit.SInfo.QL++;
                spirit.SInfo.TZ--;
                T--;
            }
            UpUI();
        }

        private void DM_Click(object sender, RoutedEventArgs e)
        {
            if (M > 0)
            {
                spirit.SInfo.QL++;
                spirit.SInfo.ML--;
                M--;
            }
            UpUI();
        }

        private void DL_Click(object sender, RoutedEventArgs e)
        {
            if (L > 0)
            {
                spirit.SInfo.QL++;
                spirit.SInfo.LL--;
                L--;
            }
            UpUI();
        }

        private void DN_Click(object sender, RoutedEventArgs e)
        {
            if (N > 0)
            {
                spirit.SInfo.QL++;
                spirit.SInfo.NL--;
                N--;
            }
            UpUI();
        }

        private void DJ_Click(object sender, RoutedEventArgs e)
        {
            if (J > 0)
            {
                spirit.SInfo.QL++;
                spirit.SInfo.MJ--;
                J--;
            }
            UpUI();
        }

        private void ButtnOK_Click(object sender, RoutedEventArgs e)
        {
            InitJiaDian();
            UpUI();
        }
        #endregion
    }
}
