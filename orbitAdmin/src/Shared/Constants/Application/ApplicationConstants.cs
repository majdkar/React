namespace SchoolV01.Shared.Constants.Application
{
    public static class ApplicationConstants
    {
        public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendUpdateMatchTable = "UpdateMatchTableAsync";
            public const string SendUpdateMPlayerTable = "UpdateMPlayerTableAsync";
            public const string ReceiveUpdateMatchTable = "UpdateMatchTable";
            public const string ReceiveUpdateMPlayerTable = "UpdateMPlayerTable";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string ReceiveTest = "ReceiveTest";
            public const string SendMessage = "SendMessageAsync";
            public const string SendTest = "SendTest";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";

            public const string Notification = "Notification";
            public const string SendNotification = "SendNotificationAsync";
            public const string ReceiveNotification = "ReceiveNotificationAsync";

        }
        
        public static class Cache
        {
            
            public const string GetAllDocumentTypesCacheKey = "all-document-types";
            public const string GetAllProductCategoriesCacheKey = "all-ProductCategories";
            public const string GetAllCourseOrdersCacheKey = "all-CourseOrders";
            public const string GetAllProductsCacheKey = "all-Products";
            public const string GetAllProductOffersCacheKey = "all-ProductOffers";
            public const string GetAllCompaniesCacheKey = "all-Companies";
            public const string GetAllPersonsCacheKey = "all-Persons";
            public const string GetAllSuggestionsCacheKey = "all-Suggestions";
            public const string GetAllCourseCategoriesCacheKey = "all-Courses";
   
            public const string GetAllGroupsCacheKey = "all-Groups";
            public const string GetAllPassportsCacheKey = "GetAllPassportsCacheKey";
            public const string GetAllCoursesCacheKey = "GetAllCoursessCacheKey";
            public const string GetAllCourseOffersCacheKey = "GetAllCourseOffersCacheKey";
            public const string GetAllProductSeosCacheKey = "GetAllProductSeoCacheKey";
            public const string GetAllCourseSeosCacheKey = "GetAllCourseSeoCacheKey";
  
            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }
       
        
        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string OpenPdf = "application/pdf";
            public const string Excel = "application/vnd.ms-excel";
        }
    }
}