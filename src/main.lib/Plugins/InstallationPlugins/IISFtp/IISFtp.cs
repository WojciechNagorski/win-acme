﻿using PKISharp.WACS.Clients.IIS;
using PKISharp.WACS.DomainObjects;
using PKISharp.WACS.Plugins.Interfaces;
using PKISharp.WACS.Plugins.StorePlugins;
using PKISharp.WACS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PKISharp.WACS.Plugins.InstallationPlugins
{
    internal class IISFtp : IInstallationPlugin
    {
        private readonly IIISClient _iisClient;
        private readonly ILogService _log;
        private readonly IISFtpOptions _options;

        public IISFtp(IISFtpOptions options, IIISClient iisClient, ILogService log)
        {
            _iisClient = iisClient;
            _options = options;
            _log = log;
        }

        Task IInstallationPlugin.Install(IEnumerable<IStorePlugin> stores, CertificateInfo newCertificate, CertificateInfo oldCertificate)
        {
            if (!stores.Any(x => x is CertificateStore))
            {
                // Unknown/unsupported store
                var errorMessage = "This installation plugin cannot be used in combination with the store plugin";
                _log.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            _iisClient.UpdateFtpSite(_options.SiteId, newCertificate, oldCertificate);
            return Task.CompletedTask;
        }
    }
}
