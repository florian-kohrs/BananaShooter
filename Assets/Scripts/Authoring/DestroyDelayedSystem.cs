using Unity.Entities;
using UnityEngine;

class DeleteDelayedAuthoring : MonoBehaviour
{

    public float delay;

    class RemoveDelayedAuthoringBaker : Baker<DeleteDelayedAuthoring>
    {
        public override void Bake(DeleteDelayedAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new DestroyDelayed
            {
                timer = authoring.delay,
            });
        }
    }

}

public struct DestroyDelayed : IComponentData
{
    public float timer;
}