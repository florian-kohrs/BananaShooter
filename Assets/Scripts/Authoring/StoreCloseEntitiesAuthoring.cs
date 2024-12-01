using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class StoreCloseEntitiesAuthoring : MonoBehaviour
{
    public Faction targetFaction;
    public float range;
    public float timerMax;

    class StoreCloseEntitiesAuthoringBaker : Baker<StoreCloseEntitiesAuthoring>
    {
        public override void Bake(StoreCloseEntitiesAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddBuffer<CloseEntity>(e);
            AddComponent(e, new FindTarget
            {
                range = authoring.range,
                targetFaction = authoring.targetFaction,
                timerMax = authoring.timerMax
            });
        }
    }
}

public struct CloseEntity : IBufferElementData
{
    public float3 entityPosition;
    public UnitHealth health;
    public Entity closeEntity;
}