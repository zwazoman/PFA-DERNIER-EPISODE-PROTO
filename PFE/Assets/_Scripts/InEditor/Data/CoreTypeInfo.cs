using UnityEngine;

[CreateAssetMenu(fileName = "NewCoreData", menuName = "Weapons/CoreData")]
public class CoreTypeInfo : ScriptableObject
{
    [field: SerializeField]
    public string coreName { get; private set; }

    [field : SerializeField]
    public Sprite sprite { get; private set; }
}
