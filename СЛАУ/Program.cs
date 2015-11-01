using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Нахождение_решений_СЛАУ
{
	class Program
	{
		public static int[,] GetMinor(int[,] matrix, int row, int col)
		{
			int[,] buf = new int[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if ((i != row) || (j != col))
					{
						if (i > row && j < col) buf[i - 1, j] = matrix[i, j];
						if (i < row && j > col) buf[i, j - 1] = matrix[i, j];
						if (i > row && j > col) buf[i - 1, j - 1] = matrix[i, j];
						if (i < row && j < col) buf[i, j] = matrix[i, j];
					}
				}
			return buf;
		}

		public static int Determ(int[,] matrix)
		{
			double det = 0;
			int Rank = matrix.GetLength(0);
			if (Rank == 1) det = matrix[0, 0];
			if (Rank == 2) det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
			if (Rank > 2)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					
					det += Math.Pow(-1, 0 + j) * matrix[0, j] * Determ(GetMinor(matrix, 0, j));
				}
			}
			return (int)det;
		}

		public static void print_1(int[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
				Console.Write(arr[i] + "  ");
			Console.WriteLine();
		}

		public static void print_2(int[,] arr)
		{
			for (int i = 0; i < arr.GetLength(0); i++)
			{
				for (int j = 0; j < arr.GetLength(1); j++)
					Console.Write(arr[i, j] + "  ");
				Console.WriteLine();
			}
		}

		public static void print_slau(int[,] a, int[] с)
		{
			int k;
			for (int i = 0; i < a.GetLength(0); i++)
			{
				k = 1;
				for (int j = 0; j < a.GetLength(1); j++)
				{
					if (j == 0)
					{
						if (a[i, j] > 0)
							Console.Write("{0:d}*x{1:d} ", a[i, j], k);
						if (a[i, j] < 0)
							Console.Write("- {0:d}*x{1:d} ", Math.Abs(a[i, j]), k);
					}
					else
					{
						if (a[i, j] > 0)
							Console.Write("+ {0:d}*x{1:d} ", a[i, j], k);
						if (a[i, j] < 0)
							Console.Write("- {0:d}*x{1:d} ", Math.Abs(a[i, j]), k);
					}
					k++;
				}
				Console.Write("= " + с[i]);
				Console.WriteLine();
			}
		}

		public static int[,] getNewMatrix(int[,] matrix, int[] svobChleni, int col)
		{
			int[,] newMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
			for (int i = 0; i < matrix.GetLength(0); i++)
				for (int j = 0; j < matrix.GetLength(1); j++)
					newMatrix[i, j] = matrix[i, j];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				newMatrix[i, col] = svobChleni[i];
			}
			return newMatrix;
		}

		public static void findAllOprs(int[,] matrix, int[] svobChleni, int opr, int[] oprs)
		{
			oprs[0] = opr;
			int[,] newMatrix;
			for (int i = 1; i < oprs.Length; i++)
			{
				newMatrix = getNewMatrix(matrix, svobChleni, i-1);
				oprs[i] = Determ(newMatrix);
			}
		}

		public static void print_answer(int[] oprs)
		{
			for (int i = 1; i < oprs.Length; i++)
			{
				int x = oprs[i]/oprs[0];
				Console.WriteLine("x" + i.ToString() + " = " + x);
			}
		}

		static void Main(string[] args)
		{
			StreamReader sr = new StreamReader("input.txt");
			int n = int.Parse(sr.ReadLine());
			int[,] matrix = new int[n, n];
			int[] svobChleni = new int[n];
			int[] oprs = new int[n + 1];
			String tmp;
			String[] temp;
			try
			{
				for (int i = 0; i < n; i++)
				{
					tmp = sr.ReadLine();
					temp = tmp.Split();
					for (int j = 0; j < n; j++)
					{
						matrix[i, j] = int.Parse(temp[j]);
					}
				}
				for (int i = 0; i < n; i++)
				{
					svobChleni[i] = int.Parse(sr.ReadLine());
				}
				sr.Close();
				print_slau(matrix, svobChleni);
				Console.WriteLine();
				int opr = Determ(matrix);
				findAllOprs(matrix, svobChleni, opr, oprs);
				if (opr == 0)
				{
                    int buf = 0;
                    for (int i = 1; i < oprs.Length; i++)
                    {
                        if (oprs[i] == 0)
                            buf++;
                    }
                    if (buf == oprs.Length-1)
                        Console.WriteLine("СЛАУ недоопределена");
                    else
                        Console.WriteLine("СЛАУ несовместна");
				}
				else
					print_answer(oprs);

			}
			catch (Exception) { Console.Write("Ошибка"); };
			Console.ReadKey();
		}
	}
}
