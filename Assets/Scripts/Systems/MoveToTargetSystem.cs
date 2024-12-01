using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(ResetTargetSystem))]
[BurstCompile]
partial struct UpdateTargetPositionSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<LocalTransform> LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        UpdateTargetPositionJob moveToTargetJob = new UpdateTargetPositionJob() 
        { 
            LocalTransformLookup = LocalTransformLookup 
        };
        moveToTargetJob.ScheduleParallel();
    }
}




[BurstCompile]
public partial struct UpdateTargetPositionJob : IJobEntity
{

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<LocalTransform> LocalTransformLookup;

    public void Execute(in Target target, ref TargetPosition mover)
    {
        if (target.target == Entity.Null)
            return;

        float3 position = LocalTransformLookup[target.target].Position;
        mover.targetPosition = position;
    }

}