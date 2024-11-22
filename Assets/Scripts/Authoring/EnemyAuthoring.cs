using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{

    class EnemyAuthoringBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy());
        }
    }

}

public struct Enemy : IComponentData
{

}