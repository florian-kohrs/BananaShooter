using System.Resources;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ResetTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ResetTargetJob job = new ResetTargetJob() 
        { 
            LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
        };
        job.ScheduleParallel();
        //foreach (RefRW<Target> target in SystemAPI.Query<RefRW<Target>>())
        //{
        //    if (target.ValueRO.target == Entity.Null)
        //        continue;

        //    if(!SystemAPI.Exists(target.ValueRO.target))
        //        target.ValueRW.target = Entity.Null;
        //}
    }


}

[BurstCompile]
public partial struct ResetTargetJob : IJobEntity
{

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public ComponentLookup<LocalTransform> LocalTransformLookup;

    public void Execute(ref Target target)
    {
        if (target.target == Entity.Null)
            return;

        if (!LocalTransformLookup.HasComponent(target.target, out bool entityexists) || !entityexists)
            target.target = Entity.Null;
    }

}