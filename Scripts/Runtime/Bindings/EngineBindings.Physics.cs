using UnityEngine;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // Physics API

        private static Vector3 GetPhysicsGravity() => Physics.gravity;
        private static void SetPhysicsGravity(Vector3 gravity) => Physics.gravity = gravity;
        private static float GetPhysicsDefaultContactOffset() => Physics.defaultContactOffset;
        private static void SetPhysicsDefaultContactOffset(float offset) => Physics.defaultContactOffset = offset;
        private static float GetPhysicsSleepThreshold() => Physics.sleepThreshold;
        private static void SetPhysicsSleepThreshold(float threshold) => Physics.sleepThreshold = threshold;
        private static float GetPhysicsBounceThreshold() => Physics.bounceThreshold;
        private static void SetPhysicsBounceThreshold(float threshold) => Physics.bounceThreshold = threshold;
        private static int GetPhysicsDefaultSolverIterations() => Physics.defaultSolverIterations;
        private static void SetPhysicsDefaultSolverIterations(int iterations) => Physics.defaultSolverIterations = iterations;
        private static int GetPhysicsDefaultSolverVelocityIterations() => Physics.defaultSolverVelocityIterations;
        private static void SetPhysicsDefaultSolverVelocityIterations(int iterations) => Physics.defaultSolverVelocityIterations = iterations;
        private static float GetPhysicsDefaultMaxAngularSpeed() => Physics.defaultMaxAngularSpeed;
        private static void SetPhysicsDefaultMaxAngularSpeed(float speed) => Physics.defaultMaxAngularSpeed = speed;
        private static float GetPhysicsDefaultMaxDepenetrationVelocity() => Physics.defaultMaxDepenetrationVelocity;
        private static void SetPhysicsDefaultMaxDepenetrationVelocity(float velocity) => Physics.defaultMaxDepenetrationVelocity = velocity;
        private static bool GetPhysicsAutoSyncTransforms() => Physics.autoSyncTransforms;
        private static void SetPhysicsAutoSyncTransforms(bool autoSync) => Physics.autoSyncTransforms = autoSync;
        private static bool GetPhysicsReuseCollisionCallbacks() => Physics.reuseCollisionCallbacks;
        private static void SetPhysicsReuseCollisionCallbacks(bool reuse) => Physics.reuseCollisionCallbacks = reuse;
        private static PhysicsScene GetPhysicsDefaultPhysicsScene() => Physics.defaultPhysicsScene;
        private static void SimulatePhysics(float step) => Physics.Simulate(step);
        private static void SyncPhysicsTransforms() => Physics.SyncTransforms();
        private static bool ComputePenetration(ObjectHandle<Collider> colliderA, Vector3 positionA, Quaternion rotationA, ObjectHandle<Collider> colliderB, Vector3 positionB, Quaternion rotationB, out Vector3 direction, out float distance)
        {
            if (colliderA && colliderB)
                return Physics.ComputePenetration(colliderA, positionA, rotationA, colliderB, positionB, rotationB, out direction, out distance);
            direction = default;
            distance = 0f;
            return false;
        }
        private static Vector3 ClosestPoint(Vector3 point, ObjectHandle<Collider> collider, Vector3 position, Quaternion rotation)
        {
            if (collider)
                return Physics.ClosestPoint(point, collider, position, rotation);
            return point;
        }
        private static void IgnoreCollision(ObjectHandle<Collider> collider1, ObjectHandle<Collider> collider2, bool ignore = true)
        {
            if (collider1 && collider2)
                Physics.IgnoreCollision(collider1, collider2, ignore);
        }
        private static void IgnoreLayerCollision(int layer1, int layer2, bool ignore = true) => Physics.IgnoreLayerCollision(layer1, layer2, ignore);
        private static bool GetIgnoreLayerCollision(int layer1, int layer2) => Physics.GetIgnoreLayerCollision(layer1, layer2);
        private static bool GetIgnoreCollision(ObjectHandle<Collider> collider1, ObjectHandle<Collider> collider2)
        {
            if (collider1 && collider2)
                return Physics.GetIgnoreCollision(collider1, collider2);
            return false;
        }

        private static bool Linecast(LayerMask layerMask, Vector3 start, Vector3 end, out RaycastHit hitInfo, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);
        private static Slice<RaycastHit> LinecastAll(LayerMask layerMask, Vector3 start, Vector3 end, Allocator allocator, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var hitCount = Physics.RaycastNonAlloc(new Ray(start, end - start), BindingsHelper.raycastHits, (end - start).magnitude, layerMask, queryTriggerInteraction);
            var hitInfos = new Slice<RaycastHit>(hitCount, allocator);
            for (var i = 0; i < hitCount; i++)
                hitInfos.ptr[i] = BindingsHelper.raycastHits[i];
            return hitInfos;
        }

        private static bool CapsuleCast(LayerMask layerMask, Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

        private static Slice<RaycastHit> CapsuleCastAll(LayerMask layerMask, Vector3 point1, Vector3 point2, float radius, Vector3 direction, Allocator allocator, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var hitCount = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, BindingsHelper.raycastHits, maxDistance, layerMask, queryTriggerInteraction);
            var hitInfos = new Slice<RaycastHit>(hitCount, allocator);
            for (var i = 0; i < hitCount; i++)
                hitInfos.ptr[i] = BindingsHelper.raycastHits[i];
            return hitInfos;
        }

        private static bool SphereCast(LayerMask layerMask, Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

        private static Slice<RaycastHit> SphereCastAll(LayerMask layerMask, Vector3 origin, float radius, Vector3 direction, Allocator allocator, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var hitCount = Physics.SphereCastNonAlloc(origin, radius, direction, BindingsHelper.raycastHits, maxDistance, layerMask, queryTriggerInteraction);
            var hitInfos = new Slice<RaycastHit>(hitCount, allocator);
            for (var i = 0; i < hitCount; i++)
                hitInfos.ptr[i] = BindingsHelper.raycastHits[i];
            return hitInfos;
        }

        private static bool BoxCast(LayerMask layerMask, Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation = default, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);

        private static Slice<RaycastHit> BoxCastAll(LayerMask layerMask, Vector3 center, Vector3 halfExtents, Vector3 direction, Allocator allocator, Quaternion orientation = default, float maxDistance = float.MaxValue, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var hitCount = Physics.BoxCastNonAlloc(center, halfExtents, direction, BindingsHelper.raycastHits, orientation, maxDistance, layerMask, queryTriggerInteraction);
            var hitInfos = new Slice<RaycastHit>(hitCount, allocator);
            for (var i = 0; i < hitCount; i++)
                hitInfos.ptr[i] = BindingsHelper.raycastHits[i];
            return hitInfos;
        }

        private static bool CheckCapsule(LayerMask layerMask, Vector3 start, Vector3 end, float radius, QueryTriggerInteraction queryTriggerInteraction = default) =>
             Physics.CheckCapsule(start, end, radius, layerMask, queryTriggerInteraction);

        private static bool CheckSphere(LayerMask layerMask, Vector3 position, float radius, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction);

        private static bool CheckBox(LayerMask layerMask, Vector3 center, Vector3 halfExtents, Quaternion orientation = default, QueryTriggerInteraction queryTriggerInteraction = default) =>
            Physics.CheckBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);

        private static Slice<ObjectHandle<Collider>> OverlapCapsule(LayerMask layerMask, Vector3 point0, Vector3 point1, float radius, Allocator allocator, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var colliderCount = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, BindingsHelper.colliders, layerMask, queryTriggerInteraction);
            var colliders = new Slice<ObjectHandle<Collider>>(colliderCount, allocator);
            for (var i = 0; i < colliderCount; i++)
                colliders.ptr[i] = BindingsHelper.colliders[i];
            for (var i = 0; i < colliderCount; i++)
                BindingsHelper.colliders[i] = null; // null it out so gc doesn't keep old references
            return colliders;
        }

        private static Slice<ObjectHandle<Collider>> OverlapSphere(LayerMask layerMask, Vector3 position, float radius, Allocator allocator, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var colliderCount = Physics.OverlapSphereNonAlloc(position, radius, BindingsHelper.colliders, layerMask, queryTriggerInteraction);
            var colliders = new Slice<ObjectHandle<Collider>>(colliderCount, allocator);
            for (var i = 0; i < colliderCount; i++)
                colliders.ptr[i] = BindingsHelper.colliders[i];
            for (var i = 0; i < colliderCount; i++)
                BindingsHelper.colliders[i] = null; // null it out so gc doesn't keep old references
            return colliders;
        }

        private static Slice<ObjectHandle<Collider>> OverlapBox(LayerMask layerMask, Vector3 center, Vector3 halfExtents, Allocator allocator, Quaternion orientation = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var colliderCount = Physics.OverlapBoxNonAlloc(center, halfExtents, BindingsHelper.colliders, orientation, layerMask, queryTriggerInteraction);
            var colliders = new Slice<ObjectHandle<Collider>>(colliderCount, allocator);
            for (var i = 0; i < colliderCount; i++)
                colliders.ptr[i] = BindingsHelper.colliders[i];
            for (var i = 0; i < colliderCount; i++)
                BindingsHelper.colliders[i] = null; // null it out so gc doesn't keep old references
            return colliders;
        }

        private static void BakeMesh(ObjectHandle<Mesh> mesh, bool convex)
        {
            if (mesh)
                Physics.BakeMesh(mesh.id, convex);
        }
    }
}
