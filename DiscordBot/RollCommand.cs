﻿using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
	internal class RollCommand
	{
		private string _message;

		public string Roll(string parameters)
		{
			_message = "";
			string errorMessage = "There was an error, try the format: !roll 5d3 -3";

			string[] tokens = parameters.Split(' ');

			int total = 0;

			try
			{
				foreach(var token in tokens)
				{
					total += GetValue(token);
					_message += $"\n\n**Total: {total}**";
				}
			}
			catch(Exception e)
			{
				_message = errorMessage;
			}

			return _message;
		}

		private int GetValue(string roll)
		{
			int result = 0;

			if(int.TryParse(roll, out result))
			{
				string sign = result > 1 ? "+" : "";
				_message += $" {sign}{result}";
			}
			else
			{
				if(roll.Contains("d") || roll.Contains("D"))
				{
					result += GetRoll(roll);
				}
			}

			return result;
		}

		private int GetRoll(string roll)
		{
			char[] diceChars = new char[] { 'd', 'D' };
			string[] numbers = roll.Split(diceChars);

			int mult;
			int sides;

			if(numbers.Length != 2)
			{
				throw new Exception("Incorrect parameters.");
			}

			if(!int.TryParse(numbers[0], out mult))
			{
				throw new Exception("Incorrect parameters.");
			}

			if(!int.TryParse(numbers[1], out sides))
			{
				throw new Exception("Incorrect parameters.");
			}

			int output = 0;

			_message += $" {roll}[";
			Random rand = new Random();
			while(mult > 0)
			{
				int rollResult = rand.Next(1, sides + 1);

				_message += rollResult;
				if(mult > 1)
				{
					_message += ", ";
				}

				output += rollResult;
				mult--;
			}
			_message += "]";

			return output;
		}
	}
}
