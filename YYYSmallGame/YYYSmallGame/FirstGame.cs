using Exiled.API.Features;
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
    public class FirstGame
    {
        public static bool firstgameisrun;
        public static List<AdminToys.PrimitiveObjectToy> primitiveObjectToys = new List<AdminToys.PrimitiveObjectToy>();
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
        private static IEnumerator<float> CreateMapTiming()
        {
            int i2 = 0;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    i2++;
                    if(i2 == 10)
                    {
                        yield return Timing.WaitForSeconds(0.01f);
                        i2 = 0;
                    }
                    primitiveObjectToys.Add(CreateMap.CreateCubeAPI(new Vector3(i, 1027, -108 + j), Color.green, new Vector3(1, 1, 1), PrimitiveType.Cube));
                }
                foreach (Player player in Player.List)
                {
                    player.ShowHint(i +"/50", 3);
                }

            }
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n游戏规则你所在平台会随着时间推迟慢慢消失不要掉下去！",10);
                player.Position = new Vector3(25, 1028, -83);
            }
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏地图创建完毕,游戏准备开始\n游戏规则你所在平台会随着时间推迟慢慢消失不要掉下去！", 10);
                player.Position = new Vector3(25, 1028, -83);
            }
            yield return Timing.WaitForSeconds(5f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(25, 1028, -83);
                player.ShowHint("3");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(25, 1028, -83);
                player.ShowHint("2");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.Position = new Vector3(25, 1028, -83);
                player.ShowHint("1");
            }
            yield return Timing.WaitForSeconds(1f);
            foreach (Player player in Player.List)
            {
                player.ShowHint("游戏开始");
            }
            firstgameisrun = true;
            while (firstgameisrun)
            {
                yield return Timing.WaitForSeconds(1f);
                List<AdminToys.PrimitiveObjectToy> radom = new List<AdminToys.PrimitiveObjectToy>();
                radom = RandomSort(primitiveObjectToys);
                List<AdminToys.PrimitiveObjectToy> needdel = new List<AdminToys.PrimitiveObjectToy>();
                int i = 0;
                foreach (AdminToys.PrimitiveObjectToy primitiveObjectToy in radom)
                {
                    i++;
                    if (new System.Random().Next(1, 100) >= 60)
                    {
                        if (primitiveObjectToy.NetworkMaterialColor == Color.green)
                        {
                            primitiveObjectToy.NetworkMaterialColor = Color.yellow;
                        }
                        else if (primitiveObjectToy.NetworkMaterialColor == Color.yellow)
                        {
                            primitiveObjectToy.NetworkMaterialColor = Color.red;
                        }
                        else if (primitiveObjectToy.NetworkMaterialColor == Color.red)
                        {
                            needdel.Add(primitiveObjectToy);
                        }
                    }
                    if(i >= 10)
                    {
                        i = 0;
                        yield return Timing.WaitForSeconds(0.01f);
                    }
                }
                foreach(AdminToys.PrimitiveObjectToy primitiveObjectToy in needdel)
                {
                    NetworkServer.Destroy(primitiveObjectToy.gameObject);
                    primitiveObjectToys.Remove(primitiveObjectToy);
                }
                foreach(Player player in Player.List)
                {
                    if(player.Position.y<=1020)
                    {
                        player.Kill("淘汰");
                        if(Player.Get(RoleType.Tutorial).Count() <= 1)
                        {
                            try
                            {
                                firstgameisrun = false;
                                foreach (Player player1 in Player.List)
                                {
                                    player1.ShowHint("<size=50>" + Player.Get(RoleType.Tutorial).ToList()[0].Nickname + "获取胜利</size>");
                                }

                            }
                            catch
                            {

                            }
                            yield return Timing.WaitForSeconds(2f);

                            foreach (Player player1 in Player.List)
                            {
                                player1.ShowHint("<size=50>请稍后 FirstGame结束 正在和主文件通讯</size>");
                            }
                            try
                            {
                                Timing.RunCoroutine(EventCenter.FristGameFinish(Player.Get(RoleType.Tutorial).ToList()[0]));

                            }
                            catch
                            {
                                Timing.RunCoroutine(EventCenter.FristGameFinish(null));
                            }

                        }
                    }
                }
            }
        }
        public static void Reg()
        {
            Ready();
        }
        public static void UnReg()
        {
            foreach(AdminToys.PrimitiveObjectToy primitiveObjectToy in primitiveObjectToys)
            {
                NetworkServer.Destroy(primitiveObjectToy.gameObject);
            }
            primitiveObjectToys.Clear();
        }
    }
}
