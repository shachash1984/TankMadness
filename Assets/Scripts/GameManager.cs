using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.AI;

[System.Serializable]
public class PlayerAchievments
{
    public int level = 0;
    public int kills = 0;
}

public class GameManager : MonoBehaviour {

    static public GameManager S;
    public PlayerAchievments playerAchievments = new PlayerAchievments();
    [SerializeField] private Renderer _background;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Vector3 _enemySpawnPoint;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _killsText;

    private bool textureLoaded = false;
    private string _jsonData;
    

    private string texture1Path = "http://2.bp.blogspot.com/-zPdgaLaGCRg/UO872s5h7NI/AAAAAAAAD6M/wgsBndEg0T4/s1600/Seamless+ground+sand+texture+(4).jpg";
    private string texture2Path = "http://4.bp.blogspot.com/-VhsbEzsuO4M/UxgnhygktVI/AAAAAAAAFS8/UmtAUpmJtyU/s1600/Dry_patch_grass_ground_land_dirt_aerial_top_seamless_texture.jpg";
    private string texture3Path = "http://2.bp.blogspot.com/-KSxm9Mf2ZUo/UO8-Kp-WKwI/AAAAAAAAD64/3C11hePzqmE/s1600/Seamless+ground+sand+texture+(6).jpg";
    private string texture4Path = "http://www.mb3d.co.uk/mb3d/Ground_Seamless_and_Tileable_High_Res_Textures_files/Ground10_1.jpg";


    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        LoadAchievments();
        SetLevelText();
        SetKillsText();
    }

    

    public void ChangeBackground(int texture)
    {
        StartCoroutine(ApplyTexture(texture));
    }

    private IEnumerator ApplyTexture(int textureNum)
    {
        string textureName = string.Format("{0}.png", textureNum);
        string path = string.Format("{0}/{1}", Application.persistentDataPath, textureName);        
        Texture2D tex = new Texture2D(1024, 1024);
        if (!File.Exists(path))
        {
            string url = "";
            switch (textureNum)
            {
                case 1:
                    url = texture1Path;
                    break;
                case 2:
                    url = texture2Path;
                    break;
                case 3:
                    url = texture3Path;
                    break;
                default:
                    url = texture4Path;
                    break;
                
            }
            yield return StartCoroutine(DownloadTexture(url, textureName));            
        }
        
        byte[] bytes = File.ReadAllBytes(path);
        tex.LoadImage(bytes);
        _background.material.mainTexture = tex;
        _loadingBar.value = 0f;

    }

    IEnumerator DownloadTexture(string url, string wantedTextureName)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            _loadingBar.value = www.progress;
            yield return new WaitForEndOfFrame();
        }
        
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
            StopCoroutine(DownloadTexture(url, wantedTextureName));
        }
        string path = string.Format("{0}/{1}", Application.persistentDataPath, wantedTextureName);
        byte[] bytes = www.texture.EncodeToPNG();

        File.WriteAllBytes(path, bytes);        
    }

    IEnumerator LoadTexture(Texture2D texture, WWW w, string path)
    {
        yield return w;
        byte[] bytes = File.ReadAllBytes(path);
        texture.LoadImage(bytes);
        
    }

    

    public void UpdateLevel()
    {
        playerAchievments.level++;
        SaveAchievments();
    }

    public void UpdateKills()
    {
        playerAchievments.kills++;        
    }
    
    public void SetLevelText()
    {
        _levelText.text = string.Format("Level: {0}", playerAchievments.level.ToString());
    }

    public void SetKillsText()
    {
        _killsText.text = string.Format("Kills: {0}", playerAchievments.kills.ToString());
    }

    private void SaveAchievments()
    {
        _jsonData = JsonUtility.ToJson(playerAchievments);
        string path = string.Format("{0}/playerAchievments.txt", Application.persistentDataPath);
        if (!File.Exists(path))
        {
            File.CreateText(path).Close();
        }
        File.WriteAllText(path, _jsonData);
        
    }

    private void LoadAchievments()
    {
        string path = string.Format("{0}/playerAchievments.txt", Application.persistentDataPath);
        if (!File.Exists(path))
        {            
            SaveAchievments();
        }
        _jsonData = File.ReadAllText(path);
        playerAchievments = JsonUtility.FromJson<PlayerAchievments>(_jsonData);
    }
}
