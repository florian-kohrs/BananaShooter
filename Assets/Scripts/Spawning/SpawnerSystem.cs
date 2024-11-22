using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[BurstCompile]
public partial struct SpawnerSystem : ISystem
{

    //public Random random;

    //[BurstCompile]
    //public void OnCreate(ref SystemState state)
    //{
    //    random = new
    //}


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        // Queries for all Spawner components. Uses RefRW because this system wants
        // to read from and write to the component. If the system only needed read-only
        // access, it would use RefRO instead.
        foreach (RefRW<OffsetSpawner> spawner in SystemAPI.Query<RefRW<OffsetSpawner>>())
        {
            ProcessSpawner(spawner, ref ecb, elapsed);
        }
        ecb.Playback(state.EntityManager);
    }

    private void ProcessSpawner(RefRW<OffsetSpawner> spawner, ref EntityCommandBuffer ecb, float elapsed)
    {
        // If the next spawn time has passed.
        if (spawner.ValueRO.NextSpawnTime >= elapsed)
            return;

        // Spawns a new entity and positions it at the spawner.
        Entity newEntity = ecb.Instantiate(spawner.ValueRO.Prefab);
        //LocalTransform t = SystemAPI.GetComponent<LocalTransform>(newEntity);

        float random = math.PI2 * spawner.ValueRW.random.NextFloat();
        float range = spawner.ValueRW.random.NextFloat() * spawner.ValueRO.maxSpawnDistance;

        math.sincos(random, out double x, out double z);
        float3 newPosition = spawner.ValueRO.SpawnPosition + new float3((float)x * range, 0, (float)z * range);

        ecb.SetComponent(newEntity, LocalTransform.FromPosition(newPosition));

        // LocalPosition.FromPosition returns a Transform initialized with the given position.
        //state.EntityManager.SetComponentData(newEntity, t);

        // Resets the next spawn time.
        spawner.ValueRW.NextSpawnTime = elapsed + spawner.ValueRO.SpawnRate;
    }
}