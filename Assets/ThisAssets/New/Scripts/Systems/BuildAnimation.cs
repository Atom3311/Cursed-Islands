using Unity.Entities;
[UpdateAfter(typeof(ConnectMainAnimatorWithEntity))]
partial struct BuildAnimation : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach (var (builderTag, healthState, animator, entity) in SystemAPI.Query<
            Builder,
            HealthState,
            AnimatorComponent>()
            .WithEntityAccess())
        {
            if (healthState.IsDead)
            {
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationBuild, false);
                continue;
            }

            bool build = SystemAPI.HasComponent<Build>(entity);
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationBuild, build);
        }
    }
}