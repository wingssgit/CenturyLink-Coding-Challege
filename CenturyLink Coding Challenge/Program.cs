using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using Octokit;

namespace CenturyLink_Coding_Challenge
{
	class Program
	{
		private static Dictionary<string, GitUser> UserList = new Dictionary<string, GitUser>();
		
		static void Main(string[] args)
		{
			Login LoginForm = new Login();
			LoginForm.ShowDialog();

			var ghClient = new GitHubClient(new ProductHeaderValue("GitInfoApp"));
			ghClient.Credentials = new Credentials(G.Login, G.Password);


			tryagain:
			Console.WriteLine("Enter github username to begin:");
			var szName = Console.ReadLine();
			szName = Regex.Replace(szName, "[^a-zA-Z0-9-]", "");
			if (szName == "")
			{
				Console.WriteLine("Invalid username, try again.");
				goto tryagain;
			}

			Console.Clear();

			Console.WriteLine($"Retrieving {szName}'s followers");

			try
			{
				Stopwatch sw = new Stopwatch();
				sw.Start();

				GitUser gitUser = GetUserInfo(ghClient, szName, "[Root]").Result;

				if (gitUser.FollowCount == 0)
				{
					Console.WriteLine("");
					Console.WriteLine("The github user you entered has 0 followers and possibly 0 friends. Having 0 friends is sad. Try someone less depressing, please.");
					goto tryagain;
				}

				UserList.Add(gitUser.Username, gitUser);

				foreach (string username in gitUser.Followers)
				{
					if (String.IsNullOrEmpty(username))
						break;
					if (UserList.ContainsKey(username))
						continue;

					Spin();
					GitUser gitUser2 = GetUserInfo(ghClient, username, gitUser.Username).Result;
					UserList.Add(gitUser2.Username, gitUser2);

					foreach (string username2 in gitUser2.Followers)
					{
						if (String.IsNullOrEmpty(username2))
							break;
						if (UserList.ContainsKey(username2))
							continue;

						Spin();
						GitUser gitUser3 = GetUserInfo(ghClient, username2, gitUser2.Username).Result;
						UserList.Add(gitUser3.Username, gitUser3);
					}
				}
				sw.Stop();

				Console.WriteLine("");
				Console.WriteLine($"User List (retrieved in {sw.ElapsedMilliseconds}ms):");
				foreach (var user in UserList.Values)
				{
					PrintUserDetails(user);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("");
				Console.WriteLine($"An error occurred. Enter 1 to try again or 2 to view the exception.");
				string userInput = Console.ReadLine();
				Console.Clear();
				UserList.Clear();
				if (userInput == "1")
					goto tryagain;
				if (userInput == "2")
					Console.WriteLine(e);
			}

			Console.WriteLine($"");
			Console.WriteLine("Enter 1 to start over.");
			string input = Console.ReadLine();
			Console.Clear();
			UserList.Clear();
			if (input == "1")
				goto tryagain;
		}
		
		private static void PrintUserDetails(GitUser user)
		{
			Console.WriteLine($"---------------------------");
			Console.WriteLine($"User: {user.Username}");
			Console.WriteLine($"Parent: {user.Parent}");
			Console.WriteLine($"Follower Count: {user.FollowCount}");
			if (user.FollowCount > 0)
			{
				Console.Write($"Followers:");
				for (int i = 0; i < user.Followers.Length; i++)
				{
					if (String.IsNullOrEmpty(user.Followers[i]))
						break;
					Console.Write($" {user.Followers[i]}");
				}
				Console.WriteLine($"");
			}
		}

		private static int counter = 0;
		private static void Spin()
		{
			counter++;
			switch (counter % 4)
			{
				case 0: Console.Write(".   ");
					counter = 0;
					break;
				case 1: Console.Write("..  ");
					break;
				case 2: Console.Write("... ");
					break;
				case 3: Console.Write("....");
					break;
			}
			Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
		}
		private static async Task<GitUser> GetUserInfo(GitHubClient client, string username, string parent = null)
		{
			User ghUser = await client.User.Get(username);
			IReadOnlyList<User> followers = await client.User.Followers.GetAll(ghUser.Login);
			if (followers.Count == 0)
			{
				return new GitUser(username, followers.Count, parent);
			}
			string[] szFollowers = new string[5];
			int Index = 0;
			foreach (User follower in followers)
			{
				if (Index > 4 || Index > (followers.Count - 1))
					break;
				szFollowers[Index++] = follower.Login;
			}

			return new GitUser(username, szFollowers, followers.Count, parent);
		}
	}
}
