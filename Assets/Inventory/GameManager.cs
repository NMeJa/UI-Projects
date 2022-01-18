using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class ItemDetails
    {
        public string Name;
        public string GUID;
        public Sprite Icon;
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Sprite> IconSprites;
        private static readonly Dictionary<string, ItemDetails> itemDatabase = new();
        private readonly List<ItemDetails> playerInventory = new();
        public static event Action<string[]> OnInventoryChanged = null;


        private void Awake()
        {
            PopulateDatabase();
        }

        private void Start()
        {
            playerInventory.AddRange(itemDatabase.Values);
            OnInventoryChanged.Invoke(playerInventory.Select(x => x.GUID).ToArray());
        }

        private void PopulateDatabase()
        {
            var randGuid = Guid.NewGuid().ToString();
            itemDatabase.Add(randGuid, new ItemDetails
            {
                Name = "One",
                GUID = randGuid,
                Icon = IconSprites[0]
            });
            randGuid = Guid.NewGuid().ToString();

            itemDatabase.Add(randGuid, new ItemDetails
            {
                Name = "Two",
                GUID = randGuid,
                Icon = IconSprites[1]
            });
            randGuid = Guid.NewGuid().ToString();

            itemDatabase.Add(randGuid, new ItemDetails
            {
                Name = "Three",
                GUID = randGuid,
                Icon = IconSprites[2]
            });
            randGuid = Guid.NewGuid().ToString();

            itemDatabase.Add(randGuid, new ItemDetails
            {
                Name = "Four",
                GUID = randGuid,
                Icon = IconSprites[3]
            });
        }


        public static ItemDetails GetItemByGuid(string guid) =>
            itemDatabase.ContainsKey(guid) ? itemDatabase[guid] : null;
    }
}