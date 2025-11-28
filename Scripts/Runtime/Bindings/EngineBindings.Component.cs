using UnityEngine;

namespace OdinInterop
{
    internal static partial class EngineBindings
    {
        // component api

        private static ObjectHandle<GameObject> GetGameObject(ObjectHandle<Component> component)
        {
            if (component)
                return component.value.gameObject;
            else
                return default;
        }

        private static ObjectHandle<Transform> GetTransformFromComponent(ObjectHandle<Component> component)
        {
            if (component)
                return component.value.transform;
            else
                return default;
        }

        // behaviour api

        private static bool IsBehaviourEnabled(ObjectHandle<Behaviour> behaviour)
        {
            if (behaviour)
                return behaviour.value.enabled;
            else
                return false;
        }

        private static void SetBehaviourEnabled(ObjectHandle<Behaviour> behaviour, bool enabled)
        {
            if (behaviour)
                behaviour.value.enabled = enabled;
        }

        private static bool IsBehaviourActiveAndEnabled(ObjectHandle<Behaviour> behaviour)
        {
            if (behaviour)
                return behaviour.value.isActiveAndEnabled;
            else
                return false;
        }
    }
}
