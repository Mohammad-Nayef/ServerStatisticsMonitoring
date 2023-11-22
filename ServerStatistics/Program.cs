using SignalREndpoint;

Console.WriteLine("This service is responsible for consuming alerts from SignalR and printing anomalies alerts.\n");

Console.WriteLine("Connecting...");

SignalRAlertConsumer? alertConsumer = null;

while (true)
{
    try
    {
        alertConsumer = new SignalRAlertConsumer();
        await alertConsumer.ConsumeAsync();
    }
    catch
    {
        Console.WriteLine("\nConnection to SignalR failed!");
        Thread.Sleep(2000);
        Console.WriteLine("Reconnecting...");

        continue;
    }

    break;
}

alertConsumer.Connection.Closed += async (args) => 
{
    Console.WriteLine("Disconnected!");
    await alertConsumer.ReconnectAsync();
};

Console.WriteLine("Connected successfully\n");
Console.ReadLine();