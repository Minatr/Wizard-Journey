using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePreviousSave : MonoBehaviour
{
    void Start()
    {
        SavedData save = SaveSystem.instance.LoadData();

        // Si la save n'a pas le champs save ou vient d'une version précédente
        if (save != null && (save.GetType().GetProperty("version") == null || save.version != Application.version))
        {
            SaveSystem.instance.DeleteSaveFile();
        }
    }
}
