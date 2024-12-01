using Unity.Entities;
using UnityEngine;

class CollectableAuthoring : MonoBehaviour
{

    public bool isEnabled = true;

    class CollectableAuthoringBaker : Baker<CollectableAuthoring>
    {
        public override void Bake(CollectableAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new Collectable { });
            SetComponentEnabled<Collectable>(e, authoring.isEnabled);
        }
    }

}

public struct Collectable : IComponentData, IEnableableComponent
{

}