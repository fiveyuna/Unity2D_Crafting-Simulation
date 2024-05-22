using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    // private bool useEncryption = false;
    // private readonly string encryptionCodeWord = "woongs";

    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        // this.useEncryption = useEncryption;
    }

    public GameData Load() {
        // Path.Combine => dataDirPath + "/" + dataFileName - 안정성을 위해
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        // 파일 있는지 확인
        if (File.Exists(fullPath)) 
        {
            try 
            {
                // 직렬화된 파일 값 읽어오기
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // optionally decrypt the data
                // if (useEncryption) 
                // {
                //     dataToLoad = EncryptDecrypt(dataToLoad);
                // }

                // json 역직렬화 (-> c# 오브젝트)
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data) {
        // OS 별 FilePath 대응
        string fullPath  = Path.Combine(dataDirPath, dataFileName);

        try {
            // directory path 없을 때를 대비해서 만들기
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // 게임 데이터 json으로 직렬화
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            // if (useEncryption) 
            // {
            //     dataToStore = EncryptDecrypt(dataToStore);
            // }

            // 직렬화한 데이터 파일로 저장
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.LogError("Error to save data in " + fullPath + "\n" + e);
        }
    }

    // private string EncryptionDecrypt(string data)
    // {
    //     string modifiedData = ""
    // }


}
