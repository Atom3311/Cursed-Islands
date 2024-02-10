using Unity.Entities;
using UnityEngine;
public class AnimatorComponent : IComponentData
{
    public Animator ThisAnimator;
    public bool HasMainAnimator;
}