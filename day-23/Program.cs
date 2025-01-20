using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-23.in");

            Dictionary<string, List<string>> graph = [];
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] vertices = line.Split('-');
                if (!graph.ContainsKey(vertices[0]))
                {
                    graph[vertices[0]] = [];
                }
                if (!graph.ContainsKey(vertices[1]))
                {
                    graph[vertices[1]] = [];
                }

                graph[vertices[0]].Add(vertices[1]);
                graph[vertices[1]].Add(vertices[0]);
            }

            /* Part 1 */
            HashSet<(string, string, string)> interconnectedComputers = [];
            foreach (string src in graph.Keys)
            {
                if (src[0] == 't')
                {
                    foreach (string first in graph[src])
                    {
                        foreach (string second in graph[first])
                        {
                            if (graph[second].Contains(src))
                            {
                                List<string> cycle = [src, first, second];
                                cycle.Sort();
                                interconnectedComputers.Add((cycle[0], cycle[1], cycle[2]));
                            }
                        }
                    }
                }
            }
            Console.WriteLine(interconnectedComputers.Count);

            /* Part 2 */
            var maxClique = FindMaximumClique(graph).ToList();
            maxClique.Sort();
            Console.WriteLine(string.Join(",", maxClique));

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Part 2
    // Uses the Bron-Kerbosch algorithm to find the maximum clique
    public static HashSet<string> FindMaximumClique(Dictionary<string, List<string>> graph)
    {
        // Find all maximumal cliques and then return the largest one
        var cliques = new List<HashSet<string>>();
        BronKerboschRecursive(new HashSet<string>(), new HashSet<string>(graph.Keys), new HashSet<string>(), graph, cliques);
        return cliques.OrderByDescending(c => c.Count).First();
    }

    private static void BronKerboschRecursive(
        HashSet<string> R, // vertices in current clique
        HashSet<string> P, // possible vertices to add
        HashSet<string> X, // vertices that cannot be added
        Dictionary<string, List<string>> graph,
        List<HashSet<string>> cliques)
    {
        if (P.Count == 0 && X.Count == 0)
        {
            cliques.Add(new HashSet<string>(R));
            return;
        }

        foreach (var v in P.ToList())
        {
            var neighbors = new HashSet<string>(graph[v]);
            BronKerboschRecursive(
                new HashSet<string>(R) { v },
                new HashSet<string>(P.Intersect(neighbors)),
                new HashSet<string>(X.Intersect(neighbors)),
                graph,
                cliques
            );

            P.Remove(v);
            X.Add(v);
        }
    }
}
