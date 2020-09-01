using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Player player;

    float updateSpeed;
    float passedTime;

    int activeModel;
    int previousActiveModel;

    public MeshRenderer model1;
    public MeshRenderer model2;
    public MeshRenderer model3;
    public MeshRenderer model4;
    List<MeshRenderer> models = new List<MeshRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
        models.Add(model1);
        models.Add(model2);
        models.Add(model3);
        models.Add(model4);
    }

    // Update is called once per frame
    void Update()
    {
        updateSpeed = player.fietsSnelheid * 0.5f;//Mathf.Pow(player.fietsSnelheid, 1 / 2) * 5;

        if (passedTime > 1/updateSpeed)
        {
            if (activeModel < models.Count - 1)
            {
                activeModel += 1;
            }
            else
            {
                activeModel = 0;
            }

            models[activeModel].enabled = true;
            models[previousActiveModel].enabled = false;

            passedTime = 0;
            previousActiveModel = activeModel;
        }

        passedTime += Time.deltaTime;
    }
}
