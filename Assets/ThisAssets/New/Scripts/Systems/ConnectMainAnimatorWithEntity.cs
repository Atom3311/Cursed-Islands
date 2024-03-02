using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
[UpdateAfter(typeof(ObjectVisualization))]
public partial class ConnectMainAnimatorWithEntity : SystemBase
{
    private bool _isInit;
    protected override void OnCreate()
    {
        SceneManager.sceneLoaded += (s, m) =>
        {
            if (s.isSubScene)
                return;

            _isInit = true;

        };
    }
    protected override void OnUpdate()
    {
        if (!_isInit)
            return;

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