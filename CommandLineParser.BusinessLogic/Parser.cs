using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLineParser.BusinessLogic
{
	public class Parser
	{
		private const char KeyToken = '-';
		private const char Space = ' ';
		private const char Quote = '\"';

		public Dictionary<string, string> Arguments(string inputString)
		{
			var claims = SplitIntoClaims(inputString).ToList();

			var arguments = new Dictionary<string, string>();
			for (int i = 0; i < claims.Count; i++)
			{
				if (i + 1 < claims.Count)
				{
					if (claims[i].IsKey && claims[i + 1].IsValue)   //operation with argument
					{
						arguments.Add(claims[i].Body, claims[i+1].Body);
						i++;
						continue;
					}
				}

				if (claims[i].IsKey)                                //operation without argument
					arguments.Add(claims[i].Body, null);
				else
					throw new Exception("Argument should not be met without its operation name! Details: "
										+ $"isKey = {claims[i].IsKey}; "
										+ $"isValue = {claims[i].IsValue}; "
										+ $"body = {claims[i].Body}.");
			}
			return arguments;
		}

		private IEnumerable<Claim> SplitIntoClaims(string inputString)
		{
			for (int i = 0; i < inputString.Length; i++)
			{
				if (inputString[i] == Quote)
				{
					var claimBody = GetValueInQuotes(ref i, inputString);
					yield return new Claim { IsKey = false, IsValue = true, Body = claimBody };
					continue;
				}

				if (inputString[i] == KeyToken)
				{
					var claimBody = GetKey(inputString, ref i);
					yield return new Claim { IsKey = true, IsValue = false, Body = claimBody };
					continue;
				}

				if (!char.IsWhiteSpace(inputString[i]))
				{
					var claimBody = GetValue(inputString, ref i);
					yield return new Claim { IsKey = false, IsValue = true, Body = claimBody };
				}
			}
		}

		private static string GetValueInQuotes(ref int i, string inputString)
		{
			var nextQuote = inputString.IndexOf(Quote, i + 1);
			if (nextQuote < i)
				throw new Exception($"There is no closing double quote symbol after position {i}.");
			var claimBody = inputString.Substring(i + 1, nextQuote - i - 1);
			i = nextQuote;
			return claimBody;
		}

		private static string GetKey(string inputString, ref int i)
		{
			string claimBody;
			var keyEndsAt = inputString.IndexOf(Space, i);
			if (keyEndsAt < 0)
			{
				claimBody = inputString.Substring(i + 1);
				i = inputString.Length;
			}
			else
			{
				claimBody = inputString.Substring(i + 1, keyEndsAt - i - 1);
				i = keyEndsAt;
			}
			return claimBody;
		}

		private static string GetValue(string inputString, ref int i)
		{
			string claimBody;
			var valueEndsAt = inputString.IndexOf(Space, i);
			if (valueEndsAt < 0)
			{
				claimBody = inputString.Substring(i);
				i = inputString.Length;
			}
			else
			{
				claimBody = inputString.Substring(i, valueEndsAt - i);
				i = valueEndsAt;
			}
			return claimBody;
		}
	}

	internal class Claim
	{
		public bool IsKey { get; set; }
		public bool IsValue { get; set; }
		public string Body { get; set; }
	}
}
