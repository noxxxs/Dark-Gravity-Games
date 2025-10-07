using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignableObject))]
public class AlignToGroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AlignableObject obj = (AlignableObject)target;

        if (GUILayout.Button("Align to Ground"))
        {
            AlignToGround(obj);
        }
    }

    private void AlignToGround(AlignableObject obj)
    {
        Vector3 origin = obj.transform.position + Vector3.up * 50;
        Collider thisCollider = obj.GetComponent<Collider>();
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, 100f);

        //Reset rotation
        obj.transform.rotation = Quaternion.identity;

        foreach (var hit in hits)
        {
            if (hit.collider == thisCollider)
                continue;

            obj.transform.position = hit.point + Vector3.up * (GetColliderHeight(obj.gameObject) / 2 +  obj.GroundOffset);
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * obj.transform.rotation;
            break;
        }
    }

    // Func to calculate object Height
    private float GetColliderHeight(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            return 0f;
        }

        // BoxCollider
        if (col is BoxCollider box)
        {
            return box.size.y * obj.transform.localScale.y;
        }

        // CapsuleCollider
        if (col is CapsuleCollider capsule)
        {
            return capsule.height * obj.transform.localScale.y;
        }

        // SphereCollider
        if (col is SphereCollider sphere)
        {
            return sphere.radius * 2f * obj.transform.localScale.y;
        }

        // MeshCollider
        if (col is MeshCollider mesh)
        {
            return mesh.sharedMesh.bounds.size.y * obj.transform.localScale.y;
        }

        return col.bounds.size.y;
    }

}
