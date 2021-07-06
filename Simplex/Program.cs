using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public class Simplex
    {
        //source - симплекс таблица без переменных базиса
        double[,] tabl; //симплекс таблица
        int a, b;
        List<int> baz; //список переменных базис
        public Simplex(double[,] source)
        {
            a = source.GetLength(0);
            b = source.GetLength(1);
            tabl = new double[a, b + a - 1];
            baz = new List<int>();
            // Добавление переменных
            for (int z = 0; z < a; z++)
            {
                for (int y = 0; y < tabl.GetLength(1); y++)
                {
                    if (y < b)
                        tabl[z, y] = source[z, y];
                    else
                        tabl[z, y] = 0;
                }
                //Добавление коофецента 1 перед базис переменной, для верного добавления фиктивынх переменных
                if ((b + z) < tabl.GetLength(1))
                {
                    tabl[z, b + z] = 1;
                    baz.Add(b + z);
                }
            }
            b = tabl.GetLength(1);
        }
        //podschet - в данный массив будут записаны уже подсчитанные значения X
        public double[,] Podschet(double[] result)
        {
            int stolbec, stroka; //Столбец и строка с результатами
            while (!Ostanovka())
            {
                stolbec = RazreshaemiyStolbec();
                stroka = RazreshaemayaStroka(stolbec);
                baz[stroka] = stolbec;
                double[,] new_table = new double[a, b];
                for (int y = 0; y < b; y++)
                    new_table[stroka, y] = tabl[stroka, y] / tabl[stroka, stolbec];
                for (int z = 0; z < a; z++)
                {
                    if (z == stroka)
                        continue;
                    for (int y = 0; y < b; y++)
                        new_table[z, y] = tabl[z, y] - tabl[z, stolbec] * new_table[stroka, y];
                }
                tabl = new_table;
            }
            //занесение подсчитанных X в массив Podschet
            for (int z = 0; z < result.Length; z++)
            {
                int k = baz.IndexOf(z + 1);
                if (k != -1)
                    result[z] = tabl[k, 0];
                else
                    result[z] = 0;
            }
            return tabl;
        }

        private int RazreshaemiyStolbec()//Поиск разрешаемого столбца
        {
            int  stolb = 1;
            for (int y = 2; y < b; y++)
                if (tabl[a - 1, y] < tabl[a - 1, stolb])
                    stolb = y;
            Debug.WriteLine("Разрешаемый столбец: " + stolb);
            return stolb;
        }

        private bool Ostanovka() //Если строка оценок меньше 0, то происходит остановка программы
        {
            bool stop = true;
            for (int y = 1; y < b; y++)
            {
                if (tabl[a - 1, y] < 0)
                {
                    stop = false;
                    break;
                }
            }
            return stop;
        }


        private int RazreshaemayaStroka(int mainCol)//Поиск разрешаемой строки
        {
            int Stroka = 0;
            for (int z = 0; z < a - 1; z++)
                if (tabl[z, mainCol] > 0)
                {
                    Stroka = z;
                    break;
                }
            for (int z = Stroka + 1; z < a - 1; z++)
                if ((tabl[z, mainCol] > 0) && ((tabl[z, 0] / tabl[z, mainCol]) < (tabl[Stroka, 0] / tabl[Stroka, mainCol])))
                    Stroka = z;
            Debug.WriteLine("Разрешаемая строка: " + Stroka);
            return Stroka;
        }

        
      
    }
    public class VvodZnacheniy
    {
        public double[,] mas;
        public double[] bufMass = { };
        public double[,] table_rezultat;

        // Метод ввода и вывода данных
        

        class Program
        {
            static void Main(string[] args)
            {
                Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Промежуточные результаты.txt")));
                Debug.AutoFlush = true;
                VvodZnacheniy vz = new VvodZnacheniy();
                vz.simplex();
                Console.ReadKey();
            }
        }
        public void simplex()
        {
            double[] ms = { };
            string str = "";
            int raz = 0, c = 0;
            //Ввод данных массива из csv файла
            try
            {
                using (StreamReader sr = new StreamReader(@"Ввод.csv"))
                {
                    sr.ReadLine();
                    str = sr.ReadToEnd();
                    string[] st = str.Split('\n');
                    raz = st.Length;
                    ms = Array.ConvertAll(st[0].Split(';'), double.Parse);
                    c = ms.Length;
                    mas = new double[raz, c];
                    for (int z = 0; z < raz; z++)
                    {
                        ms = Array.ConvertAll(st[z].Split(';'), double.Parse);
                        for (int y = 0; y < c; y++)
                        {
                            mas[z, y] = ms[y];

                        }
                    }
                    // Изменяем строку оценок на отрицательные значения для более корректной работы
                    for (int z = 0; z < raz; z++)
                    {
                        for (int y = 0; y < c; y++)
                        {
                            if (z == raz - 1)
                            {
                                mas[z, y] = mas[z, y] * (-1);
                            }
                        }
                    }

                    //Меняем первый и последний столбец местами для удобства ввода ограничений в csv файл
                    for (int z = 0; z < raz; z++)
                    {
                        for (int y = 0; y < c; y += c - 1)
                        {
                            double tmp = mas[z, y];
                            mas[z, y] = mas[z, c - 1];
                            mas[z, c - 1] = tmp;
                        }

                    }
                   
                    Console.WriteLine("Матрица в первоначальном виде");
                    for (int z = 0; z < raz; z++)
                    {
                        for (int y = 0; y < c; y++)
                        {
                            Console.Write($"{mas[z, y],5}");
                        }
                        Console.WriteLine();
                    }
                }
                //Делаем массив размерности в два раза больше, чем ранее введенный массив для фиктивных переменных
                double[] rezultat = new double[raz * 2];
                //Конструктор классов
                Simplex S = new Simplex(mas);
                //Основной метод
                table_rezultat = S.Podschet(rezultat);
                for (int z = 0; z < table_rezultat.GetLength(0); z++)
                {
                    for (int y = 0; y < table_rezultat.GetLength(1); y++)
                    {
                        if (z == raz - 1)
                        {
                            table_rezultat[z, y] = table_rezultat[z, y] * (-1);
                        }
                    }
                }
                // Вывод решенной матрицы в консоли, и перенос в cvs файл
                Console.WriteLine("\n Выполненное решение задачи:");
                for (int z = 0; z < table_rezultat.GetLength(0); z++)
                {
                    for (int y = 0; y < table_rezultat.GetLength(1); y++)
                        Console.Write($"|{Math.Round(table_rezultat[z, y]),5}" + "|");
                    
                    Console.WriteLine("");
                }
                int ind = 1;
                for (int y = c - 2; y >= 0; y--)

                    //Вывод X1, X2 в консоли
                {
                    Console.WriteLine("\n X[{0}] = {1} ", ind, rezultat[y]);
                    ind++;
                }
                //Вывод F в консоль
                Console.WriteLine("\n F = " + (table_rezultat[table_rezultat.GetLength(0) - 1, 0] * -1));
                //Вывод F' в консоль
                Console.WriteLine("\n F' = " + (table_rezultat[table_rezultat.GetLength(0) - 1, 0]));
                using (StreamWriter sw = new StreamWriter(@"Вывод.csv"))
                {
                    sw.WriteLine("Решение:");
                    for (int z = 0; z < table_rezultat.GetLength(0); z++)
                    {
                        for (int y = 0; y < table_rezultat.GetLength(1); y++)
                            sw.Write($"|{Math.Round(table_rezultat[z, y]),3}" + "|");
                        sw.WriteLine("");
                    }
                    ind = 1;
                    for (int y = c - 2; y >= 0; y--)
                        //Вывод Х1, Х2 в cvs файл
                    {
                        sw.WriteLine("\nX[{0}] = {1}", ind, rezultat[y]);
                        ind++;
                    }
                    // Вывод F в cvs файл
                    sw.WriteLine("\nF = " + (table_rezultat[table_rezultat.GetLength(0) - 1, 0] * -1));
                    // Вывод F' в cvs файл
                    sw.WriteLine("\nF' = " + (table_rezultat[table_rezultat.GetLength(0) - 1, 0]));
                }
            }
            //Вывод ошибки если cvs файл заполнен некорректно
            catch
            {
                
                Console.WriteLine("Ошибка в cvs файле, измените данные");
            }
        }
    }
    
}
