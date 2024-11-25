using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

[UpdateAfter(typeof(ResetTargetSystem))]
[BurstCompile]
partial struct SpawnProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float elapsed = (float)SystemAPI.Time.ElapsedTime;

        ProjectileSpawnerJob projectileJob = new ProjectileSpawnerJob { ecb = ecb.AsParallelWriter(), elapsed = elapsed };
        projectileJob.ScheduleParallel();
    }

}


[BurstCompile]
public partial struct ProjectileSpawnerJob : IJobEntity
{

    [ReadOnly]
    public float elapsed;

    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute(ref ProjectileSpawn spawner, in LocalToWorld position, in Target target, [ChunkIndexInQuery] int sortKey)
    {
        if (target.target == Entity.Null)
            return;

        // If the next spawn time has passed.
        if (spawner.NextSpawnTime >= elapsed)
            return;

        // Spawns a new entity and positions it at the spawner.
        Entity newEntity = ecb.Instantiate(sortKey, spawner.projectile);
        ecb.SetComponent(sortKey, newEntity, LocalTransform.FromPosition(position.Position));
        ecb.SetComponent(sortKey, newEntity, new Target { target = target.target });

        // Resets the next spawn time.
        spawner.NextSpawnTime = elapsed + spawner.SpawnRate;
    }

}
