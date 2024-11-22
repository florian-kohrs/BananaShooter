using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class TargetAuthoring : MonoBehaviour
{
    class TargetAuthoringBaker : Baker<TargetAuthoring>
    {
        public override void Bake(TargetAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new Target());
        }
    }

}

public partial struct Target : IComponentData, IEnableableComponent
{
    public Entity target;
}
