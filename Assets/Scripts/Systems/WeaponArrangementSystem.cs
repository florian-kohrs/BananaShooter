using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

//[UpdateInGroup(typeof(SimulationSystemGroup),OrderFirst =true)]
partial struct WeaponArrangementSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<LocalTransform> localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
        WeaponArrangementJob job = new WeaponArrangementJob { localTransformLookup = localTransformLookup };
        job.Schedule();
    }

}


[BurstCompile]
public partial struct WeaponArrangementJob : IJobEntity
{

    public ComponentLookup<LocalTransform> localTransformLookup;

    public void Execute(in Entity entity, in DynamicBuffer<WeaponHorder> weapons)
    {
        int total = weapons.Length;
        int remaining = total;
        int layer = 1;
        int layerDistance = 2;

        DynamicBuffer<Entity> entities = weapons.Reinterpret<Entity>();
        float3 center = localTransformLookup[entity].Position;

        while (remaining > 0)
        {
            int positionsInLayer = (int)math.pow(2, layer + 2); // 4, 8, 16, etc.
            float radius = layer * layerDistance;

            int entitiesInThisLayer = math.min(remaining, positionsInLayer);

            for (int i = 1; i <= entitiesInThisLayer; i++)
            {
                Entity e = entities[remaining - i];
                float angle = math.PI2 * i / entitiesInThisLayer; // Evenly spread
                math.sincos(angle, out float sin, out float cos);
                LocalTransform t = localTransformLookup[e];
                t.Position = center + new float3(sin * radius, 0, cos * radius);
                localTransformLookup[e] = t;
            }
            remaining -= entitiesInThisLayer;
            layer++;
        }
    }

}