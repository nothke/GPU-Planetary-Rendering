using UnityEngine;
using UnityEngine.Rendering;

public class PlanetChunkObject : MonoBehaviour
{
    public PlanetChunkProperties Properties;
	public MeshFilter Filter;
	public MeshCollider Collider;
    public Renderer Renderer;
    public bool IsVisible { get; private set; }
    public bool IsCalculating { get; private set; }

    private void OnDrawGizmos()
	{
		//Gizmos.color = Color.white;
		//Gizmos.DrawWireSphere(Properties.Bounds.center, 100 * Properties.Size);
		//Gizmos.DrawWireCube(Properties.Bounds.center, Properties.Bounds.size);
	}

    public void SetVisible(bool visible)
    {
        this.IsVisible = visible;
        if (!visible)
        {
            Renderer.enabled = false;
        }
        else if (!IsCalculating)
        {
            Renderer.enabled = true;
        }
    }

    public void MarkCalculatingVertexData() => IsCalculating = true;

    public void ApplyVertexData(AsyncGPUReadbackRequest request)
    {
        //only show renderer and rad data if the chunk is visible.
        //GPU readback might have happened too late and chuck might have been made invisible by that time.
        if (IsVisible)
        {
            var vectorData = request.GetData<Vector3>();
            Filter.sharedMesh.SetVertices(vectorData);
            Filter.sharedMesh.RecalculateBounds();
            Renderer.enabled = true;
            IsCalculating = false;

            // Must reset the mesh for the collider to update
            Collider.sharedMesh = Filter.sharedMesh;
        }
    }

    public void ApplyNormalData(AsyncGPUReadbackRequest request)
    {
        //GPU readback might have happened too late and chuck might have been made invisible by that time.
        if (IsVisible)
        {
            var vectorData = request.GetData<Vector3>();
            Filter.sharedMesh.SetNormals(vectorData);
        }
    }
}