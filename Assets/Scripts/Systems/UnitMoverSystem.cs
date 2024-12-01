using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJob job = new UnitMoverJob();
        job.deltaTime = SystemAPI.Time.DeltaTime;
        job.ScheduleParallel();
    }

}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{

    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, in TargetPosition position)
    {
        float3 moveDirection = position.targetPosition - localTransform.Position;
        if(math.lengthsq(moveDirection) == 0) 
        {
            return;
        }
        float distanceSqr = math.lengthsq(moveDirection);
        float frameSpeed = deltaTime * unitMover.speed;
        if(frameSpeed * frameSpeed > distanceSqr)
            frameSpeed = math.sqrt(distanceSqr);

        float3 frameMove = frameSpeed * math.normalize(moveDirection);
        //localTransform.Scale = math.sign(moveDirection.x);
        localTransform.Position += frameMove;
    }
}



[WithPresent(typeof(JumpToTargetPos))]
[BurstCompile]
public partial struct GoToTargetJob : IJobEntity
{

    public void Execute(ref LocalTransform localTransform,  in TargetPosition position)
    {
        localTransform.Position = position.targetPosition;
    }
}