using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

class DeathLootAuthoring : MonoBehaviour
{

    public Rarity rarityDrops;
    public float dropMultiplier;

    class DeathLootAuthoringBaker : Baker<DeathLootAuthoring>
    {
        public override void Bake(DeathLootAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new LootReference { 
                rarityDrops = authoring.rarityDrops, 
                dropMultiplier=authoring.dropMultiplier 
            });
        }
    }

}

public struct LootReference : IComponentData
{
    public float dropMultiplier;
    public Rarity rarityDrops;
}
