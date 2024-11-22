using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(ShootSystem))]
partial struct MuzzleFlashSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        foreach ((
            RefRW<Muzzle> muzzle,
            RefRW<LocalTransform> transform,
            Entity entity)
            in SystemAPI.Query<
                RefRW<Muzzle>,
                RefRW<LocalTransform>
                >().WithEntityAccess())
        {
            Entity parent = SystemAPI.GetComponent<Parent>(entity).Value;

            if (SystemAPI.GetComponent<ShootWeapon>(parent).onShootEvent.isTriggered)
                muzzle.ValueRW.startTime = elapsed;

            float t = 1-math.clamp((muzzle.ValueRO.startTime + muzzle.ValueRO.timeToVanish - elapsed) / muzzle.ValueRO.timeToVanish, 0.0f, 1.0f);
            transform.ValueRW.Rotation = math.slerp(muzzle.ValueRO.startRotation, muzzle.ValueRO.endRotation, t);
        }


    }

}
