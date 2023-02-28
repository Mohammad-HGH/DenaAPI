﻿using DenaAPI.Responses;
using System.Text.Json.Serialization;

namespace DenaAPI.Responses
{
    public class DeleteTaskResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int TaskId { get; set; }
    }
}