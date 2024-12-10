using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Code;

    public ItemData Data { get; private set; }
    public Sprite Icon { get; private set; }

    public void SetData(ItemData _data)
    {
        Data = _data;

        Icon = Resources.Load<Sprite>($"Item/Icon/{_data.Icon}");
    }
}