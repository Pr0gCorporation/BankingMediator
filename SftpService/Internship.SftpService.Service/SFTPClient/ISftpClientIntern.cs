using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Internship.SftpService.Service.SFTPClient
{
    public interface ISftpClientIntern
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public PasswordAuthenticationMethod Password { get; set; }

        public void Connect();
        public bool IsConnected();
        public IEnumerable<SftpFile> ListDirectory(string path, Action<int> downloadCallBack = null);
        public void DownloadFile(string path, Stream output, Action<ulong> downloadCallBack = null);
        public void UploadFile(Stream input, string path);
        public void DeleteFile(string path);
        public void Disconnect();
    }
}