using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
[UpdateAfter(typeof(ObjectVisualization))]
public partial class ConnectMainAnimatorWithEntity : SystemBase
{
    private bool _isInit;
    protected override void OnCreate()
    {
        Scene duringScene = SceneManager.GetSceneAt(0);
        _isInit = duringScene.isLoaded;
        if (!_isInit)
        {
            SceneManager.sceneLoaded += (s, m) =>
            {
                _isInit = duringScene.isLoaded;

            };
        }
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