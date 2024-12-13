using Unity.Entities;
using UnityEngine;

class EntitiesReferencesAuthoring : MonoBehaviour
{

    public GameObject muzzleFlashPrefab;
    public GameObject bulletPrefab;
    public GameObject appleEnemyPrefab;

    class EntitiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                muzzleFlashPrefab = GetEntity(authoring.muzzleFlashPrefab, TransformUsageFlags.Dynamic),
                appleEnemyPrefab = GetEntity(authoring.appleEnemyPrefab, TransformUsageFlags.Dynamic),
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }

}

public struct EntitiesReferences : IComponentData
{
    public Entity muzzleFlashPrefab;
    public Entity appleEnemyPrefab;
    public Entity bulletPrefab;
}

public struct SpawnReferences : IComponentData
{
    public Entity muzzleFlashPrefab;
    public Entity appleEnemyPrefab;
    public Entity bulletPrefab;
}
