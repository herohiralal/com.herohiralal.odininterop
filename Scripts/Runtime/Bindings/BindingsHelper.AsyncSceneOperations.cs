using UnityEngine;

namespace OdinInterop
{
    internal static partial class BindingsHelper
    {
        private static uint s_AsyncOperationIterator = 0;

        private static readonly AsyncOperation[] s_AsyncOperations = new AsyncOperation[0];

        internal static uint RegisterAsyncOperation(AsyncOperation operation)
        {
            uint id = ++s_AsyncOperationIterator;
            s_AsyncOperations[id] = operation;
            return id;
        }

        internal static float GetAsyncOperationProgress(uint id)
        {
            if (s_AsyncOperations.Length <= id)
                return 0f;

            var op = s_AsyncOperations[id];
            if (op == null)
                return 1f;

            var p = op.progress;
            if (p >= 0.9999f)
            {
                // equivalent to done
                s_AsyncOperations[id] = null;
                return 1f;
            }

            return p;
        }

        internal static bool IsAsyncOperationDone(uint id)
        {
            if (s_AsyncOperations.Length <= id)
                return false;

            var op = s_AsyncOperations[id];
            if (op == null) // once something gets done, we'll mark it as null so gc can collect it and treat it like it's done
                return true;

            if (op.isDone)
            {
                s_AsyncOperations[id] = null;
                return true;
            }

            return false;
        }

        internal static int GetAsyncOperationPriority(uint id)
        {
            if (s_AsyncOperations.Length <= id)
                return -1;

            var op = s_AsyncOperations[id];
            if (op == null)
                return -1;

            return op.priority;
        }

        internal static void SetAsyncOperationPriority(uint id, int priority)
        {
            if (s_AsyncOperations.Length <= id)
                return;

            var op = s_AsyncOperations[id];
            if (op == null)
                return;

            op.priority = priority;
        }

        internal static bool DoesAsyncSceneOperationAllowActivation(uint id)
        {
            if (s_AsyncOperations.Length <= id)
                return false;

            var op = s_AsyncOperations[id];
            if (op == null)
                return false;

            return op.allowSceneActivation;
        }

        internal static void SetAsyncSceneOperationAllowActivation(uint id, bool allow)
        {
            if (s_AsyncOperations.Length <= id)
                return;

            var op = s_AsyncOperations[id];
            if (op == null)
                return;

            op.allowSceneActivation = allow;
        }
    }
}