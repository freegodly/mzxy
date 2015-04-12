using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AStar
{
    /// <summary>
    /// 节点信息
    /// </summary>
    public class Node
    {
        public int X;
        public int Y;
        public double H;
        public double G;
        public double F;
        public Node Paraent;
    }

    /// <summary>
    /// 地图信息 0表示可以通过 1表示障碍
    /// </summary>
    public class Map
    {
        public int Width;
        public int Height;
        public byte[,] MapData;
    }

    public class AStar
    {
        private Map map;

        private Node startNode;
        private Node endNode;
        private Node lastNode;

        private List<Node> openList;
        private List<Node> closeList;
        private List<Node> moveList;

        public List<Node> MoveList
        {
            get { return moveList; }
            set { moveList = value; }
        }



        public List<Node> CloseList
        {
            get { return closeList; }
            set { closeList = value; }
        }

        public AStar(byte[,] map)
        {

            this.map = new Map() { Width = map.GetLength(0), Height = map.GetLength(0), MapData = map };
            MoveList = new List<Node>();
        }

        /// <summary>
        /// 查找路线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void Find(Node start, Node end)
        {
            try
            {
                this.startNode = start;
                if (this.map.MapData[end.X, end.Y] == 1)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {

                            if (this.map.MapData[end.X + i, end.Y + j] == 0)
                            {
                                end.X = end.X + i;
                                end.Y = end.Y + j;
                                break;
                            }

                        }
                    }
                }
                if (this.map.MapData[end.X, end.Y] == 1) return;
                this.endNode = end;



                openList = new List<Node>();
                closeList = new List<Node>();
                moveList = new List<Node>();

                SetFG(ref startNode);
                openList.Add(startNode);

                startNode.Paraent = null;

                StartFind();

                GetMoveList();
            }
            catch { }
        }


        private void GetMoveList()
        {
            if (lastNode != null)
            {
                while (lastNode.Paraent != null)
                {
                    moveList.Insert(0, lastNode);
                    lastNode = lastNode.Paraent;
                }
            }

        }
        private void SetFG(ref Node node)
        {
            node.G = Math.Sqrt(Math.Pow(this.endNode.X - node.X, 2) + Math.Pow(this.endNode.Y - node.Y, 2));
            node.F = node.G + node.H;
        }

        /// <summary>
        /// 判断该节点是否为障碍
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsCanGoto(Node node)
        {
            if (node.X >= 0 && node.X < map.Width && node.Y >= 0 && node.Y < map.Height && map.MapData[node.X, node.Y] == 0) return true;
            else return false;
        }

        private void StartFind()
        {
            while (openList.Count > 0)
            {
                Node MinNode = openList[0];
                foreach (Node node in openList)
                {
                    if (MinNode.F > node.F) MinNode = node;
                }
                if (MinNode.X == endNode.X && MinNode.Y == endNode.Y)
                {
                    lastNode = MinNode;
                    break;
                }

                #region 当前节点的子节点
                Node[] nextNode = new Node[4];
                nextNode[0] = new Node();
                nextNode[0].X = MinNode.X - 0;
                nextNode[0].Y = MinNode.Y - 1;
                nextNode[0].H = MinNode.H + 1;
                SetFG(ref nextNode[0]);

                nextNode[1] = new Node();
                nextNode[1].X = MinNode.X - 1;
                nextNode[1].Y = MinNode.Y - 0;
                nextNode[1].H = MinNode.H + 1;
                SetFG(ref nextNode[1]);

                nextNode[2] = new Node();
                nextNode[2].X = MinNode.X - 0;
                nextNode[2].Y = MinNode.Y + 1;
                nextNode[2].H = MinNode.H + 1;
                SetFG(ref nextNode[2]);

                nextNode[3] = new Node();
                nextNode[3].X = MinNode.X + 1;
                nextNode[3].Y = MinNode.Y - 0;
                nextNode[3].H = MinNode.H + 1;
                SetFG(ref nextNode[3]);
                #endregion


                #region 判断子节点的信息，将其加入到Open or Close 表中
                bool[] IsInOC = { false, false, false, false };
                for (int i = 0; i < 4; i++)
                {
                    if (IsCanGoto(nextNode[i]))
                    {
                        for (int j = 0; j < openList.Count; j++)
                        {
                            if (nextNode[i].X == openList[j].X && nextNode[i].Y == openList[j].Y)
                            {
                                if (nextNode[i].F < openList[j].F)
                                {
                                    openList[j] = nextNode[i];
                                }
                                IsInOC[i] = true;
                            }
                        }

                        for (int j = 0; j < closeList.Count; j++)
                        {
                            if (nextNode[i].X == closeList[j].X && nextNode[i].Y == closeList[j].Y)
                            {
                                if (nextNode[i].F < closeList[j].F)
                                {
                                    closeList.Remove(closeList[j]);
                                    openList.Add(nextNode[i]);
                                }
                                IsInOC[i] = true;
                            }
                        }
                    }
                }

                for (int i = 0; i < 4; i++)
                {

                    if (IsCanGoto(nextNode[i]))
                    {
                        nextNode[i].Paraent = MinNode;
                        if (IsInOC[i] == false)
                        {
                            openList.Add(nextNode[i]);
                        }
                    }
                }

                #endregion
                openList.Remove(MinNode);
                closeList.Add(MinNode);

            }
        }
    }
}



/*A Star算法是一智能找最短路径算法(下面简称A算法), 与 Dijkstra算法相比，A算法访问的节点比较少，因此可以缩短搜索时间。他的算法思想是：

这里有公式f

         最终路径长度f = 起点到该点的已知长度h + 该点到终点的估计长度g。

         O表(open):

                待处理的节点表。

         C表(close):

                已处理过的节点表。

算法流程:

1. 从起点开始，起点的f = 1 + g,  1表示此节点已走过的路径是1，g是此节点到终点的估计距离， 放入链表O中。

可以假设g值的计算使用勾股定理公式来计算此点到终点的直线距离。

2. 当O不为空时，从中取出一个最小f值的节点x。

3.如果x等于终点，找到路径，算法结束。否则走第4步.

4. 遍历x的所有相邻点，对所有相邻点使用公式f,计算出f值后，

先检查每个相邻节点y是否在链表O和C中，

如果在O中的话的话，更新y节点的f值，保留最小的f值，

如果在C中的话，并且此时f值比C中的f值小，则更新f值，将y节点从C中移到O中。否则不做操作。

如果不在以上两表中，按最小顺序排序将y插入链表O。最后将x插入C表中
*/