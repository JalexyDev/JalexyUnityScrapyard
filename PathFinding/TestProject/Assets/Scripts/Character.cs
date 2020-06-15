using UnityEngine;

public class Character : MonoBehaviour
{
    public const float CAMERA_Z_COMPENSATION = 9;
    public const float STEP = 1;

    public CharacterController.CharacterType type;
    public CharacterController.CharacterGrade grade;

    public Castle castle;

    private Unit unit;
    private CharacterController charController;


    void Start()
    {
        unit = GetComponent<Unit>();
        charController = GameObject.FindGameObjectWithTag("CharacterController").GetComponent<CharacterController>();
    }

    private void OnMouseDown()
    {
        charController.ChooseCharacter(this);
    }

    public void MoveToPos(Transform target)
    {
        unit.MoveToTarget(target);
    }

    
}
