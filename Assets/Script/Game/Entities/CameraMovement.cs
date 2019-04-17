using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public Fighter leftFighter;
    public Fighter rightFighter;
    public float minDistance;
    public Transform cameraGame;
    private float slowDownAmount = 1f;
    private float force = 0.7f;
    private float duration = 1.5f;
    private float initialDuration;
    private int motion;
    private float lerpTimer;
    private AudioSource audioMusic;
    private void Start()
    {
        audioMusic = GetComponent<AudioSource>();
        cameraGame = Camera.main.transform;
        initialDuration = duration;
    }
    void Update()
    {
        audioMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
        motion = leftFighter.Orientation(leftFighter.PlayerPosition());
        float distanceBetweenFighters = Mathf.Sqrt(Mathf.Pow(leftFighter.transform.position.z - rightFighter.transform.position.z, 2) + Mathf.Pow(leftFighter.transform.position.y - rightFighter.transform.position.y, 2));

        // Humain vs IA
        float centerPositition = leftFighter.transform.position.z;

        if (!leftFighter.usePower && !rightFighter.usePower)
        {
            if (distanceBetweenFighters <= minDistance )
            {
                Camera.main.fieldOfView = 38;
                centerPositition = (leftFighter.transform.position.z + rightFighter.transform.position.z) * 0.5f;
                Quaternion target = Quaternion.Euler(0, 90, 0);

                // amortie la rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 10);

            }
            if (rightFighter.transform.position.z <= leftFighter.transform.position.z && distanceBetweenFighters > minDistance )
            {
                Camera.main.fieldOfView = 45;
                centerPositition = leftFighter.transform.position.z + 5;
                Quaternion target = Quaternion.Euler(0, 120, 0);

                // amortie la rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);

            }
            if (rightFighter.transform.position.z >= leftFighter.transform.position.z && distanceBetweenFighters > minDistance )
            {
                Camera.main.fieldOfView = 45;
                centerPositition = leftFighter.transform.position.z - 5;
                Quaternion target = Quaternion.Euler(0, 60, 0);

                // amortie la rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
            }

            transform.position = new Vector3(distanceBetweenFighters > minDistance ? -distanceBetweenFighters : -minDistance, 7, centerPositition);
        }

        if (leftFighter.usePower == true)
        {
            Camera.main.fieldOfView =35;
            centerPositition = leftFighter.transform.position.z;
            
            Quaternion target = Quaternion.Euler(5, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
            transform.position = Vector3.Lerp(transform.position, new Vector3(-25, leftFighter.transform.position.y+9, Mathf.Floor(centerPositition)), Time.deltaTime * 5);


        }
        if (rightFighter.usePower == true)
        {
            Camera.main.fieldOfView = 35;
            centerPositition = rightFighter.transform.position.z;

            Quaternion target = Quaternion.Euler(5, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
            transform.position = Vector3.Lerp(transform.position, new Vector3(-25, rightFighter.transform.position.y + 9, Mathf.Floor(centerPositition)), Time.deltaTime * 5);
        }
       
        if (rightFighter.currentState == FighterStates.Hit_Power || leftFighter.currentState == FighterStates.Hit_Power)
        {
            if (duration > 0)
            {
                cameraGame.localPosition = cameraGame.localPosition + Random.insideUnitSphere * force;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                duration = initialDuration;
            }
        }
    }
}