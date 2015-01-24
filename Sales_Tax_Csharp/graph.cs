using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains_Csharp
{
    class Graph<T>
    {
        public Graph(int numVerts)
        {
            m_maxVerts = numVerts;
            m_adjMatrix = new int[m_maxVerts][];
            m_vertVisits = new int[m_maxVerts];

            for (int i = 0; i < m_maxVerts; i++)
            {
                m_adjMatrix[i] = new int[m_maxVerts];
                m_adjMatrix[i][i] = 0;
                m_vertVisits[i] = 0;
            }
        }

        ~Graph()
        {
            if (m_adjMatrix != null)
            {
                for (int i = 0; i < m_maxVerts; i++)
                {
                    if(m_adjMatrix[i] != null)
                    {
                        m_adjMatrix[i] = null;
                    }
                }
                m_adjMatrix = null;
            }

            if (m_vertVisits != null)
            {
                m_vertVisits = null;
            }
        }

        public bool Push(T node)
        {
            if ((int)m_vertices.Count >= m_maxVerts)
                return false;

            m_vertices.Add(new GraphVertex<T>(node));
            return true;
        }

        public List<GraphVertex<T>> Vertex
        {
            get
            {
                return m_vertices;
            }
        }

        public void AttachEdge(int index1, int index2, int weight)
        {
            m_adjMatrix[index1][index2] = weight;
        }

        public void outputMatrix()
        {
            for (int i = 0; i < m_maxVerts; i++)
            {
                for (int j = 0; j < m_maxVerts; j++)
                {
                    if (j < m_maxVerts - 1)
                        Console.Write(" {0}", m_adjMatrix[i][j].ToString());
                    else
                        Console.WriteLine(" {0}", m_adjMatrix[i][j].ToString());
                }
            }
        }

        public void outputVertices()
        {
            for (int i = 0; i < m_maxVerts; i++)
                Console.WriteLine("{0}. {1}", i, m_vertices[i].m_node);
        }

        public void outputEdges()
        {
            for (int i = 0; i < m_maxVerts; i++)
            {
                for (int j = 0; j < m_maxVerts; j++)
                {
                    if (m_adjMatrix[i][j] != 0)
                    {
                        Console.WriteLine("{0}:{1}", m_vertices[i].m_node, m_vertices[j].m_node);
                    }
                }
            }
        }

        public int getNextUnvisitedVertex(int index)
        {
            for (int i = 0; i < m_maxVerts; i++)
            {
                if (m_adjMatrix[index][i] > 0 && m_vertVisits[i] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public int getNextUnvisitedVertexWeight(int index)
        {
            for (int i = 0; i < m_maxVerts; i++)
            {
                if (m_adjMatrix[index][i] > 0 && m_vertVisits[i] == 0)
                {
                    return m_adjMatrix[index][i];
                }
            }

            return -1;
        }

        public int getEdgeWeight(int index1, int index2)
        {
            return m_adjMatrix[index1][index2];
        }

        public string DirectRoute(GraphVertex<T>[] route)
        {
            GraphVertex<T> vert;
            GraphVertex<T> nextVert;
            int weight = 0;
            int totalWeight = 0;
            for (int i = 0; i < route.Length - 1; i++)
            {
                vert = route[i];
                nextVert = route[i+1];
                
                if (vert.Edges.ContainsKey(nextVert))
                {
                     weight = vert.Edges[nextVert];
                     totalWeight += weight;
                }
                else
                    return "NO SUCH ROUTE";
            }

            return totalWeight.ToString();
        }

        public string DepthFirstSearch(int startIndex, int endIndex)
        {
            m_vertVisits[startIndex] = 1;

            Console.WriteLine(m_vertices[startIndex].m_node);

            Stack<int> searchStack = new Stack<int>();
            int vert = 0;
            int weight = 0;
            int totalWeight = 0;

            searchStack.Push(startIndex);

            while (searchStack.Count != 0)
            {
                vert = getNextUnvisitedVertex(searchStack.Peek());
                weight = getNextUnvisitedVertexWeight(searchStack.Peek());

                if (vert == -1)
                {
                    searchStack.Pop();
                }
                else
                {
                    m_vertVisits[vert] = 1;
                    totalWeight += weight;

                    Console.WriteLine(m_vertices[vert].m_node);

                    searchStack.Push(vert);
                }

                if (vert == endIndex)
                {
                    for (int i = 0; i < m_maxVerts; i++)
                    {
                        m_vertVisits[i] = 0;
                    }
                    return totalWeight.ToString();
                }
            }

            for (int i = 0; i < m_maxVerts; i++)
            {
                m_vertVisits[i] = 0;
            }
            return "NO SUCH ROUTE";
        }

        public string BreadthFirstSearch(int startIndex, int endIndex)
        {

            m_vertVisits[startIndex] = 1;

            Queue<int> searchQueue = new Queue<int>();
            int vert1 = 0, vert2 = 0;

            searchQueue.Enqueue(startIndex);

            Console.WriteLine("{0}", m_vertices[startIndex].m_node);

            while(searchQueue.Count != 0)
            {
                vert1 = searchQueue.Dequeue();

                if (vert1 == endIndex)
                {
                    for (int i = 0; i < m_maxVerts; i++)
                    {
                        m_vertVisits[i] = 0;
                    }
                    return "true";
                }

                while ((vert2 = getNextUnvisitedVertex(vert1)) != -1)
                {
                    Console.WriteLine("{0}", m_vertices[vert2].m_node);
                    m_vertVisits[vert2] = 1;

                    searchQueue.Enqueue(vert2);
                }
            }

            for (int i = 0; i < m_maxVerts; i++)
            {
                m_vertVisits[i] = 0;
            }
            return "false;";
        }

        public int numOfRoutesUnderNStops(GraphVertex<T> startIndex, GraphVertex<T> endIndex, int stops)
        {
            List<Dictionary<GraphVertex<T>, int>> edgeList = new List<Dictionary<GraphVertex<T>,int>>();
            int numOfRoutes = 0;

            edgeList.Add(startIndex.Edges);

            for (int i = 0; i < stops; i++)
            {
                
                foreach(KeyValuePair<GraphVertex<T>, int> edge in edgeList[i])
                {
                    edgeList.Add(edge.Key.Edges);
                }
            }

            foreach(Dictionary<GraphVertex<T>, int> list in edgeList)
            {
                Console.WriteLine("List {0}", edgeList.IndexOf(list));
                foreach (KeyValuePair<GraphVertex<T>, int> edge in list)
                {
                    Console.WriteLine(edge.Key.m_node);
                    if (edge.Key == endIndex)
                        numOfRoutes += 1;
                }
            }

            return numOfRoutes;
        }

        public int tripCount = 0;
        public void dfs(GraphVertex<T> end, string path, int maxLength)
        {
            
            // this is for debug and trace
            // System.out.println(";; " + path);
        
            // if the path reach the maximum stops, then cancel search
            if (path.Length - 1 > maxLength) return;
        
            // check if we have reach the "end" node
            if ( path.Length > 1 && path[path.Length - 1].ToString() == end.m_node.ToString() ) {
                tripCount++;
                Console.WriteLine(path + ", " + tripCount);
            }
        
            // caculate the lastest node index in map
            char lastChar = path[path.Length - 1];
            GraphVertex<T> lastNodeIndex = Vertex[lastChar - 'A'];
            //Console.WriteLine(lastNodeIndex.m_node);
        
            // loop all nodes in map which connected to lastNode, and try it
            for ( int i=0; i < lastNodeIndex.Edges.Count; i++) 
            {
                char newChar = lastNodeIndex.Edges.Keys.ToList()[i].m_node.ToString()[0];
                dfs(end, path + newChar, maxLength);
            }
        }

        public int fourStop(int startIndex, int endIndex)
        {
            Stack<int> searchStack1 = new Stack<int>();
            Stack<int> searchStack2 = new Stack<int>();
            Stack<int> searchStack3 = new Stack<int>();

            int numOfRoutes = 0;

            for (int i = 0; i < m_maxVerts; i++)
            {
                if (m_adjMatrix[startIndex][i] != 0)
                    searchStack1.Push(i);
            }

            for (int i = 0; i < searchStack1.Count; i++)
            {
                for (int j = 0; j < m_maxVerts; j++)
                {
                    if (m_adjMatrix[searchStack1.ElementAt(i)][j] != 0)
                    {
                        searchStack2.Push(j);
                    }
                }
            }

            for (int i = 0; i < searchStack2.Count; i++)
            {
                for (int j = 0; j < m_maxVerts; j++)
                {
                    if (m_adjMatrix[searchStack2.ElementAt(i)][j] != 0)
                    {
                        searchStack3.Push(j);
                    }
                }
            }

            for (int i = 0; i < searchStack3.Count; i++)
            {
                for (int j = 0; j < m_maxVerts; j++)
                {
                    if (m_adjMatrix[searchStack3.ElementAt(i)][j] != 0 && j == endIndex)
                    {
                        numOfRoutes += 1;
                    }
                }
            }

            return numOfRoutes;
        }

        public int shortestRoute(int startIndex, int endIndex)
        {
            List<int> tempStack = new List<int>();
            int totalChecked = 0;
            int nextVert = 0;
            int currentVert = startIndex;
            int totalWeight = 0;

            while (totalChecked < m_maxVerts - 1)
            {
                for (int i = 0; i < m_maxVerts; i++)
                {
                    if (m_adjMatrix[currentVert][i] != 0)
                    {
                        tempStack.Add(i);
                    }
                       
                }

                nextVert = tempStack[0];

                for (int i = 0; i < tempStack.Count - 1; i++)
                {
                    if (m_adjMatrix[currentVert][tempStack[i + 1]] < m_adjMatrix[currentVert][nextVert] && m_adjMatrix[currentVert][tempStack[i+1]] != 0)
                    {
                        nextVert = tempStack[i+1];
                    }
                }

                totalWeight += m_adjMatrix[currentVert][nextVert];

                if (nextVert == endIndex)
                {
                    return totalWeight;
                }

                tempStack.Clear();

                currentVert = nextVert;
                totalChecked++;
            }

            return totalWeight;
        }

        public GraphVertex<T> Search(GraphVertex<T> root, T edgeToSearchFor)
        {
            Queue<GraphVertex<T>> Q = new Queue<GraphVertex<T>>();
            HashSet<GraphVertex<T>> S = new HashSet<GraphVertex<T>>();
            Q.Enqueue(root);
            S.Add(root);

            while (Q.Count > 0)
            {
                GraphVertex<T> p = Q.Dequeue();
                if (p.m_node.Equals(edgeToSearchFor))
                    return p;
                foreach (KeyValuePair<GraphVertex<T>, int> edge in p.Edges)
                {
                    if (!S.Contains(edge.Key))
                    {
                        Q.Enqueue(edge.Key);
                        S.Add(edge.Key);
                    }
                }
            }
            return null;
        }

        public void Traverse(GraphVertex<T> root)
        {
            Queue<GraphVertex<T>> traverseOrder = new Queue<GraphVertex<T>>();

            Queue<GraphVertex<T>> Q = new Queue<GraphVertex<T>>();
            HashSet<GraphVertex<T>> S = new HashSet<GraphVertex<T>>();
            Q.Enqueue(root);
            S.Add(root);
            int weight = 0;

            while (Q.Count > 0)
            {
                GraphVertex<T> p = Q.Dequeue();
                traverseOrder.Enqueue(p);

                foreach (KeyValuePair<GraphVertex<T>, int> edge in p.Edges)
                {
                    if (!S.Contains(edge.Key))
                    {
                        Q.Enqueue(edge.Key);
                        S.Add(edge.Key);
                        weight += edge.Value;
                    }
                }
            }

            while (traverseOrder.Count > 0)
            {
                GraphVertex<T> p = traverseOrder.Dequeue();
                Console.WriteLine(p.m_node);
            }
            Console.WriteLine(weight);
        }

        public void test(String end, String path, int cost) 
        {
            if (cost >= 30)
                return;

            if (cost > 0 && path.EndsWith(end)) {
                Console.WriteLine(path + ", " + cost);
            }

            char lastChar = path.ElementAt(path.Length - 1);
            int lastNodeIndex = lastChar - 'A';

            for (int i = 0; i < m_adjMatrix[lastNodeIndex].Length; i++) {
                char newChar = (char) (i + 'A');
                Console.WriteLine(i);
                int newCost = m_adjMatrix[lastNodeIndex][i];
                if (newCost > 0) {
                    test(end, path + newChar, cost + newCost);
                }
            }
        }

        public List<GraphVertex<T>> route = new List<GraphVertex<T>>();

        private List<GraphVertex<T>> m_vertices = new List<GraphVertex<T>>();
        private int m_maxVerts;
        private int[][] m_adjMatrix;
        private int[] m_vertVisits;
    }
}
