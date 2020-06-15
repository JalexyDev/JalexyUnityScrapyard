using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterType
    {
        SAINT, SATAN, NEUTRAL, MIXED
    }

    // енамы для прокачки персов (если 1+3 = 4, если 2+2 = 4 и т.п.)
    public enum CharacterGrade
    {
        FIRST = 1, SECOND = 2, THIRD = 3, FOURTH = 4/*, FIFTH = 5, SIXTH = 6*/
    }

    [Header("Сатанисты")]
    [SerializeField] private List<Character> satans;

    [Header("Святоши")]
    [SerializeField] private List<Character> saints;

    [SerializeField] private ColliderCast clickPointPrefab;
    
    private ColliderCast clickPoint;

    // TODO перенести логику передвижения перса в этот класс (из ColliderCast)
    public Character selectedChar;

    private int maxValue;

    private void Start()
    {
        // определяем последний уровень перса в расе
        Array values = Enum.GetValues(typeof(CharacterGrade));
        maxValue = (int)values.GetValue(values.Length - 1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnRightBtnClicked();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnRightBtnClicked()
    {
        if (clickPoint != null) { Destroy(clickPoint.gameObject); }

        if (selectedChar == null) { return; }

        clickPoint = Instantiate(clickPointPrefab, 
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Character.CAMERA_Z_COMPENSATION - 1)), 
            Quaternion.identity);

        clickPoint.characterController = this;
    }

    // Спаривание персонажей
    public bool PairCharacters(Character donor, Character recipient)
    {
        if (CanPair(donor, recipient))
        {
            CharacterType type = IdentifyCharacterType(donor.type, recipient.type);
            CharacterGrade grade = IdentifyCharacterGrade(donor.grade, recipient.grade);

            List<Character> list = null;

            switch (type)
            {
                case CharacterType.SAINT:
                    list = saints;
                    break;

                case CharacterType.SATAN:
                    list = satans;
                    break;

                case CharacterType.MIXED:
                    //TODO добавить инициализацию листом mixed (если он появится)
                    break;
            }

            if (list != null)
            {
                Vector3 pos = recipient.transform.position;

                Destroy(donor.gameObject);
                Destroy(recipient.gameObject);
   
                selectedChar = Instantiate(list[((int)grade) - 1], pos, Quaternion.identity);

                return true;
            }
        }

        return false;
    }

    private bool CanPair(Character fChar, Character sChar)
    {
        if (fChar == null || sChar == null || fChar == sChar) { return false; }
        if (fChar.castle == null || sChar.castle == null || fChar.castle != sChar.castle) { return false; }

        //TODO м.б. добавить MIXED и NEUTRAL?
        if (fChar.type != sChar.type) { return false; }

        if (fChar.grade == (CharacterGrade) maxValue ||
            sChar.grade == (CharacterGrade) maxValue)
        {
            return false;
        }

        return true;   
    }


    private CharacterType IdentifyCharacterType(CharacterType donorType, CharacterType recipientType)
    {
        if (donorType == CharacterType.NEUTRAL || recipientType == CharacterType.NEUTRAL)
        {
            return CharacterType.MIXED;
        }

        return donorType;
    }

    private CharacterGrade IdentifyCharacterGrade(CharacterGrade donorType, CharacterGrade recipientType)
    {
        int gradeValue = (int) donorType + (int) recipientType;
        
        if (gradeValue > maxValue) { gradeValue = maxValue; }
        
        return (CharacterGrade) gradeValue;
    }

    // Логика переноса персонажа в новую точку

    public void ChooseCharacter(Character character)
    {
        selectedChar = character;
    }
}
