using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Core.Models;

namespace WalkingTec.Mvvm.Core.Support.FileHandlers
{

    [Display(Name = "local")]
    public class WtmLocalFileHandler : WtmFileHandlerBase
    {

        public WtmLocalFileHandler(Configs config, IDataContext dc) : base(config, dc)
        {
        }

        public override Stream GetFileData(IWtmFile file)
        {
            return File.OpenRead(file.Path);          
        }


        public override (string path, string handlerInfo) Upload(string fileName, long fileLength, Stream data, string group = null, string subdir = null, string extra = null)
        {
            var localSettings = _config.FileUploadOptions.Settings.Where(x => x.Key.ToLower() == "local").Select(x => x.Value).FirstOrDefault();

            var groupdir = "";
            if (string.IsNullOrEmpty(group))
            {
                groupdir = localSettings?.FirstOrDefault().GroupLocation;
            }
            else {
               groupdir = localSettings?.Where(x => x.GroupName.ToLower() == group.ToLower()).FirstOrDefault().GroupLocation;
            }
            if (string.IsNullOrEmpty(groupdir))
            {
                groupdir = "./uploads";
            }
            string pathHeader = groupdir;
            if (pathHeader.StartsWith("."))
            {
                pathHeader = Path.Combine(_config.HostRoot, pathHeader); 
            }
            if (string.IsNullOrEmpty(subdir) == false)
            {
                pathHeader = Path.Combine(pathHeader, subdir);
            }
            else
            {
                var sub = WtmFileProvider._subDirFunc?.Invoke(this);
                if(string.IsNullOrEmpty(sub)== false)
                {
                    pathHeader = Path.Combine(pathHeader, sub);
                }
            }
            if (!Directory.Exists(pathHeader))
            {
                Directory.CreateDirectory(pathHeader);
            }
            var ext = string.Empty;
            if (string.IsNullOrEmpty(fileName) == false)
            {
                var dotPos = fileName.LastIndexOf('.');
                ext = fileName.Substring(dotPos + 1);
            }

            var fullPath = Path.Combine(pathHeader, $"{Guid.NewGuid().ToNoSplitString()}.{ext}");
            var path = Path.GetFullPath(fullPath);
            using (var fileStream = File.Create(fullPath))
            {
                data.CopyTo(fileStream);
            }
            data.Dispose();
            return (path, "");
        }

        public override void DeleteFile(IWtmFile file)
        {
            if (string.IsNullOrEmpty(file?.Path) == false)
            {
                try
                {
                    File.Delete(file?.Path);
                }
                catch { }
            }
        }
    }

}