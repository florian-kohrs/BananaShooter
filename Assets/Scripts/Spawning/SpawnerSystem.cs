using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[BurstCompile]
public partial struct SpawnerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        
        SpawnerJob job = new SpawnerJob { ecb = ecb.AsParallelWriter(), elapsed = elapsed };
        job.ScheduleParallel();

    }

}

[BurstCompile]
public partial struct SpawnerJob : IJobEntity
{

    [ReadOnly]
    public float elapsed;

    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute(ref OffsetSpawner spawner, [ChunkIndexInQuery] int sortKey)
    {
        // If the next spawn time has passed.
        if (spawner.NextSpawnTime >= elapsed)
            return;

        // Spawns a new entity and positions it at the spawner.
        Entity newEntity = ecb.Instantiate(sortKey, spawner.Prefab);
        //LocalTransform t = SystemAPI.GetComponent<LocalTransform>(newEntity);

        float random = math.PI2 * spawner.random.NextFloat();
        float range = spawner.random.NextFloat() * spawner.maxSpawnDistance;

        math.sincos(random, out double x, out double z);
        float3 newPosition = spawner.SpawnPosition + new float3((float)x * range, 0, (float)z * range);

        ecb.SetComponent(sortKey, newEntity, LocalTransform.FromPositionRotationScale(newPosition, quaternion.identity, spawner.scale));

        // LocalPosition.FromPosition returns a Transform initialized with the given position.
        //state.EntityManager.SetComponentData(newEntity, t);

        // Resets the next spawn time.
        spawner.NextSpawnTime = elapsed + spawner.SpawnRate;
    }

}

