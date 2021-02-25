﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.SftpService.Service.Models;
using MassTransit;

namespace Internship.SftpService.Service.Publishers.FilePublisher
{
    public class TransactionFilePublisher : IFilePublishable
    {
        private readonly IBus _publishEndpoint;

        public TransactionFilePublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        public async void PublishByOne(List<FileModel> files)
        {
            await PublishFiles(files);
        }
        
        public async void PublishAll(List<FileModel> files)
        {
            await _publishEndpoint.Publish(files);
        }

        private async Task PublishFiles(List<FileModel> files)
        {
            try
            {
                if (files.Count == 0) throw new ArgumentException("No files found.", nameof(files));
            
                foreach (var file in files)
                {
                    await _publishEndpoint.Publish(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}