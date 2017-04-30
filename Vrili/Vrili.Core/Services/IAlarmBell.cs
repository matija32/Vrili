namespace Vrili.Core.Services
{
    public interface IAlarmBell
    {
        void RingOnce();
        void RingContinously();
        void StopRinging();
    }
}
