using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
        public class StatusGraph
    {
        public class TransportAction
        {
            public int Priest { get; private set; }
            public int Devil { get; private set; }

            public TransportAction(int priest, int devil)
            {
                Priest = priest;
                Devil = devil;
            }
        }

        public class StatusNode
        {
            // The index for generating hashcode. Indicates the max count of character.
            private const int MaxCount = 10;

            private readonly int[] _left; // [0] for priest, [1] for devil
            private readonly int[] _right;
            private readonly bool _boatAtLeft;
            public readonly List<StatusNode> Edges = new List<StatusNode>();

            // Fields to be used when looking for shortest path.
            public StatusNode Next { private get; set; }
            public int Distance { get; set; }
            public bool IsVisited { get; set; }

            public StatusNode(int[] left, int[] right, bool boatAtLeft)
            {
                _left = left;
                _right = right;
                _boatAtLeft = boatAtLeft;
                Distance = int.MaxValue;
            }

            public static int CalculateStatusHashCode(int[] elements)
            {
                var hash = 0;
                for (var i = 0; i < 5; i++)
                {
                    hash += elements[i] * (int) Math.Pow(MaxCount, i);
                }

                return hash;
            }

            public StatusNode Transport(TransportAction action)
            {
                var newLeft = (int[]) _left.Clone();
                var newRight = (int[]) _right.Clone();

                newLeft[0] += (_boatAtLeft ? -1 : 1) * action.Priest;
                newLeft[1] += (_boatAtLeft ? -1 : 1) * action.Devil;
                newRight[0] += (_boatAtLeft ? 1 : -1) * action.Priest;
                newRight[1] += (_boatAtLeft ? 1 : -1) * action.Devil;

                var newNode = new StatusNode(newLeft, newRight, !_boatAtLeft);
                return newLeft.Union(newRight).Any(e => e < 0) ? null : newNode;
            }

            public bool IsLegal()
            {
                return (_left[0] <= 0 || _left[1] <= _left[0]) && (_right[0] <= 0 || _right[1] <= _right[0]);
            }

            // Override the Object.GetHashCode() method because we want two nodes with same status to be 
            // identical in hash-based collection as we are gonna store the nodes in collection in graph.
            public override int GetHashCode()
            {
                int[] elements = {_left[0], _left[1], _right[0], _right[1], _boatAtLeft ? 1 : 0};
                return CalculateStatusHashCode(elements);
            }

            public TransportAction GetNextTransport()
            {
                var priests = Math.Abs(Next._right[0] - _right[0]);
                var devils = Math.Abs(Next._right[1] - _right[1]);
                return new TransportAction(priests, devils);
            }

            private string GetStatusPattern()
            {
                var isLegal = IsLegal();
                var s = new StringBuilder();
                s.Append(isLegal ? "(" : "[");
                s.Append(_left[0]).Append(_left[1]);
                s.Append(_boatAtLeft ? "L" : "R");
                s.Append(_right[0]).Append(_right[1]);

                return s.ToString();
            }

            public override string ToString()
            {
                var s = new StringBuilder();
                s.Append(GetStatusPattern());
                s.Append(" ->");
                foreach (var edge in Edges)
                {
                    s.Append(" ").Append(edge.GetStatusPattern()).Append(" ");
                }

                return s.ToString();
            }
        }

        // Use hashtable for an efficient query.
        private readonly Hashtable _nodes = new Hashtable();
        private readonly int _priests;
        private readonly int _devils;

        public StatusGraph(int priests, int devils)
        {
            _priests = priests;
            _devils = devils;
            
            InitGraph();
            FindPath();
        }

        private void InitGraph()
        {
            var nodeQueue = new Queue<StatusNode>();

            // Initialize the status graph.
            var srcNode = new StatusNode(new[] {0, 0}, new[] {_priests, _devils}, false);
            _nodes.Add(srcNode.GetHashCode(), srcNode);

            nodeQueue.Enqueue(srcNode);

            var actions = new[]
            {
                new TransportAction(1, 0),
                new TransportAction(0, 1),
                new TransportAction(1, 1),
                new TransportAction(2, 0),
                new TransportAction(0, 2)
            };

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();

                foreach (var action in actions)
                {
                    var newNode = currentNode.Transport(action);

                    if (newNode == null)
                    {
                        continue;
                    }

                    // If the node is already added to graph, get the complete object of it. 
                    // Because we need the information of its edges.
                    if (_nodes.Contains(newNode.GetHashCode()))
                    {
                        newNode = (StatusNode) _nodes[newNode.GetHashCode()];
                    }

                    currentNode.Edges.Add(newNode);

                    if (!_nodes.Contains(newNode.GetHashCode()) && newNode.IsLegal())
                    {
                        _nodes.Add(newNode.GetHashCode(), newNode);
                        nodeQueue.Enqueue(newNode);
                    }
                }
            }
        }

        private void FindPath()
        {
            var nodeQueue = new Queue<StatusNode>();

            // Find the path from each node to destination with BFS.
            var destNode =
                (StatusNode) _nodes[StatusNode.CalculateStatusHashCode(new[] {_priests, _devils, 0, 0, 1})];
            // There is no way to access destination.   
            if (destNode == null)
            {
                return;
            }

            destNode.Distance = 0;
            nodeQueue.Enqueue(destNode);

            while (nodeQueue.Count > 0)
            {
                var currentNode = nodeQueue.Dequeue();
                currentNode.IsVisited = true;

                foreach (var edge in currentNode.Edges)
                {
                    if (edge.Distance <= currentNode.Distance + 1)
                    {
                        continue;
                    }

                    edge.Next = currentNode;
                    edge.Distance = currentNode.Distance + 1;
                    if (!edge.IsVisited && edge.IsLegal())
                    {
                        nodeQueue.Enqueue(edge);
                    }
                }
            }
        }

        public StatusNode FindNodeByStatus(int[] status)
        {
            return (StatusNode) _nodes[StatusNode.CalculateStatusHashCode(status)];
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            foreach (var nodeObj in _nodes.Values)
            {
                var node = (StatusNode) nodeObj;
                s.Append(node).Append("\n");
            }

            return s.ToString();
        }
    }

}