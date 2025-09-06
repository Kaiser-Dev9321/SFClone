using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtbox
{
    void DisableHurtbox();
    void EnableHurtbox();
}

public class Hurtbox : MonoBehaviour, IHurtbox
{
    public void DisableHurtbox()
    {
        gameObject.SetActive(false);
    }

    public void EnableHurtbox()
    {
        gameObject.SetActive(true);
    }
}
