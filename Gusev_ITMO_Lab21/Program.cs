using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Gusev_ITMO_Lab21
{
    class Program
    {
        static object zaglushka = new object();
        static bool firstStop = false; // Первый садовник окончил работу
        static bool secondStop = false; // Второй садовник окончил работу
        static char[,] garden;
        static int lengthOfGarden = 0;
        static int widthOfGarden = 0;
        static int iteration = 0;


        static void Main(string[] args)
        {
            bool inputNotOk;



            int consoleIteration=0;


            Console.WriteLine("Введите размеры сада. Длина сада не должна превышать 20. Ширина сада не должна превышать 30.");
            do
            {
                inputNotOk = false;
                try
                {
                    Console.Write("Длина сада: ");  //количество строк
                    lengthOfGarden = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    inputNotOk = true;
                }
                if (lengthOfGarden > 20)
                {
                    Console.WriteLine("Длина сада не должна превышать 20.");
                    inputNotOk = true;
                }
            } while (inputNotOk);

            do
            {
                inputNotOk = false;
                try
                {
                    Console.Write("Ширина сада: ");  //количество символов в строке
                    widthOfGarden = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    inputNotOk = true;
                }
                if (widthOfGarden > 30)
                {
                    Console.WriteLine("Ширина сада не должна превышать 30");
                    inputNotOk = true;
                }
            } while (inputNotOk);

            garden = new char[lengthOfGarden, widthOfGarden]; //создаем двумерный массив символов - сад

            garden[0, 0] = 'X';// задаем начальное положение садовника 1 (X) в саду

            for (int j = 1; j < widthOfGarden; j++)
            {
                garden[0, j] = '_';// задаем пустые значения для первого ряда в саду
            }

            for (int i = 1; i < lengthOfGarden-1; i++)
            {
                for (int j = 0; j < widthOfGarden; j++)
                {
                    garden[i, j] = '_';// задаем пустые значения для первого ряда в саду
                }
            }

            for (int i = 0; i < widthOfGarden - 1; i++)
            {
                garden[lengthOfGarden - 1, i] = '_';// задаем пустые значения для первого ряда в саду
            }

            garden[lengthOfGarden - 1, widthOfGarden - 1] = 'O';// задаем начальное положение садовника 2 (0) в саду

            for (int i = 0; i < lengthOfGarden; i++)
            {
                for (int j = 0; j < widthOfGarden; j++)
                {
                    Console.Write(garden[i, j]);
                }
                Console.WriteLine("");
            }
            
             ThreadStart threadStart1 = new ThreadStart(Gardener1);
             ThreadStart threadStart2 = new ThreadStart(Gardener2);

             Thread thread2 = new Thread(threadStart1); // первый поток - основной; второй - действия первого садовника
             Thread thread3 = new Thread(threadStart2); // третий поток - действия второго садовника

             thread2.Start();
             thread3.Start();


             iteration++; 
             do
             {
                 lock (zaglushka)
                 {
                     if (iteration == consoleIteration)
                         break;
                     consoleIteration = iteration;
                     Console.WriteLine("Итерация {0}.", consoleIteration);
                     for (int i = 0; i < lengthOfGarden; i++)
                     {
                         for (int j = 0; j < widthOfGarden; j++)
                         {
                             Console.Write(garden[i, j]);
                         }
                         Console.WriteLine("");
                     }
                 }
             } while (!firstStop && !secondStop);
             
              Console.ReadKey();

         }

         static void Gardener1()
         {
             for (int i = 0; i < lengthOfGarden; i++)
             {
                    for (int j = 0; j < widthOfGarden; j++)
                    {
                        lock (zaglushka)
                        {
                            if ((garden[i, j] == 'o')||(garden[i, j] == 'O'))
                            {
                              break;   
                            }
                            garden[i, j] = 'x';
                            iteration++;
                        }
                    }
             }
             firstStop = true;
         }

         static void Gardener2()
         {
             for (int j = widthOfGarden-1; j > -1; j--)
             {
                     for (int i = lengthOfGarden - 1; i > -1; i--)
                     {
                         lock (zaglushka)
                         {
                            if ((garden[i, j] == 'x') || (garden[i, j] == 'X'))
                            {
                                 break;
                            }
                            garden[i, j] = 'o';
                            iteration++;
                         }
                     }
             }
             secondStop = true;
         } 
        
    }
}
