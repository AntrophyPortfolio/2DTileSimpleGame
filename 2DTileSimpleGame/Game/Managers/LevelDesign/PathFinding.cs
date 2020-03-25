using System;
using System.Collections.Generic;

namespace _2DTileSimpleGame.GameLogic
{
    class PathFinding<T> : IPathFinding<T>
    {
        class Node
        {
            public T Floor { get; set; }
            public T AboveFloor { get; set; }
            public int ICoordinate { get; set; }
            public int JCoordinate { get; set; }
            public int ZCoordinate { get; set; }
        }
        public T[,,] ArrayGrid3D { get; set; }
        public T InitialUnit { get; set; }
        private readonly Stack<Node> aroundNodes = new Stack<Node>();
        private readonly Stack<Node> visitedNodes = new Stack<Node>();
        private readonly List<T> wantedObjectsPath = new List<T>();
        private readonly List<T> obstacles = new List<T>();
        public void ObstaclesAddAll(params T[] values)
        {
            obstacles.AddRange(values);
        }
        public void TargetsAddAll(params T[] values)
        {
            wantedObjectsPath.AddRange(values);
        }
        public bool PathExists()
        {
            Node startingNode = GetStartingNode(InitialUnit);
            aroundNodes.Push(startingNode);
            while (aroundNodes.Count != 0)
            {
                Node node = aroundNodes.Pop();
                visitedNodes.Push(node);

                if (wantedObjectsPath.Contains(ArrayGrid3D[node.ICoordinate, node.JCoordinate, 1]))
                {
                    wantedObjectsPath.Remove(ArrayGrid3D[node.ICoordinate, node.JCoordinate, 1]);
                }
                if (!IsNodeMaxJCoordinate(node))
                {
                    if (!obstacles.Contains(ArrayGrid3D[node.ICoordinate, node.JCoordinate + 1, 0]))
                    {
                        Node rightNode = new Node
                        {
                            Floor = ArrayGrid3D[node.ICoordinate, node.JCoordinate + 1, 0],
                            JCoordinate = node.JCoordinate + 1,
                            ICoordinate = node.ICoordinate
                        };
                        if (!IsNodeAlreadyVisited(rightNode))
                        {
                            aroundNodes.Push(rightNode);
                        }
                    }
                }
                if (!IsNodeMaxICoordinate(node))
                {
                    if (!obstacles.Contains(ArrayGrid3D[node.ICoordinate + 1, node.JCoordinate, 0]))
                    {
                        Node bottomNode = new Node
                        {
                            Floor = ArrayGrid3D[node.ICoordinate + 1, node.JCoordinate, 0],
                            JCoordinate = node.JCoordinate,
                            ICoordinate = node.ICoordinate + 1
                        };
                        if (!IsNodeAlreadyVisited(bottomNode))
                        {
                            aroundNodes.Push(bottomNode);
                        }
                    }
                }
                if (node.JCoordinate > 0)
                {
                    if (!obstacles.Contains(ArrayGrid3D[node.ICoordinate, node.JCoordinate - 1, 0]))
                    {
                        Node leftNode = new Node
                        {
                            Floor = ArrayGrid3D[node.ICoordinate, node.JCoordinate - 1, 0],
                            JCoordinate = node.JCoordinate - 1,
                            ICoordinate = node.ICoordinate
                        };
                        if (!IsNodeAlreadyVisited(leftNode))
                        {
                            aroundNodes.Push(leftNode);
                        }
                    }
                }
                if (node.ICoordinate > 0)
                {
                    if (!obstacles.Contains(ArrayGrid3D[node.ICoordinate - 1, node.JCoordinate, 0]))
                    {
                        Node TopNode = new Node
                        {
                            Floor = ArrayGrid3D[node.ICoordinate - 1, node.JCoordinate, 0],
                            JCoordinate = node.JCoordinate,
                            ICoordinate = node.ICoordinate - 1
                        };
                        if (!IsNodeAlreadyVisited(TopNode))
                        {
                            aroundNodes.Push(TopNode);
                        }
                    }
                }
            }
            if (wantedObjectsPath.Count == 0)
            {
                return true;
            }
            return false;
        }
        private Node GetStartingNode(T value)
        {
            for (int i = 0; i < ArrayGrid3D.GetLength(1); i++)
            {
                for (int j = 0; j < ArrayGrid3D.GetLength(0); j++)
                {
                    if (ArrayGrid3D[i, j, 1] != null)
                    {
                        if (ArrayGrid3D[i, j, 1].Equals(value))
                        {
                            Node newNode = new Node
                            {
                                Floor = ArrayGrid3D[i, j, 0],
                                AboveFloor = ArrayGrid3D[i, j, 1],
                                ICoordinate = i,
                                JCoordinate = j,
                                ZCoordinate = 1
                            };
                            return newNode;
                        }
                    }
                }
            }
            throw new ArgumentOutOfRangeException("The provided value could not be found in the meshgrid");
        }
        private bool IsNodeMaxICoordinate(Node node)
        {
            return node.ICoordinate == ArrayGrid3D.GetLength(1) - 1;
        }
        private bool IsNodeMaxJCoordinate(Node node)
        {
            return node.JCoordinate == ArrayGrid3D.GetLength(0) - 1;
        }

        private bool IsNodeAlreadyVisited(Node node)
        {
            foreach (var item in visitedNodes)
            {
                if (item.ICoordinate == node.ICoordinate && item.JCoordinate == node.JCoordinate)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
