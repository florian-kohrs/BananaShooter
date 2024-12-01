using Unity.Entities;
using UnityEngine;

class FindTargetAuthoring : MonoBehaviour
{
    public float timerMax = 0.2f;
    public float range = 10;
    public Faction targetFaction;
    public bool findTargetEnabled = true;

    class FindTargetAuthoringBaker : Baker<FindTargetAuthoring>
    {

        public override void Bake(FindTargetAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FindTarget
            {
                range = authoring.range,
                targetFaction = authoring.targetFaction,
                timerMax = authoring.timerMax,
            });
            SetComponentEnabled<FindTarget>(entity, authoring.findTargetEnabled);
        }
    }

}

public struct FindTarget : IComponentData, IEnableableComponent
{
    public Faction targetFaction;
    public float range;
    public float timer;
    public float timerMax;
}