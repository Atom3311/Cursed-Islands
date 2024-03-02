using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
public partial class ObjectVisualization : SystemBase
{
    private bool _isInit;
    private Scene _duringScene = SceneManager.GetSceneAt(0);
    protected override void OnCreate()
    {
        SceneManager.sceneLoaded += delegate (Scene scene, LoadSceneMode mode)
        {
            _isInit = _duringScene.isLoaded;
        };
    }
    protected override void OnUpdate()
    {
        if (!_isInit)
            return;
        
        foreach ( var (
            transform,
            visualObject) in SystemAPI.Query<
                RefRO<LocalTransform>,
                VisualObject>())
        {
            float3 position = transform.ValueRO.Position;
            quaternion rotation = transform.ValueRO.Rotation;
            if (!visualObject.IsCreated)
            {
                visualObject.ThisGameObject = Object.Instantiate(visualObject.ThisGameObject, position, rotation);
                visualObject.IsCreated = true;
            }
            Transform transformOfVisualObject = visualObject.ThisGameObject.transform;
            transformOfVisualObject.position = position;
            transformOfVisualObject.rotation = rotation;
        }
    }
}