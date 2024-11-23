using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(ResetTargetSystem))]
[BurstCompile]
partial struct MoveToTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<LocalTransform> LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        MoveToTargetJob moveToTargetJob = new MoveToTargetJob() 
        { 
            LocalTransformLookup = LocalTransformLookup 
        };
        moveToTargetJob.ScheduleParallel();
    }
}




[BurstCompile]
public partial struct MoveToTargetJob : IJobEntity
{

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<LocalTransform> LocalTransformLookup;

    public void Execute(in Target target, ref UnitMover mover)
    {
        if (target.target == Entity.Null)
            return;

        float3 position = LocalTransformLookup[target.target].Position;
        mover.targetPosition = position;
    }

}