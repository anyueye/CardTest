using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestList : MonoBehaviour
{
    [SerializeField]
    public List<TargetableObjectData.Shield> test = new List<TargetableObjectData.Shield>()
    {
        new TargetableObjectData.Shield() {duration = 1, value = 1},
        new TargetableObjectData.Shield() {duration = 1, value = 1},
        new TargetableObjectData.Shield() {duration = 1, value = 1},
        new TargetableObjectData.Shield() {duration = 2, value = 1}
    };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            // for (int i = 0; i < test.Count; i++)
            // {
            //     var temt = new TargetableObjectData.Shield();
            //     temt = test[i];
            //     temt.duration -= 1;
            //     test[i] = temt;
            // }

            for (int i = test.Count-1; i >=0; i--)
            {
                var temt = test[i];
                temt.duration -= 1;
                test[i] = temt;
                if (test[i].duration<=0)
                {
                    test.Remove(test[i]);
                }
            }
        }
    }
}