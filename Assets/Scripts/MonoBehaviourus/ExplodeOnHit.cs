using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.PackageManager;
using UnityEngine;

public class ExplodeOnHit : MonoBehaviour
{

    Plane plane = new Plane(Vector3.up,Vector3.zero);

    protected Entity explodeEntity;
    //private BlobAssetStore blobAssetStore;
    EntityManager manager;


    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //blobAssetStore = new BlobAssetStore();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnExplosionAt(GetPosition());
        }
    }

    protected void SpawnExplosionAt(float3 position)
    {

        //Entity e = new EntityQueryBuilder(Allocator.Temp).WithAll<AoeDamage, Prefab>().Build(World.DefaultGameObjectInjectionWorld.EntityManager).GetSingletonEntity();
        //World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(e);
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(LocalTransform), typeof(LocalToWorld));
        manager.AddComponentData(e, new AoeDamage()
        {
            damage = 5,
        });
        manager.AddComponentData(e, new FindTarget()
        {
            targetFaction = Faction.Enemy,
            range = 10,
        });
        manager.AddComponentData(e, LocalTransform.FromPosition(position));
        manager.AddBuffer<CloseEntity>(e);

        //World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(e);
        //World.DefaultGameObjectInjectionWorld.Update();



        Debug.Log($"Spawned entity {e} at {position}");
    }

    public float3 GetPosition()
    {
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        }
        throw new System.Exception();
    }

    private void OnDestroy()
    {
        //blobAssetStore.Dispose();
    }

}
