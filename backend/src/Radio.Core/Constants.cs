namespace Radio.Core
{
    public struct Constants
    {
        public const string SIGNALR_PATH = "/radioHub";

        public struct UnitOfWork
        {
            public const string TOP_LEVEL_LIFETIME_SCOPE_TAG = nameof(TOP_LEVEL_LIFETIME_SCOPE_TAG);
            public const string REQUEST_CONTEXT_UNIT_OF_WORK_KEY = nameof(REQUEST_CONTEXT_UNIT_OF_WORK_KEY);
        }

        public struct StringLengths
        {
            public const int SHORT_NAME = 64;
            public const int NAME = 256;
            public const int LONG_NAME = 1024;
        }

        public struct App
        {
            public const int NUMBER_OF_CURRENT_SONGS = 1;
            public const int NUMBER_OF_VOTING_CANDIDATES = 3;

            // These values apply in a fully crossfaded scenario. We consider this to be the default scenario.
            // -------C--------                 <- Current song
            //              -----------N------- <- Next song
            //              ---                 <- Crossfade 5 seconds (overlap)
            //           X-----                 <- 15 seconds before end of current song
            //           X--                    <- 10 seconds before start of next song
            // We use the time before the end of the current song to delay sending the message to the client
            // We use the time before the start of the next song to calculate the estimated end timestamp of the next song
            // The worst case is, that the crossfade can't happen at all because of the difference in volume
            // and that the current song and the next song are played one after another without crossing.
            // This means that the estimated end of the next song would be 5 seconds earlier at most, which is not really a problem.
            public const int TIME_IN_SECONDS_BEFORE_END_OF_CURRENT_SONG_WHEN_REQUESTING_NEXT_SONG = 15;
            public const int TIME_IN_SECONDS_BEFORE_START_OF_NEXT_SONG_WHEN_REQUESTING_NEXT_SONG = 10;
        }
    }
}
