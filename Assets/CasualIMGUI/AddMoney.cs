using TMPro;
using UnityEngine;

namespace CasualIMGUI
{
    public class AddMoney : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void _AddMoneyToPlayer()
        {
            text.SetText((int.Parse(text.text) + 1000).ToString());
        }
    }
}