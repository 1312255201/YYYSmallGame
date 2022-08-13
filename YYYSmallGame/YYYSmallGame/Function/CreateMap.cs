using AdminToys;
using Exiled.API.Features;
using Footprinting;
using MEC;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YYYSmallGame.Function
{
    public class CreateMap 
    {
        public static GameObject awa;
        public static GameObject light;
        public static GameObject target;
        private static List<PrimitiveObjectToy> primitiveObjectToys = new List<PrimitiveObjectToy>();
        private static List<ShootingTarget> shootingTargets = new List<ShootingTarget>();
        private static List<PrimitiveObjectToy> chicken = new List<PrimitiveObjectToy>();

        public static PrimitiveObjectToy CreateCubeAPI(Vector3 position, Color color, Vector3 size, PrimitiveType primitiveType)
        {
            awa.TryGetComponent<AdminToyBase>(out var component);
            AdminToyBase adminToyBase = UnityEngine.Object.Instantiate(component, position, Quaternion.identity);
            PrimitiveObjectToy Base = (PrimitiveObjectToy)adminToyBase;
            Base.SpawnerFootprint = new Footprint(Server.Host.ReferenceHub);
            NetworkServer.Spawn(Base.gameObject);
            Base.NetworkPrimitiveType = primitiveType;
            Base.NetworkMaterialColor = color;
            Base.transform.position = position;
            Base.transform.rotation = Quaternion.identity;
            Base.transform.localScale = size;
            Base.NetworkScale = Base.transform.localScale;
            Base.NetworkPosition = Base.transform.position;
            Base.NetworkRotation = new LowPrecisionQuaternion(Base.transform.rotation);
            return Base;
        }

        public static PrimitiveObjectToy CreateCube(Vector3 position,Color color,Vector3 size, PrimitiveType primitiveType)
        {
            awa.TryGetComponent<AdminToyBase>(out var component);
            AdminToyBase adminToyBase = UnityEngine.Object.Instantiate(component, position, Quaternion.identity);
            PrimitiveObjectToy Base = (PrimitiveObjectToy)adminToyBase;
            Base.SpawnerFootprint = new Footprint(Server.Host.ReferenceHub);
            NetworkServer.Spawn(Base.gameObject);
            Base.NetworkPrimitiveType = primitiveType;
            Base.NetworkMaterialColor = color;
            Base.transform.position = position;
            Base.transform.rotation = Quaternion.identity;
            Base.transform.localScale = size;
            Base.NetworkScale = Base.transform.localScale;
            Base.NetworkPosition = Base.transform.position;
            Base.NetworkRotation = new LowPrecisionQuaternion(Base.transform.rotation);
            return Base;
        }
        public static LightSourceToy CreateLight(Vector3 vector3 ,int lightrange,Color color)
        {
            light.TryGetComponent<LightSourceToy>(out var component2);
            LightSourceToy Base2 = UnityEngine.Object.Instantiate(component2);
            Base2.transform.position = vector3;
            Base2.transform.localScale = Vector3.one;
            Base2.LightColor =color;
            Base2.LightRange = lightrange;
            NetworkServer.Spawn(Base2.gameObject);
            GameObject awa2 = Base2.gameObject;
            awa2.transform.localPosition = vector3;
            return Base2;
        }
        public static ShootingTarget CreatTarget(Vector3 position,Quaternion rotation,Vector3 size)
        {
            target.TryGetComponent<AdminToyBase>(out var component);
            AdminToyBase adminToyBase = UnityEngine.Object.Instantiate(component, position, rotation);
            var Base = (AdminToys.ShootingTarget)adminToyBase;
            Base.transform.localScale = ((size == default(Vector3)) ? Vector3.one : size);
            NetworkServer.Spawn(Base.gameObject);
            GameObject gameObject = Base.gameObject;
            gameObject.transform.parent = Base.transform;
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
            gameObject.transform.localScale = size;
            return Base;
        }
        public static void Del()
        {
            foreach(PrimitiveObjectToy primitiveObjectToy in primitiveObjectToys)
            {
                try
                {
                    NetworkServer.Destroy(primitiveObjectToy.gameObject);

                }
                catch
                {

                }
            }
            foreach (ShootingTarget shootingTarget in shootingTargets)
            {
                try
                {

                    NetworkServer.Destroy(shootingTarget.gameObject);
                }
                catch
                {

                }
            }
            primitiveObjectToys.Clear();
            shootingTargets.Clear();
        }
        public static void Create()
        {
            InitLate();
            
        }

        /// <summary>
        /// 生成小鸡的方法
        /// </summary>
        /// <param name="postion">小鸡的初始坐标</param>

        public static void CreateChicken(Vector3 postion)
        {
            chicken = new List<PrimitiveObjectToy>();


            chicken.Add(CreateCubeAPI(postion + 0.125f * Vector3.right + 0.4f * Vector3.up, new Color(214 / 255, 184 / 255, 58 / 255),new Vector3(0.1f,0.75f,0.01f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.125f * Vector3.right + 0.02f * Vector3.up - Vector3.forward*0.045f, new Color(214 / 255, 184 / 255, 58 / 255),new Vector3(0.1f,0.01f,0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.125f * Vector3.right + 0.02f * Vector3.up - Vector3.forward * 0.195f, new Color(214 / 255, 184 / 255, 58 / 255),new Vector3(0.3f,0.01f,0.2f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.249f * Vector3.right + 0.21f * Vector3.up - Vector3.forward * 0.2451f, new Color(173 / 255, 146 / 255, 35 / 255),new Vector3(0.1f,0.01f,0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.2251f * Vector3.right + 0.21f * Vector3.up - Vector3.forward * 0.2451f, new Color(173 / 255, 146 / 255, 35 / 255), new Vector3(0.1f,0.01f,0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.424000025f * Vector3.right + 0.4f * Vector3.up - Vector3.forward * 0, new Color(214 / 255, 184 / 255, 58 / 255), new Vector3(0.1f,0.75f,0.01f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.424000025f * Vector3.right + 0.02f * Vector3.up - Vector3.forward * 0.045f, new Color(214 / 255, 184 / 255, 58 / 255), new Vector3(0.1f, 0.01f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.424000025f * Vector3.right + 0.02f * Vector3.up - Vector3.forward * 0.195f, new Color(214 / 255, 184 / 255, 58 / 255), new Vector3(0.3f, 0.01f, 0.2f), PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.3239f * Vector3.right + 0.021f * Vector3.up - Vector3.forward * 0.2451f, new Color(173 / 255, 146 / 255, 35 / 255), new Vector3(0.1f, 0.01f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 0.818f * Vector3.up - Vector3.forward * 0, new Color(1,1,1), new Vector3(0.6f, 0.6f, 0.8f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 1.254f * Vector3.up - Vector3.forward * 0.475000024f, new Color(1,1,1), new Vector3(0.4f, 0.5f, 0.3f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.3619f * Vector3.right + 1.3541f * Vector3.up - Vector3.forward * 0.58f, new Color(0,0,0), new Vector3(0.095f, 0.1f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.062f * Vector3.right + 1.3541f * Vector3.up - Vector3.forward * 0.58f, new Color(0,0,0), new Vector3(0.095f, 0.1f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 1.205f * Vector3.up - Vector3.forward * 0.725f, new Color(188 / 255, 153 / 255, 42 / 255), new Vector3(0.4f, 0.2f, 0.2f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 1.155f * Vector3.up - Vector3.forward * 0.73f, new Color(120 / 255, 96 / 255, 20 / 255), new Vector3(0.401f, 0.1f, 0.201f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 1.005f * Vector3.up - Vector3.forward * 0.725f, new Color(255 / 255, 16 / 255, 0 / 255), new Vector3(0.2f, 0.2f, 0.201f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.0621f * Vector3.right + 1.301f * Vector3.up - Vector3.forward * 0.7749f, new Color(101 / 255, 89 / 255, 51 / 255), new Vector3(0.1f, 0.01f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.3619f * Vector3.right + 1.301f * Vector3.up - Vector3.forward * 0.7749f, new Color(101 / 255, 89 / 255, 51 / 255), new Vector3(0.1f, 0.01f, 0.1f),PrimitiveType.Cube));
            chicken.Add(CreateCubeAPI(postion + 0.1603f * Vector3.right + 1.301f * Vector3.up - Vector3.forward * 0.6758f, new Color(231 / 255, 188 / 255, 51 / 255), new Vector3(0.1f, 0.01f, 0.1f),PrimitiveType.Cube));
            //chicken.Add(CreateCubeAPI(postion + 0.212f * Vector3.right + 0.868f * Vector3.up - Vector3.forward * 0.6758f, new Color(1,1,1), new Vector3(0.75f, 0.5f, 0.6f),PrimitiveType.Cube));
            //chicken.Add(CreateCubeAPI(postion + 0.273f * Vector3.right + 0.02f * Vector3.up - Vector3.forward * 0.09239999f, new Color(1,1,1), new Vector3(0.6f, 0.01f, 0.2f),PrimitiveType.Cube));


        }
        internal static void InitLate()
        {
            try
            {
                foreach (KeyValuePair<Guid, GameObject> prefab in NetworkClient.prefabs)
                {
                    switch (prefab.Key.ToString())
                    {
                        case "bf9a7ae6-aaea-0174-d807-e0d4adb1c524":
                            awa = prefab.Value;
                            break;
                        case "6996edbf-2adf-a5b4-e8ce-e089cf9710ae":
                            light = prefab.Value;
                            break;
                        case "422b08ed-0bc0-6cb4-7a7f-81dd37c430c0":
                            target = prefab.Value;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"error in Addons => Prefabs [InitLate]:\n{ex}\n{ex.StackTrace}");
            }
        }
    }

}
