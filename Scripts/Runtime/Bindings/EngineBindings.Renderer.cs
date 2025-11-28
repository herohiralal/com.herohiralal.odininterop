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
            var materials = renderer.value.materials;
            var slice = new Slice<ObjectHandle<Material>>(materials.Length, allocator);
            for (var i = 0; i < materials.Length; i++)
                slice.ptr[i] = materials[i];
            return slice;
        }
        private static void SetRendererMaterials(ObjectHandle<Renderer> renderer, Slice<ObjectHandle<Material>> materials)
        {
            if (!renderer)
                return;
            var arr = new Material[(int)materials.len];
            for (var i = 0; i < (int)materials.len; i++)
                arr[i] = materials.ptr[i];
            renderer.value.materials = arr;
        }
        private static Slice<ObjectHandle<Material>> GetRendererSharedMaterials(ObjectHandle<Renderer> renderer, Allocator allocator)
        {
            if (!renderer)
                return default;
            var materials = renderer.value.sharedMaterials;
            var slice = new Slice<ObjectHandle<Material>>(materials.Length, allocator);
            for (var i = 0; i < materials.Length; i++)
                slice.ptr[i] = materials[i];
            return slice;
        }
        private static void SetRendererSharedMaterials(ObjectHandle<Renderer> renderer, Slice<ObjectHandle<Material>> materials)
        {
            if (!renderer)
                return;
            var arr = new Material[(int)materials.len];
            for (var i = 0; i < (int)materials.len; i++)
                arr[i] = materials.ptr[i];
            renderer.value.sharedMaterials = arr;
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
        private static int GetRendererRenderingLayerMask(ObjectHandle<Renderer> renderer) => (int)(renderer ? renderer.value.renderingLayerMask : 0);
        private static void SetRendererRenderingLayerMask(ObjectHandle<Renderer> renderer, int mask)
        {
            if (renderer)
                renderer.value.renderingLayerMask = (uint)mask;
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

        // MeshRenderer API

        private static ObjectHandle<MeshFilter> GetMeshFilterFromMeshRenderer(ObjectHandle<MeshRenderer> meshRenderer)
        {
            if (!meshRenderer)
                return default;
            return meshRenderer.value.GetComponent<MeshFilter>();
        }

        // MeshFilter API

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

        private static ObjectHandle<Mesh> CreateMesh()
        {
            return new Mesh();
        }
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
            var vertices = mesh.value.vertices;
            var slice = new Slice<Vector3>(vertices.Length, allocator);
            for (var i = 0; i < vertices.Length; i++)
                slice.ptr[i] = vertices[i];
            return slice;
        }
        private static void SetMeshVertices(ObjectHandle<Mesh> mesh, Slice<Vector3> vertices)
        {
            if (!mesh)
                return;
            var arr = new Vector3[(int)vertices.len];
            for (var i = 0; i < (int)vertices.len; i++)
                arr[i] = vertices.ptr[i];
            mesh.value.vertices = arr;
        }
        private static Slice<Vector3> GetMeshNormals(ObjectHandle<Mesh> mesh, Allocator allocator)
        {
            if (!mesh)
                return default;
            var normals = mesh.value.normals;
            var slice = new Slice<Vector3>(normals.Length, allocator);
            for (var i = 0; i < normals.Length; i++)
                slice.ptr[i] = normals[i];
            return slice;
        }
        private static void SetMeshNormals(ObjectHandle<Mesh> mesh, Slice<Vector3> normals)
        {
            if (!mesh)
                return;
            var arr = new Vector3[(int)normals.len];
            for (var i = 0; i < (int)normals.len; i++)
                arr[i] = normals.ptr[i];
            mesh.value.normals = arr;
        }
        private static Slice<Vector2> GetMeshUVs(ObjectHandle<Mesh> mesh, int channel, Allocator allocator)
        {
            if (!mesh)
                return default;
            var uvs = new System.Collections.Generic.List<Vector2>();
            mesh.value.GetUVs(channel, uvs);
            var slice = new Slice<Vector2>(uvs.Count, allocator);
            for (var i = 0; i < uvs.Count; i++)
                slice.ptr[i] = uvs[i];
            return slice;
        }
        private static void SetMeshUVs(ObjectHandle<Mesh> mesh, int channel, Slice<Vector2> uvs)
        {
            if (!mesh)
                return;
            var list = new System.Collections.Generic.List<Vector2>((int)uvs.len);
            for (var i = 0; i < (int)uvs.len; i++)
                list.Add(uvs.ptr[i]);
            mesh.value.SetUVs(channel, list);
        }
        private static Slice<int> GetMeshTriangles(ObjectHandle<Mesh> mesh, int submesh, Allocator allocator)
        {
            if (!mesh)
                return default;
            var triangles = mesh.value.GetTriangles(submesh);
            var slice = new Slice<int>(triangles.Length, allocator);
            for (var i = 0; i < triangles.Length; i++)
                slice.ptr[i] = triangles[i];
            return slice;
        }
        private static void SetMeshTriangles(ObjectHandle<Mesh> mesh, Slice<int> triangles, int submesh)
        {
            if (!mesh)
                return;
            var arr = new int[(int)triangles.len];
            for (var i = 0; i < (int)triangles.len; i++)
                arr[i] = triangles.ptr[i];
            mesh.value.SetTriangles(arr, submesh);
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

        // Material API

        private static ObjectHandle<Material> CreateMaterial(ObjectHandle<Shader> shader)
        {
            if (!shader)
                return default;
            return new Material(shader);
        }
        private static ObjectHandle<Material> CreateMaterialFromName(String8 shaderName)
        {
            var shader = Shader.Find(shaderName.ToString());
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
        private static Color GetMaterialColor(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetColor(propertyName.ToString()) : default;
        private static void SetMaterialColor(ObjectHandle<Material> material, String8 propertyName, Color color)
        {
            if (material)
                material.value.SetColor(propertyName.ToString(), color);
        }
        private static float GetMaterialFloat(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetFloat(propertyName.ToString()) : 0f;
        private static void SetMaterialFloat(ObjectHandle<Material> material, String8 propertyName, float value)
        {
            if (material)
                material.value.SetFloat(propertyName.ToString(), value);
        }
        private static int GetMaterialInt(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetInt(propertyName.ToString()) : 0;
        private static void SetMaterialInt(ObjectHandle<Material> material, String8 propertyName, int value)
        {
            if (material)
                material.value.SetInt(propertyName.ToString(), value);
        }
        private static Vector4 GetMaterialVector(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetVector(propertyName.ToString()) : default;
        private static void SetMaterialVector(ObjectHandle<Material> material, String8 propertyName, Vector4 value)
        {
            if (material)
                material.value.SetVector(propertyName.ToString(), value);
        }
        private static ObjectHandle<Texture> GetMaterialTexture(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetTexture(propertyName.ToString()) : default;
        private static void SetMaterialTexture(ObjectHandle<Material> material, String8 propertyName, ObjectHandle<Texture> texture)
        {
            if (material)
                material.value.SetTexture(propertyName.ToString(), texture);
        }
        private static Vector2 GetMaterialTextureOffset(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetTextureOffset(propertyName.ToString()) : default;
        private static void SetMaterialTextureOffset(ObjectHandle<Material> material, String8 propertyName, Vector2 offset)
        {
            if (material)
                material.value.SetTextureOffset(propertyName.ToString(), offset);
        }
        private static Vector2 GetMaterialTextureScale(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.GetTextureScale(propertyName.ToString()) : default;
        private static void SetMaterialTextureScale(ObjectHandle<Material> material, String8 propertyName, Vector2 scale)
        {
            if (material)
                material.value.SetTextureScale(propertyName.ToString(), scale);
        }
        private static bool HasMaterialProperty(ObjectHandle<Material> material, String8 propertyName) => material ? material.value.HasProperty(propertyName.ToString()) : false;
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
        private static void EnableShaderKeyword(String8 keyword) => Shader.EnableKeyword(keyword.ToString());
        private static void DisableShaderKeyword(String8 keyword) => Shader.DisableKeyword(keyword.ToString());
        private static bool IsShaderKeywordEnabled(String8 keyword) => Shader.IsKeywordEnabled(keyword.ToString());
        private static void SetGlobalShaderFloat(String8 propertyName, float value) => Shader.SetGlobalFloat(propertyName.ToString(), value);
        private static void SetGlobalShaderInt(String8 propertyName, int value) => Shader.SetGlobalInt(propertyName.ToString(), value);
        private static void SetGlobalShaderVector(String8 propertyName, Vector4 value) => Shader.SetGlobalVector(propertyName.ToString(), value);
        private static void SetGlobalShaderColor(String8 propertyName, Color value) => Shader.SetGlobalColor(propertyName.ToString(), value);
        private static void SetGlobalShaderMatrix(String8 propertyName, Matrix4x4 value) => Shader.SetGlobalMatrix(propertyName.ToString(), value);
        private static void SetGlobalShaderTexture(String8 propertyName, ObjectHandle<Texture> texture) => Shader.SetGlobalTexture(propertyName.ToString(), texture);
        private static int GetShaderPropertyToID(String8 propertyName) => Shader.PropertyToID(propertyName.ToString());
        private static void WarmupAllShaders() => Shader.WarmupAllShaders();

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

        private static ObjectHandle<Texture2D> CreateTexture2D(int width, int height, TextureFormat format, bool mipChain)
        {
            return new Texture2D(width, height, format, mipChain);
        }
        private static ObjectHandle<Texture2D> CreateTexture2DWithMipCount(int width, int height, TextureFormat format, int mipCount, bool linear)
        {
            return new Texture2D(width, height, format, mipCount, linear);
        }
        private static TextureFormat GetTexture2DFormat(ObjectHandle<Texture2D> texture) => texture ? texture.value.format : default;
        private static int GetTexture2DMipMapCount(ObjectHandle<Texture2D> texture) => texture ? texture.value.mipmapCount : 0;
        private static Color GetTexture2DPixel(ObjectHandle<Texture2D> texture, int x, int y, int mipLevel = 0) => texture ? texture.value.GetPixel(x, y, mipLevel) : default;
        private static Color GetTexture2DPixelBilinear(ObjectHandle<Texture2D> texture, float u, float v, int mipLevel = 0) => texture ? texture.value.GetPixelBilinear(u, v, mipLevel) : default;
        private static void SetTexture2DPixel(ObjectHandle<Texture2D> texture, int x, int y, Color color, int mipLevel = 0)
        {
            if (texture)
                texture.value.SetPixel(x, y, color, mipLevel);
        }
        private static Slice<Color> GetTexture2DPixels(ObjectHandle<Texture2D> texture, int mipLevel, Allocator allocator)
        {
            if (!texture)
                return default;
            var pixels = texture.value.GetPixels(mipLevel);
            var slice = new Slice<Color>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DPixels(ObjectHandle<Texture2D> texture, Slice<Color> pixels, int mipLevel = 0)
        {
            if (!texture)
                return;
            var arr = new Color[(int)pixels.len];
            for (var i = 0; i < (int)pixels.len; i++)
                arr[i] = pixels.ptr[i];
            texture.value.SetPixels(arr, mipLevel);
        }
        private static Slice<Color32> GetTexture2DPixels32(ObjectHandle<Texture2D> texture, int mipLevel, Allocator allocator)
        {
            if (!texture)
                return default;
            var pixels = texture.value.GetPixels32(mipLevel);
            var slice = new Slice<Color32>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DPixels32(ObjectHandle<Texture2D> texture, Slice<Color32> pixels, int mipLevel = 0)
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
        private static bool ResizeTexture2D(ObjectHandle<Texture2D> texture, int width, int height, TextureFormat format, bool hasMipMap) => texture ? texture.value.Reinitialize(width, height, format, hasMipMap) : false;
        private static bool ResizeTexture2DSimple(ObjectHandle<Texture2D> texture, int width, int height) => texture ? texture.value.Reinitialize(width, height) : false;
        private static void CompressTexture2D(ObjectHandle<Texture2D> texture, bool highQuality)
        {
            if (texture)
                texture.value.Compress(highQuality);
        }

        // Texture2DArray API

        private static ObjectHandle<Texture2DArray> CreateTexture2DArray(int width, int height, int depth, TextureFormat format, bool mipChain)
        {
            return new Texture2DArray(width, height, depth, format, mipChain);
        }
        private static ObjectHandle<Texture2DArray> CreateTexture2DArrayWithMipCount(int width, int height, int depth, TextureFormat format, int mipCount, bool linear)
        {
            return new Texture2DArray(width, height, depth, format, mipCount, linear);
        }
        private static int GetTexture2DArrayDepth(ObjectHandle<Texture2DArray> textureArray) => textureArray ? textureArray.value.depth : 0;
        private static TextureFormat GetTexture2DArrayFormat(ObjectHandle<Texture2DArray> textureArray) => textureArray ? textureArray.value.format : default;
        private static Slice<Color> GetTexture2DArrayPixels(ObjectHandle<Texture2DArray> textureArray, int arrayElement, int mipLevel, Allocator allocator)
        {
            if (!textureArray)
                return default;
            var pixels = textureArray.value.GetPixels(arrayElement, mipLevel);
            var slice = new Slice<Color>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DArrayPixels(ObjectHandle<Texture2DArray> textureArray, Slice<Color> pixels, int arrayElement, int mipLevel = 0)
        {
            if (!textureArray)
                return;
            var arr = new Color[(int)pixels.len];
            for (var i = 0; i < (int)pixels.len; i++)
                arr[i] = pixels.ptr[i];
            textureArray.value.SetPixels(arr, arrayElement, mipLevel);
        }
        private static Slice<Color32> GetTexture2DArrayPixels32(ObjectHandle<Texture2DArray> textureArray, int arrayElement, int mipLevel, Allocator allocator)
        {
            if (!textureArray)
                return default;
            var pixels = textureArray.value.GetPixels32(arrayElement, mipLevel);
            var slice = new Slice<Color32>(pixels.Length, allocator);
            for (var i = 0; i < pixels.Length; i++)
                slice.ptr[i] = pixels[i];
            return slice;
        }
        private static void SetTexture2DArrayPixels32(ObjectHandle<Texture2DArray> textureArray, Slice<Color32> pixels, int arrayElement, int mipLevel = 0)
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
