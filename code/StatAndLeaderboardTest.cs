
using Sandbox.Services;

public class StatAndLeaderboardTest : Component
{
	public const string STAT_EXAMPLE = "example";
	public const string LEADERBOARD_EXAMPLE = "example";

	protected override void OnStart()
	{
		base.OnStart();

		GetStat();
		GetLeaderboard();
	}

	[Button( "Increment Stat" )]
	void IncrementStat()
	{
		// This is what broke it
		//Sandbox.Services.Stats.Increment( STAT_EXAMPLE, double.MaxValue);

		Sandbox.Services.Stats.SetValue( STAT_EXAMPLE, 1);
	}

	[Button("Get Stat")]
	async void GetStat()
	{
		var localStat = Sandbox.Services.Stats.LocalPlayer.Get( STAT_EXAMPLE );
		var globalStat = Sandbox.Services.Stats.Global.Get( STAT_EXAMPLE );

		var localRefreshTask = Sandbox.Services.Stats.LocalPlayer.Refresh();
		var globalRefreshTask = Sandbox.Services.Stats.Global.Refresh();

		await Task.WhenAll(localRefreshTask, globalRefreshTask);

		Log.Info($"{STAT_EXAMPLE} local: {localStat.Value}, global: {globalStat.Value}");
	}

	[Button("Get Leaderboard")]
	async void GetLeaderboard()
	{
		var board = Sandbox.Services.Leaderboards.Get( LEADERBOARD_EXAMPLE );

		board.MaxEntries = 10;
		board.Group = "global";

		await board.Refresh();

		Log.Info($"Board: {board.DisplayName}, Group: {board.Title}, entries: {board.TotalEntries}");

		foreach (var e in board.Entries)
		{
			Log.Info($"[{e.Rank}] {e.DisplayName} - {e.Value} - Me: {e.Me}");
		}
	}
}
