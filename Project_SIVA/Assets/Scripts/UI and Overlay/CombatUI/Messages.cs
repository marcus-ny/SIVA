using TMPro;

public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        Damage,
        LightDamage,
        Healing,
        RedNotify,
        GreenNotify,
        SystemError
    }
}
