using InnBot.MessageProcessors.InfoBuffer;

namespace InnBot.MessageProcessors;

public class HelloCommand(HelloCommandInfoBuffer infoBuffer) : ICommand
{
    public string CommandName => "/hello";

    public Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
        return Task.FromResult<string[]>([infoBuffer.HostInfo]);
    }
}