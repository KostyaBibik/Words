namespace Scripts.Enums
{
    public enum ELoadPhase
    {
        None = 0,
        ConfigsLoading = 1,
        AssetsPreloading = 2,
        ServicesInit = 3,
        ConfigsProcessing = 4,
        Completed = 10,
        Failed = 404
    }
}