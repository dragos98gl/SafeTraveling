using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server
{
    class _Details
    {
        public const string DatabaseConnectionString = @"Data Source=.\SQLEXPRESS;User id=Mihawai;" + "Password=11234567;" + "Database = SafeTraveling";

        public const string GetServers = "GetServers";
        public const string LoginRequest = "LoginRequest";
        public const string AddNewAccount = "AddNewAccount";
        public const string GetStats = "GetStats";
        public const int BytesCount = (1024 * 1024);
        public const string ServerIP = "192.168.1.131";

        public const int LoginSignUpNewTripPort = 2000;
        public const int TripPort_INPUT = 8747;
        public const int TripPort_OUTPUT = 8748;
        public const int WebPort = 8080;
        public const int LocationServicePort = 5228;
        public const int NotificationServicePort = 6228;
        public const int LargeFilesPort = 52743;
        public const int GamesHostPort = 4263;

        public const string GetUserDataByPhone = "GET_USER_DATA_BY_PHONE";
        public const string EditUserInfo = "EDIT_USER_INFO";
        public const string UpdateLocation = "UPDATE_LOCATION";
        public const string GetGalleryCount = "GET_GALERRY_COUNT";
        public const string UpdateProfilePic = "UPDATE_PROFILE_PIC";
        public const string GetProfilePic = "GET_PROFILE_PIC";
        public const string GetTripId = "GET_TRIP_ID";
        public const string GetUserName = "GET_USER_NAME";
        public const string AddPhotoToGallery = "ADD_PHOTO_TO_GALERRY";
        public const string GetGalleryPhotoByIndex = "GET_GALLERY_PHOTO_BY_INDEX";

        public const string Game_XandO = "XandO";
        public const string GameInvitation_XandO = "GAMEINVITATION_XandO";
        public const string Game_Bounce = "BOUNCE_GAME";
        public const string GameInvitation_Bounce = "GAMEINVITATION_BOUNCE";

        public const string NewQuestionPool = "NEW_QESTION_POOL";
        public const string QuestionPoolForVote = "QUESTION_POOL_FOR_VOTE";
        public const string VoteQuestionPool = "VOTE_QUESTION_POOL";
        public const string RequestVotesQuestionPool = "REQUEST_VOTES_QUESTION_POOL";

        public const string BeSmartSimpleQuery = "BE_SMART_SIMPLE_QUERY";
    }
}
