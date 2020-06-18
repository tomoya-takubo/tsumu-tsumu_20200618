using System.Collections;
using System.Collections.Generic;   // ←　List型使用の際、必須
using UnityEngine;

public class BallEraser : MonoBehaviour
{
    // PiyoGenerator格納
    public PiyoGenerator pyGener;

    // 最初にドラッグしたボール格納用
    private GameObject firstBall;

    // 最後にドラッグしたボール格納用
    private GameObject lastBall;

    // ボールの名前判定用のstring変数
    private string currentName;

    // 削除するボールのリスト
    List<GameObject> removableBallList = new List<GameObject>();

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // マウスクリック
        if (Input.GetMouseButtonDown(0))
        {
            // 最初にドラッグしたボール情報を格納するfirstBallが空なら
            if(firstBall == null)
            {
                // ドラッグ開始
                OnDragStart();
            }
            // （どちらかというとOnDraging()の処理ここに記述されるべきでは？）
        }
        // マウスクリックが離される
        else if(Input.GetMouseButtonUp(0))
        {
            // クリックを離した時の処理
            OnDragEnd();

            Debug.Log("【終】ドラッグが終わりました！");
        }
        // firstBallに何か情報が格納されていれば
        else if(firstBall != null)
        {
            // ドラッグ処理
            OnDraging();

            Debug.Log("ドラッグしています！");
        }
    }

    /// <summary>
    /// ドラッグ時の処理
    /// </summary>
    private void OnDragStart()
    {
        // カメラ画面のマウスポジションからRay生成
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Rayにhitした情報を取得する(RaycastHit2D)
        RaycastHit2D hit2d
            = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        // 何かしらhitしていれば
        if(hit2d.collider != null)
        {
            // GameObject型変数に格納
            GameObject hitObj = hit2d.transform.gameObject;

            // hitしたGameObjectをその名前の前方一致で判定
            string ballName = hitObj.name;  // 名前取得
            if (ballName.StartsWith("Piyo"))    // 名前が"Piyo"で始まる場合
            {
                // firstBallとlastBallに今ドラッグされたボール情報を格納
                // （この時点では今ドラッグされたボールが全ドラッグ中の
                // 唯一のボールのため、最後のボールにもなりえるのでlastBallも
                // 更新している。ドラッグが進むことでlastBallを更新していくのであろう）
                firstBall = hitObj; // 最初にドラッグされたボール
                lastBall = hitObj;  // 最後にドラッグされたボール

                // 最初にドラッグされたボールの名前を保持
                currentName = hitObj.name;

                // （改めて）削除対象オブジェクトを格納するListの初期化
                removableBallList = new List<GameObject>();

                // ドラッグされたボールをremovableListに格納
                PushToList(hitObj);
            }
        }
    }

    /// <summary>
    /// ドラッグ中の処理
    /// </summary>
    private void OnDraging()
    {
        // Rayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Rayに当たったオブジェクトの情報を取得する
        RaycastHit2D hit2d
            = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        // 何かしらRayに当たっていれば
        if(hit2d.collider != null)
        {
            Debug.Log("☆");

            // hitしたオブジェクトを格納
            GameObject hitObj = hit2d.collider.gameObject;

            // 初回にドラッグしたボールと同じ種類で、
            // lastBall（直近で触れた（ドラッグした））とは別物
            if(hitObj.name == currentName && lastBall != hitObj)
            {
                // 一定の距離内で隣り合っていれば（密接していれば）
                float distance
                    = Vector2.Distance(hitObj.transform.position, lastBall.transform.position); // 距離を取得
            
                // 一定の距離内
                if(distance < 1.0f)
                {
                    // lastBallを更新
                    lastBall = hitObj;

                    // 削除リストに追加
                    PushToList(lastBall);

                    Debug.Log(lastBall + "が新たに削除リストに追加されました！");
                }
            }
        }
    }

    /// <summary>
    /// クリックが離れた際の処理（時点のremovableBallList評価）
    /// </summary>
    private void OnDragEnd()
    {
        // リストに入っているボール数取得
        int remove_cnt = removableBallList.Count;

        // リストに3個以上は言っていたなら
        if(remove_cnt >= 3)
        {
            // リストに入っていたオブジェクトをひとつづつ順に削除
            for(int i = 0; i < remove_cnt; i++)
            {
                // i番目を削除
                Destroy(removableBallList[i]);
            }

            // 消したボールの数だけボールを新たに生成
            StartCoroutine(pyGener.DropBall(remove_cnt));
        }

        // firstBallとlastBallを初期化
        // （☆）ここサイト記載抜けてる。ここのせいで
        // ボールドラッグ削除成立しても消えないバグ？？
        firstBall = null;
        lastBall = null;
    }

    /// <summary>
    /// ドラッグされたボールをListに格納
    /// </summary>
    /// <param name="obj"></param>
    private void PushToList(GameObject obj)
    {
        // 格納
        removableBallList.Add(obj);
    }
}
