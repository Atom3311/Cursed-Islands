using Unity.Entities;
using UnityEngine;
[UpdateAfter(typeof(ObjectVisualization))]
public partial struct ConnectMainAnimatorWithEntity : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach((
            VisualObject targetObject,
            AnimatorComponent animator) in SystemAPI.Query<
                VisualObject,
                AnimatorComponent>())
        {
            if (animator.HasMainAnimator)
                continue;

            animator.ThisAnimator = targetObject.ThisGameObject.GetComponent<Animator>();
            animator.HasMainAnimator = true;

        }
    }
}