using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;

partial struct PickupItemSystem : ISystem
{

    private EntityQuery itemQuery;


    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        itemQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, Collectable>().Build();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        var items = this.itemQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out var dependency);
        state.Dependency = dependency;

        var itemPositions = this.itemQuery.ToComponentDataListAsync<LocalTransform>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var collectableStates = this.itemQuery.ToComponentDataListAsync<Collectable>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;


        PickupCloseItemsJob job = new PickupCloseItemsJob
        {
            elapsed = (float)SystemAPI.Time.ElapsedTime,
            items = items.AsDeferredJobArray(),
            collectables = collectableStates.AsDeferredJobArray(),
            ecb = ecb.AsParallelWriter(),
            itemPositions = itemPositions.AsDeferredJobArray(),
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct PickupCloseItemsJob : IJobEntity
{

    [ReadOnly]
    public float elapsed;

    [ReadOnly]
    public NativeArray<Entity> items;

    [ReadOnly]
    public NativeArray<Collectable> collectables;

    [ReadOnly]
    public NativeArray<LocalTransform> itemPositions;


    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute([ChunkIndexInQuery] int sortKey, DynamicBuffer<WeaponHorder> weapons, ref  PickupCloseItems picker, in LocalTransform localTransform)
    {
        if (picker.nextTime > elapsed)
            return;

        picker.nextTime = elapsed + picker.cooldown;

        float range = picker.range;
        float sqrRange = range * range;

        int length = items.Length;

        for (int index = 0; index < length; index++)
        {
            Entity otherEntity = items[index];
            float3 otherPosition = itemPositions[index].Position;

            float sqrDistance = math.distancesq(localTransform.Position.xz, otherPosition.xz);
            if (sqrDistance < sqrRange)
            {
                weapons.Add(new WeaponHorder { weapon = otherEntity });
                ecb.SetComponentEnabled<Collectable>(sortKey, otherEntity, false);
                ecb.SetComponentEnabled<FindTarget>(sortKey, otherEntity, true);
            }
        }
    }
}
