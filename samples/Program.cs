namespace cloudbackupforiaaspaas.sdk.samples
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    internal class Program
    {
        private static string BaseUrl = "{Web API URL}";
        private static string IdentityServiceUrl = "{Identity Service URL}";
        private static string RelativePathOfPfxCertificate = "{Relative Path of pfx Certificate}";
        private static string CertificatePwd = "{PFX Certificate Password}";

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Welcome to Cloud Backup for IaaS + PaaS sdk sample tool. This is the tool to get information about job simple report in Cloud Backup for Iaas + PaaS. Please enter according to the instructions:");
            string accessToken = String.Empty;
            while (String.IsNullOrEmpty(accessToken))
            {
                Console.Write("Your Client ID: ");
                string clientId = Console.ReadLine();
                accessToken = await GetAccessTokenAsyncByCertificatePath(IdentityServiceUrl, clientId, RelativePathOfPfxCertificate, CertificatePwd);
            }
            HttpClient _httpClient = new HttpClient();
            GetClient(accessToken, ref _httpClient);
            long startTime = 0;
            long endTime = 0;
            int jobType = 0;
            int serviceType = 0;
            int pageSize = 0;
            int pageNumber = 0;
            while (true)
            {
                try
                {
                    Console.Write($"Start Time (Ticks): ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        startTime = Convert.ToInt64(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            while (true)
            {
                try
                {
                    Console.Write($"End Time (Ticks): ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        endTime = Convert.ToInt64(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            while (true)
            {
                try
                {
                    Console.Write($"Service Type: ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        serviceType = Convert.ToInt16(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            while (true)
            {
                try
                {
                    Console.Write($"Job Type: ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        jobType = Convert.ToInt16(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            while (true)
            {
                try
                {
                    Console.Write($"Page Size: ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        pageSize = Convert.ToInt16(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            while (true)
            {
                try
                {
                    Console.Write($"Page Number: ");
                    var obj = Console.ReadLine();
                    if (!String.IsNullOrEmpty(obj))
                    {
                        pageNumber = Convert.ToInt16(obj);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Incorrect value. Please retry!");
                }
            }
            Console.Write($"Search Text: ");
            var searchText = Console.ReadLine();
            var request = new ReportJobRequestModel()
            {
                StartTime = startTime,
                FinishTime = endTime,
                ServiceType = (ProductModel)serviceType ,
                JobType = (JobType)jobType,
                Pagination = pageSize != -1 && pageNumber != -1 ? new PaginationModel(pageSize, pageNumber) : new PaginationModel(),
                SearchText = searchText,
            };
            GetJobMonitor jobMonitor = new GetJobMonitor(request);
            await jobMonitor.GetData(_httpClient, BaseUrl);
        }

        static async Task<string> GetAccessTokenAsyncByCertificatePath(string url, string clientId, string certificatePath, string password)
        {
            try
            {
                return await new PublicIdentityServiceHelperSample().GetAccessTokenAsyncByCertificatePath(url, clientId, certificatePath, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        static void GetClient(string accessToken, ref HttpClient httpClient)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
