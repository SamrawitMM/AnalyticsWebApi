using Microsoft.AspNetCore.Mvc;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace GoogleAnalyticsApp.Controllers
{
    
    [ApiController]
    [Route("api/googleAnalyticsReport/")]
    public class GoogleAnalyticsController : ControllerBase
    {      
        public async Task<AnalyticsReportingService> GetAnalyticsReportingServiceInstance()
        {
            string keyFileName = "./demopersonalapp-d706bf58d722.json";
            string[] scopes = { AnalyticsReportingService.Scope.AnalyticsReadonly }; //Read-only access to Google Analytics
            GoogleCredential credential;
            using (var stream = new FileStream(keyFileName, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }
            // Create the  Analytics service.
            return new AnalyticsReportingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GA Reporting data extraction",
            });
        }

        /// <summary>
        /// Fetches all required reports from Google Analytics
        /// </summary>
        /// <param name="reportRequests"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetReportsResponse>  GetReport(GetReportsRequest getReportsRequest)
        {
            try{
                var analyticsService = await GetAnalyticsReportingServiceInstance();
                return analyticsService.Reports.BatchGet(getReportsRequest).Execute();
            }catch(Exception error){
                throw error;
            }
           
        }    

    }

    [ApiController]
    [Route("api/userActivitySearch/")]
    public class SearchUserActivityController : ControllerBase {
        
        [HttpPost]
        public async Task<SearchUserActivityResponse> userSearchActivity(SearchUserActivityRequest activityRequest){
            var analyticsService = await new GoogleAnalyticsController().GetAnalyticsReportingServiceInstance();
            SearchUserActivityResponse response = analyticsService.UserActivity.Search(activityRequest).Execute();
            return response;

        }
    }

}