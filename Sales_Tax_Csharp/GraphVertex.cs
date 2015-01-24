using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains_Csharp
{
    class GraphVertex<T>
    {
        public GraphVertex(T node)
        {
            this.m_node = node;
        }

        public T m_node { get; set; }

        public Dictionary<GraphVertex<T>, int> Edges
        {
            get
            {
                return EdgeList;
            }
        }

        public void isEdgeOf(GraphVertex<T> v, int  weight)
        {
            EdgeList.Add(v, weight);
        }

        Dictionary<GraphVertex<T>, int> EdgeList = new Dictionary<GraphVertex<T>, int>();
    }
}
