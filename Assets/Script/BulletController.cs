﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject goldCoinPrefab;
    public float goldCoinSpeed = 5f;
    private int raiCount; // Số lượng quái Rai còn lại
    private ParallaxController parallaxController;
    private PlayerController playerController;
    public float goldCoinZ = -6f;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        raiCount = GameObject.FindGameObjectsWithTag("Rai").Length; // Khởi tạo số lượng quái Rai
        Debug.Log(raiCount);

        parallaxController = FindObjectOfType<ParallaxController>(); // Tìm và lưu trữ tham chiếu đến ParallaxController
        playerController = FindObjectOfType<PlayerController>(); // Tìm và lưu trữ tham chiếu đến PlayerController
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rai"))
        {
            FindObjectOfType<GameManager>().IncreaseRai();
            Instantiate(explosionPrefab, collision.gameObject.transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);

            raiCount--; // Giảm số lượng quái Rai còn lại

             if (raiCount == 0)
            {
                SpawnController spawnController = FindObjectOfType<SpawnController>();
                if (spawnController != null)
                {
                    spawnController.SpawnBoss(); // Sinh ra quái BigRai khi bắn hết quái Rai
                    parallaxController.BossRaiAppeared(); // Gọi phương thức BossRaiAppeared() của ParallaxController
                   // playerController.StopAnimation(); // Gọi phương thức StopAnimation() của PlayerController

                    // Tạo vàng
                    GameObject goldCoin = Instantiate(goldCoinPrefab, collision.transform.position, Quaternion.identity);

                    // Random vị trí bay ra
                    float randomX = Random.Range(-1f, 1f); // Phạm vi random theo trục X
                    float randomY = Random.Range(0.5f, 1f); // Phạm vi random theo trục Y
                    Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

                    // Đặt vận tốc nảy của vàng
                    Rigidbody2D goldCoinRb = goldCoin.GetComponent<Rigidbody2D>();
                    goldCoinRb.velocity = randomDirection * goldCoinSpeed;

                    // Đặt vị trí Z của vàng để nằm trên các đối tượng khác
                    Vector3 goldCoinPosition = goldCoin.transform.position;
                    goldCoinPosition.z = goldCoinZ;
                    goldCoin.transform.position = goldCoinPosition;

                    FindObjectOfType<GameManager>().IncreaseGold();
                }
            }
            else
            {
                // Tạo vàng
                GameObject goldCoin = Instantiate(goldCoinPrefab, collision.transform.position, Quaternion.identity);

                // Random vị trí bay ra
                float randomX = Random.Range(-1f, 1f); // Phạm vi random theo trục X
                float randomY = Random.Range(0.5f, 1f); // Phạm vi random theo trục Y
                Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

                // Đặt vận tốc nảy của vàng
                Rigidbody2D goldCoinRb = goldCoin.GetComponent<Rigidbody2D>();
                goldCoinRb.velocity = randomDirection * goldCoinSpeed;

                // Đặt vị trí Z của vàng để nằm trên các đối tượng khác
                Vector3 goldCoinPosition = goldCoin.transform.position;
                goldCoinPosition.z = goldCoinZ;
                goldCoin.transform.position = goldCoinPosition;

                FindObjectOfType<GameManager>().IncreaseGold();

            }
        }
        else if (collision.gameObject.CompareTag("BigRai"))
        {
            gameObject.SetActive(false);
            FindObjectOfType<HeathBarController>().DecreaseHealth(1f);
            parallaxController.BossRaiAppeared(); // Gọi phương thức BossRaiAppeared() của ParallaxController
            //playerController.StopAnimation(); // Gọi phương thức StopAnimation() của PlayerController
        }
    }
}

