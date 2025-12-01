using UnityEngine.Playables;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        private static PlayableGraph CreatePlayableGraph(String8 name) => PlayableGraph.Create(name.ToString());
    }
}
