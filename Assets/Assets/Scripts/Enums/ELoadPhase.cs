namespace Scripts.Enums
{
    public enum ELoadPhase
    {
        None = 0,
        ConfigsLoading = 1,
        AssetsLoading = 2,
        ServicesInit = 3,
        ConfigsProcessing = 4,
        AudioLoading = 5,
        Completed = 10,
        Failed = 404
    }
}