﻿using System;

namespace Internship.SftpService.Service.DTOs
{
    public class FileDto
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}