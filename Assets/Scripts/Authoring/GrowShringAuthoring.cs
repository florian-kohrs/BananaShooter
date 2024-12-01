using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

class GrowShringAuthoring : MonoBehaviour
{

    public float totalTime;

    public float maxScale;

    class GrowShringAuthoringBaker : Baker<GrowShringAuthoring>
    {
        public override void Bake(GrowShringAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new GrowShrink
            {
                maxScale = authoring.maxScale,
                totalTime = authoring.totalTime,
            });
            AddComponent(e, new DestroyDelayed { timer = authoring.totalTime });
        }
    }

}

public struct GrowShrink : IComponentData
{
    public float totalTime;
    public float actualTime;
    public float maxScale;
}

[BurstCompile]
public partial struct GrowShrinkSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        GrowShrinkJob job = new GrowShrinkJob { deltaTime = SystemAPI.Time.DeltaTime};
        job.ScheduleParallel();
    }

}

public partial struct GrowShrinkJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform transform, ref GrowShrink growShrink)
    {
        growShrink.actualTime += deltaTime;
        float t = growShrink.actualTime / growShrink.totalTime;
        
        transform.Scale = (1-t) * growShrink.maxScale;
    }
}