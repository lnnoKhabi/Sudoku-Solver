
namespace SudokuSolver
{
	public class Solver
	{
		static void Main(string[] args)
		{
			Solve();
			Console.ReadLine();
		}

		static void Solve()
		{

			int[,] Sudoku = {   {5,3,0,0,7,0,0,0,0 },
								{6,0,0,1,9,5,0,0,0 },
								{0,9,8,0,0,0,0,6,0 },
								{8,0,0,0,6,0,0,0,3 },
								{4,0,0,8,0,3,0,0,1 },
								{7,0,0,0,2,0,0,0,6 },
								{0,6,0,0,0,0,2,8,0 },
								{0,0,0,4,1,9,0,0,5 },
								{0,0,0,0,8,0,0,7,9 }};

			int[,] SudokuCopy = new int[,] {{5,3,0,0,7,0,0,0,0 },
											{6,0,0,1,9,5,0,0,0 },
											{0,9,8,0,0,0,0,6,0 },
											{8,0,0,0,6,0,0,0,3 },
											{4,0,0,8,0,3,0,0,1 },
											{7,0,0,0,2,0,0,0,6 },
											{0,6,0,0,0,0,2,8,0 },
											{0,0,0,4,1,9,0,0,5 },	
											{0,0,0,0,8,0,0,7,9 }};

			Console.WriteLine("Puzzle\t\t\t\tSolution");
			Console.WriteLine();
			//PrintSudoku(Sudoku);

			//boxes coordinates [(x,y) ==> starting point for (row , column)]
			Vec2 b0 = new Vec2(0,0);
			Vec2 b1 = new Vec2(0,3);
			Vec2 b2 = new Vec2(0,6);
			Vec2 b3 = new Vec2(3,0);
			Vec2 b4 = new Vec2(3,3);
			Vec2 b5 = new Vec2(3,6);
			Vec2 b6 = new Vec2(6,0);
			Vec2 b7 = new Vec2(6,3);
			Vec2 b8 = new Vec2(6,6);

			//corresponding boxes
			Vec2[] adj_b0 = { b1, b2, b3, b6 };
			Vec2[] adj_b1 = { b0, b2, b4, b7 };
			Vec2[] adj_b2 = { b0, b1, b5, b8 };
			Vec2[] adj_b3 = { b0, b4, b5, b6 };
			Vec2[] adj_b4 = { b1, b3, b5, b7 };
			Vec2[] adj_b5 = { b2, b3, b4, b8 };
			Vec2[] adj_b6 = { b0, b3, b7, b8 };
			Vec2[] adj_b7 = { b1, b4, b6, b8 };
			Vec2[] adj_b8 = { b2, b5, b6, b7 };

			//map square to its corresponding boxes
			Vec2[][] map = { adj_b0, adj_b1, adj_b2, adj_b3, adj_b4, adj_b5, adj_b6, adj_b7, adj_b8 };

			//sum of all numbers is equal to 405 for solved Sudoku
			int sum = 0;//for quitting while loop
			for (int r0 = 0; r0 < 9; r0++)//rows
			{
				for (int c0 = 0; c0 < 9; c0++)//colunms
				{
					sum += Sudoku[r0, c0];
				}
			}

			while (sum != 405)//while its not solved
			{
				for (int r = 0; r < 9; r++)//rows
				{
					for (int c = 0; c < 9; c++)//colunms
					{
						int square = Sudoku[r, c];//current 1x1 square
						int box = GetBox(r, c);//current 3x3 box

						if (square > 0)//if square is solved
						{
							//look for the number in adjacent boxes
							Vec2[] adjacent = map[box];//array of all adjacent boxes
							for (int a = 0; a < adjacent.Length; a++)
							{
								Vec2 bx = adjacent[a];
								bool foundInSquare = false;
								for (int r2 = (int)bx.X; r2 < bx.X + 3; r2++)
								{
									for (int c2 = (int)bx.Y; c2 < bx.Y + 3; c2++)
									{
										if (square == Sudoku[r2, c2])
										{
											foundInSquare = true;
											goto nxt;
										}
									}
								}
							nxt:
								if (!foundInSquare)//if this box does not have the number
								{
									int possible = 0;
									Vec2 correct = new Vec2(-1, -1);

									//brute force number in all squares of current box
									for (int r2 = (int)bx.X; r2 < bx.X + 3; r2++)
									{
										for (int c2 = (int)bx.Y; c2 < bx.Y + 3; c2++)
										{
											if (Sudoku[r2, c2] == 0)//try number in the current square
											{
												int foundCount = 0;
												//check horizontal
												for (int row = r2; row < r2 + 1; row++)
												{
													for (int col = 0; col < 9; col++)
													{
														if (Sudoku[row, col] == square)//fails
														{
															foundCount++;
														}
													}
												}
												//check vertical
												for (int row = 0; row < 9; row++)
												{
													for (int col = c2; col < c2 + 1; col++)
													{
														if (Sudoku[row, col] == square)//fails
														{
															foundCount++;
														}
													}
												}

												//if eligible
												if (foundCount == 0)
												{
													possible++;
													correct.X = r2;
													correct.Y = c2;
												}
											}
										}
									}
									//if number can be placed in exactly 1 square of current box
									if (possible == 1)
									{
										Sudoku[(int)correct.X, (int)correct.Y] = square;//update Sudoku
										sum += square;//add to total sum
									}
								}
							}
						}
					}
				}
			}
			PrintSudoku(Sudoku,SudokuCopy);
		}

