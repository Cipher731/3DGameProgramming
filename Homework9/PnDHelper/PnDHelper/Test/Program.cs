using System;

namespace PnDHelper.Test
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var graph = new StatusGraph(3, 3);
            Console.WriteLine(graph);
            
            var node = graph.FindNodeByStatus(new[] {0, 0, 3, 3, 0});
            
            while (node != null)
            {
                Console.Write(node.GetStatusPattern());
                if (node.Next != null)
                {
                    Console.Write(" -> ");
                }

                node = node.Next;
            }
            Console.WriteLine();
            
            node = graph.FindNodeByStatus(new[] {0, 2, 3, 1, 1});
            
            while (node != null)
            {
                Console.Write(node.GetStatusPattern());
                if (node.Next != null)
                {
                    Console.Write(" -> ");
                }

                node = node.Next;
            }
        }
    }
}