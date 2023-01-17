using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    public float game_time = 0;

    public int ticks = 0;
    float game_tick = 0.05f; //game tick in seconds
    float tick_timer = 0;

    void Update()
    {
        tick_timer += Time.deltaTime;
        while (tick_timer >= game_tick)
        {
            game_time += game_tick;

            tick_timer -= game_tick;
            ticks += 1;
        }
    }
}
