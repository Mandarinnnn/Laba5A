using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba5
{
    class Vertex
    {
        public bool Checked;
        public int x, y, r;
        public Label lbl;
        public int Number;
        Pen pen = new Pen(Color.Black);
        SolidBrush brush = new SolidBrush(Color.White);
        Rectangle rect;
        public bool Colored;

        public Vertex(int x, int y, int r, int Number)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.Number = Number;
            Checked = false;
            lbl = new Label();
            lbl.AutoSize = true;
            lbl.Size = new Size(10, 15);
            lbl.Text = Number.ToString();
            lbl.Location = new Point(x - 4 - lbl.Height / 7, y - 5 - lbl.Width / 7);
            rect = new Rectangle(x - r, y - r, r * 2, r * 2);
            Colored = false;

        }

        public void Draw(Panel panel1, Graphics g)
        {
            panel1.Controls.Add(lbl);
            if (Colored == true)
            {
                brush.Color = Color.Blue;
                lbl.BackColor = Color.Blue;
                lbl.ForeColor = Color.White;
            }
            else
            {
                brush.Color = Color.White;
                lbl.BackColor = Color.White;
                lbl.ForeColor = Color.Black;
            }
            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }

        public bool isClicked(MouseEventArgs e)
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= 37 * 37)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    class Edge
    {
        public Vertex from;
        public Vertex to;
        Pen pen = new Pen(Color.Black);
        public bool Colored;

        public Edge(Vertex from, Vertex to)
        {
            this.from = from;
            this.to = to;
            Colored = false;
        }

        public void Draw(Panel panel1, Graphics g)
        {
            if (Colored == true)
            {
                pen.Color = Color.Red;
            }
            else
            {
                pen.Color = Color.Black;
            }
            g.DrawLine(pen, from.x, from.y, to.x, to.y);
        }

        public void isThisEdge(int number1, int number2)
        {
            if ((from.Number == number1 && to.Number == number2) || (from.Number == number2 && to.Number == number1))
            {
                Colored = true; 
            }
        }
    }

    class Graph
    {
        public List<Vertex> V = new List<Vertex>();
        public List<Edge> E = new List<Edge>();
        public List<int> CheckedVertecis = new List<int>();
        int[,] AdjacencyMatrix;

        public void AddV(Vertex v)
        {
            V.Add(v);

        }

        public void AddE(Vertex from, Vertex to)
        {
            Edge e = new Edge(from, to);
            E.Add(e);
        }

        public void Draw(Panel panel1, Graphics g)
        {
            for (int i = 0; i < E.Count; i++)
            {
                E[i].Draw(panel1, g);
            }
            for (int i = 0; i < V.Count; i++)
            {
                V[i].Draw(panel1, g);
            }
        }

        public bool IsClicked(MouseEventArgs e)
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (V[i].isClicked(e) == true)
                {
                    V[i].Checked = true;
                    CheckedVertecis.Add(i);
                    return true;
                }
            }
            return false;
        }

        public void MakeChecked(MouseEventArgs e)
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (V[i].isClicked(e) == true)
                {
                    V[i].Checked = true;
                }
            }
        }

        public void MakeAllnotChecked()
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (V[i].Checked == true)
                {
                    V[i].Checked = false;
                }
            }
        }

        public void CreateAdjacencyMatrix()
        {
            AdjacencyMatrix = new int[V.Count, V.Count];
            for (int i = 0; i < E.Count; i++)
            {
                AdjacencyMatrix[E[i].from.Number - 1, E[i].to.Number - 1] = 1;
                AdjacencyMatrix[E[i].to.Number - 1, E[i].from.Number - 1] = 1;
            }
        }

        /*public void DFS(int start, Panel panel)
        {
            List<int> L = new List<int>();
            Stack<int> S = new Stack<int>();
            S.Push(start);
            int[,] array = new int[V.Count, 2];


            while (S.Count != 0)
            {

                int StackElement = S.Pop();

                if (L.Contains(StackElement) == false)//если v нет в списке посещенных
                {
                    L.Add(StackElement);
                    V[StackElement - 1].Colored = true;

                    for (int i = 0; i < E.Count; i++)
                    {
                        E[i].isThisEdge(StackElement, array[StackElement - 1, 1]);
                    }

                    System.Threading.Thread.Sleep(1000);
                    panel.Refresh();
                    //здесь добавляем в список смежные вершины, которых нет в списке
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (AdjacencyMatrix[StackElement - 1, i] == 1)
                        {
                            if (L.Contains(i + 1) == false)
                            {
                                S.Push(i + 1);
                                array[i, 1] = StackElement;
                            }
                        }

                    }
                }
            }
        }*/

        public bool DoHasAdjacencyVertex(int x)
        {
            for(int i=0;i<V.Count;i++)
            {
                if (AdjacencyMatrix[x-1, i] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAdjacencyVertex(int x)
        {
            for (int i = 0; i < V.Count; i++)
            {
                if (AdjacencyMatrix[x-1, i] == 1)
                {
                    return i+1;
                }
            }
            return -1;
        }

        public bool isRequired()
        {
            int[] arr = new int[V.Count];
            for(int i =0;i<V.Count;i++)
            {
                arr[i] = 0;
            }
            for(int i =0;i<V.Count;i++)
            {
                for(int j =0;j<V.Count;j++)
                {
                    if (AdjacencyMatrix[i,j]==1)
                    {
                        arr[i] = arr[i] + 1;
                    }
                }
            }
            for (int i = 0; i < V.Count; i++)
            {
                if(arr[i]%2==1 || arr[i]==0)
                {
                    return false;
                }
            }
            return true;
        }


        public void EulerCycle(int start, Panel panel1, Label label)
        {
            if (isRequired() == true)
            {
                Stack<int> S = new Stack<int>();
                Stack<int> C = new Stack<int>();

                S.Push(start);
                int v;
                int u;
                while (S.Count != 0)
                {
                    v = S.Peek();
                    if (DoHasAdjacencyVertex(v) == true)//надо перебрать чтобы посмотреть есть ли смежные
                    {
                        u = GetAdjacencyVertex(v);//первый элемент из AdjacencyMatrix[v-1,] смежный

                        S.Push(u);
                        AdjacencyMatrix[v - 1, u - 1] = 0; ;//ставим вместо 1 0 там где AdjacencyMatrix[v-1,u-1]
                        AdjacencyMatrix[u - 1, v - 1] = 0;
                        v = u;
                    }
                    else
                    {
                        //v убираем из стека

                        C.Push(S.Pop());
                    }
                }

                Stack<int> Creverse = new Stack<int>();
                int size = C.Count;
                for (int i = 0; i < size - 1; i++)
                {
                    int tmp = C.Pop();
                    label.Text += tmp.ToString();
                    label.Text += " ";
                    if (V[tmp - 1].Colored == false)
                    {
                        V[tmp - 1].Colored = true;
                    }
                    panel1.Refresh();
                    System.Threading.Thread.Sleep(1000);
                    int x = C.Peek();
                    for (int j = 0; j < E.Count; j++)
                    {
                        E[j].isThisEdge(tmp, x);
                    }
                }
                label.Text += C.Pop().ToString();
                panel1.Refresh();
            }
            else
            {
                MessageBox.Show("Граф не эйлеров");
            }
        }


        public void MakenotColored()
        {
            System.Threading.Thread.Sleep(3000);
            for (int i = 0; i < V.Count; i++)
            {
                V[i].Colored = false;
            }
            for (int i = 0; i < E.Count; i++)
            {
                E[i].Colored = false;
            }
        }
        public void RemoveV()
        {
            for (int i = 0; i < V.Count; i++)
            {
                V[i].lbl.Dispose();
            }
            for (int i = V.Count-1; i >=0; i--)
            {
                V.RemoveAt(i);
            }

        }

        public void RemoveE()
        {
            for (int i = E.Count - 1; i >= 0; i--)
            {
                E.RemoveAt(i);
            }
        }

    }
}
