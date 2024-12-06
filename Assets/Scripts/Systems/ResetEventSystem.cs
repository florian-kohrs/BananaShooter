using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup),OrderLast=true)]
[UpdateAfter(typeof(DetectDeathSystem))]
partial struct ResetEventSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ResetEventJob job = new ResetEventJob();
        job.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ResetEventJob : IJobEntity
{
    public void Execute(ref AttackIfInRange shoot)
    {
        shoot.onAttackEvent.attackTriggered = false;
    }
}