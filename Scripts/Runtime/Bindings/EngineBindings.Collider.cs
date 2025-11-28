using UnityEngine;

namespace OdinInterop
{
    internal static partial class EngineBindings
    {
        // Collider API

        private static bool IsColliderEnabled(ObjectHandle<Collider> collider) => collider ? collider.value.enabled : false;
        private static void SetColliderEnabled(ObjectHandle<Collider> collider, bool enabled)
        {
            if (collider)
                collider.value.enabled = enabled;
        }
        private static bool IsColliderTrigger(ObjectHandle<Collider> collider) => collider ? collider.value.isTrigger : false;
        private static void SetColliderTrigger(ObjectHandle<Collider> collider, bool isTrigger)
        {
            if (collider)
                collider.value.isTrigger = isTrigger;
        }
        private static float GetColliderContactOffset(ObjectHandle<Collider> collider) => collider ? collider.value.contactOffset : 0f;
        private static void SetColliderContactOffset(ObjectHandle<Collider> collider, float offset)
        {
            if (collider)
                collider.value.contactOffset = offset;
        }
        private static Bounds GetColliderBounds(ObjectHandle<Collider> collider) => collider ? collider.value.bounds : default;
        private static ObjectHandle<PhysicsMaterial> GetColliderMaterial(ObjectHandle<Collider> collider) => collider ? collider.value.material : default;
        private static void SetColliderMaterial(ObjectHandle<Collider> collider, ObjectHandle<PhysicsMaterial> material)
        {
            if (collider)
                collider.value.material = material;
        }
        private static ObjectHandle<PhysicsMaterial> GetColliderSharedMaterial(ObjectHandle<Collider> collider) => collider ? collider.value.sharedMaterial : default;
        private static void SetColliderSharedMaterial(ObjectHandle<Collider> collider, ObjectHandle<PhysicsMaterial> material)
        {
            if (collider)
                collider.value.sharedMaterial = material;
        }
        private static Vector3 ClosestPointOnCollider(ObjectHandle<Collider> collider, Vector3 position) => collider ? collider.value.ClosestPoint(position) : position;
        private static Vector3 ClosestPointOnColliderBounds(ObjectHandle<Collider> collider, Vector3 position) => collider ? collider.value.ClosestPointOnBounds(position) : position;
        private static bool RaycastCollider(ObjectHandle<Collider> collider, Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            if (collider)
                return collider.value.Raycast(ray, out hitInfo, maxDistance);
            hitInfo = default;
            return false;
        }
        private static ObjectHandle<Rigidbody> GetColliderAttachedRigidbody(ObjectHandle<Collider> collider) => collider ? collider.value.attachedRigidbody : default;

        // BoxCollider API

        private static Vector3 GetBoxColliderCenter(ObjectHandle<BoxCollider> boxCollider) => boxCollider ? boxCollider.value.center : default;
        private static void SetBoxColliderCenter(ObjectHandle<BoxCollider> boxCollider, Vector3 center)
        {
            if (boxCollider)
                boxCollider.value.center = center;
        }
        private static Vector3 GetBoxColliderSize(ObjectHandle<BoxCollider> boxCollider) => boxCollider ? boxCollider.value.size : default;
        private static void SetBoxColliderSize(ObjectHandle<BoxCollider> boxCollider, Vector3 size)
        {
            if (boxCollider)
                boxCollider.value.size = size;
        }

        // SphereCollider API

        private static Vector3 GetSphereColliderCenter(ObjectHandle<SphereCollider> sphereCollider) => sphereCollider ? sphereCollider.value.center : default;
        private static void SetSphereColliderCenter(ObjectHandle<SphereCollider> sphereCollider, Vector3 center)
        {
            if (sphereCollider)
                sphereCollider.value.center = center;
        }
        private static float GetSphereColliderRadius(ObjectHandle<SphereCollider> sphereCollider) => sphereCollider ? sphereCollider.value.radius : 0f;
        private static void SetSphereColliderRadius(ObjectHandle<SphereCollider> sphereCollider, float radius)
        {
            if (sphereCollider)
                sphereCollider.value.radius = radius;
        }

        // CapsuleCollider API

