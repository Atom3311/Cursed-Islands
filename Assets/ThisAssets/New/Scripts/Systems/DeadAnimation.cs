using Unity.Entities;

public partial struct DeadAnimation : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        //foreach(var (healthState, animatorComponent) in SystemAPI.Query<
        //    RefRO<HealthState>,
        //    AnimatorComponent
        //    >())
        //{
        //    animatorComponent.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationDead, healthState.ValueRO.IsDead);
        //}
    }
}