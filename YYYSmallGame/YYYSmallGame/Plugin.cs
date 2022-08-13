using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YYYSmallGame.Function;

namespace YYYSmallGame
{
	public class Plugin : Plugin<YYYPluginConfig>
	{
		public static Plugin Instance;
		private Harmony hInstance;

		public int PatchesCounter { get; private set; }
		public Harmony Harmony { get; private set; }
		public override void OnEnabled()
		{
			Instance = this;

			base.OnEnabled();
			RegisterEvents();
			Log.Info("Exiled3.0插件读取完毕");
		}

		private void RegisterEvents()
		{
			if(Server.Port == 7779)
            {
				EventCenter.Reg();
				HintMainClass.Register();
			}

			try
			{
				hInstance = new Harmony($"com.gugufish.yyyserver-{DateTime.UtcNow.Ticks}");
				hInstance.PatchAll();
			}
			catch (Exception e)
			{
				Log.Error($"Patching failed!, " + e);
			}
		}


		public override void OnDisabled()
		{
			base.OnDisabled();
			UnregisterEvents();
		}

		private void UnregisterEvents()
		{
			EventCenter.UnReg();
			HintMainClass.Unregister();

			hInstance.UnpatchAll();
			hInstance = null;
			Log.Info("嘤嘤嘤服务器专用插件卸载完毕");
			Harmony.UnpatchAll();
		}
	}
}