		private static int GetBox(int row, int colunm)
		{
			if(row >= 0 && row <= 2 && colunm >= 0 && colunm <= 2)
			{
				return 0;
			}
			else if (row >= 0 && row <= 2 && colunm >= 3 && colunm <= 5)
			{
				return 1;
			}
			else if (row >= 0 && row <= 2 && colunm >= 6 && colunm <= 8)
			{
				return 2;
			}
			else if (row >= 3 && row <= 5 && colunm >= 0 && colunm <= 2)
			{
				return 3;
			}
			else if (row >= 3 && row <= 5 && colunm >= 3 && colunm <= 5)
			{
				return 4;
			}
			else if (row >= 3 && row <= 5 && colunm >= 6 && colunm <= 8)
			{
				return 5;
			}
			else if (row >= 6 && row <= 8 && colunm >= 0 && colunm <= 2)
			{
				return 6;
			}
			else if (row >= 6 && row <= 8 && colunm >= 3 && colunm <= 5)
			{
				return 7;
			}
			else if (row >= 6 && row <= 8 && colunm >= 6 && colunm <= 8)
			{
				return 8;
			}
			return -1;
		}
		
		static void PrintSudoku(int[,] Sudoku, int[,] SudokuCopy)
		{
			for (int r = 0; r < 9; r++)
			{

				//for loop below prints the unsolved puzzle
				for (int c2 = 0; c2 < 9; c2++)//column
				{
					if (c2 == 2 || c2 == 5)//draw number & vertical lines
					{
						Console.Write((SudokuCopy[r, c2] == 0 ? " " : SudokuCopy[r, c2]) + "|");
					}
					else if (c2 == 8)//draw just number
					{
						if(r != 4)
							Console.Write(SudokuCopy[r, c2] == 0 ? " \t\t" : SudokuCopy[r, c2] + "\t\t");
						else Console.Write(SudokuCopy[r, c2] == 0 ? " \t=>\t" : SudokuCopy[r, c2] + "\t=>\t");
					}
					else//draw number with space
					{
						Console.Write((SudokuCopy[r, c2] == 0 ? " " : SudokuCopy[r, c2]) + " ");
					}
				}

				//for loop below prints the solved puzzle
				for (int c = 0; c < 9; c++)
				{
					if (c == 2 || c == 5)
					{
						if (Sudoku[r, c] != SudokuCopy[r, c])//number was one of solved
						{
							Console.ForegroundColor = ConsoleColor.Green;//highlight solved number
						}
						Console.Write(Sudoku[r, c] );
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write("|");
					}

					else if (c == 8)
					{
						if (Sudoku[r, c] != SudokuCopy[r, c])//number was one of solved
						{
							Console.ForegroundColor = ConsoleColor.Green;//highlight solved number
						}
						Console.WriteLine(Sudoku[r, c]);
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						if (Sudoku[r, c] != SudokuCopy[r, c])//number was one of solved
						{
							Console.ForegroundColor = ConsoleColor.Green;//highlight solved number
						}
						Console.Write(Sudoku[r, c]);
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write(" ");
					}
				}

				if (r == 2 || r == 5)//draw horizontal lines
				{
					Console.Write("-----|-----|-----");//for unsolved
					Console.WriteLine("\t\t-----|-----|-----");//for solved
				}
			}
		}
	}

	//vector structure with two values x,y
	struct Vec2
	{
		float x = 0;
		float y = 0;

		public float X 
		{ 
			get { return x; }
			set { x = value; }
		}
		public float Y 
		{ 
			get { return y; }
			set { y = value; }
		}

		public Vec2(float xvalue, float yvalue)
		{
			X = xvalue;
			Y = yvalue;
		}
	}
}