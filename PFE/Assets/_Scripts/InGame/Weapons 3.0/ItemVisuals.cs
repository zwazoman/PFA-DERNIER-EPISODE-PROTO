using UnityEngine;
using Unity.Netcode;

public class ItemVisuals : MonoBehaviour
{
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] MeshRenderer _meshrender;

    public void ShowItemRpc(Mesh mesh)
    {
        //faut trouver un moyen d'exposer le mesh par autre chose qu'un rpc. ça coute trop cher de le serializer
        //quoi que

        print("show mesh");
        _meshFilter.mesh = Instantiate(mesh);
    }

    //[Rpc(SendTo.ClientsAndHost)]
    //public void SwapItemRpc(Mesh mesh)
    //{
    //    _meshFilter.sharedMesh = mesh;
    //}
}
