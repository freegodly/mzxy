using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using 梦之西游.Logic.Static;

namespace 梦之西游.SpiritInfo
{
    [Serializable]
    public class SpiritObject
    {
        /// <summary>
        /// 所在地图代码
        /// </summary>
        public int MapCode;
        /// <summary>
        /// 坐标
        /// </summary>
        public Point Location;


        public GG.SpiritTypes SType;

        public int Direction;

        /// <summary>
        /// 唯一的ID
        /// </summary>
        public string UID;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 头像
        /// </summary>
        public string FaceImage;
        /// <summary>
        /// 称谓
        /// </summary>
        public string Title;
        /// <summary>
        /// 人气
        /// </summary>
        public int Moods;

        /// <summary>
        /// 帮派
        /// </summary>
        public string Faction;

        /// <summary>
        /// 帮派贡献
        /// </summary>
        public int FactionDevote;
        /// <summary>
        /// 门派
        /// </summary>
        public string School;
        /// <summary>
        /// 门派贡献
        /// </summary>
        public int SchoolDevote;
        /// <summary>
        /// 最大活力
        /// </summary>
        public int MaxVigour;
        /// <summary>
        /// 当前活力值
        /// </summary>
        public int NowVigour;
        /// <summary>
        /// 最大体力
        /// </summary>
        public int MaxBeef;
        /// <summary>
        /// 当前体力
        /// </summary>
        public int NowBeef;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxLife;
        /// <summary>
        /// 当前生命值
        /// </summary>
        public int NowLife;

        /// <summary>
        /// 最大魔法值
        /// </summary>
        public int MaxMagic;
        /// <summary>
        /// 当前魔法值
        /// </summary>
        public int NowMagic;

        /// <summary>
        /// 最大愤怒
        /// </summary>
        public int MaxEnergy;
        /// <summary>
        /// 当前愤怒
        /// </summary>
        public int NowEnergy;
        /// <summary>
        /// 升级经验
        /// </summary>
        public int LevelUpExp;
        /// <summary>
        /// 获得经验
        /// </summary>
        public int NowExp;

        /// <summary>
        /// 属性
        /// </summary>
            /// <summary>
            /// 体质
            /// </summary>
            public int TZ;
            /// <summary>
            /// 魔力
            /// </summary>
            public int ML;
            /// <summary>
            /// 力量
            /// </summary>
            public int LL;
            /// <summary>
            /// 耐力
            /// </summary>
            public int NL;
            /// <summary>
            /// 敏捷
            /// </summary>
            public int MJ;
            /// <summary>
            /// 潜力
            /// </summary>
            public int QL;


        

        /// <summary>
        /// 人物的组合状态
        /// </summary>
        
            /// <summary>
            /// 命中
            /// </summary>
            public int Hit;
            /// <summary>
            /// 伤害
            /// </summary>
            public int Hurt;
            /// <summary>
            /// 防御
            /// </summary>
            public int Defense;
            /// <summary>
            /// 速度
            /// </summary>
            public int Speed;
            /// <summary>
            /// 躲避
            /// </summary>
            public int Avoid;
            /// <summary>
            /// 灵力
            /// </summary>
            public int Wakan;
        


        ///////////////////怪有的
        /// <summary>
        /// 忠诚度
        /// </summary>
        public int Loyal;
        /// <summary>
        /// 类别名字
        /// </summary>
        public string SortName;
        /// <summary>
        /// 携带等级
        /// </summary>
        public int TakeLevel;
        /// <summary>
        /// 携带宠物数量
        /// </summary>
        public int TakeNum;
        /// <summary>
        /// 参战的宠物
        /// </summary>
        public int TakeAttackID;
        /// <summary>
        /// 拥有的宠物列表
        /// </summary>
        public SpiritObject[] PetList;

        /// <summary>
        /// 记录行动图片集合
        /// </summary>
        public Dictionary<int, GG.ImageInfo> ImagePath;

    }
}
