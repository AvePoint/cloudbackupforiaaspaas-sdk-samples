# API: "/api/public/jobreport"

# Method: POST

## Request:

Field Name | Type | Description | Default Value
---------- | ---- | ----------- | -------------
**StartTime** | **long** | Sets a start time (UTC time) for the time range. | [null] 
**FinishTime** | **long** | Sets an end time (UTC time) for the time range. | [null]  
**JobType** | **Enum. Valid values: 1 (for Azure VM Backup), 128 (for Microsoft Entra ID Backup), 1024 (for Azure Storage Backup), 20003 (for Admin Portal Settings Backup), 20009 (for Amazon EC2 Backup), 20012 (for Azure SQL Backup)** | Sets the job types that you want to get. | [null]  
**ServiceType** | **Enum. Valid values: 1 (for Azure VM), 2 (for Microsoft Entra ID), 4 (for Azure Storage), 64 (for Admin Portal Settings), 256 (for Amazon EC2), 512 (for Azure SQL)** | Sets the service type of the jobs to get. | [null]  
**PageSize** | **int** | Sets the number of jobs to display on one page. The default value is 10. | [10] 
**PageNumber** | **int** | Sets the starting number of the page to get the jobs. The default value is 0. | [0] 
**SearchText** | **string** | Searches by job ID or description | [null]  

## Response:

Field Name | Type | Description 
---------- | ---- | ----------- 
**TotalCount** | **long** | The total count of the retrieved jobs
**Jobs** | **List<object>** | A list of jobs

### Object in list response:

Field Name | Type | Description 
---------- | ---- | ----------- 
**JobId** | **string** | Job ID
**FailedCount** | **int** | Number of failed objects
**SuccessCount** | **int** | Number of successful objects
**SkippedCount** | **int** | Number of skipped objects
**TotalCount** | **int** | Total count
**StartTime** | **int** | Job started time
**FinishTime** | **int** | Job finished time
**Duration** | **int** | Duration
**Comments** | **string** | Comment of current job



