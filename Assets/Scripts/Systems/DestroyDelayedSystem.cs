using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(FindTargetSystem))]
partial struct DestroyDelayedSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<DestroyDelayed> destroy,
            Entity entity) in SystemAPI.Query<
                RefRW<DestroyDelayed>>().WithEntityAccess())
        {
            destroy.ValueRW.timer -= deltaTime;
            if (destroy.ValueRO.timer > 0)
                continue;

            commandBuffer.DestroyEntity(entity);
        }
    }
}

