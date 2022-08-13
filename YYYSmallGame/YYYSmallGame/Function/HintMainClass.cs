using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YYYSmallGame.Function
{
    public class awa
    {
        private Player player;
        private int playerid;
        private List<string> strings = new List<string>();

        public Player playerawa { get => player; set => player = value; }
        public int playeridawa { get => playerid; set => playerid = value; }
        public List<string> message { get => strings; set => strings = value; }
    }
    public class HintMainClass
    {
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        private static List<string> chatList = new List<string>();

        private static int scp492num = 0;
        private static int ntfnum;
        private static int chinum;
        public static string scp49hp = "未知";
        public static string scp173hp = "未知";
        public static string scp93953hp = "未知";
        public static string scp93989hp = "未知";
        public static string scp106hp = "未知";
        public static string scp096 = "未知";
        public static string scp079 = "Offline";
        public static List<awa> awas = new List<awa>();
        private static awa temp;
        public static bool showchat;

        public static void RemovePlayerInfo(Player player)
        {
            foreach (awa awa2 in awas)
            {
                if(awa2.playeridawa == player.Id)
                {
                    string temp = "";
                    foreach (string message in awa2.message)
                    {
                        if (message.IndexOf("玩家角色介绍") != -1)
                        {
                            temp = message;
                        }
                    }
                    try
                    {
                        awa2.message.Remove(temp);

                    }
                    catch
                    {

                    }
                }

            }
        }
        public static IEnumerator<float> YYYServerHint()
        {
            yield return Timing.WaitForSeconds(5f);

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                foreach (awa awaawa in awas)
                {
                    try
                    {
                        if (awaawa.message.Count >= 1)
                        {

                            string finnal = "<size=0>我是一般</size>";
                            foreach (string message in awaawa.message)
                            {

                                if (message.IndexOf("临时消息") != -1)
                                {
                                    finnal += message;
                                }
                            }
                            
                            if (awaawa.playerawa.HasHint)
                            {

                                if (awaawa.playerawa.HintDisplay.ToString().IndexOf("我是一般") != -1)
                                {
                                    awaawa.playerawa.ShowHint(finnal, 2);
                                }
                            }
                            else
                            {
                                awaawa.playerawa.ShowHint(finnal, 2);
                            }
                        }

                    }
                    catch
                    {

                    }
                }
            }
        }
        public static void AddTempHint(Player player, string thing,int time)
        {
            foreach (awa awa2 in awas)
            {
                if(awa2.playeridawa == player.Id)
                {
                    awa2.message.Add("\n<size=0>临时消息</size>\n"+thing);
                }

            }
            Timing.CallDelayed(time, () => {
                foreach (awa awa2 in awas)
                {
                    if(awa2.playeridawa == player.Id)
                    {
                        string temp = "";
                        foreach (string message in awa2.message)
                        {
                            if (message.IndexOf(thing) != -1)
                            {
                                temp = message;
                            }
                        }
                        try
                        {
                            awa2.message.Remove(temp);

                        }
                        catch
                        {

                        }
                    }


                }

            });
        }
        public static void OnVer(VerifiedEventArgs ev)
        {
            awa tempawa = new awa();
            tempawa.playerawa = ev.Player;
            tempawa.playeridawa = ev.Player.Id;
            awas.Add(tempawa);
        }
        public static void OnRoundStart()
        {
            Coroutines.Add(Timing.RunCoroutine(YYYServerHint()));
        }
        public static void OnRoundEnd(RoundEndedEventArgs ev)
        {
            showchat = false;
            chatList.Clear();
            foreach(awa awa2 in awas)
            {
                awa2.playerawa = null;
                awa2.playeridawa = 0;
                awa2.message.Clear();
            }
            awas.Clear();
            foreach (CoroutineHandle coroutineHandle in Coroutines)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            Coroutines.Clear();
        }
        public static void OnLeft(LeftEventArgs ev)
        {
            foreach(awa awa2 in awas)
            {
                if(awa2.playeridawa == ev.Player.Id)
                {
                    temp = awa2;
                }
            }
            awas.Remove(temp);
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnd;
            Exiled.Events.Handlers.Player.Verified += OnVer;
            Exiled.Events.Handlers.Player.Left += OnLeft;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnd;
            Exiled.Events.Handlers.Player.Verified -= OnVer;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
        }
    }
}