        private static Vector3 GetCapsuleColliderCenter(ObjectHandle<CapsuleCollider> capsuleCollider) => capsuleCollider ? capsuleCollider.value.center : default;
        private static void SetCapsuleColliderCenter(ObjectHandle<CapsuleCollider> capsuleCollider, Vector3 center)
        {
            if (capsuleCollider)
                capsuleCollider.value.center = center;
        }
        private static float GetCapsuleColliderRadius(ObjectHandle<CapsuleCollider> capsuleCollider) => capsuleCollider ? capsuleCollider.value.radius : 0f;
        private static void SetCapsuleColliderRadius(ObjectHandle<CapsuleCollider> capsuleCollider, float radius)
        {
            if (capsuleCollider)
                capsuleCollider.value.radius = radius;
        }
        private static float GetCapsuleColliderHeight(ObjectHandle<CapsuleCollider> capsuleCollider) => capsuleCollider ? capsuleCollider.value.height : 0f;
        private static void SetCapsuleColliderHeight(ObjectHandle<CapsuleCollider> capsuleCollider, float height)
        {
            if (capsuleCollider)
                capsuleCollider.value.height = height;
        }
        private static int GetCapsuleColliderDirection(ObjectHandle<CapsuleCollider> capsuleCollider) => capsuleCollider ? capsuleCollider.value.direction : 0;
        private static void SetCapsuleColliderDirection(ObjectHandle<CapsuleCollider> capsuleCollider, int direction)
        {
            if (capsuleCollider)
                capsuleCollider.value.direction = direction;
        }

        // MeshCollider API

        private static ObjectHandle<Mesh> GetMeshColliderSharedMesh(ObjectHandle<MeshCollider> meshCollider) => meshCollider ? meshCollider.value.sharedMesh : default;
        private static void SetMeshColliderSharedMesh(ObjectHandle<MeshCollider> meshCollider, ObjectHandle<Mesh> mesh)
        {
            if (meshCollider)
                meshCollider.value.sharedMesh = mesh;
        }
        private static bool IsMeshColliderConvex(ObjectHandle<MeshCollider> meshCollider) => meshCollider ? meshCollider.value.convex : false;
        private static void SetMeshColliderConvex(ObjectHandle<MeshCollider> meshCollider, bool convex)
        {
            if (meshCollider)
                meshCollider.value.convex = convex;
        }
        private static MeshColliderCookingOptions GetMeshColliderCookingOptions(ObjectHandle<MeshCollider> meshCollider) => meshCollider ? meshCollider.value.cookingOptions : default;
        private static void SetMeshColliderCookingOptions(ObjectHandle<MeshCollider> meshCollider, MeshColliderCookingOptions options)
        {
            if (meshCollider)
                meshCollider.value.cookingOptions = options;
        }

        // PhysicsMaterial API

        private static ObjectHandle<PhysicsMaterial> CreatePhysicsMaterial(String8 name)
        {
            return new PhysicsMaterial(name.ToString());
        }
        private static float GetPhysicsMaterialDynamicFriction(ObjectHandle<PhysicsMaterial> material) => material ? material.value.dynamicFriction : 0f;
        private static void SetPhysicsMaterialDynamicFriction(ObjectHandle<PhysicsMaterial> material, float friction)
        {
            if (material)
                material.value.dynamicFriction = friction;
        }
        private static float GetPhysicsMaterialStaticFriction(ObjectHandle<PhysicsMaterial> material) => material ? material.value.staticFriction : 0f;
        private static void SetPhysicsMaterialStaticFriction(ObjectHandle<PhysicsMaterial> material, float friction)
        {
            if (material)
                material.value.staticFriction = friction;
        }
        private static float GetPhysicsMaterialBounciness(ObjectHandle<PhysicsMaterial> material) => material ? material.value.bounciness : 0f;
        private static void SetPhysicsMaterialBounciness(ObjectHandle<PhysicsMaterial> material, float bounciness)
        {
            if (material)
                material.value.bounciness = bounciness;
        }
        private static PhysicsMaterialCombine GetPhysicsMaterialFrictionCombine(ObjectHandle<PhysicsMaterial> material) => material ? material.value.frictionCombine : default;
        private static void SetPhysicsMaterialFrictionCombine(ObjectHandle<PhysicsMaterial> material, PhysicsMaterialCombine combine)
        {
            if (material)
                material.value.frictionCombine = combine;
        }
        private static PhysicsMaterialCombine GetPhysicsMaterialBounceCombine(ObjectHandle<PhysicsMaterial> material) => material ? material.value.bounceCombine : default;
        private static void SetPhysicsMaterialBounceCombine(ObjectHandle<PhysicsMaterial> material, PhysicsMaterialCombine combine)
        {
            if (material)
                material.value.bounceCombine = combine;
        }

    }
}