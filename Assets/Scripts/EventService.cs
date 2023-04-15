using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

public class EventService : MonoBehaviour
{
    private const int MAX_EVENTS_PER_REQUEST = 10;

    public string serverUrl = "http://example.com/analytics";
    public float cooldownBeforeSend = 5f; // in seconds

    private List<Event> eventsQueue = new List<Event>();
    private float lastSendTime = 0f;
    private bool isSending = false;

    private class Event
    {
        public string type;
        public string data;
    }

    public void TrackEvent(string type, string data)
    {
        eventsQueue.Add(new Event { type = type, data = data });
        if (Time.time - lastSendTime > cooldownBeforeSend)
        {
            SendEvents();
        }
    }

    private async void SendEvents()
    {
        if (isSending) return;
        isSending = true;

        List<Event> eventsToSend = eventsQueue.Take(MAX_EVENTS_PER_REQUEST).ToList();
        eventsQueue.RemoveRange(0, eventsToSend.Count);

        string requestBody = JsonConvert.SerializeObject(new { events = eventsToSend });
        byte[] bodyData = Encoding.UTF8.GetBytes(requestBody);

        var request = (HttpWebRequest)WebRequest.Create(serverUrl);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bodyData.Length;

        using (var requestStream = await request.GetRequestStreamAsync())
        {
            await requestStream.WriteAsync(bodyData, 0, bodyData.Length);
        }

        try
        {
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.LogWarning("Failed to send events to analytics server: " + response.StatusDescription);
                    eventsQueue.InsertRange(0, eventsToSend);
                }
            }
        }
        catch (WebException ex)
        {
            Debug.LogWarning("Failed to send events to analytics server: " + ex.Message);
            eventsQueue.InsertRange(0, eventsToSend);
        }

        isSending = false;
        lastSendTime = Time.time;
    }

    private async void Start()
    {
        while (true)
        {
            if (eventsQueue.Count > 0 && Time.time - lastSendTime > cooldownBeforeSend)
            {
                SendEvents();
            }
            await Task.Delay(1000);
        }
    }
}
