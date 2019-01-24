namespace ViberBot.Workflow
{
    public enum State
    {
        Started,
        ContainerPlacesFound,
        
        ProblemStarted,
        ProblemRegistered,
        AfterProblemRegistered,
        BeforeProblemRegistered,
        ProblemContentSended
    }
}