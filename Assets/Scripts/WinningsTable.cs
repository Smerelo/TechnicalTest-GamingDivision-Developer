using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "WinningsTable", menuName = "WinningsTable", order = 0)]
    public class WinningsTable : ScriptableObject
    {
        public int[] Winnings = new int[4];
        public Sprite Figure;
    }
}