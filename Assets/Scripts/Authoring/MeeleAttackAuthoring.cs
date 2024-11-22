using Unity.Entities;
using UnityEngine;

class MeeleAttackAuthoring : MonoBehaviour
{
    class MeeleAttackAuthoringBaker : Baker<MeeleAttackAuthoring>
    {
        public override void Bake(MeeleAttackAuthoring authoring)
        {

        }
    }

}

public struct MeeleAttack
{
    public float timer;
    public float attackCooldown;

    public float damage;
}