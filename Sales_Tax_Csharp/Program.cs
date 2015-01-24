using System;
using System.Collections.Generic;

namespace Trains_Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create Graph object to hold the directed graph;
            Graph<char> demoGraph = new Graph<char>(5);
            //add vertices to the graph
            demoGraph.Push('A');
            demoGraph.Push('B');
            demoGraph.Push('C');
            demoGraph.Push('D');
            demoGraph.Push('E');
            //connect the vertices with directed edges with weight value
            demoGraph.AttachEdge(0, 1, 5);
            demoGraph.AttachEdge(1, 2, 4);
            demoGraph.AttachEdge(2, 3, 8);
            demoGraph.AttachEdge(3, 2, 8);
            demoGraph.AttachEdge(3, 4, 6);
            demoGraph.AttachEdge(0, 3, 5);
            demoGraph.AttachEdge(2, 4, 2);
            demoGraph.AttachEdge(4, 1, 3);
            demoGraph.AttachEdge(0, 4, 7);
            demoGraph.Vertex[0].isEdgeOf(demoGraph.Vertex[1], 5);
            demoGraph.Vertex[1].isEdgeOf(demoGraph.Vertex[2], 4);
            demoGraph.Vertex[2].isEdgeOf(demoGraph.Vertex[3], 8);
            demoGraph.Vertex[3].isEdgeOf(demoGraph.Vertex[2], 8);
            demoGraph.Vertex[3].isEdgeOf(demoGraph.Vertex[4], 6);
            demoGraph.Vertex[0].isEdgeOf(demoGraph.Vertex[3], 5);
            demoGraph.Vertex[2].isEdgeOf(demoGraph.Vertex[4], 2);
            demoGraph.Vertex[4].isEdgeOf(demoGraph.Vertex[1], 3);
            demoGraph.Vertex[0].isEdgeOf(demoGraph.Vertex[4], 7);
            
            //create a list to hold the route to be sent to the Graph to determine if the route exists
            //add vertices to the route list to search for and then call the DirectRoute function of Graph to test if the route exists
            //if the route exists, the distance of the route is returned. if no route exists, "NO ROUTE FOUND" is returned.
            //test route A-B-C
            demoGraph.route.Add(demoGraph.Vertex[0]);
            demoGraph.route.Add(demoGraph.Vertex[1]);
            demoGraph.route.Add(demoGraph.Vertex[2]);
            Console.WriteLine("Output #1: {0}", demoGraph.DirectRoute(demoGraph.route.ToArray()));
            //clear the route list for the next test
            demoGraph.route.Clear();
            //test route A-D
            demoGraph.route.Add(demoGraph.Vertex[0]);
            demoGraph.route.Add(demoGraph.Vertex[3]);
            Console.WriteLine("Output #2: {0}", demoGraph.DirectRoute(demoGraph.route.ToArray()));
            //clear the route list for the next test
            demoGraph.route.Clear();
            //test route A-D-C
            demoGraph.route.Add(demoGraph.Vertex[0]);
            demoGraph.route.Add(demoGraph.Vertex[3]);
            demoGraph.route.Add(demoGraph.Vertex[2]);
            Console.WriteLine("Output #3: {0}", demoGraph.DirectRoute(demoGraph.route.ToArray()));
            //clear the route list for the next test
            demoGraph.route.Clear();
            //test route A-E-B-C-D
            demoGraph.route.Add(demoGraph.Vertex[0]);
            demoGraph.route.Add(demoGraph.Vertex[4]);
            demoGraph.route.Add(demoGraph.Vertex[1]);
            demoGraph.route.Add(demoGraph.Vertex[2]);
            demoGraph.route.Add(demoGraph.Vertex[3]);
            Console.WriteLine("Output #4: {0}", demoGraph.DirectRoute(demoGraph.route.ToArray()));
            //clear the route list for the next test
            demoGraph.route.Clear();
            //test route A-E-D
            demoGraph.route.Add(demoGraph.Vertex[0]);
            demoGraph.route.Add(demoGraph.Vertex[4]);
            demoGraph.route.Add(demoGraph.Vertex[3]);
            Console.WriteLine("Output #5: {0}", demoGraph.DirectRoute(demoGraph.route.ToArray()));
            //test whether a route exists from one vertex to another that is three or less stops
            //return the number of routes found
            //test vertex C to vertex C
            demoGraph.dfs(demoGraph.Vertex[2], demoGraph.Vertex[2].m_node.ToString(), 3);
            Console.WriteLine("Output #6: {0}", demoGraph.numOfRoutesUnderNStops(demoGraph.Vertex[2], demoGraph.Vertex[2], 3));
            //test whether a route exists from one vertex to antother that is exactly four stops in length
            //return the number of routes found
            //test from vertex A to vertex C
            Console.WriteLine("Output #7: {0}", demoGraph.numOfRoutesUnderNStops(demoGraph.Vertex[0], demoGraph.Vertex[2], 4));

            Console.WriteLine("Output #8: {0}", demoGraph.shortestRoute(0, 2));
            Console.WriteLine("Output #9: {0}", demoGraph.shortestRoute(1, 1));

            demoGraph.test("C", "C", 0);

            //Console.WriteLine("Output #10: {0}", demoGraph.underThirty(2, 2));
        }

        /*---------------------------------------------------------------------------------------------------
         * 
         * Usage: This module will take a directed graph as a string array for the first argument and a route
         *        as a string for the second argument. It will then calculate the distance of the route by
         *        comparing it to the directed graph and return it. If a part of the route 
         *        does not exist, it will return NO SUCH ROUTE.
         * 
         * Example: string[] graph = { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" };
         *          string fullRoute = "A-B-C";
         *          test1(graph, fullRoute);
         *          
         *          Return: 9
         * 
         * -----------------------------------------------------------------------------------------------*/
        string test1(string[] graph, string fullRoute)
        {
            int sum = 0;
            char source;
            char dest;
            int numOfDestinations = (fullRoute.Length + 1) / 2;
            int numOfStops = 1;
            for (int i = 0; i < fullRoute.Length - 1; i++)
            {
                foreach (string route in graph)
                {
                    if (fullRoute[i] != '-')
                    {
                        source = fullRoute[i];
                        dest = fullRoute[i + 2];
                        if (route[0] == source && route[1] == dest)
                        {
                            sum += (route[2] - 48);
                            numOfStops++;
                        }
                    }
                }
            }
            if (numOfStops == numOfDestinations)
                return sum.ToString();
            else
                return "NO SUCH ROUTE";
        }

        /*----------------------------------------------------------------------------------------------
         * 
         * Usage: This module takes the directed graph as the only arguement. It disects each route in the
         *        graph and then finds all of those that start with C and ends with C that are less than
         *        three stops and returns the number of routes it found.
         *        
         * Example: string[] graph = { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" };
         *          test2(graph);
         *          
         *          Returns: 2
         * 
         * --------------------------------------------------------------------------------------------*/

        int test2(string[] graph)
        {
            int sum = 0;
            foreach (string route_1 in graph)
            {
                if (route_1[0] == 'C')
                {
                    foreach (string route_2 in graph)
                    {
                        if (route_2[0] == route_1[1] && route_2[1] == route_1[0])
                        {
                            sum += 1;
                        }
                        if (route_2[0] == route_1[1])
                        {
                            foreach (string route_3 in graph)
                            {
                                if (route_3[0] == route_2[1] && route_3[1] == 'C')
                                {
                                    sum += 1;
                                }
                            }
                        }
                    }
                }             
            }
            return sum;
        }

        /*--------------------------------------------------------------------------------------------
         * 
         * Usage: This module will search through the directed graph that is passed to it as an argument
         *        and check for a path from A to C that is exactly four stops long. It does so by
         *        searching first for a route that starts with A, then keeps looping through the graph
         *        each time it finds the next route that allows it to travel until it reaches the fourth
         *        stop and checks if it ends in C. If it does it adds up the total number of routes it
         *        found and returns it.
         *        
         * Example: string[] graph = { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" };
         *          temp3(graph);
         *          
         *          Returns: 3
         * 
         * -----------------------------------------------------------------------------------------*/

        int test3(string[] tmp)
        {
            int sum = 0;
            foreach (string route_1 in tmp)
            {
                if (route_1[0] == 'A')
                {
                    foreach (string route_2 in tmp)
                    {
                        if (route_2[0] == route_1[1])
                        {
                            foreach (string route_3 in tmp)
                            {
                                if (route_3[0] == route_2[1])
                                {
                                    foreach (string route_4 in tmp)
                                    {
                                        if (route_4[0] == route_3[1] && route_4[1] == 'C')
                                        {
                                            sum += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sum;
        }

        int test4(string[] graph)
        {
            int sum = 0;
            int route = 0;

            foreach (string route_1 in graph)
            {
                if (route_1[0] == 'A')
                {
                    route += 1;
                    Console.WriteLine(route);
                }
            }
            Console.WriteLine(route);
            return sum;
        }
    }
}
