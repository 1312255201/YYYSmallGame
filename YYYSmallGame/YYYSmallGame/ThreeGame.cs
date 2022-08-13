using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YYYSmallGame.Function;

namespace YYYSmallGame
{
    public class ThreeGame
    {
        public static bool secondgameisrun;
        
        public static List<AdminToys.PrimitiveObjectToy> primitiveObjectToys = new List<AdminToys.PrimitiveObjectToy>();
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        
        public static void Ready()
        {
            Timing.RunCoroutine(CreateMapTiming());
        }
        private static List<T> RandomSort<T>(List<T> list)
        {
            var random = new System.Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }
        public static void OnuseingItem(UsingItemEventArgs ev)
        {
        }

        public static void OnChangingItem(ChangingItemEventArgs ev)
        {
        }
        public static Vector3 GetRamdomPos()
        {
            int x = new System.Random().Next(1, 29);
            int z = new System.Random(Environment.TickCount+11).Next(-107,-78);
            return (new Vector3(x, 1031, z));
        }
        private static IEnumerator<float> Boomtest()
        {
            yield return Timing.WaitForSeconds(1f);
            for(int i = 0; i < 10; i++)
            {
                yield return Timing.WaitForSeconds(0.1f);
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 1f;
                grenade.SpawnActive(GetRamdomPos()+Vector3.up, null);
            }

        }
        private static IEnumerator<float> CreateMapTiming()
        {
            int i2 = 0;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    i2++;
                    if(i2 == 10)
                    {
                        yield return Timing.WaitForSeconds(0.01f);
                        i2 = 0;
                    }
                    primitiveObjectToys.Add(CreateMap.CreateCubeAPI(new Vector3(i, 1027, -108 + j), Color.white, new Vector3(1, 1, 1), PrimitiveType.Cube));
                }
                foreach (Player player in Player.List)
                {
                    player.ShowHint(i +"/30", 3);
                }

            }
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n落雷 游戏规则：天上会掉下手雷 会刷新一些安全地点！",10);
                player.Position = new Vector3(15, 1028, -93);
            }
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n天上会掉下手雷 会刷新一些安全地点！", 10);
                player.Position = new Vector3(15, 1028, -93);
            }
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(15, 1028, -93);
                player.ShowHint("3");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(15, 1028, -93);
                player.ShowHint("2");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(15, 1028, -93);
                player.ShowHint("1");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏开始");
            }
            secondgameisrun = true;
            int round = 0;
            List<AdminToys.PrimitiveObjectToy> awa = new List<AdminToys.PrimitiveObjectToy>();
            List<Vector3> safe = new List<Vector3>();
            while (secondgameisrun)
            {
                round++;

                safe.Add(GetRamdomPos());
                yield return Timing.WaitForSeconds(0.01f);
                safe.Add(GetRamdomPos());
                yield return Timing.WaitForSeconds(0.01f);
                if (round <= 3)
                {
                    safe.Add(GetRamdomPos());
                }
                yield return Timing.WaitForSeconds(0.01f);
                if(round <=5)
                {
                    safe.Add(GetRamdomPos());
                }
                yield return Timing.WaitForSeconds(0.01f);
                if (round <= 10)
                {
                    safe.Add(GetRamdomPos());
                }
                foreach (Vector3 vector in safe)
                {
                    awa.Add(CreateMap.CreateCubeAPI(vector, Color.black, new Vector3(1, 1, 1), PrimitiveType.Cube));
                }
                Map.ShowHint("刷了一些安全位置快躲好",15);

                yield return Timing.WaitForSeconds(15-round);
                Timing.RunCoroutine(Boomtest());
                Map.ShowHint("快躲好!!!!",3);
                yield return Timing.WaitForSeconds(3f);

                foreach (Player player1 in Player.Get(RoleType.Tutorial))
                {
                    bool safeyes = false;
                    Vector3 vectorply = new Vector3(player1.Position.x, 1031, player1.Position.z);
                    foreach (Vector3 vector in safe)
                    {
                        if(Vector3.Distance(vector,vectorply )<=1.3)
                        {
                            safeyes = true;
                        }
                    }
                    if(safeyes ==false)
                    {
                        player1.Kill("淘汰");
                    }
                }
                foreach(AdminToys.PrimitiveObjectToy primitiveObject in awa)
                {
                    NetworkServer.Destroy(primitiveObject.gameObject);
                }
                awa.Clear();
                safe.Clear();
                if (Player.Get(RoleType.Tutorial).Count() <=1)
                {
                    foreach(Player player in Player.List)
                    {
                        player.ShowHint("游戏结束,正在和插件数据中心 交换数据");
                    }
                    try 
                    {
                        Timing.RunCoroutine(EventCenter.ThreeGameFinish(Player.Get(RoleType.Tutorial).ToList()));
                    }
                    catch
                    {
                        Log.Info("错误");
                        Timing.RunCoroutine( EventCenter.ThreeGameFinish(new List<Player>()));
                    }
                    secondgameisrun = false;
                    break;
                }
                if(round >= 12)
                {
                    foreach (Player player in Player.List)
                    {
                        player.ShowHint("游戏结束,正在和插件数据中心 交换数据");
                    }
                    try
                    {
                        Timing.RunCoroutine(EventCenter.ThreeGameFinish(Player.Get(RoleType.Tutorial).ToList()));
                    }
                    catch
                    {
                        Log.Info("错误");
                        Timing.RunCoroutine(EventCenter.ThreeGameFinish(new List<Player>()));
                    }
                    secondgameisrun = false;
                    break;
                }
            }

        }
        public static void OnPlayerHurt(HurtingEventArgs ev)
        {
            ev.Amount = 0.0000000000001f;
        }
        public static void Reg()
        {
            Ready();
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            Exiled.Events.Handlers.Player.UsingItem += OnuseingItem;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurt;

        }
        public static void UnReg()
        {
            foreach(AdminToys.PrimitiveObjectToy primitiveObjectToy in primitiveObjectToys)
            {
                NetworkServer.Destroy(primitiveObjectToy.gameObject);
            }
            primitiveObjectToys.Clear();
            foreach (CoroutineHandle coroutineHandle in Coroutines)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            Coroutines.Clear();
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
            Exiled.Events.Handlers.Player.UsingItem -= OnuseingItem;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurt;

        }
    }
}
