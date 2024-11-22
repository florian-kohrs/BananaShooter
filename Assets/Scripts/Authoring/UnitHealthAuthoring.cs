using Unity.Entities;
using UnityEngine;

class UnitHealthAuthoring : MonoBehaviour
{

    public int maxHealth;

    class UnitHealthAuthoringBaker : Baker<UnitHealthAuthoring>
    {
        public override void Bake(UnitHealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitHealth
            {
                maxHealth = authoring.maxHealth,
                currentHealth = authoring.maxHealth,
                onDeath = false,
            });;
        }
    }
}


public partial struct UnitHealth : IComponentData
{
    public int maxHealth;

    public int currentHealth;

    public bool onDeath;
}
