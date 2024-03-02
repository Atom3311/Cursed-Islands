using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
public partial class ObjectVisualization : SystemBase
{
    private bool _isInit;
    protected override void OnCreate()
    {
        Scene duringScene = SceneManager.GetSceneAt(0);
        _isInit = duringScene.isLoaded;
        if (!_isInit)
        {
            SceneManager.sceneLoaded += delegate (Scene scene, LoadSceneMode mode)
            {
                if(!_isInit)
                    _isInit = duringScene.isLoaded;
            };
        }
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