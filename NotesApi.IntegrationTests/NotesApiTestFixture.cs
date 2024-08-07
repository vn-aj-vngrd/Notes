using NotesApi.IntegrationTests;

public class NotesApiTestFixture : IDisposable
{
    public NotesApiFactory Factory { get; }

    public NotesApiTestFixture()
    {
        Factory = new NotesApiFactory();
    }

    public void Dispose()
    {
        // Dispose of resources if necessary
    }
}
