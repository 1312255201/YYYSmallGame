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
    public class SecondGame
    {
        public static bool secondgameisrun;
        
        private static Dictionary<int,ItemType> changecheck = new Dictionary<int,ItemType>();
        private static Dictionary<int, ItemType> usecheck = new Dictionary<int,ItemType>();
        public static List<AdminToys.PrimitiveObjectToy> primitiveObjectToys = new List<AdminToys.PrimitiveObjectToy>();
        public static List<int> finishplayer = new List<int>();
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        public static bool hurtjc;
        
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
            if (usecheck.ContainsKey(ev.Player.Id))
            {
                if (ev.Item.Type == usecheck[ev.Player.Id])
                {
                    usecheck.Remove(ev.Player.Id);
                    finishplayer.Add(ev.Player.Id);
                }
                else
                {
                    ev.Player.Kill("淘汰");
                }
            }
        }

        public static void OnChangingItem(ChangingItemEventArgs ev)
        {
            if(changecheck.ContainsKey(ev.Player.Id))
            {
                if(ev.NewItem.Type == changecheck[ev.Player.Id])
                {
                    changecheck.Remove(ev.Player.Id);
                    finishplayer.Add(ev.Player.Id);
                }
                else
                {
                    ev.Player.Kill("淘汰");
                }
            }
        }
        public static Vector3 GetRamdomPos()
        {
            int x = new System.Random().Next(1, 29);
            int z = new System.Random(Environment.TickCount + 11).Next(-107, -78);
            return (new Vector3(x, 1030, z));
        }
        private static IEnumerator<float> Movetest(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            Vector3 vector3 = player.Position;
            yield return Timing.WaitForSeconds(3f);
            if (Vector3.Distance(vector3,player.Position)<=0.3)
            {
                finishplayer.Add(player.Id);
            }
        }
        private static IEnumerator<float> Boomtest()
        {
            yield return Timing.WaitForSeconds(1f);
            for(int i = 0; i < 40; i++)
            {
                yield return Timing.WaitForSeconds(0.1f);
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 3f;
                grenade.SpawnActive(GetRamdomPos(), null);
            }
            yield return Timing.WaitForSeconds(4f);
            foreach(Player player in Player.Get(RoleType.Tutorial))
            {
                finishplayer.Add(player.Id);
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
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n游戏规则服务器将会告诉你指令 请根据指令完成动作 15s内未正确完成动作将会被淘汰！",10);
                player.Position = new Vector3(15, 1028, -93);
            }
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n游戏规则服务器将会告诉你指令 请根据指令完成动作 15s内未正确完成动作将会被淘汰！", 10);
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
            int wait = 15; 
            while (secondgameisrun)
            {
                yield return Timing.WaitForSeconds(3f);

                finishplayer.Clear();
                switch (new System.Random().Next(1, 11))
                {
                    case 1:
                        foreach(Player player in Player.List)
                        {
                            player.ShowHint("向上看",15);
                            Coroutines.Add(Timing.RunCoroutine(检测向上看(player)));
                        }
                        break;
                    case 2:
                        foreach (Player player in Player.List)
                        {
                            player.ShowHint("向上看", 15);
                            Coroutines.Add(Timing.RunCoroutine(检测向下看(player)));
                        }
                        break;
                    case 3:
                        List<ItemType> itemTypes = new List<ItemType>();
                        List<ItemType> itemTypes2 = new List<ItemType>();
                        itemTypes.Add(ItemType.GunE11SR);
                        itemTypes.Add(ItemType.GunFSP9);
                        itemTypes.Add(ItemType.GunAK);
                        itemTypes.Add(ItemType.Flashlight);
                        itemTypes.Add(ItemType.GrenadeFlash);
                        itemTypes.Add(ItemType.GunLogicer);
                        itemTypes.Add(ItemType.GunRevolver);
                        itemTypes.Add(ItemType.SCP500);
                        itemTypes2 = RandomSort(itemTypes);
                        foreach (Player player in Player.List)
                        {
                            player.ShowHint("掏出E11r步枪", 15);
                            player.ClearInventory();
                            foreach(ItemType itemType in itemTypes2)
                            {
                                player.AddItem(itemType);
                            }
                            changecheck.Add(player.Id,ItemType.GunE11SR);
                        }
                        break;
                    case 4:
                        List<ItemType> itemTypes3 = new List<ItemType>();
                        List<ItemType> itemTypes4 = new List<ItemType>();
                        itemTypes3.Add(ItemType.GunE11SR);
                        itemTypes3.Add(ItemType.GunFSP9);
                        itemTypes3.Add(ItemType.GunAK);
                        itemTypes3.Add(ItemType.Flashlight);
                        itemTypes3.Add(ItemType.GrenadeFlash);
                        itemTypes3.Add(ItemType.GunLogicer);
                        itemTypes3.Add(ItemType.GunRevolver);
                        itemTypes3.Add(ItemType.SCP500);
                        itemTypes4 = RandomSort(itemTypes3);
                        foreach (Player player in Player.List)
                        {
                            player.ShowHint("掏出手电筒", 15);
                            player.ClearInventory();
                            foreach (ItemType itemType in itemTypes4)
                            {
                                player.AddItem(itemType);
                            }
                            changecheck.Add(player.Id, ItemType.Flashlight);
                        }
                        break;
                    case 5:
                        List<ItemType> itemTypes5 = new List<ItemType>();
                        List<ItemType> itemTypes6 = new List<ItemType>();
                        itemTypes5.Add(ItemType.Medkit);
                        itemTypes5.Add(ItemType.Painkillers);
                        itemTypes5.Add(ItemType.SCP207);
                        itemTypes5.Add(ItemType.SCP330);
                        itemTypes5.Add(ItemType.SCP018);
                        itemTypes5.Add(ItemType.SCP500);
                        itemTypes6 = RandomSort(itemTypes5);
                        foreach (Player player in Player.List)
                        {
                            player.ShowHint("Oh不你中毒了 快使用 SCP500解毒", 15);
                            player.ClearInventory();
                            foreach (ItemType itemType in itemTypes6)
                            {
                                player.AddItem(itemType);
                            }
                            usecheck.Add(player.Id, ItemType.SCP500);
                        }
                        break;
                    case 6:
                        Map.ShowHint("跳下平台",15);
                        int waitold = wait;
                        wait = 20;
                        foreach(Player player in Player.List)
                        {
                            Coroutines.Add(Timing.RunCoroutine(检测跳下去(player)));
                        }
                        Timing.CallDelayed(10f, () => {
                            if(wait == 20)
                            {
                                wait = waitold;
                            }
                        });
                        break;
                    case 7:
                        Map.ShowHint("把血加满", 15);
                        int waitold2 = wait;
                        List<ItemType> itemTypes7 = new List<ItemType>();
                        List<ItemType> itemTypes8 = new List<ItemType>();
                        itemTypes7.Add(ItemType.Medkit);
                        itemTypes7.Add(ItemType.Painkillers);
                        itemTypes7.Add(ItemType.SCP207);
                        itemTypes7.Add(ItemType.Adrenaline);
                        itemTypes7.Add(ItemType.Adrenaline);
                        itemTypes7.Add(ItemType.Medkit);
                        itemTypes7.Add(ItemType.SCP330);
                        itemTypes7.Add(ItemType.SCP500);
                        itemTypes8 = RandomSort(itemTypes7);
                        wait = 14;
                        foreach (Player player in Player.List)
                        {
                            player.ClearInventory();
                            player.Health = 1;
                            foreach(ItemType itemType in itemTypes8)
                            {
                                player.AddItem(itemType);
                            }
                            Coroutines.Add(Timing.RunCoroutine(检测加血(player)));
                        }
                        Timing.CallDelayed(5f, () => {
                            if (wait == 14)
                            {
                                wait = waitold2;
                            }
                        });
                        break;
                    case 8:
                        Map.ShowHint("攻击别人", 15);
                        foreach(Player player in Player.List)
                        {
                            player.ClearInventory();
                            player.AddItem(ItemType.GunFSP9);
                        }
                        hurtjc = true;
                        break;
                    case 9:
                        Map.ShowHint("不要被手雷炸死", 15);
                        Coroutines.Add(Timing.RunCoroutine(Boomtest()));
                        break;
                    default:
                        Map.ShowHint("不要动", 10);
                        foreach (Player player in Player.List)
                        {
                            Coroutines.Add(Timing.RunCoroutine(Movetest(player)));

                        }
                        break;
                }
                yield return Timing.WaitForSeconds(wait);
                foreach (CoroutineHandle coroutineHandle in Coroutines)
                {
                    Timing.KillCoroutines(coroutineHandle);
                }
                usecheck.Clear();
                changecheck.Clear();
                Coroutines.Clear();
                hurtjc = false;
                foreach (Player player1 in Player.List)
                {
                    if(player1.Role == RoleType.Tutorial)
                    {
                        if(!finishplayer.Contains(player1.Id))
                        {
                            Log.Info("淘汰");
                            player1.Kill("淘汰");
                        }
                        else
                        {
                            try
                            {
                                for (int i = 0; i <= finishplayer.Count() - 1; i++)
                                {
                                    if (finishplayer[i] == player1.Id)
                                    {
                                        player1.ShowHint("恭喜你完成了 获得第" + (i + 1) + "名", 10);

                                    }
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                }
                finishplayer.Clear();
                if (Player.Get(RoleType.Tutorial).Count() <=2)
                {
                    foreach(Player player in Player.List)
                    {
                        player.ShowHint("游戏结束,正在和插件数据中心 交换数据");
                    }
                    try 
                    {
                        Timing.RunCoroutine(EventCenter.SecondGameFinish(Player.Get(RoleType.Tutorial).ToList()));
                    }
                    catch
                    {
                        Log.Info("错误");
                        Timing.RunCoroutine( EventCenter.SecondGameFinish(new List<Player>()));
                    }
                    secondgameisrun = false;
                    break;
                }
                round++;
                if (round == 5)
                {
                    foreach (Player player in Player.List)
                    {
                        wait = 10;
                        player.ShowHint("加大难度 请在10s内完成动作", 6);
                    }
                    yield return Timing.WaitForSeconds(5f);

                }
                if (round == 10)
                {
                    foreach (Player player in Player.List)
                    {
                        wait = 5;
                        player.ShowHint("加大难度 请在5s内完成动作", 6);
                    }
                    yield return Timing.WaitForSeconds(5f);

                }
                if (round == 15)
                {
                    foreach (Player player in Player.List)
                    {
                        player.ShowHint("游戏结束,正在和插件数据中心 交换数据");
                    }
                    EventCenter.SecondGameFinish(Player.Get(RoleType.Tutorial).ToList());
                    secondgameisrun = false;
                }
                foreach(Player player in Player.List)
                {
                    if (player.Position.y <= 1020)
                    {
                        finishplayer.Add(player.Id);
                        player.Position = new Vector3(15, 1028, -93);
                    }
                }

            }

        }
        private static IEnumerator<float> 检测加血(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                if (player.Health >=99)
                {
                    finishplayer.Add(player.Id);
                    break;
                }
            }
        }
        private static IEnumerator<float> 检测向上看(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                if (player.ReferenceHub.PlayerCameraReference.forward.y >= 0.8)
                {
                    finishplayer.Add(player.Id);
                    break;
                }
            }
        }
        private static IEnumerator<float> 检测跳下去(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                if (player.Position.y<= 1020)
                {
                    finishplayer.Add(player.Id);
                    player.Position = new Vector3(15, 1028, -93);
                    break;
                }
            }
        }
        private static IEnumerator<float> 检测向下看(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                if (player.ReferenceHub.PlayerCameraReference.forward.y <= -0.8)
                {
                    finishplayer.Add(player.Id);
                    break;
                }
            }
        }
        public static void OnPlayerHurt(HurtingEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(hurtjc)
                {
                    finishplayer.Add(ev.Attacker.Id);
                }
                ev.Amount = 0.0000000000001f;

            }
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
            finishplayer.Clear();
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
