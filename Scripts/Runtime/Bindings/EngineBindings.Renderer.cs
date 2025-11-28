using UnityEngine;
using UnityEngine.Rendering;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // Renderer API

        private static bool IsRendererEnabled(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.enabled : false;
        private static void SetRendererEnabled(ObjectHandle<Renderer> renderer, bool enabled)
        {
            if (renderer)
                renderer.value.enabled = enabled;
        }
        private static bool IsRendererVisible(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.isVisible : false;
        private static Bounds GetRendererBounds(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.bounds : default;
        private static ObjectHandle<Material> GetRendererMaterial(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.material : default;
        private static void SetRendererMaterial(ObjectHandle<Renderer> renderer, ObjectHandle<Material> material)
        {
            if (renderer)
                renderer.value.material = material;
        }
        private static ObjectHandle<Material> GetRendererSharedMaterial(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.sharedMaterial : default;
        private static void SetRendererSharedMaterial(ObjectHandle<Renderer> renderer, ObjectHandle<Material> material)
        {
            if (renderer)
                renderer.value.sharedMaterial = material;
        }
        private static Slice<ObjectHandle<Material>> GetRendererMaterials(ObjectHandle<Renderer> renderer, Allocator allocator)
        {
            if (!renderer)
                return default;
            BindingsHelper.tempMaterialsList.Clear();
            renderer.value.GetMaterials(BindingsHelper.tempMaterialsList);
            var slice = new Slice<ObjectHandle<Material>>(BindingsHelper.tempMaterialsList.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempMaterialsList.Count; i++)
                slice.ptr[i] = BindingsHelper.tempMaterialsList[i];
            BindingsHelper.tempMaterialsList.Clear();
            return slice;
        }
        private static void SetRendererMaterials(ObjectHandle<Renderer> renderer, Slice<ObjectHandle<Material>> materials)
        {
            if (!renderer)
                return;
            BindingsHelper.tempMaterialsList.Clear();
            for (var i = 0; i < (int)materials.len; i++)
                BindingsHelper.tempMaterialsList.Add(materials.ptr[i]);
            renderer.value.SetMaterials(BindingsHelper.tempMaterialsList);
            BindingsHelper.tempMaterialsList.Clear();
        }
        private static Slice<ObjectHandle<Material>> GetRendererSharedMaterials(ObjectHandle<Renderer> renderer, Allocator allocator)
        {
            if (!renderer)
                return default;
            BindingsHelper.tempMaterialsList.Clear();
            renderer.value.GetSharedMaterials(BindingsHelper.tempMaterialsList);
            var slice = new Slice<ObjectHandle<Material>>(BindingsHelper.tempMaterialsList.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempMaterialsList.Count; i++)
                slice.ptr[i] = BindingsHelper.tempMaterialsList[i];
            BindingsHelper.tempMaterialsList.Clear();
            return slice;
        }
        private static void SetRendererSharedMaterials(ObjectHandle<Renderer> renderer, Slice<ObjectHandle<Material>> materials)
        {
            if (!renderer)
                return;
            BindingsHelper.tempMaterialsList.Clear();
            for (var i = 0; i < (int)materials.len; i++)
                BindingsHelper.tempMaterialsList.Add(materials.ptr[i]);
            renderer.value.SetSharedMaterials(BindingsHelper.tempMaterialsList);
            BindingsHelper.tempMaterialsList.Clear();
        }
        private static int GetRendererSortingLayerID(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.sortingLayerID : 0;
        private static void SetRendererSortingLayerID(ObjectHandle<Renderer> renderer, int layerID)
        {
            if (renderer)
                renderer.value.sortingLayerID = layerID;
        }
        private static int GetRendererSortingOrder(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.sortingOrder : 0;
        private static void SetRendererSortingOrder(ObjectHandle<Renderer> renderer, int order)
        {
            if (renderer)
                renderer.value.sortingOrder = order;
        }
        private static uint GetRendererRenderingLayerMask(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.renderingLayerMask : 0;
        private static void SetRendererRenderingLayerMask(ObjectHandle<Renderer> renderer, uint mask)
        {
            if (renderer)
                renderer.value.renderingLayerMask = mask;
        }
        private static bool GetRendererReceiveShadows(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.receiveShadows : false;
        private static void SetRendererReceiveShadows(ObjectHandle<Renderer> renderer, bool receive)
        {
            if (renderer)
                renderer.value.receiveShadows = receive;
        }
        private static ShadowCastingMode GetRendererShadowCastingMode(ObjectHandle<Renderer> renderer) => renderer ? renderer.value.shadowCastingMode : default;
        private static void SetRendererShadowCastingMode(ObjectHandle<Renderer> renderer, ShadowCastingMode mode)
        {
            if (renderer)
                renderer.value.shadowCastingMode = mode;
        }

        private static ObjectHandle<Mesh> GetMeshFilterMesh(ObjectHandle<MeshFilter> meshFilter) => meshFilter ? meshFilter.value.mesh : default;
        private static void SetMeshFilterMesh(ObjectHandle<MeshFilter> meshFilter, ObjectHandle<Mesh> mesh)
        {
            if (meshFilter)
                meshFilter.value.mesh = mesh;
        }
        private static ObjectHandle<Mesh> GetMeshFilterSharedMesh(ObjectHandle<MeshFilter> meshFilter) => meshFilter ? meshFilter.value.sharedMesh : default;
        private static void SetMeshFilterSharedMesh(ObjectHandle<MeshFilter> meshFilter, ObjectHandle<Mesh> mesh)
        {
            if (meshFilter)
                meshFilter.value.sharedMesh = mesh;
        }

        // Mesh API

        private static ObjectHandle<Mesh> CreateMesh() => new Mesh();
        private static void ClearMesh(ObjectHandle<Mesh> mesh, bool keepVertexLayout = true)
        {
            if (mesh)
                mesh.value.Clear(keepVertexLayout);
        }
        private static Bounds GetMeshBounds(ObjectHandle<Mesh> mesh) => mesh ? mesh.value.bounds : default;
        private static void SetMeshBounds(ObjectHandle<Mesh> mesh, Bounds bounds)
        {
            if (mesh)
                mesh.value.bounds = bounds;
        }
        private static int GetMeshVertexCount(ObjectHandle<Mesh> mesh) => mesh ? mesh.value.vertexCount : 0;
        private static int GetMeshSubMeshCount(ObjectHandle<Mesh> mesh) => mesh ? mesh.value.subMeshCount : 0;
        private static void SetMeshSubMeshCount(ObjectHandle<Mesh> mesh, int count)
        {
            if (mesh)
                mesh.value.subMeshCount = count;
        }
        private static Slice<Vector3> GetMeshVertices(ObjectHandle<Mesh> mesh, Allocator allocator)
        {
            if (!mesh)
                return default;
            BindingsHelper.tempVector3List.Clear();
            mesh.value.GetVertices(BindingsHelper.tempVector3List);
            var slice = new Slice<Vector3>(BindingsHelper.tempVector3List.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempVector3List.Count; i++)
                slice.ptr[i] = BindingsHelper.tempVector3List[i];
            BindingsHelper.tempVector3List.Clear();
            return slice;
        }
        private static void SetMeshVertices(ObjectHandle<Mesh> mesh, Slice<Vector3> vertices)
        {
            if (!mesh)
                return;
            BindingsHelper.tempVector3List.Clear();
            for (var i = 0; i < (int)vertices.len; i++)
                BindingsHelper.tempVector3List.Add(vertices.ptr[i]);
            mesh.value.SetVertices(BindingsHelper.tempVector3List);
            BindingsHelper.tempVector3List.Clear();
        }
        private static Slice<Vector3> GetMeshNormals(ObjectHandle<Mesh> mesh, Allocator allocator)
        {
            if (!mesh)
                return default;
            BindingsHelper.tempVector3List.Clear();
            mesh.value.GetNormals(BindingsHelper.tempVector3List);
            var slice = new Slice<Vector3>(BindingsHelper.tempVector3List.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempVector3List.Count; i++)
                slice.ptr[i] = BindingsHelper.tempVector3List[i];
            BindingsHelper.tempVector3List.Clear();
            return slice;
        }
        private static void SetMeshNormals(ObjectHandle<Mesh> mesh, Slice<Vector3> normals)
        {
            if (!mesh)
                return;
            BindingsHelper.tempVector3List.Clear();
            for (var i = 0; i < (int)normals.len; i++)
                BindingsHelper.tempVector3List.Add(normals.ptr[i]);
            mesh.value.SetNormals(BindingsHelper.tempVector3List);
            BindingsHelper.tempVector3List.Clear();
        }
        private static Slice<Vector2> GetMeshUVs(ObjectHandle<Mesh> mesh, int channel, Allocator allocator)
        {
            if (!mesh)
                return default;
            BindingsHelper.tempVector2List.Clear();
            mesh.value.GetUVs(channel, BindingsHelper.tempVector2List);
            var slice = new Slice<Vector2>(BindingsHelper.tempVector2List.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempVector2List.Count; i++)
                slice.ptr[i] = BindingsHelper.tempVector2List[i];
            BindingsHelper.tempVector2List.Clear();
            return slice;
        }
        private static void SetMeshUVs(ObjectHandle<Mesh> mesh, int channel, Slice<Vector2> uvs)
        {
            if (!mesh)
                return;
            BindingsHelper.tempVector2List.Clear();
            for (var i = 0; i < (int)uvs.len; i++)
                BindingsHelper.tempVector2List.Add(uvs.ptr[i]);
            mesh.value.SetUVs(channel, BindingsHelper.tempVector2List);
            BindingsHelper.tempVector2List.Clear();
        }
        private static Slice<int> GetMeshTriangles(ObjectHandle<Mesh> mesh, int submesh, Allocator allocator)
        {
            if (!mesh)
                return default;
            BindingsHelper.tempIntList.Clear();
            mesh.value.GetTriangles(BindingsHelper.tempIntList, submesh);
            var slice = new Slice<int>(BindingsHelper.tempIntList.Count, allocator);
            for (var i = 0; i < BindingsHelper.tempIntList.Count; i++)
                slice.ptr[i] = BindingsHelper.tempIntList[i];
            BindingsHelper.tempIntList.Clear();
            return slice;
        }
        private static void SetMeshTriangles(ObjectHandle<Mesh> mesh, Slice<int> triangles, int submesh)
        {
            if (!mesh)
                return;
            BindingsHelper.tempIntList.Clear();
            for (var i = 0; i < (int)triangles.len; i++)
                BindingsHelper.tempIntList.Add(triangles.ptr[i]);
            mesh.value.SetTriangles(BindingsHelper.tempIntList, submesh);
            BindingsHelper.tempIntList.Clear();
        }
        private static void RecalculateMeshBounds(ObjectHandle<Mesh> mesh)
        {
            if (mesh)
                mesh.value.RecalculateBounds();
        }
        private static void RecalculateMeshNormals(ObjectHandle<Mesh> mesh)
        {
            if (mesh)
                mesh.value.RecalculateNormals();
        }
        private static void RecalculateMeshTangents(ObjectHandle<Mesh> mesh)
        {
            if (mesh)
                mesh.value.RecalculateTangents();
        }
        private static void MarkMeshDynamic(ObjectHandle<Mesh> mesh)
        {
            if (mesh)
                mesh.value.MarkDynamic();
        }
        private static void UploadMeshData(ObjectHandle<Mesh> mesh, bool markNoLongerReadable)
        {
            if (mesh)
                mesh.value.UploadMeshData(markNoLongerReadable);
        }

        // GlobalKeyword API

        private static GlobalKeyword CreateGlobalKeyword(String8 name) => GlobalKeyword.Create(name.ToString());
        private static String8 GetGlobalKeywordName(GlobalKeyword kw, Allocator allocator) => new String8(kw.name, allocator);

        // Shader API

        private static ObjectHandle<Shader> FindShader(String8 name) => Shader.Find(name.ToString());
        private static int GetShaderMaximumLOD(ObjectHandle<Shader> shader) => shader ? shader.value.maximumLOD : 0;
        private static void SetShaderMaximumLOD(ObjectHandle<Shader> shader, int lod)
        {
            if (shader)
                shader.value.maximumLOD = lod;
        }
        private static int GetShaderRenderQueue(ObjectHandle<Shader> shader) => shader ? shader.value.renderQueue : 0;
        private static bool IsShaderSupported(ObjectHandle<Shader> shader) => shader ? shader.value.isSupported : false;
        private static int GetGlobalShaderMaximumLOD() => Shader.globalMaximumLOD;
        private static void SetGlobalShaderMaximumLOD(int lod) => Shader.globalMaximumLOD = lod;
        private static String8 GetGlobalShaderRenderPipeline(Allocator allocator) => new String8(Shader.globalRenderPipeline, allocator);
        private static void SetGlobalShaderRenderPipeline(String8 renderPipelineName) => Shader.globalRenderPipeline = renderPipelineName.ToString();
        private static int GetShaderPropertyId(String8 propertyName) => Shader.PropertyToID(propertyName.ToString());
        private static void EnableShaderKeyword(GlobalKeyword keyword) => Shader.EnableKeyword(keyword);
        private static void DisableShaderKeyword(GlobalKeyword keyword) => Shader.DisableKeyword(keyword);
        private static bool IsShaderKeywordEnabled(GlobalKeyword keyword) => Shader.IsKeywordEnabled(keyword);
        private static float GetGlobalShaderFloat(int propertyID) => Shader.GetGlobalFloat(propertyID);
        private static void SetGlobalShaderFloat(int propertyID, float value) => Shader.SetGlobalFloat(propertyID, value);
        private static int GetGlobalShaderInt(int propertyID) => Shader.GetGlobalInt(propertyID);
        private static void SetGlobalShaderInt(int propertyID, int value) => Shader.SetGlobalInt(propertyID, value);
        private static Vector4 GetGlobalShaderVector(int propertyID) => Shader.GetGlobalVector(propertyID);
        private static void SetGlobalShaderVector(int propertyID, Vector4 value) => Shader.SetGlobalVector(propertyID, value);
        private static Color GetGlobalShaderColor(int propertyID) => Shader.GetGlobalColor(propertyID);
        private static void SetGlobalShaderColor(int propertyID, Color value) => Shader.SetGlobalColor(propertyID, value);
        private static Matrix4x4 GetGlobalShaderMatrix(int propertyID) => Shader.GetGlobalMatrix(propertyID);
        private static void SetGlobalShaderMatrix(int propertyID, Matrix4x4 value) => Shader.SetGlobalMatrix(propertyID, value);
        private static ObjectHandle<Texture> GetGlobalShaderTexture(int propertyID) => Shader.GetGlobalTexture(propertyID);
        private static void SetGlobalShaderTexture(int propertyID, ObjectHandle<Texture> value) => Shader.SetGlobalTexture(propertyID, value);
        private static void WarmupAllShaders() => Shader.WarmupAllShaders();

        // Material API

        private static ObjectHandle<Material> CreateMaterial(ObjectHandle<Shader> shader)
        {
            if (!shader)
                return default;
            return new Material(shader);
        }
        private static ObjectHandle<Shader> GetMaterialShader(ObjectHandle<Material> material) => material ? material.value.shader : default;
        private static void SetMaterialShader(ObjectHandle<Material> material, ObjectHandle<Shader> shader)
        {
            if (material)
                material.value.shader = shader;
        }
        private static int GetMaterialRenderQueue(ObjectHandle<Material> material) => material ? material.value.renderQueue : 0;
        private static void SetMaterialRenderQueue(ObjectHandle<Material> material, int queue)
        {
            if (material)
                material.value.renderQueue = queue;
        }
        private static float GetMaterialFloat(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetFloat(propertyId) : 0f;
        private static void SetMaterialFloat(ObjectHandle<Material> material, int propertyId, float value)
        {
            if (material)
                material.value.SetFloat(propertyId, value);
        }
        private static int GetMaterialInt(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetInt(propertyId) : 0;
        private static void SetMaterialInt(ObjectHandle<Material> material, int propertyId, int value)
        {
            if (material)
                material.value.SetInt(propertyId, value);
        }
        private static Vector4 GetMaterialVector(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetVector(propertyId) : default;
        private static void SetMaterialVector(ObjectHandle<Material> material, int propertyId, Vector4 value)
        {
            if (material)
                material.value.SetVector(propertyId, value);
        }
        private static Color GetMaterialColor(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetColor(propertyId) : default;
        private static void SetMaterialColor(ObjectHandle<Material> material, int propertyId, Color color)
        {
            if (material)
                material.value.SetColor(propertyId, color);
        }
        private static Matrix4x4 GetMaterialMatrix(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetMatrix(propertyId) : default;
        private static void SetMaterialMatrix(ObjectHandle<Material> material, int propertyId, Matrix4x4 mat)
        {
            if (material)
                material.value.SetMatrix(propertyId, mat);
        }
        private static ObjectHandle<Texture> GetMaterialTexture(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetTexture(propertyId) : default;
        private static void SetMaterialTexture(ObjectHandle<Material> material, int propertyId, ObjectHandle<Texture> texture)
        {
            if (material)
                material.value.SetTexture(propertyId, texture);
        }
        private static Vector2 GetMaterialTextureScale(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetTextureScale(propertyId) : default;
        private static void SetMaterialTextureScale(ObjectHandle<Material> material, int propertyId, Vector2 scale)
        {
            if (material)
                material.value.SetTextureScale(propertyId, scale);
        }
        private static Vector2 GetMaterialTextureOffset(ObjectHandle<Material> material, int propertyId) => material ? material.value.GetTextureOffset(propertyId) : default;
        private static void SetMaterialTextureOffset(ObjectHandle<Material> material, int propertyId, Vector2 offset)
        {
            if (material)
                material.value.SetTextureOffset(propertyId, offset);
        }
        private static bool HasMaterialProperty(ObjectHandle<Material> material, int propertyId) => material ? material.value.HasProperty(propertyId) : false;
        private static void EnableMaterialKeyword(ObjectHandle<Material> material, String8 keyword)
        {
            if (material)
                material.value.EnableKeyword(keyword.ToString());
        }
        private static void DisableMaterialKeyword(ObjectHandle<Material> material, String8 keyword)
        {
            if (material)
                material.value.DisableKeyword(keyword.ToString());
        }
        private static bool IsMaterialKeywordEnabled(ObjectHandle<Material> material, String8 keyword) => material ? material.value.IsKeywordEnabled(keyword.ToString()) : false;

        // Texture API

        private static int GetTextureWidth(ObjectHandle<Texture> texture) => texture ? texture.value.width : 0;
        private static void SetTextureWidth(ObjectHandle<Texture> texture, int width)
        {
            if (texture)
                texture.value.width = width;
        }
        private static int GetTextureHeight(ObjectHandle<Texture> texture) => texture ? texture.value.height : 0;
        private static void SetTextureHeight(ObjectHandle<Texture> texture, int height)
        {
            if (texture)
                texture.value.height = height;
        }
        private static int GetTextureDimension(ObjectHandle<Texture> texture) => texture ? (int)texture.value.dimension : 0;
        private static bool IsTextureReadable(ObjectHandle<Texture> texture) => texture ? texture.value.isReadable : false;
        private static FilterMode GetTextureFilterMode(ObjectHandle<Texture> texture) => texture ? texture.value.filterMode : default;
        private static void SetTextureFilterMode(ObjectHandle<Texture> texture, FilterMode filterMode)
        {
            if (texture)
                texture.value.filterMode = filterMode;
        }
        private static int GetTextureAnisoLevel(ObjectHandle<Texture> texture) => texture ? texture.value.anisoLevel : 0;
        private static void SetTextureAnisoLevel(ObjectHandle<Texture> texture, int level)
        {
            if (texture)
                texture.value.anisoLevel = level;
        }
        private static TextureWrapMode GetTextureWrapMode(ObjectHandle<Texture> texture) => texture ? texture.value.wrapMode : default;
        private static void SetTextureWrapMode(ObjectHandle<Texture> texture, TextureWrapMode wrapMode)
        {
            if (texture)
                texture.value.wrapMode = wrapMode;
        }
        private static TextureWrapMode GetTextureWrapModeU(ObjectHandle<Texture> texture) => texture ? texture.value.wrapModeU : default;
        private static void SetTextureWrapModeU(ObjectHandle<Texture> texture, TextureWrapMode wrapMode)
        {
            if (texture)
                texture.value.wrapModeU = wrapMode;
        }
        private static TextureWrapMode GetTextureWrapModeV(ObjectHandle<Texture> texture) => texture ? texture.value.wrapModeV : default;
        private static void SetTextureWrapModeV(ObjectHandle<Texture> texture, TextureWrapMode wrapMode)
        {
            if (texture)
                texture.value.wrapModeV = wrapMode;
        }
        private static TextureWrapMode GetTextureWrapModeW(ObjectHandle<Texture> texture) => texture ? texture.value.wrapModeW : default;
        private static void SetTextureWrapModeW(ObjectHandle<Texture> texture, TextureWrapMode wrapMode)
        {
            if (texture)
                texture.value.wrapModeW = wrapMode;
        }
        private static float GetTextureMipMapBias(ObjectHandle<Texture> texture) => texture ? texture.value.mipMapBias : 0f;
        private static void SetTextureMipMapBias(ObjectHandle<Texture> texture, float bias)
        {
            if (texture)
                texture.value.mipMapBias = bias;
        }
        private static ulong GetTextureUpdateCount(ObjectHandle<Texture> texture) => texture ? texture.value.updateCount : 0;

        // Texture2D API

        private static ObjectHandle<Texture2D> CreateTexture2D(int width, int height, TextureFormat format = TextureFormat.RGBA32, bool mipChain = true, bool linear = false, bool createUninitialised = false) => new Texture2D(width, height, format, mipChain, linear, createUninitialised);
        private static ObjectHandle<Texture2D> CreateTexture2DWithMipCount(int width, int height, TextureFormat format = TextureFormat.RGBA32, int mipCount = -1, bool linear = false) => new Texture2D(width, height, format, mipCount, linear);
        private static TextureFormat GetTexture2DFormat(ObjectHandle<Texture2D> texture) => texture ? texture.value.format : default;
        private static int GetTexture2DMipMapCount(ObjectHandle<Texture2D> texture) => texture ? texture.value.mipmapCount : 0;
        private static Color GetTexture2DPixel(ObjectHandle<Texture2D> texture, int x, int y, int mipLevel = 0) => texture ? texture.value.GetPixel(x, y, mipLevel) : default;
        private static Color GetTexture2DPixelBilinear(ObjectHandle<Texture2D> texture, float u, float v, int mipLevel = 0) => texture ? texture.value.GetPixelBilinear(u, v, mipLevel) : default;
        private static void SetTexture2DPixel(ObjectHandle<Texture2D> texture, int x, int y, Color color, int mipLevel = 0)
        {
            if (texture)
                texture.value.SetPixel(x, y, color, mipLevel);
        }
        private static Slice<Color32> GetTexture2DPixels(ObjectHandle<Texture2D> texture, int mipLevel, Allocator allocator)
        {
            if (!texture)
                return default;
            var pixels = texture.value.GetPixels32(mipLevel);
            var slice = new Slice<Color32>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DPixels(ObjectHandle<Texture2D> texture, Slice<Color32> pixels, int mipLevel = 0)
        {
            if (!texture)
                return;
            var arr = new Color32[(int)pixels.len];
            for (var i = 0; i < (int)pixels.len; i++)
                arr[i] = pixels.ptr[i];
            texture.value.SetPixels32(arr, mipLevel);
        }
        private static void ApplyTexture2D(ObjectHandle<Texture2D> texture, bool updateMipmaps = true, bool makeNoLongerReadable = false)
        {
            if (texture)
                texture.value.Apply(updateMipmaps, makeNoLongerReadable);
        }
        private static void ReadTexture2DPixels(ObjectHandle<Texture2D> texture, Rect source, int destX, int destY, bool recalculateMipMaps = true)
        {
            if (texture)
                texture.value.ReadPixels(source, destX, destY, recalculateMipMaps);
        }
        private static bool ReinitializeTexture2D(ObjectHandle<Texture2D> texture, int width, int height) => texture ? texture.value.Reinitialize(width, height) : false;
        private static bool ReinitializeTexture2DWithNewFormat(ObjectHandle<Texture2D> texture, int width, int height, TextureFormat format, bool hasMipMap) => texture ? texture.value.Reinitialize(width, height, format, hasMipMap) : false;
        private static void CompressTexture2D(ObjectHandle<Texture2D> texture, bool highQuality)
        {
            if (texture)
                texture.value.Compress(highQuality);
        }

        // Texture2DArray API

        private static ObjectHandle<Texture2DArray> CreateTexture2DArray(int width, int height, int depth, TextureFormat format, bool mipChain) => new Texture2DArray(width, height, depth, format, mipChain);
        private static ObjectHandle<Texture2DArray> CreateTexture2DArrayWithMipCount(int width, int height, int depth, TextureFormat format, int mipCount, bool linear) => new Texture2DArray(width, height, depth, format, mipCount, linear);
        private static int GetTexture2DArrayDepth(ObjectHandle<Texture2DArray> textureArray) => textureArray ? textureArray.value.depth : 0;
        private static TextureFormat GetTexture2DArrayFormat(ObjectHandle<Texture2DArray> textureArray) => textureArray ? textureArray.value.format : default;
        private static Slice<Color32> GetTexture2DArrayPixels(ObjectHandle<Texture2DArray> textureArray, int arrayElement, int mipLevel, Allocator allocator)
        {
            if (!textureArray)
                return default;
            var pixels = textureArray.value.GetPixels32(arrayElement, mipLevel);
            var slice = new Slice<Color32>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DArrayPixels(ObjectHandle<Texture2DArray> textureArray, Slice<Color32> pixels, int arrayElement, int mipLevel = 0)
        {
            if (!textureArray)
                return;
            var arr = new Color32[(int)pixels.len];
            for (var i = 0; i < (int)pixels.len; i++)
                arr[i] = pixels.ptr[i];
            textureArray.value.SetPixels32(arr, arrayElement, mipLevel);
        }
        private static void ApplyTexture2DArray(ObjectHandle<Texture2DArray> textureArray, bool updateMipmaps = true, bool makeNoLongerReadable = false)
        {
            if (textureArray)
                textureArray.value.Apply(updateMipmaps, makeNoLongerReadable);
        }
    }
}
