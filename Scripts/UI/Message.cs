using UnityEngine.UI;

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;
    public enum MessageType{
      playerMessage,
      info
    }
}
