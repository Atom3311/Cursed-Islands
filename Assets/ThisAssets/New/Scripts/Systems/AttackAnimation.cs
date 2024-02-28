using Unity.Entities;
[UpdateAfter(typeof(ConnectMainAnimatorWithEntity))]
public partial struct AttackAnimation : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach (var (healthState, battleComponent, animator, entity) in SystemAPI.Query<
            RefRO<HealthState>,
            RefRO<BattleComponent>,
            AnimatorComponent>()
            .WithEntityAccess())
        {
            if (healthState.ValueRO.IsDead)
            {
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationAttack, false);
                continue;
            }

            bool isAttacking = SystemAPI.HasComponent<AttackingUnit>(entity);
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationAttack, isAttacking);
        }
    }
}