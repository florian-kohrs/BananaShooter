using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


//[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
class ConvertIfReachedTargetAuthoring : MonoBehaviour
{
    public GameObject convertTo;

    class ConvertIfReachedTargetAuthoringBaker : Baker<ConvertIfReachedTargetAuthoring>
    {
        public override void Bake(ConvertIfReachedTargetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ConvertIfReachedTarget
            {
                convertTo = GetEntity(authoring.convertTo, TransformUsageFlags.Dynamic),
            });
        }
    }

}


public struct ConvertIfReachedTarget : IComponentData
{
    public Entity convertTo;
}

[BurstCompile]
public partial struct ConvertIfReachedTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        ConvertIfReachedTargetJob job = new ConvertIfReachedTargetJob { ecb = ecb.AsParallelWriter() };
        job.ScheduleParallel();
    }
}


[BurstCompile]
public partial struct ConvertIfReachedTargetJob : IJobEntity
{

    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute(Entity e,in ConvertIfReachedTarget converter, in LocalTransform transform, in UnitMover mover, [ChunkIndexInQuery] int sortKey)
    {
        float sqrDist = math.distancesq(mover.targetPosition, transform.Position);
        float triggerDistanceSqr = 0.05f;

        if (sqrDist > triggerDistanceSqr)
            return;

        Entity newEntity = ecb.Instantiate(sortKey, converter.convertTo);
        ecb.SetComponent(sortKey, newEntity, LocalTransform.FromPosition(mover.targetPosition));
        ecb.DestroyEntity(sortKey, e);
    }

}

