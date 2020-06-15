using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCast : MonoBehaviour
{
    public List<Transform> triggerObjects;

    public CharacterController characterController;

    private Coroutine coroutine;

    private void Start()
    {
        triggerObjects = new List<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerObjects.Add(collision.transform);

        if (coroutine == null)
        {
            coroutine = StartCoroutine(WtfCollider());
        }    
    }

    private IEnumerator WtfCollider()
    {
        yield return new WaitForEndOfFrame();

        if (GetTriggerCharacter() != null)
        {
            characterController.PairCharacters(characterController.selectedChar, GetTriggerCharacter());
        }
        else if (GetTriggerPos() != null)
        {
            characterController.selectedChar.MoveToPos(GetTriggerPos());
        }
    }

    public Character GetTriggerCharacter()
    {
        Character character = null;

        foreach (Transform tr in triggerObjects)
        {
            if (tr.tag == "Character")
            {
                character = tr.gameObject.GetComponent<Character>();
                break;
            }
        }

        return character;
    }

    public Transform GetTriggerPos()
    {
        Transform clickPos = null;

        foreach (Transform tr in triggerObjects)
        {
            if (tr.tag == "Ground")
            {
                clickPos = transform;
                break;
            }
        }

        return clickPos;
    }
}
