using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[UpdateAfter(typeof(ResetTargetSystem))]
[BurstCompile]
partial struct SpawnProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float elapsed = (float)SystemAPI.Time.ElapsedTime;

        ComponentLookup<LocalTransform> transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

        ProjectileSpawnerJob projectileJob = new ProjectileSpawnerJob { 
            ecb = ecb.AsParallelWriter(), 
            localTransformLookup = transformLookup, 
            elapsed = elapsed 
        };
        projectileJob.ScheduleParallel();
    }

}


[BurstCompile]
public partial struct ProjectileSpawnerJob : IJobEntity
{

    [ReadOnly]
    public float elapsed;

    public EntityCommandBuffer.ParallelWriter ecb;

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<LocalTransform> localTransformLookup;

    private void Execute(ref ProjectileSpawn spawner, in LocalTransform position, in Target target, [ChunkIndexInQuery] int sortKey)
    {
        if (target.target == Entity.Null)
            return;

        // If the next spawn time has passed.
        if (spawner.NextSpawnTime >= elapsed)
            return;

        // Resets the next spawn time.
        spawner.NextSpawnTime = elapsed + spawner.SpawnRate;

        // Spawns a new entity and positions it at the spawner.
        Entity newEntity = ecb.Instantiate(sortKey, spawner.projectile);
        float3 thisPosition = position.Position;
        float3 targetPos = localTransformLookup[target.target].Position;

        quaternion lookRotation = quaternion.LookRotation(targetPos - thisPosition, math.up());
        ecb.SetComponent(sortKey, newEntity, LocalTransform.FromPositionRotation(position.Position, lookRotation));
        ecb.SetComponent(sortKey, newEntity, new TargetPosition { targetPosition = targetPos });

    }

}
