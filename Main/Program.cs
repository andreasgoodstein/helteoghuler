using System;

namespace Main
{
	class Program
	{
		static void Main(string[] args)
		{
			ConsoleKey userConsoleInput;

			do
			{
				Console.WriteLine(TextResources.Greeting);
				Console.WriteLine(TextResources.ToExit);
				userConsoleInput = Console.ReadKey().Key;
				Console.WriteLine("\n");

				if (userConsoleInput == ConsoleKey.D1)
				{
					Console.WriteLine("\n{0}\n", TextResources.TheCave);
				}
			}
			while (userConsoleInput != ConsoleKey.D0);

			Console.WriteLine("\n{0}\n", TextResources.TheInn);
			Console.WriteLine("\n{0}", TextResources.Goodbye);
			Console.ReadKey();
		}
	}
}
