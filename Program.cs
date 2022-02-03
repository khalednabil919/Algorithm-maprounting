using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Final
{
    class Program
    {
        class PriorityQueue
        {
            public List<KeyValuePair<int, double>> list;
            public int Count { get { return list.Count; } }

            public PriorityQueue()
            {
                list = new List<KeyValuePair<int, double>>();
            }

            public PriorityQueue(int count)
            {
                list = new List<KeyValuePair<int, double>>(count);
            }


            public void push(KeyValuePair<int, double> x)
            {
                list.Add(x);
                int i = Count - 1;

                while (i > 0)
                {
                    int p = (i - 1) / 2;
                    if (list[p].Value <= x.Value) break;

                    list[i] = list[p];
                    i = p;
                }

                if (Count > 0) list[i] = x;
            }

            public KeyValuePair<int, double> pop()
            {
                KeyValuePair<int, double> min = top();
                KeyValuePair<int, double> root = list[Count - 1];
                list.RemoveAt(Count - 1);

                int i = 0;
                while (i * 2 + 1 < Count)
                {
                    int a = i * 2 + 1;
                    int b = i * 2 + 2;
                    int c = b < Count && list[b].Value < list[a].Value ? b : a;

                    if (list[c].Value >= root.Value) break;
                    list[i] = list[c];
                    i = c;
                }

                if (Count > 0) list[i] = root;
                return min;
            }

            public KeyValuePair<int, double> top()
            {
                if (Count == 0) throw new InvalidOperationException("Queue is empty.");
                return list[0];
            }

            public void Clear()
            {
                list.Clear();
            }
        }
        public static void DeepWeeb_dijkstra(List<KeyValuePair<int, double>>[] neighbours, int nodes, int numbers, Dictionary<KeyValuePair<int, int>, double> dic)
        {
            double[] time = new double[nodes];
            int[] parent = new int[nodes];
            int result = 0;
            PriorityQueue pq = new PriorityQueue();
            for (int i = 0; i < nodes; i++)
            {
                time[i] = double.MaxValue;
                parent[i] = i;
            }

            pq.push(new KeyValuePair<int, double>(nodes - 2, 0));
            double sum = 0;
            // double sum_dis = 0;
            //double sum_src = 0;
            while (pq.Count != 0)
            {
                int node = pq.top().Key;
                double weight = pq.top().Value;
                pq.pop();
                for (int i = 0; i < neighbours[node].Count; i++)
                {
                    if (neighbours[node][i].Value + weight < time[neighbours[node][i].Key])
                    {

                        time[neighbours[node][i].Key] = neighbours[node][i].Value + weight;
                        pq.push(new KeyValuePair<int, double>(neighbours[node][i].Key, time[neighbours[node][i].Key]));
                        parent[neighbours[node][i].Key] = node;
                        //parent[node]=neighbours[node][i].Key;
                    }
                    // neighbours[node][i].Key == nodes )neighbours[node][i]
                    if (neighbours[node][i].Key == nodes - 2)
                    {
                        // dic[new KeyValuePair<int, int>(neighbours[node][i].Key,node)] = (time[neighbours[node][i].Key]/60)*5;
                        neighbours[node].Remove(neighbours[node][i]);
                    }
                    else if (neighbours[node][i].Key == nodes - 1)
                    {
                        //  dic[new KeyValuePair<int, int>(node, neighbours[node][i].Key)] =(time[neighbours[node][i].Key]/60)*5;
                        result = neighbours[node][i].Key;
                        neighbours[node].Remove(neighbours[node][i]);
                        numbers--;
                    }
                    if (numbers == 0)
                    {
                        double steen = 60;
                        double khmsa = 5;
                        dic[new KeyValuePair<int, int>(parent[nodes - 1], nodes - 1)] = (Math.Abs((time[nodes - 1] - time[parent[nodes - 1]])) / steen) * khmsa;
                        dic[new KeyValuePair<int, int>(nodes - 2, parent[nodes - 2])] = (Math.Abs((time[parent[nodes - 2]] - time[nodes - 2])) / steen) * khmsa;
                        double sum_src_dis = (Math.Abs((time[nodes - 1] - time[parent[nodes - 1]])) / steen) * khmsa +
                            (Math.Abs((time[parent[nodes - 2]] - time[nodes - 2])) / steen) * khmsa;
                        // Console.WriteLine(dic[new KeyValuePair<int, int>(nodes-2, parent[nodes - 2])]);
                        Console.Write("Time = ");
                        Console.WriteLine(time[result]);
                        int k = nodes - 1;
                        Stack<int> S = new Stack<int>();
                        //sum += dic[new KeyValuePair<int, int>(parent[k],k)];
                        //sum_dis = sum;
                        //Console.WriteLine(sum_dis);
                        k = parent[k];
                        S.Push(k);
                        while (parent[k] != nodes - 2)
                        {
                            sum += dic[new KeyValuePair<int, int>(k, parent[k])];
                            k = parent[k];
                            S.Push(k);

                        }
                        //k = parent[nodes - 2];
                        //sum_src = dic[new KeyValuePair<int, int>(parent[k],k)];
                        // sum += sum_src;
                        Console.Write("Distance = ");
                        Console.WriteLine(sum + sum_src_dis);
                        Console.Write("Walking Distance = ");
                        Console.WriteLine(sum_src_dis);
                        Console.Write("Vehicle Distance = ");
                        Console.WriteLine(sum);
                        Console.Write("Fucken Road = ");
                        while (S.Count != 0)
                        {
                            Console.Write(S.Peek()); Console.Write(" ");
                            S.Pop();
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        return;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            FileStream fsmap = new FileStream("map2.txt", FileMode.Open);
            StreamReader srmap = new StreamReader(fsmap);
            Dictionary<KeyValuePair<int, int>, double> dic = new Dictionary<KeyValuePair<int, int>, double>();
            //Dictionary<int, List<KeyValuePair<int, double>>> neighbours = new Dictionary<int, List<KeyValuePair<int, double>>>();
            List<KeyValuePair<double, double>> N = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<int, double>> q = new List<KeyValuePair<int, double>>();

            string s = srmap.ReadToEnd();
            string[] split = s.Split('\n');
            int nodes = Convert.ToInt32(split[0]);
            List<KeyValuePair<int, double>>[] neighbours = new List<KeyValuePair<int, double>>[nodes + 2];
            for (int i = 0; i < nodes; i++)
            {
                //s = srmap.ReadLine();
                string[] split1 = split[i+1].Split(' ');
                KeyValuePair<double, double> d = new KeyValuePair<double, double>(Convert.ToDouble(split1[1]), Convert.ToDouble(split1[2]));
                N.Add(d);//new KeyValuePair<double, double>(Convert.ToInt16(s[2]), Convert.ToInt16(s[4]));
            }
            //s = srmap.ReadLine();
           // string[] split2 = s.Split(' ');
            int neighbours_Push = Convert.ToInt16(split[nodes+1]);
            for (int i = 0; i < nodes + 2; i++) neighbours[i] = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < neighbours_Push; i++)
            {

                //s = srmap.ReadLine();
                string[] split3 = split[nodes+2+i].Split(' ');
                KeyValuePair<int, double> d = new KeyValuePair<int, double>(Convert.ToInt16(split3[1]), (Convert.ToDouble(split3[2]) / Convert.ToDouble(split3[3])) * 60);
                neighbours[Convert.ToInt16(split3[0])].Add(d);
                d = new KeyValuePair<int, double>(Convert.ToInt16(split3[0]), (Convert.ToDouble(split3[2]) / Convert.ToDouble(split3[3])) * 60);

                neighbours[Convert.ToInt16(split3[1])].Add(d);
                dic[new KeyValuePair<int, int>(Convert.ToInt16(split3[0]), Convert.ToInt16(split3[1]))] = Convert.ToDouble(split3[2]);
                dic[new KeyValuePair<int, int>(Convert.ToInt16(split3[1]), Convert.ToInt16(split3[0]))] = Convert.ToDouble(split3[2]);

            }

            srmap.Close();
            FileStream fsqueries = new FileStream("queries2.txt", FileMode.Open);
            StreamReader srqueries = new StreamReader(fsqueries);
            //List<KeyValuePair<double, double>> src = new List<KeyValuePair<double, double>>();
            //List<KeyValuePair<double, double>> dis = new List<KeyValuePair<double, double>>();
            //List<double> R =new List <double>();
            string s1 = srqueries.ReadToEnd();
            string[] split4 = s1.Split('\n');

            int no_of_queries = Convert.ToInt32(split4[0]);
            for (int i = 0; i < no_of_queries; i++)
            {
                //s1 = srqueries.ReadLine();
                string[] split5 = split4[i+1].Split(' ');

                //src.Add(new KeyValuePair<double, double>(Convert.ToDouble(split[0]), Convert.ToDouble(split[1])));
                // dis.Add(new KeyValuePair<double, double>(Convert.ToDouble(split[2]), Convert.ToDouble(split[3])));
                double R = Convert.ToDouble(split5[4]);
                double h = 1000;
                R = R / h;
                double x = Convert.ToDouble(split5[0]);
                double y = Convert.ToDouble(split5[1]);
                double x1 = Convert.ToDouble(split5[2]);
                double y1 = Convert.ToDouble(split5[3]);
                int numbers = 0;
                for (int j = 0; j < N.Count; j++)
                {
                    double walking = Math.Sqrt((x - N[j].Key) * (x - N[j].Key) + (y - N[j].Value) * (y - N[j].Value));
                    double walking1 = Math.Sqrt((x1 - N[j].Key) * (x1 - N[j].Key) + (y1 - N[j].Value) * (y1 - N[j].Value));
                    if (walking <= R)
                    {
                        double divide = 5;
                        neighbours[nodes].Add(new KeyValuePair<int, double>(j, (walking / divide) * 60));
                        neighbours[j].Add(new KeyValuePair<int, double>(nodes, (walking / divide) * 60));
                    }
                    if (walking1 <= R)
                    {
                        double divide = 5;
                        neighbours[nodes + 1].Add(new KeyValuePair<int, double>(j, (walking1 / divide) * 60));
                        neighbours[j].Add(new KeyValuePair<int, double>(nodes + 1, (walking1 / divide) * 60));
                        numbers++;
                    }

                }
                DeepWeeb_dijkstra(neighbours, nodes + 2, numbers, dic);
                // for (int k= 0; k <nodes+2;k++) neighbours[k].Clear();
                //N.Clear();
                neighbours[nodes].Clear();
                neighbours[nodes + 1].Clear();
                //numbers = 0;


            }
            srqueries.Close();

        }
    }
}
