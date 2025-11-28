using UnityEngine;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // Physics API

        private static Vector3 GetPhysicsGravity() => Physics.gravity;
        private static void SetPhysicsGravity(Vector3 gravity)
        {
            Physics.gravity = gravity;
        }
        private static float GetPhysicsDefaultContactOffset() => Physics.defaultContactOffset;
        private static void SetPhysicsDefaultContactOffset(float offset)
        {
            Physics.defaultContactOffset = offset;
        }
        private static float GetPhysicsSleepThreshold() => Physics.sleepThreshold;
        private static void SetPhysicsSleepThreshold(float threshold)
        {
            Physics.sleepThreshold = threshold;
        }
        private static float GetPhysicsBounceThreshold() => Physics.bounceThreshold;
        private static void SetPhysicsBounceThreshold(float threshold)
        {
            Physics.bounceThreshold = threshold;
        }
        private static int GetPhysicsDefaultSolverIterations() => Physics.defaultSolverIterations;
        private static void SetPhysicsDefaultSolverIterations(int iterations)
        {
            Physics.defaultSolverIterations = iterations;
        }
        private static int GetPhysicsDefaultSolverVelocityIterations() => Physics.defaultSolverVelocityIterations;
        private static void SetPhysicsDefaultSolverVelocityIterations(int iterations)
        {
            Physics.defaultSolverVelocityIterations = iterations;
        }
        private static float GetPhysicsDefaultMaxAngularSpeed() => Physics.defaultMaxAngularSpeed;
        private static void SetPhysicsDefaultMaxAngularSpeed(float speed)
        {
            Physics.defaultMaxAngularSpeed = speed;
        }
        private static float GetPhysicsDefaultMaxDepenetrationVelocity() => Physics.defaultMaxDepenetrationVelocity;
        private static void SetPhysicsDefaultMaxDepenetrationVelocity(float velocity)
        {
            Physics.defaultMaxDepenetrationVelocity = velocity;
        }
        private static bool GetPhysicsAutoSyncTransforms() => Physics.autoSyncTransforms;
        private static void SetPhysicsAutoSyncTransforms(bool autoSync)
        {
            Physics.autoSyncTransforms = autoSync;
        }
        private static bool GetPhysicsReuseCollisionCallbacks() => Physics.reuseCollisionCallbacks;
        private static void SetPhysicsReuseCollisionCallbacks(bool reuse)
        {
            Physics.reuseCollisionCallbacks = reuse;
        }
        private static PhysicsScene GetPhysicsDefaultPhysicsScene() => Physics.defaultPhysicsScene;
        private static void SimulatePhysics(float step)
        {
            Physics.Simulate(step);
        }
        private static void SyncPhysicsTransforms()
        {
            Physics.SyncTransforms();
        }
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
        private static void IgnoreCollision(ObjectHandle<Collider> collider1, ObjectHandle<Collider> collider2, bool ignore = default)
        {
            if (collider1 && collider2)
                Physics.IgnoreCollision(collider1, collider2, ignore);
        }
        private static void IgnoreLayerCollision(int layer1, int layer2, bool ignore = default)
        {
            Physics.IgnoreLayerCollision(layer1, layer2, ignore);
        }
        private static bool GetIgnoreLayerCollision(int layer1, int layer2) => Physics.GetIgnoreLayerCollision(layer1, layer2);
        private static bool GetIgnoreCollision(ObjectHandle<Collider> collider1, ObjectHandle<Collider> collider2)
        {
            if (collider1 && collider2)
                return Physics.GetIgnoreCollision(collider1, collider2);
            return false;
        }

        // Raycast APIs

        private static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
        private static bool RaycastWithHit(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        private static bool RaycastRay(Ray ray, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Raycast(ray, maxDistance, layerMask, queryTriggerInteraction);
        private static bool RaycastRayWithHit(Ray ray, out RaycastHit hitInfo, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Raycast(ray, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        private static bool Linecast(Vector3 start, Vector3 end, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Linecast(start, end, layerMask, queryTriggerInteraction);
        private static bool LinecastWithHit(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);

        private static Slice<RaycastHit> RaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var hits = Physics.RaycastAll(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
            var slice = new Slice<RaycastHit>(hits.Length, allocator);
            for (var i = 0; i < hits.Length; i++)
                slice.ptr[i] = hits[i];
            return slice;
        }
        private static Slice<RaycastHit> RaycastAllRay(Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var hits = Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
            var slice = new Slice<RaycastHit>(hits.Length, allocator);
            for (var i = 0; i < hits.Length; i++)
                slice.ptr[i] = hits[i];
            return slice;
        }
        private static int RaycastNonAlloc(Vector3 origin, Vector3 direction, Slice<RaycastHit> results, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new RaycastHit[(int)results.len];
            var count = Physics.RaycastNonAlloc(origin, direction, arr, maxDistance, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }
        private static int RaycastNonAllocRay(Ray ray, Slice<RaycastHit> results, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new RaycastHit[(int)results.len];
            var count = Physics.RaycastNonAlloc(ray, arr, maxDistance, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        // Cast APIs

        private static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
        private static bool CapsuleCastWithHit(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        private static Slice<RaycastHit> CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var hits = Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
            var slice = new Slice<RaycastHit>(hits.Length, allocator);
            for (var i = 0; i < hits.Length; i++)
                slice.ptr[i] = hits[i];
            return slice;
        }
        private static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, Slice<RaycastHit> results, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new RaycastHit[(int)results.len];
            var count = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, arr, maxDistance, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        private static bool SphereCastWithHit(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        private static bool SphereCastRay(Ray ray, float radius, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.SphereCast(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
        private static bool SphereCastRayWithHit(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        private static Slice<RaycastHit> SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var hits = Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
            var slice = new Slice<RaycastHit>(hits.Length, allocator);
            for (var i = 0; i < hits.Length; i++)
                slice.ptr[i] = hits[i];
            return slice;
        }
        private static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, Slice<RaycastHit> results, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new RaycastHit[(int)results.len];
            var count = Physics.SphereCastNonAlloc(origin, radius, direction, arr, maxDistance, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        private static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation = default, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask, queryTriggerInteraction);
        private static bool BoxCastWithHit(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation = default, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
        private static Slice<RaycastHit> BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var hits = Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask, queryTriggerInteraction);
            var slice = new Slice<RaycastHit>(hits.Length, allocator);
            for (var i = 0; i < hits.Length; i++)
                slice.ptr[i] = hits[i];
            return slice;
        }
        private static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, Slice<RaycastHit> results, Quaternion orientation = default, float maxDistance = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new RaycastHit[(int)results.len];
            var count = Physics.BoxCastNonAlloc(center, halfExtents, direction, arr, orientation, maxDistance, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        // Overlap APIs

        private static bool CheckSphere(Vector3 position, float radius, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction);
        private static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.CheckCapsule(start, end, radius, layerMask, queryTriggerInteraction);
        private static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion orientation = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default) => Physics.CheckBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);

        private static Slice<ObjectHandle<Collider>> OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var colliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);
            var slice = new Slice<ObjectHandle<Collider>>(colliders.Length, allocator);
            for (var i = 0; i < colliders.Length; i++)
                slice.ptr[i] = colliders[i];
            return slice;
        }
        private static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Slice<ObjectHandle<Collider>> results, Quaternion orientation = default, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new Collider[(int)results.len];
            var count = Physics.OverlapBoxNonAlloc(center, halfExtents, arr, orientation, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        private static Slice<ObjectHandle<Collider>> OverlapSphere(Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var colliders = Physics.OverlapSphere(position, radius, layerMask, queryTriggerInteraction);
            var slice = new Slice<ObjectHandle<Collider>>(colliders.Length, allocator);
            for (var i = 0; i < colliders.Length; i++)
                slice.ptr[i] = colliders[i];
            return slice;
        }
        private static int OverlapSphereNonAlloc(Vector3 position, float radius, Slice<ObjectHandle<Collider>> results, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new Collider[(int)results.len];
            var count = Physics.OverlapSphereNonAlloc(position, radius, arr, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        private static Slice<ObjectHandle<Collider>> OverlapCapsule(Vector3 point0, Vector3 point1, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction, Allocator allocator)
        {
            var colliders = Physics.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction);
            var slice = new Slice<ObjectHandle<Collider>>(colliders.Length, allocator);
            for (var i = 0; i < colliders.Length; i++)
                slice.ptr[i] = colliders[i];
            return slice;
        }
        private static int OverlapCapsuleNonAlloc(Vector3 point0, Vector3 point1, float radius, Slice<ObjectHandle<Collider>> results, int layerMask = default, QueryTriggerInteraction queryTriggerInteraction = default)
        {
            var arr = new Collider[(int)results.len];
            var count = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, arr, layerMask, queryTriggerInteraction);
            for (var i = 0; i < count; i++)
                results.ptr[i] = arr[i];
            return count;
        }

        private static void BakeMesh(ObjectHandle<Mesh> mesh, bool convex)
        {
            if (mesh)
                Physics.BakeMesh(mesh.id, convex);
        }
    }
}
