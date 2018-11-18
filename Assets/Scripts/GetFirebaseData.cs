using UnityEngine;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using System.Collections.Generic;
using System.Collections;
using System;


public static class JsonHelper {
    public static T[] FromJson<T>(string json) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array) {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint) {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }
}

public class GetFirebaseData : MonoBehaviour {
    static int debug_idx = 0;
    public static Note[] allNotes;
    private Firebase firebase;

    // Use this for initialization
    void Awake() {
        //textMesh.text = "";
        StartCoroutine(Tests());
    }
    string fixJson(string value) {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    void GetOKHandler(Firebase sender, DataSnapshot snapshot) {
        string notesRaw = snapshot.RawJson;
        DoDebug(notesRaw);
        string jsonString = fixJson(notesRaw);
        allNotes = JsonHelper.FromJson<Note>(jsonString);
        //GetTimeStamp(sender, snapshot);
    }
    #region handlers
    void GetFailHandler(Firebase sender, FirebaseError err) {
        DoDebug("[ERR] Get from key: <" + sender.FullKey + ">,  " + err.Message + " (" + (int)err.Status + ")");
    }

    void SetOKHandler(Firebase sender, DataSnapshot snapshot) {
        DoDebug("[OK] Set from key: <" + sender.FullKey + ">");
    }

    void SetFailHandler(Firebase sender, FirebaseError err) {
        DoDebug("[ERR] Set from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void UpdateOKHandler(Firebase sender, DataSnapshot snapshot) {
        DoDebug("[OK] Update from key: <" + sender.FullKey + ">");
    }

    void UpdateFailHandler(Firebase sender, FirebaseError err) {
        DoDebug("[ERR] Update from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void DelOKHandler(Firebase sender, DataSnapshot snapshot) {
        DoDebug("[OK] Del from key: <" + sender.FullKey + ">");
    }

    void DelFailHandler(Firebase sender, FirebaseError err) {
        DoDebug("[ERR] Del from key: <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    void PushOKHandler(Firebase sender, DataSnapshot snapshot) {
        DoDebug("[OK] Push from key: <" + sender.FullKey + ">");
    }

    void PushFailHandler(Firebase sender, FirebaseError err) {
    }

    void GetRulesOKHandler(Firebase sender, DataSnapshot snapshot) {
        DoDebug("[OK] GetRules");
        DoDebug("[OK] Raw Json: " + snapshot.RawJson);
    }

    void GetRulesFailHandler(Firebase sender, FirebaseError err) {
        DoDebug("[ERR] GetRules,  " + err.Message + " (" + (int)err.Status + ")");
    }

    void GetTimeStamp(Firebase sender, DataSnapshot snapshot) {
        long timeStamp = snapshot.Value<long>();
        DateTime dateTime = Firebase.TimeStampToDateTime(timeStamp);
        DoDebug("Date: " + timeStamp + " --> " + dateTime.ToString());
    }

    void DoDebug(string str) {
        Debug.Log(str);
        //if (textMesh != null) {
        //   textMesh.text += (++debug_idx + ". " + str) + "\n";
        //}
    }
    #endregion

    IEnumerator Tests() {
        DoDebug("tests are running");
        // Inits Firebase using Firebase Secret Key as Auth
        // The current provided implementation not yet including Auth Token Generation
        // If you're using this sample Firebase End, 
        // there's a possibility that your request conflicts with other simple-firebase-c# user's request
        const string ROOT = "e";
        const string KEY = "e";
        firebase = Firebase.CreateNew(ROOT, KEY);
        // Init callbacks
        firebase.OnGetSuccess += GetOKHandler;
        firebase.OnGetFailed += GetFailHandler;
        firebase.OnSetSuccess += SetOKHandler;
        firebase.OnSetFailed += SetFailHandler;
        firebase.OnUpdateSuccess += UpdateOKHandler;
        firebase.OnUpdateFailed += UpdateFailHandler;
        firebase.OnPushSuccess += PushOKHandler;
        firebase.OnPushFailed += PushFailHandler;
        firebase.OnDeleteSuccess += DelOKHandler;
        firebase.OnDeleteFailed += DelFailHandler;

        // Get child node from firebase, if false then all the callbacks are not inherited.
        Firebase temporary = firebase.Child("temporary", true);
        Firebase lastUpdate = firebase.Child("lastUpdate");

        lastUpdate.OnGetSuccess += GetTimeStamp;

        // Unnecessarily skips a frame, really, unnecessary.
        yield return null;

        // Create a FirebaseQueue
        FirebaseQueue firebaseQueue = new FirebaseQueue();

        // ~~(-.- ~~)
        yield return null;

        // Test #3: Delete the frb_child and broadcasts
        firebaseQueue.AddQueueGet(firebase);
        firebaseQueue.AddQueueDelete(temporary);
        // please notice that the OnSuccess/OnFailed handler is not inherited since Child second parameter not set to true.
        DoDebug("'broadcasts' node is deleted silently.");
        firebaseQueue.AddQueueDelete(firebase.Child("broadcasts"));
        firebaseQueue.AddQueueGet(firebase);
        // ~~(-.-)~~
        yield return null;
        yield return new WaitForSeconds(1f);
    }

    public void setNoteToRead(int index) {
        firebase.Child(index.ToString(), false).UpdateValue("{\"read\": true}", true, FirebaseParam.Empty.PrintSilent());
    }
}