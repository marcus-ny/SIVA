public interface IObserver
{
    /*
     * Function for carrying out actions on receiving a particular gameEvent raised from Publisher
     */
    public void OnNotify(GameEvents gameEvent);
}