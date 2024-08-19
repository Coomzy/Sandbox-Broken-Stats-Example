
using Sandbox.Services;

public class StatAndLeaderboardTest : Component
{
	public const string STAT_EXAMPLE = "example";
	public const string LEADERBOARD_EXAMPLE = "example";

	public const string STAT_NON_BROKEN_EXAMPLE = "non-broken-example";
	public const string LEADERBOARD_NON_BROKEN_EXAMPLE = "non-broken-example";

	protected override void OnStart()
	{
		base.OnStart();

		GetExampleStat();
		GetNonBrokenExampleStat();
		GetExampleLeaderboard();
		GetNonBrokenExampleLeaderboard();
	}

	[Group ("Set/Increment Stat"), Button( "Increment Stat" )]
	void IncrementExampleStat()
	{
		// This is what broke it
		//Sandbox.Services.Stats.Increment( STAT_EXAMPLE, double.MaxValue);

		Sandbox.Services.Stats.SetValue( STAT_EXAMPLE, 1);
	}

	[Group( "Set/Increment Stat" ), Button( "Increment Non Broken Stat" )]
	void IncrementNonBrokenExampleStat()
	{
		Sandbox.Services.Stats.Increment( STAT_NON_BROKEN_EXAMPLE, 1 );
	}

	[Group( "Get Stat" ), Button( "Get Stat")]
	void GetExampleStat()
	{
		GetStat( STAT_EXAMPLE );
	}

	[Group( "Get Stat" ), Button( "Get Non Broken Stat" )]
	void GetNonBrokenExampleStat()
	{
		GetStat( STAT_NON_BROKEN_EXAMPLE );
	}

	[Group( "Get Leaderboard" ), Button( "Get Leaderboard" )]
	void GetExampleLeaderboard()
	{
		GetLeaderboard( LEADERBOARD_EXAMPLE );
	}

	[Group( "Get Leaderboard" ), Button( "Get Non Broken Leaderboard" )]
	void GetNonBrokenExampleLeaderboard()
	{
		GetLeaderboard( LEADERBOARD_NON_BROKEN_EXAMPLE );
	}

	async void GetStat(string statName)
	{
		var localStat = Sandbox.Services.Stats.LocalPlayer.Get( statName );
		var globalStat = Sandbox.Services.Stats.Global.Get( statName );

		var localRefreshTask = Sandbox.Services.Stats.LocalPlayer.Refresh();
		var globalRefreshTask = Sandbox.Services.Stats.Global.Refresh();

		await Task.WhenAll(localRefreshTask, globalRefreshTask);

		Log.Info($"{statName} local: {localStat.Value}, global: {globalStat.Value}");
	}

	async void GetLeaderboard(string leaderboardName)
	{
		var board = Sandbox.Services.Leaderboards.Get( leaderboardName );

		board.MaxEntries = 10;
		board.Group = "global";

		await board.Refresh();

		Log.Info($"'{leaderboardName}' Board: {board.DisplayName}, Group: {board.Title}, entries: {board.TotalEntries}");

		foreach (var e in board.Entries)
		{
			Log.Info($"[{e.Rank}] {e.DisplayName} - {e.Value} - Me: {e.Me}");
		}
	}
}
