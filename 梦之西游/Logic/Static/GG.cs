using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 梦之西游.Logic.Static
{
    
    public static class GG
    {
        #region 共有的属性、方法
        #region 属性转换参数
        public static double RTZ = 3;
        public static double RML= 3;
        public static double RLL = 2;
        public static double RMZ = 3;
        public static double RNL = 3;
        public static double RMJ = 3;
        public static double RWakan = 0.5;

        public static int RLeveUpExp = 30;
        #endregion

        /// <summary>
        /// 游戏状态
        /// </summary>
        public enum GameStateType
        {   
            /// <summary>
            /// 默认的
            /// </summary>
            Default=0,
            /// <summary>
            /// 战斗中
            /// </summary>
            Fighting=1
        }

        /// <summary>
        /// 战斗时的行动
        /// </summary>
        public enum SpiritFightAction
        { 
            /// <summary>
            /// 攻击
            /// </summary>
            Attack=0,
            /// <summary>
            /// 施法
            /// </summary>
            Magic=1,
            /// <summary>
            /// 防御
            /// </summary>
            Defense,
            /// <summary>
            /// 使用道具
            /// </summary>
            Using,
            /// <summary>
            /// 保护
            /// </summary>
            Protect,
            /// <summary>
            /// 逮捕
            /// </summary>
            Arrest,
            /// <summary>
            /// 逃跑
            /// </summary>
            Away,
            /// <summary>
            /// 未知
            /// </summary>
            UnKnow

        }

        /// <summary>
        /// 精灵类型
        /// </summary>
        public enum SpiritTypes
        {
            /// <summary>
            /// 主角
            /// </summary>
            Leader = 0,
            /// <summary>
            /// 怪物
            /// </summary>
            Monster = 1,
            /// <summary>
            /// NPC
            /// </summary>
            NPC = 2,
        }
        /// <summary>
        /// 线程状态
        /// </summary>
        public enum TimerStates
        {
            /// <summary>
            /// 启动
            /// </summary>
            Start = 0,
            /// <summary>
            /// 停止
            /// </summary>
            Stop = 1,
        }
        /// <summary>
        /// 动作
        /// </summary>
        public enum Actions
        {
            /// <summary>
            /// 未知
            /// </summary>
            UnKnow=0,
            /// <summary>
            /// 站立
            /// </summary>
            Stand=1,
            /// <summary>
            /// 走动
            /// </summary>
            Run=2,
            /// <summary>
            /// 打招呼
            /// </summary>
            Hello=3,
            /// <summary>
            /// 行礼
            /// </summary>
            Salute=4,
            /// <summary>
            /// 发火
            /// </summary>
            Angry=5,
            /// <summary>
            /// 坐下
            /// </summary>
            SitDown=6,
            /// <summary>
            /// 哭泣
            /// </summary>
            Cry=7,
            /// <summary>
            /// 跳舞
            /// </summary>
            Dance=8,
            /// <summary>
            /// 攻击1
            /// </summary>
            Attack=9,
            /// <summary>
            /// 攻击2
            /// </summary>
            Attack2=10,
            /// <summary>
            /// 施法1
            /// </summary>
            Magic=11,
            /// <summary>
            /// 施法
            /// </summary>
            Magic2=12,
            /// <summary>
            /// 死亡
            /// </summary>
            Death=13,
            /// <summary>
            /// 死亡
            /// </summary>
            Death2=14,
            /// <summary>
            /// 战斗
            /// </summary>
            Fight=15,
            /// <summary>
            ///  战斗
            /// </summary>
            Fight2=16,
            /// <summary>
            /// 被打
            /// </summary>
            TakeABeating=17,
            /// <summary>
            /// 被打
            /// </summary>
            TakeABeating2=18,
            /// <summary>
            /// 防御
            /// </summary>
            Defense=19,
            /// <summary>
            /// 防御
            /// </summary>
            Defense2=20,
            /// <summary>
            /// 向前冲
            /// </summary>
            Onrush=21,
            /// <summary>
            /// 向前冲
            /// </summary>
            Onrush2=22,
            /// <summary>
            /// 返回
            /// </summary>
            GoBack=23,
            /// <summary>
            /// 返回
            /// </summary>
            GoBack2=24

        }
        /// <summary>
        /// 方向
        /// </summary>
        public enum Directions { ES = 0, WS = 1, WN = 2, EN = 3, S = 4, W = 5, N = 6, E = 7 };



        /// <summary>
        /// 人物图像信息
        /// </summary>
        [Serializable]
        public class ImageInfo
        {
            public ImageInfo(string sname, int sw, int sh, int startx, int starty, int endx, int endy)
            {
                SpiritImageName = sname;
                WFrameNum = sw;
                HFrameNum = sh;
                StartX = startx;
                StartY = starty;
                EndX = endx;
                EndY = endy;
            }
            public ImageInfo(string sname, int sw, int sh)
            {
                SpiritImageName = sname;
                WFrameNum = sw;
                HFrameNum = sh;
            }
            public ImageInfo(string sname, int sw, int sh, string weapon)
            {
                SpiritImageName = sname;
                WFrameNum = sw;
                HFrameNum = sh;
                Weapom = weapon;
            }
            public string Weapom;
            public string SpiritImageName;
            public int WFrameNum;
            public int HFrameNum;
            public int StartX;
            public int StartY;
            public int EndX;
            public int EndY;
        }
        #endregion
    }
}
