using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
partial struct LookAtTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        LookAtTargetJob job = new LookAtTargetJob
        {
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
[WithPresent(typeof(LookAtTarget))]
public partial struct LookAtTargetJob : IJobEntity
{

    private void Execute(ref LocalTransform local, in TargetPosition target)
    {
        float3 direction = target.targetPosition - local.Position;
        if (math.lengthsq(direction) > 0)
        {
            local.Rotation = quaternion.LookRotation(direction, math.up());
        }
    }

}