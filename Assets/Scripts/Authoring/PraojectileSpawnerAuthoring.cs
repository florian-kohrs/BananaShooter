using BovineLabs.Core.Extensions;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class PraojectileSpawnerAuthoring : MonoBehaviour
{

    public GameObject projectile;
    public float cooldown;

    class PraojectileSpawnerAuthoringBaker : Baker<PraojectileSpawnerAuthoring>
    {
        public override void Bake(PraojectileSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileSpawn
            {
                projectile = GetEntity(authoring.projectile, TransformUsageFlags.Dynamic),
                SpawnRate = authoring.cooldown,
            });
        }
    }

}

public struct ProjectileSpawn : IComponentData
{
    public Entity projectile;
    public float NextSpawnTime;
    public float SpawnRate;
}