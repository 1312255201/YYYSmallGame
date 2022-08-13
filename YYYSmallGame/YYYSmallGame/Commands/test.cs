using AdminToys;
using CommandSystem;
using Exiled.API.Features;
using InventorySystem;
using MEC;
using Mirror;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YYYSmallGame.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class test : ICommand
	{
		private Player player;

		public string Command { get; } = "test";
		public string[] Aliases { get; } = new string[] { "test" };
		public string Description { get; } = "测试命令";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender)
			{
				var plr = sender as PlayerCommandSender;
				player = Player.Get(plr.PlayerId);
			}
			FirstGame.Ready();
			response = "执行完毕";
			return true;


		}
	}
}
