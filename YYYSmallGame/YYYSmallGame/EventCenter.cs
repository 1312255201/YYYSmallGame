using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YYYSmallGame.Function;

namespace YYYSmallGame
{
    public class EventCenter
    {
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        public static Dictionary<string,int> point = new Dictionary<string,int>();

        public static void OnWaitingForPlayer()
        {
            CreateMap.Create();
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            Round.IsLobbyLocked = true;
            Round.IsLocked = true;
            Coroutines.Add(Timing.RunCoroutine(WelcomeToGame()));
        }
        private static IEnumerator<float> WelcomeToGame()
        {
            int i = 0;
            while(!Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                
                foreach(Player player in Player.List)
                {
                    player.ShowHint("欢迎来到 嘤嘤嘤服务器 小游戏合集服务器 当前人数:" + Player.List.Count().ToString()+"\n人数大于4时游戏会开启\n"+i.ToString()+"/40");
                }
                if(Player.List.Count()>=4)
                {
                    i++;
                    if(i>=40)
                    {
                        Round.Start();
                    }
                }
            }
        }
        public static IEnumerator<float> FristGameFinish(Player winner)
        {
            yield return Timing.WaitForSeconds(2f);
            try
            {
                foreach (Player player in Player.List)
                {
                    player.SetRole(RoleType.Tutorial);
                    player.ShowHint("首先恭喜" + winner.Nickname + "取得了游戏胜利\n不要气馁还有很多游戏 准备开始载入 第二个游戏");
                }
                if (point.ContainsKey(winner.UserId))
                {
                    point[winner.UserId] += 1;
                }
                else
                {
                    point.Add(winner.UserId, 1);
                }
            }
            catch
            {
                foreach (Player player in Player.List)
                {
                    player.SetRole(RoleType.Tutorial);
                    player.ShowHint("没人获得胜利?\n不要气馁还有很多游戏 准备开始载入 第二个游戏");
                }
            }

            FirstGame.UnReg();
            yield return Timing.WaitForSeconds(2f);
            SecondGame.Reg();
        }
        public static IEnumerator<float> SecondGameFinish(List<Player> players)
        {
            yield return Timing.WaitForSeconds(2f);
            foreach (Player player in Player.List)
            {
                player.SetRole(RoleType.Tutorial);
                player.ShowHint("首先恭喜有"+ players.Count()+ "人 取得了游戏胜利\n不要气馁还有很多游戏 准备开始载入 第三个游戏");
            }
            foreach(Player winner in players)
            {
                if (point.ContainsKey(winner.UserId))
                {
                    point[winner.UserId] += 1;
                }
                else
                {
                    point.Add(winner.UserId, 1);
                }
            }

            SecondGame.UnReg();
            yield return Timing.WaitForSeconds(2f);
            ThreeGame.Reg();
        }
        public static IEnumerator<float> ThreeGameFinish(List<Player> players)
        {
            yield return Timing.WaitForSeconds(2f);
            foreach (Player player in Player.List)
            {
                player.SetRole(RoleType.Tutorial);
                player.ShowHint("首先恭喜有" + players.Count() + "人 取得了游戏胜利\n不要气馁还有很多游戏 准备开始载入 第四个游戏");
            }
            foreach (Player winner in players)
            {
                if (point.ContainsKey(winner.UserId))
                {
                    point[winner.UserId] += 1;
                }
                else
                {
                    point.Add(winner.UserId, 1);
                }
            }

            ThreeGame.UnReg();
            yield return Timing.WaitForSeconds(2f);
            Map.ShowHint("目前本服还是个 测试版 希望你喜欢这种体验 欢迎加群反馈bug 回合结束 后续游戏正在开发jpg", 5);
            yield return Timing.WaitForSeconds(5f);
            Round.EndRound(true);
            Round.Restart(false);
        }
        private static IEnumerator<float> ReadyToPlay()
        {
            yield return Timing.WaitForSeconds(1f);
            foreach(Player player in Player.List)
            {
                player.SetRole(RoleType.Tutorial);
                player.ShowHint("请稍后服务器正在准备游戏");          
            }
            yield return Timing.WaitForSeconds(3f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("开始清理服务器全部掉落物");
            }
            foreach (Exiled.API.Features.Items.Pickup item in Map.Pickups)
            {
                item.Destroy();
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("完成");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("开始准备游戏地图");
            }
            FirstGame.Reg();
        }

        public static void Onroundstart()
        {
            Timing.RunCoroutine(ReadyToPlay());
        }
        public static void OnRestartingRound()
        {
            if (Server.Port == 7779)
            {
                Timing.CallDelayed(0.5f, () => {
                    var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
                    EndPoint point = new IPEndPoint(ipaddress, 12342);
                    tcpClient.Connect(point);
                    tcpClient.Send(Encoding.UTF8.GetBytes("7779"));
                });
            }
        }
        public static void OnRoundender(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutineHandle in Coroutines)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            point.Clear();
        }
        public static void OnJoin(VerifiedEventArgs ev)
        {
            if(!Round.IsStarted)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.SetRole(RoleType.Tutorial);
                });
            }

        }
        public static void Reg()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayer;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundender;
            Exiled.Events.Handlers.Server.RoundStarted += Onroundstart;
            Exiled.Events.Handlers.Player.Verified += OnJoin;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;

        }
        public static void UnReg()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayer;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundender;
            Exiled.Events.Handlers.Server.RoundStarted -= Onroundstart;
            Exiled.Events.Handlers.Player.Verified -= OnJoin;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;

        }
    }
}
