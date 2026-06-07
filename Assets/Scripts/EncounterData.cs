using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Scriptable Objects/EncounterData")]
public class EncounterData : ScriptableObject
{
    public GameObject[] enemyPrefabs;

    public void ClearData()
    {
        enemyPrefabs = null;
    }
}
