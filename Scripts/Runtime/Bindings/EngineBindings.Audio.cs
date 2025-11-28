using UnityEngine;
using UnityEngine.Audio;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // AudioSource API

        private static bool IsAudioSourcePlaying(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.isPlaying : false;
        private static bool IsAudioSourceVirtual(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.isVirtual : false;
        private static void PlayAudioSource(ObjectHandle<AudioSource> audioSource, ulong delay = 0)
        {
            if (audioSource)
                audioSource.value.Play(delay);
        }
        private static void PlayAudioSourceDelayed(ObjectHandle<AudioSource> audioSource, float delay)
        {
            if (audioSource)
                audioSource.value.PlayDelayed(delay);
        }
        private static void PlayAudioSourceScheduled(ObjectHandle<AudioSource> audioSource, double time)
        {
            if (audioSource)
                audioSource.value.PlayScheduled(time);
        }
        private static void PlayAudioSourceOneShot(ObjectHandle<AudioSource> audioSource, ObjectHandle<AudioClip> clip, float volumeScale = 1.0f)
        {
            if (audioSource && clip)
                audioSource.value.PlayOneShot(clip, volumeScale);
        }
        private static void StopAudioSource(ObjectHandle<AudioSource> audioSource)
        {
            if (audioSource)
                audioSource.value.Stop();
        }
        private static void PauseAudioSource(ObjectHandle<AudioSource> audioSource)
        {
            if (audioSource)
                audioSource.value.Pause();
        }
        private static void UnPauseAudioSource(ObjectHandle<AudioSource> audioSource)
        {
            if (audioSource)
                audioSource.value.UnPause();
        }
        private static void SetAudioSourceScheduledStartTime(ObjectHandle<AudioSource> audioSource, double time)
        {
            if (audioSource)
                audioSource.value.SetScheduledStartTime(time);
        }
        private static void SetAudioSourceScheduledEndTime(ObjectHandle<AudioSource> audioSource, double time)
        {
            if (audioSource)
                audioSource.value.SetScheduledEndTime(time);
        }
        private static ObjectHandle<AudioClip> GetAudioSourceClip(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.clip : default;
        private static void SetAudioSourceClip(ObjectHandle<AudioSource> audioSource, ObjectHandle<AudioClip> clip)
        {
            if (audioSource)
                audioSource.value.clip = clip;
        }
        private static float GetAudioSourceVolume(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.volume : 0f;
        private static void SetAudioSourceVolume(ObjectHandle<AudioSource> audioSource, float volume)
        {
            if (audioSource)
                audioSource.value.volume = volume;
        }
        private static float GetAudioSourcePitch(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.pitch : 1f;
        private static void SetAudioSourcePitch(ObjectHandle<AudioSource> audioSource, float pitch)
        {
            if (audioSource)
                audioSource.value.pitch = pitch;
        }
        private static float GetAudioSourceTime(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.time : 0f;
        private static void SetAudioSourceTime(ObjectHandle<AudioSource> audioSource, float time)
        {
            if (audioSource)
                audioSource.value.time = time;
        }
        private static int GetAudioSourceTimeSamples(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.timeSamples : 0;
        private static void SetAudioSourceTimeSamples(ObjectHandle<AudioSource> audioSource, int timeSamples)
        {
            if (audioSource)
                audioSource.value.timeSamples = timeSamples;
        }
        private static bool IsAudioSourceLooping(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.loop : false;
        private static void SetAudioSourceLooping(ObjectHandle<AudioSource> audioSource, bool loop)
        {
            if (audioSource)
                audioSource.value.loop = loop;
        }
        private static bool GetAudioSourcePlayOnAwake(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.playOnAwake : false;
        private static void SetAudioSourcePlayOnAwake(ObjectHandle<AudioSource> audioSource, bool playOnAwake)
        {
            if (audioSource)
                audioSource.value.playOnAwake = playOnAwake;
        }
        private static bool IsAudioSourceMuted(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.mute : false;
        private static void SetAudioSourceMuted(ObjectHandle<AudioSource> audioSource, bool muted)
        {
            if (audioSource)
                audioSource.value.mute = muted;
        }
        private static bool DoesAudioSourceBypassEffects(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.bypassEffects : false;
        private static void SetAudioSourceBypassEffects(ObjectHandle<AudioSource> audioSource, bool bypass)
        {
            if (audioSource)
                audioSource.value.bypassEffects = bypass;
        }
        private static bool DoesAudioSourceBypassListenerEffects(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.bypassListenerEffects : false;
        private static void SetAudioSourceBypassListenerEffects(ObjectHandle<AudioSource> audioSource, bool bypass)
        {
            if (audioSource)
                audioSource.value.bypassListenerEffects = bypass;
        }
        private static bool DoesAudioSourceBypassReverbZones(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.bypassReverbZones : false;
        private static void SetAudioSourceBypassReverbZones(ObjectHandle<AudioSource> audioSource, bool bypass)
        {
            if (audioSource)
                audioSource.value.bypassReverbZones = bypass;
        }
        private static int GetAudioSourcePriority(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.priority : 128;
        private static void SetAudioSourcePriority(ObjectHandle<AudioSource> audioSource, int priority)
        {
            if (audioSource)
                audioSource.value.priority = priority;
        }
        private static float GetAudioSourcePanStereo(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.panStereo : 0f;
        private static void SetAudioSourcePanStereo(ObjectHandle<AudioSource> audioSource, float pan)
        {
            if (audioSource)
                audioSource.value.panStereo = pan;
        }
        private static float GetAudioSourceSpatialBlend(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.spatialBlend : 0f;
        private static void SetAudioSourceSpatialBlend(ObjectHandle<AudioSource> audioSource, float blend)
        {
            if (audioSource)
                audioSource.value.spatialBlend = blend;
        }
        private static float GetAudioSourceReverbZoneMix(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.reverbZoneMix : 1f;
        private static void SetAudioSourceReverbZoneMix(ObjectHandle<AudioSource> audioSource, float mix)
        {
            if (audioSource)
                audioSource.value.reverbZoneMix = mix;
        }
        private static float GetAudioSourceDopplerLevel(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.dopplerLevel : 1f;
        private static void SetAudioSourceDopplerLevel(ObjectHandle<AudioSource> audioSource, float level)
        {
            if (audioSource)
                audioSource.value.dopplerLevel = level;
        }
        private static float GetAudioSourceSpread(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.spread : 0f;
        private static void SetAudioSourceSpread(ObjectHandle<AudioSource> audioSource, float spread)
        {
            if (audioSource)
                audioSource.value.spread = spread;
        }
        private static AudioRolloffMode GetAudioSourceRolloffMode(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.rolloffMode : AudioRolloffMode.Logarithmic;
        private static void SetAudioSourceRolloffMode(ObjectHandle<AudioSource> audioSource, AudioRolloffMode mode)
        {
            if (audioSource)
                audioSource.value.rolloffMode = mode;
        }
        private static float GetAudioSourceMinDistance(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.minDistance : 1f;
        private static void SetAudioSourceMinDistance(ObjectHandle<AudioSource> audioSource, float distance)
        {
            if (audioSource)
                audioSource.value.minDistance = distance;
        }
        private static float GetAudioSourceMaxDistance(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.maxDistance : 500f;
        private static void SetAudioSourceMaxDistance(ObjectHandle<AudioSource> audioSource, float distance)
        {
            if (audioSource)
                audioSource.value.maxDistance = distance;
        }
        private static ObjectHandle<AudioMixerGroup> GetAudioSourceOutputMixerGroup(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.outputAudioMixerGroup : default;
        private static void SetAudioSourceOutputMixerGroup(ObjectHandle<AudioSource> audioSource, ObjectHandle<AudioMixerGroup> mixerGroup)
        {
            if (audioSource)
                audioSource.value.outputAudioMixerGroup = mixerGroup;
        }
        private static bool GetAudioSourceIgnoreListenerVolume(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.ignoreListenerVolume : false;
        private static void SetAudioSourceIgnoreListenerVolume(ObjectHandle<AudioSource> audioSource, bool ignore)
        {
            if (audioSource)
                audioSource.value.ignoreListenerVolume = ignore;
        }
        private static bool GetAudioSourceIgnoreListenerPause(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.ignoreListenerPause : false;
        private static void SetAudioSourceIgnoreListenerPause(ObjectHandle<AudioSource> audioSource, bool ignore)
        {
            if (audioSource)
                audioSource.value.ignoreListenerPause = ignore;
        }
        private static AudioVelocityUpdateMode GetAudioSourceVelocityUpdateMode(ObjectHandle<AudioSource> audioSource) => audioSource ? audioSource.value.velocityUpdateMode : AudioVelocityUpdateMode.Auto;
        private static void SetAudioSourceVelocityUpdateMode(ObjectHandle<AudioSource> audioSource, AudioVelocityUpdateMode mode)
        {
            if (audioSource)
                audioSource.value.velocityUpdateMode = mode;
        }

        // AudioClip API

        private static float GetAudioClipLength(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.length : 0f;
        private static int GetAudioClipSamples(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.samples : 0;
        private static int GetAudioClipChannels(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.channels : 0;
        private static int GetAudioClipFrequency(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.frequency : 0;
        private static bool IsAudioClipPreloaded(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.preloadAudioData : false;
        private static bool IsAudioClipAmbisonic(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.ambisonic : false;
        private static AudioClipLoadType GetAudioClipLoadType(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.loadType : AudioClipLoadType.DecompressOnLoad;
        private static bool LoadAudioClipData(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.LoadAudioData() : false;
        private static bool UnloadAudioClipData(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.UnloadAudioData() : false;
        private static AudioDataLoadState GetAudioClipLoadState(ObjectHandle<AudioClip> audioClip) => audioClip ? audioClip.value.loadState : AudioDataLoadState.Unloaded;

        // AudioListener API

        private static float GetAudioListenerVolume() => AudioListener.volume;
        private static void SetAudioListenerVolume(float volume) => AudioListener.volume = volume;
        private static bool GetAudioListenerPause() => AudioListener.pause;
        private static void SetAudioListenerPause(bool pause) => AudioListener.pause = pause;
        private static AudioVelocityUpdateMode GetAudioListenerVelocityUpdateMode(ObjectHandle<AudioListener> listener) => listener ? listener.value.velocityUpdateMode : AudioVelocityUpdateMode.Auto;
        private static void SetAudioListenerVelocityUpdateMode(ObjectHandle<AudioListener> listener, AudioVelocityUpdateMode mode)
        {
            if (listener)
                listener.value.velocityUpdateMode = mode;
        }

        // AudioSettings API

        private static double GetAudioSettingsDspTime() => AudioSettings.dspTime;
        private static AudioSpeakerMode GetAudioSettingsSpeakerMode() => AudioSettings.speakerMode;
        private static void GetAudioSettingsDSPBufferSize(out int bufferLength, out int numBuffers) => AudioSettings.GetDSPBufferSize(out bufferLength, out numBuffers);
        private static int GetAudioSettingsOutputSampleRate() => AudioSettings.outputSampleRate;
        private static void SetAudioSettingsOutputSampleRate(int sampleRate) => AudioSettings.outputSampleRate = sampleRate;
        private static void ResetAudioSettings(AudioConfiguration config) => AudioSettings.Reset(config);
        private static AudioConfiguration GetAudioConfiguration() => AudioSettings.GetConfiguration();

        // AudioMixer API

        private static bool SetAudioMixerFloatValue(ObjectHandle<AudioMixer> mixer, String8 name, float value) => mixer ? mixer.value.SetFloat(name.ToString(), value) : false;
        private static bool GetAudioMixerFloatValue(ObjectHandle<AudioMixer> mixer, String8 name, out float value)
        {
            if (mixer)
                return mixer.value.GetFloat(name.ToString(), out value);
            value = 0f;
            return false;
        }
        private static bool ClearAudioMixerFloatValue(ObjectHandle<AudioMixer> mixer, String8 name) => mixer ? mixer.value.ClearFloat(name.ToString()) : false;
        private static ObjectHandle<AudioMixerSnapshot> FindAudioMixerSnapshot(ObjectHandle<AudioMixer> mixer, String8 name) => mixer ? mixer.value.FindSnapshot(name.ToString()) : default;
        private static void TransitionToAudioMixerSnapshot(ObjectHandle<AudioMixerSnapshot> snapshot, float timeToReach)
        {
            if (snapshot)
                snapshot.value.TransitionTo(timeToReach);
        }
        private static void TransitionToAudioMixerSnapshots(Slice<ObjectHandle<AudioMixerSnapshot>> snapshots, Slice<float> weights, float timeToReach)
        {
            if (snapshots.len != weights.len || snapshots.len.ToInt32() == 0)
                return;

            var snapshotArr = new AudioMixerSnapshot[(int)snapshots.len];
            var weightArr = new float[(int)weights.len];
            for (var i = 0; i < (int)snapshots.len; i++)
            {
                snapshotArr[i] = snapshots.ptr[i];
                weightArr[i] = weights.ptr[i];
            }
            if (snapshotArr[0] != null)
                snapshotArr[0].audioMixer.TransitionToSnapshots(snapshotArr, weightArr, timeToReach);
        }

        // AudioMixerGroup API

        private static ObjectHandle<AudioMixer> GetAudioMixerFromGroup(ObjectHandle<AudioMixerGroup> group) => group ? group.value.audioMixer : default;

        // Microphone API

        private static Slice<String8> GetMicrophoneDevices(Allocator allocator)
        {
            var devices = Microphone.devices;
            var slice = new Slice<String8>(devices.Length, allocator);
            for (var i = 0; i < devices.Length; i++)
                slice.ptr[i] = new String8(devices[i], allocator);
            return slice;
        }
        private static void GetMicrophoneDeviceCaps(String8 deviceName, out int minFreq, out int maxFreq) => Microphone.GetDeviceCaps(deviceName.ToString(), out minFreq, out maxFreq);
        private static ObjectHandle<AudioClip> StartMicrophoneRecording(String8 deviceName, bool loop, int lengthSec, int frequency) => Microphone.Start(deviceName.ToString(), loop, lengthSec, frequency);
        private static void EndMicrophoneRecording(String8 deviceName) => Microphone.End(deviceName.ToString());
        private static bool IsMicrophoneRecording(String8 deviceName) => Microphone.IsRecording(deviceName.ToString());
        private static int GetMicrophonePosition(String8 deviceName) => Microphone.GetPosition(deviceName.ToString());

        // Audio static play methods

        private static void PlayClipAtPoint(ObjectHandle<AudioClip> clip, Vector3 position, float volume = 1.0f)
        {
            if (clip)
                AudioSource.PlayClipAtPoint(clip, position, volume);
        }

        // AudioReverbZone API

        private static float GetAudioReverbZoneMinDistance(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.minDistance : 0f;
        private static void SetAudioReverbZoneMinDistance(ObjectHandle<AudioReverbZone> zone, float distance)
        {
            if (zone)
                zone.value.minDistance = distance;
        }
        private static float GetAudioReverbZoneMaxDistance(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.maxDistance : 0f;
        private static void SetAudioReverbZoneMaxDistance(ObjectHandle<AudioReverbZone> zone, float distance)
        {
            if (zone)
                zone.value.maxDistance = distance;
        }
        private static AudioReverbPreset GetAudioReverbZonePreset(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.reverbPreset : AudioReverbPreset.User;
        private static void SetAudioReverbZonePreset(ObjectHandle<AudioReverbZone> zone, AudioReverbPreset preset)
        {
            if (zone)
                zone.value.reverbPreset = preset;
        }
        private static int GetAudioReverbZoneRoom(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.room : 0;
        private static void SetAudioReverbZoneRoom(ObjectHandle<AudioReverbZone> zone, int room)
        {
            if (zone)
                zone.value.room = room;
        }
        private static int GetAudioReverbZoneRoomHF(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.roomHF : 0;
        private static void SetAudioReverbZoneRoomHF(ObjectHandle<AudioReverbZone> zone, int roomHF)
        {
            if (zone)
                zone.value.roomHF = roomHF;
        }
        private static int GetAudioReverbZoneRoomLF(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.roomLF : 0;
        private static void SetAudioReverbZoneRoomLF(ObjectHandle<AudioReverbZone> zone, int roomLF)
        {
            if (zone)
                zone.value.roomLF = roomLF;
        }
        private static float GetAudioReverbZoneDecayTime(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.decayTime : 1f;
        private static void SetAudioReverbZoneDecayTime(ObjectHandle<AudioReverbZone> zone, float time)
        {
            if (zone)
                zone.value.decayTime = time;
        }
        private static float GetAudioReverbZoneDecayHFRatio(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.decayHFRatio : 0.5f;
        private static void SetAudioReverbZoneDecayHFRatio(ObjectHandle<AudioReverbZone> zone, float ratio)
        {
            if (zone)
                zone.value.decayHFRatio = ratio;
        }
        private static int GetAudioReverbZoneReflections(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.reflections : 0;
        private static void SetAudioReverbZoneReflections(ObjectHandle<AudioReverbZone> zone, int reflections)
        {
            if (zone)
                zone.value.reflections = reflections;
        }
        private static float GetAudioReverbZoneReflectionsDelay(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.reflectionsDelay : 0f;
        private static void SetAudioReverbZoneReflectionsDelay(ObjectHandle<AudioReverbZone> zone, float delay)
        {
            if (zone)
                zone.value.reflectionsDelay = delay;
        }
        private static int GetAudioReverbZoneReverb(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.reverb : 0;
        private static void SetAudioReverbZoneReverb(ObjectHandle<AudioReverbZone> zone, int reverb)
        {
            if (zone)
                zone.value.reverb = reverb;
        }
        private static float GetAudioReverbZoneReverbDelay(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.reverbDelay : 0f;
        private static void SetAudioReverbZoneReverbDelay(ObjectHandle<AudioReverbZone> zone, float delay)
        {
            if (zone)
                zone.value.reverbDelay = delay;
        }
        private static float GetAudioReverbZoneDiffusion(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.diffusion : 100f;
        private static void SetAudioReverbZoneDiffusion(ObjectHandle<AudioReverbZone> zone, float diffusion)
        {
            if (zone)
                zone.value.diffusion = diffusion;
        }
        private static float GetAudioReverbZoneDensity(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.density : 100f;
        private static void SetAudioReverbZoneDensity(ObjectHandle<AudioReverbZone> zone, float density)
        {
            if (zone)
                zone.value.density = density;
        }
        private static float GetAudioReverbZoneHFReference(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.HFReference : 5000f;
        private static void SetAudioReverbZoneHFReference(ObjectHandle<AudioReverbZone> zone, float reference)
        {
            if (zone)
                zone.value.HFReference = reference;
        }
        private static float GetAudioReverbZoneLFReference(ObjectHandle<AudioReverbZone> zone) => zone ? zone.value.LFReference : 250f;
        private static void SetAudioReverbZoneLFReference(ObjectHandle<AudioReverbZone> zone, float reference)
        {
            if (zone)
                zone.value.LFReference = reference;
        }
    }
}
