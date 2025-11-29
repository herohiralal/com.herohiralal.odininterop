using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // SkinnedMeshRenderer API

        private static ObjectHandle<Mesh> GetSkinnedMeshRendererSharedMesh(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.sharedMesh : default;
        private static void SetSkinnedMeshRendererSharedMesh(ObjectHandle<SkinnedMeshRenderer> renderer, ObjectHandle<Mesh> mesh)
        {
            if (renderer)
                renderer.value.sharedMesh = mesh;
        }
        private static SkinQuality GetSkinnedMeshRendererQuality(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.quality : SkinQuality.Auto;
        private static void SetSkinnedMeshRendererQuality(ObjectHandle<SkinnedMeshRenderer> renderer, SkinQuality quality)
        {
            if (renderer)
                renderer.value.quality = quality;
        }
        private static bool GetSkinnedMeshRendererUpdateWhenOffscreen(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.updateWhenOffscreen : false;
        private static void SetSkinnedMeshRendererUpdateWhenOffscreen(ObjectHandle<SkinnedMeshRenderer> renderer, bool update)
        {
            if (renderer)
                renderer.value.updateWhenOffscreen = update;
        }
        private static bool GetSkinnedMeshRendererForceMatrixRecalculationPerRender(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.forceMatrixRecalculationPerRender : false;
        private static void SetSkinnedMeshRendererForceMatrixRecalculationPerRender(ObjectHandle<SkinnedMeshRenderer> renderer, bool force)
        {
            if (renderer)
                renderer.value.forceMatrixRecalculationPerRender = force;
        }
        private static ObjectHandle<Transform> GetSkinnedMeshRendererRootBone(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.rootBone : default;
        private static void SetSkinnedMeshRendererRootBone(ObjectHandle<SkinnedMeshRenderer> renderer, ObjectHandle<Transform> rootBone)
        {
            if (renderer)
                renderer.value.rootBone = rootBone;
        }
        private static Slice<ObjectHandle<Transform>> GetSkinnedMeshRendererBones(ObjectHandle<SkinnedMeshRenderer> renderer, Allocator allocator)
        {
            if (!renderer)
                return default;
            var bones = renderer.value.bones;
            var slice = new Slice<ObjectHandle<Transform>>(bones.Length, allocator);
            for (var i = 0; i < bones.Length; i++)
                slice.ptr[i] = bones[i];
            return slice;
        }
        private static void SetSkinnedMeshRendererBones(ObjectHandle<SkinnedMeshRenderer> renderer, Slice<ObjectHandle<Transform>> bones)
        {
            if (!renderer)
                return;
            var arr = new Transform[(int)bones.len];
            for (var i = 0; i < (int)bones.len; i++)
                arr[i] = bones.ptr[i];
            renderer.value.bones = arr;
        }
        private static Bounds GetSkinnedMeshRendererLocalBounds(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.localBounds : default;
        private static void SetSkinnedMeshRendererLocalBounds(ObjectHandle<SkinnedMeshRenderer> renderer, Bounds bounds)
        {
            if (renderer)
                renderer.value.localBounds = bounds;
        }
        private static void BakeSkinnedMesh(ObjectHandle<SkinnedMeshRenderer> renderer, ObjectHandle<Mesh> mesh, bool useScale = false)
        {
            if (renderer && mesh)
                renderer.value.BakeMesh(mesh, useScale);
        }
        private static float GetSkinnedMeshRendererBlendShapeWeight(ObjectHandle<SkinnedMeshRenderer> renderer, int index)
        {
            if (renderer && index >= 0 && index < renderer.value.sharedMesh.blendShapeCount)
                return renderer.value.GetBlendShapeWeight(index);
            return 0f;
        }
        private static void SetSkinnedMeshRendererBlendShapeWeight(ObjectHandle<SkinnedMeshRenderer> renderer, int index, float weight)
        {
            if (renderer && index >= 0 && index < renderer.value.sharedMesh.blendShapeCount)
                renderer.value.SetBlendShapeWeight(index, weight);
        }
        private static bool GetSkinnedMeshRendererSkinnedMotionVectors(ObjectHandle<SkinnedMeshRenderer> renderer) => renderer ? renderer.value.skinnedMotionVectors : false;
        private static void SetSkinnedMeshRendererSkinnedMotionVectors(ObjectHandle<SkinnedMeshRenderer> renderer, bool skinnedMotionVectors)
        {
            if (renderer)
                renderer.value.skinnedMotionVectors = skinnedMotionVectors;
        }

        // Mesh BlendShape API (extension for skinned meshes)

        private static int GetMeshBlendShapeCount(ObjectHandle<Mesh> mesh) => mesh ? mesh.value.blendShapeCount : 0;
        private static String8 GetMeshBlendShapeName(ObjectHandle<Mesh> mesh, int shapeIndex, Allocator allocator)
        {
            if (mesh && shapeIndex >= 0 && shapeIndex < mesh.value.blendShapeCount)
                return new String8(mesh.value.GetBlendShapeName(shapeIndex), allocator);
            return default;
        }
        private static int GetMeshBlendShapeIndex(ObjectHandle<Mesh> mesh, String8 blendShapeName)
        {
            if (mesh)
                return mesh.value.GetBlendShapeIndex(blendShapeName.ToString());
            return -1;
        }
        private static int GetMeshBlendShapeFrameCount(ObjectHandle<Mesh> mesh, int shapeIndex)
        {
            if (mesh && shapeIndex >= 0 && shapeIndex < mesh.value.blendShapeCount)
                return mesh.value.GetBlendShapeFrameCount(shapeIndex);
            return 0;
        }
        private static float GetMeshBlendShapeFrameWeight(ObjectHandle<Mesh> mesh, int shapeIndex, int frameIndex)
        {
            if (mesh && shapeIndex >= 0 && shapeIndex < mesh.value.blendShapeCount)
                return mesh.value.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
            return 0f;
        }
        private static void AddMeshBlendShapeFrame(ObjectHandle<Mesh> mesh, String8 shapeName, float frameWeight, Slice<Vector3> deltaVertices, Slice<Vector3> deltaNormals, Slice<Vector3> deltaTangents)
        {
            if (!mesh)
                return;
            var verts = new Vector3[(int)deltaVertices.len];
            var norms = new Vector3[(int)deltaNormals.len];
            var tans = new Vector3[(int)deltaTangents.len];
            for (var i = 0; i < (int)deltaVertices.len; i++)
                verts[i] = deltaVertices.ptr[i];
            for (var i = 0; i < (int)deltaNormals.len; i++)
                norms[i] = deltaNormals.ptr[i];
            for (var i = 0; i < (int)deltaTangents.len; i++)
                tans[i] = deltaTangents.ptr[i];
            mesh.value.AddBlendShapeFrame(shapeName.ToString(), frameWeight, verts, norms, tans);
        }
        private static void ClearMeshBlendShapes(ObjectHandle<Mesh> mesh)
        {
            if (mesh)
                mesh.value.ClearBlendShapes();
        }

        // Avatar API

        private static bool IsAvatarValid(ObjectHandle<Avatar> avatar) => avatar ? avatar.value.isValid : false;
        private static bool IsAvatarHuman(ObjectHandle<Avatar> avatar) => avatar ? avatar.value.isHuman : false;

        // Animator API

        private static bool IsAnimatorInitialized(ObjectHandle<Animator> animator) => animator ? animator.value.isInitialized : false;
        private static bool IsAnimatorHuman(ObjectHandle<Animator> animator) => animator ? animator.value.isHuman : false;
        private static bool GetAnimatorHasRootMotion(ObjectHandle<Animator> animator) => animator ? animator.value.hasRootMotion : false;
        private static float GetAnimatorHumanScale(ObjectHandle<Animator> animator) => animator ? animator.value.humanScale : 1f;
        private static bool GetAnimatorHasTransformHierarchy(ObjectHandle<Animator> animator) => animator ? animator.value.hasTransformHierarchy : true;
        private static Vector3 GetAnimatorDeltaPosition(ObjectHandle<Animator> animator) => animator ? animator.value.deltaPosition : default;
        private static Quaternion GetAnimatorDeltaRotation(ObjectHandle<Animator> animator) => animator ? animator.value.deltaRotation : Quaternion.identity;
        private static Vector3 GetAnimatorVelocity(ObjectHandle<Animator> animator) => animator ? animator.value.velocity : default;
        private static Vector3 GetAnimatorAngularVelocity(ObjectHandle<Animator> animator) => animator ? animator.value.angularVelocity : default;
        private static Vector3 GetAnimatorRootPosition(ObjectHandle<Animator> animator) => animator ? animator.value.rootPosition : default;
        private static void SetAnimatorRootPosition(ObjectHandle<Animator> animator, Vector3 position)
        {
            if (animator)
                animator.value.rootPosition = position;
        }
        private static Quaternion GetAnimatorRootRotation(ObjectHandle<Animator> animator) => animator ? animator.value.rootRotation : Quaternion.identity;
        private static void SetAnimatorRootRotation(ObjectHandle<Animator> animator, Quaternion rotation)
        {
            if (animator)
                animator.value.rootRotation = rotation;
        }
        private static bool GetAnimatorApplyRootMotion(ObjectHandle<Animator> animator) => animator ? animator.value.applyRootMotion : false;
        private static void SetAnimatorApplyRootMotion(ObjectHandle<Animator> animator, bool apply)
        {
            if (animator)
                animator.value.applyRootMotion = apply;
        }
        private static AnimatorUpdateMode GetAnimatorUpdateMode(ObjectHandle<Animator> animator) => animator ? animator.value.updateMode : AnimatorUpdateMode.Normal;
        private static void SetAnimatorUpdateMode(ObjectHandle<Animator> animator, AnimatorUpdateMode mode)
        {
            if (animator)
                animator.value.updateMode = mode;
        }
        private static AnimatorCullingMode GetAnimatorCullingMode(ObjectHandle<Animator> animator) => animator ? animator.value.cullingMode : AnimatorCullingMode.AlwaysAnimate;
        private static void SetAnimatorCullingMode(ObjectHandle<Animator> animator, AnimatorCullingMode mode)
        {
            if (animator)
                animator.value.cullingMode = mode;
        }
        private static float GetAnimatorPlaybackTime(ObjectHandle<Animator> animator) => animator ? animator.value.playbackTime : 0f;
        private static void SetAnimatorPlaybackTime(ObjectHandle<Animator> animator, float time)
        {
            if (animator)
                animator.value.playbackTime = time;
        }
        private static float GetAnimatorRecorderStartTime(ObjectHandle<Animator> animator) => animator ? animator.value.recorderStartTime : 0f;
        private static void SetAnimatorRecorderStartTime(ObjectHandle<Animator> animator, float time)
        {
            if (animator)
                animator.value.recorderStartTime = time;
        }
        private static float GetAnimatorRecorderStopTime(ObjectHandle<Animator> animator) => animator ? animator.value.recorderStopTime : 0f;
        private static void SetAnimatorRecorderStopTime(ObjectHandle<Animator> animator, float time)
        {
            if (animator)
                animator.value.recorderStopTime = time;
        }
        private static AnimatorRecorderMode GetAnimatorRecorderMode(ObjectHandle<Animator> animator) => animator ? animator.value.recorderMode : AnimatorRecorderMode.Offline;
        private static ObjectHandle<RuntimeAnimatorController> GetAnimatorRuntimeController(ObjectHandle<Animator> animator) => animator ? animator.value.runtimeAnimatorController : default;
        private static void SetAnimatorRuntimeController(ObjectHandle<Animator> animator, ObjectHandle<RuntimeAnimatorController> controller)
        {
            if (animator)
                animator.value.runtimeAnimatorController = controller;
        }
        private static ObjectHandle<Avatar> GetAnimatorAvatar(ObjectHandle<Animator> animator) => animator ? animator.value.avatar : default;
        private static void SetAnimatorAvatar(ObjectHandle<Animator> animator, ObjectHandle<Avatar> avatar)
        {
            if (animator)
                animator.value.avatar = avatar;
        }
        private static float GetAnimatorSpeed(ObjectHandle<Animator> animator) => animator ? animator.value.speed : 1f;
        private static void SetAnimatorSpeed(ObjectHandle<Animator> animator, float speed)
        {
            if (animator)
                animator.value.speed = speed;
        }
        private static Vector3 GetAnimatorTargetPosition(ObjectHandle<Animator> animator) => animator ? animator.value.targetPosition : default;
        private static Quaternion GetAnimatorTargetRotation(ObjectHandle<Animator> animator) => animator ? animator.value.targetRotation : Quaternion.identity;
        private static bool GetAnimatorStabilizeFeet(ObjectHandle<Animator> animator) => animator ? animator.value.stabilizeFeet : false;
        private static void SetAnimatorStabilizeFeet(ObjectHandle<Animator> animator, bool stabilize)
        {
            if (animator)
                animator.value.stabilizeFeet = stabilize;
        }
        private static float GetAnimatorFeetPivotActive(ObjectHandle<Animator> animator) => animator ? animator.value.feetPivotActive : 0f;
        private static void SetAnimatorFeetPivotActive(ObjectHandle<Animator> animator, float value)
        {
            if (animator)
                animator.value.feetPivotActive = value;
        }
        private static bool GetAnimatorLayersAffectMassCenter(ObjectHandle<Animator> animator) => animator ? animator.value.layersAffectMassCenter : false;
        private static void SetAnimatorLayersAffectMassCenter(ObjectHandle<Animator> animator, bool affect)
        {
            if (animator)
                animator.value.layersAffectMassCenter = affect;
        }
        private static float GetAnimatorLeftFeetBottomHeight(ObjectHandle<Animator> animator) => animator ? animator.value.leftFeetBottomHeight : 0f;
        private static float GetAnimatorRightFeetBottomHeight(ObjectHandle<Animator> animator) => animator ? animator.value.rightFeetBottomHeight : 0f;
        private static bool GetAnimatorLogWarnings(ObjectHandle<Animator> animator) => animator ? animator.value.logWarnings : true;
        private static void SetAnimatorLogWarnings(ObjectHandle<Animator> animator, bool log)
        {
            if (animator)
                animator.value.logWarnings = log;
        }
        private static bool GetAnimatorFireEvents(ObjectHandle<Animator> animator) => animator ? animator.value.fireEvents : true;
        private static void SetAnimatorFireEvents(ObjectHandle<Animator> animator, bool fire)
        {
            if (animator)
                animator.value.fireEvents = fire;
        }
        private static bool DoesAnimatorKeepStateOnDisable(ObjectHandle<Animator> animator) => animator ? animator.value.keepAnimatorStateOnDisable : false;
        private static void KeepAnimatorStateOnDisable(ObjectHandle<Animator> animator, bool keep)
        {
            if (animator)
                animator.value.keepAnimatorStateOnDisable = keep;
        }

        // Animator Parameter API

        private static float GetAnimatorFloat(ObjectHandle<Animator> animator, int id) => animator ? animator.value.GetFloat(id) : 0f;
        private static void SetAnimatorFloat(ObjectHandle<Animator> animator, int id, float value, float dampTime = 0f, float deltaTime = 0f)
        {
            if (!animator)
                return;
            if (dampTime > 0f)
                animator.value.SetFloat(id, value, dampTime, deltaTime > 0f ? deltaTime : Time.deltaTime);
            else
                animator.value.SetFloat(id, value);
        }
        private static bool GetAnimatorBool(ObjectHandle<Animator> animator, int id) => animator ? animator.value.GetBool(id) : false;
        private static void SetAnimatorBool(ObjectHandle<Animator> animator, int id, bool value)
        {
            if (animator)
                animator.value.SetBool(id, value);
        }
        private static int GetAnimatorInteger(ObjectHandle<Animator> animator, int id) => animator ? animator.value.GetInteger(id) : 0;
        private static void SetAnimatorInteger(ObjectHandle<Animator> animator, int id, int value)
        {
            if (animator)
                animator.value.SetInteger(id, value);
        }
        private static void SetAnimatorTrigger(ObjectHandle<Animator> animator, int id)
        {
            if (animator)
                animator.value.SetTrigger(id);
        }
        private static void ResetAnimatorTrigger(ObjectHandle<Animator> animator, int id)
        {
            if (animator)
                animator.value.ResetTrigger(id);
        }
        private static bool IsAnimatorParameterControlledByCurve(ObjectHandle<Animator> animator, int id) => animator ? animator.value.IsParameterControlledByCurve(id) : false;
        private static int GetAnimatorParameterCount(ObjectHandle<Animator> animator) => animator ? animator.value.parameterCount : 0;

        // Animator Layer API

        private static int GetAnimatorLayerCount(ObjectHandle<Animator> animator) => animator ? animator.value.layerCount : 0;
        private static String8 GetAnimatorLayerName(ObjectHandle<Animator> animator, int layerIndex, Allocator allocator)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return new String8(animator.value.GetLayerName(layerIndex), allocator);
            return default;
        }
        private static int GetAnimatorLayerIndex(ObjectHandle<Animator> animator, String8 layerName) => animator ? animator.value.GetLayerIndex(layerName.ToString()) : -1;
        private static float GetAnimatorLayerWeight(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetLayerWeight(layerIndex);
            return 0f;
        }
        private static void SetAnimatorLayerWeight(ObjectHandle<Animator> animator, int layerIndex, float weight)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                animator.value.SetLayerWeight(layerIndex, weight);
        }

        // Animator State API

        private static AnimatorStateInfo GetAnimatorCurrentState(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetCurrentAnimatorStateInfo(layerIndex);
            return default;
        }
        private static AnimatorStateInfo GetAnimatorNextState(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetNextAnimatorStateInfo(layerIndex);
            return default;
        }
        private static bool IsAnimatorInTransition(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.IsInTransition(layerIndex);
            return false;
        }
        private static AnimatorTransitionInfo GetAnimatorTransitionInfo(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetAnimatorTransitionInfo(layerIndex);
            return default;
        }
        private static int GetAnimatorCurrentClipInfoCount(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetCurrentAnimatorClipInfoCount(layerIndex);
            return 0;
        }
        private static int GetAnimatorNextClipInfoCount(ObjectHandle<Animator> animator, int layerIndex)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.GetNextAnimatorClipInfoCount(layerIndex);
            return 0;
        }
        private static Slice<AnimatorClipInfo> GetAnimatorCurrentClipInfo(ObjectHandle<Animator> animator, int layerIndex, Allocator allocator)
        {
            if (!animator || layerIndex < 0 || layerIndex >= animator.value.layerCount)
                return default;
            BindingsHelper.tempAnimatorClipInfoList.Clear();
            animator.value.GetCurrentAnimatorClipInfo(layerIndex, BindingsHelper.tempAnimatorClipInfoList);
            var slice = new Slice<AnimatorClipInfo>(BindingsHelper.tempAnimatorClipInfoList.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempAnimatorClipInfoList.Count; i++)
                slice.ptr[i] = BindingsHelper.tempAnimatorClipInfoList[i];
            return slice;
        }
        private static Slice<AnimatorClipInfo> GetAnimatorNextClipInfo(ObjectHandle<Animator> animator, int layerIndex, Allocator allocator)
        {
            if (!animator || layerIndex < 0 || layerIndex >= animator.value.layerCount)
                return default;
            BindingsHelper.tempAnimatorClipInfoList.Clear();
            animator.value.GetNextAnimatorClipInfo(layerIndex, BindingsHelper.tempAnimatorClipInfoList);
            var slice = new Slice<AnimatorClipInfo>(BindingsHelper.tempAnimatorClipInfoList.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempAnimatorClipInfoList.Count; i++)
                slice.ptr[i] = BindingsHelper.tempAnimatorClipInfoList[i];
            return slice;
        }

        // Animator Playback API

        private static void PlayAnimator(ObjectHandle<Animator> animator, int stateNameHash, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            if (animator)
                animator.value.Play(stateNameHash, layer, normalizedTime);
        }
        private static void PlayAnimatorInFixedTime(ObjectHandle<Animator> animator, int stateNameHash, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            if (animator)
                animator.value.PlayInFixedTime(stateNameHash, layer, fixedTime);
        }
        private static void CrossFadeAnimator(ObjectHandle<Animator> animator, int stateNameHash, float normalizedTransitionDuration, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            if (animator)
                animator.value.CrossFade(stateNameHash, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime);
        }
        private static void CrossFadeAnimatorInFixedTime(ObjectHandle<Animator> animator, int stateNameHash, float fixedTransitionDuration, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            if (animator)
                animator.value.CrossFadeInFixedTime(stateNameHash, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime);
        }
        private static void UpdateAnimator(ObjectHandle<Animator> animator, float deltaTime)
        {
            if (animator)
                animator.value.Update(deltaTime);
        }
        private static void RebindAnimator(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.Rebind();
        }
        private static void ApplyAnimatorBuiltinRootMotion(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.ApplyBuiltinRootMotion();
        }
        private static void StartAnimatorPlayback(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.StartPlayback();
        }
        private static void StopAnimatorPlayback(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.StopPlayback();
        }
        private static void StartAnimatorRecording(ObjectHandle<Animator> animator, int frameCount)
        {
            if (animator)
                animator.value.StartRecording(frameCount);
        }
        private static void StopAnimatorRecording(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.StopRecording();
        }
        private static bool HasAnimatorState(ObjectHandle<Animator> animator, int layerIndex, int stateID)
        {
            if (animator && layerIndex >= 0 && layerIndex < animator.value.layerCount)
                return animator.value.HasState(layerIndex, stateID);
            return false;
        }
        private static int GetAnimatorStringHash(String8 name) => Animator.StringToHash(name.ToString());

        // Animator IK API

        private static void SetAnimatorLookAtPosition(ObjectHandle<Animator> animator, Vector3 lookAtPosition)
        {
            if (animator)
                animator.value.SetLookAtPosition(lookAtPosition);
        }
        private static void SetAnimatorLookAtWeight(ObjectHandle<Animator> animator, float weight, float bodyWeight = 0f, float headWeight = 1f, float eyesWeight = 0f, float clampWeight = 0.5f)
        {
            if (animator)
                animator.value.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        }
        private static void SetAnimatorIKPosition(ObjectHandle<Animator> animator, AvatarIKGoal goal, Vector3 goalPosition)
        {
            if (animator)
                animator.value.SetIKPosition(goal, goalPosition);
        }
        private static Vector3 GetAnimatorIKPosition(ObjectHandle<Animator> animator, AvatarIKGoal goal) => animator ? animator.value.GetIKPosition(goal) : default;
        private static void SetAnimatorIKPositionWeight(ObjectHandle<Animator> animator, AvatarIKGoal goal, float value)
        {
            if (animator)
                animator.value.SetIKPositionWeight(goal, value);
        }
        private static float GetAnimatorIKPositionWeight(ObjectHandle<Animator> animator, AvatarIKGoal goal) => animator ? animator.value.GetIKPositionWeight(goal) : 0f;
        private static void SetAnimatorIKRotation(ObjectHandle<Animator> animator, AvatarIKGoal goal, Quaternion goalRotation)
        {
            if (animator)
                animator.value.SetIKRotation(goal, goalRotation);
        }
        private static Quaternion GetAnimatorIKRotation(ObjectHandle<Animator> animator, AvatarIKGoal goal) => animator ? animator.value.GetIKRotation(goal) : Quaternion.identity;
        private static void SetAnimatorIKRotationWeight(ObjectHandle<Animator> animator, AvatarIKGoal goal, float value)
        {
            if (animator)
                animator.value.SetIKRotationWeight(goal, value);
        }
        private static float GetAnimatorIKRotationWeight(ObjectHandle<Animator> animator, AvatarIKGoal goal) => animator ? animator.value.GetIKRotationWeight(goal) : 0f;
        private static void SetAnimatorIKHintPosition(ObjectHandle<Animator> animator, AvatarIKHint hint, Vector3 hintPosition)
        {
            if (animator)
                animator.value.SetIKHintPosition(hint, hintPosition);
        }
        private static Vector3 GetAnimatorIKHintPosition(ObjectHandle<Animator> animator, AvatarIKHint hint) => animator ? animator.value.GetIKHintPosition(hint) : default;
        private static void SetAnimatorIKHintPositionWeight(ObjectHandle<Animator> animator, AvatarIKHint hint, float value)
        {
            if (animator)
                animator.value.SetIKHintPositionWeight(hint, value);
        }
        private static float GetAnimatorIKHintPositionWeight(ObjectHandle<Animator> animator, AvatarIKHint hint) => animator ? animator.value.GetIKHintPositionWeight(hint) : 0f;

        // Animator Body/Bone API

        private static void SetAnimatorBoneLocalRotation(ObjectHandle<Animator> animator, HumanBodyBones humanBoneId, Quaternion rotation)
        {
            if (animator)
                animator.value.SetBoneLocalRotation(humanBoneId, rotation);
        }
        private static ObjectHandle<Transform> GetAnimatorBoneTransform(ObjectHandle<Animator> animator, HumanBodyBones humanBoneId) => animator ? animator.value.GetBoneTransform(humanBoneId) : default;
        private static Vector3 GetAnimatorBodyPosition(ObjectHandle<Animator> animator) => animator ? animator.value.bodyPosition : default;
        private static void SetAnimatorBodyPosition(ObjectHandle<Animator> animator, Vector3 position)
        {
            if (animator)
                animator.value.bodyPosition = position;
        }
        private static Quaternion GetAnimatorBodyRotation(ObjectHandle<Animator> animator) => animator ? animator.value.bodyRotation : Quaternion.identity;
        private static void SetAnimatorBodyRotation(ObjectHandle<Animator> animator, Quaternion rotation)
        {
            if (animator)
                animator.value.bodyRotation = rotation;
        }
        private static float GetAnimatorGravityWeight(ObjectHandle<Animator> animator) => animator ? animator.value.gravityWeight : 0f;
        private static Vector3 GetAnimatorPivotPosition(ObjectHandle<Animator> animator) => animator ? animator.value.pivotPosition : default;
        private static float GetAnimatorPivotWeight(ObjectHandle<Animator> animator) => animator ? animator.value.pivotWeight : 0f;

        // Animator Matching API

        private static void MatchAnimatorTarget(ObjectHandle<Animator> animator, Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime = 1f, bool completeMatch = true)
        {
            if (animator)
                animator.value.MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime, completeMatch);
        }
        private static void InterruptAnimatorMatchTarget(ObjectHandle<Animator> animator, bool completeMatch = true)
        {
            if (animator)
                animator.value.InterruptMatchTarget(completeMatch);
        }
        private static bool IsAnimatorMatchingTarget(ObjectHandle<Animator> animator) => animator ? animator.value.isMatchingTarget : false;

        // Animator Misc API

        private static void WriteAnimatorDefaultValues(ObjectHandle<Animator> animator)
        {
            if (animator)
                animator.value.WriteDefaultValues();
        }

        // RuntimeAnimatorController API

        private static Slice<ObjectHandle<AnimationClip>> GetRuntimeAnimatorControllerAnimationClips(ObjectHandle<RuntimeAnimatorController> controller, Allocator allocator)
        {
            if (!controller)
                return default;
            var clips = controller.value.animationClips;
            var slice = new Slice<ObjectHandle<AnimationClip>>(clips.Length, allocator);
            for (var i = 0; i < clips.Length; i++)
                slice.ptr[i] = clips[i];
            return slice;
        }

        // AnimatorOverrideController API

        private static ObjectHandle<AnimatorOverrideController> CreateAnimatorOverrideController(ObjectHandle<RuntimeAnimatorController> controller)
        {
            if (!controller)
                return default;
            return new AnimatorOverrideController(controller);
        }
        private static ObjectHandle<RuntimeAnimatorController> GetAnimatorOverrideControllerBase(ObjectHandle<AnimatorOverrideController> overrideController) => overrideController ? overrideController.value.runtimeAnimatorController : default;
        private static void SetAnimatorOverrideControllerBase(ObjectHandle<AnimatorOverrideController> overrideController, ObjectHandle<RuntimeAnimatorController> controller)
        {
            if (overrideController)
                overrideController.value.runtimeAnimatorController = controller;
        }
        private static ObjectHandle<AnimationClip> GetAnimatorOverrideClipByName(ObjectHandle<AnimatorOverrideController> overrideController, String8 name)
        {
            if (!overrideController)
                return default;
            return overrideController.value[name.ToString()];
        }
        private static void SetAnimatorOverrideClipByName(ObjectHandle<AnimatorOverrideController> overrideController, String8 name, ObjectHandle<AnimationClip> clip)
        {
            if (overrideController)
                overrideController.value[name.ToString()] = clip;
        }
        private static ObjectHandle<AnimationClip> GetAnimatorOverrideClipByOriginal(ObjectHandle<AnimatorOverrideController> overrideController, ObjectHandle<AnimationClip> originalClip)
        {
            if (!overrideController || !originalClip)
                return default;
            return overrideController.value[originalClip];
        }
        private static void SetAnimatorOverrideClipByOriginal(ObjectHandle<AnimatorOverrideController> overrideController, ObjectHandle<AnimationClip> originalClip, ObjectHandle<AnimationClip> overrideClip)
        {
            if (overrideController && originalClip)
                overrideController.value[originalClip] = overrideClip;
        }
    }
}
