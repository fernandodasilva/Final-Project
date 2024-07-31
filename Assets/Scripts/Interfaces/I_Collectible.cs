using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Collectible
{
    bool KeepPosition { get; }

    GameObject Collect();

}
