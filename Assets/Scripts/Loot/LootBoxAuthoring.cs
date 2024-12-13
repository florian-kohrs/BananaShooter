using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

class LootBoxAuthoring : MonoBehaviour
{

    class LootBoxAuthoringBaker : Baker<LootBoxAuthoring>
    {
        public override void Bake(LootBoxAuthoring authoring)
        {
            AddBuffer<LootBox>(GetEntity(TransformUsageFlags.None));
        }
    }
}

public struct LootBox : IBufferElementData
{
    public Rarity rarity;
    public float probability;
    public Entity spawn;
}

[Flags]
public enum Rarity { None=0,Common=1, Rare=2, Epic=4, Legendary=8}

