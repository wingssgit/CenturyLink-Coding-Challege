using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenturyLink_Coding_Challenge
{
	public class GitUser
	{
		public string Username { get; protected set; }
		public string Parent { get; protected set; }
		public int FollowCount { get; protected set; }
		public string[] Followers { get; protected set; }
		public string[] Repos { get; protected set; }

		public GitUser(string username, string[] followers, string[] repos, int followCount = 0, string parent = null)
		{
			Username = username;
			Parent = parent;
			Followers = followers;
			Repos = repos;
			FollowCount = followCount;
		}
		public GitUser(string username, string[] followers, int followCount = 0, string parent = null)
		{
			Username = username;
			Parent = parent;
			Followers = followers;
			Repos = null;
			FollowCount = followCount;
		}
		public GitUser(string username, int followCount = 0, string parent = null)
		{
			Username = username;
			Parent = parent;
			Followers = null;
			Repos = null;
			FollowCount = followCount;
		}
	}

	/*public class GitRepo
	{
		public string Name;
		public string Owner;
		public string[] Stargazers;

		public GitRepo(string repoName, string owner, string[] stargazers)
		{
			Name = repoName;
			Owner = owner;
			Stargazers = stargazers;
		}

		public GitRepo(string repoName, string owner)
		{
			Name = repoName;
			Owner = owner;
			Stargazers = null;
		}
		
	}*/
}
