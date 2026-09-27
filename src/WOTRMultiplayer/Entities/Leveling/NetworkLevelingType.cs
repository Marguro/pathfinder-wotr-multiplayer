namespace WOTRMultiplayer.Entities.Leveling
{
    public enum NetworkLevelingType
    {
        Leveling,
        MythicLeveling,
        Mercenary,
        NewGameSequence,
        DungeonRestart,

        /// <summary>
        /// A second (or further) player-created character built during New Campaign chargen,
        /// joining the party as a companion once the game world has loaded - see
        /// MultiplayerActorBase.StartPendingNewGameCompanionCreation.
        /// </summary>
        NewGameCompanion
    }
}
