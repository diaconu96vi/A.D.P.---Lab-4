using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int finalsum = 0;
            using (new MPI.Environment(ref args))
            {
                Intracommunicator com = MPI.Communicator.world;


                int[] array = new int[100];
                int[] localArray = new int[10];
                int localsum = 0;
                int localsum1 = 0;
                

                for (int i =1;i<=100;i++)
                {
                    array[i - 1] = i;
                }

                if (com.Rank == 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        localsum1 += array[j];
                    }

                    for (int i = 1; i<10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            localArray[j]= array[j+10*i];
                        }

                        
                        com.Send<int[]>(localArray,i , 0);
                        finalsum += com.Receive<int>(i, 0);
                    }
                    finalsum += localsum1;
                    
                }
                else
                {
                    com.Receive(0, 0, out int[] localarr);
                    for (int j = 0; j < 10; j++)
                    {
                        localsum += localarr[j];
                    }
                    com.Send<int>(localsum, 0, 0);
                }

                com.Dispose();
               //     MPI.Communicator.world.Send(3, 0, 0);
            }

            Console.WriteLine("Suma finala este:" + finalsum);
        }
    }
}