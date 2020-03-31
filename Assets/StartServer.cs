﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartServer : MonoBehaviour
{
   [SerializeField]
    TMPro.TextMeshProUGUI WaitingText;

    bool ServerRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ServerRoutine());
    }
    void PrepareGame()
    {
        WaitingText.text = "Press any key to start the game";
        ServerRunning= true;
        UnityEngine.Debug.Log("server fin");

    }

    IEnumerator ServerRoutine1()
    {

        string command = "\"" + Application.streamingAssetsPath + "\"/start.BAT "+ "\""   +  Application.streamingAssetsPath + "\"" ;
        command = command.Replace("/", @"\");
        command = "\"" + command + "\"";
        UnityEngine.Debug.Log(command);
        ProcessStartInfo processInfo;
        Process process;

        //processInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
        bool cmd = true;
        if (cmd)
        {
            processInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
         //   processInfo.CreateNoWindow = false;
          //  processInfo.UseShellExecute = true;

            process = Process.Start(processInfo);
        }else
        /*
        */
        Process.Start("powershell.exe", command);
   



        // *** Read the streams ***
        // Warning: This approach can lead to deadlocks, see Edit #2
        //string output = process.StandardOutput.ReadToEnd();
        //string error = process.StandardError.ReadToEnd();

        //  exitCode = process.ExitCode;
        // UnityEngine.Debug.Log(output);
        // UnityEngine.Debug.Log(error);
        //  Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
        // Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
        // Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
        //  process.Close();






        yield return null;
    }


    IEnumerator ServerRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get("localhost:8081/scroll/list");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            UnityEngine.Debug.Log("no running server");



         //   int exitCode;
            ProcessStartInfo processInfo;
            Process process;


#if UNITY_STANDALONE_LINUX

             ProcessStartInfo proc = new ProcessStartInfo();
             proc.FileName = "xdg-open";
             proc.WorkingDirectory = Application.streamingAssetsPath;
             proc.Arguments = "startServer.sh "+Application.streamingAssetsPath;
             proc.WindowStyle = ProcessWindowStyle.Minimized;
             proc.CreateNoWindow = true;
             process = Process.Start(proc);

#else
            string command = "";
            command = "\"" + Application.streamingAssetsPath + "\"/start.BAT " + "\"" + Application.streamingAssetsPath + "\"";
            command = command.Replace("/", @"\");
            command = "\"" + command + "\"";


            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = true;
            // *** Redirect the output ***
            // processInfo.RedirectStandardError = true;
            //processInfo.RedirectStandardOutput = true;



            process = Process.Start(processInfo);
#endif
            yield return null;

            while (true)
            {
                request = UnityWebRequest.Get("localhost:8081/scroll/list");
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                   // UnityEngine.Debug.Log("no running server");
                }
                else
                {
                    break;
                }



                yield return null;
            }

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            //string output = process.StandardOutput.ReadToEnd();
            //string error = process.StandardError.ReadToEnd();

          //  exitCode = process.ExitCode;
            // UnityEngine.Debug.Log(output);
            // UnityEngine.Debug.Log(error);
            //  Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            // Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            // Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }

        PrepareGame();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(ServerRunning && Input.anyKey)
        {
             SceneManager.LoadScene(1);
        }

        //if(!ServerRunning) UnityEngine.Debug.Log("waiting " + ServerRunning);
    }
}