using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class HM : MonoBehaviour{
    #region
    //private public
    [Header("跳躍次數"),Range(1,10),Tooltip("描述空間")]
    public int jump_count = 2;
    [Header("跳躍高度")]
    public int jump_h = 100;
    [Header("是否在地面上")]
    public bool isgdground = false;
    [Header("速度")]
    public float speed = 5.5f;
    [Header("名稱")]
    public string hm_name = "FK";
    [Header("HP")]
    public float HP= 500f;
    private float MHP;
    public Image hpbar;//使用圖片需要 using UnityEngine.UI;
    [Header("傷害")]
    public float obstacle_hurt = 50f;
    public float Time_hurt = 50f;
    [Header("積分計算")]
    public int dmint = 0, uwint = 0;
    public Text DMTEXT, UWTEXT;

    [Header("END內容")]
    public GameObject ENDIMAGE;//使用圖片需要 using UnityEngine.UI;

    private int[] compute=new int[4];

    public Text DM_END_TEXT, UW_END_TEXT,T_END_T,timetext,s2_1, s2_2, s2_3,s2_4, s2_5, s2_6;



    [Header("視角與人物移動")]//變數欄位 可使用其他的class來當作宣告
    private Transform  cabera;
    private Animator anima;//動畫控制器
    private CapsuleCollider2D cc2D;//取得碰撞設定器的欄位
    private SpriteRenderer SR;
    private Rigidbody2D RG2D;
    private AudioSource Audio;
    [Header("音效檔")]
    public AudioClip J, S,DMS,UWS;

    [Header("拼接地圖")]
    public Tilemap TileM;

    #endregion
 
    /// <summary>
    /// 啟動時執行1次
    /// </summary>
    private void Start()
    {
        MHP = HP;
        SR = GetComponent<SpriteRenderer>();
        Audio = GetComponent<AudioSource>();
        anima = GetComponent<Animator>();
        cc2D = GetComponent<CapsuleCollider2D>();
        RG2D = GetComponent<Rigidbody2D>();
        cabera = GameObject.Find("Main Camera").transform;
        s2_1.text = "跳躍";
        s2_1.color = new Color(1, 1, 1, 1f);
        s2_2.text = "滑行";
        s2_2.color = new Color(1, 1, 1, 1f);
        s2_3.text = "重新開始";
        s2_3.color = new Color(1, 1, 1, 1f);
        s2_4.text = "關閉遊戲";
        s2_4.color = new Color(1, 1, 1, 1f);
        s2_5.text = "計時";
        s2_5.color = new Color(1, 1, 1, 1f);
        s2_6.text = "總分";
        s2_6.color = new Color(1, 1, 1, 1f);

    }
    /// <summary>
    ///迴圈主程式
    /// </summary>
    private void Update()
    {
        MoveCa();
        Movehm();
        los_Hp();
    }
    /// <summary>
    /// 偵測碰撞物件
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "地")
        {
            isgdground = true;
        }

        if (collision.gameObject.name == "櫻桃")
        {
            EAT2l4rm4(collision);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "障礙")
        {
            /// Debug.Log("1");
            HP -= obstacle_hurt;
            hpbar.fillAmount = HP / MHP;
            SR.color = new Color(1, 0, 0);
            Invoke("ShowColor", .1F);
            dead();
        }
        if (collision.name == "死亡範圍")
        {
            HP = 0;
            dead();
        }
        if (collision.tag == "鑽石")
        {
            EATDM(collision);
        }
    }
    /// <summary>
    /// 傷害顏色修改
    /// </summary>
    private void ShowColor()
    {
        SR.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// 人物移動方式
    /// </summary>
    private void Movehm()
    {
        transform.Translate(speed*Time.deltaTime,0,0);//Time.deltaTime 解決在不同裝置上偵數不一致所倒置的人物速率問題
    }
    /// <summary>
    /// 相機移動方式
    /// </summary>
    private void MoveCa()
    {
        cabera.Translate(speed * Time.deltaTime, 0, 0);
    }
    /// <summary>
    /// 進食鑽石
    /// </summary>
    /// <param name="collision"></param>
    private void EATDM(Collider2D collision)
    {
        Destroy(collision.gameObject);
        dmint++;
        DMTEXT.text = dmint + "";
        Audio.PlayOneShot(DMS);
    }
    /// <summary>
    /// 進食櫻桃
    /// </summary>
    /// <param name="collision"></param>
    private void EAT2l4rm4(Collision2D collision)
    {
        ///Debug.Log("5");
        ///ˋ中心
        Vector3 center = Vector3.zero;
        ///取得碰撞
        Vector3 HITPOINT = Vector3.zero;
        HITPOINT = collision.contacts[0].point;
        ///法線座標
        Vector3 normal = Vector3.zero;
        normal = collision.contacts[0].normal;

        ///中心計算
        center.x = HITPOINT.x - normal.x * 0.01f;
        center.y = HITPOINT.y - normal.y * 0.01f;


        TileM.SetTile(TileM.WorldToCell(center), null);
        Audio.PlayOneShot(UWS);
        uwint++;
        UWTEXT.text = uwint + "";
    }
    /// <summary>
    /// 人物跳躍
    /// </summary>
    public void hmjump()
    {
        if (HP <= 0) return;
        if (isgdground == true)
        {
            Audio.PlayOneShot(J,1f);
            anima.SetBool("跳躍開關", true);
            RG2D.AddForce(new Vector2(0, jump_h));// 剛體.推力(二維向量)
            isgdground = false;
        }
    }
    /// <summary>
    /// 人物滑行
    /// </summary>
    public void SLAD()
    {
        if (HP <= 0) return;
        if (isgdground == true)
        {
            Audio.PlayOneShot(S, 1f);
            cc2D.offset = new Vector2(0f, -0.5f);//設定碰撞控制的大小或位置
            cc2D.size = new Vector2(1.6f, 1.5f);//設定碰撞控制的大小或位置
            anima.SetBool("滑行開關", true);
        }
    }
    /// <summary>
    /// 重製動畫元件
    /// </summary>
    public void resetanimator()
    {
        cc2D.offset = new Vector2(0f, 0.018f);
        cc2D.size = new Vector2(1.6f, 2.9f);
        anima.SetBool("跳躍開關", false);
        anima.SetBool("滑行開關", false);
    }
    /// <summary>
    /// 時間損寫
    /// </summary>
    private void los_Hp()
    {
        HP -= Time.deltaTime * Time_hurt;
        hpbar.fillAmount = HP / MHP;
        dead();
    }
    /// <summary>
    /// 死去
    /// </summary>
    private void dead(){

        if (HP <= 0)
        {
            speed = 0;
            anima.SetBool("死", true);
            Final();
        }
    }
    /// <summary>
    /// 結算畫面
    /// </summary>
    private void Final()
    {
        if (ENDIMAGE.activeInHierarchy== false)
        {
            ENDIMAGE.SetActive(true);
            StartCoroutine(DMUWcount(dmint, 0, 300, DMS, DM_END_TEXT));
            StartCoroutine(DMUWcount(uwint, 1, 200, UWS, UW_END_TEXT, dmint * 0.2f));
            int time = (int)Time.timeSinceLevelLoad;
            StartCoroutine(DMUWcount(time, 2, 100, UWS, timetext, dmint * 0.2f+uwint*0.2f));


        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="count">數量/總時</param>
    /// <param name="scon">計算分數總和</param>
    /// <param name="integral">分數</param>
    /// <param name="aud">音效</param>
    /// <param name="tet">文字介面</param>
    /// <param name="wait">等待時間</param>
    /// <returns></returns>
    private IEnumerator DMUWcount(int count,int scon,int integral,AudioClip aud,Text tet,float wait=0,float delaytime=0.2f)
    {
        yield return new WaitForSeconds(wait);
        while (count > 0)
        {
            count--;
            compute[scon] += integral;
            tet.text = compute[scon].ToString();
            Audio.PlayOneShot(aud);
            yield return new WaitForSeconds(delaytime);
        }
        //總分
        if (scon != 3) compute[3] += compute[scon];///預先記錄總分
        if (scon == 2)
        {
            int total = compute[3] / 100;
            compute[3] = 0;
            StartCoroutine(DMUWcount(total, 3, 100, UWS, T_END_T,0,0.05f));///呼叫總分計算
        }
    }
    

}
