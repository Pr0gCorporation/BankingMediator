﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Internship.SftpService.Service.SFTPClient
{
    public class SftpBankClient
    {
        private SftpClient SftpClient { get; set; }

        public SftpBankClient(HostBuilderContext hostBuilderContext)
        {
            var configuration = hostBuilderContext.Configuration;
            var Host = configuration.GetValue<string>("SftpConfig:Host");
            var Port = configuration.GetValue<int>("SftpConfig:Port");
            var Username = configuration.GetValue<string>("SftpConfig:Username");
            var Password = configuration.GetValue<string>("SftpConfig:Password");
            var connectionInfo = new ConnectionInfo(Host,
                Port,
                Username,
                new PasswordAuthenticationMethod(Username, Password));
            SftpClient = new SftpClient(connectionInfo);
        }

        public void Connect()
        {
            SftpClient.Connect();
        }

        public bool IsConnected()
        {
            return SftpClient.IsConnected;
        }

        public IEnumerable<SftpFile> ListDirectory(string path, Action<int> downloadCallBack = null)
        {
            return SftpClient.ListDirectory(path, downloadCallBack);
        }

        public void DownloadFile(string path, Stream output, Action<ulong> downloadCallBack = null)
        {
            SftpClient.DownloadFile(path, output, downloadCallBack);
        }

        public void UploadFile(Stream input, string path)
        {
            SftpClient.UploadFile(input, path);
        }

        public void DeleteFile(string path)
        {
            SftpClient.DeleteFile(path);
        }

        public void Disconnect()
        {
            SftpClient.Disconnect();
        }
    }
}