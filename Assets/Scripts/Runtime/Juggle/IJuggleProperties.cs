using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJuggleProperties
{
    int juggle_startProperty { get; set; }
    int juggle_increaseProperty { get; set; }
    int juggle_potentialProperty { get; set; }
}
