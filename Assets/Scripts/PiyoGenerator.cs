using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiyoGenerator : MonoBehaviour
{
    // プレハブ
    public GameObject ballPrefab;

    // piyo絵格納用
    public Sprite[] ballSprites;

    // 生成するボールの個数
    public int ballMax;

    // Start is called before the first frame update
    void Start()
    {
        // ボールを50個生成
        StartCoroutine(DropBall(ballMax));
    }

    /// <summary>
    /// ボールを生成するメソッド
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerator DropBall(int count)
    {
        // 要求された個数だけ生成する
        for (int i = 0; i < count; i++)
        {
            // 位置決定
            Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 7f);

            // インスタンス生成
            GameObject ball
                = Instantiate(ballPrefab
                            , pos
                            , Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));

            // ボール種類選定
            int spriteId = Random.Range(0, 5);

            // 名前づけ
            ball.name = "Piyo" + spriteId;

            // Sprite差し替え
            SpriteRenderer spriteObj = ball.GetComponent<SpriteRenderer>(); //SpriteRendererコンポ取得
            spriteObj.sprite = ballSprites[spriteId];   // Sprite差し替え

            // ウェイト
            yield return new WaitForSeconds(0.05f);
        }
    }
}
