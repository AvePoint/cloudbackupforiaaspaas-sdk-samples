namespace cloudbackupforiaaspaas.sdk.samples
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetJobMonitor
    {
        public ReportJobRequestModel reportJobRequestModel = new ReportJobRequestModel();

        public GetJobMonitor(ReportJobRequestModel reportJobRequestModel)
        {
            this.reportJobRequestModel = reportJobRequestModel;
        }

        public GetJobMonitor() { }

        public async Task GetData(HttpClient _httpClient, string baseUrl)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append((baseUrl != null) ? baseUrl.TrimEnd(new char[1] { '/' }) : "").Append("/api/public/jobreport");
            HttpClient client_ = _httpClient;
            bool disposeClient_ = false;
            try
            {
                using HttpRequestMessage request_ = new HttpRequestMessage();
                var body = this.reportJobRequestModel;
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(body));
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");
                request_.Content = stringContent;
                request_.Method = new HttpMethod("POST");
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
                string uriString = stringBuilder.ToString();
                request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
                HttpResponseMessage response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(continueOnCapturedContext: false);
                bool disposeResponse_ = true;
                try
                {
                    Dictionary<string, IEnumerable<string>> headers_ = response_.Headers.ToDictionary<KeyValuePair<string, IEnumerable<string>>, string, IEnumerable<string>>((KeyValuePair<string, IEnumerable<string>> h_) => h_.Key, (KeyValuePair<string, IEnumerable<string>> h_) => h_.Value);
                    if (response_.Content != null && response_.Content.Headers != null)
                    {
                        foreach (KeyValuePair<string, IEnumerable<string>> header in response_.Content.Headers)
                        {
                            headers_[header.Key] = header.Value;
                        }
                    }

                    int status_ = (int)response_.StatusCode;
                    if (status_ == 200)
                    {
                        ObjectResponseResult<ReportJobModel> objectResponseResult = await ReadObjectResponseAsync<ReportJobModel>(response_, headers_).ConfigureAwait(continueOnCapturedContext: false);
                        if (objectResponseResult.Object == null)
                        {
                            throw new Exception($"Response was null which was not expected. Status:{status_} Text: {objectResponseResult.Text} Headers: {headers_}");
                        }
                        WriteToCSVFile(objectResponseResult.Object);
                        return;
                    }

                    string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                    string response = text;
                    throw new Exception($"Response was null which was not expected. Status:{status_} Headers: {headers_}");
                }
                finally
                {
                    if (disposeResponse_)
                    {
                        response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                {
                    client_.Dispose();
                }
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public T Object { get; }

            public string Text { get; }

            public ObjectResponseResult(T responseObject, string responseText)
            {
                Object = responseObject;
                Text = responseText;
            }
        }

        protected static async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }
            try
            {
                using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
                using StreamReader reader = new StreamReader(stream);
                using JsonTextReader reader2 = new JsonTextReader(reader);
                T responseObject2 = JsonSerializer.Create(/*JsonSerializerSettings*/).Deserialize<T>(reader2);
                return new ObjectResponseResult<T>(responseObject2, string.Empty);
            }
            catch (JsonException innerException2)
            {
                throw new Exception($"Could not deserialize the response body stream as " + typeof(T).FullName + $", StatusCode: {(int)response.StatusCode}");
            }
        }

        private string ConvertToString(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }

            if (value is Enum)
            {
                string name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    FieldInfo declaredField = value.GetType().GetTypeInfo().GetDeclaredField(name);
                    if (declaredField != null)
                    {
                        EnumMemberAttribute enumMemberAttribute = declaredField.GetCustomAttribute(typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                        if (enumMemberAttribute != null)
                        {
                            if (enumMemberAttribute.Value == null)
                            {
                                return name;
                            }

                            return enumMemberAttribute.Value;
                        }
                    }

                    string text = Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    if (text != null)
                    {
                        return text;
                    }

                    return string.Empty;
                }
            }
            else
            {
                if (value is bool)
                {
                    return Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
                }

                if (value is byte[])
                {
                    return Convert.ToBase64String((byte[])value);
                }

                if (value.GetType().IsArray)
                {
                    IEnumerable<object> source = ((Array)value).OfType<object>();
                    return string.Join(",", source.Select((object o) => ConvertToString(o, cultureInfo)));
                }
            }

            string text2 = Convert.ToString(value, cultureInfo);
            if (text2 != null)
            {
                return text2;
            }

            return "";
        }

        private void WriteToCSVFile(ReportJobModel reportModel)
        {
            try
            {
                long timer = DateTime.Now.Ticks;
                string dir = $"../reports/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string csvName = $"{dir}Report_{timer}.csv";
                using (StreamWriter sw = new StreamWriter(csvName, true))
                {
                    sw.WriteLine($"Total Count: {reportModel.TotalCount}");
                    sw.WriteLine("JobId,State,Failed Count,Successful Count,Skipped Count,Total Count,Start Time,Finish Time,Duration,Comments");
                    foreach(var obj in reportModel.Jobs)
                    {
                        sw.WriteLine($"{obj.JobId},{obj.State},{obj.FailedCount},{obj.SuccessfulCount},{obj.SkippedCount},{obj.TotalCount}, {obj.StartTime},{obj.FinishTime},{obj.Duration},{obj.Comments}");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
