using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
   [TextArea] public string itemDescription;
    public Sprite icon;


}
