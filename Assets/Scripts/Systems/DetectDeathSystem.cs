using System.Globalization;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast=true)]
public partial struct DetectDeathSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        DetectDeathJob job = new DetectDeathJob()
        {
            commandBuffer = commandBuffer.AsParallelWriter(),
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct DetectDeathJob : IJobEntity
{

    public EntityCommandBuffer.ParallelWriter commandBuffer;

    private void Execute(in UnitHealth health, [ChunkIndexInQuery] int sortKey, in Entity entity)
    {
        if (health.currentHealth <= 0)
        {
            commandBuffer.DestroyEntity(sortKey, entity);
        }
    }
}