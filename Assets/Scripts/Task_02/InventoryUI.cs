using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    private string[] _itemNames = new string[]
    {
        "Sword", "Apple", "Wand", "Stick", "Door"
    };

    private UIDocument _uiDocument;

    private Button _addItemButton;
    private ScrollView _itemList;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _addItemButton = root.Q<Button>("AddItemButton");
        _itemList = root.Q<ScrollView>("ItemList");

        _addItemButton.clicked += AddRandomItem;
    }
    private void AddRandomItem()
    {
        Button item = new Button();
        item.text = _itemNames[Random.Range(0, _itemNames.Length)];
        item.AddToClassList("inventoryItem");
        item.clicked += () => _itemList.Remove(item);
        _itemList.Add(item);
    }
    private void OnDisable()
    {
        _addItemButton.clicked -= AddRandomItem;
    }
}
