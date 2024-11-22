using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(ResetTargetSystem))]
partial struct LookAtTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<LocalToWorld> localTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>();

        LookAtTargetJob job = new LookAtTargetJob
        {
            TransformLookup = localTransformLookup
        };
        job.ScheduleParallel();
    }
}

[WithPresent(typeof(LookAtTarget))]
public partial struct LookAtTargetJob : IJobEntity
{

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<LocalToWorld> TransformLookup;

    private void Execute(ref LocalTransform local, in LocalToWorld global, in Target target)
    {
        if (target.target == Entity.Null)
            return;

        LocalToWorld otherPosition = TransformLookup[target.target];
        float3 direction = otherPosition.Position - global.Position;
        if (math.lengthsq(direction) > 0)
        {
            local.Rotation = quaternion.LookRotation(direction, math.up());
        }
    }


}