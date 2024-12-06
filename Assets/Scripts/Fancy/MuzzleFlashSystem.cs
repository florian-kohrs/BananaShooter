using NUnit.Framework.Constraints;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(AttackIfInRangeSystem))]
partial struct MuzzleFlashSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<AttackIfInRange> w = SystemAPI.GetComponentLookup<AttackIfInRange>(true);
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        MuzzleFlashJob job = new MuzzleFlashJob { elapsed = elapsed, weaponLookup = w };
        job.ScheduleParallel();
    }

}

[BurstCompile]
public partial struct MuzzleFlashJob : IJobEntity
{

    [ReadOnly]
    public float elapsed;

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<AttackIfInRange> weaponLookup;

    public void Execute(ref LocalTransform transform, in Parent p, ref Muzzle muzzle)
    {
        if (weaponLookup[p.Value].onAttackEvent.attackTriggered)
            muzzle.startTime = elapsed;

        float t = 1 - math.clamp((muzzle.startTime + muzzle.timeToVanish - elapsed) / muzzle.timeToVanish, 0.0f, 1.0f);
        transform.Rotation = math.slerp(muzzle.startRotation, muzzle.endRotation, t);
    }

}
