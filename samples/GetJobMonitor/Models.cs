namespace cloudbackupforiaaspaas.sdk.samples
{
    using System;
    using System.Collections.Generic;

    public class ReportJobRequestModel
    {
        public string SearchText { get; set; }
        public ProductModel ServiceType { get; set; }
        public JobType JobType { get; set; }
        public PaginationModel Pagination { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }

        public ReportJobRequestModel()
        {
            this.Pagination = new PaginationModel() { PageNumber = 1, PageSize = 10 };
        }
    }

    public enum ProductModel
    {
        VM = 1,
        AAD = 2,
        AzureFile = 4,
        APSetting = 64,
        AmazonEC2 = 256,
        AzureSQL = 512
    }

    public enum JobType
    {
        Backup = 1,
        AADBackup = 128,
        FileBackup = 1024,
        APSettingBackup = 20003,
        EC2Backup = 20009,
        SQLBackup = 20012,
    }

    public class PaginationModel
    {
        public Int32 PageNumber { get; set; }

        public Int32 PageSize { get; set; }

        public PaginationModel(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PaginationModel()
        {
            PageNumber = 1;
            PageSize = 10;
        }        
    }

    public class ReportJobModel
    {
        public int TotalCount { get; set; }

        public List<Jobs> Jobs { get; set; }
    }

    public class Jobs
    {
        public string JobId { get; set; }
        public CommonStatus State { get; set; }
        public int FailedCount { get; set; }
        public int SuccessfulCount { get; set; }
        public int SkippedCount { get; set; }
        public int TotalCount { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }
        public long Duration { get; set; }
        public string Comments { get; set; }
    }

    public enum CommonStatus
    {
        /// <summary>
        /// Job is created by Timer.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Job is running on agent.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Job completed with no exceptions.
        /// </summary>
        Successful = 2,

        /// <summary>
        /// Job is skipped.
        /// </summary>
        Skipped = 4,

        /// <summary>
        /// Job completed with exceptions.
        /// </summary>
        Exception = 8,

        /// <summary>
        /// Job failed with no successful objects.
        /// </summary>
        Failed = 16,

        /// <summary>
        /// Job is sent from Timer to Agent
        /// </summary>
        Waiting = 32,

    }
}
